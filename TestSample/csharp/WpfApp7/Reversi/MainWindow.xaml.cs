namespace Reversi
{
    using Microsoft.Win32;
    using Play;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReversiProc _reversiProc;
        private DispatcherTimer _gameTimer;
        private IPlay _player1Instance;
        private IPlay _player2Instance;
        private CancellationTokenSource _playerTimeOutCancelToken = null;
        private bool turn_TimeOverChecking = false;

        public MainWindow()
        {
            InitializeComponent();

            _reversiProc = new ReversiProc();
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            _gameTimer.Tick += this.GameTimer_Tick;

            this.Loaded += this.MainWindow_Loaded;
        }

        public int Turn
        {
            get;
            set;
        } = 1;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.xReversiArray.ItemsSource = _reversiProc.BoardArray;

            _reversiProc.CalcScores();
            this.xBlackScore.Text = _reversiProc.BlackScore.ToString();
            this.xWhiteScore.Text = _reversiProc.WhiteScore.ToString(); 
        }

        private void XStartGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 플레이어 1 dll 로드 [흑]
                OpenFileDialog fileOpenDig_player1 = new OpenFileDialog();
                fileOpenDig_player1.Title = "플레이어1 [흑] dll선택";
                if (fileOpenDig_player1.ShowDialog().Value)
                {
                    Assembly player1 = Assembly.LoadFile(fileOpenDig_player1.FileName);
                    var fullName = player1.DefinedTypes.ToList()[0].FullName;
                    _player1Instance = (IPlay)player1.CreateInstance(fullName);
                    _player1Instance.Player = 1;
                    this.xPlayer1NickName.Text = _player1Instance.PlayerNickName;
                }

                // 플레이어 2 dll 로드 [백]
                OpenFileDialog fileOpenDig_player2 = new OpenFileDialog();
                fileOpenDig_player2.Title = "플레이어2 [백] dll선택";
                if (fileOpenDig_player2.ShowDialog().Value)
                {
                    Assembly player2 = Assembly.LoadFile(fileOpenDig_player2.FileName);
                    var fullName = player2.DefinedTypes.ToList()[0].FullName;
                    _player2Instance = (IPlay)player2.CreateInstance(fullName);
                    _player2Instance.Player = 2;
                    this.xPlayer2NickName.Text = _player2Instance.PlayerNickName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if(_player1Instance != null &&
                _player2Instance != null)
                _gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _gameTimer.Stop();

            int row = 0, col = 0;
            if (Turn == 1)
            {
                if (turn_TimeOverChecking == false)
                {
                    _playerTimeOutCancelToken = new CancellationTokenSource();
                    Task.Delay(TimeSpan.FromSeconds(10), _playerTimeOutCancelToken.Token)
                        .ContinueWith((t) =>
                        {
                            if (_playerTimeOutCancelToken.Token.IsCancellationRequested == false)
                            {
                                _gameTimer.Stop();
                                // 시간초과
                                MessageBox.Show($"{_player1Instance.PlayerNickName} 시간 초과 입니다.");
                                return;
                            }
                        });
                    turn_TimeOverChecking = true;
                }

                // 플레이어 Move메서드 호출
                _player1Instance.Move(out row, out col, _reversiProc.BoardArrayByResponse);
            }
            else if (Turn == 2)
            {
                if (turn_TimeOverChecking == false)
                {
                    _playerTimeOutCancelToken = new CancellationTokenSource();
                    Task.Delay(TimeSpan.FromSeconds(10), _playerTimeOutCancelToken.Token)
                        .ContinueWith((t) =>
                        {
                            if (_playerTimeOutCancelToken.Token.IsCancellationRequested == false)
                            {
                                _gameTimer.Stop();
                                // 시간초과
                                MessageBox.Show($"{_player2Instance.PlayerNickName} 시간 초과 입니다.");
                                return;
                            }
                        });
                    turn_TimeOverChecking = true;
                }

                // 플레이어 Move메서드 호출
                _player2Instance.Move(out row, out col, _reversiProc.BoardArrayByResponse);
            }

            // 바로 다음 플레이어에게 턴 전환
            if(row == -1 && col == -1)
            {
                turn_TimeOverChecking = false;
                _playerTimeOutCancelToken.Cancel();

                if (Turn == 1)
                {
                    Turn = 2;
                }
                else if (Turn == 2)
                {
                    Turn = 1;
                }

                this.xTurn.Text = (Turn == 1 ? "흑" : "백");
                _gameTimer.Start();
                return;
            }

            bool? mode = _reversiProc.MakeMove(Turn, row, col);
            this.xBlackScore.Text = _reversiProc.BlackScore.ToString();
            this.xWhiteScore.Text = _reversiProc.WhiteScore.ToString();
            if (mode == null)
            {
                turn_TimeOverChecking = false;
                _playerTimeOutCancelToken.Cancel();

                MessageBox.Show("게임 오버!");
                return;
            }
            else if (mode.Value)
            {
                turn_TimeOverChecking = false;
                _playerTimeOutCancelToken.Cancel();

                if (Turn == 1)
                {
                    Turn = 2;
                }
                else if (Turn == 2)
                {
                    Turn = 1;
                }

                this.xTurn.Text = (Turn == 1 ? "흑" : "백");
            }
            else
            {
                // 이동할 수 없음.
                _gameTimer.Start();
            }

            _gameTimer.Start();
        }

        private void xReversiCell_Click(object sender, RoutedEventArgs e)
        {
            return;


            Button btn = (Button)sender;
            ReversiCellModel cellModel = (ReversiCellModel)btn.DataContext;
            int y = _reversiProc.BoardArray.FindIndex( p => p.Contains(cellModel));
            int x = _reversiProc.BoardArray[y].FindIndex(p => p == cellModel);

            bool? mode = _reversiProc.MakeMove(Turn, x, y);
            if(mode == null)
            {
                MessageBox.Show("게임 오버!");
            }
            else if (mode.Value)
            {
                if (Turn == 1)
                {
                    Turn = 2;
                }
                else if (Turn == 2)
                {
                    Turn = 1;
                }

                this.xBlackScore.Text = _reversiProc.BlackScore.ToString();
                this.xWhiteScore.Text = _reversiProc.WhiteScore.ToString();

                this.xTurn.Text = (Turn == 1 ? "흑" : "백");
            }
            else
            {
                // 이동할 수 없음.
            }
        }
    }
}
