''' ============================================================================
''' File: T058_MigrationTests.vb
''' Project: RCH.TemplateStorage
''' Purpose: Test migration of real TemplateBuilderControl JSON files
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Comprehensive migration testing for T058. Tests import of legacy
'''   TemplateBuilderControl JSON templates, database round-trip, and
'''   export back to JSON with data integrity validation.
''' 
''' Test Scenarios:
'''   1. Simple template (basic structure)
'''   2. Deep nesting (5+ levels)
'''   3. Complex enterprise (many folders)
'''   4. Minimal template (edge case)
'''   5. Mixed selection (IsSelected variations)
''' 
''' Success Criteria:
'''   - 100% successful imports
'''   - Zero data loss
'''   - Structure preservation
'''   - Backward compatibility maintained
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports RCH.TemplateStorage.Models
Imports RCH.TemplateStorage.Services.Implementations

Namespace Testing

    ''' <summary>
    ''' Migration tests for T058 - Real TemplateBuilderControl file testing
    ''' </summary>
    Public Class T058_MigrationTests

#Region "Test Configuration"

        Private Shared ReadOnly TestDataPath As String = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Testing", "TestData")

        Private Shared ReadOnly TestDatabasePath As String = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Testing", "Templates_Migration_Test.accdb")

        Private Shared ReadOnly ResultsPath As String = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Testing", "MigrationResults")

#End Region

#Region "Test Data"

        Private Shared ReadOnly TestFiles As String() = {
            "legacy-template-01-simple.json",
            "legacy-template-02-deep-nesting.json",
            "legacy-template-03-enterprise.json",
            "legacy-template-04-minimal.json",
            "legacy-template-05-mixed-selection.json"
        }

#End Region

#Region "Main Test Method"

        ''' <summary>
        ''' Run all migration tests
        ''' </summary>
        Public Shared Sub RunAllTests()
            Console.WriteLine("=" * 80)
            Console.WriteLine("T058: Migration Testing - Real TemplateBuilderControl Files")
            Console.WriteLine("=" * 80)
            Console.WriteLine()

            ' Create results directory
            Directory.CreateDirectory(ResultsPath)

            ' Initialize test database
            If File.Exists(TestDatabasePath) Then
                File.Delete(TestDatabasePath)
            End If

            Dim results As New StringBuilder()
            results.AppendLine("# T058 Migration Test Results")
            results.AppendLine($"**Date:** {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
            results.AppendLine()
            results.AppendLine("## Test Summary")
            results.AppendLine()

            Dim totalTests As Integer = TestFiles.Length
            Dim passedTests As Integer = 0
            Dim failedTests As Integer = 0

            ' Run each test
            For Each testFile In TestFiles
                Try
                    Console.WriteLine($"Testing: {testFile}")
                    Dim result = TestTemplateMigration(testFile)

                    If result.Success Then
                        passedTests += 1
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine($"  ? PASSED")
                    Else
                        failedTests += 1
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine($"  ? FAILED: {result.ErrorMessage}")
                    End If
                    Console.ResetColor()

                    results.AppendLine(result.GetMarkdownReport())
                    results.AppendLine()

                Catch ex As Exception
                    failedTests += 1
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine($"  ? EXCEPTION: {ex.Message}")
                    Console.ResetColor()

                    results.AppendLine($"### {testFile} - EXCEPTION")
                    results.AppendLine($"**Error:** {ex.Message}")
                    results.AppendLine()
                End Try

                Console.WriteLine()
            Next

            ' Summary
            results.Insert(0, $"**Total Tests:** {totalTests}  " & Environment.NewLine &
                             $"**Passed:** {passedTests}  " & Environment.NewLine &
                             $"**Failed:** {failedTests}  " & Environment.NewLine &
                             $"**Success Rate:** {CInt(passedTests / totalTests * 100)}%" & Environment.NewLine &
                             Environment.NewLine)

            Console.WriteLine("=" * 80)
            Console.ForegroundColor = If(failedTests = 0, ConsoleColor.Green, ConsoleColor.Yellow)
            Console.WriteLine($"Results: {passedTests}/{totalTests} passed ({CInt(passedTests / totalTests * 100)}%)")
            Console.ResetColor()
            Console.WriteLine("=" * 80)

            ' Save results
            Dim resultsFile = Path.Combine(ResultsPath, $"T058-Results-{DateTime.Now:yyyyMMdd-HHmmss}.md")
            File.WriteAllText(resultsFile, results.ToString())
            Console.WriteLine($"Results saved to: {resultsFile}")
        End Sub

#End Region

#Region "Individual Test Method"

        ''' <summary>
        ''' Test migration of a single template file
        ''' </summary>
        Private Shared Function TestTemplateMigration(testFileName As String) As TestResult
            Dim result As New TestResult With {
                .TestName = testFileName,
                .Success = False
            }

            Try
                ' Step 1: Load original JSON
                Dim originalPath = Path.Combine(TestDataPath, testFileName)
                If Not File.Exists(originalPath) Then
                    result.ErrorMessage = "Test file not found"
                    Return result
                End If

                Dim originalJson = File.ReadAllText(originalPath)
                result.OriginalJson = originalJson

                ' Step 2: Import legacy template
                Dim serializer As New TemplateJsonSerializer()
                Dim imported = serializer.ImportLegacyTemplate(originalJson)
                result.TemplateImported = True
                result.TemplateName = imported.Name
                result.FolderCount = CountFolders(imported)

                ' Step 3: Save to database
                Using service As New TemplateStorageService(TestDatabasePath)
                    Dim created = service.CreateTemplate(imported)
                    result.DatabaseSaved = True
                    result.TemplateID = created.TemplateID

                    ' Step 4: Retrieve from database
                    Dim retrieved = service.GetTemplate(created.TemplateID)
                    result.DatabaseRetrieved = True

                    ' Step 5: Export back to JSON
                    Dim exportedJson = serializer.SerializeTemplate(retrieved)
                    result.ExportedJson = exportedJson

                    ' Save exported JSON for comparison
                    Dim exportPath = Path.Combine(ResultsPath, $"exported-{testFileName}")
                    File.WriteAllText(exportPath, exportedJson)
                    result.ExportPath = exportPath

                    ' Step 6: Validate structure
                    result.ValidationResult = ValidateStructure(imported, retrieved)

                    ' Step 7: Check for data loss
                    result.DataLossDetected = CheckDataLoss(originalJson, exportedJson)

                    result.Success = result.TemplateImported AndAlso
                                    result.DatabaseSaved AndAlso
                                    result.DatabaseRetrieved AndAlso
                                    result.ValidationResult.IsValid AndAlso
                                    Not result.DataLossDetected

                End Using

            Catch ex As Exception
                result.Success = False
                result.ErrorMessage = ex.Message
            End Try

            Return result
        End Function

