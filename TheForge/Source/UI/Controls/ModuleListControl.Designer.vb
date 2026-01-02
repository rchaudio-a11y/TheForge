Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace UI.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ModuleListControl
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
            lstModules = New ListBox()
            btnRunModule = New Button()
            btnUnloadModule = New Button()
            btnReloadModule = New Button()
            btnRefreshModules = New Button()
            SuspendLayout()
            ' 
            ' lstModules
            ' 
            lstModules.FormattingEnabled = True
            lstModules.Location = New Point(3, 3)
            lstModules.Name = "lstModules"
            lstModules.Size = New Size(244, 244)
            lstModules.TabIndex = 0
            ' 
            ' btnRunModule
            ' 
            btnRunModule.Enabled = False
            btnRunModule.Location = New Point(3, 253)
            btnRunModule.Name = "btnRunModule"
            btnRunModule.Size = New Size(244, 35)
            btnRunModule.TabIndex = 1
            btnRunModule.Text = "Run Module"
            btnRunModule.UseVisualStyleBackColor = True
            ' 
            ' btnUnloadModule
            ' 
            btnUnloadModule.Enabled = False
            btnUnloadModule.Location = New Point(3, 294)
            btnUnloadModule.Name = "btnUnloadModule"
            btnUnloadModule.Size = New Size(244, 35)
            btnUnloadModule.TabIndex = 2
            btnUnloadModule.Text = "Unload Module"
            btnUnloadModule.UseVisualStyleBackColor = True
            ' 
            ' btnReloadModule
            ' 
            btnReloadModule.Enabled = False
            btnReloadModule.Location = New Point(3, 335)
            btnReloadModule.Name = "btnReloadModule"
            btnReloadModule.Size = New Size(244, 35)
            btnReloadModule.TabIndex = 3
            btnReloadModule.Text = "Reload Module"
            btnReloadModule.UseVisualStyleBackColor = True
            ' 
            ' btnRefreshModules
            ' 
            btnRefreshModules.Location = New Point(3, 376)
            btnRefreshModules.Name = "btnRefreshModules"
            btnRefreshModules.Size = New Size(244, 35)
            btnRefreshModules.TabIndex = 4
            btnRefreshModules.Text = "Refresh Modules"
            btnRefreshModules.UseVisualStyleBackColor = True
            ' 
            ' ModuleListControl
            ' 
            AutoScaleDimensions = New SizeF(8.0F, 20.0F)
            AutoScaleMode = AutoScaleMode.Font
            BackColor = Color.FromArgb(CByte(240), CByte(240), CByte(240))
            Controls.Add(btnRefreshModules)
            Controls.Add(btnReloadModule)
            Controls.Add(btnUnloadModule)
            Controls.Add(btnRunModule)
            Controls.Add(lstModules)
            Name = "ModuleListControl"
            Size = New Size(250, 420)
            ResumeLayout(False)
        End Sub

        Friend WithEvents lstModules As ListBox
        Friend WithEvents btnRunModule As Button
        Friend WithEvents btnUnloadModule As Button
        Friend WithEvents btnReloadModule As Button
        Friend WithEvents btnRefreshModules As Button
    End Class
End Namespace
