Public Class fmMain
    Private FDisc As Discoverer

    Private Sub btClear_Click(sender As Object, e As EventArgs) Handles btClear.Click
        lbLog.Items.Clear()
    End Sub

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
