Namespace Services.Interfaces
    ''' <summary>
    ''' Interface for module loading and discovery services.
    ''' </summary>
    Public Interface IModuleLoaderService
        ''' <summary>
        ''' Discovers available modules in the modules directory.
        ''' </summary>
        ''' <returns>A list of discovered module metadata.</returns>
        Function DiscoverModules() As List(Of Models.ModuleMetadata)

        ''' <summary>
        ''' Loads a module by its file path and returns the instantiated module.
        ''' </summary>
        ''' <param name="path">The path to the module file.</param>
        ''' <returns>The instantiated module instance.</returns>
        Function LoadModule(path As String) As Modules.Interfaces.IModule

        ''' <summary>
        ''' Initializes a module and loads its configuration.
        ''' </summary>
        ''' <param name="moduleInterface">The module instance to initialize.</param>
        ''' <param name="moduleName">The name of the module.</param>
        Sub InitializeAndConfigureModule(moduleInterface As Modules.Interfaces.IModule, moduleName As String)

        ''' <summary>
        ''' Unloads a module and cleans up its resources.
        ''' </summary>
        ''' <param name="metadata">The module metadata to unload.</param>
        Sub UnloadModule(metadata As Models.ModuleMetadata)

        ''' <summary>
        ''' Reloads a module by unloading and re-loading it.
        ''' </summary>
        ''' <param name="metadata">The module metadata to reload.</param>
        ''' <returns>The reloaded module instance.</returns>
        Function ReloadModule(metadata As Models.ModuleMetadata) As Modules.Interfaces.IModule
    End Interface
End Namespace
