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
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.ParamsBtn = new System.Windows.Forms.Button();
            this.DeviceObserveGrid = new System.Windows.Forms.DataGridView();
            this.Ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClearLogBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.refreshAliesBtn = new System.Windows.Forms.Button();
            this.MrB = new System.Windows.Forms.PictureBox();
            this.ControlTab = new System.Windows.Forms.TabControl();
            this.MasterMode = new System.Windows.Forms.TabPage();
            this.HostMode = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceObserveGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MrB)).BeginInit();
            this.ControlTab.SuspendLayout();
            this.MasterMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.OrangeRed;
            this.StartBtn.Location = new System.Drawing.Point(424, 6);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(105, 66);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Старт";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(538, 6);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(105, 66);
            this.StopBtn.TabIndex = 2;
            this.StopBtn.Text = "Стоп";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // ParamsBtn
            // 
            this.ParamsBtn.Location = new System.Drawing.Point(175, 27);
            this.ParamsBtn.Name = "ParamsBtn";
            this.ParamsBtn.Size = new System.Drawing.Size(121, 23);
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
            this.DeviceObserveGrid.Location = new System.Drawing.Point(6, 27);
            this.DeviceObserveGrid.Name = "DeviceObserveGrid";
            this.DeviceObserveGrid.ReadOnly = true;
            this.DeviceObserveGrid.RowHeadersVisible = false;
            this.DeviceObserveGrid.Size = new System.Drawing.Size(216, 245);
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
            this.ClearLogBtn.Location = new System.Drawing.Point(557, 89);
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
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Подключенные устройства";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Сообщения";
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LogBox.DetectUrls = false;
            this.LogBox.Location = new System.Drawing.Point(255, 118);
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(381, 370);
            this.LogBox.TabIndex = 12;
            this.LogBox.Text = "";
            // 
            // refreshAliesBtn
            // 
            this.refreshAliesBtn.FlatAppearance.BorderSize = 0;
            this.refreshAliesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.refreshAliesBtn.Location = new System.Drawing.Point(148, 3);
            this.refreshAliesBtn.Name = "refreshAliesBtn";
            this.refreshAliesBtn.Size = new System.Drawing.Size(78, 23);
            this.refreshAliesBtn.TabIndex = 13;
            this.refreshAliesBtn.Text = "Обновить";
            this.refreshAliesBtn.UseVisualStyleBackColor = true;
            this.refreshAliesBtn.Click += new System.EventHandler(this.refreshAliesBtn_Click);
            // 
            // MrB
            // 
            this.MrB.Location = new System.Drawing.Point(36, 12);
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
            this.ControlTab.Location = new System.Drawing.Point(12, 96);
            this.ControlTab.Name = "ControlTab";
            this.ControlTab.SelectedIndex = 0;
            this.ControlTab.Size = new System.Drawing.Size(237, 392);
            this.ControlTab.TabIndex = 15;
            this.ControlTab.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.TabChangingHandler);
            // 
            // MasterMode
            // 
            this.MasterMode.Controls.Add(this.DeviceObserveGrid);
            this.MasterMode.Controls.Add(this.label2);
            this.MasterMode.Controls.Add(this.refreshAliesBtn);
            this.MasterMode.Location = new System.Drawing.Point(4, 22);
            this.MasterMode.Name = "MasterMode";
            this.MasterMode.Padding = new System.Windows.Forms.Padding(3);
            this.MasterMode.Size = new System.Drawing.Size(229, 366);
            this.MasterMode.TabIndex = 0;
            this.MasterMode.Text = "Режим Мастера";
            this.MasterMode.UseVisualStyleBackColor = true;
            // 
            // HostMode
            // 
            this.HostMode.Location = new System.Drawing.Point(4, 22);
            this.HostMode.Name = "HostMode";
            this.HostMode.Padding = new System.Windows.Forms.Padding(3);
            this.HostMode.Size = new System.Drawing.Size(229, 366);
            this.HostMode.TabIndex = 1;
            this.HostMode.Text = "Режим хоста";
            this.HostMode.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 500);
            this.Controls.Add(this.ControlTab);
            this.Controls.Add(this.MrB);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ClearLogBtn);
            this.Controls.Add(this.ParamsBtn);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.StartBtn);
            this.Name = "MainForm";
            this.Text = "BananaBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.DeviceObserveGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MrB)).EndInit();
            this.ControlTab.ResumeLayout(false);
            this.MasterMode.ResumeLayout(false);
            this.MasterMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Button ParamsBtn;
        private System.Windows.Forms.DataGridView DeviceObserveGrid;
        private System.Windows.Forms.Button ClearLogBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.Button refreshAliesBtn;
        private System.Windows.Forms.PictureBox MrB;
        private System.Windows.Forms.TabControl ControlTab;
        private System.Windows.Forms.TabPage MasterMode;
        private System.Windows.Forms.TabPage HostMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}

