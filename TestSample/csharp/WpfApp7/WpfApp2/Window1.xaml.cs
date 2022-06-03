namespace WpfApp2
{
    using System;
    using System.Windows;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private BitmapImage Capture(System.Drawing.Point mousePoint)
        {
            Bitmap bitmap = new Bitmap(50, 50, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int currPosXOnCenter = (int)mousePoint.X - 25;
                int currPosyOnCenter = (int)mousePoint.Y - 25;
                g.CopyFromScreen(currPosXOnCenter <= 0 ? 0 : currPosXOnCenter, currPosyOnCenter <= 0 ? 0 : currPosyOnCenter, 0, 0, bitmap.Size);
                //g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

                using (MemoryStream memory = new MemoryStream())
                {
                    bitmap.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task captureTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    this.xImg.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.xImg.Source = this.Capture(System.Windows.Forms.Cursor.Position);
                    }));
                    await Task.Delay(50);
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
