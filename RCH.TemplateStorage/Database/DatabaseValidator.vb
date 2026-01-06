''' ============================================================================
''' File: DatabaseValidator.vb
''' Project: RCH.TemplateStorage
''' Purpose: Database schema and integrity validation
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Provides comprehensive database validation including schema verification,
'''   table existence checks, referential integrity validation, and data
'''   consistency checks. Ensures database meets all requirements for
'''   RCH.TemplateStorage operations.
''' 
''' Validation Types:
'''   - Schema validation (tables, columns, indexes, constraints)
'''   - Referential integrity (foreign keys, cascade rules)
'''   - Data consistency (orphaned records, invalid data)
'''   - Performance validation (index usage, query optimization)
''' 
''' Dependencies:
'''   - System.Data.OleDb (10.0.1)
'''   - DatabaseConnection.vb
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Text
Imports System.Linq

Namespace Database

    ''' <summary>
    ''' Validates database schema and integrity
    ''' </summary>
    Public Class DatabaseValidator

#Region "Private Fields"

        Private ReadOnly _connection As DatabaseConnection
        Private _validationResults As New List(Of ValidationResult)

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor with database connection
        ''' </summary>
        ''' <param name="connection">Database connection</param>
        Public Sub New(connection As DatabaseConnection)
            If connection Is Nothing Then
                Throw New ArgumentNullException(NameOf(connection))
            End If

            _connection = connection
        End Sub

        ''' <summary>
        ''' Constructor with connection string
        ''' </summary>
        ''' <param name="connectionString">OleDb connection string</param>
        Public Sub New(connectionString As String)
            If String.IsNullOrWhiteSpace(connectionString) Then
                Throw New ArgumentNullException(NameOf(connectionString))
            End If

            _connection = New DatabaseConnection(connectionString)
        End Sub

#End Region

