namespace SimpleMVVMWpf.ViewModels
{
    using SimpleMVVMWpf.Base;
    using SimpleMVVMWpf.Common;
    using SimpleMVVMWpf.Models;
    using SimpleMVVMWpf.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Linq;

    public class CPageViewModel : BaseViewModel
    {
        private readonly IDBService _dbService;
        private ObservableCollection<PolygonModel> _polygonList;
        private bool _isDrawing = false;

        public CPageViewModel(IDBService dbService)
        {
            _dbService = dbService;
            PolygonList = new ObservableCollection<PolygonModel>();
        }

        public double MouseX
        {
            get;
            set;
        }

        public double MouseY
        {
            get;
            set;
        }

        public ObservableCollection<PolygonModel> PolygonList
        {
            get => _polygonList;
            set
            {
                _polygonList = value;
                OnPropertyChanged();
            }
        }

        private DelegateCommand<Object> _startDrawingCommand;
        public ICommand StartDrawingCommand
        {
            get
            {
                return _startDrawingCommand ??
                    (_startDrawingCommand = new DelegateCommand<Object>(
                        param => this.DrawingCommandExecute(param),
                        null));
            }
        }

        private DelegateCommand<Object> _endDrawingCommand;
        public ICommand EndDrawingCommand
        {
            get
            {
                return _endDrawingCommand ??
                    (_endDrawingCommand = new DelegateCommand<Object>(
                        param =>
                        {
                            _isDrawing = false;
                        },
                        null));
            }
        }

        private void DrawingCommandExecute(object param)
        {
            PolygonModel polygonModel = null;

            // 처음 드로잉 시작 지점
            if (_isDrawing == false)
            {
                polygonModel = new PolygonModel()
                {
                    Left = (int)MouseX,
                    Top = (int)MouseY,
                    Points = new System.Windows.Media.PointCollection()
                };

                Point startPoint = new Point(MouseX, MouseY);
                polygonModel.AddPoint(startPoint);
                PolygonList.Add(polygonModel);

                _isDrawing = true;
                return;
            }

            // 계속해서 이어서 드로잉
            if (PolygonList.Count <= 0)
                return;

            polygonModel = PolygonList.Last();
            Point mousePoint = new Point(MouseX, MouseY);
            polygonModel.AddPoint(mousePoint);
        }
    }
}
