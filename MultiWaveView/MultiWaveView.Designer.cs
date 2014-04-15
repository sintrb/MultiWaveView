namespace Sin.UI
{
    partial class MultiWaveView
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
            this.SuspendLayout();
            // 
            // MultiWaveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.DoubleBuffered = true;
            this.Name = "MultiWaveView";
            this.Size = new System.Drawing.Size(360, 107);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MultiWaveView_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MultiWaveView_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MultiWaveView_MouseUp);
            this.SizeChanged += new System.EventHandler(this.MultiWaveView_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
