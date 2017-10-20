using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nothrow.smartbuttons
{
    public partial class DetailsWindow : Form
    {
        private ButtonsWindow.ButtonExecutionInfo _executionInfo = _empty;
        private static readonly ButtonsWindow.ButtonExecutionInfo _empty = new ButtonsWindow.ButtonExecutionInfo();

        public DetailsWindow()
        {
            InitializeComponent();
        }

        public ButtonsWindow.ButtonExecutionInfo ExecutionInfo
        {
            get => _executionInfo;
            set {
                _executionInfo = value ?? _empty;
                UpdateFields(); 
            }
        }

        private void UpdateFields()
        {
            if (_executionInfo == null)
                _executionInfo = _empty;

            lock (ExecutionInfo.SyncRoot)
            {
                var exitDateText = "";
                if (ExecutionInfo.ExitDate.HasValue)
                {
                    exitDateText =
                        $"{ExecutionInfo.ExitDate.Value} - {(DateTime.Now - ExecutionInfo.ExitDate.Value).TotalMinutes:0:00} minutes ago";
                }

                exitCode.Text = ExecutionInfo.ExitCode?.ToString();
                exitTime.Text = exitDateText;
                runningFor.Text = ExecutionInfo.Stopwatch.Elapsed.ToString();
                logs.Text = string.Join(Environment.NewLine, ExecutionInfo.OutputLines);
            }
        }

        private void DetailsWindow_Load(object sender, EventArgs e)
        {

        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (_executionInfo != null)
                UpdateFields();
        }
    }
}
