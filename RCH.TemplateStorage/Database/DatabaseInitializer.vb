''' ============================================================================
''' File: DatabaseInitializer.vb
''' Project: RCH.TemplateStorage
''' Purpose: Database creation and initialization for MS Access
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Programmatically creates and initializes MS Access (.accdb) database
'''   for template storage. Uses ADOX for database creation and OleDb for
'''   schema execution. Supports validation and sample data insertion.
''' 
''' Dependencies:
'''   - System.Data.OleDb (10.0.1)
'''   - MS Access Database Engine (ACE/JET)
''' 
''' Methods:
'''   - CreateDatabase() - Creates new .accdb file
'''   - InitializeSchema() - Executes schema SQL
'''   - ValidateDatabaseSchema() - Verifies tables exist
'''   - InsertSampleData() - Populates test data
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports System.Data
Imports System.Data.OleDb

Namespace Database

    ''' <summary>
    ''' Creates and initializes MS Access database for template storage
    ''' </summary>
    Public Class DatabaseInitializer

#Region "Private Fields"

        Private ReadOnly _databasePath As String
        Private ReadOnly _connectionString As String

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor with database path
        ''' </summary>
        ''' <param name="databasePath">Full path to .accdb file</param>
        Public Sub New(databasePath As String)
            If String.IsNullOrWhiteSpace(databasePath) Then
                Throw New ArgumentNullException(NameOf(databasePath))
            End If

            _databasePath = databasePath
            _connectionString = BuildConnectionString(databasePath)
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Creates new MS Access database file
        ''' </summary>
        ''' <returns>True if created successfully</returns>
        Public Function CreateDatabase() As Boolean
            Try
                ' Check if file already exists
                If File.Exists(_databasePath) Then
                    Throw New InvalidOperationException($"Database already exists at: {_databasePath}")
                End If

                ' Ensure directory exists
                Dim directory = Path.GetDirectoryName(_databasePath)
                If Not String.IsNullOrEmpty(directory) AndAlso Not IO.Directory.Exists(directory) Then
                    IO.Directory.CreateDirectory(directory)
                End If

                ' Create .accdb database using ADOX (proven method from AccessSqlGeneratorControl)
                Dim connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_databasePath};Jet OLEDB:Engine Type=5"
                Dim catalog As Object = CreateObject("ADOX.Catalog")
                catalog.Create(connStr)
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(catalog)

                Return File.Exists(_databasePath)

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to create database: {ex.Message}. Install Microsoft Access Database Engine from: https://www.microsoft.com/download/details.aspx?id=54920", ex)
            End Try
        End Function

        ''' <summary>
        ''' Initializes database schema (creates tables, indexes, constraints)
        ''' </summary>
        ''' <param name="includeSampleData">Whether to insert sample data</param>
        ''' <returns>True if successful</returns>
        Public Function InitializeSchema(Optional includeSampleData As Boolean = False) As Boolean
            Try
                If Not File.Exists(_databasePath) Then
                    Throw New FileNotFoundException($"Database not found: {_databasePath}")
                End If

                Using connection As New OleDbConnection(_connectionString)
                    connection.Open()

                    ' Create tables
                    CreateTemplateTable(connection)
                    CreateTemplateFolderTable(connection)
                    CreateTemplateFileTable(connection)

                    ' Create indexes
                    CreateIndexes(connection)

                    ' Insert sample data if requested
                    If includeSampleData Then
                        InsertSampleData(connection)
                    End If
                End Using

                Return True

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to initialize schema: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Validates that database schema is correct
        ''' </summary>
        ''' <returns>True if schema is valid</returns>
        Public Function ValidateDatabaseSchema() As Boolean
            Try
                If Not File.Exists(_databasePath) Then
                    Return False
                End If

                Using connection As New OleDbConnection(_connectionString)
                    connection.Open()

                    ' Check for required tables
                    Dim requiredTables = {"Template", "TemplateFolder", "TemplateFile"}

                    For Each tableName In requiredTables
                        If Not TableExists(connection, tableName) Then
                            Return False
                        End If
                    Next

                    Return True
                End Using

            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Gets database file path
        ''' </summary>
        Public ReadOnly Property DatabasePath As String
            Get
                Return _databasePath
            End Get
        End Property

        ''' <summary>
        ''' Gets connection string
        ''' </summary>
        Public ReadOnly Property ConnectionString As String
            Get
                Return _connectionString
            End Get
        End Property

#End Region

