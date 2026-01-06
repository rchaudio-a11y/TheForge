''' ============================================================================
''' File: CreateTestDatabase.vb
''' Project: RCH.TemplateStorage
''' Purpose: Test database creation script
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active - Test Script
''' Version: 1.0.0
''' 
''' Description:
'''   Creates a test database with sample data for RCH.TemplateStorage.
'''   Uses DatabaseInitializer to create the database structure and populate
'''   with test templates including legacy-compatible data.
''' 
''' Requirements:
'''   - MS Access Database Engine (ACE) installed
'''   - Write permissions to output directory
''' 
''' Usage:
'''   Dim creator As New CreateTestDatabase()
'''   creator.CreateDatabase("C:\Temp\Templates_Test.accdb")
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports RCH.TemplateStorage.Database

Namespace Testing

    ''' <summary>
    ''' Creates test database with sample data
    ''' </summary>
    Public Class CreateTestDatabase

#Region "Public Methods"

        ''' <summary>
        ''' Creates test database at specified path
        ''' </summary>
        ''' <param name="databasePath">Full path to create database</param>
        ''' <returns>True if successful</returns>
        Public Function CreateDatabase(databasePath As String) As Boolean
            Try
                Console.WriteLine("==========================================")
                Console.WriteLine("Creating Test Database")
                Console.WriteLine("==========================================")
                Console.WriteLine($"Path: {databasePath}")
                Console.WriteLine()

                ' Check if file already exists
                If File.Exists(databasePath) Then
                    Console.WriteLine("WARNING: Database already exists. Deleting...")
                    File.Delete(databasePath)
                End If

                ' Create database
                Console.WriteLine("Step 1: Creating database file...")
                Dim initializer As New DatabaseInitializer(databasePath)

                If Not initializer.CreateDatabase() Then
                    Console.WriteLine("ERROR: Failed to create database file")
                    Return False
                End If
                Console.WriteLine("✓ Database file created")
                Console.WriteLine()

                ' Initialize schema
                Console.WriteLine("Step 2: Initializing schema...")
                If Not initializer.InitializeSchema(includeSampleData:=True) Then
                    Console.WriteLine("ERROR: Failed to initialize schema")
                    Return False
                End If
                Console.WriteLine("✓ Schema initialized with sample data")
                Console.WriteLine()

                ' Validate database
                Console.WriteLine("Step 3: Validating database...")
                If Not ValidateDatabase(databasePath) Then
                    Console.WriteLine("WARNING: Database validation found issues")
                Else
                    Console.WriteLine("✓ Database validated successfully")
                End If
                Console.WriteLine()

                ' Display summary
                DisplayDatabaseSummary(databasePath)

                Console.WriteLine("==========================================")
                Console.WriteLine("✅ Test Database Created Successfully!")
                Console.WriteLine("==========================================")
                Console.WriteLine($"Location: {databasePath}")
                Console.WriteLine()

                Return True

            Catch ex As Exception
                Console.WriteLine()
                Console.WriteLine("==========================================")
                Console.WriteLine($"❌ ERROR: {ex.Message}")
                Console.WriteLine("==========================================")
                Console.WriteLine(ex.StackTrace)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Creates test database in default location
        ''' </summary>
        ''' <returns>True if successful</returns>
        Public Function CreateDefaultDatabase() As Boolean
            Dim defaultPath = Path.Combine(Environment.CurrentDirectory, "Templates_Test.accdb")
            Return CreateDatabase(defaultPath)
        End Function

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Validates the created database
        ''' </summary>
        Private Function ValidateDatabase(databasePath As String) As Boolean
            Try
                Using connection As New DatabaseConnection(databasePath)
                    Dim validator As New DatabaseValidator(connection)

                    ' Validate schema
                    If Not validator.ValidateSchema() Then
                        Console.WriteLine("  Schema validation failed:")
                        For Each result In validator.GetValidationResults()
                            If result.Severity = ValidationSeverity.Error Then
                                Console.WriteLine($"    [ERROR] {result.Message}")
                            End If
                        Next
                        Return False
                    End If

                    ' Validate referential integrity
                    If Not validator.ValidateReferentialIntegrity() Then
                        Console.WriteLine("  Integrity validation failed:")
                        For Each result In validator.GetValidationResults()
                            If result.Severity = ValidationSeverity.Error Then
                                Console.WriteLine($"    [ERROR] {result.Message}")
                            End If
                        Next
                        Return False
                    End If

                    Return True
                End Using

            Catch ex As Exception
                Console.WriteLine($"  Validation error: {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Displays database summary statistics
        ''' </summary>
        Private Sub DisplayDatabaseSummary(databasePath As String)
            Try
                Using connection As New DatabaseConnection(databasePath)
                    connection.Open()

                    ' Count templates
                    Dim templateCount = CInt(connection.ExecuteScalar("SELECT COUNT(*) FROM Template"))
                    Console.WriteLine($"Templates: {templateCount}")

                    ' Count folders
                    Dim folderCount = CInt(connection.ExecuteScalar("SELECT COUNT(*) FROM TemplateFolder"))
                    Console.WriteLine($"Folders: {folderCount}")

                    ' Count files
                    Dim fileCount = CInt(connection.ExecuteScalar("SELECT COUNT(*) FROM TemplateFile"))
                    Console.WriteLine($"Files: {fileCount}")

                    Console.WriteLine()

                    ' List templates
                    Console.WriteLine("Sample Templates:")
                    Using reader = connection.ExecuteReader("SELECT TemplateID, Name, Category FROM Template")
                        While reader.Read()
                            Console.WriteLine($"  - [{reader("TemplateID")}] {reader("Name")} ({reader("Category")})")
                        End While
                    End Using
                End Using

            Catch ex As Exception
                Console.WriteLine($"Could not display summary: {ex.Message}")
            End Try
        End Sub

#End Region

#Region "Static Helper Methods"

        ''' <summary>
        ''' Runs test database creation with default settings
        ''' </summary>
        Public Shared Sub RunTest()
            Dim creator As New CreateTestDatabase()
            creator.CreateDefaultDatabase()
        End Sub

#End Region

    End Class

End Namespace
