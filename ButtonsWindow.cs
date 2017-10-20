using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        private const string ButtonsConfigFile = "buttons.yml";

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private Button[] _buttons = new Button[0];
        private ButtonsConfiguration _currentConfiguration = new ButtonsConfiguration();
        private int _targetOpacity;

        public ButtonsWindow()
        {
            InitializeComponent();

            SystemEvents.DisplaySettingsChanged += SystemEventsOnDisplaySettingsChanged;

            _targetOpacity = VisibleOpacity;
            Opacity = VisibleOpacity / 100.0;
            Task.Run(UpdateOpacityTask, _cts.Token);

            configWatcher.Filter = ButtonsConfigFile;
            configWatcher.Path = ConfigPath;

            ReloadConfiguration();
        }

        private static string ButtonsPath => Path.Combine(ConfigPath, ButtonsConfigFile);

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

        private async Task UpdateOpacityTask()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (OpacityPercentage != _targetOpacity)
                {
                    OpacityPercentage += Math.Sign(_targetOpacity - OpacityPercentage) * OpacitySpeed;
                    if (Math.Abs(_targetOpacity - OpacityPercentage) < OpacitySpeed)
                        OpacityPercentage = _targetOpacity;
                }

                await Task.Delay(40);
            }
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
            Process.Start("notepad.exe", ButtonsPath);
        }

        private void configWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ReloadConfiguration();
        }

        private void ReloadConfiguration()
        {
            try
            {
                var config = LoadConfiguration();
                if (config.Equals(_currentConfiguration))
                    return;

                ApplyConfiguration(config);
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
                Controls.Remove(button);
            }

            _buttons = new Button[config.Buttons.Count];
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

                top += but.Height + InternalMarginVertical;

                _buttons[i] = but;
                Controls.Add(but);
            }

            Height = top;
            _currentConfiguration = config;

            errorLabel.Text = "";
        }

        private static ButtonsConfiguration LoadConfiguration()
        {
            using (var f = File.Open(ButtonsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
    }
}