namespace WindowsFormsApp1
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class DataProcCompletedEventArgs : AsyncCompletedEventArgs
    {
        public DataProcCompletedEventArgs(int value, object result, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
        {
            Value = value;
            Result = result;
        }

        public int Value { get; private set; }
        public object Result { get; private set; }
    }

    public class DataProcess
    {
        public delegate void DataProcCompletedEventHandler(object sender, DataProcCompletedEventArgs e);

        public event DataProcCompletedEventHandler DataProcCompleted;
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private SendOrPostCallback completed;

        private SendOrPostCallback progress;

        private HybridDictionary userStateToLifetime = new HybridDictionary();

        public DataProcess()
        {
            completed = this.ProcessCompleted;
            progress = this.ReportProgress;
        }

        private void ProcessCompleted(object operationState)
        {
            DataProcCompletedEventArgs e =
                operationState as DataProcCompletedEventArgs;

            DataProcCompleted(this, e);
        }

        private void ReportProgress(object operationState)
        {
            ProgressChangedEventArgs e =
                operationState as ProgressChangedEventArgs;

            ProgressChanged(this, e);
        }

        public void ProcessAsync(int value, object taskId)
        {
            AsyncOperation asyncOp =
                AsyncOperationManager.CreateOperation(taskId);

            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    throw new ArgumentException("동일 Task ID가 이미 존재함", "taskId");
                }

                userStateToLifetime[taskId] = asyncOp;
            }

            Action<int, AsyncOperation> startDelegate = this.ProcessWorker;
            // DataProcCompleted 및 ProgressChanged 이벤트 알림으로 통보가 되어 콜백 파라메터는 null로 처리 합니다.
            startDelegate.BeginInvoke(value, asyncOp, null, null);
        }

        public void CancelAsync(object taskId)
        {
            AsyncOperation asyncOp = userStateToLifetime[taskId] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(taskId);
                }
            }
        }

        private void ProcessWorker(int value, AsyncOperation asyncOp)
        {
            bool result = false;
            Exception exception = null;

            if (this.TaskCanceled(asyncOp.UserSuppliedState) == false)
            {
                try
                {
                    result = this.Process(value, asyncOp);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            this.CompletionMethod(
                value,
                result,
                exception,
                this.TaskCanceled(asyncOp.UserSuppliedState),
                asyncOp);
        }

        private bool TaskCanceled(object taskId)
        {
            return (userStateToLifetime[taskId] == null);
        }

        /// <summary>
        /// 비동기 호출 작업
        /// </summary>
        /// <param name="value"></param>
        /// <param name="asyncOp"></param>
        /// <returns></returns>
        private bool Process(int value, AsyncOperation asyncOp)
        {
            ProgressChangedEventArgs e = null;

            for (int i = 0; i < value; i++)
            {
                if(this.TaskCanceled(asyncOp.UserSuppliedState))
                {
                    return false;
                }

                System.Threading.Thread.Sleep(1000);
                e = new ProgressChangedEventArgs(i, asyncOp.UserSuppliedState);
                asyncOp.Post(progress, e);
            }

            return true;
        }

        private void CompletionMethod(
            int value,
            bool result,
            Exception exception,
            bool canceled,
            AsyncOperation asyncOp)

        {
            if (!canceled)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }

            DataProcCompletedEventArgs e =
                new DataProcCompletedEventArgs(
                value,
                result,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            asyncOp.PostOperationCompleted(completed, e);
        }
    }
}
