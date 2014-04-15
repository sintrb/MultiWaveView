using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MultiWaveViewDemo
{
    public partial class FormDemo : Form
    {
        double a = 0;

        public FormDemo()
        {
            InitializeComponent();
        }

        private void tmRefresh_Tick(object sender, EventArgs e)
        {
            a += 0.01;
            double v1 = Math.Sin(a);
            double v2 = Math.Cos(a);
            double v3 = Math.Tan(a);
            mwvWaves.Values = new double[]{v1,v2,v3};
        }
    }
}