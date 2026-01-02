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
            lblModuleDetailsHeader = New Label()
            lblFileName = New Label()
            txtFileName = New TextBox()
            lblDisplayName = New Label()
            txtDisplayName = New TextBox()
            lblTypeName = New Label()
            txtTypeName = New TextBox()
            lblLastLoadedTime = New Label()
            txtLastLoadedTime = New TextBox()
            lblIsLoaded = New Label()
            txtIsLoaded = New TextBox()
            lblDependencies = New Label()
            txtDependencies = New TextBox()
            lblConfigPath = New Label()
            txtConfigPath = New TextBox()
            SuspendLayout()
            ' 
            ' lblModuleDetailsHeader
            ' 
            lblModuleDetailsHeader.AutoSize = True
            lblModuleDetailsHeader.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
            lblModuleDetailsHeader.Location = New Point(10, 10)
            lblModuleDetailsHeader.Name = "lblModuleDetailsHeader"
            lblModuleDetailsHeader.Size = New Size(114, 20)
            lblModuleDetailsHeader.TabIndex = 0
            lblModuleDetailsHeader.Text = "Module Details"
            ' 
            ' lblFileName
            ' 
            lblFileName.AutoSize = True
            lblFileName.Location = New Point(10, 40)
            lblFileName.Name = "lblFileName"
            lblFileName.Size = New Size(79, 20)
            lblFileName.TabIndex = 1
            lblFileName.Text = "File Name:"
            ' 
            ' txtFileName
            ' 
            txtFileName.Location = New Point(10, 63)
            txtFileName.Name = "txtFileName"
            txtFileName.ReadOnly = True
            txtFileName.Size = New Size(280, 27)
            txtFileName.TabIndex = 2
            ' 
            ' lblDisplayName
            ' 
            lblDisplayName.AutoSize = True
            lblDisplayName.Location = New Point(10, 100)
            lblDisplayName.Name = "lblDisplayName"
            lblDisplayName.Size = New Size(105, 20)
            lblDisplayName.TabIndex = 3
            lblDisplayName.Text = "Display Name:"
            ' 
            ' txtDisplayName
            ' 
            txtDisplayName.Location = New Point(10, 123)
            txtDisplayName.Name = "txtDisplayName"
            txtDisplayName.ReadOnly = True
            txtDisplayName.Size = New Size(280, 27)
            txtDisplayName.TabIndex = 4
            ' 
            ' lblTypeName
            ' 
            lblTypeName.AutoSize = True
            lblTypeName.Location = New Point(10, 160)
            lblTypeName.Name = "lblTypeName"
            lblTypeName.Size = New Size(87, 20)
            lblTypeName.TabIndex = 5
            lblTypeName.Text = "Type Name:"
            ' 
            ' txtTypeName
            ' 
            txtTypeName.Location = New Point(10, 183)
            txtTypeName.Multiline = True
            txtTypeName.Name = "txtTypeName"
            txtTypeName.ReadOnly = True
            txtTypeName.Size = New Size(280, 60)
            txtTypeName.TabIndex = 6
            ' 
            ' lblLastLoadedTime
            ' 
            lblLastLoadedTime.AutoSize = True
            lblLastLoadedTime.Location = New Point(10, 253)
            lblLastLoadedTime.Name = "lblLastLoadedTime"
            lblLastLoadedTime.Size = New Size(92, 20)
            lblLastLoadedTime.TabIndex = 7
            lblLastLoadedTime.Text = "Last Loaded:"
            ' 
            ' txtLastLoadedTime
            ' 
            txtLastLoadedTime.Location = New Point(10, 276)
            txtLastLoadedTime.Name = "txtLastLoadedTime"
            txtLastLoadedTime.ReadOnly = True
            txtLastLoadedTime.Size = New Size(280, 27)
            txtLastLoadedTime.TabIndex = 8
            ' 
            ' lblIsLoaded
            ' 
            lblIsLoaded.AutoSize = True
            lblIsLoaded.Location = New Point(10, 313)
            lblIsLoaded.Name = "lblIsLoaded"
            lblIsLoaded.Size = New Size(76, 20)
            lblIsLoaded.TabIndex = 9
            lblIsLoaded.Text = "Is Loaded:"
            ' 
            ' txtIsLoaded
            ' 
            txtIsLoaded.Location = New Point(10, 336)
            txtIsLoaded.Name = "txtIsLoaded"
            txtIsLoaded.ReadOnly = True
            txtIsLoaded.Size = New Size(280, 27)
            txtIsLoaded.TabIndex = 10
            ' 
            ' lblDependencies
            ' 
            lblDependencies.AutoSize = True
            lblDependencies.Location = New Point(10, 373)
            lblDependencies.Name = "lblDependencies"
            lblDependencies.Size = New Size(106, 20)
            lblDependencies.TabIndex = 11
            lblDependencies.Text = "Dependencies:"
            ' 
            ' txtDependencies
            ' 
            txtDependencies.Location = New Point(10, 396)
            txtDependencies.Multiline = True
            txtDependencies.Name = "txtDependencies"
            txtDependencies.ReadOnly = True
            txtDependencies.ScrollBars = ScrollBars.Vertical
            txtDependencies.Size = New Size(280, 60)
            txtDependencies.TabIndex = 12
            ' 
            ' lblConfigPath
            ' 
            lblConfigPath.AutoSize = True
            lblConfigPath.Location = New Point(10, 466)
            lblConfigPath.Name = "lblConfigPath"
            lblConfigPath.Size = New Size(88, 20)
            lblConfigPath.TabIndex = 13
            lblConfigPath.Text = "Config Path:"
            ' 
            ' txtConfigPath
            ' 
            txtConfigPath.Location = New Point(10, 489)
            txtConfigPath.Name = "txtConfigPath"
            txtConfigPath.ReadOnly = True
            txtConfigPath.Size = New Size(280, 27)
            txtConfigPath.TabIndex = 14
            ' 
            ' ModuleDetailsControl
            ' 
            AutoScaleDimensions = New SizeF(8F, 20F)
            AutoScaleMode = AutoScaleMode.Font
            AutoScroll = True
            BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
            Controls.Add(txtConfigPath)
            Controls.Add(lblConfigPath)
            Controls.Add(txtDependencies)
            Controls.Add(lblDependencies)
            Controls.Add(txtIsLoaded)
            Controls.Add(lblIsLoaded)
            Controls.Add(txtLastLoadedTime)
            Controls.Add(lblLastLoadedTime)
            Controls.Add(txtTypeName)
            Controls.Add(lblTypeName)
            Controls.Add(txtDisplayName)
            Controls.Add(lblDisplayName)
            Controls.Add(txtFileName)
            Controls.Add(lblFileName)
            Controls.Add(lblModuleDetailsHeader)
            Name = "ModuleDetailsControl"
            Size = New Size(300, 530)
            ResumeLayout(False)
            PerformLayout()
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
