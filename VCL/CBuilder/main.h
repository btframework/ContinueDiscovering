//---------------------------------------------------------------------------

#ifndef mainH
#define mainH
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include "Discoverer.h"
#include <Vcl.ComCtrls.hpp>
#include "wclBluetooth.hpp"
//---------------------------------------------------------------------------
class TfmMain : public TForm
{
__published:	// IDE-managed Components
	TButton *btStart;
	TButton *btStop;
	TListView *lvDevices;
	TButton *btClear;
	TListBox *lbLog;
	void __fastcall btClearClick(TObject *Sender);
	void __fastcall btStartClick(TObject *Sender);
	void __fastcall btStopClick(TObject *Sender);
	void __fastcall FormCreate(TObject *Sender);
	void __fastcall FormDestroy(TObject *Sender);

private:
	TDiscoverer* FDisc;

	void __fastcall DiscOnDeviceLost(TObject* Sender, const __int64 Address);
	void __fastcall DiscOnDeviceFound(TObject* Sender, const __int64 Address);
	void __fastcall DiscOnStopped(TObject* Sender);
	void __fastcall DiscOnStarted(TObject* Sender);

public:		// User declarations
	__fastcall TfmMain(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TfmMain *fmMain;
//---------------------------------------------------------------------------
#endif
