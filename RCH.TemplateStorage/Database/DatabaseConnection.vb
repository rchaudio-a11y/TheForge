''' ============================================================================
''' File: DatabaseConnection.vb
''' Project: RCH.TemplateStorage
''' Purpose: Database connection wrapper with IDisposable pattern
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Provides safe database connection management with automatic cleanup
'''   using IDisposable pattern. Wraps OleDbConnection for MS Access databases
'''   with connection lifecycle management, error handling, and resource disposal.
''' 
''' Features:
'''   - IDisposable implementation for automatic cleanup
'''   - Connection state management
'''   - Transaction support
'''   - Connection pooling (inherent in OleDb)
'''   - Error handling and logging
''' 
''' Dependencies:
'''   - System.Data.OleDb (10.0.1)
'''   - ConnectionStringBuilder.vb
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.Data
Imports System.Data.OleDb

Namespace Database

    ''' <summary>
    ''' Database connection wrapper implementing IDisposable
    ''' </summary>
    Public Class DatabaseConnection
        Implements IDisposable

#Region "Private Fields"

        Private _connection As OleDbConnection
        Private _connectionString As String
        Private _disposed As Boolean = False

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor with connection string
        ''' </summary>
        ''' <param name="connectionString">OleDb connection string</param>
        Public Sub New(connectionString As String)
            If String.IsNullOrWhiteSpace(connectionString) Then
                Throw New ArgumentNullException(NameOf(connectionString))
            End If

            _connectionString = connectionString
            _connection = New OleDbConnection(connectionString)
        End Sub

        ''' <summary>
        ''' Constructor with ConnectionStringBuilder
        ''' </summary>
        ''' <param name="builder">Connection string builder</param>
        Public Sub New(builder As ConnectionStringBuilder)
            If builder Is Nothing Then
                Throw New ArgumentNullException(NameOf(builder))
            End If

            _connectionString = builder.GetConnectionString()
            _connection = New OleDbConnection(_connectionString)
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Gets the underlying OleDbConnection
        ''' </summary>
        Public ReadOnly Property Connection As OleDbConnection
            Get
                CheckDisposed()
                Return _connection
            End Get
        End Property

        ''' <summary>
        ''' Gets the connection string
        ''' </summary>
        Public ReadOnly Property ConnectionString As String
            Get
                Return _connectionString
            End Get
        End Property

        ''' <summary>
        ''' Gets the current connection state
        ''' </summary>
        Public ReadOnly Property State As ConnectionState
            Get
                CheckDisposed()
                Return _connection.State
            End Get
        End Property

        ''' <summary>
        ''' Gets whether the connection is open
        ''' </summary>
        Public ReadOnly Property IsOpen As Boolean
            Get
                CheckDisposed()
                Return _connection.State = ConnectionState.Open
            End Get
        End Property

#End Region

#Region "Connection Management Methods"

        ''' <summary>
        ''' Opens the database connection
        ''' </summary>
        Public Sub Open()
            CheckDisposed()

            If _connection.State = ConnectionState.Open Then
                Return ' Already open
            End If

            Try
                _connection.Open()
            Catch ex As OleDbException
                Throw New InvalidOperationException($"Failed to open database connection: {ex.Message}", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Closes the database connection
        ''' </summary>
        Public Sub Close()
            CheckDisposed()

            If _connection.State <> ConnectionState.Closed Then
                _connection.Close()
            End If
        End Sub

        ''' <summary>
        ''' Ensures connection is open (opens if closed)
        ''' </summary>
        Public Sub EnsureOpen()
            CheckDisposed()

            If _connection.State <> ConnectionState.Open Then
                Open()
            End If
        End Sub

#End Region

#Region "Command Execution Methods"

        ''' <summary>
        ''' Executes a non-query SQL command
        ''' </summary>
        ''' <param name="sql">SQL command text</param>
        ''' <returns>Number of rows affected</returns>
        Public Function ExecuteNonQuery(sql As String) As Integer
            CheckDisposed()
            EnsureOpen()

            Using cmd As New OleDbCommand(sql, _connection)
                Return cmd.ExecuteNonQuery()
            End Using
        End Function

        ''' <summary>
        ''' Executes a non-query SQL command with parameters
        ''' </summary>
        ''' <param name="sql">SQL command text</param>
        ''' <param name="parameters">Command parameters</param>
        ''' <returns>Number of rows affected</returns>
        Public Function ExecuteNonQuery(sql As String, ParamArray parameters As OleDbParameter()) As Integer
            CheckDisposed()
            EnsureOpen()

            Using cmd As New OleDbCommand(sql, _connection)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Function

        ''' <summary>
        ''' Executes a scalar query and returns the first column of the first row
        ''' </summary>
        ''' <param name="sql">SQL query text</param>
        ''' <returns>Scalar value</returns>
        Public Function ExecuteScalar(sql As String) As Object
            CheckDisposed()
            EnsureOpen()

            Using cmd As New OleDbCommand(sql, _connection)
                Return cmd.ExecuteScalar()
            End Using
        End Function

        ''' <summary>
        ''' Executes a query and returns a data reader
        ''' </summary>
        ''' <param name="sql">SQL query text</param>
        ''' <returns>OleDbDataReader</returns>
        Public Function ExecuteReader(sql As String) As OleDbDataReader
            CheckDisposed()
            EnsureOpen()

            Dim cmd As New OleDbCommand(sql, _connection)
            Return cmd.ExecuteReader()
        End Function

        ''' <summary>
        ''' Creates a new command for this connection
        ''' </summary>
        ''' <returns>OleDbCommand</returns>
        Public Function CreateCommand() As OleDbCommand
            CheckDisposed()
            EnsureOpen()

            Return _connection.CreateCommand()
        End Function

