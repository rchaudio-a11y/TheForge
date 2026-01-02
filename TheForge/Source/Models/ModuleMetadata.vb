' **Character Count:** 1899
' **Document Type:** Code
' **Created:** 2024-11-01
' **Last Updated:** 2025-01-02
' **Related:** IModule.vb, ModuleLoaderService.vb

Namespace Models
    ''' <summary>
    ''' Represents metadata for a discovered module.
    ''' </summary>
    Public Class ModuleMetadata
        ''' <summary>
        ''' Gets or sets the file name of the module.
        ''' </summary>
        Public Property FileName As String

        ''' <summary>
        ''' Gets or sets the display name of the module.
        ''' </summary>
        Public Property DisplayName As String

        ''' <summary>
        ''' Gets or sets the full type name of the module class.
        ''' </summary>
        Public Property TypeName As String

        ''' <summary>
        ''' Gets or sets the last time this module was loaded.
        ''' </summary>
        Public Property LastLoadedTime As DateTime?

        ''' <summary>
        ''' Gets or sets whether the module is currently loaded.
        ''' </summary>
        Public Property IsLoaded As Boolean

        ''' <summary>
        ''' Gets or sets the cached module instance.
        ''' </summary>
        Public Property CachedInstance As Modules.Interfaces.IModule

        ''' <summary>
        ''' Gets or sets the array of module type names this module depends on.
        ''' </summary>
        Public Property Dependencies As String()

        Public Sub New()
            Me.IsLoaded = False
            Me.LastLoadedTime = Nothing
            Me.CachedInstance = Nothing
            Me.Dependencies = New String() {}
        End Sub

        Public Sub New(fileName As String, displayName As String)
            Me.FileName = fileName
            Me.DisplayName = displayName
            Me.TypeName = String.Empty
            Me.IsLoaded = False
            Me.LastLoadedTime = Nothing
            Me.CachedInstance = Nothing
            Me.Dependencies = New String() {}
        End Sub

        Public Sub New(fileName As String, displayName As String, typeName As String)
            Me.FileName = fileName
            Me.DisplayName = displayName
            Me.TypeName = typeName
            Me.IsLoaded = False
            Me.LastLoadedTime = Nothing
            Me.CachedInstance = Nothing
            Me.Dependencies = New String() {}
        End Sub
    End Class
End Namespace
