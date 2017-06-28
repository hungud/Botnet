namespace Botnet
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.ParamsBtn = new System.Windows.Forms.Button();
            this.DeviceObserveGrid = new System.Windows.Forms.DataGridView();
            this.Ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClearLogBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.refreshAliesBtn = new System.Windows.Forms.Button();
            this.MrB = new System.Windows.Forms.PictureBox();
            this.ControlTab = new System.Windows.Forms.TabControl();
            this.MasterMode = new System.Windows.Forms.TabPage();
            this.MIpEndPointlab = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MUdpTotalLab = new System.Windows.Forms.Label();
            this.MHttpTotLab = new System.Windows.Forms.Label();
            this.MUpdLocLab = new System.Windows.Forms.Label();
            this.MHttpLocLab = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.HostMode = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.HUdpTotalLab = new System.Windows.Forms.Label();
            this.HostHttpTotalLab = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.HMasterLab = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.HIpEndPointLab = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ConnectBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceObserveGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MrB)).BeginInit();
            this.ControlTab.SuspendLayout();
            this.MasterMode.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.HostMode.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.OrangeRed;
            this.StartBtn.Location = new System.Drawing.Point(451, 7);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(90, 66);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Старт";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(547, 9);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(86, 64);
            this.StopBtn.TabIndex = 2;
            this.StopBtn.Text = "Стоп";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // ParamsBtn
            // 
            this.ParamsBtn.Location = new System.Drawing.Point(355, 9);
            this.ParamsBtn.Name = "ParamsBtn";
            this.ParamsBtn.Size = new System.Drawing.Size(90, 64);
            this.ParamsBtn.TabIndex = 4;
            this.ParamsBtn.Text = "Параметры атаки";
            this.ParamsBtn.UseVisualStyleBackColor = true;
            this.ParamsBtn.Click += new System.EventHandler(this.ParamsBtn_Click);
            // 
            // DeviceObserveGrid
            // 
            this.DeviceObserveGrid.AllowUserToAddRows = false;
            this.DeviceObserveGrid.AllowUserToDeleteRows = false;
            this.DeviceObserveGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DeviceObserveGrid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.DeviceObserveGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DeviceObserveGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Ip,
            this.port,
            this.Status});
            this.DeviceObserveGrid.Location = new System.Drawing.Point(6, 108);
            this.DeviceObserveGrid.Name = "DeviceObserveGrid";
            this.DeviceObserveGrid.ReadOnly = true;
            this.DeviceObserveGrid.RowHeadersVisible = false;
            this.DeviceObserveGrid.Size = new System.Drawing.Size(216, 164);
            this.DeviceObserveGrid.TabIndex = 7;
            this.DeviceObserveGrid.Tag = "";
            // 
            // Ip
            // 
            this.Ip.HeaderText = "Ip";
            this.Ip.Name = "Ip";
            this.Ip.ReadOnly = true;
            this.Ip.Width = 41;
            // 
            // port
            // 
            this.port.HeaderText = "Port";
            this.port.Name = "port";
            this.port.ReadOnly = true;
            this.port.Width = 51;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 62;
            // 
            // ClearLogBtn
            // 
            this.ClearLogBtn.Location = new System.Drawing.Point(547, 529);
            this.ClearLogBtn.Name = "ClearLogBtn";
            this.ClearLogBtn.Size = new System.Drawing.Size(86, 23);
            this.ClearLogBtn.TabIndex = 9;
            this.ClearLogBtn.Text = "Очистить лог";
            this.ClearLogBtn.UseVisualStyleBackColor = true;
            this.ClearLogBtn.Click += new System.EventHandler(this.ClearLogBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Подключенные устройства";
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LogBox.DetectUrls = false;
            this.LogBox.Location = new System.Drawing.Point(255, 79);
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(381, 444);
            this.LogBox.TabIndex = 12;
            this.LogBox.Text = "";
            // 
            // refreshAliesBtn
            // 
            this.refreshAliesBtn.FlatAppearance.BorderSize = 0;
            this.refreshAliesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.refreshAliesBtn.Location = new System.Drawing.Point(148, 86);
            this.refreshAliesBtn.Name = "refreshAliesBtn";
            this.refreshAliesBtn.Size = new System.Drawing.Size(78, 19);
            this.refreshAliesBtn.TabIndex = 13;
            this.refreshAliesBtn.Text = "Обновить";
            this.refreshAliesBtn.UseVisualStyleBackColor = true;
            this.refreshAliesBtn.Click += new System.EventHandler(this.refreshAliesBtn_Click);
            // 
            // MrB
            // 
            this.MrB.Location = new System.Drawing.Point(99, 12);
            this.MrB.Name = "MrB";
            this.MrB.Size = new System.Drawing.Size(50, 38);
            this.MrB.TabIndex = 14;
            this.MrB.TabStop = false;
            this.MrB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MrB_MouseClick);
            // 
            // ControlTab
            // 
            this.ControlTab.Controls.Add(this.MasterMode);
            this.ControlTab.Controls.Add(this.HostMode);
            this.ControlTab.Location = new System.Drawing.Point(16, 57);
            this.ControlTab.Name = "ControlTab";
            this.ControlTab.SelectedIndex = 0;
            this.ControlTab.Size = new System.Drawing.Size(237, 470);
            this.ControlTab.TabIndex = 15;
            this.ControlTab.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabChangingHandler);
            // 
            // MasterMode
            // 
            this.MasterMode.Controls.Add(this.MIpEndPointlab);
            this.MasterMode.Controls.Add(this.label9);
            this.MasterMode.Controls.Add(this.label8);
            this.MasterMode.Controls.Add(this.groupBox1);
            this.MasterMode.Controls.Add(this.DeviceObserveGrid);
            this.MasterMode.Controls.Add(this.label2);
            this.MasterMode.Controls.Add(this.refreshAliesBtn);
            this.MasterMode.Location = new System.Drawing.Point(4, 22);
            this.MasterMode.Name = "MasterMode";
            this.MasterMode.Padding = new System.Windows.Forms.Padding(3);
            this.MasterMode.Size = new System.Drawing.Size(229, 444);
            this.MasterMode.TabIndex = 0;
            this.MasterMode.Text = "Режим Мастера";
            this.MasterMode.UseVisualStyleBackColor = true;
            // 
            // MIpEndPointlab
            // 
            this.MIpEndPointlab.AutoSize = true;
            this.MIpEndPointlab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MIpEndPointlab.Location = new System.Drawing.Point(99, 48);
            this.MIpEndPointlab.Name = "MIpEndPointlab";
            this.MIpEndPointlab.Size = new System.Drawing.Size(64, 17);
            this.MIpEndPointlab.TabIndex = 17;
            this.MIpEndPointlab.Text = "0.0.0.0:0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(59, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "IP:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(59, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Роль: Мастер";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MUdpTotalLab);
            this.groupBox1.Controls.Add(this.MHttpTotLab);
            this.groupBox1.Controls.Add(this.MUpdLocLab);
            this.groupBox1.Controls.Add(this.MHttpLocLab);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 278);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 160);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Статистика";
            // 
            // MUdpTotalLab
            // 
            this.MUdpTotalLab.AutoSize = true;
            this.MUdpTotalLab.Location = new System.Drawing.Point(162, 124);
            this.MUdpTotalLab.Name = "MUdpTotalLab";
            this.MUdpTotalLab.Size = new System.Drawing.Size(13, 13);
            this.MUdpTotalLab.TabIndex = 9;
            this.MUdpTotalLab.Text = "0";
            // 
            // MHttpTotLab
            // 
            this.MHttpTotLab.AutoSize = true;
            this.MHttpTotLab.Location = new System.Drawing.Point(70, 124);
            this.MHttpTotLab.Name = "MHttpTotLab";
            this.MHttpTotLab.Size = new System.Drawing.Size(13, 13);
            this.MHttpTotLab.TabIndex = 8;
            this.MHttpTotLab.Text = "0";
            // 
            // MUpdLocLab
            // 
            this.MUpdLocLab.AutoSize = true;
            this.MUpdLocLab.Location = new System.Drawing.Point(162, 63);
            this.MUpdLocLab.Name = "MUpdLocLab";
            this.MUpdLocLab.Size = new System.Drawing.Size(13, 13);
            this.MUpdLocLab.TabIndex = 7;
            this.MUpdLocLab.Text = "0";
            // 
            // MHttpLocLab
            // 
            this.MHttpLocLab.AutoSize = true;
            this.MHttpLocLab.Location = new System.Drawing.Point(70, 63);
            this.MHttpLocLab.Name = "MHttpLocLab";
            this.MHttpLocLab.Size = new System.Drawing.Size(13, 13);
            this.MHttpLocLab.TabIndex = 6;
            this.MHttpLocLab.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(123, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "UDP:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "HTTP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Отправлено всего:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(123, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "UDP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "HTTP:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Отправлено с текущей машины:";
            // 
            // HostMode
            // 
            this.HostMode.Controls.Add(this.groupBox2);
            this.HostMode.Controls.Add(this.HMasterLab);
            this.HostMode.Controls.Add(this.label10);
            this.HostMode.Controls.Add(this.HIpEndPointLab);
            this.HostMode.Controls.Add(this.label11);
            this.HostMode.Controls.Add(this.label12);
            this.HostMode.Location = new System.Drawing.Point(4, 22);
            this.HostMode.Name = "HostMode";
            this.HostMode.Padding = new System.Windows.Forms.Padding(3);
            this.HostMode.Size = new System.Drawing.Size(229, 444);
            this.HostMode.TabIndex = 1;
            this.HostMode.Text = "Режим хоста";
            this.HostMode.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.HUdpTotalLab);
            this.groupBox2.Controls.Add(this.HostHttpTotalLab);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Location = new System.Drawing.Point(3, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 160);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Статистика";
            // 
            // HUdpTotalLab
            // 
            this.HUdpTotalLab.AutoSize = true;
            this.HUdpTotalLab.Location = new System.Drawing.Point(162, 63);
            this.HUdpTotalLab.Name = "HUdpTotalLab";
            this.HUdpTotalLab.Size = new System.Drawing.Size(13, 13);
            this.HUdpTotalLab.TabIndex = 7;
            this.HUdpTotalLab.Text = "0";
            // 
            // HostHttpTotalLab
            // 
            this.HostHttpTotalLab.AutoSize = true;
            this.HostHttpTotalLab.Location = new System.Drawing.Point(70, 63);
            this.HostHttpTotalLab.Name = "HostHttpTotalLab";
            this.HostHttpTotalLab.Size = new System.Drawing.Size(13, 13);
            this.HostHttpTotalLab.TabIndex = 6;
            this.HostHttpTotalLab.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(123, 63);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(33, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "UDP:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(25, 63);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(39, 13);
            this.label21.TabIndex = 1;
            this.label21.Text = "HTTP:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(25, 27);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(171, 13);
            this.label22.TabIndex = 0;
            this.label22.Text = "Отправлено с текущей машины:";
            // 
            // HMasterLab
            // 
            this.HMasterLab.AutoSize = true;
            this.HMasterLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HMasterLab.Location = new System.Drawing.Point(94, 83);
            this.HMasterLab.Name = "HMasterLab";
            this.HMasterLab.Size = new System.Drawing.Size(64, 17);
            this.HMasterLab.TabIndex = 22;
            this.HMasterLab.Text = "0.0.0.0:0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(32, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 17);
            this.label10.TabIndex = 21;
            this.label10.Text = "Мастер:";
            // 
            // HIpEndPointLab
            // 
            this.HIpEndPointLab.AutoSize = true;
            this.HIpEndPointLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HIpEndPointLab.Location = new System.Drawing.Point(94, 46);
            this.HIpEndPointLab.Name = "HIpEndPointLab";
            this.HIpEndPointLab.Size = new System.Drawing.Size(64, 17);
            this.HIpEndPointLab.TabIndex = 20;
            this.HIpEndPointLab.Text = "0.0.0.0:0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(54, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 17);
            this.label11.TabIndex = 19;
            this.label11.Text = "IP:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(54, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 17);
            this.label12.TabIndex = 18;
            this.label12.Text = "Роль: Хост";
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.Location = new System.Drawing.Point(259, 9);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(90, 64);
            this.ConnectBtn.TabIndex = 16;
            this.ConnectBtn.Text = "Подключить";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 555);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.ControlTab);
            this.Controls.Add(this.MrB);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.ClearLogBtn);
            this.Controls.Add(this.ParamsBtn);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.StartBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "BananaBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.DeviceObserveGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MrB)).EndInit();
            this.ControlTab.ResumeLayout(false);
            this.MasterMode.ResumeLayout(false);
            this.MasterMode.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.HostMode.ResumeLayout(false);
            this.HostMode.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Button ParamsBtn;
        private System.Windows.Forms.DataGridView DeviceObserveGrid;
        private System.Windows.Forms.Button ClearLogBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.Button refreshAliesBtn;
        private System.Windows.Forms.PictureBox MrB;
        private System.Windows.Forms.TabControl ControlTab;
        private System.Windows.Forms.TabPage MasterMode;
        private System.Windows.Forms.TabPage HostMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Button ConnectBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label MUpdLocLab;
        private System.Windows.Forms.Label MHttpLocLab;
        private System.Windows.Forms.Label MUdpTotalLab;
        private System.Windows.Forms.Label MHttpTotLab;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label MIpEndPointlab;
        private System.Windows.Forms.Label HIpEndPointLab;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label HMasterLab;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label HUdpTotalLab;
        private System.Windows.Forms.Label HostHttpTotalLab;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
    }
}