#End Region

#Region "Transaction Methods"

        ''' <summary>
        ''' Begins a database transaction
        ''' </summary>
        ''' <returns>OleDbTransaction</returns>
        Public Function BeginTransaction() As OleDbTransaction
            CheckDisposed()
            EnsureOpen()

            Return _connection.BeginTransaction()
        End Function

        ''' <summary>
        ''' Begins a database transaction with isolation level
        ''' </summary>
        ''' <param name="isolationLevel">Transaction isolation level</param>
        ''' <returns>OleDbTransaction</returns>
        Public Function BeginTransaction(isolationLevel As IsolationLevel) As OleDbTransaction
            CheckDisposed()
            EnsureOpen()

            Return _connection.BeginTransaction(isolationLevel)
        End Function

#End Region

#Region "Utility Methods"

        ''' <summary>
        ''' Tests the connection
        ''' </summary>
        ''' <returns>True if connection successful</returns>
        Public Function TestConnection() As Boolean
            Try
                CheckDisposed()
                Open()
                Close()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Gets the database schema information
        ''' </summary>
        ''' <param name="collectionName">Schema collection name (e.g., "Tables", "Columns")</param>
        ''' <returns>DataTable with schema information</returns>
        Public Function GetSchema(collectionName As String) As DataTable
            CheckDisposed()
            EnsureOpen()

            Return _connection.GetSchema(collectionName)
        End Function

        ''' <summary>
        ''' Gets all table names in the database
        ''' </summary>
        ''' <returns>List of table names</returns>
        Public Function GetTableNames() As List(Of String)
            CheckDisposed()
            EnsureOpen()

            Dim tables As New List(Of String)
            Dim schema = _connection.GetSchema("Tables")

            For Each row As DataRow In schema.Rows
                Dim tableName = row("TABLE_NAME").ToString()
                Dim tableType = row("TABLE_TYPE").ToString()

                ' Only include user tables (not system tables)
                If tableType = "TABLE" Then
                    tables.Add(tableName)
                End If
            Next

            Return tables
        End Function

#End Region

#Region "IDisposable Implementation"

        ''' <summary>
        ''' Disposes the database connection
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Protected dispose method
        ''' </summary>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    ' Dispose managed resources
                    If _connection IsNot Nothing Then
                        If _connection.State <> ConnectionState.Closed Then
                            _connection.Close()
                        End If
                        _connection.Dispose()
                        _connection = Nothing
                    End If
                End If

                _disposed = True
            End If
        End Sub

        ''' <summary>
        ''' Finalizer
        ''' </summary>
        Protected Overrides Sub Finalize()
            Dispose(False)
        End Sub

        ''' <summary>
        ''' Checks if object has been disposed
        ''' </summary>
        Private Sub CheckDisposed()
            If _disposed Then
                Throw New ObjectDisposedException(GetType(DatabaseConnection).Name)
            End If
        End Sub

#End Region

    End Class

End Namespace
