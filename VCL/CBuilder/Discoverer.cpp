//---------------------------------------------------------------------------

#pragma hdrstop

#include <algorithm>
#include "Discoverer.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)

__fastcall TDiscoverer::TDiscoverer() : TwclBluetoothManager(NULL)
{
	FFoundDevices.clear();
	FDevices.clear();
	FDiscovering = false;
	FRadio = NULL;

	FOnDeviceFound = NULL;
	FOnDeviceLost = NULL;
	FOnStarted = NULL;
	FOnStopped = NULL;
}
//---------------------------------------------------------------------------

__fastcall TDiscoverer::~TDiscoverer()
{
	Stop();
}
//---------------------------------------------------------------------------

void __fastcall TDiscoverer::DoDeviceFound(TwclBluetoothRadio* const Radio,
	const __int64 Address)
{
	FFoundDevices.push_back(Address);
	if (find(FDevices.begin(), FDevices.end(), Address) == FDevices.end())
	{
		FDevices.push_back(Address);
		if (FOnDeviceFound != NULL)
			FOnDeviceFound(this, Address);
	}
}
//---------------------------------------------------------------------------

void __fastcall TDiscoverer::DoDiscoveringCompleted(
	TwclBluetoothRadio* const Radio, const int Error)
{
	list<__int64> Devices;

	for (list<__int64>::iterator Address = FDevices.begin(); Address != FDevices.end(); Address++)
	{
		if (find(FFoundDevices.begin(), FFoundDevices.end(), *Address) != FFoundDevices.end())
			Devices.push_back(*Address);
		else
		{
			if (FOnDeviceLost != NULL)
				FOnDeviceLost(this, *Address);
		}
	}

	FDevices = Devices;

	if (FDiscovering)
		Radio->Discover(10, dkClassic);
}
//---------------------------------------------------------------------------

void __fastcall TDiscoverer::DoDiscoveringStarted(
	TwclBluetoothRadio* const Radio)
{
	FFoundDevices.clear();
}
//---------------------------------------------------------------------------

int __fastcall TDiscoverer::Start()
{
	if (FDiscovering)
		return WCL_E_BLUETOOTH_DISCOVERING_RUNNING;
	if (Count == 0)
		return WCL_E_BLUETOOTH_HARDWARE_NOT_AVAILABLE;

	TwclBluetoothRadio* Radio = NULL;
	for (int i = 0; i < Count; i++)
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

	int Result = Radio->Discover(10, dkClassic);
	if (Result == WCL_E_SUCCESS)
	{
		FDiscovering = true;
		FRadio = Radio;

		if (FOnStarted != NULL)
			FOnStarted(this);
	}
	return Result;
}
//---------------------------------------------------------------------------

int __fastcall TDiscoverer::Stop()
{
	if (!FDiscovering)
		return WCL_E_BLUETOOTH_DISCOVERING_NOT_RUNNING;

	FDiscovering = false;

	int Result = FRadio->Terminate();

	FDevices.clear();
	FFoundDevices.clear();

	FRadio = NULL;

	if (FOnStopped != NULL)
		FOnStopped(this);

	return Result;
}
//---------------------------------------------------------------------------
