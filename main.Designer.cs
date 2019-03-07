namespace ContinueDiscovering
{
    partial class fmMain
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
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.chDeviceAddress = new System.Windows.Forms.ColumnHeader();
            this.chDeviceName = new System.Windows.Forms.ColumnHeader();
            this.btClear = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(12, 12);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 0;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(93, 12);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 1;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // lvDevices
            // 
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDeviceAddress,
            this.chDeviceName});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.Location = new System.Drawing.Point(12, 41);
            this.lvDevices.MultiSelect = false;
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(447, 174);
            this.lvDevices.TabIndex = 2;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            // 
            // chDeviceAddress
            // 
            this.chDeviceAddress.Text = "Address";
            this.chDeviceAddress.Width = 200;
            // 
            // chDeviceName
            // 
            this.chDeviceName.Text = "Name";
            this.chDeviceName.Width = 200;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(384, 221);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(75, 23);
            this.btClear.TabIndex = 3;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(12, 250);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(447, 199);
            this.lbLog.TabIndex = 4;
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 464);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.lvDevices);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continue Discovering Demo";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmMain_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ColumnHeader chDeviceAddress;
        private System.Windows.Forms.ColumnHeader chDeviceName;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.ListBox lbLog;
    }
}

