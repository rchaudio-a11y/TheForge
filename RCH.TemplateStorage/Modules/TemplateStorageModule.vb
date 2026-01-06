''' ============================================================================
''' File: TemplateStorageModule.vb
''' Project: RCH.TemplateStorage
''' Purpose: Forge module wrapper for TemplateStorageService
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Implements IModule interface to make TemplateStorageService discoverable
'''   and loadable by TheForge dashboard. Provides module metadata, lifecycle
'''   management, and integration with Forge infrastructure.
''' 
''' Module Capabilities:
'''   - CRUD operations for templates
'''   - JSON import/export
'''   - Database management
'''   - Search and filtering
'''   - Usage tracking
''' 
''' Dependencies:
'''   - TheForge.Modules.Interfaces.IModule
'''   - TemplateStorageService
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports RCH.TemplateStorage.Services.Implementations
Imports RCH.TemplateStorage.Database

Namespace Modules

    ''' <summary>
    ''' Forge module wrapper for template storage functionality
    ''' Implements IModuleContract which matches TheForge IModule interface signature
    ''' </summary>
    Public Class TemplateStorageModule
        Implements Interfaces.IModuleContract

#Region "Private Fields"

        Private _loggingService As Object ' ILoggingService via duck typing
        Private _service As TemplateStorageService
        Private _databasePath As String
        Private _disposed As Boolean = False

#End Region

#Region "Module Metadata"

        ''' <summary>
        ''' Module name
        ''' </summary>
        Public ReadOnly Property Name As String
            Get
                Return "Template Storage Service"
            End Get
        End Property

        ''' <summary>
        ''' Module version
        ''' </summary>
        Public ReadOnly Property Version As String
            Get
                Return "1.0.0"
            End Get
        End Property

        ''' <summary>
        ''' Module description
        ''' </summary>
        Public ReadOnly Property Description As String
            Get
                Return "Provides CRUD operations for project templates with MS Access database persistence. " &
                       "Supports unlimited folder nesting, file templates, JSON import/export, and legacy compatibility."
            End Get
        End Property

        ''' <summary>
        ''' Module author
        ''' </summary>
        Public ReadOnly Property Author As String
            Get
                Return "RCH Audio"
            End Get
        End Property

        ''' <summary>
        ''' Module category
        ''' </summary>
        Public ReadOnly Property Category As String
            Get
                Return "Data Management"
            End Get
        End Property

#End Region

