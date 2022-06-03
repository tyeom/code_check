namespace WpfApp13
{
    using Microsoft.Toolkit.Mvvm.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Popup1ViewModel : ViewModelBase
    {
        private IList<MenuModel> _menuList;

        public Popup1ViewModel()
        {
            // 초기화
            MenuList = new List<MenuModel>(
                Enumerable.Range(0, 10)
                          .Select(p => new MenuModel() { Id = p, Content = $"메뉴 - {p}" })
                );
        }
        
        public IList<MenuModel> MenuList
        {
            get => _menuList;
            set => this.SetProperty(ref _menuList, value);
        }

        private RelayCommand<MenuModel> _menuCommand;

        public RelayCommand<MenuModel> MenuCommand
        {
            get
            {
                return _menuCommand ??
                    (_menuCommand = new RelayCommand<MenuModel>(
                        (menu) =>
                        {
                            Console.Write(menu.Id);
                        },
                        null));
            }
        }
    }

    public class MenuModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
