namespace SimpleMVVMWpf.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using SimpleMVVMWpf.Base;
    using SimpleMVVMWpf.Common;
    using SimpleMVVMWpf.Models;

    public class DPageViewModel : BaseViewModel
    {
        private ObservableCollection<ChatModel> _chatModelList;

        public DPageViewModel()
        {
            ChatModelList = new ObservableCollection<ChatModel>();
        }

        public ObservableCollection<ChatModel> ChatModelList
        {
            get => _chatModelList;
            set
            {
                if(_chatModelList != value)
                {
                    _chatModelList = value;
                    OnPropertyChanged();
                }
            }
        }

        private DelegateCommand<String> _leftCommand;
        public ICommand LeftCommand
        {
            get
            {
                return _leftCommand ??
                    (_leftCommand = new DelegateCommand<String>(
                        param => this.ExecuteLeftCommand(param),
                        null));
            }
        }

        private DelegateCommand<String> _rightCommand;
        public ICommand RightCommand
        {
            get
            {
                return _rightCommand ??
                    (_rightCommand = new DelegateCommand<String>(
                        param => this.ExecuteRightCommand(param),
                        null));
            }
        }

        private void ExecuteLeftCommand(string param)
        {
            ChatModel.EChatType chatType = ChatModel.EChatType.NONE;
            if(Enum.TryParse(param, out chatType)) {
                ChatModel chatModel = new ChatModel()
                {
                    ChatType = chatType,
                    // 너꺼
                    MINE = false
                };

                if(chatType == ChatModel.EChatType.Normal)
                {
                    chatModel.msg = "너꺼 테스트 메세지";
                }
                else if(chatType == ChatModel.EChatType.Photo)
                {
                    chatModel.msg = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
                }

                ChatModelList.Add(chatModel);
            }
        }

        private void ExecuteRightCommand(string param)
        {
            ChatModel.EChatType chatType = ChatModel.EChatType.NONE;
            if (Enum.TryParse(param, out chatType))
            {
                ChatModel chatModel = new ChatModel()
                {
                    ChatType = chatType,
                    // 내꺼
                    MINE = true
                };

                if (chatType == ChatModel.EChatType.Normal)
                {
                    chatModel.msg = "내꺼 테스트 메세지";
                }
                else if (chatType == ChatModel.EChatType.Photo)
                {
                    chatModel.msg = "https://pds.joins.com/news/component/betanews/201811/28/6934b11a.jpg";
                }

                ChatModelList.Add(chatModel);
            }
        }
    }
}