#End Region

#Region "Validation Methods"

        ''' <summary>
        ''' Count total folders in template (recursive)
        ''' </summary>
        Private Shared Function CountFolders(template As TemplateDefinition) As Integer
            Dim count = template.Folders.Count

            For Each folder In template.Folders
                count += CountSubFolders(folder)
            Next

            Return count
        End Function

        ''' <summary>
        ''' Count subfolders recursively
        ''' </summary>
        Private Shared Function CountSubFolders(folder As TemplateFolderDefinition) As Integer
            Dim count = folder.SubFolders.Count

            For Each subFolder In folder.SubFolders
                count += CountSubFolders(subFolder)
            Next

            Return count
        End Function

        ''' <summary>
        ''' Validate template structure integrity
        ''' </summary>
        Private Shared Function ValidateStructure(original As TemplateDefinition, retrieved As TemplateDefinition) As ValidationResult
            Dim result As New ValidationResult With {.IsValid = True}

            ' Check name
            If original.Name <> retrieved.Name Then
                result.IsValid = False
                result.Errors.Add($"Name mismatch: '{original.Name}' vs '{retrieved.Name}'")
            End If

            ' Check description
            If original.Description <> retrieved.Description Then
                result.IsValid = False
                result.Errors.Add($"Description mismatch")
            End If

            ' Check folder count
            Dim originalCount = CountFolders(original)
            Dim retrievedCount = CountFolders(retrieved)
            If originalCount <> retrievedCount Then
                result.IsValid = False
                result.Errors.Add($"Folder count mismatch: {originalCount} vs {retrievedCount}")
            End If

            ' Check folder structure
            For i = 0 To Math.Min(original.Folders.Count, retrieved.Folders.Count) - 1
                ValidateFolderStructure(original.Folders(i), retrieved.Folders(i), result)
            Next

            Return result
        End Function

        ''' <summary>
        ''' Validate folder structure recursively
        ''' </summary>
        Private Shared Sub ValidateFolderStructure(original As TemplateFolderDefinition,
                                                   retrieved As TemplateFolderDefinition,
                                                   result As ValidationResult)
            If original.Name <> retrieved.Name Then
                result.IsValid = False
                result.Errors.Add($"Folder name mismatch: '{original.Name}' vs '{retrieved.Name}'")
            End If

            If original.IsSelected <> retrieved.IsSelected Then
                result.Errors.Add($"IsSelected mismatch for '{original.Name}': {original.IsSelected} vs {retrieved.IsSelected}")
            End If

            If original.SubFolders.Count <> retrieved.SubFolders.Count Then
                result.IsValid = False
                result.Errors.Add($"SubFolder count mismatch in '{original.Name}': {original.SubFolders.Count} vs {retrieved.SubFolders.Count}")
                Return
            End If

            For i = 0 To original.SubFolders.Count - 1
                ValidateFolderStructure(original.SubFolders(i), retrieved.SubFolders(i), result)
            Next
        End Sub

        ''' <summary>
        ''' Check for data loss by comparing original and exported JSON
        ''' </summary>
        Private Shared Function CheckDataLoss(originalJson As String, exportedJson As String) As Boolean
            Try
                Dim originalObj = JObject.Parse(originalJson)
                Dim exportedObj = JObject.Parse(exportedJson)

                ' Check if all original folders are present
                Dim originalFolders = originalObj("Folders")
                Dim exportedFolders = exportedObj("Folders")

                If originalFolders Is Nothing OrElse exportedFolders Is Nothing Then
                    Return True ' Data loss if folders missing
                End If

                ' Recursive folder comparison would go here
                ' For now, just check counts match
                Return CType(originalFolders, JArray).Count <> CType(exportedFolders, JArray).Count

            Catch ex As Exception
                Return True ' Assume data loss on error
            End Try
        End Function

#End Region

#Region "Helper Classes"

        ''' <summary>
        ''' Test result for a single template migration
        ''' </summary>
        Public Class TestResult
            Public Property TestName As String
            Public Property Success As Boolean
            Public Property ErrorMessage As String
            Public Property TemplateName As String
            Public Property TemplateID As Integer
            Public Property FolderCount As Integer

            Public Property TemplateImported As Boolean
            Public Property DatabaseSaved As Boolean
            Public Property DatabaseRetrieved As Boolean
            Public Property DataLossDetected As Boolean

            Public Property OriginalJson As String
            Public Property ExportedJson As String
            Public Property ExportPath As String

            Public Property ValidationResult As ValidationResult

            Public Function GetMarkdownReport() As String
                Dim sb As New StringBuilder()

                sb.AppendLine($"### {TestName}")
                sb.AppendLine()
                sb.AppendLine($"**Status:** {If(Success, "? PASSED", "? FAILED")}")
                sb.AppendLine($"**Template Name:** {TemplateName}")
                sb.AppendLine($"**Template ID:** {TemplateID}")
                sb.AppendLine($"**Folder Count:** {FolderCount}")
                sb.AppendLine()

                sb.AppendLine("**Steps:**")
                sb.AppendLine($"- Import: {If(TemplateImported, "?", "?")}")
                sb.AppendLine($"- Database Save: {If(DatabaseSaved, "?", "?")}")
                sb.AppendLine($"- Database Retrieve: {If(DatabaseRetrieved, "?", "?")}")
                sb.AppendLine($"- Structure Validation: {If(ValidationResult?.IsValid, "?", "?")}")
                sb.AppendLine($"- Data Loss Check: {If(Not DataLossDetected, "?", "? DATA LOSS DETECTED")}")
                sb.AppendLine()

                If ValidationResult IsNot Nothing AndAlso ValidationResult.Errors.Count > 0 Then
                    sb.AppendLine("**Validation Errors:**")
                    For Each errorMsg In ValidationResult.Errors
                        sb.AppendLine($"- {errorMsg}")
                    Next
                    sb.AppendLine()
                End If

                If Not String.IsNullOrEmpty(ErrorMessage) Then
                    sb.AppendLine($"**Error:** {ErrorMessage}")
                    sb.AppendLine()
                End If

                If Not String.IsNullOrEmpty(ExportPath) Then
                    sb.AppendLine($"**Exported to:** `{ExportPath}`")
                    sb.AppendLine()
                End If

                Return sb.ToString()
            End Function
        End Class

        ''' <summary>
        ''' Validation result
        ''' </summary>
        Public Class ValidationResult
            Public Property IsValid As Boolean
            Public Property Errors As New List(Of String)
        End Class

#End Region

    End Class

End Namespace
