namespace FA_RFIDTextBox
{
    partial class WarningForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarningForm));
            this.InfoLabel = new System.Windows.Forms.Label();
            this.OKPictureBox = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.InfoLabel.Location = new System.Drawing.Point(3, 11);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(194, 54);
            this.InfoLabel.Text = "The line selected contains special values represented in blue color. Please edit " +
                "them under HEX mode if neccessary.";
            // 
            // OKPictureBox
            // 
            this.OKPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("OKPictureBox.Image")));
            this.OKPictureBox.Location = new System.Drawing.Point(139, 77);
            this.OKPictureBox.Name = "OKPictureBox";
            this.OKPictureBox.Size = new System.Drawing.Size(58, 20);
            this.OKPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.OKPictureBox.Click += new System.EventHandler(this.OKPictureBox_Click);
            // 
            // WarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(200, 100);
            this.Controls.Add(this.OKPictureBox);
            this.Controls.Add(this.InfoLabel);
            this.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular);
            this.MinimizeBox = false;
            this.Name = "WarningForm";
            this.Text = "Warning";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.WarningForm_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.PictureBox OKPictureBox;
    }
}