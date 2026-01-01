Imports System.Windows.Forms

''' <summary>
''' Main form for the RCH.Forge.Dashboard application.
''' Provides a test harness for loading and testing Forge modules.
''' </summary>
Public Class DashboardMainForm
    Inherits Form

    Private pnlModuleList As Panel
    Private pnlLogOutput As Panel
    Private pnlTestArea As Panel
    Private splitterVertical As Splitter
    Private splitterHorizontal As Splitter

    Public Sub New()
        InitializeComponent()
        InitializeLayout()
    End Sub

    Private Sub InitializeLayout()
        pnlModuleList = New Panel()
        pnlModuleList.Dock = DockStyle.Left
        pnlModuleList.Width = 250
        pnlModuleList.BackColor = Color.FromArgb(240, 240, 240)
        
        splitterVertical = New Splitter()
        splitterVertical.Dock = DockStyle.Left
        splitterVertical.Width = 3
        
        pnlLogOutput = New Panel()
        pnlLogOutput.Dock = DockStyle.Bottom
        pnlLogOutput.Height = 200
        pnlLogOutput.BackColor = Color.FromArgb(250, 250, 250)
        
        splitterHorizontal = New Splitter()
        splitterHorizontal.Dock = DockStyle.Bottom
        splitterHorizontal.Height = 3
        
        pnlTestArea = New Panel()
        pnlTestArea.Dock = DockStyle.Fill
        pnlTestArea.BackColor = Color.White
        
        Me.Controls.Add(pnlTestArea)
        Me.Controls.Add(splitterHorizontal)
        Me.Controls.Add(pnlLogOutput)
        Me.Controls.Add(splitterVertical)
        Me.Controls.Add(pnlModuleList)
    End Sub

End Class
