namespace FA_RFIDTextBox
{
    partial class RFIDTextBox
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BasePanel = new System.Windows.Forms.Panel();
            this.VisualPanel = new System.Windows.Forms.Panel();
            this.EditeTextBox = new System.Windows.Forms.TextBox();
            this.ScrollPanel = new System.Windows.Forms.Panel();
            this.VisualScrollBar = new System.Windows.Forms.VScrollBar();
            this.AddressPanel = new System.Windows.Forms.Panel();
            this.UpdateTimer = new System.Windows.Forms.Timer();
            this.BasePanel.SuspendLayout();
            this.VisualPanel.SuspendLayout();
            this.ScrollPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BasePanel
            // 
            this.BasePanel.Controls.Add(this.VisualPanel);
            this.BasePanel.Controls.Add(this.ScrollPanel);
            this.BasePanel.Controls.Add(this.AddressPanel);
            this.BasePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BasePanel.Location = new System.Drawing.Point(0, 0);
            this.BasePanel.Name = "BasePanel";
            this.BasePanel.Size = new System.Drawing.Size(246, 350);
            // 
            // VisualPanel
            // 
            this.VisualPanel.BackColor = System.Drawing.Color.White;
            this.VisualPanel.Controls.Add(this.EditeTextBox);
            this.VisualPanel.Location = new System.Drawing.Point(30, 1);
            this.VisualPanel.Name = "VisualPanel";
            this.VisualPanel.Size = new System.Drawing.Size(183, 346);
            this.VisualPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VisualPanel_MouseDown);
            // 
            // EditeTextBox
            // 
            this.EditeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EditeTextBox.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular);
            this.EditeTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.EditeTextBox.Location = new System.Drawing.Point(28, 50);
            this.EditeTextBox.Name = "EditeTextBox";
            this.EditeTextBox.Size = new System.Drawing.Size(100, 17);
            this.EditeTextBox.TabIndex = 0;
            this.EditeTextBox.Visible = false;
            this.EditeTextBox.TextChanged += new System.EventHandler(this.EditeTextBox_TextChanged);
            this.EditeTextBox.LostFocus += new System.EventHandler(this.EditeTextBox_LostFocus);
            // 
            // ScrollPanel
            // 
            this.ScrollPanel.Controls.Add(this.VisualScrollBar);
            this.ScrollPanel.Location = new System.Drawing.Point(216, 0);
            this.ScrollPanel.Name = "ScrollPanel";
            this.ScrollPanel.Size = new System.Drawing.Size(30, 350);
            // 
            // VisualScrollBar
            // 
            this.VisualScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VisualScrollBar.Location = new System.Drawing.Point(0, 0);
            this.VisualScrollBar.Name = "VisualScrollBar";
            this.VisualScrollBar.Size = new System.Drawing.Size(30, 350);
            this.VisualScrollBar.TabIndex = 0;
            this.VisualScrollBar.ValueChanged += new System.EventHandler(this.VisualScrollBar_ValueChanged);
            // 
            // AddressPanel
            // 
            this.AddressPanel.BackColor = System.Drawing.Color.Lime;
            this.AddressPanel.Location = new System.Drawing.Point(0, 0);
            this.AddressPanel.Name = "AddressPanel";
            this.AddressPanel.Size = new System.Drawing.Size(30, 350);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // RFIDTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.BasePanel);
            this.Name = "RFIDTextBox";
            this.Size = new System.Drawing.Size(246, 350);
            this.BasePanel.ResumeLayout(false);
            this.VisualPanel.ResumeLayout(false);
            this.ScrollPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BasePanel;
        private System.Windows.Forms.Panel VisualPanel;
        private System.Windows.Forms.Panel ScrollPanel;
        private System.Windows.Forms.VScrollBar VisualScrollBar;
        private System.Windows.Forms.Panel AddressPanel;
        private System.Windows.Forms.TextBox EditeTextBox;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}
