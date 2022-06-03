using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChatStream _chatStream = null;
        public MainWindow()
        {
            InitializeComponent();

            ChatList = new ObservableCollection<string>();
            this.DataContext = this;
            this.Loaded += MainWindow_Loaded;
        }

        public ObservableCollection<String> ChatList
        {
            get;set;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _chatStream = new ChatStream();
            await foreach (var chat in _chatStream.GetChatAsync())
            {
                ChatList.Add(chat);
            }
        }

        public async void A()
        {
            await Task.Delay(1000);
        }

        public void B()
        {
            System.Threading.Thread.Sleep(1000);
        }

        private async void xSendBtn_Click(object sender, RoutedEventArgs e)
        {
            _chatStream.Send(this.xChatTxt.Text);
            this.xChatTxt.Text = String.Empty;
        }
    }

    public class ChatStream
    {
        private CancellationTokenSource _cancel;
        private readonly SemaphoreSlim _sem;
        private ConcurrentQueue<String> _chatCollection;

        public ChatStream()
        {
            _cancel = new CancellationTokenSource();
            _sem = new SemaphoreSlim(0);
            _chatCollection = new ConcurrentQueue<String>();
        }

        public async IAsyncEnumerable<string> GetChatAsync()
        {
            while (_cancel.IsCancellationRequested == false)
            {
                await _sem.WaitAsync();

                string chat;
                if (_chatCollection.TryDequeue(out chat))
                {
                    yield return chat;
                }
            }
        }

        public void Send(string chat)
        {
            _chatCollection.Enqueue(chat);
            _sem.Release(1);
        }
    }
}
