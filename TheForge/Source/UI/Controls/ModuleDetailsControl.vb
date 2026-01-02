Imports System.Windows.Forms

Namespace UI.Controls
    ''' <summary>
    ''' UserControl for displaying module metadata details.
    ''' </summary>
    Public Class ModuleDetailsControl
        Inherits UserControl

        Private detailsTable As TableLayoutPanel
        Private lblModuleDetailsHeader As Label
        Private lblFileName As Label
        Private txtFileName As TextBox
        Private lblDisplayName As Label
        Private txtDisplayName As TextBox
        Private lblTypeName As Label
        Private txtTypeName As TextBox
        Private lblLastLoadedTime As Label
        Private txtLastLoadedTime As TextBox
        Private lblIsLoaded As Label
        Private txtIsLoaded As TextBox
        Private lblDependencies As Label
        Private txtDependencies As TextBox
        Private lblConfigPath As Label
        Private txtConfigPath As TextBox

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.SuspendLayout()

            detailsTable = New TableLayoutPanel()
            detailsTable.Name = "detailsTable"
            detailsTable.Dock = DockStyle.Fill
            detailsTable.ColumnCount = 1
            detailsTable.RowCount = 15
            detailsTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
            detailsTable.AutoScroll = True
            detailsTable.Padding = New Padding(10)

            lblModuleDetailsHeader = New Label()
            lblModuleDetailsHeader.Name = "lblModuleDetailsHeader"
            lblModuleDetailsHeader.Text = "Module Details"
            lblModuleDetailsHeader.Font = New Font(lblModuleDetailsHeader.Font, FontStyle.Bold)
            lblModuleDetailsHeader.Height = 30
            lblModuleDetailsHeader.Dock = DockStyle.Fill

            lblFileName = New Label()
            lblFileName.Name = "lblFileName"
            lblFileName.Text = "File Name:"
            lblFileName.Height = 20
            lblFileName.Dock = DockStyle.Fill

            txtFileName = New TextBox()
            txtFileName.Name = "txtFileName"
            txtFileName.ReadOnly = True
            txtFileName.Height = 23
            txtFileName.Dock = DockStyle.Fill

            lblDisplayName = New Label()
            lblDisplayName.Name = "lblDisplayName"
            lblDisplayName.Text = "Display Name:"
            lblDisplayName.Height = 20
            lblDisplayName.Dock = DockStyle.Fill

            txtDisplayName = New TextBox()
            txtDisplayName.Name = "txtDisplayName"
            txtDisplayName.ReadOnly = True
            txtDisplayName.Height = 23
            txtDisplayName.Dock = DockStyle.Fill

            lblTypeName = New Label()
            lblTypeName.Name = "lblTypeName"
            lblTypeName.Text = "Type Name:"
            lblTypeName.Height = 20
            lblTypeName.Dock = DockStyle.Fill

            txtTypeName = New TextBox()
            txtTypeName.Name = "txtTypeName"
            txtTypeName.ReadOnly = True
            txtTypeName.Multiline = True
            txtTypeName.Height = 60
            txtTypeName.Dock = DockStyle.Fill

            lblLastLoadedTime = New Label()
            lblLastLoadedTime.Name = "lblLastLoadedTime"
            lblLastLoadedTime.Text = "Last Loaded:"
            lblLastLoadedTime.Height = 20
            lblLastLoadedTime.Dock = DockStyle.Fill

            txtLastLoadedTime = New TextBox()
            txtLastLoadedTime.Name = "txtLastLoadedTime"
            txtLastLoadedTime.ReadOnly = True
            txtLastLoadedTime.Height = 23
            txtLastLoadedTime.Dock = DockStyle.Fill

            lblIsLoaded = New Label()
            lblIsLoaded.Name = "lblIsLoaded"
            lblIsLoaded.Text = "Is Loaded:"
            lblIsLoaded.Height = 20
            lblIsLoaded.Dock = DockStyle.Fill

            txtIsLoaded = New TextBox()
            txtIsLoaded.Name = "txtIsLoaded"
            txtIsLoaded.ReadOnly = True
            txtIsLoaded.Height = 23
            txtIsLoaded.Dock = DockStyle.Fill

            lblDependencies = New Label()
            lblDependencies.Name = "lblDependencies"
            lblDependencies.Text = "Dependencies:"
            lblDependencies.Height = 20
            lblDependencies.Dock = DockStyle.Fill

            txtDependencies = New TextBox()
            txtDependencies.Name = "txtDependencies"
            txtDependencies.ReadOnly = True
            txtDependencies.Multiline = True
            txtDependencies.Height = 60
            txtDependencies.Dock = DockStyle.Fill
            txtDependencies.ScrollBars = ScrollBars.Vertical

            lblConfigPath = New Label()
            lblConfigPath.Name = "lblConfigPath"
            lblConfigPath.Text = "Config Path:"
            lblConfigPath.Height = 20
            lblConfigPath.Dock = DockStyle.Fill

            txtConfigPath = New TextBox()
            txtConfigPath.Name = "txtConfigPath"
            txtConfigPath.ReadOnly = True
            txtConfigPath.Height = 23
            txtConfigPath.Dock = DockStyle.Fill

            detailsTable.Controls.Add(lblModuleDetailsHeader, 0, 0)
            detailsTable.Controls.Add(lblFileName, 0, 1)
            detailsTable.Controls.Add(txtFileName, 0, 2)
            detailsTable.Controls.Add(lblDisplayName, 0, 3)
            detailsTable.Controls.Add(txtDisplayName, 0, 4)
            detailsTable.Controls.Add(lblTypeName, 0, 5)
            detailsTable.Controls.Add(txtTypeName, 0, 6)
            detailsTable.Controls.Add(lblLastLoadedTime, 0, 7)
            detailsTable.Controls.Add(txtLastLoadedTime, 0, 8)
            detailsTable.Controls.Add(lblIsLoaded, 0, 9)
            detailsTable.Controls.Add(txtIsLoaded, 0, 10)
            detailsTable.Controls.Add(lblDependencies, 0, 11)
            detailsTable.Controls.Add(txtDependencies, 0, 12)
            detailsTable.Controls.Add(lblConfigPath, 0, 13)
            detailsTable.Controls.Add(txtConfigPath, 0, 14)

            Me.Controls.Add(detailsTable)

            Me.Name = "ModuleDetailsControl"
            Me.BackColor = Color.FromArgb(245, 245, 245)

            Me.ResumeLayout(False)
        End Sub

        ''' <summary>
        ''' Updates the details display with module metadata.
        ''' </summary>
        Public Sub UpdateDetails(moduleMetadata As Models.ModuleMetadata)
            If moduleMetadata Is Nothing Then
                ClearDetails()
                Return
            End If

            txtFileName.Text = If(moduleMetadata.FileName, "N/A")
            txtDisplayName.Text = If(moduleMetadata.DisplayName, "N/A")
            txtTypeName.Text = If(String.IsNullOrEmpty(moduleMetadata.TypeName), "N/A", moduleMetadata.TypeName)
            txtLastLoadedTime.Text = If(moduleMetadata.LastLoadedTime.HasValue, moduleMetadata.LastLoadedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), "Never")
            txtIsLoaded.Text = If(moduleMetadata.IsLoaded, "Yes", "No")

            If moduleMetadata.Dependencies IsNot Nothing AndAlso moduleMetadata.Dependencies.Length > 0 Then
                txtDependencies.Text = String.Join(Environment.NewLine, moduleMetadata.Dependencies)
            Else
                txtDependencies.Text = "None"
            End If

            Dim modulesDir As String = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules")
            Dim configPath As String = IO.Path.Combine(modulesDir, IO.Path.GetFileNameWithoutExtension(moduleMetadata.FileName) & ".config")
            txtConfigPath.Text = If(IO.File.Exists(configPath), configPath, "Not found")
        End Sub

        ''' <summary>
        ''' Clears all detail fields.
        ''' </summary>
        Public Sub ClearDetails()
            txtFileName.Text = String.Empty
            txtDisplayName.Text = String.Empty
            txtTypeName.Text = String.Empty
            txtLastLoadedTime.Text = String.Empty
            txtIsLoaded.Text = String.Empty
            txtDependencies.Text = String.Empty
            txtConfigPath.Text = String.Empty
        End Sub
    End Class
End Namespace
