''' ============================================================================
''' File: ConnectionStringBuilder.vb
''' Project: RCH.TemplateStorage
''' Purpose: Connection string builder for MS Access database connections
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Provides centralized connection string building and validation for
'''   MS Access (.accdb) database connections. Supports multiple database
'''   engines (ACE, JET) and connection options.
''' 
''' Features:
'''   - Connection string generation for Access 2007+ (.accdb)
'''   - Path validation and normalization
'''   - Support for read-only and exclusive modes
'''   - Connection string testing and validation
''' 
''' Dependencies:
'''   - System.Data.OleDb (10.0.1)
'''   - MS Access Database Engine (ACE/JET)
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports System.Data.OleDb
Imports System.Text

Namespace Database

    ''' <summary>
    ''' Builds and validates connection strings for MS Access databases
    ''' </summary>
    Public Class ConnectionStringBuilder

#Region "Constants"

        ''' <summary>
        ''' Default provider for Access 2007+ (.accdb)
        ''' </summary>
        Private Const DefaultProvider As String = "Microsoft.ACE.OLEDB.12.0"

        ''' <summary>
        ''' Legacy provider for Access 2003 (.mdb)
        ''' </summary>
        Private Const LegacyProvider As String = "Microsoft.Jet.OLEDB.4.0"

#End Region

#Region "Private Fields"

        Private _databasePath As String
        Private _provider As String = DefaultProvider
        Private _readOnly As Boolean = False
        Private _exclusive As Boolean = False
        Private _persistSecurityInfo As Boolean = False

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constructor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor with database path
        ''' </summary>
        ''' <param name="databasePath">Full path to .accdb file</param>
        Public Sub New(databasePath As String)
            Me.DatabasePath = databasePath
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Gets or sets the database file path
        ''' </summary>
        Public Property DatabasePath As String
            Get
                Return _databasePath
            End Get
            Set(value As String)
                If String.IsNullOrWhiteSpace(value) Then
                    Throw New ArgumentNullException(NameOf(value), "Database path cannot be null or empty")
                End If
                _databasePath = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the OleDb provider
        ''' </summary>
        Public Property Provider As String
            Get
                Return _provider
            End Get
            Set(value As String)
                If String.IsNullOrWhiteSpace(value) Then
                    Throw New ArgumentNullException(NameOf(value), "Provider cannot be null or empty")
                End If
                _provider = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to open database in read-only mode
        ''' </summary>
        Public Property IsReadOnly As Boolean
            Get
                Return _readOnly
            End Get
            Set(value As Boolean)
                _readOnly = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to open database in exclusive mode
        ''' </summary>
        Public Property IsExclusive As Boolean
            Get
                Return _exclusive
            End Get
            Set(value As Boolean)
                _exclusive = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether to persist security info in connection string
        ''' </summary>
        Public Property PersistSecurityInfo As Boolean
            Get
                Return _persistSecurityInfo
            End Get
            Set(value As Boolean)
                _persistSecurityInfo = value
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Builds the connection string
        ''' </summary>
        ''' <returns>Complete OleDb connection string</returns>
        Public Function GetConnectionString() As String
            If String.IsNullOrWhiteSpace(_databasePath) Then
                Throw New InvalidOperationException("Database path must be set before building connection string")
            End If

            Dim builder As New StringBuilder()

            ' Provider
            builder.Append($"Provider={_provider};")

            ' Data Source
            builder.Append($"Data Source={_databasePath};")

            ' Additional options
            If _readOnly Then
                builder.Append("Mode=Read;")
            End If

            If _exclusive Then
                builder.Append("Mode=Share Exclusive;")
            End If

            If _persistSecurityInfo Then
                builder.Append("Persist Security Info=True;")
            Else
                builder.Append("Persist Security Info=False;")
            End If

            Return builder.ToString()
        End Function

        ''' <summary>
        ''' Validates that the database path exists
        ''' </summary>
        ''' <param name="errorMessage">Error message if validation fails</param>
        ''' <returns>True if path is valid</returns>
        Public Function ValidatePath(ByRef errorMessage As String) As Boolean
            If String.IsNullOrWhiteSpace(_databasePath) Then
                errorMessage = "Database path is not set"
                Return False
            End If

            ' Normalize path
            Try
                _databasePath = Path.GetFullPath(_databasePath)
            Catch ex As Exception
                errorMessage = $"Invalid path format: {ex.Message}"
                Return False
            End Try

            ' Check file extension
            Dim extension = Path.GetExtension(_databasePath).ToLower()
            If extension <> ".accdb" AndAlso extension <> ".mdb" Then
                errorMessage = $"Invalid database file extension: {extension}. Expected .accdb or .mdb"
                Return False
            End If

            ' Check if file exists (for existing databases)
            If Not File.Exists(_databasePath) Then
                errorMessage = $"Database file not found: {_databasePath}"
                Return False
            End If

            errorMessage = String.Empty
            Return True
        End Function

        ''' <summary>
        ''' Tests the connection string by attempting to open a connection
        ''' </summary>
        ''' <param name="errorMessage">Error message if connection fails</param>
        ''' <returns>True if connection successful</returns>
        Public Function TestConnection(ByRef errorMessage As String) As Boolean
            Try
                Dim connectionString = GetConnectionString()

                Using connection As New OleDbConnection(connectionString)
                    connection.Open()
                    connection.Close()
                End Using

                errorMessage = String.Empty
                Return True

            Catch ex As OleDbException
                errorMessage = $"Database connection failed: {ex.Message}"
                Return False
            Catch ex As Exception
                errorMessage = $"Connection test failed: {ex.Message}"
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Gets the directory containing the database file
        ''' </summary>
        ''' <returns>Directory path</returns>
        Public Function GetDatabaseDirectory() As String
            If String.IsNullOrWhiteSpace(_databasePath) Then
                Return String.Empty
            End If

            Return Path.GetDirectoryName(_databasePath)
        End Function

        ''' <summary>
        ''' Gets the database file name without path
        ''' </summary>
        ''' <returns>File name</returns>
        Public Function GetDatabaseFileName() As String
            If String.IsNullOrWhiteSpace(_databasePath) Then
                Return String.Empty
            End If

            Return Path.GetFileName(_databasePath)
        End Function

        ''' <summary>
        ''' Gets the database file name without extension
        ''' </summary>
        ''' <returns>File name without extension</returns>
        Public Function GetDatabaseFileNameWithoutExtension() As String
            If String.IsNullOrWhiteSpace(_databasePath) Then
                Return String.Empty
            End If

            Return Path.GetFileNameWithoutExtension(_databasePath)
        End Function

        ''' <summary>
        ''' Checks if the database file exists
        ''' </summary>
        ''' <returns>True if file exists</returns>
        Public Function DatabaseExists() As Boolean
            If String.IsNullOrWhiteSpace(_databasePath) Then
                Return False
            End If

            Return File.Exists(_databasePath)
        End Function

