Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace UI.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class LogOutputControl
        Inherits UserControl

        'UserControl overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.txtLogOutput = New System.Windows.Forms.TextBox()
            Me.lblLogLevel = New System.Windows.Forms.Label()
            Me.cmbLogLevel = New System.Windows.Forms.ComboBox()
            Me.lblLogSearch = New System.Windows.Forms.Label()
            Me.txtLogSearch = New System.Windows.Forms.TextBox()
            Me.btnApplyFilter = New System.Windows.Forms.Button()
            Me.btnClearLog = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'txtLogOutput
            '
            Me.txtLogOutput.Location = New System.Drawing.Point(3, 3)
            Me.txtLogOutput.Multiline = True
            Me.txtLogOutput.Name = "txtLogOutput"
            Me.txtLogOutput.ReadOnly = True
            Me.txtLogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtLogOutput.Size = New System.Drawing.Size(594, 194)
            Me.txtLogOutput.TabIndex = 0
            '
            'lblLogLevel
            '
            Me.lblLogLevel.AutoSize = True
            Me.lblLogLevel.Location = New System.Drawing.Point(3, 210)
            Me.lblLogLevel.Name = "lblLogLevel"
            Me.lblLogLevel.Size = New System.Drawing.Size(48, 20)
            Me.lblLogLevel.TabIndex = 1
            Me.lblLogLevel.Text = "Level:"
            '
            'cmbLogLevel
            '
            Me.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbLogLevel.FormattingEnabled = True
            Me.cmbLogLevel.Items.AddRange(New Object() {"All", "Info", "Warning", "Error"})
            Me.cmbLogLevel.Location = New System.Drawing.Point(57, 207)
            Me.cmbLogLevel.Name = "cmbLogLevel"
            Me.cmbLogLevel.Size = New System.Drawing.Size(140, 28)
            Me.cmbLogLevel.TabIndex = 2
            '
            'lblLogSearch
            '
            Me.lblLogSearch.AutoSize = True
            Me.lblLogSearch.Location = New System.Drawing.Point(210, 210)
            Me.lblLogSearch.Name = "lblLogSearch"
            Me.lblLogSearch.Size = New System.Drawing.Size(56, 20)
            Me.lblLogSearch.TabIndex = 3
            Me.lblLogSearch.Text = "Search:"
            '
            'txtLogSearch
            '
            Me.txtLogSearch.Location = New System.Drawing.Point(272, 207)
            Me.txtLogSearch.Name = "txtLogSearch"
            Me.txtLogSearch.Size = New System.Drawing.Size(200, 27)
            Me.txtLogSearch.TabIndex = 4
            '
            'btnApplyFilter
            '
            Me.btnApplyFilter.Location = New System.Drawing.Point(3, 241)
            Me.btnApplyFilter.Name = "btnApplyFilter"
            Me.btnApplyFilter.Size = New System.Drawing.Size(120, 29)
            Me.btnApplyFilter.TabIndex = 5
            Me.btnApplyFilter.Text = "Apply Search"
            Me.btnApplyFilter.UseVisualStyleBackColor = True
            '
            'btnClearLog
            '
            Me.btnClearLog.Location = New System.Drawing.Point(477, 241)
            Me.btnClearLog.Name = "btnClearLog"
            Me.btnClearLog.Size = New System.Drawing.Size(120, 29)
            Me.btnClearLog.TabIndex = 6
            Me.btnClearLog.Text = "Clear Log"
            Me.btnClearLog.UseVisualStyleBackColor = True
            '
            'LogOutputControl
            '
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.Controls.Add(Me.btnClearLog)
            Me.Controls.Add(Me.btnApplyFilter)
            Me.Controls.Add(Me.txtLogSearch)
            Me.Controls.Add(Me.lblLogSearch)
            Me.Controls.Add(Me.cmbLogLevel)
            Me.Controls.Add(Me.lblLogLevel)
            Me.Controls.Add(Me.txtLogOutput)
            Me.Name = "LogOutputControl"
            Me.Size = New System.Drawing.Size(600, 280)
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        Friend txtLogOutput As TextBox
        Friend lblLogLevel As Label
        Friend WithEvents cmbLogLevel As ComboBox
        Friend lblLogSearch As Label
        Friend WithEvents txtLogSearch As TextBox
        Friend WithEvents btnApplyFilter As Button
        Friend WithEvents btnClearLog As Button
    End Class
End Namespace
