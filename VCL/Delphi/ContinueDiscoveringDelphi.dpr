program ContinueDiscoveringDelphi;

uses
  Vcl.Forms,
  main in 'main.pas' {fmMain},
  Discoverer in 'Discoverer.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfmMain, fmMain);
  Application.Run;
end.
