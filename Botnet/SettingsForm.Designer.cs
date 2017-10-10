namespace Botnet
{
    partial class SettingsForm
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
            this.SetParamnBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.AttackParamsBox = new System.Windows.Forms.GroupBox();
            this.RestPoolsSetBox = new System.Windows.Forms.GroupBox();
            this.RestStartBox = new System.Windows.Forms.TextBox();
            this.RestEndBox = new System.Windows.Forms.TextBox();
            this.FloodEnablingCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HttpContentBox = new System.Windows.Forms.TextBox();
            this.TargetAddressBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.masterPointBox = new System.Windows.Forms.GroupBox();
            this.MasterPortBox = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.MasterIpBox = new System.Windows.Forms.TextBox();
            this.RoleCheckBox = new System.Windows.Forms.CheckBox();
            this.adapterGBox = new System.Windows.Forms.GroupBox();
            this.MacLab = new System.Windows.Forms.Label();
            this.IpLab = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CurAdapterLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AdapterBox = new System.Windows.Forms.ListBox();
            this.HttpsSetsBox = new System.Windows.Forms.GroupBox();
            this.HttpSetsCheckBox = new System.Windows.Forms.CheckBox();
            this.AttackParamsBox.SuspendLayout();
            this.RestPoolsSetBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.masterPointBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MasterPortBox)).BeginInit();
            this.adapterGBox.SuspendLayout();
            this.HttpsSetsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetParamnBtn
            // 
            this.SetParamnBtn.Location = new System.Drawing.Point(661, 467);
            this.SetParamnBtn.Name = "SetParamnBtn";
            this.SetParamnBtn.Size = new System.Drawing.Size(145, 23);
            this.SetParamnBtn.TabIndex = 0;
            this.SetParamnBtn.Text = "Apply Settings";
            this.SetParamnBtn.UseVisualStyleBackColor = true;
            this.SetParamnBtn.Click += new System.EventHandler(this.SetParamnBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(580, 467);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target address";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AttackParamsBox
            // 
            this.AttackParamsBox.Controls.Add(this.HttpSetsCheckBox);
            this.AttackParamsBox.Controls.Add(this.HttpsSetsBox);
            this.AttackParamsBox.Controls.Add(this.RestPoolsSetBox);
            this.AttackParamsBox.Controls.Add(this.FloodEnablingCheckBox);
            this.AttackParamsBox.Controls.Add(this.TargetAddressBox);
            this.AttackParamsBox.Controls.Add(this.label1);
            this.AttackParamsBox.Location = new System.Drawing.Point(3, 4);
            this.AttackParamsBox.Name = "AttackParamsBox";
            this.AttackParamsBox.Size = new System.Drawing.Size(327, 463);
            this.AttackParamsBox.TabIndex = 3;
            this.AttackParamsBox.TabStop = false;
            this.AttackParamsBox.Text = "Attack parameters (Wizard mode only)";
            // 
            // RestPoolsSetBox
            // 
            this.RestPoolsSetBox.Controls.Add(this.RestStartBox);
            this.RestPoolsSetBox.Controls.Add(this.RestEndBox);
            this.RestPoolsSetBox.Location = new System.Drawing.Point(18, 380);
            this.RestPoolsSetBox.Name = "RestPoolsSetBox";
            this.RestPoolsSetBox.Size = new System.Drawing.Size(276, 76);
            this.RestPoolsSetBox.TabIndex = 13;
            this.RestPoolsSetBox.TabStop = false;
            this.RestPoolsSetBox.Text = "Range of denied addresses";
            // 
            // RestStartBox
            // 
            this.RestStartBox.Location = new System.Drawing.Point(11, 45);
            this.RestStartBox.Name = "RestStartBox";
            this.RestStartBox.Size = new System.Drawing.Size(100, 20);
            this.RestStartBox.TabIndex = 6;
            this.RestStartBox.Text = "192.168.0.1";
            // 
            // RestEndBox
            // 
            this.RestEndBox.Location = new System.Drawing.Point(141, 45);
            this.RestEndBox.Name = "RestEndBox";
            this.RestEndBox.Size = new System.Drawing.Size(100, 20);
            this.RestEndBox.TabIndex = 7;
            this.RestEndBox.Text = "192.168.0.80";
            // 
            // FloodEnablingCheckBox
            // 
            this.FloodEnablingCheckBox.AutoSize = true;
            this.FloodEnablingCheckBox.Location = new System.Drawing.Point(116, 357);
            this.FloodEnablingCheckBox.Name = "FloodEnablingCheckBox";
            this.FloodEnablingCheckBox.Size = new System.Drawing.Size(77, 17);
            this.FloodEnablingCheckBox.TabIndex = 12;
            this.FloodEnablingCheckBox.Text = "UDP flood";
            this.FloodEnablingCheckBox.UseVisualStyleBackColor = true;
            this.FloodEnablingCheckBox.CheckedChanged += new System.EventHandler(this.FloodEnablingCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "HTTP request text";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // HttpContentBox
            // 
            this.HttpContentBox.Location = new System.Drawing.Point(12, 37);
            this.HttpContentBox.Multiline = true;
            this.HttpContentBox.Name = "HttpContentBox";
            this.HttpContentBox.Size = new System.Drawing.Size(276, 207);
            this.HttpContentBox.TabIndex = 8;
            this.HttpContentBox.Text = "GET http://www.php.net/ HTTP/1.0\\r\\n\\r\\n";
            // 
            // TargetAddressBox
            // 
            this.TargetAddressBox.Location = new System.Drawing.Point(151, 28);
            this.TargetAddressBox.Name = "TargetAddressBox";
            this.TargetAddressBox.Size = new System.Drawing.Size(100, 20);
            this.TargetAddressBox.TabIndex = 4;
            this.TargetAddressBox.Text = "192.168.0.5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Preferred application port:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(296, 23);
            this.portBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(105, 20);
            this.portBox.TabIndex = 5;
            this.portBox.Value = new decimal(new int[] {
            27000,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.masterPointBox);
            this.groupBox2.Controls.Add(this.RoleCheckBox);
            this.groupBox2.Controls.Add(this.adapterGBox);
            this.groupBox2.Controls.Add(this.portBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(336, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 408);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Other settings";
            // 
            // masterPointBox
            // 
            this.masterPointBox.Controls.Add(this.MasterPortBox);
            this.masterPointBox.Controls.Add(this.label7);
            this.masterPointBox.Controls.Add(this.MasterIpBox);
            this.masterPointBox.Enabled = false;
            this.masterPointBox.Location = new System.Drawing.Point(173, 65);
            this.masterPointBox.Name = "masterPointBox";
            this.masterPointBox.Size = new System.Drawing.Size(270, 43);
            this.masterPointBox.TabIndex = 10;
            this.masterPointBox.TabStop = false;
            // 
            // MasterPortBox
            // 
            this.MasterPortBox.Location = new System.Drawing.Point(200, 14);
            this.MasterPortBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.MasterPortBox.Name = "MasterPortBox";
            this.MasterPortBox.Size = new System.Drawing.Size(55, 20);
            this.MasterPortBox.TabIndex = 10;
            this.MasterPortBox.Value = new decimal(new int[] {
            27000,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Address of the Master";
            // 
            // MasterIpBox
            // 
            this.MasterIpBox.Location = new System.Drawing.Point(94, 13);
            this.MasterIpBox.Name = "MasterIpBox";
            this.MasterIpBox.Size = new System.Drawing.Size(100, 20);
            this.MasterIpBox.TabIndex = 8;
            // 
            // RoleCheckBox
            // 
            this.RoleCheckBox.AutoSize = true;
            this.RoleCheckBox.Checked = true;
            this.RoleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RoleCheckBox.Location = new System.Drawing.Point(53, 83);
            this.RoleCheckBox.Name = "RoleCheckBox";
            this.RoleCheckBox.Size = new System.Drawing.Size(98, 17);
            this.RoleCheckBox.TabIndex = 7;
            this.RoleCheckBox.Text = "Role of the Master";
            this.RoleCheckBox.UseVisualStyleBackColor = true;
            this.RoleCheckBox.CheckedChanged += new System.EventHandler(this.RoleCheckBox_CheckedChanged);
            // 
            // adapterGBox
            // 
            this.adapterGBox.Controls.Add(this.MacLab);
            this.adapterGBox.Controls.Add(this.IpLab);
            this.adapterGBox.Controls.Add(this.label6);
            this.adapterGBox.Controls.Add(this.CurAdapterLabel);
            this.adapterGBox.Controls.Add(this.label4);
            this.adapterGBox.Controls.Add(this.AdapterBox);
            this.adapterGBox.Location = new System.Drawing.Point(6, 131);
            this.adapterGBox.Name = "adapterGBox";
            this.adapterGBox.Size = new System.Drawing.Size(458, 271);
            this.adapterGBox.TabIndex = 6;
            this.adapterGBox.TabStop = false;
            this.adapterGBox.Text = "AC adapter";
            // 
            // MacLab
            // 
            this.MacLab.AutoSize = true;
            this.MacLab.Location = new System.Drawing.Point(198, 239);
            this.MacLab.Name = "MacLab";
            this.MacLab.Size = new System.Drawing.Size(0, 13);
            this.MacLab.TabIndex = 5;
            // 
            // IpLab
            // 
            this.IpLab.AutoSize = true;
            this.IpLab.Location = new System.Drawing.Point(58, 239);
            this.IpLab.Name = "IpLab";
            this.IpLab.Size = new System.Drawing.Size(0, 13);
            this.IpLab.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Selected adapter:";
            // 
            // CurAdapterLabel
            // 
            this.CurAdapterLabel.AutoSize = true;
            this.CurAdapterLabel.Location = new System.Drawing.Point(108, 22);
            this.CurAdapterLabel.Name = "CurAdapterLabel";
            this.CurAdapterLabel.Size = new System.Drawing.Size(0, 13);
            this.CurAdapterLabel.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Current adapter:";
            // 
            // AdapterBox
            // 
            this.AdapterBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.AdapterBox.FormattingEnabled = true;
            this.AdapterBox.ItemHeight = 16;
            this.AdapterBox.Location = new System.Drawing.Point(6, 48);
            this.AdapterBox.Name = "AdapterBox";
            this.AdapterBox.Size = new System.Drawing.Size(446, 148);
            this.AdapterBox.TabIndex = 0;
            this.AdapterBox.SelectedIndexChanged += new System.EventHandler(this.AdapterBox_SelectedIndexChanged);
            // 
            // HttpsSetsBox
            // 
            this.HttpsSetsBox.Controls.Add(this.HttpContentBox);
            this.HttpsSetsBox.Controls.Add(this.label2);
            this.HttpsSetsBox.Location = new System.Drawing.Point(6, 86);
            this.HttpsSetsBox.Name = "HttpsSetsBox";
            this.HttpsSetsBox.Size = new System.Drawing.Size(315, 249);
            this.HttpsSetsBox.TabIndex = 11;
            this.HttpsSetsBox.TabStop = false;
            this.HttpsSetsBox.Text = "Options HTTP";
            // 
            // HttpSetsCheckBox
            // 
            this.HttpSetsCheckBox.AutoSize = true;
            this.HttpSetsCheckBox.Checked = true;
            this.HttpSetsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HttpSetsCheckBox.Location = new System.Drawing.Point(116, 63);
            this.HttpSetsCheckBox.Name = "HttpSetsCheckBox";
            this.HttpSetsCheckBox.Size = new System.Drawing.Size(83, 17);
            this.HttpSetsCheckBox.TabIndex = 14;
            this.HttpSetsCheckBox.Text = "HTTP flood";
            this.HttpSetsCheckBox.UseVisualStyleBackColor = true;
            this.HttpSetsCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 502);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.AttackParamsBox);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SetParamnBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.Text = "SettingsForm";
            this.AttackParamsBox.ResumeLayout(false);
            this.AttackParamsBox.PerformLayout();
            this.RestPoolsSetBox.ResumeLayout(false);
            this.RestPoolsSetBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.masterPointBox.ResumeLayout(false);
            this.masterPointBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MasterPortBox)).EndInit();
            this.adapterGBox.ResumeLayout(false);
            this.adapterGBox.PerformLayout();
            this.HttpsSetsBox.ResumeLayout(false);
            this.HttpsSetsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SetParamnBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox AttackParamsBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown portBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox HttpContentBox;
        private System.Windows.Forms.TextBox RestEndBox;
        private System.Windows.Forms.TextBox RestStartBox;
        private System.Windows.Forms.TextBox TargetAddressBox;
        private System.Windows.Forms.CheckBox FloodEnablingCheckBox;
        private System.Windows.Forms.GroupBox RestPoolsSetBox;
        private System.Windows.Forms.GroupBox adapterGBox;
        private System.Windows.Forms.ListBox AdapterBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label CurAdapterLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label IpLab;
        private System.Windows.Forms.Label MacLab;
        private System.Windows.Forms.CheckBox RoleCheckBox;
        private System.Windows.Forms.GroupBox masterPointBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox MasterIpBox;
        private System.Windows.Forms.NumericUpDown MasterPortBox;
        private System.Windows.Forms.GroupBox HttpsSetsBox;
        private System.Windows.Forms.CheckBox HttpSetsCheckBox;
    }
}