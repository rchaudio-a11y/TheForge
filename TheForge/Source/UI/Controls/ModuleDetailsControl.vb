Imports System.Windows.Forms

Namespace UI.Controls
    ''' <summary>
    ''' UserControl for displaying module metadata details.
    ''' </summary>
    Partial Public Class ModuleDetailsControl
        Inherits UserControl

        ' NOTE: Control declarations are in ModuleDetailsControl.Designer.vb
        ' DO NOT declare them here - already declared as Friend in Designer

        Public Sub New()
            InitializeComponent()
        End Sub

        ' InitializeComponent() is now in ModuleDetailsControl.Designer.vb

        ''' <summary>
        ''' Updates the details display with module metadata.
        ''' </summary>
        Public Sub UpdateDetails(moduleMetadata As Models.ModuleMetadata)
            If moduleMetadata Is Nothing Then
                ClearDetails()
                Return
            End If

            txtFileName.Text = If(moduleMetadata.FileName, "N/A")
            txtDisplayName.Text = If(moduleMetadata.DisplayName, "N/A")
            txtTypeName.Text = If(String.IsNullOrEmpty(moduleMetadata.TypeName), "N/A", moduleMetadata.TypeName)
            txtLastLoadedTime.Text = If(moduleMetadata.LastLoadedTime.HasValue, moduleMetadata.LastLoadedTime.Value.ToString("yyyy-MM-dd HH:mm:ss"), "Never")
            txtIsLoaded.Text = If(moduleMetadata.IsLoaded, "Yes", "No")

            If moduleMetadata.Dependencies IsNot Nothing AndAlso moduleMetadata.Dependencies.Length > 0 Then
                txtDependencies.Text = String.Join(Environment.NewLine, moduleMetadata.Dependencies)
            Else
                txtDependencies.Text = "None"
            End If

            Dim modulesDir As String = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules")
            Dim configPath As String = IO.Path.Combine(modulesDir, IO.Path.GetFileNameWithoutExtension(moduleMetadata.FileName) & ".config")
            txtConfigPath.Text = If(IO.File.Exists(configPath), configPath, "Not found")
        End Sub

        ''' <summary>
        ''' Clears all detail fields.
        ''' </summary>
        Public Sub ClearDetails()
            txtFileName.Text = String.Empty
            txtDisplayName.Text = String.Empty
            txtTypeName.Text = String.Empty
            txtLastLoadedTime.Text = String.Empty
            txtIsLoaded.Text = String.Empty
            txtDependencies.Text = String.Empty
            txtConfigPath.Text = String.Empty
        End Sub
    End Class
End Namespace
