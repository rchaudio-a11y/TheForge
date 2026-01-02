Imports System.Windows.Forms

Public Class DashboardMainForm
    Inherits Form

    Private WithEvents moduleListControl As UI.Controls.ModuleListControl
    Private moduleDetailsControl As UI.Controls.ModuleDetailsControl
    Private WithEvents logOutputControl As UI.Controls.LogOutputControl
    Private testAreaControl As UI.Controls.TestAreaControl
    Private splitterVertical As Splitter
    Private splitterHorizontal As Splitter
    Private splitterVerticalRight As Splitter
    Private MainStatusStrip As StatusStrip
    Private StatusLabel As ToolStripStatusLabel

    Private _moduleLoaderService As Services.Interfaces.IModuleLoaderService
    Private _loggingService As Services.Interfaces.ILoggingService
    Private _discoveredModules As List(Of Models.ModuleMetadata)
    Private _currentModule As Modules.Interfaces.IModule

    Public Sub New()
        InitializeComponent()
        InitializeLayout()
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

        moduleListControl = New UI.Controls.ModuleListControl()
        moduleListControl.Name = "moduleListControl"
        moduleListControl.Dock = DockStyle.Left
        moduleListControl.Width = 250

        splitterVertical = New Splitter()
        splitterVertical.Name = "splitterVertical"
        splitterVertical.Dock = DockStyle.Left
        splitterVertical.Width = 3

        moduleDetailsControl = New UI.Controls.ModuleDetailsControl()
        moduleDetailsControl.Name = "moduleDetailsControl"
        moduleDetailsControl.Dock = DockStyle.Right
        moduleDetailsControl.Width = 300

        splitterVerticalRight = New Splitter()
        splitterVerticalRight.Name = "splitterVerticalRight"
        splitterVerticalRight.Dock = DockStyle.Right
        splitterVerticalRight.Width = 3

        logOutputControl = New UI.Controls.LogOutputControl()
        logOutputControl.Name = "logOutputControl"
        logOutputControl.Dock = DockStyle.Bottom
        logOutputControl.Height = 200

        splitterHorizontal = New Splitter()
        splitterHorizontal.Name = "splitterHorizontal"
        splitterHorizontal.Dock = DockStyle.Bottom
        splitterHorizontal.Height = 3

        testAreaControl = New UI.Controls.TestAreaControl()
        testAreaControl.Name = "testAreaControl"
        testAreaControl.Dock = DockStyle.Fill

        Me.Controls.Add(testAreaControl)
        Me.Controls.Add(splitterVerticalRight)
        Me.Controls.Add(moduleDetailsControl)
        Me.Controls.Add(splitterHorizontal)
        Me.Controls.Add(logOutputControl)
        Me.Controls.Add(splitterVertical)
        Me.Controls.Add(moduleListControl)
        Me.Controls.Add(MainStatusStrip)

        AddHandler Me.Load, AddressOf DashboardMainForm_Load
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
            moduleListControl.LoadModules(_discoveredModules)

            Dim message As String = String.Format("Discovered {0} module(s)", _discoveredModules.Count)
            StatusLabel.Text = message
            _loggingService.LogInfo(message)
        Catch ex As Exception
            _loggingService.LogError(String.Format("Error discovering modules: {0}", ex.Message))
            StatusLabel.Text = "Error during module discovery"
        End Try
    End Sub

    Private Sub moduleListControl_ModuleSelected(sender As Object, e As UI.Controls.ModuleSelectedEventArgs) Handles moduleListControl.ModuleSelected
        If e.SelectedIndex >= 0 AndAlso e.SelectedIndex < _discoveredModules.Count Then
            Dim selectedModule As Models.ModuleMetadata = _discoveredModules(e.SelectedIndex)
            HandleModuleSelection(selectedModule)
        Else
            moduleListControl.UpdateButtonStates(False, False, False)
            moduleDetailsControl.ClearDetails()
            _currentModule = Nothing
        End If
    End Sub

    Private Sub HandleModuleSelection(moduleMetadata As Models.ModuleMetadata)
        _currentModule = Nothing
        moduleDetailsControl.UpdateDetails(moduleMetadata)

        If String.IsNullOrEmpty(moduleMetadata.TypeName) Then
            _loggingService.LogWarning(String.Format("Module {0} does not implement IModule", moduleMetadata.DisplayName))
            StatusLabel.Text = "Invalid module selected"
            moduleListControl.UpdateButtonStates(False, False, False)
            Return
        End If

        If moduleMetadata.IsLoaded Then
            StatusLabel.Text = String.Format("Selected: {0} (Loaded)", moduleMetadata.DisplayName)
            moduleListControl.UpdateButtonStates(False, True, True)
        Else
            StatusLabel.Text = String.Format("Selected: {0} (Not Loaded)", moduleMetadata.DisplayName)
            moduleListControl.UpdateButtonStates(True, False, False)
        End If
    End Sub

    Private Sub moduleListControl_RunRequested(sender As Object, e As EventArgs) Handles moduleListControl.RunRequested
        Dim selectedIndex As Integer = moduleListControl.SelectedIndex
        If selectedIndex < 0 OrElse selectedIndex >= _discoveredModules.Count Then
            Return
        End If

        Dim selectedModule As Models.ModuleMetadata = _discoveredModules(selectedIndex)
        ExecuteModule(selectedModule)
    End Sub

    Private Sub moduleListControl_UnloadRequested(sender As Object, e As EventArgs) Handles moduleListControl.UnloadRequested
        Dim selectedIndex As Integer = moduleListControl.SelectedIndex
        If selectedIndex < 0 OrElse selectedIndex >= _discoveredModules.Count Then
            Return
        End If

        Dim selectedModule As Models.ModuleMetadata = _discoveredModules(selectedIndex)
        UnloadModule(selectedModule)
    End Sub

    Private Sub moduleListControl_ReloadRequested(sender As Object, e As EventArgs) Handles moduleListControl.ReloadRequested
        Dim selectedIndex As Integer = moduleListControl.SelectedIndex
        If selectedIndex < 0 OrElse selectedIndex >= _discoveredModules.Count Then
            Return
        End If

        Dim selectedModule As Models.ModuleMetadata = _discoveredModules(selectedIndex)
        ReloadModule(selectedModule)
    End Sub

    Private Sub moduleListControl_RefreshRequested(sender As Object, e As EventArgs) Handles moduleListControl.RefreshRequested
        _loggingService.LogInfo("Refreshing module list...")
        StatusLabel.Text = "Refreshing modules..."

        Dim previousSelection As Integer = moduleListControl.SelectedIndex
        LoadAvailableModules()

        If previousSelection >= 0 AndAlso previousSelection < _discoveredModules.Count Then
            moduleListControl.SetSelectedIndex(previousSelection)
        End If

        _loggingService.LogInfo("Module list refreshed")
        StatusLabel.Text = String.Format("Refreshed: {0} module(s) found", _discoveredModules.Count)
    End Sub

    Private Sub logOutputControl_ClearLogRequested(sender As Object, e As EventArgs) Handles logOutputControl.ClearLogRequested
        logOutputControl.ClearLog()
        _loggingService.ClearLog()
        _loggingService.LogInfo("Log cleared by user")
        StatusLabel.Text = "Log cleared"
    End Sub

    Private Sub logOutputControl_FilterApplied(sender As Object, e As UI.Controls.FilterAppliedEventArgs) Handles logOutputControl.FilterApplied
        _loggingService.FilterLevel = e.FilterLevel
        _loggingService.SearchText = e.SearchText

        Dim allEntries As List(Of String) = _loggingService.GetLogEntries()
        logOutputControl.RebuildLog(allEntries, e.FilterLevel, e.SearchText)

        Dim filterDesc As String = "All"
        If e.FilterLevel.HasValue Then
            filterDesc = e.FilterLevel.Value.ToString()
        End If

        Dim statusMsg As String = String.Format("Filter applied: Level={0}", filterDesc)
        If Not String.IsNullOrEmpty(e.SearchText) Then
            statusMsg &= String.Format(", Search=""{0}""", e.SearchText)
        End If

        StatusLabel.Text = statusMsg
        _loggingService.LogInfo(statusMsg)
    End Sub

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

            moduleDetailsControl.UpdateDetails(moduleMetadata)
            moduleListControl.UpdateButtonStates(False, True, True)
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

            moduleDetailsControl.UpdateDetails(moduleMetadata)
            moduleListControl.UpdateButtonStates(True, False, False)
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

            moduleDetailsControl.UpdateDetails(moduleMetadata)
            moduleListControl.UpdateButtonStates(False, True, True)
        Catch ex As Exception
            _loggingService.LogError(String.Format("Failed to reload module: {0}", ex.Message))
            StatusLabel.Text = "Error reloading module"
            moduleDetailsControl.UpdateDetails(moduleMetadata)
        End Try
    End Sub

    Private Sub LoggingService_LogMessageReceived(sender As Object, e As Services.Interfaces.LogMessageEventArgs)
        logOutputControl.AppendLog(e.Message)
    End Sub
End Class
