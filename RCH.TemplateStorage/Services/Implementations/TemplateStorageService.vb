''' ============================================================================
''' File: TemplateStorageService.vb
''' Project: RCH.TemplateStorage
''' Purpose: Service implementation for template CRUD operations
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active - Phase 4
''' Version: 1.0.0
''' 
''' Description:
'''   Complete implementation of ITemplateStorageService providing CRUD
'''   operations for templates with full database persistence. Handles
'''   template creation, retrieval, updating, deletion, search, and
'''   JSON import/export with automatic database synchronization.
''' 
''' Features:
'''   - Full CRUD operations with transaction support
'''   - Hierarchical folder/file persistence (unlimited nesting)
'''   - JSON import/export with legacy compatibility
'''   - Search by name, tag, category, keyword
'''   - Database validation and connection testing
'''   - Automatic relationship management (cascade operations)
''' 
''' Dependencies:
'''   - DatabaseConnection.vb - Connection management
'''   - DatabaseValidator.vb - Schema validation
'''   - TemplateJsonSerializer.vb - JSON operations
'''   - Models: TemplateDefinition, TemplateFolderDefinition, TemplateFileDefinition
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.IO
Imports System.Linq
Imports RCH.TemplateStorage.Models
Imports RCH.TemplateStorage.Database
Imports RCH.TemplateStorage.Services.Implementations

Namespace Services.Implementations

    ''' <summary>
    ''' Service implementation for template storage operations
    ''' </summary>
    Public Class TemplateStorageService
        Implements Services.Interfaces.ITemplateStorageService

#Region "Private Fields"

        Private ReadOnly _connection As DatabaseConnection
        Private ReadOnly _serializer As TemplateJsonSerializer
        Private ReadOnly _databasePath As String
        Private _disposed As Boolean = False

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor with database path
        ''' </summary>
        Public Sub New(databasePath As String)
            If String.IsNullOrWhiteSpace(databasePath) Then
                Throw New ArgumentNullException(NameOf(databasePath))
            End If

            _databasePath = databasePath
            
            ' Build connection string from path
            Dim builder As New ConnectionStringBuilder(databasePath)
            _connection = New DatabaseConnection(builder)
            _serializer = New TemplateJsonSerializer()

            ' Ensure connection opens successfully
            _connection.EnsureOpen()
        End Sub

        ''' <summary>
        ''' Constructor with existing connection
        ''' </summary>
        Public Sub New(connection As DatabaseConnection)
            If connection Is Nothing Then
                Throw New ArgumentNullException(NameOf(connection))
            End If

            _connection = connection
            _databasePath = connection.ConnectionString
            _serializer = New TemplateJsonSerializer()
        End Sub

#End Region

