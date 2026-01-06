''' ============================================================================
''' File: DuckTypedModuleWrapper.vb
''' Project: TheForge
''' Purpose: Wrapper to adapt duck-typed modules to IModule interface
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Wraps modules that use duck typing (matching method signatures but not
'''   implementing IModule directly) and adapts them to the IModule interface
'''   using reflection to invoke methods.
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System.Reflection

Namespace Modules.Implementations

    ''' <summary>
    ''' Wraps a duck-typed module to implement IModule interface
    ''' </summary>
    Public Class DuckTypedModuleWrapper
        Implements Modules.Interfaces.IModule

        Private ReadOnly _instance As Object
        Private ReadOnly _instanceType As Type
        Private _disposed As Boolean = False

        ''' <summary>
        ''' Creates a wrapper around a duck-typed module instance
        ''' </summary>
        Public Sub New(instance As Object)
            If instance Is Nothing Then
                Throw New ArgumentNullException(NameOf(instance))
            End If

            _instance = instance
            _instanceType = instance.GetType()
        End Sub

        ''' <summary>
        ''' Initializes the wrapped module
        ''' </summary>
        Public Sub Initialize(loggingService As Services.Interfaces.ILoggingService) Implements Modules.Interfaces.IModule.Initialize
            Dim method As MethodInfo = _instanceType.GetMethod("Initialize")
            If method IsNot Nothing Then
                method.Invoke(_instance, {loggingService})
            Else
                Throw New InvalidOperationException("Duck-typed module missing Initialize method")
            End If
        End Sub

        ''' <summary>
        ''' Loads configuration for the wrapped module
        ''' </summary>
        Public Sub LoadConfiguration(config As Modules.Interfaces.IModuleConfiguration) Implements Modules.Interfaces.IModule.LoadConfiguration
            Dim method As MethodInfo = _instanceType.GetMethod("LoadConfiguration")
            If method IsNot Nothing Then
                method.Invoke(_instance, {config})
            Else
                Throw New InvalidOperationException("Duck-typed module missing LoadConfiguration method")
            End If
        End Sub

        ''' <summary>
        ''' Executes the wrapped module
        ''' </summary>
        Public Sub Execute() Implements Modules.Interfaces.IModule.Execute
            Dim method As MethodInfo = _instanceType.GetMethod("Execute")
            If method IsNot Nothing Then
                method.Invoke(_instance, Nothing)
            Else
                Throw New InvalidOperationException("Duck-typed module missing Execute method")
            End If
        End Sub

        ''' <summary>
        ''' Called before the wrapped module is unloaded
        ''' </summary>
        Public Sub OnUnload() Implements Modules.Interfaces.IModule.OnUnload
            Dim method As MethodInfo = _instanceType.GetMethod("OnUnload")
            If method IsNot Nothing Then
                method.Invoke(_instance, Nothing)
            End If
        End Sub

        ''' <summary>
        ''' Disposes the wrapped module
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    ' Check if wrapped instance is IDisposable
                    Dim disposable As IDisposable = TryCast(_instance, IDisposable)
                    If disposable IsNot Nothing Then
                        disposable.Dispose()
                    Else
                        ' Try calling Dispose via reflection
                        Dim method As MethodInfo = _instanceType.GetMethod("Dispose")
                        If method IsNot Nothing Then
                            method.Invoke(_instance, Nothing)
                        End If
                    End If
                End If
                _disposed = True
            End If
        End Sub

    End Class

End Namespace
