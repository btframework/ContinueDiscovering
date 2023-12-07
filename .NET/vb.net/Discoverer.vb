Friend Delegate Sub DeviceEvent(sender As Object, Address As Int64)

Friend Class Discoverer
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
        FRadio = Nothing

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