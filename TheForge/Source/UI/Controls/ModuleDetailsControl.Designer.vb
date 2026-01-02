Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace UI.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ModuleDetailsControl
        Inherits UserControl

        'UserControl overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
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
        Private components As IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.lblModuleDetailsHeader = New System.Windows.Forms.Label()
            Me.lblFileName = New System.Windows.Forms.Label()
            Me.txtFileName = New System.Windows.Forms.TextBox()
            Me.lblDisplayName = New System.Windows.Forms.Label()
            Me.txtDisplayName = New System.Windows.Forms.TextBox()
            Me.lblTypeName = New System.Windows.Forms.Label()
            Me.txtTypeName = New System.Windows.Forms.TextBox()
            Me.lblLastLoadedTime = New System.Windows.Forms.Label()
            Me.txtLastLoadedTime = New System.Windows.Forms.TextBox()
            Me.lblIsLoaded = New System.Windows.Forms.Label()
            Me.txtIsLoaded = New System.Windows.Forms.TextBox()
            Me.lblDependencies = New System.Windows.Forms.Label()
            Me.txtDependencies = New System.Windows.Forms.TextBox()
            Me.lblConfigPath = New System.Windows.Forms.Label()
            Me.txtConfigPath = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'lblModuleDetailsHeader
            '
            Me.lblModuleDetailsHeader.AutoSize = True
            Me.lblModuleDetailsHeader.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
            Me.lblModuleDetailsHeader.Location = New System.Drawing.Point(10, 10)
            Me.lblModuleDetailsHeader.Name = "lblModuleDetailsHeader"
            Me.lblModuleDetailsHeader.Size = New System.Drawing.Size(114, 20)
            Me.lblModuleDetailsHeader.TabIndex = 0
            Me.lblModuleDetailsHeader.Text = "Module Details"
            '
            'lblFileName
            '
            Me.lblFileName.AutoSize = True
            Me.lblFileName.Location = New System.Drawing.Point(10, 40)
            Me.lblFileName.Name = "lblFileName"
            Me.lblFileName.Size = New System.Drawing.Size(76, 20)
            Me.lblFileName.TabIndex = 1
            Me.lblFileName.Text = "File Name:"
            '
            'txtFileName
            '
            Me.txtFileName.Location = New System.Drawing.Point(10, 63)
            Me.txtFileName.Name = "txtFileName"
            Me.txtFileName.ReadOnly = True
            Me.txtFileName.Size = New System.Drawing.Size(280, 27)
            Me.txtFileName.TabIndex = 2
            '
            'lblDisplayName
            '
            Me.lblDisplayName.AutoSize = True
            Me.lblDisplayName.Location = New System.Drawing.Point(10, 100)
            Me.lblDisplayName.Name = "lblDisplayName"
            Me.lblDisplayName.Size = New System.Drawing.Size(104, 20)
            Me.lblDisplayName.TabIndex = 3
            Me.lblDisplayName.Text = "Display Name:"
            '
            'txtDisplayName
            '
            Me.txtDisplayName.Location = New System.Drawing.Point(10, 123)
            Me.txtDisplayName.Name = "txtDisplayName"
            Me.txtDisplayName.ReadOnly = True
            Me.txtDisplayName.Size = New System.Drawing.Size(280, 27)
            Me.txtDisplayName.TabIndex = 4
            '
            'lblTypeName
            '
            Me.lblTypeName.AutoSize = True
            Me.lblTypeName.Location = New System.Drawing.Point(10, 160)
            Me.lblTypeName.Name = "lblTypeName"
            Me.lblTypeName.Size = New System.Drawing.Size(87, 20)
            Me.lblTypeName.TabIndex = 5
            Me.lblTypeName.Text = "Type Name:"
            '
            'txtTypeName
            '
            Me.txtTypeName.Location = New System.Drawing.Point(10, 183)
            Me.txtTypeName.Multiline = True
            Me.txtTypeName.Name = "txtTypeName"
            Me.txtTypeName.ReadOnly = True
            Me.txtTypeName.Size = New System.Drawing.Size(280, 60)
            Me.txtTypeName.TabIndex = 6
            '
            'lblLastLoadedTime
            '
            Me.lblLastLoadedTime.AutoSize = True
            Me.lblLastLoadedTime.Location = New System.Drawing.Point(10, 253)
            Me.lblLastLoadedTime.Name = "lblLastLoadedTime"
            Me.lblLastLoadedTime.Size = New System.Drawing.Size(98, 20)
            Me.lblLastLoadedTime.TabIndex = 7
            Me.lblLastLoadedTime.Text = "Last Loaded:"
            '
            'txtLastLoadedTime
            '
            Me.txtLastLoadedTime.Location = New System.Drawing.Point(10, 276)
            Me.txtLastLoadedTime.Name = "txtLastLoadedTime"
            Me.txtLastLoadedTime.ReadOnly = True
            Me.txtLastLoadedTime.Size = New System.Drawing.Size(280, 27)
            Me.txtLastLoadedTime.TabIndex = 8
            '
            'lblIsLoaded
            '
            Me.lblIsLoaded.AutoSize = True
            Me.lblIsLoaded.Location = New System.Drawing.Point(10, 313)
            Me.lblIsLoaded.Name = "lblIsLoaded"
            Me.lblIsLoaded.Size = New System.Drawing.Size(75, 20)
            Me.lblIsLoaded.TabIndex = 9
            Me.lblIsLoaded.Text = "Is Loaded:"
            '
            'txtIsLoaded
            '
            Me.txtIsLoaded.Location = New System.Drawing.Point(10, 336)
            Me.txtIsLoaded.Name = "txtIsLoaded"
            Me.txtIsLoaded.ReadOnly = True
            Me.txtIsLoaded.Size = New System.Drawing.Size(280, 27)
            Me.txtIsLoaded.TabIndex = 10
            '
            'lblDependencies
            '
            Me.lblDependencies.AutoSize = True
            Me.lblDependencies.Location = New System.Drawing.Point(10, 373)
            Me.lblDependencies.Name = "lblDependencies"
            Me.lblDependencies.Size = New System.Drawing.Size(102, 20)
            Me.lblDependencies.TabIndex = 11
            Me.lblDependencies.Text = "Dependencies:"
            '
            'txtDependencies
            '
            Me.txtDependencies.Location = New System.Drawing.Point(10, 396)
            Me.txtDependencies.Multiline = True
            Me.txtDependencies.Name = "txtDependencies"
            Me.txtDependencies.ReadOnly = True
            Me.txtDependencies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtDependencies.Size = New System.Drawing.Size(280, 60)
            Me.txtDependencies.TabIndex = 12
            '
            'lblConfigPath
            '
            Me.lblConfigPath.AutoSize = True
            Me.lblConfigPath.Location = New System.Drawing.Point(10, 466)
            Me.lblConfigPath.Name = "lblConfigPath"
            Me.lblConfigPath.Size = New System.Drawing.Size(90, 20)
            Me.lblConfigPath.TabIndex = 13
            Me.lblConfigPath.Text = "Config Path:"
            '
            'txtConfigPath
            '
            Me.txtConfigPath.Location = New System.Drawing.Point(10, 489)
            Me.txtConfigPath.Name = "txtConfigPath"
            Me.txtConfigPath.ReadOnly = True
            Me.txtConfigPath.Size = New System.Drawing.Size(280, 27)
            Me.txtConfigPath.TabIndex = 14
            '
            'ModuleDetailsControl
            '
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoScroll = True
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.Controls.Add(Me.txtConfigPath)
            Me.Controls.Add(Me.lblConfigPath)
            Me.Controls.Add(Me.txtDependencies)
            Me.Controls.Add(Me.lblDependencies)
            Me.Controls.Add(Me.txtIsLoaded)
            Me.Controls.Add(Me.lblIsLoaded)
            Me.Controls.Add(Me.txtLastLoadedTime)
            Me.Controls.Add(Me.lblLastLoadedTime)
            Me.Controls.Add(Me.txtTypeName)
            Me.Controls.Add(Me.lblTypeName)
            Me.Controls.Add(Me.txtDisplayName)
            Me.Controls.Add(Me.lblDisplayName)
            Me.Controls.Add(Me.txtFileName)
            Me.Controls.Add(Me.lblFileName)
            Me.Controls.Add(Me.lblModuleDetailsHeader)
            Me.Name = "ModuleDetailsControl"
            Me.Size = New System.Drawing.Size(300, 530)
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        Friend lblModuleDetailsHeader As Label
        Friend lblFileName As Label
        Friend txtFileName As TextBox
        Friend lblDisplayName As Label
        Friend txtDisplayName As TextBox
        Friend lblTypeName As Label
        Friend txtTypeName As TextBox
        Friend lblLastLoadedTime As Label
        Friend txtLastLoadedTime As TextBox
        Friend lblIsLoaded As Label
        Friend txtIsLoaded As TextBox
        Friend lblDependencies As Label
        Friend txtDependencies As TextBox
        Friend lblConfigPath As Label
        Friend txtConfigPath As TextBox
    End Class
End Namespace
