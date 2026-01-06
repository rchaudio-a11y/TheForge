''' ============================================================================
''' File: JsonSerializationTests.vb
''' Project: RCH.TemplateStorage
''' Purpose: Unit tests for JSON serialization/deserialization functionality
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Comprehensive unit tests for TemplateJsonSerializer covering:
'''   - Serialization/deserialization round-trips
'''   - Legacy format import
'''   - Format conversion accuracy
'''   - Export/import workflow
'''   - Error handling
'''   - Edge cases
''' 
''' Test Coverage:
'''   - Basic serialization/deserialization
'''   - Legacy DirectoryTemplate format
'''   - Complex nested structures
'''   - Null/empty values
'''   - Malformed JSON
'''   - Schema validation
'''   - File I/O operations
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports Newtonsoft.Json
Imports RCH.TemplateStorage.Models
Imports RCH.TemplateStorage.Services.Implementations

Namespace Testing

    ''' <summary>
    ''' Unit tests for JSON serialization/deserialization
    ''' </summary>
    Public Class JsonSerializationTests

#Region "Test Configuration"

        Private Shared ReadOnly TestOutputPath As String = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Testing", "JsonTestResults")

#End Region

#Region "Main Test Runner"

        ''' <summary>
        ''' Run all JSON serialization tests
        ''' </summary>
        Public Shared Sub RunAllTests()
            Console.WriteLine("=" * 80)
            Console.WriteLine("JSON Serialization Unit Tests")
            Console.WriteLine("=" * 80)
            Console.WriteLine()

            Directory.CreateDirectory(TestOutputPath)

            Dim results As New List(Of TestResult)

            ' Run all test methods
            results.Add(Test_BasicSerialization())
            results.Add(Test_BasicDeserialization())
            results.Add(Test_RoundTrip_NewFormat())
            results.Add(Test_RoundTrip_LegacyFormat())
            results.Add(Test_LegacyImport_Simple())
            results.Add(Test_LegacyImport_Nested())
            results.Add(Test_LegacyImport_Complex())
            results.Add(Test_FormatConversion())
            results.Add(Test_ExportToFile())
            results.Add(Test_ImportFromFile())
            results.Add(Test_NullTemplate())
            results.Add(Test_EmptyTemplate())
            results.Add(Test_MalformedJson())
            results.Add(Test_SchemaValidation_Valid())
            results.Add(Test_SchemaValidation_Invalid())
            results.Add(Test_CloneTemplate())

            ' Summary
            Dim passed = results.Where(Function(r) r.Passed).Count()
            Dim failed = results.Count - passed

            Console.WriteLine()
            Console.WriteLine("=" * 80)
            Console.ForegroundColor = If(failed = 0, ConsoleColor.Green, ConsoleColor.Yellow)
            Console.WriteLine($"Results: {passed}/{results.Count} passed")
            Console.ResetColor()
            Console.WriteLine("=" * 80)

            If failed > 0 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine()
                Console.WriteLine("Failed Tests:")
                For Each result In results.Where(Function(r) Not r.Passed)
                    Console.WriteLine($"  ? {result.TestName}: {result.ErrorMessage}")
                Next
                Console.ResetColor()
            End If
        End Sub

#End Region

#Region "Serialization Tests"

        Private Shared Function Test_BasicSerialization() As TestResult
            Dim result As New TestResult With {.TestName = "Basic Serialization"}

            Try
                Dim template As New TemplateDefinition With {
                    .Name = "Test Template",
                    .Description = "Test Description",
                    .Version = "1.0.0"
                }
                template.Folders.Add(New TemplateFolderDefinition With {.Name = "Folder1"})

                Dim serializer As New TemplateJsonSerializer()
                Dim json = serializer.SerializeTemplate(template)

                result.Passed = Not String.IsNullOrEmpty(json) AndAlso json.Contains("Test Template")
                If Not result.Passed Then
                    result.ErrorMessage = "Serialization produced empty or invalid JSON"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_BasicDeserialization() As TestResult
            Dim result As New TestResult With {.TestName = "Basic Deserialization"}

            Try
                Dim json = "{""Name"":""Test"",""Description"":""Desc"",""Folders"":[{""Name"":""F1""}]}"
                Dim serializer As New TemplateJsonSerializer()
                Dim template = serializer.DeserializeTemplate(json)

                result.Passed = template IsNot Nothing AndAlso
                               template.Name = "Test" AndAlso
                               template.Folders.Count = 1

                If Not result.Passed Then
                    result.ErrorMessage = "Deserialization failed or data incorrect"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Round-Trip Tests"

        Private Shared Function Test_RoundTrip_NewFormat() As TestResult
            Dim result As New TestResult With {.TestName = "Round-Trip: New Format"}

            Try
                Dim original As New TemplateDefinition With {
                    .Name = "RoundTrip Test",
                    .Description = "Testing round-trip",
                    .Version = "2.0.0",
                    .Category = "Testing",
                    .Tags = "test,roundtrip"
                }
                original.Folders.Add(New TemplateFolderDefinition With {
                    .Name = "Root",
                    .Description = "Root folder"
                })

                Dim serializer As New TemplateJsonSerializer()
                Dim json = serializer.SerializeTemplate(original)
                Dim restored = serializer.DeserializeTemplate(json)

                result.Passed = original.Name = restored.Name AndAlso
                               original.Description = restored.Description AndAlso
                               original.Version = restored.Version AndAlso
                               original.Folders.Count = restored.Folders.Count

                If Not result.Passed Then
                    result.ErrorMessage = "Round-trip data mismatch"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_RoundTrip_LegacyFormat() As TestResult
            Dim result As New TestResult With {.TestName = "Round-Trip: Legacy Format"}

            Try
                ' Legacy format JSON
                Dim legacyJson = "{""Name"":""Legacy"",""Description"":""Legacy template"",""Folders"":[{""Name"":""Old"",""IsSelected"":true,""SubFolders"":[]}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim imported = serializer.ImportLegacyTemplate(legacyJson)
                Dim newJson = serializer.SerializeTemplate(imported)
                Dim restored = serializer.DeserializeTemplate(newJson)

                result.Passed = imported.Name = restored.Name AndAlso
                               imported.Folders.Count = restored.Folders.Count

                If Not result.Passed Then
                    result.ErrorMessage = "Legacy round-trip data mismatch"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Legacy Import Tests"

        Private Shared Function Test_LegacyImport_Simple() As TestResult
            Dim result As New TestResult With {.TestName = "Legacy Import: Simple"}

            Try
                Dim legacyJson = "{""Name"":""Simple"",""Description"":""Simple template"",""Folders"":[{""Name"":""Folder1"",""IsSelected"":true,""SubFolders"":[]}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim imported = serializer.ImportLegacyTemplate(legacyJson)

                result.Passed = imported IsNot Nothing AndAlso
                               imported.Name = "Simple" AndAlso
                               imported.Folders.Count = 1 AndAlso
                               imported.Folders(0).Name = "Folder1"

                If Not result.Passed Then
                    result.ErrorMessage = "Legacy import failed or data incorrect"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_LegacyImport_Nested() As TestResult
            Dim result As New TestResult With {.TestName = "Legacy Import: Nested"}

            Try
                Dim legacyJson = "{""Name"":""Nested"",""Description"":""Nested template"",""Folders"":[{""Name"":""Parent"",""IsSelected"":true,""SubFolders"":[{""Name"":""Child"",""IsSelected"":true,""SubFolders"":[]}]}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim imported = serializer.ImportLegacyTemplate(legacyJson)

                result.Passed = imported IsNot Nothing AndAlso
                               imported.Folders.Count = 1 AndAlso
                               imported.Folders(0).SubFolders.Count = 1 AndAlso
                               imported.Folders(0).SubFolders(0).Name = "Child"

                If Not result.Passed Then
                    result.ErrorMessage = "Nested legacy import failed"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_LegacyImport_Complex() As TestResult
            Dim result As New TestResult With {.TestName = "Legacy Import: Complex"}

            Try
                Dim legacyJson = "{""Name"":""Complex"",""Description"":""Complex template"",""Folders"":[{""Name"":""F1"",""IsSelected"":true,""SubFolders"":[]},{""Name"":""F2"",""IsSelected"":false,""SubFolders"":[{""Name"":""F2.1"",""IsSelected"":true,""SubFolders"":[]}]},{""Name"":""F3"",""IsSelected"":true,""SubFolders"":[]}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim imported = serializer.ImportLegacyTemplate(legacyJson)

                result.Passed = imported IsNot Nothing AndAlso
                               imported.Folders.Count = 3 AndAlso
                               imported.Folders(1).SubFolders.Count = 1

                If Not result.Passed Then
                    result.ErrorMessage = "Complex legacy import failed"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Format Conversion Tests"

        Private Shared Function Test_FormatConversion() As TestResult
            Dim result As New TestResult With {.TestName = "Format Conversion Accuracy"}

            Try
                ' Legacy format
                Dim legacyJson = "{""Name"":""Convert"",""Description"":""Conversion test"",""Folders"":[{""Name"":""Root"",""IsSelected"":true,""SubFolders"":[{""Name"":""Sub"",""IsSelected"":false,""SubFolders"":[]}]}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim converted = serializer.ImportLegacyTemplate(legacyJson)

                ' Check new format fields added
                result.Passed = converted.Category = "Imported" AndAlso
                               converted.Version = "1.0.0" AndAlso
                               converted.IsActive = True AndAlso
                               converted.Folders(0).IsSelected = True AndAlso
                               converted.Folders(0).SubFolders(0).IsSelected = False

                If Not result.Passed Then
                    result.ErrorMessage = "Format conversion incomplete or incorrect"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "File I/O Tests"

        Private Shared Function Test_ExportToFile() As TestResult
            Dim result As New TestResult With {.TestName = "Export to File"}

            Try
                Dim template As New TemplateDefinition With {
                    .Name = "Export Test",
                    .Description = "Testing export"
                }
                template.Folders.Add(New TemplateFolderDefinition With {.Name = "ExportFolder"})

                Dim filePath = Path.Combine(TestOutputPath, "export-test.json")
                Dim serializer As New TemplateJsonSerializer()
                serializer.ExportTemplate(template, filePath)

                result.Passed = File.Exists(filePath) AndAlso
                               New FileInfo(filePath).Length > 0

                If Not result.Passed Then
                    result.ErrorMessage = "Export file not created or empty"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_ImportFromFile() As TestResult
            Dim result As New TestResult With {.TestName = "Import from File"}

            Try
                ' Create test file
                Dim filePath = Path.Combine(TestOutputPath, "import-test.json")
                Dim testJson = "{""Name"":""Import Test"",""Description"":""Testing import"",""Folders"":[{""Name"":""ImportFolder""}]}"
                File.WriteAllText(filePath, testJson)

                Dim serializer As New TemplateJsonSerializer()
                Dim imported = serializer.ImportTemplate(filePath)

                result.Passed = imported IsNot Nothing AndAlso
                               imported.Name = "Import Test" AndAlso
                               imported.Folders.Count = 1

                If Not result.Passed Then
                    result.ErrorMessage = "Import from file failed"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Edge Case Tests"

        Private Shared Function Test_NullTemplate() As TestResult
            Dim result As New TestResult With {.TestName = "Null Template Handling"}

            Try
                Dim serializer As New TemplateJsonSerializer()
                serializer.SerializeTemplate(Nothing)

                result.Passed = False
                result.ErrorMessage = "Should throw ArgumentNullException"

            Catch ex As ArgumentNullException
                result.Passed = True
            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = $"Wrong exception type: {ex.GetType().Name}"
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_EmptyTemplate() As TestResult
            Dim result As New TestResult With {.TestName = "Empty Template"}

            Try
                Dim template As New TemplateDefinition With {
                    .Name = "Empty",
                    .Description = ""
                }
                ' No folders added

                Dim serializer As New TemplateJsonSerializer()
                Dim json = serializer.SerializeTemplate(template)
                Dim restored = serializer.DeserializeTemplate(json)

                result.Passed = restored IsNot Nothing AndAlso
                               restored.Name = "Empty" AndAlso
                               restored.Folders.Count = 0

                If Not result.Passed Then
                    result.ErrorMessage = "Empty template handling failed"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_MalformedJson() As TestResult
            Dim result As New TestResult With {.TestName = "Malformed JSON Handling"}

            Try
                Dim badJson = "{""Name"":""Test"",""Description"":""Bad"",""Folders"":[{""Name"":""F1""}}"  ' Missing closing bracket

                Dim serializer As New TemplateJsonSerializer()
                serializer.DeserializeTemplate(badJson)

                result.Passed = False
                result.ErrorMessage = "Should throw exception for malformed JSON"

            Catch ex As InvalidOperationException
                result.Passed = True
            Catch ex As JsonException
                result.Passed = True
            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = $"Wrong exception type: {ex.GetType().Name}"
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Schema Validation Tests"

        Private Shared Function Test_SchemaValidation_Valid() As TestResult
            Dim result As New TestResult With {.TestName = "Schema Validation: Valid"}

            Try
                Dim validJson = "{""Name"":""Valid"",""Description"":""Valid template"",""Version"":""1.0.0"",""Folders"":[{""Name"":""F1""}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim validationResult = serializer.ValidateWithDetails(validJson)

                result.Passed = validationResult.IsValid

                If Not result.Passed Then
                    result.ErrorMessage = $"Valid JSON marked as invalid: {validationResult.GetErrorMessage()}"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

        Private Shared Function Test_SchemaValidation_Invalid() As TestResult
            Dim result As New TestResult With {.TestName = "Schema Validation: Invalid"}

            Try
                ' Missing required Name field
                Dim invalidJson = "{""Description"":""No name"",""Folders"":[{""Name"":""F1""}]}"

                Dim serializer As New TemplateJsonSerializer()
                Dim validationResult = serializer.ValidateWithDetails(invalidJson)

                result.Passed = Not validationResult.IsValid

                If Not result.Passed Then
                    result.ErrorMessage = "Invalid JSON marked as valid"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Utility Tests"

        Private Shared Function Test_CloneTemplate() As TestResult
            Dim result As New TestResult With {.TestName = "Clone Template"}

            Try
                Dim original As New TemplateDefinition With {
                    .Name = "Original",
                    .Description = "Original template"
                }
                original.Folders.Add(New TemplateFolderDefinition With {.Name = "Folder1"})

                Dim serializer As New TemplateJsonSerializer()
                Dim clone = serializer.CloneTemplate(original)

                result.Passed = clone IsNot Nothing AndAlso
                               clone IsNot original AndAlso
                               clone.Name = original.Name AndAlso
                               clone.Folders.Count = original.Folders.Count

                If Not result.Passed Then
                    result.ErrorMessage = "Clone failed or is same instance"
                End If

            Catch ex As Exception
                result.Passed = False
                result.ErrorMessage = ex.Message
            End Try

            PrintResult(result)
            Return result
        End Function

#End Region

#Region "Helper Methods"

        Private Shared Sub PrintResult(result As TestResult)
            Console.Write($"[{If(result.Passed, "?", "?")}] {result.TestName}")
            If Not result.Passed Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write($" - {result.ErrorMessage}")
                Console.ResetColor()
            End If
            Console.WriteLine()
        End Sub

        Private Class TestResult
            Public Property TestName As String
            Public Property Passed As Boolean
            Public Property ErrorMessage As String
        End Class

#End Region

    End Class

End Namespace
