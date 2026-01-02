Public Class SampleModule
    Implements TheForge.Modules.Interfaces.IModule

    Private _loggingService As TheForge.Services.Interfaces.ILoggingService
    Private _config As TheForge.Modules.Interfaces.IModuleConfiguration
    Private _disposed As Boolean = False

    Public Sub Initialize(loggingService As TheForge.Services.Interfaces.ILoggingService) Implements TheForge.Modules.Interfaces.IModule.Initialize
        _loggingService = loggingService
        _loggingService.LogInfo("SampleModule initialized")
    End Sub

    Public Sub LoadConfiguration(config As TheForge.Modules.Interfaces.IModuleConfiguration) Implements TheForge.Modules.Interfaces.IModule.LoadConfiguration
        _config = config

        If _loggingService IsNot Nothing Then
            _loggingService.LogInfo("SampleModule configuration loaded")

            If _config.HasKey("WelcomeMessage") Then
                _loggingService.LogInfo(String.Format("Configuration: WelcomeMessage = {0}", _config.GetValue("WelcomeMessage")))
            End If

            If _config.HasKey("MaxIterations") Then
                _loggingService.LogInfo(String.Format("Configuration: MaxIterations = {0}", _config.GetValue("MaxIterations")))
            End If
        End If
    End Sub

    Public Sub Execute() Implements TheForge.Modules.Interfaces.IModule.Execute
        If _loggingService Is Nothing Then
            Return
        End If

        _loggingService.LogInfo("SampleModule executing...")

        Dim welcomeMsg As String = "Hello from the Forge!"
        If _config IsNot Nothing AndAlso _config.HasKey("WelcomeMessage") Then
            welcomeMsg = _config.GetValue("WelcomeMessage")
        End If

        _loggingService.LogInfo(welcomeMsg)
        _loggingService.LogInfo("This is a sample module demonstrating the Forge module system.")
        _loggingService.LogInfo("Module v0.8.0 - Now with configuration support!")
        _loggingService.LogInfo("SampleModule execution complete")
    End Sub

    Public Sub OnUnload() Implements TheForge.Modules.Interfaces.IModule.OnUnload
        If _loggingService IsNot Nothing Then
            _loggingService.LogInfo("SampleModule OnUnload() called")
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                If _loggingService IsNot Nothing Then
                    _loggingService.LogInfo("SampleModule disposed")
                End If
            End If
            _disposed = True
        End If
    End Sub
End Class
