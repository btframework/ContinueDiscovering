//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "main.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma link "wclBluetooth"
#pragma resource "*.dfm"
TfmMain *fmMain;
//---------------------------------------------------------------------------
__fastcall TfmMain::TfmMain(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::btClearClick(TObject *Sender)
{
	lbLog->Items->Clear();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::btStartClick(TObject *Sender)
{
	int Res = FDisc->Start();
	if (Res != WCL_E_SUCCESS)
	{
		lbLog->Items->Add("Start failed: 0x" + IntToHex(Res, 8));
		FDisc->Close();
	}
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::btStopClick(TObject *Sender)
{
	int Res = FDisc->Stop();
	if (Res != WCL_E_SUCCESS)
		lbLog->Items->Add("Stop failed: 0x" + IntToHex(Res, 8));
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::DiscOnDeviceFound(TObject* Sender,
	const __int64 Address)
{
	lbLog->Items->Add("Device found: " + IntToHex(Address, 12));
	TListItem* Item = lvDevices->Items->Add();
	Item->Caption = IntToHex(Address, 12);

	String Name;
	int Res = FDisc->Radio->GetRemoteName(Address, Name);
	if (Res != WCL_E_SUCCESS)
		Name = "Error: 0x" + IntToHex(Res, 8);

	Item->SubItems->Add(Name);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::DiscOnDeviceLost(TObject* Sender,
	const __int64 Address)
{
	lbLog->Items->Add("Device lost: " + IntToHex(Address, 12));
	for (int i = 0; i < lvDevices->Items->Count; i++)
	{
		TListItem* Item = lvDevices->Items->Item[i];
		if (Item->Caption == IntToHex(Address, 12))
		{
			lvDevices->Items->Delete(Item->Index);
			break;
		}
	}
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::DiscOnStarted(TObject* Sender)
{
	lbLog->Items->Add("Started");
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::DiscOnStopped(TObject* Sender)
{
	lbLog->Items->Add("Stopped");

	lvDevices->Items->Clear();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::FormCreate(TObject *Sender)
{
	FDisc = new TDiscoverer();
	FDisc->OnStarted = DiscOnStarted;
	FDisc->OnStopped = DiscOnStopped;
	FDisc->OnDeviceFound = DiscOnDeviceFound;
	FDisc->OnDeviceLost = DiscOnDeviceLost;

	int Res = FDisc->Open();
	if (Res != WCL_E_SUCCESS)
	{
		lbLog->Items->Add("Open failed; 0x" + IntToHex(Res, 8));
		FDisc->Free();
	}
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::FormDestroy(TObject *Sender)
{
	FDisc->Stop();
	FDisc->Close();
	FDisc->Free();
	FDisc = NULL;
}
//---------------------------------------------------------------------------

