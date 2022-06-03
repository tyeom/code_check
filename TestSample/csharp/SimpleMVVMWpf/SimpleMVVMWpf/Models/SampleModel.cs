namespace SimpleMVVMWpf.Models
{
    using SimpleMVVMWpf.Base;
    using SimpleMVVMWpf.Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class SampleModel : BaseModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        private bool _isStack;
        public bool IsStack
        {
            get => _isStack;
            set
            {
                _isStack = value;
                OnPropertyChanged();
            }
        }
    }

    public class SampleModelList : BaseListModel<SampleModel>
    {
        private readonly IDBService _dbService;

        public SampleModelList(IDBService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// 데이터 요청
        /// </summary>
        public async Task FillData()
        {
            IEnumerable<SampleModel> dataList = await _dbService.GetData<SampleModel>(100);
            this.AddRange(dataList);
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public async Task<bool> SaveData(SampleModel saveModel)
        {
            return await _dbService.SaveData<SampleModel>(saveModel);
        }
    }
}
