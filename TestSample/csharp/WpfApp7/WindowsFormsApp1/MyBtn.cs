using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class MyBtn : Button
    {
        protected override void OnCreateControl()
        {
            base.BackColor = Color.FromArgb(255, 100, 50);
            base.OnCreateControl();
        }
    }
}
