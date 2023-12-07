using System;
using System.Collections.Generic;

using wclCommon;
using wclBluetooth;

namespace ContinueDiscovering
{
    internal delegate void DeviceEvent(Object sender, Int64 Address);

    internal class Discoverer : wclBluetoothManager
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
            FRadio = null;

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
    };
}
