Imports System.Windows.Forms

Namespace UI.Controls
    ''' <summary>
    ''' UserControl for log output with filtering capabilities.
    ''' </summary>
    Partial Public Class LogOutputControl
        Inherits UserControl

        ' NOTE: Control declarations are in LogOutputControl.Designer.vb
        ' DO NOT declare them here - already declared as Friend WithEvents in Designer

        ''' <summary>
        ''' Raised when the Clear Log button is clicked.
        ''' </summary>
        Public Event ClearLogRequested As EventHandler

        ''' <summary>
        ''' Raised when filters are changed (dropdown or Apply Filter button).
        ''' </summary>
        Public Event FilterApplied As EventHandler(Of FilterAppliedEventArgs)

        Public Sub New()
            InitializeComponent()
        End Sub

        ' InitializeComponent() is now in LogOutputControl.Designer.vb

        ''' <summary>
        ''' Appends a log message to the output.
        ''' </summary>
        Public Sub AppendLog(message As String)
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Of String)(AddressOf AppendLog), message)
                Return
            End If

            txtLogOutput.AppendText(message & Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Rebuilds the log output with filtered entries.
        ''' </summary>
        Public Sub RebuildLog(entries As List(Of String), filterLevel As Services.Interfaces.LogLevel?, searchText As String)
            txtLogOutput.Clear()

            For Each entry As String In entries
                If ShouldDisplayEntry(entry, filterLevel, searchText) Then
                    txtLogOutput.AppendText(entry & Environment.NewLine)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Clears all log output.
        ''' </summary>
        Public Sub ClearLog()
            txtLogOutput.Clear()
        End Sub

        Private Function ShouldDisplayEntry(entry As String, filterLevel As Services.Interfaces.LogLevel?, searchText As String) As Boolean
            If filterLevel.HasValue Then
                Dim entryLevel As Services.Interfaces.LogLevel = GetEntryLevel(entry)
                If entryLevel < filterLevel.Value Then
                    Return False
                End If
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                If entry.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0 Then
                    Return False
                End If
            End If

            Return True
        End Function

        Private Function GetEntryLevel(entry As String) As Services.Interfaces.LogLevel
            If entry.StartsWith("[INFO]") Then
                Return Services.Interfaces.LogLevel.Info
            ElseIf entry.StartsWith("[WARN]") Then
                Return Services.Interfaces.LogLevel.Warning
            ElseIf entry.StartsWith("[ERROR]") Then
                Return Services.Interfaces.LogLevel.[Error]
            End If
            Return Services.Interfaces.LogLevel.Info
        End Function

        Private Sub cmbLogLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLogLevel.SelectedIndexChanged
            ApplyCurrentFilters()
        End Sub

        Private Sub txtLogSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLogSearch.KeyPress
            If e.KeyChar = Convert.ToChar(Keys.Enter) Then
                e.Handled = True
                ApplyCurrentFilters()
            End If
        End Sub

        Private Sub btnApplyFilter_Click(sender As Object, e As EventArgs) Handles btnApplyFilter.Click
            ApplyCurrentFilters()
        End Sub

        Private Sub ApplyCurrentFilters()
            If txtLogSearch Is Nothing OrElse cmbLogLevel Is Nothing Then
                Return
            End If

            Dim filterLevel As Services.Interfaces.LogLevel? = Nothing
            Dim searchText As String = txtLogSearch.Text.Trim()

            Select Case cmbLogLevel.SelectedIndex
                Case 0
                    filterLevel = Nothing
                Case 1
                    filterLevel = Services.Interfaces.LogLevel.Info
                Case 2
                    filterLevel = Services.Interfaces.LogLevel.Warning
                Case 3
                    filterLevel = Services.Interfaces.LogLevel.[Error]
            End Select

            RaiseEvent FilterApplied(Me, New FilterAppliedEventArgs(filterLevel, searchText))
        End Sub

        Private Sub btnClearLog_Click(sender As Object, e As EventArgs) Handles btnClearLog.Click
            RaiseEvent ClearLogRequested(Me, EventArgs.Empty)
        End Sub
    End Class

    ''' <summary>
    ''' Event arguments for filter applied event.
    ''' </summary>
    Public Class FilterAppliedEventArgs
        Inherits EventArgs

        Public Property FilterLevel As Services.Interfaces.LogLevel?
        Public Property SearchText As String

        Public Sub New(filterLevel As Services.Interfaces.LogLevel?, searchText As String)
            Me.FilterLevel = filterLevel
            Me.SearchText = searchText
        End Sub
    End Class
End Namespace
