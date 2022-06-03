using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer _winTmr;
        //private System.Windows.Threading.DispatcherTimer

        private System.Threading.Timer _thrTmr;

        public Form1()
        {
            InitializeComponent();

            _winTmr = new System.Windows.Forms.Timer();
            _winTmr.Interval = 3000;
            _winTmr.Tick += _winTmr_Tick;

            _thrTmr = new System.Threading.Timer(this.thrTmr_Tick, null, 1000, 1000);
        }

        private void _winTmr_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!!!!!!");
            for (long i = 0; i <= long.MaxValue; i ++ )
            {

            }
        }

        private void thrTmr_Tick(object state)
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!!!!!!");
            for (long i = 0; i <= long.MaxValue; i++)
            {

            }
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //_winTmr.Start();
            //_thrTmr.Change(1000, 1000);

            this.richTextBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.richTextBox3, "Rtf", true));


            ///////////////////////this.test1();

            this.listView1.View = View.Details;

            this.listView1.Columns.Add("item1", 200, HorizontalAlignment.Left);
            this.listView1.Columns.Add("item2", 200, HorizontalAlignment.Left);
            this.listView1.Columns.Add("item3", 200, HorizontalAlignment.Left);

            for (int i = 0; i < 3; i++)
            {
                ListViewItem item = new ListViewItem("item" + i);
                //item.SubItems.Add("buttonA " + i);
                //item.SubItems.Add("buttonB " + i);

                this.listView1.Items.Add(item);
            }
        }

        private bool _flag = true;

        private void test1()
        {
            Task.Factory.StartNew(() =>
            {
                while(_flag)
                {
                    Console.WriteLine("aaa");
                    Task.Delay(10000);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.BackColor == System.Drawing.Color.FromArgb(255, 0, 0))
            {

            }

            if (button1.BackColor.Equals(System.Drawing.Color.FromArgb(255, 0, 0)))
            {

            }


            _flag = false;
        }

        private void myBtn1_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
        }

        private void myBtn2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                List<byte> dataNew = new List<byte>();

                dataNew = data.ToList();
                byte[] insertByteArr = Encoding.UTF8.GetBytes(".net_dev_openChat for kakao");

                dataNew.InsertRange(100, insertByteArr);

                File.WriteAllBytes(ofd.FileName, dataNew.ToArray());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            this.richTextBox2.Text = this.richTextBox3.Text;
        }
    }
}
