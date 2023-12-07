<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lvDevices = New System.Windows.Forms.ListView()
        Me.chDeviceAddress = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chDeviceName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btStop = New System.Windows.Forms.Button()
        Me.btStart = New System.Windows.Forms.Button()
        Me.lbLog = New System.Windows.Forms.ListBox()
        Me.btClear = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lvDevices
        '
        Me.lvDevices.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chDeviceAddress, Me.chDeviceName})
        Me.lvDevices.FullRowSelect = True
        Me.lvDevices.Location = New System.Drawing.Point(12, 43)
        Me.lvDevices.MultiSelect = False
        Me.lvDevices.Name = "lvDevices"
        Me.lvDevices.Size = New System.Drawing.Size(447, 174)
        Me.lvDevices.TabIndex = 7
        Me.lvDevices.UseCompatibleStateImageBehavior = False
        Me.lvDevices.View = System.Windows.Forms.View.Details
        '
        'chDeviceAddress
        '
        Me.chDeviceAddress.Text = "Address"
        Me.chDeviceAddress.Width = 200
        '
        'chDeviceName
        '
        Me.chDeviceName.Text = "Name"
        Me.chDeviceName.Width = 200
        '
        'btStop
        '
        Me.btStop.Location = New System.Drawing.Point(93, 14)
        Me.btStop.Name = "btStop"
        Me.btStop.Size = New System.Drawing.Size(75, 23)
        Me.btStop.TabIndex = 6
        Me.btStop.Text = "Stop"
        Me.btStop.UseVisualStyleBackColor = True
        '
        'btStart
        '
        Me.btStart.Location = New System.Drawing.Point(12, 14)
        Me.btStart.Name = "btStart"
        Me.btStart.Size = New System.Drawing.Size(75, 23)
        Me.btStart.TabIndex = 5
        Me.btStart.Text = "Start"
        Me.btStart.UseVisualStyleBackColor = True
        '
        'lbLog
        '
        Me.lbLog.FormattingEnabled = True
        Me.lbLog.Location = New System.Drawing.Point(12, 252)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.Size = New System.Drawing.Size(447, 199)
        Me.lbLog.TabIndex = 9
        '
        'btClear
        '
        Me.btClear.Location = New System.Drawing.Point(384, 223)
        Me.btClear.Name = "btClear"
        Me.btClear.Size = New System.Drawing.Size(75, 23)
        Me.btClear.TabIndex = 8
        Me.btClear.Text = "Clear"
        Me.btClear.UseVisualStyleBackColor = True
        '
        'fmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(471, 464)
        Me.Controls.Add(Me.lvDevices)
        Me.Controls.Add(Me.btStop)
        Me.Controls.Add(Me.btStart)
        Me.Controls.Add(Me.lbLog)
        Me.Controls.Add(Me.btClear)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "fmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Continue Discovering Demo"
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents lvDevices As ListView
    Private WithEvents chDeviceAddress As ColumnHeader
    Private WithEvents chDeviceName As ColumnHeader
    Private WithEvents btStop As Button
    Private WithEvents btStart As Button
    Private WithEvents lbLog As ListBox
    Private WithEvents btClear As Button
End Class
