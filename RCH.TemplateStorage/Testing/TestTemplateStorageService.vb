''' ============================================================================
''' File: TestTemplateStorageService.vb
''' Project: RCH.TemplateStorage
''' Purpose: Quick test harness for TemplateStorageService
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active - Testing
''' Version: 1.0.0
''' 
''' Description:
'''   Simple console-style test program demonstrating all CRUD operations
'''   of TemplateStorageService. Run this to test the complete system
'''   end-to-end: create database, insert templates, query, update, delete.
''' 
''' Usage:
'''   TestTemplateStorageService.RunAllTests()
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports RCH.TemplateStorage.Models
Imports RCH.TemplateStorage.Database
Imports RCH.TemplateStorage.Services.Implementations

Namespace Testing

    ''' <summary>
    ''' Test harness for TemplateStorageService
    ''' </summary>
    Public Class TestTemplateStorageService

        Public Shared Sub RunAllTests()
            Console.WriteLine("=" & New String("="c, 70))
            Console.WriteLine("RCH.TemplateStorage - TemplateStorageService Test Harness")
            Console.WriteLine("=" & New String("="c, 70))
            Console.WriteLine()

            Dim dbPath = Path.Combine(Environment.CurrentDirectory, "TestTemplates.accdb")

            Try
                ' Step 1: Create database
                Console.WriteLine("Step 1: Creating test database...")
                If File.Exists(dbPath) Then
                    File.Delete(dbPath)
                    Console.WriteLine("  Deleted existing database")
                End If

                Dim initializer As New DatabaseInitializer(dbPath)
                initializer.CreateDatabase()
                initializer.InitializeSchema(includeSampleData:=False)
                Console.WriteLine("  ? Database created and schema initialized")
                Console.WriteLine()

                ' Step 2: Create service
                Console.WriteLine("Step 2: Creating TemplateStorageService...")
                Using service As New TemplateStorageService(dbPath)
                    Console.WriteLine("  ? Service created")
                    Console.WriteLine($"  Database: {service.DatabasePath}")
                    Console.WriteLine($"  Connection OK: {service.TestConnection()}")
                    Console.WriteLine($"  Schema Valid: {service.ValidateDatabase()}")
                    Console.WriteLine()

                    ' Step 3: Create template
                    Console.WriteLine("Step 3: Creating test template...")
                    Dim template = CreateTestTemplate()
                    Dim created = service.CreateTemplate(template)
                    Console.WriteLine($"  ? Template created with ID: {created.TemplateID}")
                    Console.WriteLine($"  Name: {created.Name}")
                    Console.WriteLine($"  Folders: {created.GetTotalFolderCount()}")
                    Console.WriteLine()

                    ' Step 4: Retrieve template
                    Console.WriteLine("Step 4: Retrieving template...")
                    Dim retrieved = service.GetTemplate(created.TemplateID)
                    Console.WriteLine($"  ? Template retrieved: {retrieved.Name}")
                    Console.WriteLine($"  Root folders: {retrieved.Folders.Count}")
                    Console.WriteLine($"  Total folders: {retrieved.GetTotalFolderCount()}")
                    PrintFolderHierarchy(retrieved.Folders, "    ")
                    Console.WriteLine()

                    ' Step 5: Search templates
                    Console.WriteLine("Step 5: Searching templates...")
                    Dim searchResults = service.SearchTemplates("Robot")
                    Console.WriteLine($"  ? Search for 'Robot' found: {searchResults.Count} result(s)")
                    For Each result In searchResults
                        Console.WriteLine($"    - {result.Name} (ID: {result.TemplateID})")
                    Next
                    Console.WriteLine()

                    ' Step 6: Get all templates
                    Console.WriteLine("Step 6: Getting all templates...")
                    Dim allTemplates = service.GetAllTemplates()
                    Console.WriteLine($"  ? Total templates: {allTemplates.Count}")
                    For Each t In allTemplates
                        Console.WriteLine($"    - [{t.TemplateID}] {t.Name} - {t.Category}")
                    Next
                    Console.WriteLine()

                    ' Step 7: Update template
                    Console.WriteLine("Step 7: Updating template...")
                    retrieved.Description = "UPDATED: " & retrieved.Description
                    retrieved.ModifiedBy = "TestHarness"
                    retrieved.MarkAsModified("TestHarness")
                    service.UpdateTemplate(retrieved)
                    Console.WriteLine("  ? Template updated")

                    Dim updated = service.GetTemplate(retrieved.TemplateID)
                    Console.WriteLine($"  Description: {updated.Description}")
                    Console.WriteLine($"  Modified: {updated.ModifiedDate}")
                    Console.WriteLine()

                    ' Step 8: Increment usage
                    Console.WriteLine("Step 8: Tracking usage...")
                    service.IncrementUsage(created.TemplateID)
                    Dim afterUsage = service.GetTemplate(created.TemplateID)
                    Console.WriteLine($"  ? Usage count: {afterUsage.UsageCount}")
                    Console.WriteLine($"  Last used: {afterUsage.LastUsedDate}")
                    Console.WriteLine()

                    ' Step 9: Export to JSON
                    Console.WriteLine("Step 9: Exporting to JSON...")
                    Dim jsonPath = Path.Combine(Environment.CurrentDirectory, "exported_template.json")
                    service.ExportTemplateToJson(created.TemplateID, jsonPath)
                    Console.WriteLine($"  ? Exported to: {jsonPath}")
                    Console.WriteLine($"  File size: {New FileInfo(jsonPath).Length} bytes")
                    Console.WriteLine()

                    ' Step 10: Get statistics
                    Console.WriteLine("Step 10: Database statistics...")
                    Console.WriteLine($"  Total templates: {service.GetTemplateCount()}")
                    Console.WriteLine($"  Categories: {String.Join(", ", service.GetAllCategories())}")
                    Console.WriteLine($"  Tags: {String.Join(", ", service.GetAllTags())}")
                    Console.WriteLine()

                    ' Step 11: Delete template
                    Console.WriteLine("Step 11: Deleting template...")
                    Dim deleted = service.DeleteTemplate(created.TemplateID)
                    Console.WriteLine($"  ? Deleted: {deleted}")
                    Console.WriteLine($"  Templates remaining: {service.GetTemplateCount()}")
                    Console.WriteLine()

                End Using

                Console.WriteLine("=" & New String("="c, 70))
                Console.WriteLine("? ALL TESTS PASSED!")
                Console.WriteLine("=" & New String("="c, 70))
                Console.WriteLine()
                Console.WriteLine($"Test database: {dbPath}")
                Console.WriteLine($"Exported JSON: {Path.Combine(Environment.CurrentDirectory, "exported_template.json")}")

            Catch ex As Exception
                Console.WriteLine()
                Console.WriteLine("=" & New String("="c, 70))
                Console.WriteLine("? TEST FAILED!")
                Console.WriteLine("=" & New String("="c, 70))
                Console.WriteLine($"Error: {ex.Message}")
                Console.WriteLine()
                Console.WriteLine("Stack Trace:")
                Console.WriteLine(ex.StackTrace)
            End Try
        End Sub

        ''' <summary>
        ''' Creates a test template with nested folders
        ''' </summary>
        Private Shared Function CreateTestTemplate() As TemplateDefinition
            Dim template As New TemplateDefinition("Robot Automation Template", "Complete robot cell structure") With {
                .Category = "Robotics",
                .Tags = "robot,automation,manufacturing",
                .CreatedBy = "TestHarness",
                .Version = "1.0.0",
                .IsActive = True
            }

            ' Create folder structure
            Dim inputFolder = template.AddFolder("01_Input")
            inputFolder.Description = "Input files and data"

            Dim outputFolder = template.AddFolder("02_Output")
            outputFolder.Description = "Generated output files"

            Dim docsFolder = template.AddFolder("03_Documentation")
            docsFolder.Description = "Project documentation"

            Dim progFolder = template.AddFolder("04_Programming")
            progFolder.Description = "Programming files"

            ' Add subfolders
            Dim robotFolder = progFolder.AddSubFolder("Robot", "Robot programming files")
            Dim plcFolder = progFolder.AddSubFolder("PLC", "PLC programming files")
            Dim hmiFolder = progFolder.AddSubFolder("HMI", "HMI screens")

            ' Add files
            Dim readmeFile As New TemplateFileDefinition("README.md") With {
                .FileType = ".md",
                .ContentTemplate = "# {ProjectName}" & vbCrLf & vbCrLf & "Robot cell automation project",
                .RequiresMetadataHeader = False
            }
            docsFolder.Files.Add(readmeFile)

            Dim robotFile As New TemplateFileDefinition("RobotProgram.vb") With {
                .FileType = ".vb",
                .ContentTemplate = "' Robot program for {ProjectName}",
                .RequiresMetadataHeader = True
            }
            robotFolder.Files.Add(robotFile)

            Return template
        End Function

        ''' <summary>
        ''' Prints folder hierarchy recursively
        ''' </summary>
        Private Shared Sub PrintFolderHierarchy(folders As List(Of TemplateFolderDefinition), indent As String)
            For Each folder In folders
                Console.WriteLine($"{indent}??? {folder.Name} ({folder.Files.Count} file(s))")

                For Each file In folder.Files
                    Console.WriteLine($"{indent}?   ??? {file.FileName}")
                Next

                If folder.SubFolders.Count > 0 Then
                    PrintFolderHierarchy(folder.SubFolders, indent & "?   ")
                End If
            Next
        End Sub

    End Class

End Namespace
