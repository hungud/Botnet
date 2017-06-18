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
            this.TargetPortNum = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HttpContentBox = new System.Windows.Forms.TextBox();
            this.TargetAddressBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AttackParamsBox.SuspendLayout();
            this.RestPoolsSetBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetPortNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetParamnBtn
            // 
            this.SetParamnBtn.Location = new System.Drawing.Point(27, 426);
            this.SetParamnBtn.Name = "SetParamnBtn";
            this.SetParamnBtn.Size = new System.Drawing.Size(145, 23);
            this.SetParamnBtn.TabIndex = 0;
            this.SetParamnBtn.Text = "Применить параметры";
            this.SetParamnBtn.UseVisualStyleBackColor = true;
            this.SetParamnBtn.Click += new System.EventHandler(this.SetParamnBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(212, 426);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Отмена";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Адрес цели";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AttackParamsBox
            // 
            this.AttackParamsBox.Controls.Add(this.RestPoolsSetBox);
            this.AttackParamsBox.Controls.Add(this.FloodEnablingCheckBox);
            this.AttackParamsBox.Controls.Add(this.TargetPortNum);
            this.AttackParamsBox.Controls.Add(this.label5);
            this.AttackParamsBox.Controls.Add(this.label2);
            this.AttackParamsBox.Controls.Add(this.HttpContentBox);
            this.AttackParamsBox.Controls.Add(this.TargetAddressBox);
            this.AttackParamsBox.Controls.Add(this.label1);
            this.AttackParamsBox.Location = new System.Drawing.Point(3, 4);
            this.AttackParamsBox.Name = "AttackParamsBox";
            this.AttackParamsBox.Size = new System.Drawing.Size(317, 327);
            this.AttackParamsBox.TabIndex = 3;
            this.AttackParamsBox.TabStop = false;
            this.AttackParamsBox.Text = "Параметры атаки (только для режима мастера)";
            // 
            // RestPoolsSetBox
            // 
            this.RestPoolsSetBox.Controls.Add(this.RestStartBox);
            this.RestPoolsSetBox.Controls.Add(this.RestEndBox);
            this.RestPoolsSetBox.Location = new System.Drawing.Point(24, 241);
            this.RestPoolsSetBox.Name = "RestPoolsSetBox";
            this.RestPoolsSetBox.Size = new System.Drawing.Size(260, 65);
            this.RestPoolsSetBox.TabIndex = 13;
            this.RestPoolsSetBox.TabStop = false;
            this.RestPoolsSetBox.Text = "Диапазон запрещенных адресов";
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
            this.FloodEnablingCheckBox.Location = new System.Drawing.Point(118, 206);
            this.FloodEnablingCheckBox.Name = "FloodEnablingCheckBox";
            this.FloodEnablingCheckBox.Size = new System.Drawing.Size(77, 17);
            this.FloodEnablingCheckBox.TabIndex = 12;
            this.FloodEnablingCheckBox.Text = "UDP флуд";
            this.FloodEnablingCheckBox.UseVisualStyleBackColor = true;
            this.FloodEnablingCheckBox.CheckedChanged += new System.EventHandler(this.FloodEnablingCheckBox_CheckedChanged);
            // 
            // TargetPortNum
            // 
            this.TargetPortNum.Location = new System.Drawing.Point(251, 28);
            this.TargetPortNum.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.TargetPortNum.Name = "TargetPortNum";
            this.TargetPortNum.Size = new System.Drawing.Size(49, 20);
            this.TargetPortNum.TabIndex = 11;
            this.TargetPortNum.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Порт";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Текст HTTP запроса";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // HttpContentBox
            // 
            this.HttpContentBox.Location = new System.Drawing.Point(24, 79);
            this.HttpContentBox.Multiline = true;
            this.HttpContentBox.Name = "HttpContentBox";
            this.HttpContentBox.Size = new System.Drawing.Size(276, 108);
            this.HttpContentBox.TabIndex = 8;
            this.HttpContentBox.Text = "GET http://www.php.net/ HTTP/1.0\\r\\n\\r\\n";
            // 
            // TargetAddressBox
            // 
            this.TargetAddressBox.Location = new System.Drawing.Point(80, 28);
            this.TargetAddressBox.Name = "TargetAddressBox";
            this.TargetAddressBox.Size = new System.Drawing.Size(100, 20);
            this.TargetAddressBox.TabIndex = 4;
            this.TargetAddressBox.Text = "192.168.0.5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Предпочитаемый порт приложения:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(97, 32);
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
            this.groupBox2.Controls.Add(this.portBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(3, 337);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(317, 83);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Прочие настройки";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 454);
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
            ((System.ComponentModel.ISupportInitialize)(this.TargetPortNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown TargetPortNum;
        private System.Windows.Forms.CheckBox FloodEnablingCheckBox;
        private System.Windows.Forms.GroupBox RestPoolsSetBox;
    }
}