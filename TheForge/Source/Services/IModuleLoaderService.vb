Namespace Services
    ''' <summary>
    ''' Interface for module loading and discovery services.
    ''' </summary>
    Public Interface IModuleLoaderService
        ''' <summary>
        ''' Gets a list of available modules.
        ''' </summary>
        Function GetAvailableModules() As List(Of String)

        ''' <summary>
        ''' Loads a module by name.
        ''' </summary>
        Sub LoadModule(moduleName As String)

        ''' <summary>
        ''' Unloads the currently loaded module.
        ''' </summary>
        Sub UnloadModule()
    End Interface
End Namespace
