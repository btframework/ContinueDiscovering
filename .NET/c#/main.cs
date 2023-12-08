using System;
using System.Windows.Forms;

using wclCommon;

namespace ContinueDiscovering
{
    public partial class fmMain : Form
    {
        private Discoverer FDisc;

        public fmMain()
        {
            InitializeComponent();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            lbLog.Items.Clear();
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            FDisc = new Discoverer();
            FDisc.OnStarted += new EventHandler(FDisc_OnStarted);
            FDisc.OnStopped += new EventHandler(FDisc_OnStopped);
            FDisc.OnDeviceFound += new DeviceEvent(FDisc_OnDeviceFound);
            FDisc.OnDeviceLost += new DeviceEvent(FDisc_OnDeviceLost);

            Int32 Res = FDisc.Open();
            if (Res != wclErrors.WCL_E_SUCCESS)
                lbLog.Items.Add("Open failed; 0x" + Res.ToString("X8"));
        }

        private void FDisc_OnDeviceLost(object sender, Int64 Address)
        {
            lbLog.Items.Add("Device lost: " + Address.ToString("X12"));
            foreach (ListViewItem Item in lvDevices.Items)
            {
                if (Item.Text == Address.ToString("X12"))
                {
                    lvDevices.Items.Remove(Item);
                    break;
                }
            }
        }

        private void FDisc_OnDeviceFound(object sender, Int64 Address)
        {
            lbLog.Items.Add("Device found: " + Address.ToString("X12"));
            ListViewItem Item = lvDevices.Items.Add(Address.ToString("X12"));

            String Name;
            Int32 Res = FDisc.Radio.GetRemoteName(Address, out Name);
            if (Res != wclErrors.WCL_E_SUCCESS)
                Name = "Error: 0x" + Res.ToString("X8");

            Item.SubItems.Add(Name);
        }

        private void FDisc_OnStopped(object sender, EventArgs e)
        {
            lbLog.Items.Add("Stopped");

            lvDevices.Items.Clear();
        }

        private void FDisc_OnStarted(object sender, EventArgs e)
        {
            lbLog.Items.Add("Started");
        }

        private void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FDisc.Stop();
            FDisc.Close();
            FDisc = null;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            Int32 Res = FDisc.Start();
            if (Res != wclErrors.WCL_E_SUCCESS)
            {
                lbLog.Items.Add("Start failed: 0x" + Res.ToString("X8"));
                FDisc.Close();
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            Int32 Res = FDisc.Stop();
            if (Res != wclErrors.WCL_E_SUCCESS)
                lbLog.Items.Add("Stop failed: 0x" + Res.ToString("X8"));
        }
    };
}