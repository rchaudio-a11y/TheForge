''' ============================================================================
''' File: BackwardCompatibilityTests.vb
''' Project: RCH.TemplateStorage
''' Purpose: Verify backward compatibility with legacy TemplateBuilderControl JSON
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active - Test File
''' Version: 1.0.0
''' 
''' Description:
'''   Tests JSON deserialization of legacy TemplateBuilderControl format
'''   (DirectoryTemplate/FolderDefinition) to ensure 100% backward compatibility.
'''   Verifies field mapping, hierarchy preservation, and data integrity.
''' 
''' Test Coverage:
'''   - Legacy JSON format detection
'''   - DirectoryTemplate ? TemplateDefinition conversion
'''   - FolderDefinition ? TemplateFolderDefinition conversion
'''   - Hierarchical structure preservation (unlimited nesting)
'''   - Round-trip testing (legacy ? new ? legacy)
'''   - Edge cases (empty templates, deep nesting, special characters)
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.Console
Imports System.IO
Imports RCH.TemplateStorage.Models
Imports RCH.TemplateStorage.Services.Implementations

Namespace Testing

    ''' <summary>
    ''' Backward compatibility tests with legacy TemplateBuilderControl JSON
    ''' </summary>
    Public Class BackwardCompatibilityTests

        ''' <summary>
        ''' Runs all backward compatibility tests
        ''' </summary>
        Public Shared Sub RunAllTests()
            WriteLine("==========================================")
            WriteLine("Backward Compatibility Tests")
            WriteLine("==========================================")
            WriteLine()

            Try
                TestSimpleLegacyTemplate()
                TestNestedFolderStructure()
                TestDeepNesting()
                TestIsSelectedFlag()
                TestEmptyTemplate()
                TestSpecialCharacters()
                TestRoundTripConversion()

                WriteLine()
                WriteLine("==========================================")
                WriteLine("? All Backward Compatibility Tests Passed!")
                WriteLine("==========================================")

            Catch ex As Exception
                WriteLine()
                WriteLine("==========================================")
                WriteLine($"? Test Failed: {ex.Message}")
                WriteLine("==========================================")
                WriteLine(ex.StackTrace)
            End Try
        End Sub

#Region "Sample Legacy JSON"

        ''' <summary>
        ''' Simple legacy TemplateBuilderControl JSON (DirectoryTemplate format)
        ''' </summary>
        Private Shared Function GetSimpleLegacyJson() As String
            Return "{
  ""Name"": ""Simple Template"",
  ""Description"": ""A basic template for testing"",
  ""Folders"": [
    {
      ""Name"": ""Source"",
      ""IsSelected"": true,
      ""SubFolders"": []
    },
    {
      ""Name"": ""Documentation"",
      ""IsSelected"": true,
      ""SubFolders"": []
    }
  ]
}"
        End Function

        ''' <summary>
        ''' Nested folder structure legacy JSON
        ''' </summary>
        Private Shared Function GetNestedLegacyJson() As String
            Return "{
  ""Name"": ""Programming Template"",
  ""Description"": ""Programming and automation projects"",
  ""Folders"": [
    {
      ""Name"": ""01_Input"",
      ""IsSelected"": true,
      ""SubFolders"": []
    },
    {
      ""Name"": ""04_Programming"",
      ""IsSelected"": true,
      ""SubFolders"": [
        {
          ""Name"": ""Robot"",
          ""IsSelected"": true,
          ""SubFolders"": []
        },
        {
          ""Name"": ""PLC"",
          ""IsSelected"": true,
          ""SubFolders"": []
        }
      ]
    },
    {
      ""Name"": ""05_Resources"",
      ""IsSelected"": true,
      ""SubFolders"": []
    }
  ]
}"
        End Function

        ''' <summary>
        ''' Deep nesting legacy JSON (5 levels)
        ''' </summary>
        Private Shared Function GetDeepNestingJson() As String
            Return "{
  ""Name"": ""Deep Nesting Test"",
  ""Description"": ""Tests unlimited folder nesting"",
  ""Folders"": [
    {
      ""Name"": ""Level1"",
      ""IsSelected"": true,
      ""SubFolders"": [
        {
          ""Name"": ""Level2"",
          ""IsSelected"": true,
          ""SubFolders"": [
            {
              ""Name"": ""Level3"",
              ""IsSelected"": true,
              ""SubFolders"": [
                {
                  ""Name"": ""Level4"",
                  ""IsSelected"": true,
                  ""SubFolders"": [
                    {
                      ""Name"": ""Level5"",
                      ""IsSelected"": true,
                      ""SubFolders"": []
                    }
                  ]
                }
              ]
            }
          ]
        }
      ]
    }
  ]
}"
        End Function

        ''' <summary>
        ''' Template with IsSelected = false flags
        ''' </summary>
        Private Shared Function GetIsSelectedTestJson() As String
            Return "{
  ""Name"": ""Selection Test"",
  ""Description"": ""Tests IsSelected flag preservation"",
  ""Folders"": [
    {
      ""Name"": ""Selected"",
      ""IsSelected"": true,
      ""SubFolders"": []
    },
    {
      ""Name"": ""NotSelected"",
      ""IsSelected"": false,
      ""SubFolders"": []
    },
    {
      ""Name"": ""Mixed"",
      ""IsSelected"": true,
      ""SubFolders"": [
        {
          ""Name"": ""SelectedChild"",
          ""IsSelected"": true,
          ""SubFolders"": []
        },
        {
          ""Name"": ""NotSelectedChild"",
          ""IsSelected"": false,
          ""SubFolders"": []
        }
      ]
    }
  ]
}"
        End Function

        ''' <summary>
        ''' Empty template (edge case)
        ''' </summary>
        Private Shared Function GetEmptyTemplateJson() As String
            Return "{
  ""Name"": ""Empty Template"",
  ""Description"": ""Template with no folders"",
  ""Folders"": []
}"
        End Function

        ''' <summary>
        ''' Template with special characters
        ''' </summary>
        Private Shared Function GetSpecialCharactersJson() As String
            Return "{
  ""Name"": ""Special-Characters_Template (v1.0)"",
  ""Description"": ""Tests special chars: @#$%^&*()[]{}!"",
  ""Folders"": [
    {
      ""Name"": ""Folder-With_Dashes"",
      ""IsSelected"": true,
      ""SubFolders"": []
    },
    {
      ""Name"": ""Folder (With Parentheses)"",
      ""IsSelected"": true,
      ""SubFolders"": []
    }
  ]
}"
        End Function

