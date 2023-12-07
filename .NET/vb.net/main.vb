Public Class fmMain
    Private FDisc As Discoverer

    Private Sub btClear_Click(sender As Object, e As EventArgs) Handles btClear.Click
        lbLog.Items.Clear()
    End Sub

    Public Delegate Sub DeviceEvent(sender As Object, Address As Int64)

    Public Class Discoverer
        Inherits wclBluetoothManager

        Private FFoundDevices As List(Of Int64)
        Private FDevices As List(Of Int64)
        Private FDiscovering As Boolean
        Private FRadio As wclBluetoothRadio

        Protected Overrides Sub DoDiscoveringStarted(Radio As wclBluetoothRadio)
            FFoundDevices.Clear()
        End Sub

        Protected Overrides Sub DoDeviceFound(Radio As wclBluetoothRadio, Address As Int64)
            FFoundDevices.Add(Address)
            If Not FDevices.Contains(Address) Then
                FDevices.Add(Address)
                If OnDeviceFoundEvent IsNot Nothing Then OnDeviceFoundEvent(Me, Address)
            End If
        End Sub

        Protected Overrides Sub DoDiscoveringCompleted(Radio As wclBluetoothRadio, [Error] As Integer)
            Dim Devices As List(Of Int64) = New List(Of Int64)()

            For Each Address As Int64 In FDevices
                If FFoundDevices.Contains(Address) Then
                    Devices.Add(Address)
                Else
                    If OnDeviceLostEvent IsNot Nothing Then OnDeviceLostEvent(Me, Address)
                End If
            Next

            FDevices.Clear()
            FDevices = Devices

            If FDiscovering Then Radio.Discover(10, wclBluetoothDiscoverKind.dkClassic)
        End Sub

        Sub New()
            MyBase.New()

            FFoundDevices = Nothing
            FDevices = Nothing
            FDiscovering = False

            OnDeviceFoundEvent = Nothing
            OnDeviceLostEvent = Nothing
            OnStartedEvent = Nothing
            OnStoppedEvent = Nothing
        End Sub

        Public Function Start() As Integer
            If FDiscovering Then Return wclBluetoothErrors.WCL_E_BLUETOOTH_DISCOVERING_RUNNING
            If Count = 0 Then Return wclBluetoothErrors.WCL_E_BLUETOOTH_HARDWARE_NOT_AVAILABLE

            Dim Radio As wclBluetoothRadio = Nothing
            For i As Integer = 0 To Count - 1
                If Me(i).Available Then
                    Radio = Me(i)
                    Exit For
                End If
            Next

            If Radio Is Nothing Then Return wclBluetoothErrors.WCL_E_BLUETOOTH_DRIVER_NOT_AVAILABLE

            FDevices = New List(Of Int64)()
            FFoundDevices = New List(Of Int64)()

            Dim Res As Integer = Radio.Discover(10, wclBluetoothDiscoverKind.dkClassic)
            If Res <> wclErrors.WCL_E_SUCCESS Then
                FDevices = Nothing
                FFoundDevices = Nothing
            Else
                FDiscovering = True
                FRadio = Radio

                If OnStartedEvent IsNot Nothing Then OnStartedEvent(Me, EventArgs.Empty)
            End If

            Return Res
        End Function

        Public Function [Stop]() As Integer
            If Not FDiscovering Then Return wclBluetoothErrors.WCL_E_BLUETOOTH_DISCOVERING_NOT_RUNNING

            FDiscovering = False
            Dim Res As Integer = FRadio.Terminate()

            FDevices.Clear()
            FFoundDevices.Clear()

            FRadio = Nothing
            FDevices = Nothing
            FFoundDevices = Nothing

            If OnStoppedEvent IsNot Nothing Then OnStoppedEvent(Me, EventArgs.Empty)

            Return Res
        End Function

        Public ReadOnly Property Radio As wclBluetoothRadio
            Get
                Return FRadio
            End Get
        End Property

        Public Shadows Event OnDeviceFound As DeviceEvent
        Public Event OnDeviceLost As DeviceEvent
        Public Event OnStarted As EventHandler
        Public Event OnStopped As EventHandler
    End Class

    Private Sub fmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FDisc = New Discoverer()
        AddHandler FDisc.OnStarted, AddressOf FDisc_OnStarted
        AddHandler FDisc.OnStopped, AddressOf FDisc_OnStopped
        AddHandler FDisc.OnDeviceFound, AddressOf FDisc_OnDeviceFound
        AddHandler FDisc.OnDeviceLost, AddressOf FDisc_OnDeviceLost

        Dim Res As Integer = FDisc.Open()
        If Res <> wclErrors.WCL_E_SUCCESS Then lbLog.Items.Add("Open failed; 0x" + Res.ToString("X8"))
    End Sub

    Private Sub FDisc_OnDeviceLost(sender As Object, Address As Int64)
        lbLog.Items.Add("Device lost: " + Address.ToString("X12"))
        For Each Item As ListViewItem In lvDevices.Items
            If Item.Text = Address.ToString("X12") Then
                lvDevices.Items.Remove(Item)
                Exit For
            End If
        Next
    End Sub

    Private Sub FDisc_OnDeviceFound(sender As Object, Address As Int64)
        lbLog.Items.Add("Device found: " + Address.ToString("X12"))
        Dim Item As ListViewItem = lvDevices.Items.Add(Address.ToString("X12"))

        Dim Name As String = ""
        Dim Res As Int32 = FDisc.Radio.GetRemoteName(Address, Name)
        If Res <> wclErrors.WCL_E_SUCCESS Then Name = "Error: 0x" + Res.ToString("X8")

        Item.SubItems.Add(Name)
    End Sub

    Private Sub FDisc_OnStopped(sender As Object, e As EventArgs)
        lbLog.Items.Add("Stopped")

        lvDevices.Items.Clear()
    End Sub

    Private Sub FDisc_OnStarted(sender As Object, e As EventArgs)
        lbLog.Items.Add("Started")
    End Sub

    Private Sub fmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        FDisc.Stop()
        FDisc.Close()
        FDisc = Nothing
    End Sub

    Private Sub btStart_Click(sender As Object, e As EventArgs) Handles btStart.Click
        Dim Res As Integer = FDisc.Start()
        If Res <> wclErrors.WCL_E_SUCCESS Then
            lbLog.Items.Add("Start failed: 0x" + Res.ToString("X8"))
            FDisc.Close()
        End If
    End Sub

    Private Sub btStop_Click(sender As Object, e As EventArgs) Handles btStop.Click
        Dim Res As Integer = FDisc.Stop()
        If Res <> wclErrors.WCL_E_SUCCESS Then lbLog.Items.Add("Stop failed: 0x" + Res.ToString("X8"))
    End Sub
End Class
