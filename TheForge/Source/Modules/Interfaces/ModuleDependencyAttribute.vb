Imports System

Namespace Modules.Interfaces
    ''' <summary>
    ''' Attribute to specify module dependencies.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)>
    Public Class ModuleDependencyAttribute
        Inherits Attribute

        ''' <summary>
        ''' Gets the array of module type names this module depends on.
        ''' </summary>
        Public Property DependsOn As String()

        Public Sub New(ParamArray dependsOn As String())
            Me.DependsOn = dependsOn
        End Sub
    End Class
End Namespace
