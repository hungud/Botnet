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
        private Timer StatisticTimer;
        private delegate void ReceiveMessageCallBack(string data);
        private delegate void LabelChangeCallBack(Label target, uint data);
        public MainForm()
        {
            InitializeComponent();
            BananaGuy = new MascotController(ref MrB);
            StatisticTimer = new Timer();
            StatisticTimer.Tick += new EventHandler(getStats);
            StatisticTimer.Interval = 1000;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            BananaGuy.setState((int)MascotController.states.salutation);
            Cursor.Current = Cursors.WaitCursor;
            ControlTab.Enabled = false;
            InitController();
            Cursor.Current = Cursors.Arrow;
            StatisticTimer.Start();
        }
        private void InitController()
        {
            try
            {   //change lostconn handler to error handler
                Controller = new NetworkController(new AttackParams(), NetworkInstruments.getAnyAdaptor(), UpdateData, StatisticRespond, ChangeMode, ErrorHandler, 27000, null);
                ControlTab.Enabled = true;
                System.Threading.Thread.Sleep(400);
                if (Controller.mode)
                {
                    //currentmode is master
                    ChangeMode(true);
                    MIpEndPointlab.Text = Controller.LocalIpEndPoint.ToString();
                    //write info abot computer
                    //fill daemon list
                }
                else
                {
                    ChangeMode(false);
                    //write info about master
                }
                ConnectBtn.Enabled = false;
            }
            catch (Exception err)
            {
                UpdateData("An Error occured");
               
            }
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
 //what will happen when user push once more?
            }
            else { MessageBox.Show("Отсутвует подключение к сети, проверьте подкллючение и переподключитесь", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            if (Controller != null)
            {
                BananaGuy.setState((int)MascotController.states.pause);
                Controller.Stop();

            }
            else { MessageBox.Show("Отсутвует подключение к сети, проверьте подкллючение и переподключитесь", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void refreshAliesBtn_Click(object sender, EventArgs e)
        {
            BananaGuy.setState((int)MascotController.states.preparing);
            RefreshHostList(DeviceObserveGrid);
        }
        private void RefreshHostList(DataGridView Target)
        {
            //parse ip, port, status fields
            //NetworkController.Daemon[] list = Controller.GetDaemonList();
            string[] list = Controller.GetDaemonList();
            Target.RowCount = list.Length;
            Target.ColumnCount = 2;
            for (int i = 0; i < list.Length; ++i)
            {
                string[] info = list[i].Split(' ');
                Target.Rows[i].Cells[0].Value = info[0];
                Target.Rows[i].Cells[1].Value = info[1];
                //switch ((NetworkController.ControllerState)Convert.ToByte(info[3]))
                //{
                //    case NetworkController.ControllerState.Attacking: Target.Rows[i].Cells[1].Value = "Attacking"; break;  //check do these codes properly installed 
                //    case NetworkController.ControllerState.Error: Target.Rows[i].Cells[1].Value = "Error"; break;
                //    case NetworkController.ControllerState.Master: Target.Rows[i].Cells[1].Value = "Master"; break;
                //    case NetworkController.ControllerState.Suspending: Target.Rows[i].Cells[1].Value = "Suspending"; break;
                //    case NetworkController.ControllerState.Tuning: Target.Rows[i].Cells[1].Value = "Tuning"; break;
                //}
            }
        }

        private void ParamsBtn_Click(object sender, EventArgs e)
        {
            BananaGuy.setState((int)MascotController.states.preparing);
            //open config window
            //specify restricted address pool
            SettingsForm Sets = new SettingsForm(Controller.Params, Controller.Adapter, Controller.CurrentPort, SetSettings, Controller.mode, Controller.mode ? null : Controller.MasterIpEndPont);
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
                try
                {
                    this.Invoke(DataCallBack, new Object[] { Message });
                }
                catch (ObjectDisposedException)
                {

                    return;
                }
            }
            else
            {
                LogBox.AppendText(DateTime.Now.ToString("HH:mm:ss.ffff") + ": " + Message + Environment.NewLine);
                LogBox.SelectionStart = LogBox.TextLength; //obj disposed
                LogBox.ScrollToCaret();
            }
        }
        private void ChangeMode(bool mode)
        {
            if (Controller.mode == true)
            {
                ControlTab.SelectTab(0);
                RefreshHostList(DeviceObserveGrid);
                MIpEndPointlab.Text = Controller.LocalIpEndPoint.ToString();
            }
            else
            {
                ControlTab.SelectTab(1);
                ConnectBtn.Enabled = true;
                HIpEndPointLab.Text = Controller.LocalIpEndPoint.ToString();
                HMasterLab.Text = Controller.MasterIpEndPont.ToString();
            }
        }

        private void TabChangingHandler(object sender, TabControlCancelEventArgs e)
        {
            //check who is before
            if (e.TabPageIndex == 0)  // if there was master mode, 
            {
                if (!Controller.mode) // and now user mode
                {

                    e.Cancel = false; //do not cancel tab changing
                    return;
                }
                else
                {
                    //show new master choosing dialog
                }
            }
            else  //if there was user mode
            {
                if (Controller.mode) // and now master mode
                {
                    //MIpEndPointlab.Text = Controller.LocalIpEndPoint.ToString();
                    e.Cancel = false; //do not cancel tab changing
                    return;
                }
                else
                {
                    //print: only master can assign new master  //
                }
            }
            e.Cancel = true;

        }
        private void SetSettings(AttackParams Params, System.Net.NetworkInformation.NetworkInterface Adapter, int altport, IPEndPoint NewMasterPoint)
        {
            //Controller.Close();
            LogBox.Text = "";
            Controller.InitInterface(Adapter, altport, NewMasterPoint);  //catch occupied port exception
            Controller.InitParams(Params);
            if (Controller.mode)
            {
                ChangeMode(true);
            }
            else ChangeMode(false);


        }
        private void ErrorHandler(string message)
        {
            UpdateData("Ошибка " + message);
            BananaGuy.setState((int)MascotController.states.pause);
        }
        private void StatisticRespond(UInt32 http, UInt32 udp, UInt32 totalhttp, UInt32 totaludp)
        {
            if (Controller.mode)
            {
                if (MHttpLocLab.InvokeRequired)  //invoke required always, do we need to check?
                {
                    LabelChangeCallBack SetLabel = new LabelChangeCallBack(setLabel);
                    this.Invoke(SetLabel, new Object[] { MHttpLocLab, http });
                    this.Invoke(SetLabel, new Object[] { MUpdLocLab, udp });
                    this.Invoke(SetLabel, new Object[] { MHttpTotLab, totalhttp });
                    this.Invoke(SetLabel, new Object[] { MUdpTotalLab, totaludp });
                    
                    //RefreshHostList(DeviceObserveGrid); 
                }
                else
                {
                    setLabel(MHttpLocLab, http);
                    setLabel(MUpdLocLab, udp);
                    setLabel(MHttpTotLab, totalhttp);
                    setLabel(MUdpTotalLab, totaludp);
                }
            }
            else
            {
                HostHttpTotalLab.Text = http.ToString();
                HUdpTotalLab.Text = udp.ToString();
            }
        }
        private void setLabel(Label Target, uint data)
        {
            Target.Text = data.ToString();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Controller != null)
            {
                Controller.Close();
            }
            if (StatisticTimer.Enabled) StatisticTimer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateData("Попытка подключения к мастеру");
            //Controller.InitInterface(Controller.Adapter, Controller.CurrentPort, Controller.MasterIpEndPont);
            Controller.ConnectToMaster();
            //if (Controller.mode)
            //{
            //    ChangeMode(true);
            //}
            //else ChangeMode(false);

        }
        private void getStats(object sender, EventArgs e)
        {
            if(Controller.isAttacking)
            {
                Controller.Statistic();
            }

        }
    }
}
