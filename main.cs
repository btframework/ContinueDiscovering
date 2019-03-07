using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using wclBluetooth;
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
            FDisc.OnDeviceFound +=new DeviceEvent(FDisc_OnDeviceFound);
            FDisc.OnDeviceLost += new DeviceEvent(FDisc_OnDeviceLost);

            Int32 Res = FDisc.Open();
            if (Res != wclErrors.WCL_E_SUCCESS)
                lbLog.Items.Add("Open failed; 0x" + Res.ToString("X8"));
        }

        void FDisc_OnDeviceLost(object sender, Int64 Address)
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
        
        void  FDisc_OnDeviceFound(object sender, Int64 Address)
        {
            lbLog.Items.Add("Device found: " + Address.ToString("X12"));
            ListViewItem Item = lvDevices.Items.Add(Address.ToString("X12"));

            String Name;
            Int32 Res = FDisc.Radio.GetRemoteName(Address, out Name);
            if (Res != wclErrors.WCL_E_SUCCESS)
                Name = "Error: 0x" + Res.ToString("X8");

            Item.SubItems.Add(Name);
        }

        void FDisc_OnStopped(object sender, EventArgs e)
        {
            lbLog.Items.Add("Stopped");

            lvDevices.Items.Clear();
        }

        void FDisc_OnStarted(object sender, EventArgs e)
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
    }

    public delegate void DeviceEvent(Object sender, Int64 Address);

    public class Discoverer : wclBluetoothManager
    {
        private List<Int64> FFoundDevices;
        private List<Int64> FDevices;
        private Boolean FDiscovering;
        private wclBluetoothRadio FRadio;

        protected override void DoDiscoveringStarted(wclBluetoothRadio Radio)
        {
            FFoundDevices.Clear();
        }

        protected override void DoDeviceFound(wclBluetoothRadio Radio, Int64 Address)
        {
            FFoundDevices.Add(Address);
            if (!FDevices.Contains(Address))
            {
                FDevices.Add(Address);
                if (OnDeviceFound != null)
                    OnDeviceFound(this, Address);
            }
        }

        protected override void DoDiscoveringCompleted(wclBluetoothRadio Radio, int Error)
        {
            List<Int64> Devices = new List<Int64>();

            foreach (Int64 Address in FDevices)
            {
                if (FFoundDevices.Contains(Address))
                    Devices.Add(Address);
                else
                {
                    if (OnDeviceLost != null)
                        OnDeviceLost(this, Address);
                }
            }

            FDevices.Clear();
            FDevices = Devices;

            if (FDiscovering)
                Radio.Discover(10, wclBluetoothDiscoverKind.dkClassic);
        }

        public Discoverer()
            : base()
        {
            FFoundDevices = null;
            FDevices = null;
            FDiscovering = false;

            OnDeviceFound = null;
            OnDeviceLost = null;
            OnStarted = null;
            OnStopped = null;
        }

        public Int32 Start()
        {
            if (FDiscovering)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DISCOVERING_RUNNING;

            if (Count == 0)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_HARDWARE_NOT_AVAILABLE;

            wclBluetoothRadio Radio = null;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Available)
                {
                    Radio = this[i];
                    break;
                }
            }

            if (Radio == null)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DRIVER_NOT_AVAILABLE;

            FDevices = new List<Int64>();
            FFoundDevices = new List<Int64>();

            Int32 Res = Radio.Discover(10, wclBluetoothDiscoverKind.dkClassic);
            if (Res != wclErrors.WCL_E_SUCCESS)
            {
                FDevices = null;
                FFoundDevices = null;
            }
            else
            {
                FDiscovering = true;
                FRadio = Radio;

                if (OnStarted != null)
                    OnStarted(this, EventArgs.Empty);
            }

            return Res;
        }

        public Int32 Stop()
        {
            if (!FDiscovering)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DISCOVERING_NOT_RUNNING;

            FDiscovering = false;
            int Res = FRadio.Terminate();

            FDevices.Clear();
            FFoundDevices.Clear();

            FRadio = null;
            FDevices = null;
            FFoundDevices = null;

            if (OnStopped != null)
                OnStopped(this, EventArgs.Empty);

            return Res;
        }

        public wclBluetoothRadio Radio { get { return FRadio; } }

        public new event DeviceEvent OnDeviceFound;
        public event DeviceEvent OnDeviceLost;
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;
    }
}