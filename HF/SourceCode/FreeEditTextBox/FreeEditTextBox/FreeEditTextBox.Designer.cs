namespace FreeEditTextBox
{
    partial class FreeEditTextBox
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
            this.EditTextBox = new System.Windows.Forms.TextBox();
            this.UpdateTimer = new System.Windows.Forms.Timer();
            this.BasePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BasePanel
            // 
            this.BasePanel.Controls.Add(this.EditTextBox);
            this.BasePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BasePanel.Location = new System.Drawing.Point(0, 0);
            this.BasePanel.Name = "BasePanel";
            this.BasePanel.Size = new System.Drawing.Size(150, 150);
            // 
            // EditTextBox
            // 
            this.EditTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EditTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditTextBox.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular);
            this.EditTextBox.Location = new System.Drawing.Point(0, 0);
            this.EditTextBox.Multiline = true;
            this.EditTextBox.Name = "EditTextBox";
            this.EditTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EditTextBox.Size = new System.Drawing.Size(150, 150);
            this.EditTextBox.TabIndex = 0;
            this.EditTextBox.TextChanged += new System.EventHandler(this.EditTextBox_TextChanged);
            this.EditTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditTextBox_KeyPress);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // FreeEditTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.BasePanel);
            this.Name = "FreeEditTextBox";
            this.BasePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BasePanel;
        private System.Windows.Forms.TextBox EditTextBox;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}
