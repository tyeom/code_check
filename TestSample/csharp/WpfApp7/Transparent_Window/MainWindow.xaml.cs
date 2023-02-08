namespace Transparent_Window
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.SetNotification();
        }

        private void SetNotification()
        {
            // NotifyIcon 생성
            NotifyIcon ni = new NotifyIcon();
            Stream iconStream = App.GetResourceStream(new Uri("pack://application:,,,/Transparent_Window;component/ex.ico")).Stream;
            ni.Icon = new System.Drawing.Icon(iconStream);
            ni.Visible = true;
            ni.Text = "포켓몬 보기";
            ni.ContextMenu = this.SetContextMenu(ni);
        }

        private System.Windows.Forms.ContextMenu SetContextMenu(NotifyIcon ni)
        {
            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();

            // 첫번째 메뉴 '포켓몬 추가' MenuItem 생성
            System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem();
            menu1.Text = "포켓몬 추가";

            // '포켓몬 추가' > '리자드'
            System.Windows.Forms.MenuItem p000501 = new System.Windows.Forms.MenuItem();
            p000501.Text = "리자드";
            p000501.Click += (_, __) =>
            {
                PokemonWindow pokemonWindow = new PokemonWindow();
                pokemonWindow.SetPokemon(PokemonType.p000501);
                pokemonWindow.Show();
            };
            // '포켓몬 추가' > '피카츄'
            System.Windows.Forms.MenuItem p002501 = new System.Windows.Forms.MenuItem();
            p002501.Text = "피카츄";
            p002501.Click += (_, __) =>
            {
                PokemonWindow pokemonWindow = new PokemonWindow();
                pokemonWindow.SetPokemon(PokemonType.p002501);
                pokemonWindow.Show();
            };
            // '포켓몬 추가' > '푸린'
            System.Windows.Forms.MenuItem p003901 = new System.Windows.Forms.MenuItem();
            p003901.Text = "푸린";
            p003901.Click += (_, __) =>
            {
                PokemonWindow pokemonWindow = new PokemonWindow();
                pokemonWindow.SetPokemon(PokemonType.p003901);
                pokemonWindow.Show();
            };
            // '포켓몬 추가' > '이상해꽃'
            System.Windows.Forms.MenuItem p000301 = new System.Windows.Forms.MenuItem();
            p000301.Text = "이상해꽃";
            p000301.Click += (_, __) =>
            {
                PokemonWindow pokemonWindow = new PokemonWindow();
                pokemonWindow.SetPokemon(PokemonType.p000301);
                pokemonWindow.Show();
            };

            menu1.MenuItems.Add(p000501);
            menu1.MenuItems.Add(p002501);
            menu1.MenuItems.Add(p003901);
            menu1.MenuItems.Add(p000301);

            // 두번째 메뉴 '항상 위에 보이기' MenuItem 생성
            System.Windows.Forms.MenuItem menu2 = new System.Windows.Forms.MenuItem();
            menu2.Text = "항상 위에 보이기";
            menu2.Checked = true;
            menu2.Click += (_, __) =>
            {
                AppSetting.Instance.IsTopMost = !menu2.Checked;
                menu2.Checked = !menu2.Checked;
            };

            // 세번째 메뉴 '마우스 이벤트 허용' MenuItem 생성
            System.Windows.Forms.MenuItem menu3 = new System.Windows.Forms.MenuItem();
            menu3.Text = "마우스 이벤트 허용";
            menu3.Checked = false;
            menu3.Click += (_, __) =>
            {
                AppSetting.Instance.IsMouseEventMessagePass = !menu3.Checked;
                menu3.Checked = !menu3.Checked;
                this.SetWindowExTransparent();
            };

            // 네번째 메뉴 '모든 스티커 닫기' MenuItem 생성
            System.Windows.Forms.MenuItem menu4 = new System.Windows.Forms.MenuItem();
            menu4.Text = "모든 스티커 닫기";
            menu4.Click += (_, __) =>
            {
                this.CloseAll();
            };

            // 네번째 메뉴 '종료' MenuItem 생성
            System.Windows.Forms.MenuItem menu5 = new System.Windows.Forms.MenuItem();
            menu5.Text = "종료";
            menu5.Click += (_, __) =>
            {
                this.Close();
            };

            menu.MenuItems.Add(menu1);
            menu.MenuItems.Add(menu2);
            menu.MenuItems.Add(menu3);
            menu.MenuItems.Add(menu4);
            menu.MenuItems.Add(menu5);

            return menu;
        }

        private void SetWindowExTransparent()
        {
            foreach (var win in App.Current.Windows)
            {
                if (win is PokemonWindow pokemonWin)
                {
                    pokemonWin.SetWindowExTransparent();
                }
            }
        }

        private void CloseAll()
        {
            foreach(var win in App.Current.Windows)
            {
                if(win is PokemonWindow pokemonWin)
                {
                    pokemonWin.Dispose();
                    pokemonWin.Close();
                }
            }
        }
    }
}
