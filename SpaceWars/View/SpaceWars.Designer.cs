namespace View
{
    partial class SpaceWars
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpaceWars));
            this.serverLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.serverConnectPanel = new System.Windows.Forms.Panel();
            this.connectButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.closeTab = new System.Windows.Forms.ToolStripMenuItem();
            this.helpTab = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsTab = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutTab = new System.Windows.Forms.ToolStripMenuItem();
            this.gameMenuStrip = new System.Windows.Forms.MenuStrip();
            this.serverConnectPanel.SuspendLayout();
            this.gameMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(16, 11);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(54, 17);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server:";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(232, 11);
            this.usernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(77, 17);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username:";
            // 
            // serverConnectPanel
            // 
            this.serverConnectPanel.Controls.Add(this.connectButton);
            this.serverConnectPanel.Controls.Add(this.usernameTextBox);
            this.serverConnectPanel.Controls.Add(this.serverTextBox);
            this.serverConnectPanel.Controls.Add(this.serverLabel);
            this.serverConnectPanel.Controls.Add(this.usernameLabel);
            this.serverConnectPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.serverConnectPanel.Location = new System.Drawing.Point(0, 28);
            this.serverConnectPanel.Margin = new System.Windows.Forms.Padding(4);
            this.serverConnectPanel.Name = "serverConnectPanel";
            this.serverConnectPanel.Size = new System.Drawing.Size(1309, 42);
            this.serverConnectPanel.TabIndex = 2;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(472, 5);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(100, 28);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(317, 7);
            this.usernameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(132, 22);
            this.usernameTextBox.TabIndex = 2;
            this.usernameTextBox.TextChanged += new System.EventHandler(this.UsernameTextBox_TextChanged);
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(79, 7);
            this.serverTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(132, 22);
            this.serverTextBox.TabIndex = 0;
            this.serverTextBox.Text = "localhost";
            // 
            // closeTab
            // 
            this.closeTab.Name = "closeTab";
            this.closeTab.Size = new System.Drawing.Size(57, 24);
            this.closeTab.Text = "Close";
            this.closeTab.Click += new System.EventHandler(this.CloseTab_Click);
            // 
            // helpTab
            // 
            this.helpTab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlsTab,
            this.aboutTab});
            this.helpTab.Name = "helpTab";
            this.helpTab.Size = new System.Drawing.Size(53, 24);
            this.helpTab.Text = "Help";
            // 
            // controlsTab
            // 
            this.controlsTab.Name = "controlsTab";
            this.controlsTab.Size = new System.Drawing.Size(139, 26);
            this.controlsTab.Text = "Controls";
            this.controlsTab.Click += new System.EventHandler(this.ControlsTab_Click);
            // 
            // aboutTab
            // 
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Size = new System.Drawing.Size(139, 26);
            this.aboutTab.Text = "About";
            this.aboutTab.Click += new System.EventHandler(this.aboutTab_Click);
            // 
            // gameMenuStrip
            // 
            this.gameMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.gameMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeTab,
            this.helpTab});
            this.gameMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.gameMenuStrip.Name = "gameMenuStrip";
            this.gameMenuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.gameMenuStrip.Size = new System.Drawing.Size(1309, 28);
            this.gameMenuStrip.TabIndex = 4;
            this.gameMenuStrip.Text = "spaceWarsMenuStrip";
            // 
            // SpaceWars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 1010);
            this.Controls.Add(this.serverConnectPanel);
            this.Controls.Add(this.gameMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SpaceWars";
            this.Text = "Battlefront II : Now Microtransaction Free!";
            this.serverConnectPanel.ResumeLayout(false);
            this.serverConnectPanel.PerformLayout();
            this.gameMenuStrip.ResumeLayout(false);
            this.gameMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Panel serverConnectPanel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.ToolStripMenuItem closeTab;
        private System.Windows.Forms.ToolStripMenuItem helpTab;
        private System.Windows.Forms.ToolStripMenuItem controlsTab;
        private System.Windows.Forms.ToolStripMenuItem aboutTab;
        private System.Windows.Forms.MenuStrip gameMenuStrip;
    }
}

