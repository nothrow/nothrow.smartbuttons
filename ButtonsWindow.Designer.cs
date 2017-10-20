namespace nothrow.smartbuttons
{
    partial class ButtonsWindow
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
            this.rclickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configWatcher = new System.IO.FileSystemWatcher();
            this.errorLabel = new System.Windows.Forms.Label();
            this.hidingTimer = new System.Windows.Forms.Timer(this.components);
            this.opacityTimer = new System.Windows.Forms.Timer(this.components);
            this.rclickMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // rclickMenu
            // 
            this.rclickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editConfigurationToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.rclickMenu.Name = "rclickMenu";
            this.rclickMenu.Size = new System.Drawing.Size(170, 54);
            // 
            // editConfigurationToolStripMenuItem
            // 
            this.editConfigurationToolStripMenuItem.Name = "editConfigurationToolStripMenuItem";
            this.editConfigurationToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.editConfigurationToolStripMenuItem.Text = "&Edit configuration";
            this.editConfigurationToolStripMenuItem.Click += new System.EventHandler(this.editConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configWatcher
            // 
            this.configWatcher.EnableRaisingEvents = true;
            this.configWatcher.SynchronizingObject = this;
            this.configWatcher.Changed += new System.IO.FileSystemEventHandler(this.configWatcher_Changed);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(13, 13);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 13);
            this.errorLabel.TabIndex = 1;
            // 
            // ButtonsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 332);
            this.ContextMenuStrip = this.rclickMenu;
            this.Controls.Add(this.errorLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ButtonsWindow";
            this.Opacity = 0.8D;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ButtonsWindow_FormClosing);
            this.Load += new System.EventHandler(this.ButtonsWindow_Load);
            this.MouseEnter += new System.EventHandler(this.ButtonsWindow_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ButtonsWindow_MouseLeave);
            this.Resize += new System.EventHandler(this.ButtonsWindow_Resize);
            this.rclickMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.configWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip rclickMenu;
        private System.Windows.Forms.ToolStripMenuItem editConfigurationToolStripMenuItem;
        private System.IO.FileSystemWatcher configWatcher;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer hidingTimer;
        private System.Windows.Forms.Timer opacityTimer;
    }
}

