<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DashboardMainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        moduleListControl = New UI.Controls.ModuleListControl()
        splitterVertical = New Splitter()
        testAreaControl = New UI.Controls.TestAreaControl()
        splitterHorizontal = New Splitter()
        logOutputControl = New UI.Controls.LogOutputControl()
        splitterVerticalRight = New Splitter()
        moduleDetailsControl = New UI.Controls.ModuleDetailsControl()
        MainStatusStrip = New StatusStrip()
        StatusLabel = New ToolStripStatusLabel()
        MainStatusStrip.SuspendLayout()
        SuspendLayout()
        ' 
        ' moduleListControl
        ' 
        moduleListControl.BackColor = Color.FromArgb(CByte(240), CByte(240), CByte(240))
        moduleListControl.Dock = DockStyle.Left
        moduleListControl.Location = New Point(0, 0)
        moduleListControl.Name = "moduleListControl"
        moduleListControl.Size = New Size(254, 886)
        moduleListControl.TabIndex = 0
        ' 
        ' splitterVertical
        ' 
        splitterVertical.Location = New Point(254, 0)
        splitterVertical.Name = "splitterVertical"
        splitterVertical.Size = New Size(3, 886)
        splitterVertical.TabIndex = 1
        splitterVertical.TabStop = False
        ' 
        ' testAreaControl
        ' 
        testAreaControl.BackColor = Color.White
        testAreaControl.Dock = DockStyle.Fill
        testAreaControl.Location = New Point(257, 0)
        testAreaControl.Name = "testAreaControl"
        testAreaControl.Size = New Size(738, 557)
        testAreaControl.TabIndex = 2
        ' 
        ' splitterHorizontal
        ' 
        splitterHorizontal.Dock = DockStyle.Bottom
        splitterHorizontal.Location = New Point(257, 557)
        splitterHorizontal.Name = "splitterHorizontal"
        splitterHorizontal.Size = New Size(1035, 3)
        splitterHorizontal.TabIndex = 3
        splitterHorizontal.TabStop = False
        ' 
        ' logOutputControl
        ' 
        logOutputControl.BackColor = Color.FromArgb(CByte(250), CByte(250), CByte(250))
        logOutputControl.Dock = DockStyle.Bottom
        logOutputControl.Location = New Point(257, 560)
        logOutputControl.Name = "logOutputControl"
        logOutputControl.Size = New Size(1035, 326)
        logOutputControl.TabIndex = 4
        ' 
        ' splitterVerticalRight
        ' 
        splitterVerticalRight.Dock = DockStyle.Right
        splitterVerticalRight.Location = New Point(995, 0)
        splitterVerticalRight.Name = "splitterVerticalRight"
        splitterVerticalRight.Size = New Size(3, 557)
        splitterVerticalRight.TabIndex = 5
        splitterVerticalRight.TabStop = False
        ' 
        ' moduleDetailsControl
        ' 
        moduleDetailsControl.AutoScroll = True
        moduleDetailsControl.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        moduleDetailsControl.Dock = DockStyle.Right
        moduleDetailsControl.Location = New Point(998, 0)
        moduleDetailsControl.Name = "moduleDetailsControl"
        moduleDetailsControl.Size = New Size(294, 557)
        moduleDetailsControl.TabIndex = 6
        ' 
        ' MainStatusStrip
        ' 
        MainStatusStrip.ImageScalingSize = New Size(20, 20)
        MainStatusStrip.Items.AddRange(New ToolStripItem() {StatusLabel})
        MainStatusStrip.Location = New Point(0, 886)
        MainStatusStrip.Name = "MainStatusStrip"
        MainStatusStrip.Size = New Size(1292, 26)
        MainStatusStrip.TabIndex = 7
        MainStatusStrip.Text = "StatusStrip"
        ' 
        ' StatusLabel
        ' 
        StatusLabel.Name = "StatusLabel"
        StatusLabel.Size = New Size(50, 20)
        StatusLabel.Text = "Ready"
        ' 
        ' DashboardMainForm
        ' 
        ClientSize = New Size(1292, 912)
        Controls.Add(testAreaControl)
        Controls.Add(splitterVerticalRight)
        Controls.Add(moduleDetailsControl)
        Controls.Add(splitterHorizontal)
        Controls.Add(logOutputControl)
        Controls.Add(splitterVertical)
        Controls.Add(moduleListControl)
        Controls.Add(MainStatusStrip)
        MinimumSize = New Size(800, 600)
        Name = "DashboardMainForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "RCH Forge Dashboard"
        MainStatusStrip.ResumeLayout(False)
        MainStatusStrip.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents moduleListControl As UI.Controls.ModuleListControl
    Friend splitterVertical As System.Windows.Forms.Splitter
    Friend testAreaControl As UI.Controls.TestAreaControl
    Friend splitterHorizontal As System.Windows.Forms.Splitter
    Friend WithEvents logOutputControl As UI.Controls.LogOutputControl
    Friend splitterVerticalRight As System.Windows.Forms.Splitter
    Friend moduleDetailsControl As UI.Controls.ModuleDetailsControl
    Friend MainStatusStrip As System.Windows.Forms.StatusStrip
    Friend StatusLabel As System.Windows.Forms.ToolStripStatusLabel

End Class