#End Region

#Region "Static Factory Methods"

        ''' <summary>
        ''' Creates a connection string builder for a new database
        ''' </summary>
        ''' <param name="databasePath">Path for new database</param>
        ''' <returns>Configured ConnectionStringBuilder</returns>
        Public Shared Function ForNewDatabase(databasePath As String) As ConnectionStringBuilder
            Return New ConnectionStringBuilder(databasePath) With {
                .Provider = DefaultProvider,
                .IsReadOnly = False,
                .IsExclusive = False
            }
        End Function

        ''' <summary>
        ''' Creates a connection string builder for an existing database
        ''' </summary>
        ''' <param name="databasePath">Path to existing database</param>
        ''' <returns>Configured ConnectionStringBuilder</returns>
        Public Shared Function ForExistingDatabase(databasePath As String) As ConnectionStringBuilder
            If Not File.Exists(databasePath) Then
                Throw New FileNotFoundException($"Database not found: {databasePath}")
            End If

            Return New ConnectionStringBuilder(databasePath) With {
                .Provider = DefaultProvider,
                .IsReadOnly = False,
                .IsExclusive = False
            }
        End Function

        ''' <summary>
        ''' Creates a connection string builder for read-only access
        ''' </summary>
        ''' <param name="databasePath">Path to existing database</param>
        ''' <returns>Configured ConnectionStringBuilder</returns>
        Public Shared Function ForReadOnly(databasePath As String) As ConnectionStringBuilder
            If Not File.Exists(databasePath) Then
                Throw New FileNotFoundException($"Database not found: {databasePath}")
            End If

            Return New ConnectionStringBuilder(databasePath) With {
                .Provider = DefaultProvider,
                .IsReadOnly = True,
                .IsExclusive = False
            }
        End Function

        ''' <summary>
        ''' Creates a connection string builder for exclusive access
        ''' </summary>
        ''' <param name="databasePath">Path to existing database</param>
        ''' <returns>Configured ConnectionStringBuilder</returns>
        Public Shared Function ForExclusive(databasePath As String) As ConnectionStringBuilder
            If Not File.Exists(databasePath) Then
                Throw New FileNotFoundException($"Database not found: {databasePath}")
            End If

            Return New ConnectionStringBuilder(databasePath) With {
                .Provider = DefaultProvider,
                .IsReadOnly = False,
                .IsExclusive = True
            }
        End Function

        ''' <summary>
        ''' Creates a connection string builder for legacy .mdb database
        ''' </summary>
        ''' <param name="databasePath">Path to .mdb database</param>
        ''' <returns>Configured ConnectionStringBuilder</returns>
        Public Shared Function ForLegacyDatabase(databasePath As String) As ConnectionStringBuilder
            Return New ConnectionStringBuilder(databasePath) With {
                .Provider = LegacyProvider,
                .IsReadOnly = False,
                .IsExclusive = False
            }
        End Function

#End Region

#Region "Overrides"

        ''' <summary>
        ''' Returns the connection string
        ''' </summary>
        Public Overrides Function ToString() As String
            Try
                Return GetConnectionString()
            Catch ex As Exception
                Return $"[Invalid Connection String: {ex.Message}]"
            End Try
        End Function

#End Region

    End Class

End Namespace
