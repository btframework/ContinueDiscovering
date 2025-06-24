#pragma once

#include <list>
#include "wclBluetooth.h"

using namespace std;
using namespace wclBluetooth;


#define DeviceEvent(_event_name_) \
	__event void _event_name_(void* sender, const __int64 Address)


class CDiscoverer : public CwclBluetoothManager
{
	DISABLE_COPY(CDiscoverer);

private:
	list<__int64>		FFoundDevices;
	list<__int64>		FDevices;
	bool				FDiscovering;
	CwclBluetoothRadio*	FRadio;

protected:
	virtual void DoDiscoveringStarted(CwclBluetoothRadio* const Radio) override;
	virtual void DoDeviceFound(CwclBluetoothRadio* const Radio, const __int64 Address) override;
	virtual void DoDiscoveringCompleted(CwclBluetoothRadio* const Radio, const int Error) override;

public:
	CDiscoverer();

	int Start();
	int Stop();

	DeviceEvent(OnDeviceFound);
	DeviceEvent(OnDeviceLost);
	wclNotifyEvent(OnStarted);
	wclNotifyEvent(OnStopped);

	CwclBluetoothRadio* GetRadio() const;

	_declspec(property(get = GetRadio)) CwclBluetoothRadio* Radio;
};