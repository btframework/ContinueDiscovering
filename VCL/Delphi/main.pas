unit main;

interface

uses
  Vcl.Forms, Vcl.StdCtrls, Vcl.Controls, Vcl.ComCtrls, System.Classes,
  Discoverer;

type
  TfmMain = class(TForm)
    btStart: TButton;
    btStop: TButton;
    lvDevices: TListView;
    btClear: TButton;
    lbLog: TListBox;
    procedure btClearClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure btStopClick(Sender: TObject);

  private
    FDisc: TDiscoverer;

    procedure DiscOnDeviceLost(Sender: TObject; const Address: Int64);
    procedure DiscOnDeviceFound(Sender: TObject; const Address: Int64);
    procedure DiscOnStopped(Sender: TObject);
    procedure DiscOnStarted(Sender: TObject);
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors, SysUtils;

{$R *.dfm}

procedure TfmMain.btClearClick(Sender: TObject);
begin
  lbLog.Items.Clear;
end;

procedure TfmMain.btStartClick(Sender: TObject);
var
  Res: Integer;
begin
  Res := FDisc.Start;
  if Res <> WCL_E_SUCCESS then begin
    lbLog.Items.Add('Start failed: 0x' + IntToHex(Res, 8));
    FDisc.Close;
  end;
end;

procedure TfmMain.btStopClick(Sender: TObject);
var
  Res: Integer;
begin
  Res := FDisc.Stop;
  if Res <> WCL_E_SUCCESS then
    lbLog.Items.Add('Stop failed: 0x' + IntToHex(Res, 8));
end;

procedure TfmMain.DiscOnDeviceFound(Sender: TObject; const Address: Int64);
var
  Item: TListItem;
  Name: string;
  Res: Integer;
begin
  lbLog.Items.Add('Device found: ' + IntToHex(Address, 12));
  Item := lvDevices.Items.Add;
  Item.Caption := IntToHex(Address, 12);

  Res := FDisc.Radio.GetRemoteName(Address, Name);
  if Res <> WCL_E_SUCCESS then
    Name := 'Error: 0x' + IntToHex(Res, 8);

  Item.SubItems.Add(Name);
end;

procedure TfmMain.DiscOnDeviceLost(Sender: TObject; const Address: Int64);
var
  Item: TListItem;
begin
  lbLog.Items.Add('Device lost: ' + IntToHex(Address, 12));
  for Item in lvDevices.Items do begin
   if Item.Caption = IntToHex(Address, 12) then begin
     lvDevices.Items.Delete(Item.Index);
     Break;
   end;
  end;
end;

procedure TfmMain.DiscOnStarted(Sender: TObject);
begin
  lbLog.Items.Add('Started');
end;

procedure TfmMain.DiscOnStopped(Sender: TObject);
begin
  lbLog.Items.Add('Stopped');

  lvDevices.Items.Clear;
end;

procedure TfmMain.FormCreate(Sender: TObject);
var
  Res: Integer;
begin
  FDisc := TDiscoverer.Create;
  FDisc.OnStarted := DiscOnStarted;
  FDisc.OnStopped := DiscOnStopped;
  FDisc.OnDeviceFound := DiscOnDeviceFound;
  FDisc.OnDeviceLost := DiscOnDeviceLost;

  Res := FDisc.Open;
  if Res <> WCL_E_SUCCESS then begin
    lbLog.Items.Add('Open failed; 0x' + IntToHex(Res, 8));
    FDisc.Free;
  end;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  FDisc.Stop;
  FDisc.Close;
  FDisc.Free;
  FDisc := nil;
end;

end.
