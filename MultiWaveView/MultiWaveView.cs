using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Sin.UI
{
    public sealed partial class MultiWaveView : UserControl
    {
        public static String VERSION = "1.1";
        private int[] xLocations = null;
        private int valIndex = 0;
        private long valueCount = 0;

        private bool Authed
        {
            get
            {
                return !AU_need;
            }
        }

        private bool Expired
        {
            get
            {
                return AU_need && AU_expired;
            }
        }

        #region 控件属性
        private int _WaveCount = 0;
        [Category("外观"), Description("波形数目，不能直接编辑，可以通过修改Waves来改变。"), DefaultValue(0)]
        public int WaveCount
        {
            get
            {
                return _WaveCount;
            }
        }

        private int _PointCount = 128;
        [Category("外观"), Description("每条波上的点数"), DefaultValue(128)]
        public int PointCount
        {
            get
            {
                return _PointCount;
            }
            set
            {
                _PointCount = value;
                ReadyXLocation();
                ReadyWaves();
                Invalidate();
            }
        }

        private WaveProperty[] _Waves = null;
        [Category("外观"), Description("波形"), DefaultValue(null)]
        public WaveProperty[] Waves
        {
            get
            {
                return _Waves;
            }
            set
            {
                if (!Authed && value.Length > 5)
                {
                    MessageBox.Show("未注册版本最多只能设置5条波形");
                }
                else
                {
                    _WaveCount = value.Length;
                    _Waves = value;
                    ReadyWaves();
                }
            }
        }

        private bool _ShowLegend = true;
        [Category("外观"), Description("是否显示标注"), DefaultValue(true)]
        public bool ShowLegend
        {
            get
            {
                return _ShowLegend;
            }
            set
            {
                _ShowLegend = value;
                this.Invalidate();
            }
        }


        private bool _ShowGrid = true;
        [Category("外观"), Description("是否显示网格"), DefaultValue(true)]
        public bool ShowGrid
        {
            get
            {
                return _ShowGrid;
            }
            set
            {
                _ShowGrid = value;
                this.Invalidate();
            }
        }


        private uint _GridSize = 20;
        [Category("外观"), Description("网格大小"), DefaultValue(20)]
        public uint GridSize
        {
            get
            {
                return _GridSize;
            }
            set
            {
                if (value == 0)
                {
                    throw new Exception("网格大小必须大于0");
                }
                _GridSize = value;
                this.Invalidate();
            }
        }

        private Pen _GridPen = Pens.Green;
        private Color _GridColor = Color.Green;
        [Category("外观"), Description("网格颜色")]
        public Color GridColor
        {
            get
            {
                return _GridColor;
            }
            set
            {
                _GridPen = new Pen(value);
                _GridColor = value;
                this.Invalidate();
            }
        }


        private int _ValueLineIndex = -1;
        private bool _ShowValueLine = true;
        [Category("外观"), Description("显示数值线"), DefaultValue(true)]
        public bool ShowValueLine
        {
            get
            {
                return _ShowValueLine;
            }
            set
            {
                _ShowValueLine = value;
            }
        }

        private bool _ValueLineWithMouse = false;
        [Category("外观"), Description("鼠标移动的时候改变数值线位置"), DefaultValue(false)]
        public bool ValueLineWithMouse
        {
            get
            {
                return _ValueLineWithMouse;
            }
            set
            {
                _ValueLineWithMouse = value;
            }
        }

        private Pen _ValueLinePen = Pens.Red;
        [Category("外观"), Description("数值线颜色")]
        public Color MouseLinePen
        {
            get
            {
                return _ValueLinePen.Color;
            }
            set
            {
                _ValueLinePen = new Pen(value);
                _ValueLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            }
        }


        #endregion


        #region 接口方法
        public double Value
        {
            set
            {
                this.Values = new double[] { value };
            }
        }

        public double[] Values
        {
            set
            {
                if (value.Length == _WaveCount)
                {
                    for (int i = 0; i < _WaveCount; ++i)
                    {
                        WaveProperty wp = _Waves[i];
                        if (wp.AutoUpdateMaxMin)
                        {
                            if (value[i] > wp.Max)
                                wp.Max = value[i];
                            if (value[i] < wp.Min)
                                wp.Min = value[i];
                        }
                        wp.Values[valIndex] = value[i];
                        wp.CurVal = value[i];
                    }

                    valIndex = (valIndex + 1) % _PointCount;
                    ++valueCount;
                    this.Invalidate();
                }
                else
                {
                    throw new Exception(String.Format("更新的数据个数({0})和波形条数({1})不等", value.Length, _WaveCount));
                }
            }
        }

        #endregion


        private Font AU_font = null;
        private String AU_str = "未授权...";
        private bool AU_expired = true;
        private bool AU_need = false;
        public MultiWaveView()
        {
            AU_expired = AU_need && DateTime.Parse("2015-01-01") < DateTime.Now;
            int fontsize = 36;
            if (AU_expired)
            {
                AU_str = "已过期,请联系QQ:472497084";
                fontsize = 20;
            }

            try
            {
                AU_font = new Font("微软雅黑", fontsize, System.Drawing.FontStyle.Italic);
            }
            catch (Exception)
            {
                AU_font = new Font(FontFamily.Families[0], fontsize, System.Drawing.FontStyle.Italic);
            }


            _ValueLinePen = new Pen(_ValueLinePen.Color);
            _ValueLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            InitializeComponent();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ReadyXLocation();
            this.ReadyWaves();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int h = this.Height;
            int w = this.Width;

            if (!Authed || Expired)
            {
                SizeF size = g.MeasureString(AU_str, AU_font);
                Color c = Color.FromArgb(255 - BackColor.R, 255 - BackColor.G, 255 - BackColor.B);
                g.DrawString(AU_str, AU_font, new SolidBrush(c), new PointF(w - size.Width, h - size.Height));
            }

            

            // 网格
            if (this._ShowGrid)
            {
                int gw = (int)(w / _GridSize);
                for (int i = 0; i <= gw; ++i)
                {
                    g.DrawLine(_GridPen, i * _GridSize, 0, i * _GridSize, h);
                }
                int gh = (int)(h / _GridSize);
                for (int i = 0; i <= gh; ++i)
                {
                    g.DrawLine(_GridPen, 0, i * _GridSize, w, i * _GridSize);
                }
            }

            if (Expired)
                return;

            if (this._WaveCount > 0)
            {
                // 绘制各个波
                int stix = (valIndex) % _PointCount;
                for (int i = 0; i < _WaveCount; ++i)
                {
                    WaveProperty wp = _Waves[i];
                    if (!wp.Enable)
                        continue;
                    double k = wp.Max == wp.Min ? 0 : (this.Height - 8) / (wp.Max - wp.Min);
                    for (int j = 0; j < (this._PointCount - 1); ++j)
                    {
                        int cj = (stix + j) % _PointCount;
                        int nj = (stix + j + 1) % _PointCount;
                        if (cj > valueCount)
                            continue;
                        int y1 = h - (int)((wp.Values[cj] - wp.Min) * k) - 4;
                        int y2 = h - (int)((wp.Values[nj] - wp.Min) * k) - 4;
                        if (y1 < 0)
                            y1 = 0;
                        if (y1 > h)
                            y1 = h;

                        if (y2 < 0)
                            y2 = 0;
                        if (y2 > h)
                            y2 = h;

                        g.DrawLine(wp.Pen, xLocations[j], y1, xLocations[j + 1], y2);
                    }
                }


                // 位置线
                if (_ShowValueLine && _ValueLineIndex>=0)
                {
                    g.DrawLine(_ValueLinePen, xLocations[_ValueLineIndex], 0, xLocations[_ValueLineIndex], h);
                    int ix = (valIndex + _ValueLineIndex) % _PointCount;
                    for (int j = 0; j < _WaveCount; ++j)
                    {
                        WaveProperty wp = _Waves[j];
                        wp.CurVal = wp.Values[ix];
                    }
                }

                // 标注
                if (_ShowLegend)
                {
                    int titleh = 20;
                    int titlel = 20;
                    for (int i = 0; i < _WaveCount; ++i)
                    {
                        WaveProperty wp = _Waves[i];
                        if (wp.ShowTitle)
                        {
                            g.DrawLine(wp.Pen, 0, titleh * i + titleh / 2, titlel, titleh * i + titleh / 2);
                            g.DrawString(wp.GetTitle(), new Font(FontFamily.Families[0], titleh / 2), wp.Brush, new Point(titlel, titleh * i));
                        }
                    }
                }
            }
            else
            {
                // 没有波形事的图案
                SizeF size = g.MeasureString(AU_str, AU_font);
                Font font = new Font(FontFamily.Families[0], 24);
                String s = "MultiWaveView";
                SizeF sz = g.MeasureString(s, font);
                g.DrawString(s, font, Brushes.Red, new PointF((w - sz.Width) / 2, (h - sz.Height) / 2));
            }
        }

        private void MultiWaveView_SizeChanged(object sender, EventArgs e)
        {
            ReadyXLocation();
            this.Invalidate();
        }


        private void ReadyWaves()
        {
            for (int i = 0; i < _WaveCount; ++i)
            {
                WaveProperty wp = _Waves[i];
                wp.Values = new double[_PointCount];
                for (int j = 0; j < this._PointCount; ++j)
                {
                    wp.Values[j] = (wp.Max - wp.Min) / _WaveCount * (i + 0.5);
                }
            }
            valIndex = _PointCount-1;
            valueCount = 0;
        }


        private void ReadyXLocation()
        {
            double kx = (double)this.Width / (double)(this._PointCount - 1);
            this.xLocations = new int[this._PointCount];
            for (int i = 0; i < this._PointCount; ++i)
            {
                this.xLocations[i] = (int)(i * kx);
            }
        }

        private int Spoint2Index(float x)
        {
            double kx = (double)this.Width / (double)(this._PointCount - 1);
            int ix = kx == 0 ? 0 : (int)(x / kx);
            return ix < 0 ? 0 : ix >= PointCount ? PointCount - 1 : ix;
        }

        private void MultiWaveView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_ShowValueLine || (!_ValueLineWithMouse && !_MouseLeftDown))
                return;

            ValueLineTo(e.X);
        }

        private void ValueLineTo(float x)
        {
            _ValueLineIndex = Spoint2Index(x);
            Invalidate();
        }


        private bool _MouseLeftDown = false;
        private void MultiWaveView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _MouseLeftDown = true;
                if (_ShowValueLine)
                    ValueLineTo(e.X);
            }
        }

        private void MultiWaveView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _MouseLeftDown = false;
            }
        }
    }


    /// <summary>
    /// 波形属性
    /// </summary>
    public class WaveProperty
    {
        private Pen _Pen = new Pen(Color.Red);
        
        public Pen Pen
        {
            get
            {
                return _Pen;
            }
        }

        private Brush _Brush = Brushes.Red;
        
        public Brush Brush
        {
            get
            {
                return _Brush;
            }
        }

        private Color _Color = Color.Red;
        [Category("外观"), Description("波形颜色")]
        public Color Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                _Brush = new SolidBrush(_Color);
                _Pen = new Pen(_Brush);
            }
        }

        private String _Name = "波形";
        [Category("外观"), Description("波形名称"), DefaultValue("波形")]
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private bool _Enable = true;
        [Category("外观"), Description("是否显示该条波形"), DefaultValue(true)]
        public bool Enable
        {
            get
            {
                return _Enable;
            }
            set
            {
                _Enable = value;
            }
        }

        private bool _ShowTitle = true;
        [Category("外观"), Description("是否显示波形标题"), DefaultValue(true)]
        public bool ShowTitle
        {
            get
            {
                return _ShowTitle;
            }
            set
            {
                _ShowTitle = value;
            }
        }


        private double _Max = 1;
        private double _Min = 0;
        private bool _AutoMaxMin = true;
        [Category("数据"), Description("该条波形的最大值"), DefaultValue(1)]
        public Double Max
        {
            get
            {
                return _Max;
            }
            set
            {
                _Max = value;
            }
        }


        [Category("数据"), Description("该条波形的最小值"), DefaultValue(1)]
        public Double Min
        {
            get
            {
                return _Min;
            }
            set
            {
                _Min = value;
            }
        }

        
        [Category("数据"), Description("自动更新极值"), DefaultValue(1)]
        public bool AutoUpdateMaxMin
        {
            get
            {
                return _AutoMaxMin;
            }
            set
            {
                _AutoMaxMin = value;
            }
        }

        private String _Format = "{0} {3:#0.00}/{2:#0.00}~{1:#0.00}";
        [Category("外观"), Description("标题格式,{0}:名称 {1}:最大值 {2}:最小值 {3}:当前值"), DefaultValue(null)]
        public String Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }


        public double[] Values = null;
        public double CurVal = 0;


        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns>返回波形标题</returns>
        public String GetTitle()
        {
            return String.Format(_Format, Name, Max, Min, CurVal);
        }
    }
}
