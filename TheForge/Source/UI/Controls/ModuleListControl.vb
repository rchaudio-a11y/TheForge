Imports System.Windows.Forms

Namespace UI.Controls
    ''' <summary>
    ''' UserControl for module list and action buttons.
    ''' </summary>
    Public Class ModuleListControl
        Inherits UserControl

        Private WithEvents lstModules As ListBox
        Private WithEvents btnRunModule As Button
        Private WithEvents btnUnloadModule As Button
        Private WithEvents btnReloadModule As Button
        Private WithEvents btnRefreshModules As Button

        ''' <summary>
        ''' Raised when a module is selected from the list.
        ''' </summary>
        Public Event ModuleSelected As EventHandler(Of ModuleSelectedEventArgs)

        ''' <summary>
        ''' Raised when the Run button is clicked.
        ''' </summary>
        Public Event RunRequested As EventHandler

        ''' <summary>
        ''' Raised when the Unload button is clicked.
        ''' </summary>
        Public Event UnloadRequested As EventHandler

        ''' <summary>
        ''' Raised when the Refresh button is clicked.
        ''' </summary>
        Public Event RefreshRequested As EventHandler

        ''' <summary>
        ''' Raised when the Reload button is clicked.
        ''' </summary>
        Public Event ReloadRequested As EventHandler

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.SuspendLayout()

            lstModules = New ListBox()
            lstModules.Name = "lstModules"
            lstModules.Dock = DockStyle.Fill

            btnRefreshModules = New Button()
            btnRefreshModules.Name = "btnRefreshModules"
            btnRefreshModules.Text = "Refresh Modules"
            btnRefreshModules.Dock = DockStyle.Bottom
            btnRefreshModules.Height = 35

            btnReloadModule = New Button()
            btnReloadModule.Name = "btnReloadModule"
            btnReloadModule.Text = "Reload Module"
            btnReloadModule.Dock = DockStyle.Bottom
            btnReloadModule.Height = 35
            btnReloadModule.Enabled = False

            btnUnloadModule = New Button()
            btnUnloadModule.Name = "btnUnloadModule"
            btnUnloadModule.Text = "Unload Module"
            btnUnloadModule.Dock = DockStyle.Bottom
            btnUnloadModule.Height = 35
            btnUnloadModule.Enabled = False

            btnRunModule = New Button()
            btnRunModule.Name = "btnRunModule"
            btnRunModule.Text = "Run Module"
            btnRunModule.Dock = DockStyle.Bottom
            btnRunModule.Height = 35
            btnRunModule.Enabled = False

            Me.Controls.Add(lstModules)
            Me.Controls.Add(btnRefreshModules)
            Me.Controls.Add(btnReloadModule)
            Me.Controls.Add(btnUnloadModule)
            Me.Controls.Add(btnRunModule)

            Me.Name = "ModuleListControl"
            Me.BackColor = Color.FromArgb(240, 240, 240)

            Me.ResumeLayout(False)
        End Sub

        ''' <summary>
        ''' Loads modules into the list.
        ''' </summary>
        Public Sub LoadModules(modules As List(Of Models.ModuleMetadata))
            Dim previousSelection As Integer = lstModules.SelectedIndex

            lstModules.Items.Clear()

            For Each moduleMetadata As Models.ModuleMetadata In modules
                lstModules.Items.Add(moduleMetadata.DisplayName)
            Next

            If previousSelection >= 0 AndAlso previousSelection < lstModules.Items.Count Then
                lstModules.SelectedIndex = previousSelection
            End If
        End Sub

        ''' <summary>
        ''' Gets the currently selected module index.
        ''' </summary>
        Public ReadOnly Property SelectedIndex As Integer
            Get
                Return lstModules.SelectedIndex
            End Get
        End Property

        ''' <summary>
        ''' Sets the selected module index.
        ''' </summary>
        Public Sub SetSelectedIndex(index As Integer)
            If index >= 0 AndAlso index < lstModules.Items.Count Then
                lstModules.SelectedIndex = index
            End If
        End Sub

        ''' <summary>
        ''' Updates button states based on module state.
        ''' </summary>
        Public Sub UpdateButtonStates(canRun As Boolean, canUnload As Boolean, canReload As Boolean)
            btnRunModule.Enabled = canRun
            btnUnloadModule.Enabled = canUnload
            btnReloadModule.Enabled = canReload
        End Sub

        Private Sub lstModules_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstModules.SelectedIndexChanged
            RaiseEvent ModuleSelected(Me, New ModuleSelectedEventArgs(lstModules.SelectedIndex))
        End Sub

        Private Sub btnRunModule_Click(sender As Object, e As EventArgs) Handles btnRunModule.Click
            RaiseEvent RunRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub btnUnloadModule_Click(sender As Object, e As EventArgs) Handles btnUnloadModule.Click
            RaiseEvent UnloadRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub btnReloadModule_Click(sender As Object, e As EventArgs) Handles btnReloadModule.Click
            RaiseEvent ReloadRequested(Me, EventArgs.Empty)
        End Sub

        Private Sub btnRefreshModules_Click(sender As Object, e As EventArgs) Handles btnRefreshModules.Click
            RaiseEvent RefreshRequested(Me, EventArgs.Empty)
        End Sub
    End Class

    ''' <summary>
    ''' Event arguments for module selection.
    ''' </summary>
    Public Class ModuleSelectedEventArgs
        Inherits EventArgs

        Public Property SelectedIndex As Integer

        Public Sub New(selectedIndex As Integer)
            Me.SelectedIndex = selectedIndex
        End Sub
    End Class
End Namespace
