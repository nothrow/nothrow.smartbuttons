using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace nothrow.smartbuttons
{
    public partial class ButtonsWindow : Form
    {
        private const int RightPadding = 10;
        private const int InternalMarginHorizontal = 10;
        private const int InternalMarginVertical = 2;
        private const int InternalPaddingVertical = 5;
        private const int OpacitySpeed = 4;
        private const int InvisibleOpacity = 10;
        private const int VisibleOpacity = 80;

        private readonly DetailsWindow _detailsWindow = new DetailsWindow();
        private readonly object _configurationSyncLock = new object();

        private const string ButtonsConfigFile = "buttons.yml";

        private const string DefaultConfigFile = @"buttons:
  - caption: Edit configuration
    action:
      command: notepad
      parameters:
        - {0}
      pwd: c:\
";

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private ButtonInfo[] _buttons = new ButtonInfo[0];
        private ButtonsConfiguration _currentConfiguration = new ButtonsConfiguration();
        private int _targetOpacity;

        public ButtonsWindow()
        {
            InitializeComponent();

            SystemEvents.DisplaySettingsChanged += SystemEventsOnDisplaySettingsChanged;

            _targetOpacity = VisibleOpacity;
            Opacity = VisibleOpacity / 100.0;

            configWatcher.Filter = ButtonsConfigFile;
            configWatcher.Path = ConfigPath;

            ReloadConfiguration();
        }

        private static string ButtonsConfigFilePath => Path.Combine(ConfigPath, ButtonsConfigFile);

        private static string ConfigPath
        {
            get
            {
                var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                             "nothrow",
                                             "smartbuttons");

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                return directory;
            }
        }

        private int OpacityPercentage
        {
            get => (int)(Opacity * 100);
            set => Invoke((MethodInvoker)(() => Opacity = value / 100.0));
        }

        private void SystemEventsOnDisplaySettingsChanged(object sender, EventArgs eventArgs)
        {
            MoveToProperLocation();
        }

        private void MoveToProperLocation()
        {
            Location =
                new Point(SystemInformation.VirtualScreen.Width - Width - RightPadding,
                          SystemInformation.VirtualScreen.Height / 2 - Height / 2);
        }

        private void ButtonsWindow_Load(object sender, EventArgs e)
        {
            MoveToProperLocation();
        }

        private void ButtonsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.DisplaySettingsChanged -= SystemEventsOnDisplaySettingsChanged;
            _cts.Cancel();
        }

        private void ButtonsWindow_MouseEnter(object sender, EventArgs e)
        {
            _targetOpacity = InvisibleOpacity;
        }

        private void ButtonsWindow_MouseLeave(object sender, EventArgs e)
        {
            _targetOpacity = VisibleOpacity;
        }

        private void ButtonsWindow_Resize(object sender, EventArgs e)
        {
            MoveToProperLocation();
        }

        private void editConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", ButtonsConfigFilePath);
        }

        private void configWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ReloadConfiguration();
        }

        private void ReloadConfiguration()
        {
            try
            {
                lock (_configurationSyncLock)
                {
                    if (!File.Exists(ButtonsConfigFilePath))
                    {
                        File.WriteAllText(ButtonsConfigFilePath,
                                          string.Format(DefaultConfigFile, ButtonsConfigFilePath));
                    }

                    var config = LoadConfiguration();
                    if (config.Equals(_currentConfiguration))
                        return;

                    ApplyConfiguration(config);
                }
            }
            catch (Exception)
            {
                errorLabel.Text = "Reload of configuration failed.";
            }
        }

        private void ApplyConfiguration(ButtonsConfiguration config)
        {
            foreach (var button in _buttons)
            {
                Controls.Remove(button.ActionButton);
                button.ActionButton.Dispose();
            }

            _buttons = new ButtonInfo[config.Buttons.Count];
            var top = errorLabel.Top + errorLabel.Height + InternalMarginVertical;

            for (var i = 0; i < config.Buttons.Count; i++)
            {
                var cb = config.Buttons[i];
                var but = new Button();

                but.Height = TextRenderer.MeasureText(cb.Caption, but.Font).Height + InternalPaddingVertical * 2;
                but.Width = Width - InternalMarginHorizontal * 2;
                but.Left = InternalMarginHorizontal;
                but.Top = top;
                but.Text = cb.Caption;

                but.Click += InvokeActionButton;
                but.MouseEnter += EnterActionButton;
                but.MouseLeave += LeaveActionButton;


                top += but.Height + InternalMarginVertical;

                _buttons[i] = new ButtonInfo(but, cb);
                but.Tag = _buttons[i];

                Controls.Add(but);
            }

            Height = top;
            _currentConfiguration = config;

            errorLabel.Text = "";
        }

        private void EnterActionButton(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var info = (ButtonInfo)button.Tag;
            var executionInfo = info.ExecutionInfo;

            var exitDateText = "";
            if (executionInfo.ExitDate.HasValue)
            {
                exitDateText = $"{executionInfo.ExitDate.Value} - {(DateTime.Now - executionInfo.ExitDate.Value).TotalMinutes:0:00} minutes ago";
            }

            _detailsWindow.exitCode.Text = executionInfo.ExitCode?.ToString();
            _detailsWindow.exitTime.Text = exitDateText;
            _detailsWindow.logs.Text = string.Join("\n", executionInfo.OutputLines);

            _detailsWindow.Left = Left - _detailsWindow.Width;
            _detailsWindow.Top = Top + button.Top;

            hideDetailsTimer.Enabled = false;
            showDetailsTimer.Enabled = true;
        }

        private void LeaveActionButton(object sender, EventArgs e)
        {
            hideDetailsTimer.Enabled = true;
            showDetailsTimer.Enabled = false;
        }

        private void SetButtonStatus(Button button, ButtonStatus buttonStatus)
        {
            switch (buttonStatus)
            {
                case ButtonStatus.Running:
                    button.BackColor = Color.Gray;
                    break;
                case ButtonStatus.Success:
                    button.BackColor = Color.LightGreen;
                    break;
                case ButtonStatus.Failure:
                    button.BackColor = Color.LightCoral;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttonStatus), buttonStatus, null);
            }
        }

        private void InvokeActionButton(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var info = (ButtonInfo)button.Tag;
            var executionInfo = info.ExecutionInfo;

            lock (executionInfo.SyncRoot)
            {
                if (executionInfo.RunningProcess != null)
                    return;

                var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.Arguments =
                    string.Join(" ", info.Configuration.Action.Parameters.Select(x => $"\"{x}\""));

                process.StartInfo.FileName = info.Configuration.Action.Command;
                process.StartInfo.WorkingDirectory = info.Configuration.Action.Pwd;

                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.EnableRaisingEvents = true;

                process.ErrorDataReceived += (o, args) => executionInfo.AppendOutputLines(args.Data);
                process.OutputDataReceived += (o, args) => executionInfo.AppendOutputLines(args.Data);

                process.Exited += (o, args) =>
                                  {
                                      if (process.ExitCode == 0)
                                      {
                                          SetButtonStatus(button, ButtonStatus.Success);
                                      }
                                      else
                                      {
                                          SetButtonStatus(button, ButtonStatus.Failure);
                                      }
                                      lock (executionInfo.SyncRoot)
                                      {
                                          executionInfo.RunningProcess = null;
                                          executionInfo.ExitDate = process.ExitTime;
                                          executionInfo.ExitCode = process.ExitCode;
                                      }
                                  };

                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    executionInfo.SetOutputLine(ex.Message);
                    SetButtonStatus(button, ButtonStatus.Failure);
                    return;
                }

                executionInfo.Reset();
                executionInfo.RunningProcess = process;
                SetButtonStatus(button, ButtonStatus.Running);
            }
        }

        private static ButtonsConfiguration LoadConfiguration()
        {
            using (var f = File.Open(ButtonsConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(f))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).
                    Build();

                return deserializer.Deserialize<ButtonsConfiguration>(sr);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void opacityTimer_Tick(object sender, EventArgs e)
        {
            if (OpacityPercentage != _targetOpacity)
            {
                OpacityPercentage += Math.Sign(_targetOpacity - OpacityPercentage) * OpacitySpeed;
                if (Math.Abs(_targetOpacity - OpacityPercentage) < OpacitySpeed)
                    OpacityPercentage = _targetOpacity;
            }
        }

        private void hidingTimer_Tick(object sender, EventArgs e)
        {
            Show();
            hidingTimer.Enabled = false;
        }

        private void hideFor10SecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidingTimer.Enabled = true;
            Hide();
        }

        private class ButtonInfo
        {
            public ButtonInfo(Button actionButton, ButtonConfiguration configuration)
            {
                Configuration = configuration;
                ActionButton = actionButton;
            }

            public ButtonConfiguration Configuration { get; }

            public Button ActionButton { get; }

            public ButtonExecutionInfo ExecutionInfo { get; } = new ButtonExecutionInfo();
        }

        private class ButtonExecutionInfo
        {
            private const int Threshold = 10;

            private readonly Queue<string> _outputLines = new Queue<string>();
            public object SyncRoot { get; } = new object();
            public Process RunningProcess { get; set; }
            public int? ExitCode { get; set; }
            public DateTime? ExitDate { get; set; }

            public IEnumerable<string> OutputLines
            {
                get
                {
                    lock (SyncRoot)
                    {
                        return _outputLines.ToArray();
                    }
                }
            }

            public void SetOutputLine(string line)
            {
                lock (SyncRoot)
                {
                    _outputLines.Clear();
                    _outputLines.Enqueue(line);
                }
            }

            public void AppendOutputLines(string text)
            {
                lock (SyncRoot)
                {
                    foreach (var line in text.Split('\n'))
                    {
                        _outputLines.Enqueue(line.Trim());
                        if (_outputLines.Count > Threshold)
                            _outputLines.Dequeue();
                    }
                }
            }

            public void Reset()
            {
                _outputLines.Clear();
                ExitCode = null;
                ExitDate = null;
            }
        }

        private enum ButtonStatus
        {
            Running,
            Success,
            Failure
        }

        private void showDetailsTimer_Tick(object sender, EventArgs e)
        {
            _detailsWindow.Show();
            showDetailsTimer.Enabled = false;
        }

        private void hideDetailsTimer_Tick(object sender, EventArgs e)
        {
            _detailsWindow.Hide();
            hideDetailsTimer.Enabled = false;
        }
    }
}