#Region "Private Methods - Table Creation"

        ''' <summary>
        ''' Creates Template table
        ''' </summary>
        Private Sub CreateTemplateTable(connection As OleDbConnection)
            Dim sql = "CREATE TABLE Template (" &
                      "TemplateID AUTOINCREMENT PRIMARY KEY, " &
                      "Name TEXT(255) NOT NULL, " &
                      "Description MEMO, " &
                      "Category TEXT(100), " &
                      "Tags MEMO, " &
                      "Version TEXT(20) DEFAULT '1.0.0', " &
                      "CreatedBy TEXT(100), " &
                      "CreatedDate DATETIME DEFAULT NOW(), " &
                      "ModifiedBy TEXT(100), " &
                      "ModifiedDate DATETIME DEFAULT NOW(), " &
                      "UsageCount LONG DEFAULT 0, " &
                      "LastUsedDate DATETIME, " &
                      "IsActive BIT DEFAULT TRUE, " &
                      "Dependencies MEMO, " &
                      "Notes MEMO" &
                      ")"

            ExecuteNonQuery(connection, sql)
        End Sub

        ''' <summary>
        ''' Creates TemplateFolder table
        ''' </summary>
        Private Sub CreateTemplateFolderTable(connection As OleDbConnection)
            Dim sql = "CREATE TABLE TemplateFolder (" &
                      "FolderID AUTOINCREMENT PRIMARY KEY, " &
                      "TemplateID LONG NOT NULL, " &
                      "ParentFolderID LONG, " &
                      "Name TEXT(255) NOT NULL, " &
                      "IsSelected BIT DEFAULT TRUE, " &
                      "Description MEMO, " &
                      "CreatedDate DATETIME DEFAULT NOW(), " &
                      "DisplayOrder LONG DEFAULT 0" &
                      ")"

            ExecuteNonQuery(connection, sql)
        End Sub

        ''' <summary>
        ''' Creates TemplateFile table
        ''' </summary>
        Private Sub CreateTemplateFileTable(connection As OleDbConnection)
            Dim sql = "CREATE TABLE TemplateFile (" &
                      "FileID AUTOINCREMENT PRIMARY KEY, " &
                      "FolderID LONG NOT NULL, " &
                      "TemplateID LONG NOT NULL, " &
                      "FileName TEXT(255) NOT NULL, " &
                      "FileType TEXT(20), " &
                      "Description MEMO, " &
                      "ContentTemplate MEMO, " &
                      "RequiresMetadataHeader BIT DEFAULT FALSE, " &
                      "MetadataHeaderTemplate MEMO, " &
                      "Encoding TEXT(20) DEFAULT 'UTF-8', " &
                      "LineEnding TEXT(10) DEFAULT 'CRLF', " &
                      "IsAutoGenerated BIT DEFAULT TRUE, " &
                      "DisplayOrder LONG DEFAULT 0, " &
                      "CreatedDate DATETIME DEFAULT NOW(), " &
                      "IsActive BIT DEFAULT TRUE" &
                      ")"

            ExecuteNonQuery(connection, sql)
        End Sub

        ''' <summary>
        ''' Creates all indexes for performance
        ''' </summary>
        Private Sub CreateIndexes(connection As OleDbConnection)
            ' Template indexes
            ExecuteNonQuery(connection, "CREATE INDEX IDX_Template_Name ON Template(Name)")
            ExecuteNonQuery(connection, "CREATE INDEX IDX_Template_Category ON Template(Category)")
            ExecuteNonQuery(connection, "CREATE INDEX IDX_Template_IsActive ON Template(IsActive)")

            ' TemplateFolder indexes
            ExecuteNonQuery(connection, "CREATE INDEX IDX_TemplateFolder_TemplateID ON TemplateFolder(TemplateID)")
            ExecuteNonQuery(connection, "CREATE INDEX IDX_TemplateFolder_ParentFolderID ON TemplateFolder(ParentFolderID)")
            ExecuteNonQuery(connection, "CREATE INDEX IDX_TemplateFolder_Name ON TemplateFolder(Name)")

            ' TemplateFile indexes
            ExecuteNonQuery(connection, "CREATE INDEX IDX_TemplateFile_FolderID ON TemplateFile(FolderID)")
            ExecuteNonQuery(connection, "CREATE INDEX IDX_TemplateFile_TemplateID ON TemplateFile(TemplateID)")
            ExecuteNonQuery(connection, "CREATE INDEX IDX_TemplateFile_FileName ON TemplateFile(FileName)")
        End Sub

        ''' <summary>
        ''' Inserts sample data for testing
        ''' </summary>
        Private Sub InsertSampleData(connection As OleDbConnection)
            ' Insert sample template
            Dim templateSql = "INSERT INTO Template (Name, Description, Category, CreatedBy, Tags, IsActive) " &
                             "VALUES (?, ?, ?, ?, ?, ?)"

            Using cmd As New OleDbCommand(templateSql, connection)
                cmd.Parameters.AddWithValue("@Name", "Sample Programming Template")
                cmd.Parameters.AddWithValue("@Description", "Standard programming project structure")
                cmd.Parameters.AddWithValue("@Category", "Software Development")
                cmd.Parameters.AddWithValue("@CreatedBy", "System")
                cmd.Parameters.AddWithValue("@Tags", "programming,automation,vb.net")
                cmd.Parameters.AddWithValue("@IsActive", True)
                cmd.ExecuteNonQuery()
            End Using

            ' Get the inserted TemplateID
            Dim templateId As Integer
            Using cmd As New OleDbCommand("SELECT @@IDENTITY", connection)
                templateId = CInt(cmd.ExecuteScalar())
            End Using

            ' Insert root folders
            Dim folderIds As New List(Of Integer)
            Dim folders = {
                ("01_Input", "Input files and data", 0),
                ("02_Output", "Generated output files", 1),
                ("03_Documentation", "Project documentation", 2),
                ("04_Programming", "Source code and scripts", 3)
            }

            For Each folder In folders
                folderIds.Add(InsertFolder(connection, templateId, Nothing, folder.Item1, folder.Item2, folder.Item3))
            Next

            ' Insert subfolders under Programming (assuming it's the 4th folder)
            If folderIds.Count >= 4 Then
                Dim programmingFolderId = folderIds(3)
                InsertFolder(connection, templateId, programmingFolderId, "Robot", "Robot programming files", 0)
                InsertFolder(connection, templateId, programmingFolderId, "PLC", "PLC programming files", 1)
            End If

            ' Insert sample files
            If folderIds.Count >= 3 Then
                Dim docsFolderId = folderIds(2)
                InsertFile(connection, docsFolderId, templateId, "README.md", ".md",
                          "# {ProjectName}" & vbCrLf & vbCrLf & "## Overview" & vbCrLf & "{Description}",
                          False, 0)
            End If
        End Sub

        ''' <summary>
        ''' Inserts a folder and returns its ID
        ''' </summary>
        Private Function InsertFolder(connection As OleDbConnection, templateId As Integer,
                                     parentFolderId As Integer?, name As String,
                                     description As String, displayOrder As Integer) As Integer
            Dim sql = "INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder) " &
                     "VALUES (?, ?, ?, ?, ?, ?)"

            Using cmd As New OleDbCommand(sql, connection)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)
                cmd.Parameters.AddWithValue("@ParentFolderID", If(parentFolderId, DBNull.Value))
                cmd.Parameters.AddWithValue("@Name", name)
                cmd.Parameters.AddWithValue("@IsSelected", True)
                cmd.Parameters.AddWithValue("@Description", description)
                cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder)
                cmd.ExecuteNonQuery()
            End Using

            ' Get the inserted FolderID
            Using cmd As New OleDbCommand("SELECT @@IDENTITY", connection)
                Return CInt(cmd.ExecuteScalar())
            End Using
        End Function

        ''' <summary>
        ''' Inserts a file
        ''' </summary>
        Private Sub InsertFile(connection As OleDbConnection, folderId As Integer, templateId As Integer,
                              fileName As String, fileType As String, contentTemplate As String,
                              requiresHeader As Boolean, displayOrder As Integer)
            Dim sql = "INSERT INTO TemplateFile (FolderID, TemplateID, FileName, FileType, ContentTemplate, RequiresMetadataHeader, DisplayOrder) " &
                     "VALUES (?, ?, ?, ?, ?, ?, ?)"

            Using cmd As New OleDbCommand(sql, connection)
                cmd.Parameters.AddWithValue("@FolderID", folderId)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)
                cmd.Parameters.AddWithValue("@FileName", fileName)
                cmd.Parameters.AddWithValue("@FileType", fileType)
                cmd.Parameters.AddWithValue("@ContentTemplate", contentTemplate)
                cmd.Parameters.AddWithValue("@RequiresMetadataHeader", requiresHeader)
                cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

#End Region

#Region "Private Helper Methods"

        ''' <summary>
        ''' Builds OleDb connection string for Access database
        ''' </summary>
        Private Shared Function BuildConnectionString(databasePath As String) As String
            Return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};"
        End Function

        ''' <summary>
        ''' Executes a non-query SQL command
        ''' </summary>
        Private Sub ExecuteNonQuery(connection As OleDbConnection, sql As String)
            Using cmd As New OleDbCommand(sql, connection)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

        ''' <summary>
        ''' Checks if a table exists in the database
        ''' </summary>
        Private Function TableExists(connection As OleDbConnection, tableName As String) As Boolean
            Try
                Dim schema = connection.GetSchema("Tables")
                For Each row As DataRow In schema.Rows
                    If row("TABLE_NAME").ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase) Then
                        Return True
                    End If
                Next
                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace
