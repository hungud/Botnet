using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace Botnet
{
    public partial class MainForm : Form
    {
        private NetworkController Controller;
        private delegate void ReceiveMessageCallBack(string data);
        public MainForm()
        {
            InitializeComponent();
            BananaGuy = new MascotController(ref MrB);
            //Socket soc = new Socket(SocketType.Raw, ProtocolType.IPv4);
            //Socket test = new Socket(SocketType.Raw,ProtocolType.);
            //test.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.HeaderIncluded, true);
            //soc.SendTo(new byte[] { 34,234}, new IPEndPoint(new IPAddress(new byte[] { 192, 168, 10, 9 }), 80));
            //IPv4Packet Packet = new IPv4Packet();
            //Packet.DestIp = new Address(213,180,193,3);
            //Packet.SourceIp = new Address(192, 168, 0, 3);
            //Packet.TTL = 128;
            //Packet.ID = 32344;
            ////Packet.setFlagBit(2, true);
            //Packet.Payload = new ICMPv4Packet();
            //Packet.Protocol = (byte)ProtocolID.Icmpv4;
            //Packet.calculateOptions();
            //byte[] ser = Packet.Serialize();
           

        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            BananaGuy.setState((int)MascotController.states.salutation);
            Cursor.Current = Cursors.WaitCursor;
            ControlTab.Enabled = false;
            Controller = new NetworkController(new AttackParams(), UpdateData, StatisticRespond, ChangeMode, LostConnectionHandler, 27000);
            ControlTab.Enabled = true;
            System.Threading.Thread.Sleep(400);
            if (Controller.mode)
            {
                //currentmode is master
                ControlTab.SelectTab(0);
                RefreshHostList(DeviceObserveGrid);
                //write info abot computer
                //fill daemon list
            }
            else
            {
                ControlTab.SelectTab(1);
                //write info about master
            }
            Cursor.Current = Cursors.Arrow;
        }

        private MascotController BananaGuy;


        private void MrB_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) BananaGuy.Counters(true);
            else BananaGuy.Counters(false);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (Controller != null)
            {
                BananaGuy.setState((int)MascotController.states.attacking);
                Controller.Start();
            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            if (Controller != null)
            {
                BananaGuy.setState((int)MascotController.states.pause);
                Controller.Stop();
            }
        }

        private void refreshAliesBtn_Click(object sender, EventArgs e)
        {
            BananaGuy.setState((int)MascotController.states.preparing);
            RefreshHostList(DeviceObserveGrid);
        }
        private void RefreshHostList(DataGridView Target)
        {
            //parse ip, port, status fields
            NetworkController.Daemon[] list = Controller.GetDaemonList();
            Target.RowCount = list.Length;
            Target.ColumnCount = 3;
            for (int i = 0; i < list.Length; ++i)
            {
                Target.Rows[i].Cells[0].Value = list[i].IpEndPoint.Address.ToString();
                Target.Rows[i].Cells[1].Value = list[i].IpEndPoint.Port.ToString();
                switch (list[i].state)
                {
                    case NetworkController.ControllerState.Attacking: Target.Rows[i].Cells[1].Value = "Attacking"; break;  //check do these codes properly installed 
                    case NetworkController.ControllerState.Error: Target.Rows[i].Cells[1].Value = "Error"; break;
                    case NetworkController.ControllerState.Master: Target.Rows[i].Cells[1].Value = "Master"; break;
                    case NetworkController.ControllerState.Suspending: Target.Rows[i].Cells[1].Value = "Suspending"; break;
                    case NetworkController.ControllerState.Tuning: Target.Rows[i].Cells[1].Value = "Tuning"; break;
                }
            }

        }

        private void ParamsBtn_Click(object sender, EventArgs e)
        {
            BananaGuy.setState((int)MascotController.states.preparing);
            //open config window
            //specify restricted address pool
            SettingsForm Sets = new SettingsForm(Controller.Params, Controller.BroadcastPort, SetSettings, Controller.mode);
            Sets.Show();
        }

        private void ClearLogBtn_Click(object sender, EventArgs e)
        {
            LogBox.Text = "";
        }
        private void UpdateData(string Message)
        {
            if (LogBox.InvokeRequired)
            {
                ReceiveMessageCallBack DataCallBack = new ReceiveMessageCallBack(UpdateData);
                this.Invoke(DataCallBack, new Object[] { Message });
            }
            else
            {
                LogBox.AppendText(DateTime.Now.ToString("HH:mm:ss.ffff") + ": " + Message + Environment.NewLine);
                LogBox.SelectionStart = LogBox.TextLength;
                LogBox.ScrollToCaret();
            }
        }
        private void ChangeMode(bool mode)
        {
            if (Controller.mode == true) ControlTab.SelectTab(0);
            else ControlTab.SelectTab(1);
        }

        private void TabChangingHandler(object sender, TabControlCancelEventArgs e)
        {
            //check who is before
            if (e.TabPageIndex == 0)  // if there was master mode, 
            {
                if (!Controller.mode) // and now user mode
                {
                    e.Cancel = false; //do not cancel tab changing
                }
            }
            else  //if there was user mode
            {
                if (!Controller.mode) // and now master mode
                {
                    e.Cancel = false; //do not cancel tab changing
                }
            }
            e.Cancel = true;

        }
        private void SetSettings(AttackParams Params, int altport)
        {
            if (Controller.BroadcastPort != altport) Controller.InitPort(altport);  //catch occ port exception
            Controller.InitParams(Params);

        }
        private void MasterChoosing()
        {
            //Controller.MasterEnabling();
            //here user should choose new master, controller will send him masterOn request, then receive master ack, and then we send brodcast maste idle, that elicir master search
        }


        private void LostConnectionHandler()
        {
            Controller = null;
            MessageBox.Show("Похоже что подключение к сети остутсвует, проверьте подключение и примените параметры заново ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void StatisticRespond(int am)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Controller != null)
            {
                Controller.Close();
            }
        }


    }
}
