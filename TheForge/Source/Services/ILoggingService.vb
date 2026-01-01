Namespace Services
    ''' <summary>
    ''' Interface for logging and diagnostic services.
    ''' </summary>
    Public Interface ILoggingService
        ''' <summary>
        ''' Logs an informational message.
        ''' </summary>
        Sub LogInfo(message As String)

        ''' <summary>
        ''' Logs a warning message.
        ''' </summary>
        Sub LogWarning(message As String)

        ''' <summary>
        ''' Logs an error message.
        ''' </summary>
        Sub LogError(message As String)

        ''' <summary>
        ''' Clears all log entries.
        ''' </summary>
        Sub ClearLog()

        ''' <summary>
        ''' Gets all log entries.
        ''' </summary>
        Function GetLogEntries() As List(Of String)
    End Interface
End Namespace
