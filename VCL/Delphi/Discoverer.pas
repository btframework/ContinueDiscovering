unit Discoverer;

interface

uses
  wclBluetooth, System.Generics.Collections, Classes;

type
  TDeviceEvent = procedure(Sender: TObject; const Address: Int64) of object;

  TDiscoverer = class(TwclBluetoothManager)
  private
    FFoundDevices: TList<Int64>;
    FDevices: TList<Int64>;
    FDiscovering: Boolean;
    FRadio: TwclBluetoothRadio;

    FOnDeviceFound: TDeviceEvent;
    FOnDeviceLost: TDeviceEvent;
    FOnStarted: TNotifyEvent;
    FOnStopped: TNotifyEvent;

  protected
    procedure DoDiscoveringStarted(const Radio: TwclBluetoothRadio); override;
    procedure DoDeviceFound(const Radio: TwclBluetoothRadio;
        const Address: Int64); override;
    procedure DoDiscoveringCompleted(const Radio: TwclBluetoothRadio;
      const Error: Integer); override;

  public
    constructor Create; reintroduce;
    destructor Destroy; override;

    function Start: Integer;
    function Stop: Integer;

    property Radio: TwclBluetoothRadio read FRadio;

    property OnDeviceFound: TDeviceEvent read FOnDeviceFound
      write FOnDeviceFound;
    property OnDeviceLost: TDeviceEvent read FOnDeviceLost write FOnDeviceLost;
    property OnStarted: TNotifyEvent read FOnStarted write FOnStarted;
    property OnStopped: TNotifyEvent read FOnStopped write FOnStopped;
  end;

implementation

uses
  wclBluetoothErrors, wclErrors;

{ TDiscoverer }

constructor TDiscoverer.Create;
begin
  inherited Create(nil);

  FFoundDevices := nil;
  FDevices := nil;
  FDiscovering := False;
  FRadio := nil;

  FOnDeviceFound := nil;
  FOnDeviceLost := nil;
  FOnStarted := nil;
  FOnStopped := nil;
end;

destructor TDiscoverer.Destroy;
begin
  Stop;

  inherited;
end;

procedure TDiscoverer.DoDeviceFound(const Radio: TwclBluetoothRadio;
  const Address: Int64);
begin
  FFoundDevices.Add(Address);
  if not FDevices.Contains(Address) then begin
    FDevices.Add(Address);
    if Assigned(FOnDeviceFound) then
      FOnDeviceFound(Self, Address);
  end;
end;

procedure TDiscoverer.DoDiscoveringCompleted(const Radio: TwclBluetoothRadio;
  const Error: Integer);
var
  Devices: TList<Int64>;
  Address: Int64;
begin
  Devices := TList<Int64>.Create;

  for Address in FDevices do begin
    if FFoundDevices.Contains(Address) then
      Devices.Add(Address)
    else begin
      if Assigned(FOnDeviceLost) then
        FOnDeviceLost(Self, Address);
    end;
  end;

  FDevices.Free;
  FDevices := Devices;

  if FDiscovering then
    Radio.Discover(10, dkClassic);
end;

procedure TDiscoverer.DoDiscoveringStarted(const Radio: TwclBluetoothRadio);
begin
  FFoundDevices.Clear;
end;

function TDiscoverer.Start: Integer;
var
  Radio: TwclBluetoothRadio;
  i: Integer;
begin
  if FDiscovering then
    Result := WCL_E_BLUETOOTH_DISCOVERING_RUNNING

  else begin
    if Count = 0 then
      Result := WCL_E_BLUETOOTH_HARDWARE_NOT_AVAILABLE

    else begin
      Radio := nil;
      for i := 0 to Count - 1 do begin
        if Radios[i].Available then begin
          Radio := Radios[i];
          Break;
        end;
      end;

      if Radio = nil then
        Result := WCL_E_BLUETOOTH_DRIVER_NOT_AVAILABLE

      else begin
        FDevices := TList<Int64>.Create;
        FFoundDevices := TList<Int64>.Create;

        Result := Radio.Discover(10, dkClassic);
        if Result <> WCL_E_SUCCESS then begin
          FDevices.Free;
          FFoundDevices.Free;

          FDevices := nil;
          FFoundDevices := nil;
        end else begin
          FDiscovering := True;
          FRadio := Radio;

          if Assigned(FOnStarted) then
            FOnStarted(Self);
        end;
      end;
    end;
  end;
end;

function TDiscoverer.Stop: Integer;
begin
  if not FDiscovering then
    Result := WCL_E_BLUETOOTH_DISCOVERING_NOT_RUNNING

  else begin
    FDiscovering := False;

    Result := FRadio.Terminate;

    FDevices.Free;
    FFoundDevices.Free;

    FRadio := nil;
    FDevices := nil;
    FFoundDevices := nil;

    if Assigned(FOnStopped) then
      FOnStopped(Self);
  end;
end;

end.
