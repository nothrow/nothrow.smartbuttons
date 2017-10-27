namespace nothrow.smartbuttons
{
    partial class DetailsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.exitCode = new System.Windows.Forms.Label();
            this.exitTime = new System.Windows.Forms.Label();
            this.runningFor = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // logs
            // 
            this.logs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logs.Location = new System.Drawing.Point(11, 33);
            this.logs.Multiline = true;
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(610, 337);
            this.logs.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Exit code: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Exit time: ";
            // 
            // exitCode
            // 
            this.exitCode.AutoSize = true;
            this.exitCode.Location = new System.Drawing.Point(66, 9);
            this.exitCode.Name = "exitCode";
            this.exitCode.Size = new System.Drawing.Size(0, 13);
            this.exitCode.TabIndex = 3;
            // 
            // exitTime
            // 
            this.exitTime.AutoSize = true;
            this.exitTime.Location = new System.Drawing.Point(201, 9);
            this.exitTime.Name = "exitTime";
            this.exitTime.Size = new System.Drawing.Size(0, 13);
            this.exitTime.TabIndex = 4;
            // 
            // runningFor
            // 
            this.runningFor.AutoSize = true;
            this.runningFor.Location = new System.Drawing.Point(507, 9);
            this.runningFor.Name = "runningFor";
            this.runningFor.Size = new System.Drawing.Size(0, 13);
            this.runningFor.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(449, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Running for: ";
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // DetailsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 382);
            this.Controls.Add(this.runningFor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.exitTime);
            this.Controls.Add(this.exitCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DetailsWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DetailsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox logs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label exitCode;
        public System.Windows.Forms.Label exitTime;
        public System.Windows.Forms.Label runningFor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer updateTimer;
    }
}