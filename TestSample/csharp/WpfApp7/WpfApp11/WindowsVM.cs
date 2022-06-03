using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp11
{
    public class WindowsVM : INotifyPropertyChanged
    {
        public FolderCommand folderCommand { get; set; }

        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged("FolderPath");
            }
        }
        private int _statusFileSize;
        public int StatusFileSize
        {
            get { return _statusFileSize; }
            set
            {
                _statusFileSize = value;
                OnPropertyChanged();
            }
        }

        public WindowsVM()
        {
            folderCommand = new FolderCommand(this);
        }

        
        public void GetFolderPath()
        {
            //Task.Run(() =>
            //{
                for (long i = 0; i < 1000000; i++)
                {
                    double dIndex = (double)i;
                    double dTotal = (double)1000;
                    double dProgressPercentage = (dIndex / dTotal);
                    int iProgressPercentage = (int)(dProgressPercentage * 100);

                    StatusFileSize = (int)(dProgressPercentage * 100);

                //System.Windows.Threading.Dispatcher dispatcher = null;
                //if (System.Windows.Application.Current != null)
                //    dispatcher = System.Windows.Application.Current.Dispatcher;

                //dispatcher.Invoke(
                //    (System.Threading.ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                //System.Windows.Application.Current.Dispatcher.Invoke(
                //    System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                //    new Action(delegate { }));
            }
            //});
            


            //string orignalFilePath = file.OriginUri;
            //string destinationFilePath = Path.Combine(path, file.FileName);

            //Uri url = new Uri(orignalFilePath);
            //System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            //System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            //response.Close();
            //Int64 iSize = response.ContentLength;
            //Int64 iRunningByteTotal = 0;

            //using (System.Net.WebClient client = new System.Net.WebClient())
            //{
            //    using (System.IO.Stream streamRemote = client.OpenRead(new Uri(orignalFilePath)))
            //    {
            //        using (Stream streamLocal = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            //        {
            //            int iByteSize = 0;
            //            byte[] byteBuffer = new byte[iSize];
            //            while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
            //            {
            //                streamLocal.Write(byteBuffer, 0, iByteSize);
            //                iRunningByteTotal += iByteSize;

            //                double dIndex = (double)(iRunningByteTotal);
            //                double dTotal = (double)byteBuffer.Length;
            //                double dProgressPercentage = (dIndex / dTotal);
            //                int iProgressPercentage = (int)(dProgressPercentage * 100);

            //                StatusFileSize = (int)(dProgressPercentage * 100);

            //                Console.WriteLine(iProgressPercentage);
            //            }
            //            streamLocal.Close();
            //        }
            //        streamRemote.Close();
            //    }
            //}
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
