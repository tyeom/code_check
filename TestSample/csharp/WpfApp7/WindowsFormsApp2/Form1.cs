using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public static class CoreExtCfg
    {
        public static string GetAttribute(this Form f)
        {
            return "a";
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string a = this.GetAttribute();
        }

        int n1;
        string a;
        Person p1;

        private void QQ(Person arg)
        {
            //arg.Name = "ddddddddd";
            arg = null;
        }
        

        private List<decimal> _numInputList = new List<decimal>();
        private void button1_Click(object sender, EventArgs e)
        {
            Person tmp = new Person();
            tmp.Name = "aaaaa";
            this.QQ(tmp);

            string vv = "aaa";
            ref string vv2 = ref vv;

            vv = "bbbbbbb";

            int n2 = n1;
            n1 = 999;

            
            string b = a;
            a = new string(new char[] { 'a', 'e', 'd' });

            
            Person p2 = p1;
            p1 = new Person();


            decimal sum = _numInputList.Sum();
            this.label1.Text = $"추가된 총 합은 {sum}입니다!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _numInputList.Add(this.numericUpDown1.Value);
        }
    }
}
