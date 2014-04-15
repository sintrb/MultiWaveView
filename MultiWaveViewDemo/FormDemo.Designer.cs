namespace MultiWaveViewDemo
{
    partial class FormDemo
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
            this.components = new System.ComponentModel.Container();
            Sin.UI.WaveProperty waveProperty1 = new Sin.UI.WaveProperty();
            Sin.UI.WaveProperty waveProperty2 = new Sin.UI.WaveProperty();
            Sin.UI.WaveProperty waveProperty3 = new Sin.UI.WaveProperty();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDemo));
            this.tmRefresh = new System.Windows.Forms.Timer(this.components);
            this.mwvWaves = new Sin.UI.MultiWaveView();
            this.SuspendLayout();
            // 
            // tmRefresh
            // 
            this.tmRefresh.Enabled = true;
            this.tmRefresh.Interval = 10;
            this.tmRefresh.Tick += new System.EventHandler(this.tmRefresh_Tick);
            // 
            // mwvWaves
            // 
            this.mwvWaves.BackColor = System.Drawing.SystemColors.ControlText;
            this.mwvWaves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mwvWaves.GridColor = System.Drawing.Color.Green;
            this.mwvWaves.GridSize = ((uint)(20u));
            this.mwvWaves.Location = new System.Drawing.Point(0, 0);
            this.mwvWaves.MouseLinePen = System.Drawing.Color.WhiteSmoke;
            this.mwvWaves.Name = "mwvWaves";
            this.mwvWaves.PointCount = 1024;
            this.mwvWaves.Size = new System.Drawing.Size(795, 473);
            this.mwvWaves.TabIndex = 0;
            waveProperty1.AutoUpdateMaxMin = true;
            waveProperty1.Color = System.Drawing.Color.Chartreuse;
            waveProperty1.Max = 1;
            waveProperty1.Min = -1;
            waveProperty1.Name = "正弦";
            waveProperty2.AutoUpdateMaxMin = true;
            waveProperty2.Color = System.Drawing.Color.White;
            waveProperty2.Max = 1;
            waveProperty2.Min = -1;
            waveProperty2.Name = "余弦";
            waveProperty3.AutoUpdateMaxMin = true;
            waveProperty3.Color = System.Drawing.Color.Red;
            waveProperty3.Max = 1;
            waveProperty3.Min = -1;
            waveProperty3.Name = "正切";
            this.mwvWaves.Waves = new Sin.UI.WaveProperty[] {
        waveProperty1,
        waveProperty2,
        waveProperty3};
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 473);
            this.Controls.Add(this.mwvWaves);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDemo";
            this.Text = "MultiWaveViewDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private Sin.UI.MultiWaveView mwvWaves;
        private System.Windows.Forms.Timer tmRefresh;

    }
}

