using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nothrow.smartbuttons
{
    public partial class ButtonsWindow : Form
    {
        private const int RightPadding = 10;
        private const int OpacitySpeed = 4;
        private const int InvisibleOpacity = 10;
        private const int VisibleOpacity = 80;
        private int _targetOpacity;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public ButtonsWindow()
        {
            InitializeComponent();

            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEventsOnDisplaySettingsChanged;

            _targetOpacity = VisibleOpacity;
            Opacity = VisibleOpacity / 100.0;
            Task.Run(UpdateOpacityTask, _cts.Token);
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
                new Point(SystemInformation.VirtualScreen.Width - Width - RightPadding, SystemInformation.VirtualScreen.Height / 2 - Height / 2);
        }

        private void ButtonsWindow_Load(object sender, EventArgs e)
        {
            MoveToProperLocation();
        }

        private void ButtonsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= SystemEventsOnDisplaySettingsChanged;
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
    }
}
