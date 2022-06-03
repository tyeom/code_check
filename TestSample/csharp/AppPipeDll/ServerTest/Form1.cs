using AppPipe;
using AppPipe.PipeProc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerTest
{
    public partial class Form1 : Form
    {
        private PipeProvider _pipeProvider = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string pipeName = @"\\.\test\Kiosk";
            _pipeProvider = AppPipe.PipeFactory.CreatePipe(PipeFactory.PipeType.Server, new PipeServer());
            _pipeProvider.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageData messageData = new MessageData();
            messageData.Message = "server Test";

            _pipeProvider.SendMessage(messageData);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pipeProvider.Close();
        }
    }

    public class PipeServer : MessagingService
    {
        public override void MessageReceived(PipeProvider sender, RemoteMessage remoteMessage)
        {
            if (remoteMessage == null || remoteMessage.MessageData == null)
            {
                MessageBox.Show("null");
            }
            else
            {
                MessageBox.Show(remoteMessage.MessageData.Message);
            }
        }
    }
}
