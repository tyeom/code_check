namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    internal class MenuModel : INotifyPropertyChanged
    {
        private string _menuName;
        private int _price;
        private string _desc;

        public string MenuName
        {
            get => _menuName;
            set
            {
                if(_menuName != value)
                {
                    _menuName = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Desc
        {
            get => _desc;
            set
            {
                if (_desc != value)
                {
                    _desc = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task<List<MenuModel>> GetDataAsync()
        {
            await Task.Delay(5000);

            var task = Task.Run(() =>
            {
                // 가라 데이터
                List<MenuModel> menuList = new List<MenuModel>();
                MenuModel menuModel1 = new MenuModel() { MenuName = "아메리카노", Price = 100000, Desc = "고급진 아메리카노" };
                MenuModel menuModel2 = new MenuModel() { MenuName = "바닐라라떼", Price = 150000, Desc = "달콤한 바닐라라떼" };
                MenuModel menuModel3 = new MenuModel() { MenuName = "고구마라떼", Price = 20000, Desc = "고구마 고구마라떼" };
                MenuModel menuModel4 = new MenuModel() { MenuName = "악마민트", Price = 11000, Desc = "민트는 악마다" };
                menuList.Add(menuModel1);
                menuList.Add(menuModel2);
                menuList.Add(menuModel3);
                menuList.Add(menuModel4);

                return menuList;
            });

            return await task;
        }

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChange Implementation
    }
}