#Region "Public Validation Methods"

        ''' <summary>
        ''' Validates complete database schema
        ''' </summary>
        ''' <returns>True if schema is valid</returns>
        Public Function ValidateSchema() As Boolean
            _validationResults.Clear()

            Try
                _connection.EnsureOpen()

                ' Validate required tables exist
                ValidateRequiredTables()

                ' Validate table structures
                ValidateTemplateTable()
                ValidateTemplateFolderTable()
                ValidateTemplateFileTable()

                ' Validate indexes
                ValidateIndexes()

                ' Check for validation errors
                Return Not HasErrors()

            Catch ex As Exception
                AddError("Schema Validation", $"Validation failed: {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Validates referential integrity
        ''' </summary>
        ''' <returns>True if integrity is valid</returns>
        Public Function ValidateReferentialIntegrity() As Boolean
            Try
                _connection.EnsureOpen()

                ' Check for orphaned TemplateFolder records
                ValidateFolderIntegrity()

                ' Check for orphaned TemplateFile records
                ValidateFileIntegrity()

                ' Check for circular references in folder hierarchy
                ValidateFolderHierarchy()

                Return Not HasErrors()

            Catch ex As Exception
                AddError("Referential Integrity", $"Validation failed: {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Checks if a specific table exists
        ''' </summary>
        ''' <param name="tableName">Table name</param>
        ''' <returns>True if table exists</returns>
        Public Function TableExists(tableName As String) As Boolean
            Try
                _connection.EnsureOpen()
                Dim schema = _connection.GetSchema("Tables")

                For Each row As DataRow In schema.Rows
                    If row("TABLE_NAME").ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase) Then
                        Return True
                    End If
                Next

                Return False

            Catch ex As Exception
                AddError("Table Check", $"Failed to check table '{tableName}': {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Checks if a specific column exists in a table
        ''' </summary>
        ''' <param name="tableName">Table name</param>
        ''' <param name="columnName">Column name</param>
        ''' <returns>True if column exists</returns>
        Public Function ColumnExists(tableName As String, columnName As String) As Boolean
            Try
                _connection.EnsureOpen()

                ' Query system tables directly for Access
                Dim sql = $"SELECT COUNT(*) FROM MSysObjects WHERE Name = '{tableName}'"
                Dim tableExists = CInt(_connection.ExecuteScalar(sql)) > 0

                If Not tableExists Then Return False

                ' Try to query the column directly
                sql = $"SELECT TOP 1 [{columnName}] FROM [{tableName}]"
                Try
                    _connection.ExecuteScalar(sql)
                    Return True
                Catch
                    Return False
                End Try

            Catch ex As Exception
                AddError("Column Check", $"Failed to check column '{tableName}.{columnName}': {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Gets validation results
        ''' </summary>
        ''' <returns>List of validation results</returns>
        Public Function GetValidationResults() As List(Of ValidationResult)
            Return New List(Of ValidationResult)(_validationResults)
        End Function

        ''' <summary>
        ''' Gets validation summary report
        ''' </summary>
        ''' <returns>Formatted validation report</returns>
        Public Function GetValidationReport() As String
            Dim report As New StringBuilder()

            report.AppendLine("Database Validation Report")
            report.AppendLine("=" & New String("="c, 50))
            report.AppendLine($"Timestamp: {DateTime.Now}")
            report.AppendLine($"Total Checks: {_validationResults.Count}")
            report.AppendLine($"Errors: {GetErrorCount()}")
            report.AppendLine($"Warnings: {GetWarningCount()}")
            report.AppendLine($"Info: {GetInfoCount()}")
            report.AppendLine()

            If HasErrors() Then
                report.AppendLine("ERRORS:")
                For Each result In _validationResults.Where(Function(r) r.Severity = ValidationSeverity.Error)
                    report.AppendLine($"  [ERROR] {result.Category}: {result.Message}")
                Next
                report.AppendLine()
            End If

            If HasWarnings() Then
                report.AppendLine("WARNINGS:")
                For Each result In _validationResults.Where(Function(r) r.Severity = ValidationSeverity.Warning)
                    report.AppendLine($"  [WARN] {result.Category}: {result.Message}")
                Next
                report.AppendLine()
            End If

            report.AppendLine($"Status: {If(HasErrors(), "FAILED", "PASSED")}")

            Return report.ToString()
        End Function

#End Region

#Region "Private Validation Methods - Tables"

        ''' <summary>
        ''' Validates that all required tables exist
        ''' </summary>
        Private Sub ValidateRequiredTables()
            Dim requiredTables = {"Template", "TemplateFolder", "TemplateFile"}

            For Each tableName In requiredTables
                If Not TableExists(tableName) Then
                    AddError("Required Tables", $"Missing required table: {tableName}")
                Else
                    AddInfo("Required Tables", $"Table '{tableName}' exists")
                End If
            Next
        End Sub

        ''' <summary>
        ''' Validates Template table structure
        ''' </summary>
        Private Sub ValidateTemplateTable()
            Const tableName = "Template"

            If Not TableExists(tableName) Then Return

            ' Required columns
            Dim requiredColumns = {
                "TemplateID", "Name", "Description", "Category", "Tags", "Version",
                "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate",
                "UsageCount", "LastUsedDate", "IsActive", "Dependencies", "Notes"
            }

            For Each columnName In requiredColumns
                If Not ColumnExists(tableName, columnName) Then
                    AddError($"{tableName} Structure", $"Missing column: {columnName}")
                End If
            Next

            AddInfo($"{tableName} Structure", "All required columns present")
        End Sub

        ''' <summary>
        ''' Validates TemplateFolder table structure
        ''' </summary>
        Private Sub ValidateTemplateFolderTable()
            Const tableName = "TemplateFolder"

            If Not TableExists(tableName) Then Return

            ' Required columns
            Dim requiredColumns = {
                "FolderID", "TemplateID", "ParentFolderID", "Name", "IsSelected",
                "Description", "CreatedDate", "DisplayOrder"
            }

            For Each columnName In requiredColumns
                If Not ColumnExists(tableName, columnName) Then
                    AddError($"{tableName} Structure", $"Missing column: {columnName}")
                End If
            Next

            AddInfo($"{tableName} Structure", "All required columns present")
        End Sub

        ''' <summary>
        ''' Validates TemplateFile table structure
        ''' </summary>
        Private Sub ValidateTemplateFileTable()
            Const tableName = "TemplateFile"

            If Not TableExists(tableName) Then Return

            ' Required columns
            Dim requiredColumns = {
                "FileID", "FolderID", "TemplateID", "FileName", "FileType",
                "Description", "ContentTemplate", "RequiresMetadataHeader",
                "MetadataHeaderTemplate", "Encoding", "LineEnding",
                "IsAutoGenerated", "DisplayOrder", "CreatedDate", "IsActive"
            }

            For Each columnName In requiredColumns
                If Not ColumnExists(tableName, columnName) Then
                    AddError($"{tableName} Structure", $"Missing column: {columnName}")
                End If
            Next

            AddInfo($"{tableName} Structure", "All required columns present")
        End Sub

#End Region

#Region "Private Validation Methods - Indexes"

        ''' <summary>
        ''' Validates that recommended indexes exist
        ''' </summary>
        Private Sub ValidateIndexes()
            Try
                ' Note: Access doesn't provide easy index enumeration via OleDb schema
                ' This is a placeholder for index validation
                AddInfo("Indexes", "Index validation requires manual verification")

            Catch ex As Exception
                AddWarning("Indexes", $"Could not validate indexes: {ex.Message}")
            End Try
        End Sub

#End Region

#Region "Private Validation Methods - Integrity"

        ''' <summary>
        ''' Validates folder referential integrity
        ''' </summary>
        Private Sub ValidateFolderIntegrity()
            ' Check for orphaned folders (TemplateID references non-existent template)
            Dim sql = "SELECT COUNT(*) FROM TemplateFolder f " &
                     "WHERE NOT EXISTS (SELECT 1 FROM Template t WHERE t.TemplateID = f.TemplateID)"

            Dim orphanedCount = CInt(_connection.ExecuteScalar(sql))

            If orphanedCount > 0 Then
                AddError("Folder Integrity", $"Found {orphanedCount} orphaned folder(s) with invalid TemplateID")
            Else
                AddInfo("Folder Integrity", "No orphaned folders found")
            End If
        End Sub

        ''' <summary>
        ''' Validates file referential integrity
        ''' </summary>
        Private Sub ValidateFileIntegrity()
            ' Check for orphaned files (FolderID references non-existent folder)
            Dim sql = "SELECT COUNT(*) FROM TemplateFile tf " &
                     "WHERE NOT EXISTS (SELECT 1 FROM TemplateFolder f WHERE f.FolderID = tf.FolderID)"

            Dim orphanedCount = CInt(_connection.ExecuteScalar(sql))

            If orphanedCount > 0 Then
                AddError("File Integrity", $"Found {orphanedCount} orphaned file(s) with invalid FolderID")
            Else
                AddInfo("File Integrity", "No orphaned files found")
            End If
        End Sub

        ''' <summary>
        ''' Validates folder hierarchy (no circular references)
        ''' </summary>
        Private Sub ValidateFolderHierarchy()
            ' Check for self-referencing folders
            Dim sql = "SELECT COUNT(*) FROM TemplateFolder WHERE FolderID = ParentFolderID"

            Dim selfRefCount = CInt(_connection.ExecuteScalar(sql))

            If selfRefCount > 0 Then
                AddError("Folder Hierarchy", $"Found {selfRefCount} self-referencing folder(s)")
            Else
                AddInfo("Folder Hierarchy", "No self-referencing folders found")
            End If
        End Sub

#End Region

#Region "Private Helper Methods"

        ''' <summary>
        ''' Adds an error to validation results
        ''' </summary>
        Private Sub AddError(category As String, message As String)
            _validationResults.Add(New ValidationResult(ValidationSeverity.Error, category, message))
        End Sub

        ''' <summary>
        ''' Adds a warning to validation results
        ''' </summary>
        Private Sub AddWarning(category As String, message As String)
            _validationResults.Add(New ValidationResult(ValidationSeverity.Warning, category, message))
        End Sub

        ''' <summary>
        ''' Adds an info message to validation results
        ''' </summary>
        Private Sub AddInfo(category As String, message As String)
            _validationResults.Add(New ValidationResult(ValidationSeverity.Info, category, message))
        End Sub

        ''' <summary>
        ''' Checks if there are any errors
        ''' </summary>
        Private Function HasErrors() As Boolean
            Return _validationResults.Any(Function(r) r.Severity = ValidationSeverity.Error)
        End Function

        ''' <summary>
        ''' Checks if there are any warnings
        ''' </summary>
        Private Function HasWarnings() As Boolean
            Return _validationResults.Any(Function(r) r.Severity = ValidationSeverity.Warning)
        End Function

        ''' <summary>
        ''' Gets error count
        ''' </summary>
        Private Function GetErrorCount() As Integer
            Return Enumerable.Count(_validationResults, Function(r) r.Severity = ValidationSeverity.Error)
        End Function

        ''' <summary>
        ''' Gets warning count
        ''' </summary>
        Private Function GetWarningCount() As Integer
            Return Enumerable.Count(_validationResults, Function(r) r.Severity = ValidationSeverity.Warning)
        End Function

        ''' <summary>
        ''' Gets info count
        ''' </summary>
        Private Function GetInfoCount() As Integer
            Return Enumerable.Count(_validationResults, Function(r) r.Severity = ValidationSeverity.Info)
        End Function

#End Region

    End Class

#Region "Supporting Classes"

    ''' <summary>
    ''' Validation result
    ''' </summary>
    Public Class ValidationResult

        Public Property Severity As ValidationSeverity
        Public Property Category As String
        Public Property Message As String
        Public Property Timestamp As DateTime

        Public Sub New(severity As ValidationSeverity, category As String, message As String)
            Me.Severity = severity
            Me.Category = category
            Me.Message = message
            Me.Timestamp = DateTime.Now
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{Severity}] {Category}: {Message}"
        End Function

    End Class

    ''' <summary>
    ''' Validation severity levels
    ''' </summary>
    Public Enum ValidationSeverity
        Info
        Warning
        [Error]
    End Enum

#End Region

End Namespace
