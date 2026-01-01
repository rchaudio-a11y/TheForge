Namespace Services
    ''' <summary>
    ''' Basic implementation of ILoggingService for console and diagnostic logging.
    ''' </summary>
    Public Class LoggingService
        Implements ILoggingService

        Private ReadOnly _logEntries As List(Of String)

        Public Sub New()
            _logEntries = New List(Of String)()
        End Sub

        Public Sub LogInfo(message As String) Implements ILoggingService.LogInfo
            Dim entry As String = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}"
            _logEntries.Add(entry)
            Console.WriteLine(entry)
        End Sub

        Public Sub LogWarning(message As String) Implements ILoggingService.LogWarning
            Dim entry As String = $"[WARN] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}"
            _logEntries.Add(entry)
            Console.WriteLine(entry)
        End Sub

        Public Sub LogError(message As String) Implements ILoggingService.LogError
            Dim entry As String = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}"
            _logEntries.Add(entry)
            Console.WriteLine(entry)
        End Sub

        Public Sub ClearLog() Implements ILoggingService.ClearLog
            _logEntries.Clear()
        End Sub

        Public Function GetLogEntries() As List(Of String) Implements ILoggingService.GetLogEntries
            Return New List(Of String)(_logEntries)
        End Function
    End Class
End Namespace