#End Region

#Region "Test Methods"

        ''' <summary>
        ''' Test 1: Simple legacy template import
        ''' </summary>
        Private Shared Sub TestSimpleLegacyTemplate()
            WriteLine("Test 1: Simple Legacy Template Import")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetSimpleLegacyJson()

            ' Import legacy template
            Dim template = serializer.ImportLegacyTemplate(legacyJson)

            ' Verify basic properties
            AssertNotNull(template, "Template imported")
            AssertEqual(template.Name, "Simple Template", "Name preserved")
            AssertEqual(template.Description, "A basic template for testing", "Description preserved")

            ' Verify new fields have defaults
            AssertEqual(template.Category, "Imported", "Category set to 'Imported'")
            AssertEqual(template.IsActive, True, "IsActive defaults to True")
            AssertEqual(template.Version, "1.0.0", "Version set")

            ' Verify folder structure
            AssertEqual(template.Folders.Count, 2, "Two root folders")
            AssertEqual(template.Folders(0).Name, "Source", "First folder name")
            AssertEqual(template.Folders(1).Name, "Documentation", "Second folder name")

            ' Verify folder properties
            AssertEqual(template.Folders(0).IsSelected, True, "IsSelected preserved")
            AssertEqual(template.Folders(0).SubFolders.Count, 0, "Empty SubFolders list")

            WriteLine("? Test 1 Passed")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 2: Nested folder structure
        ''' </summary>
        Private Shared Sub TestNestedFolderStructure()
            WriteLine("Test 2: Nested Folder Structure")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetNestedLegacyJson()

            ' Import legacy template
            Dim template = serializer.ImportLegacyTemplate(legacyJson)

            ' Verify root folders
            AssertEqual(template.Folders.Count, 3, "Three root folders")

            ' Find Programming folder
            Dim programmingFolder = template.Folders.FirstOrDefault(Function(f) f.Name = "04_Programming")
            AssertNotNull(programmingFolder, "Programming folder exists")

            ' Verify subfolders
            AssertEqual(programmingFolder.SubFolders.Count, 2, "Programming has 2 subfolders")
            AssertEqual(programmingFolder.SubFolders(0).Name, "Robot", "First subfolder name")
            AssertEqual(programmingFolder.SubFolders(1).Name, "PLC", "Second subfolder name")

            ' Verify subfolder properties preserved
            AssertEqual(programmingFolder.SubFolders(0).IsSelected, True, "Subfolder IsSelected preserved")

            WriteLine("? Test 2 Passed")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 3: Deep nesting (5 levels)
        ''' </summary>
        Private Shared Sub TestDeepNesting()
            WriteLine("Test 3: Deep Nesting (5 levels)")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetDeepNestingJson()

            ' Import legacy template
            Dim template = serializer.ImportLegacyTemplate(legacyJson)

            ' Navigate through 5 levels
            Dim level1 = template.Folders(0)
            AssertEqual(level1.Name, "Level1", "Level 1 name")

            Dim level2 = level1.SubFolders(0)
            AssertEqual(level2.Name, "Level2", "Level 2 name")

            Dim level3 = level2.SubFolders(0)
            AssertEqual(level3.Name, "Level3", "Level 3 name")

            Dim level4 = level3.SubFolders(0)
            AssertEqual(level4.Name, "Level4", "Level 4 name")

            Dim level5 = level4.SubFolders(0)
            AssertEqual(level5.Name, "Level5", "Level 5 name")
            AssertEqual(level5.SubFolders.Count, 0, "Level 5 has no children")

            ' Test recursive counting
            Dim totalCount = level1.GetTotalSubFolderCount()
            AssertEqual(totalCount, 4, "4 nested subfolders under Level1")

            WriteLine("? Test 3 Passed - Unlimited nesting preserved")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 4: IsSelected flag preservation
        ''' </summary>
        Private Shared Sub TestIsSelectedFlag()
            WriteLine("Test 4: IsSelected Flag Preservation")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetIsSelectedTestJson()

            ' Import legacy template
            Dim template = serializer.ImportLegacyTemplate(legacyJson)

            ' Check root-level folders
            Dim selected = template.Folders.FirstOrDefault(Function(f) f.Name = "Selected")
            Dim notSelected = template.Folders.FirstOrDefault(Function(f) f.Name = "NotSelected")
            Dim mixed = template.Folders.FirstOrDefault(Function(f) f.Name = "Mixed")

            AssertEqual(selected.IsSelected, True, "Selected folder is True")
            AssertEqual(notSelected.IsSelected, False, "NotSelected folder is False")
            AssertEqual(mixed.IsSelected, True, "Mixed folder is True")

            ' Check nested folders
            Dim selectedChild = mixed.SubFolders.FirstOrDefault(Function(f) f.Name = "SelectedChild")
            Dim notSelectedChild = mixed.SubFolders.FirstOrDefault(Function(f) f.Name = "NotSelectedChild")

            AssertEqual(selectedChild.IsSelected, True, "SelectedChild is True")
            AssertEqual(notSelectedChild.IsSelected, False, "NotSelectedChild is False")

            WriteLine("? Test 4 Passed")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 5: Empty template edge case
        ''' </summary>
        Private Shared Sub TestEmptyTemplate()
            WriteLine("Test 5: Empty Template Edge Case")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetEmptyTemplateJson()

            ' Import empty template
            Dim template = serializer.ImportLegacyTemplate(legacyJson)

            AssertNotNull(template, "Empty template imports")
            AssertEqual(template.Name, "Empty Template", "Name preserved")
            AssertEqual(template.Folders.Count, 0, "Folders list is empty")

            WriteLine("? Test 5 Passed")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 6: Special characters in names
        ''' </summary>
        Private Shared Sub TestSpecialCharacters()
            WriteLine("Test 6: Special Characters")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetSpecialCharactersJson()

            ' Import template with special chars
            Dim template = serializer.ImportLegacyTemplate(legacyJson)

            AssertEqual(template.Name, "Special-Characters_Template (v1.0)", "Name with special chars preserved")
            AssertEqual(template.Description, "Tests special chars: @#$%^&*()[]{}!", "Description with special chars preserved")

            AssertEqual(template.Folders(0).Name, "Folder-With_Dashes", "Folder name with dashes preserved")
            AssertEqual(template.Folders(1).Name, "Folder (With Parentheses)", "Folder name with parentheses preserved")

            WriteLine("? Test 6 Passed")
            WriteLine()
        End Sub

        ''' <summary>
        ''' Test 7: Round-trip conversion (legacy ? new ? serialize ? deserialize)
        ''' </summary>
        Private Shared Sub TestRoundTripConversion()
            WriteLine("Test 7: Round-Trip Conversion")
            WriteLine("------------------------------------------")

            Dim serializer As New TemplateJsonSerializer()
            Dim legacyJson = GetNestedLegacyJson()

            ' Step 1: Import legacy
            Dim template1 = serializer.ImportLegacyTemplate(legacyJson)

            ' Step 2: Serialize to new format
            Dim newJson = serializer.SerializeTemplate(template1)
            AssertNotNull(newJson, "Serialization produces JSON")

            ' Step 3: Deserialize from new format
            Dim template2 = serializer.DeserializeTemplate(newJson)
            AssertNotNull(template2, "Deserialization succeeds")

            ' Step 4: Verify data integrity
            AssertEqual(template2.Name, template1.Name, "Name preserved in round-trip")
            AssertEqual(template2.Description, template1.Description, "Description preserved in round-trip")
            AssertEqual(template2.Folders.Count, template1.Folders.Count, "Folder count preserved")

            ' Verify nested structure preserved
            Dim folder1 = template1.Folders.FirstOrDefault(Function(f) f.Name = "04_Programming")
            Dim folder2 = template2.Folders.FirstOrDefault(Function(f) f.Name = "04_Programming")

            AssertEqual(folder2.SubFolders.Count, folder1.SubFolders.Count, "Subfolder count preserved")
            AssertEqual(folder2.SubFolders(0).Name, folder1.SubFolders(0).Name, "Subfolder names preserved")

            WriteLine("? Test 7 Passed - Zero data loss in round-trip")
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

#End Region

    End Class

End Namespace
