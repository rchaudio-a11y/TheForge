Imports System.Windows.Forms

Public Class DashboardMainForm
    Inherits Form

    Private pnlModuleList As Panel
    Private pnlLogOutput As Panel
    Private pnlTestArea As Panel
    Private pnlModuleDetails As Panel
    Private pnlLogFilter As Panel
    Private splitterVertical As Splitter
    Private splitterHorizontal As Splitter
    Private splitterVerticalRight As Splitter
    Private lstModules As ListBox
    Private txtLogOutput As TextBox
    Private MainStatusStrip As StatusStrip
    Private StatusLabel As ToolStripStatusLabel
    Private btnRunModule As Button
    Private btnUnloadModule As Button
    Private btnReloadModule As Button
    Private btnRefreshModules As Button
    Private btnClearLog As Button
    Private btnApplyFilter As Button
    Private cmbLogLevel As ComboBox
    Private txtLogSearch As TextBox
    Private lblLogLevel As Label
    Private lblLogSearch As Label
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
    Private _moduleLoaderService As Services.Interfaces.IModuleLoaderService
    Private _loggingService As Services.Interfaces.ILoggingService
    Private _discoveredModules As List(Of Models.ModuleMetadata)
    Private _currentModule As Modules.Interfaces.IModule

    Public Sub New()
        InitializeComponent()
        InitializeLayout()
        InitializeControls()
        InitializeServices()
    End Sub

    Private Sub InitializeLayout()
        MainStatusStrip = New StatusStrip()
        MainStatusStrip.Name = "MainStatusStrip"
        MainStatusStrip.Dock = DockStyle.Bottom

        StatusLabel = New ToolStripStatusLabel()
        StatusLabel.Name = "StatusLabel"
        StatusLabel.Text = "Ready"
        MainStatusStrip.Items.Add(StatusLabel)

        pnlModuleList = New Panel()
        pnlModuleList.Name = "pnlModuleList"
        pnlModuleList.Dock = DockStyle.Left
        pnlModuleList.Width = 250
        pnlModuleList.BackColor = Color.FromArgb(240, 240, 240)

        lstModules = New ListBox()
        lstModules.Name = "lstModules"
        lstModules.Dock = DockStyle.Fill

        btnRefreshModules = New Button()
        btnRefreshModules.Name = "btnRefreshModules"
        btnRefreshModules.Text = "Refresh Modules"
        btnRefreshModules.Dock = DockStyle.Bottom
        btnRefreshModules.Height = 35

        btnReloadModule = New Button()
        btnReloadModule.Name = "btnReloadModule"
        btnReloadModule.Text = "Reload Module"
        btnReloadModule.Dock = DockStyle.Bottom
        btnReloadModule.Height = 35
        btnReloadModule.Enabled = False

        btnUnloadModule = New Button()
        btnUnloadModule.Name = "btnUnloadModule"
        btnUnloadModule.Text = "Unload Module"
        btnUnloadModule.Dock = DockStyle.Bottom
        btnUnloadModule.Height = 35
        btnUnloadModule.Enabled = False

        btnRunModule = New Button()
        btnRunModule.Name = "btnRunModule"
        btnRunModule.Text = "Run Module"
        btnRunModule.Dock = DockStyle.Bottom
        btnRunModule.Height = 35
        btnRunModule.Enabled = False

        pnlModuleList.Controls.Add(lstModules)
        pnlModuleList.Controls.Add(btnRefreshModules)
        pnlModuleList.Controls.Add(btnReloadModule)
        pnlModuleList.Controls.Add(btnUnloadModule)
        pnlModuleList.Controls.Add(btnRunModule)

        splitterVertical = New Splitter()
        splitterVertical.Name = "splitterVertical"
        splitterVertical.Dock = DockStyle.Left
        splitterVertical.Width = 3

        pnlModuleDetails = New Panel()
        pnlModuleDetails.Name = "pnlModuleDetails"
        pnlModuleDetails.Dock = DockStyle.Right
        pnlModuleDetails.Width = 300
        pnlModuleDetails.BackColor = Color.FromArgb(245, 245, 245)
        pnlModuleDetails.Padding = New Padding(10)

        Dim detailsTable As New TableLayoutPanel()
        detailsTable.Name = "detailsTable"
        detailsTable.Dock = DockStyle.Fill
        detailsTable.ColumnCount = 1
        detailsTable.RowCount = 11
        detailsTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        detailsTable.AutoScroll = True

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

        pnlModuleDetails.Controls.Add(detailsTable)

        splitterVerticalRight = New Splitter()
        splitterVerticalRight.Name = "splitterVerticalRight"
        splitterVerticalRight.Dock = DockStyle.Right
        splitterVerticalRight.Width = 3

        pnlLogOutput = New Panel()
        pnlLogOutput.Name = "pnlLogOutput"
        pnlLogOutput.Dock = DockStyle.Bottom
        pnlLogOutput.Height = 200
        pnlLogOutput.BackColor = Color.FromArgb(250, 250, 250)

        txtLogOutput = New TextBox()
        txtLogOutput.Name = "txtLogOutput"
        txtLogOutput.Dock = DockStyle.Fill
        txtLogOutput.Multiline = True
        txtLogOutput.ReadOnly = True
        txtLogOutput.ScrollBars = ScrollBars.Vertical

        pnlLogFilter = New Panel()
        pnlLogFilter.Name = "pnlLogFilter"
        pnlLogFilter.Dock = DockStyle.Bottom
        pnlLogFilter.Height = 70
        pnlLogFilter.Padding = New Padding(5)

        Dim filterTable As New TableLayoutPanel()
        filterTable.Name = "filterTable"
        filterTable.Dock = DockStyle.Fill
        filterTable.ColumnCount = 2
        filterTable.RowCount = 3
        filterTable.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
        filterTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        lblLogLevel = New Label()
        lblLogLevel.Name = "lblLogLevel"
        lblLogLevel.Text = "Level:"
        lblLogLevel.Dock = DockStyle.Fill
        lblLogLevel.TextAlign = ContentAlignment.MiddleLeft

        cmbLogLevel = New ComboBox()
        cmbLogLevel.Name = "cmbLogLevel"
        cmbLogLevel.Dock = DockStyle.Fill
        cmbLogLevel.DropDownStyle = ComboBoxStyle.DropDownList
        cmbLogLevel.Items.Add("All")
        cmbLogLevel.Items.Add("Info")
        cmbLogLevel.Items.Add("Warning")
        cmbLogLevel.Items.Add("Error")
        cmbLogLevel.SelectedIndex = 0

        lblLogSearch = New Label()
        lblLogSearch.Name = "lblLogSearch"
        lblLogSearch.Text = "Search:"
        lblLogSearch.Dock = DockStyle.Fill
        lblLogSearch.TextAlign = ContentAlignment.MiddleLeft

        txtLogSearch = New TextBox()
        txtLogSearch.Name = "txtLogSearch"
        txtLogSearch.Dock = DockStyle.Fill

        btnApplyFilter = New Button()
        btnApplyFilter.Name = "btnApplyFilter"
        btnApplyFilter.Text = "Apply Filter"
        btnApplyFilter.Dock = DockStyle.Fill
        btnApplyFilter.Height = 25

        btnClearLog = New Button()
        btnClearLog.Name = "btnClearLog"
        btnClearLog.Text = "Clear Log"
        btnClearLog.Dock = DockStyle.Fill
        btnClearLog.Height = 25

        filterTable.Controls.Add(lblLogLevel, 0, 0)
        filterTable.Controls.Add(cmbLogLevel, 1, 0)
        filterTable.Controls.Add(lblLogSearch, 0, 1)
        filterTable.Controls.Add(txtLogSearch, 1, 1)
        filterTable.Controls.Add(btnApplyFilter, 0, 2)
        filterTable.Controls.Add(btnClearLog, 1, 2)

        pnlLogFilter.Controls.Add(filterTable)

        pnlLogOutput.Controls.Add(txtLogOutput)
        pnlLogOutput.Controls.Add(pnlLogFilter)

        splitterHorizontal = New Splitter()
        splitterHorizontal.Name = "splitterHorizontal"
        splitterHorizontal.Dock = DockStyle.Bottom
        splitterHorizontal.Height = 3

        pnlTestArea = New Panel()
        pnlTestArea.Name = "pnlTestArea"
        pnlTestArea.Dock = DockStyle.Fill
        pnlTestArea.BackColor = Color.White

        Me.Controls.Add(pnlTestArea)
        Me.Controls.Add(splitterVerticalRight)
        Me.Controls.Add(pnlModuleDetails)
        Me.Controls.Add(splitterHorizontal)
        Me.Controls.Add(pnlLogOutput)
        Me.Controls.Add(splitterVertical)
        Me.Controls.Add(pnlModuleList)
        Me.Controls.Add(MainStatusStrip)
    End Sub

    Private Sub InitializeControls()
        StatusLabel.Text = "Ready"
        AddHandler Me.Load, AddressOf DashboardMainForm_Load
        AddHandler lstModules.SelectedIndexChanged, AddressOf lstModules_SelectedIndexChanged
        AddHandler btnRunModule.Click, AddressOf btnRunModule_Click
        AddHandler btnUnloadModule.Click, AddressOf btnUnloadModule_Click
        AddHandler btnReloadModule.Click, AddressOf btnReloadModule_Click
        AddHandler btnRefreshModules.Click, AddressOf btnRefreshModules_Click
        AddHandler btnClearLog.Click, AddressOf btnClearLog_Click
        AddHandler btnApplyFilter.Click, AddressOf btnApplyFilter_Click
        ClearModuleDetails()
    End Sub

    Private Sub InitializeServices()
        _loggingService = New Services.Implementations.LoggingService()
        _moduleLoaderService = New Services.Implementations.ModuleLoaderService(_loggingService)
        _discoveredModules = New List(Of Models.ModuleMetadata)()
        _currentModule = Nothing
        AddHandler _loggingService.LogMessageReceived, AddressOf LoggingService_LogMessageReceived
    End Sub

    Private Sub DashboardMainForm_Load(sender As Object, e As EventArgs)
        _loggingService.LogInfo("Dashboard started")
        LoadAvailableModules()
    End Sub

    Private Sub LoadAvailableModules()
        _loggingService.LogInfo("Discovering modules...")
        
        Try
            _discoveredModules = _moduleLoaderService.DiscoverModules()

            lstModules.Items.Clear()

            For Each moduleMetadata As Models.ModuleMetadata In _discoveredModules
                lstModules.Items.Add(moduleMetadata.DisplayName)
            Next

            Dim message As String = String.Format("Discovered {0} module(s)", _discoveredModules.Count)
            StatusLabel.Text = message
            _loggingService.LogInfo(message)
        Catch ex As Exception
            _loggingService.LogError(String.Format("Error discovering modules: {0}", ex.Message))
            StatusLabel.Text = "Error during module discovery"
        End Try
    End Sub

    Private Sub lstModules_SelectedIndexChanged(sender As Object, e As EventArgs)
        If lstModules.SelectedIndex >= 0 AndAlso lstModules.SelectedIndex < _discoveredModules.Count Then
            Dim selectedModule As Models.ModuleMetadata = _discoveredModules(lstModules.SelectedIndex)
            HandleModuleSelection(selectedModule)
        Else
            btnRunModule.Enabled = False
            btnUnloadModule.Enabled = False
            btnReloadModule.Enabled = False
            _currentModule = Nothing
            ClearModuleDetails()
        End If
    End Sub

    Private Sub HandleModuleSelection(moduleMetadata As Models.ModuleMetadata)
        _currentModule = Nothing
        UpdateModuleDetails(moduleMetadata)

        If String.IsNullOrEmpty(moduleMetadata.TypeName) Then
            _loggingService.LogWarning(String.Format("Module {0} does not implement IModule", moduleMetadata.DisplayName))
            StatusLabel.Text = "Invalid module selected"
            btnRunModule.Enabled = False
            btnUnloadModule.Enabled = False
            btnReloadModule.Enabled = False
            Return
        End If

        If moduleMetadata.IsLoaded Then
            StatusLabel.Text = String.Format("Selected: {0} (Loaded)", moduleMetadata.DisplayName)
            btnRunModule.Enabled = False
            btnUnloadModule.Enabled = True
            btnReloadModule.Enabled = True
        Else
            StatusLabel.Text = String.Format("Selected: {0} (Not Loaded)", moduleMetadata.DisplayName)
            btnRunModule.Enabled = True
            btnUnloadModule.Enabled = False
            btnReloadModule.Enabled = False
        End If
    End Sub

    Private Sub UpdateModuleDetails(moduleMetadata As Models.ModuleMetadata)
        txtFileName.Text = If(moduleMetadata.FileName, "N/A")
        txtDisplayName.Text = If(moduleMetadata.DisplayName, "N/A")
        txtTypeName.Text = If(String.IsNullOrEmpty(moduleMetadata.TypeName), "N/A", moduleMetadata.TypeName)
        txtLastLoadedTime.Text = If(moduleMetadata.LastLoadedTime.HasValue, moduleMetadata.LastLoadedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), "Never")
        txtIsLoaded.Text = If(moduleMetadata.IsLoaded, "Yes", "No")
    End Sub

    Private Sub ClearModuleDetails()
        txtFileName.Text = String.Empty
        txtDisplayName.Text = String.Empty
        txtTypeName.Text = String.Empty
        txtLastLoadedTime.Text = String.Empty
        txtIsLoaded.Text = String.Empty
    End Sub

    Private Sub btnRunModule_Click(sender As Object, e As EventArgs)
        If lstModules.SelectedIndex < 0 OrElse lstModules.SelectedIndex >= _discoveredModules.Count Then
            Return
        End If

        Dim selectedModule As Models.ModuleMetadata = _discoveredModules(lstModules.SelectedIndex)
        ExecuteModule(selectedModule)
    End Sub

    Private Sub btnUnloadModule_Click(sender As Object, e As EventArgs)
        If lstModules.SelectedIndex < 0 OrElse lstModules.SelectedIndex >= _discoveredModules.Count Then
            Return
        End If

        Dim selectedModule As Models.ModuleMetadata = _discoveredModules(lstModules.SelectedIndex)
        UnloadModule(selectedModule)
    End Sub

    Private Sub btnReloadModule_Click(sender As Object, e As EventArgs)
        If lstModules.SelectedIndex < 0 OrElse lstModules.SelectedIndex >= _discoveredModules.Count Then
            Return
        End If

        Dim selectedModule As Models.ModuleMetadata = _discoveredModules(lstModules.SelectedIndex)
        ReloadModule(selectedModule)
    End Sub

    Private Sub btnRefreshModules_Click(sender As Object, e As EventArgs)
        _loggingService.LogInfo("Refreshing module list...")
        StatusLabel.Text = "Refreshing modules..."
        
        Dim previousSelection As Integer = lstModules.SelectedIndex
        LoadAvailableModules()
        
        If previousSelection >= 0 AndAlso previousSelection < lstModules.Items.Count Then
            lstModules.SelectedIndex = previousSelection
        End If
        
        _loggingService.LogInfo("Module list refreshed")
        StatusLabel.Text = String.Format("Refreshed: {0} module(s) found", _discoveredModules.Count)
    End Sub

    Private Sub btnClearLog_Click(sender As Object, e As EventArgs)
        txtLogOutput.Clear()
        _loggingService.ClearLog()
        _loggingService.LogInfo("Log cleared by user")
        StatusLabel.Text = "Log cleared"
    End Sub

    Private Sub btnApplyFilter_Click(sender As Object, e As EventArgs)
        Dim filterLevel As Services.Interfaces.LogLevel? = Nothing
        Dim searchText As String = txtLogSearch.Text.Trim()

        Select Case cmbLogLevel.SelectedIndex
            Case 1
                filterLevel = Services.Interfaces.LogLevel.Info
            Case 2
                filterLevel = Services.Interfaces.LogLevel.Warning
            Case 3
                filterLevel = Services.Interfaces.LogLevel.[Error]
        End Select

        _loggingService.FilterLevel = filterLevel
        _loggingService.SearchText = searchText

        RebuildLogOutput()

        Dim filterDesc As String = "All"
        If filterLevel.HasValue Then
            filterDesc = filterLevel.Value.ToString()
        End If

        Dim statusMsg As String = String.Format("Filter applied: Level={0}", filterDesc)
        If Not String.IsNullOrEmpty(searchText) Then
            statusMsg &= String.Format(", Search=""{0}""", searchText)
        End If

        StatusLabel.Text = statusMsg
        _loggingService.LogInfo(statusMsg)
    End Sub

    Private Sub RebuildLogOutput()
        txtLogOutput.Clear()
        Dim allEntries As List(Of String) = _loggingService.GetLogEntries()

        For Each entry As String In allEntries
            If ShouldDisplayEntry(entry) Then
                txtLogOutput.AppendText(entry & Environment.NewLine)
            End If
        Next
    End Sub

    Private Function ShouldDisplayEntry(entry As String) As Boolean
        Dim filterLevel As Services.Interfaces.LogLevel? = _loggingService.FilterLevel
        Dim searchText As String = _loggingService.SearchText

        If filterLevel.HasValue Then
            Dim entryLevel As Services.Interfaces.LogLevel = GetEntryLevel(entry)
            If entryLevel < filterLevel.Value Then
                Return False
            End If
        End If

        If Not String.IsNullOrEmpty(searchText) Then
            If entry.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0 Then
                Return False
            End If
        End If

        Return True
    End Function

    Private Function GetEntryLevel(entry As String) As Services.Interfaces.LogLevel
        If entry.StartsWith("[INFO]") Then
            Return Services.Interfaces.LogLevel.Info
        ElseIf entry.StartsWith("[WARN]") Then
            Return Services.Interfaces.LogLevel.Warning
        ElseIf entry.StartsWith("[ERROR]") Then
            Return Services.Interfaces.LogLevel.[Error]
        End If
        Return Services.Interfaces.LogLevel.Info
    End Function

    Private Sub ExecuteModule(moduleMetadata As Models.ModuleMetadata)
        Dim modulePath As String = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules", moduleMetadata.FileName)

        Try
            _loggingService.LogInfo(String.Format("Loading module: {0}", moduleMetadata.DisplayName))
            _currentModule = _moduleLoaderService.LoadModule(modulePath)

            _moduleLoaderService.InitializeAndConfigureModule(_currentModule, moduleMetadata.DisplayName)

            _loggingService.LogInfo(String.Format("Executing module: {0}", moduleMetadata.DisplayName))
            _currentModule.Execute()

            StatusLabel.Text = String.Format("Executed: {0}", moduleMetadata.DisplayName)
            _loggingService.LogInfo(String.Format("Module execution completed: {0}", moduleMetadata.DisplayName))

            UpdateModuleDetails(moduleMetadata)
            btnRunModule.Enabled = False
            btnUnloadModule.Enabled = True
            btnReloadModule.Enabled = True
        Catch ex As Exception
            _loggingService.LogError(String.Format("Failed to execute module: {0}", ex.Message))
            StatusLabel.Text = "Error executing module"
        End Try
    End Sub

    Private Sub UnloadModule(moduleMetadata As Models.ModuleMetadata)
        Try
            _loggingService.LogInfo(String.Format("Unloading module: {0}", moduleMetadata.DisplayName))
            _moduleLoaderService.UnloadModule(moduleMetadata)

            StatusLabel.Text = String.Format("Unloaded: {0}", moduleMetadata.DisplayName)
            _loggingService.LogInfo(String.Format("Module unloaded: {0}", moduleMetadata.DisplayName))

            UpdateModuleDetails(moduleMetadata)
            btnRunModule.Enabled = True
            btnUnloadModule.Enabled = False
            btnReloadModule.Enabled = False
        Catch ex As Exception
            _loggingService.LogError(String.Format("Failed to unload module: {0}", ex.Message))
            StatusLabel.Text = "Error unloading module"
        End Try
    End Sub

    Private Sub ReloadModule(moduleMetadata As Models.ModuleMetadata)
        Try
            _loggingService.LogInfo(String.Format("Reloading module: {0}", moduleMetadata.DisplayName))
            _currentModule = _moduleLoaderService.ReloadModule(moduleMetadata)

            _moduleLoaderService.InitializeAndConfigureModule(_currentModule, moduleMetadata.DisplayName)

            StatusLabel.Text = String.Format("Reloaded: {0}", moduleMetadata.DisplayName)
            _loggingService.LogInfo(String.Format("Module reloaded successfully: {0}", moduleMetadata.DisplayName))

            UpdateModuleDetails(moduleMetadata)
            btnRunModule.Enabled = False
            btnUnloadModule.Enabled = True
            btnReloadModule.Enabled = True
        Catch ex As Exception
            _loggingService.LogError(String.Format("Failed to reload module: {0}", ex.Message))
            StatusLabel.Text = "Error reloading module"
            UpdateModuleDetails(moduleMetadata)
        End Try
    End Sub

    Private Sub LoggingService_LogMessageReceived(sender As Object, e As Services.Interfaces.LogMessageEventArgs)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of Object, Services.Interfaces.LogMessageEventArgs)(AddressOf LoggingService_LogMessageReceived), sender, e)
            Return
        End If

        txtLogOutput.AppendText(e.Message & Environment.NewLine)
    End Sub

End Class