#Region "ITemplateStorageService Implementation - CRUD"

        ''' <summary>
        ''' Creates a new template in the database
        ''' </summary>
        Public Function CreateTemplate(template As TemplateDefinition) As TemplateDefinition Implements Services.Interfaces.ITemplateStorageService.CreateTemplate
            If template Is Nothing Then
                Throw New ArgumentNullException(NameOf(template))
            End If

            _connection.EnsureOpen()

            Using transaction = _connection.BeginTransaction()
                Try
                    ' Insert template record
                    Dim templateId = InsertTemplateRecord(template, transaction)
                    template.TemplateID = templateId

                    ' Insert folders recursively
                    For Each folder In template.Folders
                        InsertFolderHierarchy(folder, templateId, Nothing, transaction)
                    Next

                    transaction.Commit()
                    Return template

                Catch ex As Exception
                    transaction.Rollback()
                    Throw New InvalidOperationException($"Failed to create template: {ex.Message}", ex)
                End Try
            End Using
        End Function

        ''' <summary>
        ''' Gets a template by ID with complete hierarchy
        ''' </summary>
        Public Function GetTemplate(templateId As Integer) As TemplateDefinition Implements Services.Interfaces.ITemplateStorageService.GetTemplate
            _connection.EnsureOpen()

            ' Load template record
            Dim template = LoadTemplateRecord(templateId)
            If template Is Nothing Then Return Nothing

            ' Load folder hierarchy
            template.Folders = LoadFolderHierarchy(templateId, Nothing)

            Return template
        End Function

        ''' <summary>
        ''' Gets all templates (without folder details)
        ''' </summary>
        Public Function GetAllTemplates() As List(Of TemplateDefinition) Implements Services.Interfaces.ITemplateStorageService.GetAllTemplates
            _connection.EnsureOpen()

            Dim templates As New List(Of TemplateDefinition)
            Dim sql = "SELECT TemplateID, Name, Description, Category, Tags, Version, " &
                     "CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UsageCount, " &
                     "LastUsedDate, IsActive, Dependencies, Notes FROM Template"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        templates.Add(MapReaderToTemplate(reader))
                    End While
                End Using
            End Using

            Return templates
        End Function

        ''' <summary>
        ''' Updates an existing template
        ''' </summary>
        Public Function UpdateTemplate(template As TemplateDefinition) As Boolean Implements Services.Interfaces.ITemplateStorageService.UpdateTemplate
            If template Is Nothing Then
                Throw New ArgumentNullException(NameOf(template))
            End If

            _connection.EnsureOpen()

            Using transaction = _connection.BeginTransaction()
                Try
                    ' Update template record
                    UpdateTemplateRecord(template, transaction)

                    ' Delete existing folders/files
                    DeleteFoldersByTemplateId(template.TemplateID, transaction)

                    ' Reinsert folders recursively
                    For Each folder In template.Folders
                        InsertFolderHierarchy(folder, template.TemplateID, Nothing, transaction)
                    Next

                    transaction.Commit()
                    Return True

                Catch ex As Exception
                    transaction.Rollback()
                    Throw New InvalidOperationException($"Failed to update template: {ex.Message}", ex)
                End Try
            End Using
        End Function

        ''' <summary>
        ''' Deletes a template by ID (cascade deletes folders/files)
        ''' </summary>
        Public Function DeleteTemplate(templateId As Integer) As Boolean Implements Services.Interfaces.ITemplateStorageService.DeleteTemplate
            _connection.EnsureOpen()

            Try
                ' Cascade delete handled by database foreign keys
                Dim sql = "DELETE FROM Template WHERE TemplateID = ?"
                Using cmd As New OleDbCommand(sql, _connection.Connection)
                    cmd.Parameters.AddWithValue("@TemplateID", templateId)
                    Dim rowsAffected = cmd.ExecuteNonQuery()
                    Return rowsAffected > 0
                End Using

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to delete template: {ex.Message}", ex)
            End Try
        End Function

#End Region

#Region "ITemplateStorageService Implementation - Search"

        ''' <summary>
        ''' Gets a template by exact name
        ''' </summary>
        Public Function GetTemplateByName(name As String) As TemplateDefinition Implements Services.Interfaces.ITemplateStorageService.GetTemplateByName
            _connection.EnsureOpen()

            Dim sql = "SELECT TemplateID FROM Template WHERE Name = ?"
            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@Name", name)
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    Return GetTemplate(CInt(result))
                End If
            End Using

            Return Nothing
        End Function

        ''' <summary>
        ''' Searches templates by keyword
        ''' </summary>
        Public Function SearchTemplates(keyword As String) As List(Of TemplateDefinition) Implements Services.Interfaces.ITemplateStorageService.SearchTemplates
            _connection.EnsureOpen()

            Dim templates As New List(Of TemplateDefinition)
            Dim sql = "SELECT TemplateID, Name, Description, Category, Tags, Version, " &
                     "CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UsageCount, " &
                     "LastUsedDate, IsActive, Dependencies, Notes FROM Template " &
                     "WHERE Name LIKE ? OR Description LIKE ? OR Tags LIKE ?"

            Dim searchPattern = $"%{keyword}%"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@Name", searchPattern)
                cmd.Parameters.AddWithValue("@Description", searchPattern)
                cmd.Parameters.AddWithValue("@Tags", searchPattern)

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        templates.Add(MapReaderToTemplate(reader))
                    End While
                End Using
            End Using

            Return templates
        End Function

        ''' <summary>
        ''' Gets templates by category
        ''' </summary>
        Public Function GetTemplatesByCategory(category As String) As List(Of TemplateDefinition) Implements Services.Interfaces.ITemplateStorageService.GetTemplatesByCategory
            _connection.EnsureOpen()

            Dim templates As New List(Of TemplateDefinition)
            Dim sql = "SELECT TemplateID, Name, Description, Category, Tags, Version, " &
                     "CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UsageCount, " &
                     "LastUsedDate, IsActive, Dependencies, Notes FROM Template WHERE Category = ?"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@Category", category)

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        templates.Add(MapReaderToTemplate(reader))
                    End While
                End Using
            End Using

            Return templates
        End Function

        ''' <summary>
        ''' Gets templates by tag
        ''' </summary>
        Public Function GetTemplatesByTag(tag As String) As List(Of TemplateDefinition) Implements Services.Interfaces.ITemplateStorageService.GetTemplatesByTag
            _connection.EnsureOpen()

            Dim templates As New List(Of TemplateDefinition)
            Dim sql = "SELECT TemplateID, Name, Description, Category, Tags, Version, " &
                     "CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UsageCount, " &
                     "LastUsedDate, IsActive, Dependencies, Notes FROM Template WHERE Tags LIKE ?"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@Tags", $"%{tag}%")

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        templates.Add(MapReaderToTemplate(reader))
                    End While
                End Using
            End Using

            Return templates
        End Function

        ''' <summary>
        ''' Gets only active templates
        ''' </summary>
        Public Function GetActiveTemplates() As List(Of TemplateDefinition) Implements Services.Interfaces.ITemplateStorageService.GetActiveTemplates
            _connection.EnsureOpen()

            Dim templates As New List(Of TemplateDefinition)
            Dim sql = "SELECT TemplateID, Name, Description, Category, Tags, Version, " &
                     "CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UsageCount, " &
                     "LastUsedDate, IsActive, Dependencies, Notes FROM Template WHERE IsActive = TRUE"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        templates.Add(MapReaderToTemplate(reader))
                    End While
                End Using
            End Using

            Return templates
        End Function