#Region "IModule Implementation"

        ''' <summary>
        ''' Initializes the module with logging service
        ''' </summary>
        Public Sub Initialize(loggingService As Object) Implements Interfaces.IModuleContract.Initialize
            _loggingService = loggingService
            LogInfo("Template Storage Module initializing...")

            ' Set default database path
            _databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                         "RCH", "TheForge", "Templates.accdb")

            ' Ensure directory exists
            Dim directory As String = Path.GetDirectoryName(_databasePath)
            If Not IO.Directory.Exists(directory) Then
                IO.Directory.CreateDirectory(directory)
            End If

            ' Check if database exists, if not create it
            If Not File.Exists(_databasePath) Then
                LogInfo("Database not found. Creating new database...")
                Try
                    Dim initializer As New DatabaseInitializer(_databasePath)
                    initializer.CreateDatabase()
                    initializer.InitializeSchema(includeSampleData:=False)
                    LogInfo("Database created successfully at: " & _databasePath)
                Catch ex As Exception
                    LogWarning($"Could not create database: {ex.Message}")
                    LogWarning("Module will operate in limited mode.")
                End Try
            Else
                ' Database exists, but verify schema
                Try
                    Dim initializer As New DatabaseInitializer(_databasePath)
                    If Not initializer.ValidateDatabaseSchema() Then
                        LogInfo("Database found but schema missing. Initializing schema...")
                        initializer.InitializeSchema(includeSampleData:=False)
                        LogInfo("Schema initialized successfully")
                    End If
                Catch ex As Exception
                    LogWarning($"Could not validate/initialize schema: {ex.Message}")
                End Try
            End If

            ' Create service
            If File.Exists(_databasePath) Then
                Try
                    _service = New TemplateStorageService(_databasePath)
                    LogInfo("Template Storage Service initialized successfully")
                    LogInfo($"Database: {_databasePath}")
                    LogInfo($"Template count: {_service.GetTemplateCount()}")
                Catch ex As Exception
                    LogError($"Error initializing service: {ex.Message}")
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Loads module configuration
        ''' </summary>
        Public Sub LoadConfiguration(config As Object) Implements Interfaces.IModuleContract.LoadConfiguration
            LogInfo("Loading configuration...")

            Try
                ' Use reflection to get DatabasePath from config
                Dim getValueMethod = config.GetType().GetMethod("GetValue")
                If getValueMethod IsNot Nothing Then
                    Dim customPath = CStr(getValueMethod.Invoke(config, {"DatabasePath"}))
                    
                    If Not String.IsNullOrEmpty(customPath) Then
                        _databasePath = customPath
                        LogInfo($"Using custom database path: {customPath}")

                        ' Recreate service with new path
                        If _service IsNot Nothing Then
                            _service.Dispose()
                        End If
                        
                        If File.Exists(_databasePath) Then
                            _service = New TemplateStorageService(_databasePath)
                            LogInfo("Service recreated with custom database path")
                        Else
                            LogWarning($"Custom database path not found: {customPath}")
                        End If
                    End If
                End If
            Catch ex As Exception
                LogError($"Error loading configuration: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Executes the module's primary functionality
        ''' </summary>
        Public Sub Execute() Implements Interfaces.IModuleContract.Execute
            LogInfo("Executing Template Storage Module...")

            If _service Is Nothing Then
                LogError("Service not initialized. Cannot execute.")
                Return
            End If

            Try
                ' Display module capabilities
                LogInfo("=== Template Storage Module ===")
                LogInfo($"Database: {_service.DatabasePath}")
                LogInfo($"Connection: {If(_service.TestConnection(), "OK", "FAILED")}")
                LogInfo($"Schema: {If(_service.ValidateDatabase(), "Valid", "Invalid")}")
                LogInfo($"Templates: {_service.GetTemplateCount()}")

                ' List available templates
                Dim templates = _service.GetAllTemplates()
                If templates.Count > 0 Then
                    LogInfo("Available templates:")
                    For Each template In templates
                        LogInfo($"  - [{template.TemplateID}] {template.Name} ({template.Category})")
                    Next
                Else
                    LogInfo("No templates found in database.")
                End If

                ' Display statistics
                Dim categories = _service.GetAllCategories()
                If categories.Count > 0 Then
                    LogInfo($"Categories: {String.Join(", ", categories)}")
                End If

                Dim tags = _service.GetAllTags()
                If tags.Count > 0 Then
                    LogInfo($"Tags: {String.Join(", ", tags)}")
                End If

                LogInfo("Module execution complete.")

            Catch ex As Exception
                LogError($"Error during execution: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Called before module unload
        ''' </summary>
        Public Sub OnUnload() Implements Interfaces.IModuleContract.OnUnload
            LogInfo("Template Storage Module unloading...")

            If _service IsNot Nothing Then
                _service.Dispose()
                _service = Nothing
            End If

            LogInfo("Module unloaded successfully.")
        End Sub

#End Region

#Region "Public API"

        ''' <summary>
        ''' Gets the underlying template storage service
        ''' </summary>
        Public ReadOnly Property Service As TemplateStorageService
            Get
                Return _service
            End Get
        End Property

        ''' <summary>
        ''' Gets the database path
        ''' </summary>
        Public ReadOnly Property DatabasePath As String
            Get
                Return _databasePath
            End Get
        End Property

#End Region

#Region "IDisposable Implementation"

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    If _service IsNot Nothing Then
                        _service.Dispose()
                        _service = Nothing
                    End If
                End If
                _disposed = True
            End If
        End Sub

#End Region

#Region "Private Helpers"

        ''' <summary>
        ''' Logs info message via reflection
        ''' </summary>
        Private Sub LogInfo(message As String)
            Try
                Dim method = _loggingService?.GetType().GetMethod("LogInfo")
                method?.Invoke(_loggingService, {message})
            Catch
                Console.WriteLine($"[TemplateStorage] {message}")
            End Try
        End Sub

        ''' <summary>
        ''' Logs warning message via reflection
        ''' </summary>
        Private Sub LogWarning(message As String)
            Try
                Dim method = _loggingService?.GetType().GetMethod("LogWarning")
                method?.Invoke(_loggingService, {message})
            Catch
                Console.WriteLine($"[TemplateStorage] WARNING: {message}")
            End Try
        End Sub

        ''' <summary>
        ''' Logs error message via reflection
        ''' </summary>
        Private Sub LogError(message As String)
            Try
                Dim method = _loggingService?.GetType().GetMethod("LogError")
                method?.Invoke(_loggingService, {message})
            Catch
                Console.WriteLine($"[TemplateStorage] ERROR: {message}")
            End Try
        End Sub

#End Region

    End Class

End Namespace
