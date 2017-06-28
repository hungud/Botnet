using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

namespace Botnet
{
    public partial class SettingsForm : Form
    {
        private NetworkInterface[] Adapters;
        private NetworkInterface curAdapter;
        public delegate void SetSettingsCallBack(AttackParams Params, NetworkInterface Adapter, int port, IPEndPoint MasterAddr);
        SetSettingsCallBack SetSettings;

        public SettingsForm(AttackParams Params, NetworkInterface CurAdapter, int curport, SetSettingsCallBack funct, bool mode, IPEndPoint OldMaster)//if true init new ex, otherwise
        {
            InitializeComponent(); //fill previous params to form if obkect is initialized
            SetSettings = funct;
            curAdapter = CurAdapter;
            CurAdapterLabel.Text = CurAdapter.Name + " - Status: " + CurAdapter.OperationalStatus + " Type:" + CurAdapter.NetworkInterfaceType;
            fillAdapterList();
            FillPreviousSettings(Params, curport);
            RoleCheckBox.Enabled = true;
            if (mode)
            {
                AttackParamsBox.Enabled = true;
                RoleCheckBox.Checked = true;
                
            }
            else
            {
                RoleCheckBox.Checked = false;
                AttackParamsBox.Enabled = false;
                //RoleCheckBox.Enabled = false;
                MasterIpBox.Text = OldMaster.Address.ToString();
                MasterPortBox.Value = OldMaster.Port;
            }

            

        }
        private void FillPreviousSettings(AttackParams Params, int cur_port)
        {
            TargetAddressBox.Text = Params.Target.Address.ToString();
            TargetPortNum.Value = Params.Target.Port;
            HttpContentBox.Text = Params.HttpMsg;
            FloodEnablingCheckBox.Checked = Params.UdpFloodEnabled;
            RestStartBox.Text = Params.RestrictedPool[0].ToString();
            RestEndBox.Text = Params.RestrictedPool[1].ToString();
            portBox.Value = cur_port;
        }
        private void SetParamnBtn_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(portBox.Value);
            int targPort = Convert.ToInt32(TargetPortNum.Value);
            IPAddress TargAddress;
            if ((IPAddress.TryParse(TargetAddressBox.Text, out TargAddress) && (HttpContentBox.Text.Length > 10)))
            {
                IPEndPoint Target = new IPEndPoint(TargAddress, targPort);
                if (FloodEnablingCheckBox.Checked)
                {
                    IPAddress RestStartAddr;
                    IPAddress RestEndAddr;
                    if (IPAddress.TryParse(RestStartBox.Text, out RestStartAddr) && IPAddress.TryParse(RestEndBox.Text, out RestEndAddr))
                    {
                        AttackParams Params = new AttackParams(Target, HttpContentBox.Text, new NetworkInstruments.AddressPool(RestStartAddr, RestEndAddr));
                        if (RoleCheckBox.Checked)
                        {
                            SetSettings(Params, Adapters[AdapterBox.SelectedIndex], port, null); 
                        }
                        else
                        {
                            SetSettings(Params, Adapters[AdapterBox.SelectedIndex], port, new IPEndPoint(IPAddress.Parse(MasterIpBox.Text),Convert.ToUInt16(MasterPortBox.Value)));
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Введенные данные некорректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    AttackParams Params = new AttackParams(Target, HttpContentBox.Text);
                    if (RoleCheckBox.Checked)
                    {
                        SetSettings(Params, Adapters[AdapterBox.SelectedIndex], port, null);
                    }
                    else
                    {
                        SetSettings(Params, Adapters[AdapterBox.SelectedIndex], port, new IPEndPoint(IPAddress.Parse(MasterIpBox.Text), Convert.ToUInt16(MasterPortBox.Value)));
                    }
                    this.Close();
                }

            }
            else
            {
                MessageBox.Show("Введенные данные некорректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FloodEnablingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (FloodEnablingCheckBox.Checked)
            {
                RestPoolsSetBox.Enabled = true;
            }
            else RestPoolsSetBox.Enabled = false;
        }

        private void AdapterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //IpLab.Text = Adapters[AdapterBox.SelectedIndex].GetIPProperties().UnicastAddresses.W 

            IpLab.Text = NetworkInstruments.getAdapterIPAddress(Adapters[AdapterBox.SelectedIndex]).ToString();
            MacLab.Text = Adapters[AdapterBox.SelectedIndex].GetPhysicalAddress().ToString();

        }
        private void fillAdapterList()
        {
            Adapters = NetworkInterface.GetAllNetworkInterfaces();

            for (int i = 0; i < Adapters.Length; ++i)
            {
                AdapterBox.Items.Add(Adapters[i].Name + " Status: " + Adapters[i].OperationalStatus.ToString() + " Type: " + Adapters[i].NetworkInterfaceType);
                if(Adapters[i].Id == curAdapter.Id)
                {
                    AdapterBox.SelectedIndex = i;
                }
            }
        }

        private void RoleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RoleCheckBox.Checked)
            {
                masterPointBox.Enabled = false;
                AttackParamsBox.Enabled = true;
            }
            else
            {
                masterPointBox.Enabled = true;
                AttackParamsBox.Enabled = false;
            }
        }
    }
}