#End Region

#Region "ITemplateStorageService Implementation - JSON"

        ''' <summary>
        ''' Exports template to JSON file
        ''' </summary>
        Public Function ExportTemplateToJson(templateId As Integer, filePath As String) As Boolean Implements Services.Interfaces.ITemplateStorageService.ExportTemplateToJson
            Try
                Dim template = GetTemplate(templateId)
                If template Is Nothing Then Return False

                _serializer.SerializeToFile(template, filePath)
                Return True

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to export template: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Imports template from JSON file
        ''' </summary>
        Public Function ImportTemplateFromJson(filePath As String) As TemplateDefinition Implements Services.Interfaces.ITemplateStorageService.ImportTemplateFromJson
            Try
                Dim template = _serializer.DeserializeFromFile(filePath)
                Return CreateTemplate(template)

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to import template: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Imports legacy TemplateBuilderControl JSON
        ''' </summary>
        Public Function ImportLegacyTemplate(filePath As String) As TemplateDefinition Implements Services.Interfaces.ITemplateStorageService.ImportLegacyTemplate
            Try
                Dim json = File.ReadAllText(filePath)
                Dim template = _serializer.ImportLegacyTemplate(json)
                Return CreateTemplate(template)

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to import legacy template: {ex.Message}", ex)
            End Try
        End Function

#End Region

#Region "ITemplateStorageService Implementation - Utility"

        ''' <summary>
        ''' Checks if template exists by name
        ''' </summary>
        Public Function TemplateExists(name As String) As Boolean Implements Services.Interfaces.ITemplateStorageService.TemplateExists
            _connection.EnsureOpen()

            Dim sql = "SELECT COUNT(*) FROM Template WHERE Name = ?"
            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@Name", name)
                Return CInt(cmd.ExecuteScalar()) > 0
            End Using
        End Function

        ''' <summary>
        ''' Gets template count
        ''' </summary>
        Public Function GetTemplateCount() As Integer Implements Services.Interfaces.ITemplateStorageService.GetTemplateCount
            _connection.EnsureOpen()

            Dim sql = "SELECT COUNT(*) FROM Template"
            Return CInt(_connection.ExecuteScalar(sql))
        End Function

        ''' <summary>
        ''' Increments usage count
        ''' </summary>
        Public Function IncrementUsage(templateId As Integer) As Boolean Implements Services.Interfaces.ITemplateStorageService.IncrementUsage
            _connection.EnsureOpen()

            Try
                Dim sql = "UPDATE Template SET UsageCount = UsageCount + 1, LastUsedDate = NOW() WHERE TemplateID = ?"
                Using cmd As New OleDbCommand(sql, _connection.Connection)
                    cmd.Parameters.AddWithValue("@TemplateID", templateId)
                    Return cmd.ExecuteNonQuery() > 0
                End Using

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to increment usage: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Gets all unique categories
        ''' </summary>
        Public Function GetAllCategories() As List(Of String) Implements Services.Interfaces.ITemplateStorageService.GetAllCategories
            _connection.EnsureOpen()

            Dim categories As New List(Of String)
            Dim sql = "SELECT DISTINCT Category FROM Template WHERE Category IS NOT NULL ORDER BY Category"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        categories.Add(reader("Category").ToString())
                    End While
                End Using
            End Using

            Return categories
        End Function

        ''' <summary>
        ''' Gets all unique tags
        ''' </summary>
        Public Function GetAllTags() As List(Of String) Implements Services.Interfaces.ITemplateStorageService.GetAllTags
            _connection.EnsureOpen()

            Dim allTags As New HashSet(Of String)
            Dim sql = "SELECT Tags FROM Template WHERE Tags IS NOT NULL"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim tags = reader("Tags").ToString()
                        If Not String.IsNullOrEmpty(tags) Then
                            For Each tag In tags.Split(","c)
                                allTags.Add(tag.Trim())
                            Next
                        End If
                    End While
                End Using
            End Using

            Return allTags.ToList()
        End Function

