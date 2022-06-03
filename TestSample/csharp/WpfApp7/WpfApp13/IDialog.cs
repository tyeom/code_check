namespace WpfApp13
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDialog
    {
        object DataContext { get; set; }

        void Show();

        bool? ShowDialog();

        void Close();
    }
}
