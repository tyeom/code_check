using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            this.vScrollBar1.Maximum = this.flowLayoutPanel1.VerticalScroll.Maximum;

            //progressBarControl1.Invoke(
            //    (MethodInvoker)delegate
            //    {
            //        progressBarControl1.Maximum = 100;
            //    });

            //        Task.Run(() =>
            //{
            //    for (int i = 1; i < 100; i++)
            //    {
            //        progressBarControl1.Invoke((MethodInvoker)delegate
            //        {
            //            progressBarControl1.Value = i;
            //        });
            //        Thread.Sleep(1000);
            //    }
            //});
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.flowLayoutPanel1.VerticalScroll.Value = this.vScrollBar1.Value;
        }
    }
}