#End Region

#Region "ITemplateStorageService Implementation - Database Management"

        ''' <summary>
        ''' Gets connection string
        ''' </summary>
        Public ReadOnly Property ConnectionString As String Implements Services.Interfaces.ITemplateStorageService.ConnectionString
            Get
                Return _connection.ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Gets database path
        ''' </summary>
        Public ReadOnly Property DatabasePath As String Implements Services.Interfaces.ITemplateStorageService.DatabasePath
            Get
                Return _databasePath
            End Get
        End Property

        ''' <summary>
        ''' Tests database connection
        ''' </summary>
        Public Function TestConnection() As Boolean Implements Services.Interfaces.ITemplateStorageService.TestConnection
            Return _connection.TestConnection()
        End Function

        ''' <summary>
        ''' Validates database schema
        ''' </summary>
        Public Function ValidateDatabase() As Boolean Implements Services.Interfaces.ITemplateStorageService.ValidateDatabase
            Dim validator As New DatabaseValidator(_connection)
            Return validator.ValidateSchema() AndAlso validator.ValidateReferentialIntegrity()
        End Function

#End Region

#Region "Private Helper Methods - Template Operations"

        ''' <summary>
        ''' Inserts template record and returns ID
        ''' </summary>
        Private Function InsertTemplateRecord(template As TemplateDefinition, transaction As OleDbTransaction) As Integer
            Dim sql = "INSERT INTO Template (Name, Description, Category, Tags, Version, CreatedBy, CreatedDate, " &
                     "ModifiedBy, ModifiedDate, UsageCount, LastUsedDate, IsActive, Dependencies, Notes) " &
                     "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"

            Using cmd As New OleDbCommand(sql, _connection.Connection, transaction)
                cmd.Parameters.AddWithValue("@Name", template.Name)
                cmd.Parameters.AddWithValue("@Description", If(template.Description, DBNull.Value))
                cmd.Parameters.AddWithValue("@Category", If(template.Category, DBNull.Value))
                cmd.Parameters.AddWithValue("@Tags", If(template.Tags, DBNull.Value))
                cmd.Parameters.AddWithValue("@Version", If(template.Version, "1.0.0"))
                cmd.Parameters.AddWithValue("@CreatedBy", If(template.CreatedBy, DBNull.Value))
                cmd.Parameters.AddWithValue("@CreatedDate", template.CreatedDate)
                cmd.Parameters.AddWithValue("@ModifiedBy", If(template.ModifiedBy, DBNull.Value))
                cmd.Parameters.AddWithValue("@ModifiedDate", template.ModifiedDate)
                cmd.Parameters.AddWithValue("@UsageCount", template.UsageCount)
                cmd.Parameters.AddWithValue("@LastUsedDate", If(template.LastUsedDate, DBNull.Value))
                cmd.Parameters.AddWithValue("@IsActive", template.IsActive)
                cmd.Parameters.AddWithValue("@Dependencies", If(template.Dependencies, DBNull.Value))
                cmd.Parameters.AddWithValue("@Notes", If(template.Notes, DBNull.Value))

                cmd.ExecuteNonQuery()
            End Using

            ' Get inserted ID
            Using cmd As New OleDbCommand("SELECT @@IDENTITY", _connection.Connection, transaction)
                Return CInt(cmd.ExecuteScalar())
            End Using
        End Function

        ''' <summary>
        ''' Loads template record from database
        ''' </summary>
        Private Function LoadTemplateRecord(templateId As Integer) As TemplateDefinition
            Dim sql = "SELECT TemplateID, Name, Description, Category, Tags, Version, " &
                     "CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, UsageCount, " &
                     "LastUsedDate, IsActive, Dependencies, Notes FROM Template WHERE TemplateID = ?"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)

                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapReaderToTemplate(reader)
                    End If
                End Using
            End Using

            Return Nothing
        End Function

        ''' <summary>
        ''' Maps data reader to template
        ''' </summary>
        Private Function MapReaderToTemplate(reader As OleDbDataReader) As TemplateDefinition
            Return New TemplateDefinition() With {
                .TemplateID = CInt(reader("TemplateID")),
                .Name = reader("Name").ToString(),
                .Description = If(IsDBNull(reader("Description")), Nothing, reader("Description").ToString()),
                .Category = If(IsDBNull(reader("Category")), Nothing, reader("Category").ToString()),
                .Tags = If(IsDBNull(reader("Tags")), Nothing, reader("Tags").ToString()),
                .Version = If(IsDBNull(reader("Version")), "1.0.0", reader("Version").ToString()),
                .CreatedBy = If(IsDBNull(reader("CreatedBy")), Nothing, reader("CreatedBy").ToString()),
                .CreatedDate = CDate(reader("CreatedDate")),
                .ModifiedBy = If(IsDBNull(reader("ModifiedBy")), Nothing, reader("ModifiedBy").ToString()),
                .ModifiedDate = CDate(reader("ModifiedDate")),
                .UsageCount = CInt(reader("UsageCount")),
                .LastUsedDate = If(IsDBNull(reader("LastUsedDate")), Nothing, CType(reader("LastUsedDate"), DateTime?)),
                .IsActive = CBool(reader("IsActive")),
                .Dependencies = If(IsDBNull(reader("Dependencies")), Nothing, reader("Dependencies").ToString()),
                .Notes = If(IsDBNull(reader("Notes")), Nothing, reader("Notes").ToString())
            }
        End Function

        ''' <summary>
        ''' Updates template record
        ''' </summary>
        Private Sub UpdateTemplateRecord(template As TemplateDefinition, transaction As OleDbTransaction)
            Dim sql = "UPDATE Template SET Name = ?, Description = ?, Category = ?, Tags = ?, Version = ?, " &
                     "ModifiedBy = ?, ModifiedDate = ?, UsageCount = ?, LastUsedDate = ?, IsActive = ?, " &
                     "Dependencies = ?, Notes = ? WHERE TemplateID = ?"

            Using cmd As New OleDbCommand(sql, _connection.Connection, transaction)
                cmd.Parameters.AddWithValue("@Name", template.Name)
                cmd.Parameters.AddWithValue("@Description", If(template.Description, DBNull.Value))
                cmd.Parameters.AddWithValue("@Category", If(template.Category, DBNull.Value))
                cmd.Parameters.AddWithValue("@Tags", If(template.Tags, DBNull.Value))
                cmd.Parameters.AddWithValue("@Version", template.Version)
                cmd.Parameters.AddWithValue("@ModifiedBy", If(template.ModifiedBy, DBNull.Value))
                cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now)
                cmd.Parameters.AddWithValue("@UsageCount", template.UsageCount)
                cmd.Parameters.AddWithValue("@LastUsedDate", If(template.LastUsedDate, DBNull.Value))
                cmd.Parameters.AddWithValue("@IsActive", template.IsActive)
                cmd.Parameters.AddWithValue("@Dependencies", If(template.Dependencies, DBNull.Value))
                cmd.Parameters.AddWithValue("@Notes", If(template.Notes, DBNull.Value))
                cmd.Parameters.AddWithValue("@TemplateID", template.TemplateID)

                cmd.ExecuteNonQuery()
            End Using
        End Sub

