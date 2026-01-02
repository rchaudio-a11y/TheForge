' **Character Count:** TBD
' **Document Type:** Code
' **Created:** 2024-11-01
' **Last Updated:** 2025-01-02
' **Related:** IModuleLoaderService.vb, ILoggingService.vb, ModuleMetadata.vb

Imports System.IO
Imports System.Reflection

Namespace Services.Implementations
    ''' <summary>
    ''' Implementation of IModuleLoaderService for module discovery and loading.
    ''' </summary>
    Public Class ModuleLoaderService
        Implements Services.Interfaces.IModuleLoaderService

        Private ReadOnly _modulesDirectory As String
        Private ReadOnly _loggingService As Services.Interfaces.ILoggingService
        Private ReadOnly _moduleCache As Dictionary(Of String, Models.ModuleMetadata)
        Private _cachedDirectoryListing As String()
        Private _lastDirectoryScanTime As DateTime

        Public Sub New(loggingService As Services.Interfaces.ILoggingService)
            Dim appDirectory As String = AppDomain.CurrentDomain.BaseDirectory
            _modulesDirectory = IO.Path.Combine(appDirectory, "Modules")
            _loggingService = loggingService
            _moduleCache = New Dictionary(Of String, Models.ModuleMetadata)()
            _cachedDirectoryListing = Nothing
            _lastDirectoryScanTime = DateTime.MinValue
        End Sub

        Public Function DiscoverModules() As List(Of Models.ModuleMetadata) Implements Services.Interfaces.IModuleLoaderService.DiscoverModules
            Dim modules As New List(Of Models.ModuleMetadata)()

            If Not Directory.Exists(_modulesDirectory) Then
                Directory.CreateDirectory(_modulesDirectory)
                _loggingService.LogInfo("Modules directory created")
                Return modules
            End If

            Dim dllFiles As String() = GetModuleFiles()
            _loggingService.LogInfo(String.Format("Found {0} DLL file(s) in Modules directory", dllFiles.Length))

            For Each filePath As String In dllFiles
                Try
                    Dim fileName As String = IO.Path.GetFileName(filePath)
                    Dim displayName As String = IO.Path.GetFileNameWithoutExtension(filePath)

                    Dim metadata As Models.ModuleMetadata = Nothing
                    If _moduleCache.ContainsKey(fileName) Then
                        metadata = _moduleCache(fileName)
                        _loggingService.LogInfo(String.Format("Using cached metadata for: {0}", displayName))
                    Else
                        Dim assembly As Assembly = Assembly.LoadFrom(filePath)
                        Dim moduleType As Type = FindModuleType(assembly)

                        Dim typeName As String = String.Empty
                        Dim dependencies As String() = New String() {}

                        If moduleType IsNot Nothing Then
                            typeName = moduleType.FullName
                            dependencies = ExtractDependencies(moduleType)

                            If dependencies.Length > 0 Then
                                _loggingService.LogInfo(String.Format("Discovered module: {0} ({1}) with {2} dependencies", displayName, typeName, dependencies.Length))
                            Else
                                _loggingService.LogInfo(String.Format("Discovered module: {0} ({1})", displayName, typeName))
                            End If
                        Else
                            _loggingService.LogWarning(String.Format("No IModule implementation found in {0}", fileName))
                        End If

                        metadata = New Models.ModuleMetadata(fileName, displayName, typeName)
                        metadata.Dependencies = dependencies
                        _moduleCache(fileName) = metadata
                    End If

                    modules.Add(metadata)
                Catch ex As Exception
                    _loggingService.LogError(String.Format("Error discovering module {0}: {1}", IO.Path.GetFileName(filePath), ex.Message))
                End Try
            Next

            ValidateDependencies(modules)
            Return modules
        End Function

        Private Function GetModuleFiles() As String()
            Dim timeSinceLastScan As TimeSpan = DateTime.Now - _lastDirectoryScanTime

            If _cachedDirectoryListing IsNot Nothing AndAlso timeSinceLastScan.TotalSeconds < 2 Then
                _loggingService.LogInfo("Using cached directory listing (scanned " & timeSinceLastScan.TotalMilliseconds.ToString("F0") & "ms ago)")
                Return _cachedDirectoryListing
            End If

            _cachedDirectoryListing = Directory.GetFiles(_modulesDirectory, "*.dll", SearchOption.TopDirectoryOnly)
            _lastDirectoryScanTime = DateTime.Now
            _loggingService.LogInfo("Directory listing refreshed")

            Return _cachedDirectoryListing
        End Function

        Public Function LoadModule(path As String) As Modules.Interfaces.IModule Implements Services.Interfaces.IModuleLoaderService.LoadModule
            If Not File.Exists(path) Then
                Throw New FileNotFoundException("Module file not found.", path)
            End If

            Dim fileName As String = IO.Path.GetFileName(path)

            If _moduleCache.ContainsKey(fileName) Then
                Dim cachedMetadata As Models.ModuleMetadata = _moduleCache(fileName)
                If cachedMetadata.IsLoaded AndAlso cachedMetadata.CachedInstance IsNot Nothing Then
                    _loggingService.LogInfo(String.Format("Returning cached module instance: {0}", cachedMetadata.DisplayName))
                    Return cachedMetadata.CachedInstance
                End If
            End If

            Return LoadModuleInternal(path, True)
        End Function

        Public Function ReloadModule(metadata As Models.ModuleMetadata) As Modules.Interfaces.IModule Implements Services.Interfaces.IModuleLoaderService.ReloadModule
            If metadata Is Nothing Then
                Throw New ArgumentNullException("metadata", "Module metadata cannot be null")
            End If

            Dim reloadStartTime As DateTime = DateTime.Now
            _loggingService.LogInfo(String.Format("Reloading module: {0}", metadata.DisplayName))

            UnloadModule(metadata)

            Dim modulePath As String = IO.Path.Combine(_modulesDirectory, metadata.FileName)
            Dim reloadedModule As Modules.Interfaces.IModule = LoadModuleInternal(modulePath, False)

            Dim reloadDuration As TimeSpan = DateTime.Now - reloadStartTime
            _loggingService.LogInfo(String.Format("Module reloaded successfully: {0} (took {1}ms)", metadata.DisplayName, reloadDuration.TotalMilliseconds.ToString("F0")))

            Return reloadedModule
        End Function

        Private Function LoadModuleInternal(path As String, useCache As Boolean) As Modules.Interfaces.IModule
            _loggingService.LogInfo(String.Format("Loading assembly from: {0}", path))

            Dim assembly As Assembly = Assembly.LoadFrom(path)
            Dim moduleType As Type = FindModuleType(assembly)

            If moduleType Is Nothing Then
                Dim errorMsg As String = String.Format("No type implementing IModule found in assembly: {0}", path)
                _loggingService.LogError(errorMsg)
                Throw New InvalidOperationException(errorMsg)
            End If

            _loggingService.LogInfo(String.Format("Instantiating module type: {0}", moduleType.FullName))

            Dim moduleInstance As Object = Activator.CreateInstance(moduleType)
            Dim moduleInterface As Modules.Interfaces.IModule = TryCast(moduleInstance, Modules.Interfaces.IModule)

            If moduleInterface Is Nothing Then
                Dim errorMsg As String = String.Format("Failed to cast module instance to IModule: {0}", moduleType.FullName)
                _loggingService.LogError(errorMsg)
                Throw New InvalidOperationException(errorMsg)
            End If

            Dim fileName As String = IO.Path.GetFileName(path)

            If _moduleCache.ContainsKey(fileName) Then
                Dim metadata As Models.ModuleMetadata = _moduleCache(fileName)
                metadata.CachedInstance = moduleInterface
                metadata.IsLoaded = True
                metadata.LastLoadedTime = DateTime.Now
            End If

            _loggingService.LogInfo(String.Format("Module loaded successfully: {0}", moduleType.FullName))
            Return moduleInterface
        End Function

        Public Sub InitializeAndConfigureModule(moduleInterface As Modules.Interfaces.IModule, moduleName As String) Implements Services.Interfaces.IModuleLoaderService.InitializeAndConfigureModule
            If moduleInterface Is Nothing Then
                Return
            End If

            _loggingService.LogInfo(String.Format("Initializing module: {0}", moduleName))
            moduleInterface.Initialize(_loggingService)

            LoadModuleConfiguration(moduleInterface, moduleName)
        End Sub

        Private Sub LoadModuleConfiguration(moduleInterface As Modules.Interfaces.IModule, moduleName As String)
            Dim configPath As String = IO.Path.Combine(_modulesDirectory, moduleName & ".config")

            If Not File.Exists(configPath) Then
                _loggingService.LogInfo(String.Format("No configuration file found for module: {0}", moduleName))
                Return
            End If

            Try
                _loggingService.LogInfo(String.Format("Loading configuration from: {0}", configPath))
                Dim config As New Models.ModuleConfiguration()

                Dim lines As String() = File.ReadAllLines(configPath)
                For Each line As String In lines
                    Dim trimmedLine As String = line.Trim()
                    If String.IsNullOrEmpty(trimmedLine) OrElse trimmedLine.StartsWith("#") OrElse trimmedLine.StartsWith(";") Then
                        Continue For
                    End If

                    Dim separatorIndex As Integer = trimmedLine.IndexOf("="c)
                    If separatorIndex > 0 Then
                        Dim key As String = trimmedLine.Substring(0, separatorIndex).Trim()
                        Dim value As String = trimmedLine.Substring(separatorIndex + 1).Trim()
                        config.SetValue(key, value)
                        _loggingService.LogInfo(String.Format("Configuration loaded: {0}={1}", key, value))
                    End If
                Next

                _loggingService.LogInfo(String.Format("Calling LoadConfiguration() for: {0}", moduleName))
                moduleInterface.LoadConfiguration(config)
                _loggingService.LogInfo(String.Format("Configuration applied successfully: {0}", moduleName))
            Catch ex As Exception
                _loggingService.LogError(String.Format("Error loading configuration for {0}: {1}", moduleName, ex.Message))
            End Try
        End Sub

        Public Sub UnloadModule(metadata As Models.ModuleMetadata) Implements Services.Interfaces.IModuleLoaderService.UnloadModule
            If metadata Is Nothing Then
                _loggingService.LogWarning("Cannot unload null module metadata")
                Return
            End If

            If Not metadata.IsLoaded Then
                _loggingService.LogWarning(String.Format("Module {0} is not currently loaded", metadata.DisplayName))
                Return
            End If

            If metadata.CachedInstance Is Nothing Then
                _loggingService.LogWarning(String.Format("Module {0} has no cached instance", metadata.DisplayName))
                metadata.IsLoaded = False
                Return
            End If

            Try
                _loggingService.LogInfo(String.Format("Unloading module: {0}", metadata.DisplayName))

                _loggingService.LogInfo(String.Format("Calling OnUnload() for: {0}", metadata.DisplayName))
                metadata.CachedInstance.OnUnload()

                _loggingService.LogInfo(String.Format("Disposing module: {0}", metadata.DisplayName))
                metadata.CachedInstance.Dispose()

                metadata.CachedInstance = Nothing
                metadata.IsLoaded = False

                _loggingService.LogInfo(String.Format("Module unloaded successfully: {0}", metadata.DisplayName))
            Catch ex As Exception
                _loggingService.LogError(String.Format("Error unloading module {0}: {1}", metadata.DisplayName, ex.Message))
                metadata.CachedInstance = Nothing
                metadata.IsLoaded = False
            End Try
        End Sub

        Private Function FindModuleType(assembly As Assembly) As Type
            Dim moduleInterfaceType As Type = GetType(Modules.Interfaces.IModule)

            For Each type As Type In assembly.GetTypes()
                If type.IsClass AndAlso Not type.IsAbstract AndAlso moduleInterfaceType.IsAssignableFrom(type) Then
                    Return type
                End If
            Next

            Return Nothing
        End Function

        Private Function ExtractDependencies(moduleType As Type) As String()
            Dim depAttr As Modules.Interfaces.ModuleDependencyAttribute =
                TryCast(Attribute.GetCustomAttribute(moduleType, GetType(Modules.Interfaces.ModuleDependencyAttribute)),
                        Modules.Interfaces.ModuleDependencyAttribute)

            If depAttr IsNot Nothing AndAlso depAttr.DependsOn IsNot Nothing Then
                Return depAttr.DependsOn
            End If

            Return New String() {}
        End Function

        Private Sub ValidateDependencies(modules As List(Of Models.ModuleMetadata))
            _loggingService.LogInfo("Validating module dependencies...")

            Dim moduleTypeNames As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each metadata As Models.ModuleMetadata In modules
                If Not String.IsNullOrEmpty(metadata.TypeName) Then
                    moduleTypeNames.Add(metadata.TypeName)
                End If
            Next

            For Each metadata As Models.ModuleMetadata In modules
                If metadata.Dependencies IsNot Nothing AndAlso metadata.Dependencies.Length > 0 Then
                    For Each dep As String In metadata.Dependencies
                        If Not moduleTypeNames.Contains(dep) Then
                            Dim errorMsg As String = String.Format("Module {0} depends on {1}, but it is not found", metadata.DisplayName, dep)
                            _loggingService.LogError(errorMsg)
                            Throw New InvalidOperationException(errorMsg)
                        End If
                    Next
                End If
            Next

            DetectCircularDependencies(modules)
            _loggingService.LogInfo("Dependency validation completed successfully")
        End Sub

        Private Sub DetectCircularDependencies(modules As List(Of Models.ModuleMetadata))
            Dim visited As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            Dim recursionStack As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            For Each metadata As Models.ModuleMetadata In modules
                If Not String.IsNullOrEmpty(metadata.TypeName) Then
                    If HasCircularDependency(metadata.TypeName, modules, visited, recursionStack) Then
                        Dim errorMsg As String = String.Format("Circular dependency detected involving module: {0}", metadata.DisplayName)
                        _loggingService.LogError(errorMsg)
                        Throw New InvalidOperationException(errorMsg)
                    End If
                End If
            Next
        End Sub

        Private Function HasCircularDependency(typeName As String, modules As List(Of Models.ModuleMetadata),
                                               visited As HashSet(Of String), recursionStack As HashSet(Of String)) As Boolean
            If Not visited.Contains(typeName) Then
                visited.Add(typeName)
                recursionStack.Add(typeName)

                Dim currentModule As Models.ModuleMetadata = modules.FirstOrDefault(Function(m) m.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                If currentModule IsNot Nothing AndAlso currentModule.Dependencies IsNot Nothing Then
                    For Each dep As String In currentModule.Dependencies
                        If Not visited.Contains(dep) Then
                            If HasCircularDependency(dep, modules, visited, recursionStack) Then
                                Return True
                            End If
                        ElseIf recursionStack.Contains(dep) Then
                            Return True
                        End If
                    Next
                End If
            End If

            recursionStack.Remove(typeName)
            Return False
        End Function
    End Class
End Namespace
