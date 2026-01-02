' **Character Count:** TBD
' **Document Type:** Code
' **Created:** 2024-11-01
' **Last Updated:** 2025-01-02
' **Related:** ILoggingService.vb

Namespace Services.Implementations
    ''' <summary>
    ''' Implementation of ILoggingService for logging operations.
    ''' </summary>
    Public Class LoggingService
        Implements Services.Interfaces.ILoggingService

        Private ReadOnly _logEntries As List(Of String)
        Private _filterLevel As Services.Interfaces.LogLevel?
        Private _searchText As String

        Public Event LogMessageReceived As EventHandler(Of Services.Interfaces.LogMessageEventArgs) Implements Services.Interfaces.ILoggingService.LogMessageReceived

        Public Sub New()
            _logEntries = New List(Of String)()
            _filterLevel = Nothing
            _searchText = String.Empty
        End Sub

        Public Property FilterLevel As Services.Interfaces.LogLevel? Implements Services.Interfaces.ILoggingService.FilterLevel
            Get
                Return _filterLevel
            End Get
            Set(value As Services.Interfaces.LogLevel?)
                _filterLevel = value
            End Set
        End Property

        Public Property SearchText As String Implements Services.Interfaces.ILoggingService.SearchText
            Get
                Return _searchText
            End Get
            Set(value As String)
                _searchText = If(value, String.Empty)
            End Set
        End Property

        Public Sub LogInfo(message As String) Implements Services.Interfaces.ILoggingService.LogInfo
            Dim entry As String = String.Format("[INFO] {0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, message)
            _logEntries.Add(entry)

            If ShouldRaiseEvent(Services.Interfaces.LogLevel.Info, entry) Then
                RaiseEvent LogMessageReceived(Me, New Services.Interfaces.LogMessageEventArgs(entry, Services.Interfaces.LogLevel.Info))
            End If
        End Sub

        Public Sub LogWarning(message As String) Implements Services.Interfaces.ILoggingService.LogWarning
            Dim entry As String = String.Format("[WARN] {0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, message)
            _logEntries.Add(entry)

            If ShouldRaiseEvent(Services.Interfaces.LogLevel.Warning, entry) Then
                RaiseEvent LogMessageReceived(Me, New Services.Interfaces.LogMessageEventArgs(entry, Services.Interfaces.LogLevel.Warning))
            End If
        End Sub

        Public Sub LogError(message As String) Implements Services.Interfaces.ILoggingService.LogError
            Dim entry As String = String.Format("[ERROR] {0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, message)
            _logEntries.Add(entry)

            If ShouldRaiseEvent(Services.Interfaces.LogLevel.[Error], entry) Then
                RaiseEvent LogMessageReceived(Me, New Services.Interfaces.LogMessageEventArgs(entry, Services.Interfaces.LogLevel.[Error]))
            End If
        End Sub

        Public Sub ClearLog() Implements Services.Interfaces.ILoggingService.ClearLog
            _logEntries.Clear()
        End Sub

        Public Function GetLogEntries() As List(Of String) Implements Services.Interfaces.ILoggingService.GetLogEntries
            Return New List(Of String)(_logEntries)
        End Function

        Private Function ShouldRaiseEvent(level As Services.Interfaces.LogLevel, entry As String) As Boolean
            If _filterLevel.HasValue AndAlso level < _filterLevel.Value Then
                Return False
            End If

            If Not String.IsNullOrEmpty(_searchText) Then
                If entry.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) < 0 Then
                    Return False
                End If
            End If

            Return True
        End Function
    End Class
End Namespace
