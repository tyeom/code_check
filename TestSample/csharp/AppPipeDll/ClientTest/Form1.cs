using AppPipe;
using AppPipe.PipeProc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTest
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
            _pipeProvider = AppPipe.PipeFactory.CreatePipe(PipeFactory.PipeType.Client, new PipeClient());
            _pipeProvider.Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageData messageData = new MessageData();
            messageData.Message = "client Test";

            _pipeProvider.SendMessage(messageData);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pipeProvider.Close();
        }
    }

    public class PipeClient : MessagingService
    {
        public override void MessageReceived(PipeProvider sender, RemoteMessage remoteMessage)
        {
            //
        }
    }
}
