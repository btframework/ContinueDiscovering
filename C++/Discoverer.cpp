#include "stdafx.h"

#include <algorithm>
#include "Discoverer.h"

void CDiscoverer::DoDiscoveringStarted(CwclBluetoothRadio* const Radio)
{
	FFoundDevices.clear();
}

void CDiscoverer::DoDeviceFound(CwclBluetoothRadio* const Radio, const __int64 Address)
{
	FFoundDevices.push_back(Address);
	if (find(FDevices.begin(), FDevices.end(), Address) == FDevices.end())
	{
		FDevices.push_back(Address);
		__raise OnDeviceFound(this, Address);
	}
}

void CDiscoverer::DoDiscoveringCompleted(CwclBluetoothRadio* const Radio, const int Error)
{
	list<__int64> Devices;
	
	for (list<__int64>::iterator Address = FDevices.begin(); Address != FDevices.end(); Address++)
	{
		if (find(FFoundDevices.begin(), FFoundDevices.end(), *Address) != FFoundDevices.end())
			Devices.push_back(*Address);
		else
			__raise OnDeviceLost(this, *Address);
	}

	FDevices.clear();
	FDevices = Devices;

	if (FDiscovering)
		Radio->Discover(10, dkClassic);
}

CDiscoverer::CDiscoverer() : CwclBluetoothManager()
{
	FRadio = NULL;
	FFoundDevices.clear();
	FDevices.clear();
	FDiscovering = false;
}

int CDiscoverer::Start()
{
	if (FDiscovering)
		return WCL_E_BLUETOOTH_DISCOVERING_RUNNING;

	if (Count == 0)
		return WCL_E_BLUETOOTH_HARDWARE_NOT_AVAILABLE;

	CwclBluetoothRadio* Radio = NULL;
	for (size_t i = 0; i < Count; i++)
	{
		if (Radios[i]->Available)
		{
			Radio = Radios[i];
			break;
		}
	}

	if (Radio == NULL)
		return WCL_E_BLUETOOTH_DRIVER_NOT_AVAILABLE;

	FDevices.clear();
	FFoundDevices.clear();

	int Res = Radio->Discover(10, dkClassic);
	if (Res != WCL_E_SUCCESS)
	{
		FDevices.clear();
		FFoundDevices.clear();
	}
	else
	{
		FDiscovering = true;
		FRadio = Radio;

		__raise OnStarted(this);
	}

	return Res;
}

int CDiscoverer::Stop()
{
	if (!FDiscovering)
		return WCL_E_BLUETOOTH_DISCOVERING_NOT_RUNNING;

	FDiscovering = false;
	int Res = FRadio->Terminate();

	FDevices.clear();
	FFoundDevices.clear();

	FRadio = NULL;
	
	__raise OnStopped(this);

	return Res;
}

CwclBluetoothRadio* CDiscoverer::GetRadio() const
{
	return FRadio;
}