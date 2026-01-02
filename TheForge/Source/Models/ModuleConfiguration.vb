Namespace Models
    ''' <summary>
    ''' Thread-safe implementation of IModuleConfiguration.
    ''' </summary>
    Public Class ModuleConfiguration
        Implements Modules.Interfaces.IModuleConfiguration

        Private ReadOnly _configData As Dictionary(Of String, String)
        Private ReadOnly _syncLock As Object

        Public Sub New()
            _configData = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
            _syncLock = New Object()
        End Sub

        Public ReadOnly Property ConfigurationData As Dictionary(Of String, String) Implements Modules.Interfaces.IModuleConfiguration.ConfigurationData
            Get
                SyncLock _syncLock
                    Return New Dictionary(Of String, String)(_configData)
                End SyncLock
            End Get
        End Property

        Public Function GetValue(key As String) As String Implements Modules.Interfaces.IModuleConfiguration.GetValue
            SyncLock _syncLock
                If _configData.ContainsKey(key) Then
                    Return _configData(key)
                End If
                Return String.Empty
            End SyncLock
        End Function

        Public Sub SetValue(key As String, value As String) Implements Modules.Interfaces.IModuleConfiguration.SetValue
            SyncLock _syncLock
                _configData(key) = value
            End SyncLock
        End Sub

        Public Function HasKey(key As String) As Boolean Implements Modules.Interfaces.IModuleConfiguration.HasKey
            SyncLock _syncLock
                Return _configData.ContainsKey(key)
            End SyncLock
        End Function
    End Class
End Namespace
