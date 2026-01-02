Imports System.Windows.Forms

Namespace UI.Controls
    ''' <summary>
    ''' UserControl for log output with filtering capabilities.
    ''' </summary>
    Public Class LogOutputControl
        Inherits UserControl

        Private txtLogOutput As TextBox
        Private pnlLogFilter As Panel
        Private filterTable As TableLayoutPanel
        Private lblLogLevel As Label
        Private WithEvents cmbLogLevel As ComboBox
        Private lblLogSearch As Label
        Private WithEvents txtLogSearch As TextBox
        Private WithEvents btnApplyFilter As Button
        Private WithEvents btnClearLog As Button

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

        Private Sub InitializeComponent()
            Me.SuspendLayout()

            txtLogOutput = New TextBox()
            txtLogOutput.Name = "txtLogOutput"
            txtLogOutput.Dock = DockStyle.Fill
            txtLogOutput.Multiline = True
            txtLogOutput.ReadOnly = True
            txtLogOutput.ScrollBars = ScrollBars.Vertical

            pnlLogFilter = New Panel()
            pnlLogFilter.Name = "pnlLogFilter"
            pnlLogFilter.Dock = DockStyle.Bottom
            pnlLogFilter.Height = 70
            pnlLogFilter.Padding = New Padding(5)

            filterTable = New TableLayoutPanel()
            filterTable.Name = "filterTable"
            filterTable.Dock = DockStyle.Fill
            filterTable.ColumnCount = 2
            filterTable.RowCount = 3
            filterTable.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80.0F))
            filterTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

            lblLogLevel = New Label()
            lblLogLevel.Name = "lblLogLevel"
            lblLogLevel.Text = "Level:"
            lblLogLevel.Dock = DockStyle.Fill
            lblLogLevel.TextAlign = ContentAlignment.MiddleLeft

            cmbLogLevel = New ComboBox()
            cmbLogLevel.Name = "cmbLogLevel"
            cmbLogLevel.Dock = DockStyle.Fill
            cmbLogLevel.DropDownStyle = ComboBoxStyle.DropDownList
            cmbLogLevel.Items.Add("All")
            cmbLogLevel.Items.Add("Info")
            cmbLogLevel.Items.Add("Warning")
            cmbLogLevel.Items.Add("Error")

            lblLogSearch = New Label()
            lblLogSearch.Name = "lblLogSearch"
            lblLogSearch.Text = "Search:"
            lblLogSearch.Dock = DockStyle.Fill
            lblLogSearch.TextAlign = ContentAlignment.MiddleLeft

            txtLogSearch = New TextBox()
            txtLogSearch.Name = "txtLogSearch"
            txtLogSearch.Dock = DockStyle.Fill

            btnApplyFilter = New Button()
            btnApplyFilter.Name = "btnApplyFilter"
            btnApplyFilter.Text = "Apply Search"
            btnApplyFilter.Dock = DockStyle.Fill
            btnApplyFilter.Height = 25

            btnClearLog = New Button()
            btnClearLog.Name = "btnClearLog"
            btnClearLog.Text = "Clear Log"
            btnClearLog.Dock = DockStyle.Fill
            btnClearLog.Height = 25

            filterTable.Controls.Add(lblLogLevel, 0, 0)
            filterTable.Controls.Add(cmbLogLevel, 1, 0)
            filterTable.Controls.Add(lblLogSearch, 0, 1)
            filterTable.Controls.Add(txtLogSearch, 1, 1)
            filterTable.Controls.Add(btnApplyFilter, 0, 2)
            filterTable.Controls.Add(btnClearLog, 1, 2)

            pnlLogFilter.Controls.Add(filterTable)

            Me.Controls.Add(txtLogOutput)
            Me.Controls.Add(pnlLogFilter)

            Me.Name = "LogOutputControl"
            Me.BackColor = Color.FromArgb(250, 250, 250)

            Me.ResumeLayout(False)

            cmbLogLevel.SelectedIndex = 0
        End Sub

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
