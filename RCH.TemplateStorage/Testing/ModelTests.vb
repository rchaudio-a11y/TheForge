''' ============================================================================
''' File: ModelTests.vb
''' Project: RCH.TemplateStorage
''' Purpose: Basic tests for extracted model functionality
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active - Test File
''' Version: 1.0.0
''' 
''' Description:
'''   Simple test file to verify extracted models (TemplateDefinition,
'''   TemplateFolderDefinition, TemplateFileDefinition) are functional.
'''   Tests basic instantiation, property assignment, hierarchical structure,
'''   and unlimited nesting support.
''' 
''' Test Coverage:
'''   - TemplateDefinition instantiation and methods
'''   - TemplateFolderDefinition hierarchy and nesting
'''   - TemplateFileDefinition creation and validation
'''   - Recursive operations (folder counting, searching)
'''   - Backward compatibility (original properties preserved)
''' 
''' Note:
'''   This is a manual test file, not a unit test framework test.
'''   For production, consider migrating to xUnit or MSTest.
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.Console
Imports RCH.TemplateStorage.Models

Namespace Testing

    ''' <summary>
    ''' Manual tests for extracted models
    ''' </summary>
    Public Class ModelTests

        ''' <summary>
        ''' Runs all model tests
        ''' </summary>
        Public Shared Sub RunAllTests()
            WriteLine("==========================================")
            WriteLine("RCH.TemplateStorage - Model Tests")
            WriteLine("==========================================")
            WriteLine()

            Try
                TestTemplateDefinitionBasics()
                TestTemplateFolderHierarchy()
                TestUnlimitedNesting()
                TestTemplateFileDefinition()
                TestRecursiveOperations()
                TestBackwardCompatibility()

                WriteLine()
                WriteLine("==========================================")
                WriteLine("? All Tests Passed!")
                WriteLine("==========================================")

            Catch ex As Exception
                WriteLine()
                WriteLine("==========================================")
                WriteLine($"? Test Failed: {ex.Message}")
                WriteLine("==========================================")
                WriteLine(ex.StackTrace)
            End Try
        End Sub

#Region "TemplateDefinition Tests"

        ''' <summary>
        ''' Test 1: TemplateDefinition basic instantiation and properties
        ''' </summary>
        Private Shared Sub TestTemplateDefinitionBasics()
            WriteLine("Test 1: TemplateDefinition Basics")
            WriteLine("------------------------------------------")

            ' Test default constructor
            Dim template1 As New TemplateDefinition()
            AssertNotNull(template1, "Default constructor")

            ' Test name/description constructor (backward compatible)
            Dim template2 As New TemplateDefinition("Test Template", "Test Description")
            AssertEqual(template2.Name, "Test Template", "Name property")
            AssertEqual(template2.Description, "Test Description", "Description property")

            ' Test full constructor
            Dim template3 As New TemplateDefinition("Full Template", "Full Description", "Software", "John Doe")
            AssertEqual(template3.Category, "Software", "Category property")
            AssertEqual(template3.CreatedBy, "John Doe", "CreatedBy property")
            AssertEqual(template3.ModifiedBy, "John Doe", "ModifiedBy property (auto-set)")

            ' Test database properties
            template3.TemplateID = 1
            template3.Tags = "vb.net,template,test"
            template3.IsActive = True
            AssertEqual(template3.TemplateID, 1, "TemplateID property")
            AssertEqual(template3.Tags, "vb.net,template,test", "Tags property")
            AssertEqual(template3.IsActive, True, "IsActive property")

            ' Test AddFolder method (original method preserved)
            Dim folder = template3.AddFolder("TestFolder")
            AssertNotNull(folder, "AddFolder returns folder")
            AssertEqual(template3.Folders.Count, 1, "Folders list count")
            AssertEqual(folder.Name, "TestFolder", "Added folder name")

            ' Test MarkAsModified method
            template3.MarkAsModified("Jane Doe")
            AssertEqual(template3.ModifiedBy, "Jane Doe", "MarkAsModified updates ModifiedBy")

            ' Test IncrementUsage method
            Dim initialCount = template3.UsageCount
            template3.IncrementUsage()
            AssertEqual(template3.UsageCount, initialCount + 1, "IncrementUsage increments count")
            AssertNotNull(template3.LastUsedDate, "IncrementUsage sets LastUsedDate")

            WriteLine("? Test 1 Passed")
            WriteLine()
        End Sub

#End Region

#Region "TemplateFolderDefinition Tests"

        ''' <summary>
        ''' Test 2: TemplateFolderDefinition hierarchy
        ''' </summary>
        Private Shared Sub TestTemplateFolderHierarchy()
            WriteLine("Test 2: TemplateFolderDefinition Hierarchy")
            WriteLine("------------------------------------------")

            ' Test default constructor
            Dim folder1 As New TemplateFolderDefinition()
            AssertNotNull(folder1, "Default constructor")

            ' Test name constructor (backward compatible)
            Dim folder2 As New TemplateFolderDefinition("Root Folder")
            AssertEqual(folder2.Name, "Root Folder", "Name property")
            AssertEqual(folder2.IsSelected, True, "IsSelected default value")

            ' Test full constructor
            Dim folder3 As New TemplateFolderDefinition("Full Folder", "Folder Description", 1)
            AssertEqual(folder3.Description, "Folder Description", "Description property")
            AssertEqual(folder3.TemplateID, 1, "TemplateID property")

            ' Test database properties
            folder3.FolderID = 10
            folder3.ParentFolderID = 5
            folder3.DisplayOrder = 2
            AssertEqual(folder3.FolderID, 10, "FolderID property")
            AssertEqual(folder3.ParentFolderID.Value, 5, "ParentFolderID property")
            AssertEqual(folder3.DisplayOrder, 2, "DisplayOrder property")

            ' Test AddSubFolder method (original method preserved)
            Dim subFolder1 = folder3.AddSubFolder("SubFolder1")
            AssertNotNull(subFolder1, "AddSubFolder returns folder")
            AssertEqual(folder3.SubFolders.Count, 1, "SubFolders list count")
            AssertEqual(subFolder1.Name, "SubFolder1", "SubFolder name")

            ' Test enhanced AddSubFolder with description
            Dim subFolder2 = folder3.AddSubFolder("SubFolder2", "Sub description")
            AssertEqual(subFolder2.Description, "Sub description", "Enhanced AddSubFolder sets description")

            ' Test AddFile method
            Dim file1 = folder3.AddFile("README.md")
            AssertNotNull(file1, "AddFile returns file")
            AssertEqual(folder3.Files.Count, 1, "Files list count")
            AssertEqual(file1.FileName, "README.md", "File name")

            WriteLine("? Test 2 Passed")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 3: Unlimited nesting support
        ''' </summary>
        Private Shared Sub TestUnlimitedNesting()
            WriteLine("Test 3: Unlimited Nesting Support")
            WriteLine("------------------------------------------")

            ' Create root folder
            Dim root As New TemplateFolderDefinition("Root")

            ' Create deep nesting: Root ? Level1 ? Level2 ? Level3 ? Level4 ? Level5
            Dim level1 = root.AddSubFolder("Level1")
            Dim level2 = level1.AddSubFolder("Level2")
            Dim level3 = level2.AddSubFolder("Level3")
            Dim level4 = level3.AddSubFolder("Level4")
            Dim level5 = level4.AddSubFolder("Level5")

            ' Verify structure
            AssertEqual(root.SubFolders.Count, 1, "Root has 1 child")
            AssertEqual(level1.SubFolders.Count, 1, "Level1 has 1 child")
            AssertEqual(level2.SubFolders.Count, 1, "Level2 has 1 child")
            AssertEqual(level3.SubFolders.Count, 1, "Level3 has 1 child")
            AssertEqual(level4.SubFolders.Count, 1, "Level4 has 1 child")
            AssertEqual(level5.SubFolders.Count, 0, "Level5 has no children")

            ' Verify names
            AssertEqual(level5.Name, "Level5", "Deep nesting preserves names")

            ' Test GetTotalSubFolderCount (recursive)
            Dim totalCount = root.GetTotalSubFolderCount()
            AssertEqual(totalCount, 5, "GetTotalSubFolderCount counts all nested folders")

            WriteLine("? Test 3 Passed - Unlimited nesting functional")
            WriteLine()
        End Sub

#End Region

#Region "TemplateFileDefinition Tests"

        ''' <summary>
        ''' Test 4: TemplateFileDefinition creation and validation
        ''' </summary>
        Private Shared Sub TestTemplateFileDefinition()
            WriteLine("Test 4: TemplateFileDefinition Tests")
            WriteLine("------------------------------------------")

            ' Test default constructor
            Dim file1 As New TemplateFileDefinition()
            AssertNotNull(file1, "Default constructor")

            ' Test filename constructor
            Dim file2 As New TemplateFileDefinition("Program.vb")
            AssertEqual(file2.FileName, "Program.vb", "FileName property")
            AssertEqual(file2.FileType, ".vb", "FileType auto-extracted")

            ' Test full constructor
            Dim file3 As New TemplateFileDefinition("README.md", "# Project Title", "Documentation file", False)
            AssertEqual(file3.ContentTemplate, "# Project Title", "ContentTemplate property")
            AssertEqual(file3.Description, "Documentation file", "Description property")
            AssertEqual(file3.RequiresMetadataHeader, False, "RequiresMetadataHeader property")

            ' Test GetFileNameWithoutExtension
            AssertEqual(file2.GetFileNameWithoutExtension(), "Program", "GetFileNameWithoutExtension")

            ' Test GetExtension
            AssertEqual(file2.GetExtension(), ".vb", "GetExtension")

            ' Test IsCodeFile
            AssertEqual(file2.IsCodeFile(), True, "IsCodeFile detects .vb")
            AssertEqual(file3.IsCodeFile(), False, "IsCodeFile rejects .md")

            ' Test IsDocumentationFile
            AssertEqual(file3.IsDocumentationFile(), True, "IsDocumentationFile detects .md")
            AssertEqual(file2.IsDocumentationFile(), False, "IsDocumentationFile rejects .vb")

            ' Test GenerateContent with placeholders
            Dim file4 As New TemplateFileDefinition("Config.txt", "Project: {ProjectName}, Date: {Date}")
            Dim placeholders As New Dictionary(Of String, String) From {
                {"ProjectName", "TestProject"},
                {"Date", "2026-01-05"}
            }
            Dim generated = file4.GenerateContent(placeholders)
            AssertEqual(generated, "Project: TestProject, Date: 2026-01-05", "GenerateContent replaces placeholders")

            ' Test Validate
            Dim file5 As New TemplateFileDefinition("ValidFile.txt")
            Dim errorMsg As String = ""
            Dim isValid = file5.Validate(errorMsg)
            AssertEqual(isValid, True, "Validate passes for valid file")

            Dim file6 As New TemplateFileDefinition()
            file6.FileName = "NoExtension"
            isValid = file6.Validate(errorMsg)
            AssertEqual(isValid, False, "Validate fails for file without extension")

            WriteLine("? Test 4 Passed")
            WriteLine()
        End Sub

#End Region

#Region "Recursive Operations Tests"

        ''' <summary>
        ''' Test 5: Recursive operations (counting, searching)
        ''' </summary>
        Private Shared Sub TestRecursiveOperations()
            WriteLine("Test 5: Recursive Operations")
            WriteLine("------------------------------------------")

            ' Create template with complex structure
            Dim template As New TemplateDefinition("Complex Template", "Multi-level structure")

            ' Add folders with deep nesting
            Dim source = template.AddFolder("Source")
            Dim backend = source.AddSubFolder("Backend")
            Dim controllers = backend.AddSubFolder("Controllers")
            Dim services = backend.AddSubFolder("Services")

            Dim frontend = source.AddSubFolder("Frontend")
            Dim components = frontend.AddSubFolder("Components")

            Dim tests = template.AddFolder("Tests")
            Dim unit = tests.AddSubFolder("Unit")
            Dim integration = tests.AddSubFolder("Integration")

            ' Add files to various folders
            source.AddFile("README.md")
            controllers.AddFile("UserController.vb")
            controllers.AddFile("ProductController.vb")
            services.AddFile("UserService.vb")
            components.AddFile("Header.html")
            unit.AddFile("UserServiceTests.vb")

            ' Test GetTotalFolderCount
            Dim totalFolders = template.GetTotalFolderCount()
            AssertEqual(totalFolders, 8, "GetTotalFolderCount counts all nested folders")

            ' Test GetTotalSubFolderCount on specific folder
            Dim sourceSubCount = source.GetTotalSubFolderCount()
            AssertEqual(sourceSubCount, 4, "Source folder has 4 nested subfolders")

            ' Test GetTotalFileCount (recursive)
            Dim totalFiles = source.GetTotalFileCount()
            AssertEqual(totalFiles, 5, "Source folder has 5 files recursively")

            ' Test FindSubFolder (immediate children only)
            Dim found1 = source.FindSubFolder("Backend")
            AssertNotNull(found1, "FindSubFolder finds immediate child")
            AssertEqual(found1.Name, "Backend", "Found correct folder")

            Dim notFound1 = source.FindSubFolder("Controllers")
            AssertNull(notFound1, "FindSubFolder doesn't find grandchildren")

            ' Test FindSubFolderRecursive (entire tree)
            Dim found2 = source.FindSubFolderRecursive("Controllers")
            AssertNotNull(found2, "FindSubFolderRecursive finds nested folder")
            AssertEqual(found2.Name, "Controllers", "Found correct nested folder")

            Dim notFound2 = source.FindSubFolderRecursive("NonExistent")
            AssertNull(notFound2, "FindSubFolderRecursive returns Nothing when not found")

            WriteLine("? Test 5 Passed")
            WriteLine()
        End Sub

#End Region

#Region "Backward Compatibility Tests"

        ''' <summary>
        ''' Test 6: Backward compatibility with original DirectoryTemplate/FolderDefinition
        ''' </summary>
        Private Shared Sub TestBackwardCompatibility()
            WriteLine("Test 6: Backward Compatibility")
            WriteLine("------------------------------------------")

            ' Test that original DirectoryTemplate interface is preserved
            Dim template As New TemplateDefinition("Legacy Template", "Original interface test")

            ' Original properties must be accessible
            AssertEqual(template.Name, "Legacy Template", "Original Name property preserved")
            AssertEqual(template.Description, "Original interface test", "Original Description property preserved")
            AssertNotNull(template.Folders, "Original Folders property preserved")

            ' Original AddFolder method must work
            Dim folder1 = template.AddFolder("OriginalFolder")
            AssertNotNull(folder1, "Original AddFolder method preserved")
            AssertEqual(folder1.Name, "OriginalFolder", "AddFolder creates folder correctly")

            ' Test that original FolderDefinition interface is preserved
            Dim rootFolder As New TemplateFolderDefinition("Root")

            ' Original properties must be accessible
            AssertEqual(rootFolder.Name, "Root", "Original Name property preserved")
            AssertEqual(rootFolder.IsSelected, True, "Original IsSelected property preserved")
            AssertNotNull(rootFolder.SubFolders, "Original SubFolders property preserved")

            ' Original AddSubFolder method must work
            Dim subFolder1 = rootFolder.AddSubFolder("OriginalSubFolder")
            AssertNotNull(subFolder1, "Original AddSubFolder method preserved")
            AssertEqual(subFolder1.Name, "OriginalSubFolder", "AddSubFolder creates subfolder correctly")

            ' Test unlimited nesting (original capability)
            Dim level2 = subFolder1.AddSubFolder("Level2")
            Dim level3 = level2.AddSubFolder("Level3")
            AssertNotNull(level3, "Unlimited nesting preserved")

            WriteLine("? Test 6 Passed - Backward compatibility maintained")
            WriteLine()
        End Sub

#End Region

#Region "Test Helper Methods"

        Private Shared Sub AssertEqual(Of T)(actual As T, expected As T, message As String)
            If Not Object.Equals(actual, expected) Then
                Throw New Exception($"Assert Failed: {message}" & vbCrLf &
                                  $"  Expected: {expected}" & vbCrLf &
                                  $"  Actual: {actual}")
            End If
            WriteLine($"  ? {message}")
        End Sub

        Private Shared Sub AssertNotNull(obj As Object, message As String)
            If obj Is Nothing Then
                Throw New Exception($"Assert Failed: {message} - Object is Nothing")
            End If
            WriteLine($"  ? {message}")
        End Sub

        Private Shared Sub AssertNull(obj As Object, message As String)
            If obj IsNot Nothing Then
                Throw New Exception($"Assert Failed: {message} - Object is not Nothing")
            End If
            WriteLine($"  ? {message}")
        End Sub

#End Region

    End Class

End Namespace
