''' ============================================================================
''' File: IModuleContract.vb  
''' Project: RCH.TemplateStorage
''' Purpose: Marker interface contract for duck-typed modules
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Provides IModule contract signature for modules that cannot directly
'''   reference TheForge (to avoid circular dependencies). Modules implement
'''   this interface which matches IModule signature exactly.
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Namespace Modules.Interfaces

    ''' <summary>
    ''' Module interface contract (matches TheForge.Modules.Interfaces.IModule)
    ''' Use this when module cannot reference TheForge directly
    ''' </summary>
    Public Interface IModuleContract
        Inherits IDisposable

        ''' <summary>
        ''' Initializes the module with required services
        ''' </summary>
        Sub Initialize(loggingService As Object)

        ''' <summary>
        ''' Loads module configuration
        ''' </summary>
        Sub LoadConfiguration(config As Object)

        ''' <summary>
        ''' Executes the module's primary functionality
        ''' </summary>
        Sub Execute()

        ''' <summary>
        ''' Called before the module is unloaded
        ''' </summary>
        Sub OnUnload()
    End Interface

End Namespace