#End Region

#Region "Private Helper Methods - Folder Operations"

        ''' <summary>
        ''' Inserts folder hierarchy recursively
        ''' </summary>
        Private Sub InsertFolderHierarchy(folder As TemplateFolderDefinition, templateId As Integer,
                                          parentFolderId As Integer?, transaction As OleDbTransaction)
            ' Insert folder record
            Dim folderId = InsertFolderRecord(folder, templateId, parentFolderId, transaction)
            folder.FolderID = folderId

            ' Insert files
            For Each file In folder.Files
                InsertFileRecord(file, folderId, templateId, transaction)
            Next

            ' Insert subfolders recursively
            For Each subfolder In folder.SubFolders
                InsertFolderHierarchy(subfolder, templateId, folderId, transaction)
            Next
        End Sub

        ''' <summary>
        ''' Inserts folder record and returns ID
        ''' </summary>
        Private Function InsertFolderRecord(folder As TemplateFolderDefinition, templateId As Integer,
                                           parentFolderId As Integer?, transaction As OleDbTransaction) As Integer
            Dim sql = "INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, CreatedDate, DisplayOrder) " &
                     "VALUES (?, ?, ?, ?, ?, ?, ?)"

            Using cmd As New OleDbCommand(sql, _connection.Connection, transaction)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)
                cmd.Parameters.AddWithValue("@ParentFolderID", If(parentFolderId, DBNull.Value))
                cmd.Parameters.AddWithValue("@Name", folder.Name)
                cmd.Parameters.AddWithValue("@IsSelected", folder.IsSelected)
                cmd.Parameters.AddWithValue("@Description", If(folder.Description, DBNull.Value))
                cmd.Parameters.AddWithValue("@CreatedDate", folder.CreatedDate)
                cmd.Parameters.AddWithValue("@DisplayOrder", folder.DisplayOrder)

                cmd.ExecuteNonQuery()
            End Using

            ' Get inserted ID
            Using cmd As New OleDbCommand("SELECT @@IDENTITY", _connection.Connection, transaction)
                Return CInt(cmd.ExecuteScalar())
            End Using
        End Function

        ''' <summary>
        ''' Loads folder hierarchy recursively
        ''' </summary>
        Private Function LoadFolderHierarchy(templateId As Integer, parentFolderId As Integer?) As List(Of TemplateFolderDefinition)
            Dim folders As New List(Of TemplateFolderDefinition)

            Dim sql = "SELECT FolderID, TemplateID, ParentFolderID, Name, IsSelected, Description, CreatedDate, DisplayOrder " &
                     "FROM TemplateFolder WHERE TemplateID = ? AND "

            If parentFolderId.HasValue Then
                sql &= "ParentFolderID = ? ORDER BY DisplayOrder"
            Else
                sql &= "ParentFolderID IS NULL ORDER BY DisplayOrder"
            End If

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)
                If parentFolderId.HasValue Then
                    cmd.Parameters.AddWithValue("@ParentFolderID", parentFolderId.Value)
                End If

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim folder = MapReaderToFolder(reader)

                        ' Load files for this folder
                        folder.Files = LoadFilesByFolderId(folder.FolderID)

                        ' Load subfolders recursively
                        folder.SubFolders = LoadFolderHierarchy(templateId, folder.FolderID)

                        folders.Add(folder)
                    End While
                End Using
            End Using

            Return folders
        End Function

        ''' <summary>
        ''' Maps data reader to folder
        ''' </summary>
        Private Function MapReaderToFolder(reader As OleDbDataReader) As TemplateFolderDefinition
            Return New TemplateFolderDefinition() With {
                .FolderID = CInt(reader("FolderID")),
                .TemplateID = CInt(reader("TemplateID")),
                .ParentFolderID = If(IsDBNull(reader("ParentFolderID")), Nothing, CType(reader("ParentFolderID"), Integer?)),
                .Name = reader("Name").ToString(),
                .IsSelected = CBool(reader("IsSelected")),
                .Description = If(IsDBNull(reader("Description")), Nothing, reader("Description").ToString()),
                .CreatedDate = CDate(reader("CreatedDate")),
                .DisplayOrder = CInt(reader("DisplayOrder"))
            }
        End Function

        ''' <summary>
        ''' Deletes all folders for a template
        ''' </summary>
        Private Sub DeleteFoldersByTemplateId(templateId As Integer, transaction As OleDbTransaction)
            ' Cascade delete handled by database, just delete root folders
            Dim sql = "DELETE FROM TemplateFolder WHERE TemplateID = ?"
            Using cmd As New OleDbCommand(sql, _connection.Connection, transaction)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

#End Region

#Region "Private Helper Methods - File Operations"

        ''' <summary>
        ''' Inserts file record
        ''' </summary>
        Private Sub InsertFileRecord(file As TemplateFileDefinition, folderId As Integer,
                                     templateId As Integer, transaction As OleDbTransaction)
            Dim sql = "INSERT INTO TemplateFile (FolderID, TemplateID, FileName, FileType, Description, " &
                     "ContentTemplate, RequiresMetadataHeader, MetadataHeaderTemplate, Encoding, LineEnding, " &
                     "IsAutoGenerated, DisplayOrder, CreatedDate, IsActive) " &
                     "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"

            Using cmd As New OleDbCommand(sql, _connection.Connection, transaction)
                cmd.Parameters.AddWithValue("@FolderID", folderId)
                cmd.Parameters.AddWithValue("@TemplateID", templateId)
                cmd.Parameters.AddWithValue("@FileName", file.FileName)
                cmd.Parameters.AddWithValue("@FileType", If(file.FileType, DBNull.Value))
                cmd.Parameters.AddWithValue("@Description", If(file.Description, DBNull.Value))
                cmd.Parameters.AddWithValue("@ContentTemplate", If(file.ContentTemplate, DBNull.Value))
                cmd.Parameters.AddWithValue("@RequiresMetadataHeader", file.RequiresMetadataHeader)
                cmd.Parameters.AddWithValue("@MetadataHeaderTemplate", If(file.MetadataHeaderTemplate, DBNull.Value))
                cmd.Parameters.AddWithValue("@Encoding", If(file.Encoding, "UTF-8"))
                cmd.Parameters.AddWithValue("@LineEnding", If(file.LineEnding, "CRLF"))
                cmd.Parameters.AddWithValue("@IsAutoGenerated", file.IsAutoGenerated)
                cmd.Parameters.AddWithValue("@DisplayOrder", file.DisplayOrder)
                cmd.Parameters.AddWithValue("@CreatedDate", file.CreatedDate)
                cmd.Parameters.AddWithValue("@IsActive", file.IsActive)

                cmd.ExecuteNonQuery()
            End Using
        End Sub

        ''' <summary>
        ''' Loads files by folder ID
        ''' </summary>
        Private Function LoadFilesByFolderId(folderId As Integer) As List(Of TemplateFileDefinition)
            Dim files As New List(Of TemplateFileDefinition)

            Dim sql = "SELECT FileID, FolderID, TemplateID, FileName, FileType, Description, " &
                     "ContentTemplate, RequiresMetadataHeader, MetadataHeaderTemplate, Encoding, LineEnding, " &
                     "IsAutoGenerated, DisplayOrder, CreatedDate, IsActive " &
                     "FROM TemplateFile WHERE FolderID = ? ORDER BY DisplayOrder"

            Using cmd As New OleDbCommand(sql, _connection.Connection)
                cmd.Parameters.AddWithValue("@FolderID", folderId)

                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        files.Add(MapReaderToFile(reader))
                    End While
                End Using
            End Using

            Return files
        End Function

        ''' <summary>
        ''' Maps data reader to file
        ''' </summary>
        Private Function MapReaderToFile(reader As OleDbDataReader) As TemplateFileDefinition
            Return New TemplateFileDefinition() With {
                .FileID = CInt(reader("FileID")),
                .FolderID = CInt(reader("FolderID")),
                .TemplateID = CInt(reader("TemplateID")),
                .FileName = reader("FileName").ToString(),
                .FileType = If(IsDBNull(reader("FileType")), Nothing, reader("FileType").ToString()),
                .Description = If(IsDBNull(reader("Description")), Nothing, reader("Description").ToString()),
                .ContentTemplate = If(IsDBNull(reader("ContentTemplate")), Nothing, reader("ContentTemplate").ToString()),
                .RequiresMetadataHeader = CBool(reader("RequiresMetadataHeader")),
                .MetadataHeaderTemplate = If(IsDBNull(reader("MetadataHeaderTemplate")), Nothing, reader("MetadataHeaderTemplate").ToString()),
                .Encoding = If(IsDBNull(reader("Encoding")), "UTF-8", reader("Encoding").ToString()),
                .LineEnding = If(IsDBNull(reader("LineEnding")), "CRLF", reader("LineEnding").ToString()),
                .IsAutoGenerated = CBool(reader("IsAutoGenerated")),
                .DisplayOrder = CInt(reader("DisplayOrder")),
                .CreatedDate = CDate(reader("CreatedDate")),
                .IsActive = CBool(reader("IsActive"))
            }
        End Function

#End Region

#Region "IDisposable Implementation"

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    If _connection IsNot Nothing Then
                        _connection.Dispose()
                    End If
                End If
                _disposed = True
            End If
        End Sub

#End Region

    End Class

End Namespace
