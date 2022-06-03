namespace WpfApp13
{
    public interface IView
    {
        object DataContext { get; set; }

        void Show();

        bool? ShowDialog();

        void Close();
    }

    public interface IMainView : IView
    {
        public bool? ShowPopupWindow();
    }
}
