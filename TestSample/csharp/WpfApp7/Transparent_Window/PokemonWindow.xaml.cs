namespace Transparent_Window
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// PokemonWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PokemonWindow : Window
    {
        public PokemonWindow()
        {
            InitializeComponent();
            this.TargetUpdated += this.PokemonWindow_TargetUpdated;
        }

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);
        //    this.SetWindowExTransparent();
        //}

        private void PokemonWindow_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if(this.Topmost)
            {
                this.Activate();
            }
        }

        public void SetPokemon(PokemonType type)
        {
            ImageHelper.SetImageForImage(this.xPokemon, $"/Transparent_Window;component/Images/{type.ToString().Remove(0, 1)}.png", false, 390);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            this.DragMove();
        }

        public void SetWindowExTransparent()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        public void Dispose()
        {
            this.TargetUpdated -= this.PokemonWindow_TargetUpdated;
        }
    }
}
