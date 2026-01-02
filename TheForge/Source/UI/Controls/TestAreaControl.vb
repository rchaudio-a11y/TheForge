Imports System.Windows.Forms

Namespace UI.Controls
    ''' <summary>
    ''' UserControl placeholder for module UI testing area.
    ''' </summary>
    Public Class TestAreaControl
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.SuspendLayout()

            Me.Name = "TestAreaControl"
            Me.BackColor = Color.White

            Me.ResumeLayout(False)
        End Sub
    End Class
End Namespace
