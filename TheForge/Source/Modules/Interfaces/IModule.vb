' **Character Count:** TBD
' **Document Type:** Code
' **Created:** 2024-11-01
' **Last Updated:** 2025-01-02
' **Related:** ModuleConfiguration.vb, ModuleLoaderService.vb

Namespace Modules.Interfaces
    ''' <summary>
    ''' Interface that all Forge modules must implement.
    ''' </summary>
    Public Interface IModule
        Inherits IDisposable

        ''' <summary>
        ''' Initializes the module with required services.
        ''' </summary>
        ''' <param name="loggingService">The logging service instance.</param>
        Sub Initialize(loggingService As Services.Interfaces.ILoggingService)

        ''' <summary>
        ''' Loads module configuration. Optional method called after Initialize() and before Execute().
        ''' </summary>
        ''' <param name="config">The module configuration instance.</param>
        Sub LoadConfiguration(config As IModuleConfiguration)

        ''' <summary>
        ''' Executes the module's primary functionality.
        ''' </summary>
        Sub Execute()

        ''' <summary>
        ''' Called before the module is unloaded. Optional cleanup operations.
        ''' </summary>
        Sub OnUnload()
    End Interface
End Namespace
