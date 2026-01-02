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
            txtLogOutput = New TextBox()
            pnlLogFilter = New Panel()
            filterTable = New TableLayoutPanel()
            lblLogLevel = New Label()
            cmbLogLevel = New ComboBox()
            lblLogSearch = New Label()
            txtLogSearch = New TextBox()
            btnApplyFilter = New Button()
            btnClearLog = New Button()
            pnlLogFilter.SuspendLayout()
            filterTable.SuspendLayout()
            SuspendLayout()
            ' 
            ' txtLogOutput
            ' 
            txtLogOutput.Dock = DockStyle.Fill
            txtLogOutput.Location = New Point(0, 0)
            txtLogOutput.Multiline = True
            txtLogOutput.Name = "txtLogOutput"
            txtLogOutput.ReadOnly = True
            txtLogOutput.ScrollBars = ScrollBars.Vertical
            txtLogOutput.Size = New Size(150, 80)
            txtLogOutput.TabIndex = 0
            ' 
            ' pnlLogFilter
            ' 
            pnlLogFilter.Controls.Add(filterTable)
            pnlLogFilter.Dock = DockStyle.Bottom
            pnlLogFilter.Location = New Point(0, 80)
            pnlLogFilter.Name = "pnlLogFilter"
            pnlLogFilter.Padding = New Padding(5)
            pnlLogFilter.Size = New Size(150, 70)
            pnlLogFilter.TabIndex = 1
            ' 
            ' filterTable
            ' 
            filterTable.ColumnCount = 2
            filterTable.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80F))
            filterTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
            filterTable.Controls.Add(lblLogLevel, 0, 0)
            filterTable.Controls.Add(cmbLogLevel, 1, 0)
            filterTable.Controls.Add(lblLogSearch, 0, 1)
            filterTable.Controls.Add(txtLogSearch, 1, 1)
            filterTable.Controls.Add(btnApplyFilter, 0, 2)
            filterTable.Controls.Add(btnClearLog, 1, 2)
            filterTable.Dock = DockStyle.Fill
            filterTable.Location = New Point(5, 5)
            filterTable.Name = "filterTable"
            filterTable.RowCount = 3
            filterTable.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
            filterTable.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
            filterTable.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
            filterTable.Size = New Size(140, 60)
            filterTable.TabIndex = 0
            ' 
            ' lblLogLevel
            ' 
            lblLogLevel.Dock = DockStyle.Fill
            lblLogLevel.Location = New Point(3, 0)
            lblLogLevel.Name = "lblLogLevel"
            lblLogLevel.Size = New Size(74, 20)
            lblLogLevel.TabIndex = 0
            lblLogLevel.Text = "Level:"
            lblLogLevel.TextAlign = ContentAlignment.MiddleLeft
            ' 
            ' cmbLogLevel
            ' 
            cmbLogLevel.Dock = DockStyle.Fill
            cmbLogLevel.DropDownStyle = ComboBoxStyle.DropDownList
            cmbLogLevel.Items.AddRange(New Object() {"All", "Info", "Warning", "Error"})
            cmbLogLevel.Location = New Point(83, 3)
            cmbLogLevel.Name = "cmbLogLevel"
            cmbLogLevel.Size = New Size(54, 28)
            cmbLogLevel.TabIndex = 1
            ' 
            ' lblLogSearch
            ' 
            lblLogSearch.Dock = DockStyle.Fill
            lblLogSearch.Location = New Point(3, 20)
            lblLogSearch.Name = "lblLogSearch"
            lblLogSearch.Size = New Size(74, 20)
            lblLogSearch.TabIndex = 2
            lblLogSearch.Text = "Search:"
            lblLogSearch.TextAlign = ContentAlignment.MiddleLeft
            ' 
            ' txtLogSearch
            ' 
            txtLogSearch.Dock = DockStyle.Fill
            txtLogSearch.Location = New Point(83, 23)
            txtLogSearch.Name = "txtLogSearch"
            txtLogSearch.Size = New Size(54, 27)
            txtLogSearch.TabIndex = 3
            ' 
            ' btnApplyFilter
            ' 
            btnApplyFilter.Dock = DockStyle.Fill
            btnApplyFilter.Location = New Point(3, 43)
            btnApplyFilter.Name = "btnApplyFilter"
            btnApplyFilter.Size = New Size(74, 14)
            btnApplyFilter.TabIndex = 4
            btnApplyFilter.Text = "Apply Search"
            ' 
            ' btnClearLog
            ' 
            btnClearLog.Dock = DockStyle.Fill
            btnClearLog.Location = New Point(83, 43)
            btnClearLog.Name = "btnClearLog"
            btnClearLog.Size = New Size(54, 14)
            btnClearLog.TabIndex = 5
            btnClearLog.Text = "Clear Log"
            ' 
            ' LogOutputControl
            ' 
            BackColor = Color.FromArgb(CByte(250), CByte(250), CByte(250))
            Controls.Add(txtLogOutput)
            Controls.Add(pnlLogFilter)
            Name = "LogOutputControl"
            pnlLogFilter.ResumeLayout(False)
            filterTable.ResumeLayout(False)
            filterTable.PerformLayout()
            ResumeLayout(False)
            PerformLayout()
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
