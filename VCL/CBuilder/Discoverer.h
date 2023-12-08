//---------------------------------------------------------------------------

#ifndef DiscovererH
#define DiscovererH
//---------------------------------------------------------------------------

#include <list>
#include "wclBluetooth.hpp"

using namespace std;

typedef void __fastcall (__closure *TDeviceEvent)(TObject* Sender, const __int64 Address);

class TDiscoverer : public TwclBluetoothManager
{
private:
	list<__int64> FFoundDevices;
	list<__int64> FDevices;
	bool FDiscovering;
	TwclBluetoothRadio* FRadio;

	TDeviceEvent FOnDeviceFound;
	TDeviceEvent FOnDeviceLost;
	TNotifyEvent FOnStarted;
	TNotifyEvent FOnStopped;

protected:
	virtual void __fastcall DoDiscoveringStarted(
	  TwclBluetoothRadio* const Radio);
	virtual void __fastcall DoDeviceFound(TwclBluetoothRadio* const Radio,
		const __int64 Address);
	virtual void __fastcall DoDiscoveringCompleted(
		TwclBluetoothRadio* const Radio, const int Error);

public:
	virtual __fastcall TDiscoverer();
	virtual __fastcall ~TDiscoverer();

	int __fastcall Start();
	int __fastcall Stop();

	__property TwclBluetoothRadio* Radio = { read = FRadio };

	__property TDeviceEvent OnDeviceFound = { read = FOnDeviceFound,
		write = FOnDeviceFound };
	__property TDeviceEvent OnDeviceLost = { read = FOnDeviceLost,
		write = FOnDeviceLost };
	__property TNotifyEvent OnStarted = { read = FOnStarted,
		write = FOnStarted };
	__property TNotifyEvent OnStopped = { read = FOnStopped,
		write = FOnStopped };
};

#endif
