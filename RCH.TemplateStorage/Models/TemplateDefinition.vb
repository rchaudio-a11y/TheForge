''' ============================================================================
''' File: TemplateDefinition.vb
''' Project: RCH.TemplateStorage
''' Purpose: Template definition model for directory structure templates
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Extracted and enhanced from DirectoryTemplate class in TemplateBuilderControl.
'''   Represents a complete directory structure template with metadata for database
'''   persistence and backward compatibility with legacy JSON format.
''' 
''' Extracted From:
'''   Source: NewDatabaseGenerator\NewDatabaseGenerator\TemplateBuilderControl.vb
'''   Original Class: DirectoryTemplate
'''   Extraction Date: 2026-01-05
''' 
''' Enhancements:
'''   - Added database-specific properties (TemplateID, CreatedBy, ModifiedBy, etc.)
'''   - Added comprehensive metadata (Category, Tags, Dependencies, Notes)
'''   - Added usage tracking (UsageCount, LastUsedDate)
'''   - Preserved original properties for backward compatibility
'''   - Added Forge metadata header
''' 
''' Backward Compatibility:
'''   - Maintains original Name, Description, and Folders properties
'''   - Supports legacy JSON deserialization from TemplateBuilderControl format
'''   - All original functionality preserved
''' 
''' Dependencies:
'''   - TemplateFolderDefinition (previously FolderDefinition)
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System.Collections.Generic

Namespace Models

    ''' <summary>
    ''' Represents a directory structure template with comprehensive metadata.
    ''' Extracted from DirectoryTemplate class in TemplateBuilderControl.
    ''' </summary>
    Public Class TemplateDefinition

#Region "Database Properties (New)"

        ''' <summary>
        ''' Unique identifier for database persistence (AutoIncrement Primary Key)
        ''' </summary>
        Public Property TemplateID As Integer

        ''' <summary>
        ''' User who created the template
        ''' </summary>
        Public Property CreatedBy As String

        ''' <summary>
        ''' Date and time the template was created
        ''' </summary>
        Public Property CreatedDate As DateTime = DateTime.Now

        ''' <summary>
        ''' User who last modified the template
        ''' </summary>
        Public Property ModifiedBy As String

        ''' <summary>
        ''' Date and time the template was last modified
        ''' </summary>
        Public Property ModifiedDate As DateTime = DateTime.Now

        ''' <summary>
        ''' Template category for organization (e.g., "Programming", "Engineering", "Software")
        ''' </summary>
        Public Property Category As String

        ''' <summary>
        ''' Comma-separated tags for searching and filtering
        ''' </summary>
        Public Property Tags As String

        ''' <summary>
        ''' Other templates this template depends on (JSON array of TemplateIDs)
        ''' </summary>
        Public Property Dependencies As String

        ''' <summary>
        ''' Additional notes or instructions for using the template
        ''' </summary>
        Public Property Notes As String

        ''' <summary>
        ''' Number of times this template has been used to create structures
        ''' </summary>
        Public Property UsageCount As Integer = 0

        ''' <summary>
        ''' Date and time the template was last used
        ''' </summary>
        Public Property LastUsedDate As DateTime?

        ''' <summary>
        ''' Whether the template is active and available for use
        ''' </summary>
        Public Property IsActive As Boolean = True

        ''' <summary>
        ''' Version number for template versioning (e.g., "1.0.0")
        ''' </summary>
        Public Property Version As String = "1.0.0"

#End Region

#Region "Original Properties (Preserved for Backward Compatibility)"

        ''' <summary>
        ''' Template name (original property from DirectoryTemplate)
        ''' </summary>
        Public Property Name As String

        ''' <summary>
        ''' Template description (original property from DirectoryTemplate)
        ''' </summary>
        Public Property Description As String

        ''' <summary>
        ''' List of root-level folders in the template (original property from DirectoryTemplate)
        ''' Supports unlimited nesting through TemplateFolderDefinition.SubFolders
        ''' </summary>
        Public Property Folders As New List(Of TemplateFolderDefinition)

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constructor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor with name and description (maintains backward compatibility)
        ''' </summary>
        ''' <param name="name">Template name</param>
        ''' <param name="description">Template description</param>
        Public Sub New(name As String, description As String)
            Me.Name = name
            Me.Description = description
        End Sub

        ''' <summary>
        ''' Full constructor with all database fields
        ''' </summary>
        Public Sub New(name As String, description As String, category As String, createdBy As String)
            Me.Name = name
            Me.Description = description
            Me.Category = category
            Me.CreatedBy = createdBy
            Me.ModifiedBy = createdBy
        End Sub

#End Region

#Region "Methods (Original - Preserved for Backward Compatibility)"

        ''' <summary>
        ''' Adds a root-level folder to the template (original method from DirectoryTemplate)
        ''' </summary>
        ''' <param name="folderName">Name of the folder to add</param>
        ''' <returns>The created TemplateFolderDefinition</returns>
        Public Function AddFolder(folderName As String) As TemplateFolderDefinition
            Dim folder As New TemplateFolderDefinition(folderName)
            Folders.Add(folder)
            Return folder
        End Function

#End Region

#Region "Methods (New - Database Support)"

        ''' <summary>
        ''' Updates the ModifiedDate and ModifiedBy properties
        ''' </summary>
        ''' <param name="modifiedBy">User making the modification</param>
        Public Sub MarkAsModified(modifiedBy As String)
            Me.ModifiedBy = modifiedBy
            Me.ModifiedDate = DateTime.Now
        End Sub

        ''' <summary>
        ''' Increments the usage count and updates LastUsedDate
        ''' </summary>
        Public Sub IncrementUsage()
            Me.UsageCount += 1
            Me.LastUsedDate = DateTime.Now
        End Sub

        ''' <summary>
        ''' Gets a count of all folders (root + nested) in the template
        ''' </summary>
        ''' <returns>Total folder count</returns>
        Public Function GetTotalFolderCount() As Integer
            Dim count = Folders.Count

            For Each folder In Folders
                count += GetSubFolderCountRecursive(folder)
            Next

            Return count
        End Function

        ''' <summary>
        ''' Recursively counts subfolders
        ''' </summary>
        Private Function GetSubFolderCountRecursive(folder As TemplateFolderDefinition) As Integer
            Dim count = 0

            If folder.SubFolders IsNot Nothing Then
                count = folder.SubFolders.Count

                For Each subFolder In folder.SubFolders
                    count += GetSubFolderCountRecursive(subFolder)
                Next
            End If

            Return count
        End Function

#End Region

    End Class

End Namespace
