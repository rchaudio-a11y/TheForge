''' ============================================================================
''' File: TemplateFolderDefinition.vb
''' Project: RCH.TemplateStorage
''' Purpose: Folder definition model for template folder hierarchy
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Extracted and enhanced from FolderDefinition class in TemplateBuilderControl.
'''   Represents a folder within a directory structure template with support for
'''   unlimited nesting through SubFolders property.
''' 
''' Extracted From:
'''   Source: NewDatabaseGenerator\NewDatabaseGenerator\TemplateBuilderControl.vb
'''   Original Class: FolderDefinition
'''   Extraction Date: 2026-01-05
''' 
''' Enhancements:
'''   - Added database-specific properties (FolderID, ParentFolderID)
'''   - Added metadata properties (Description, CreatedDate)
'''   - Preserved original properties for backward compatibility
'''   - Supports unlimited folder nesting
'''   - Added Forge metadata header
''' 
''' Backward Compatibility:
'''   - Maintains original Name, IsSelected, and SubFolders properties
'''   - Supports legacy JSON deserialization from TemplateBuilderControl format
'''   - All original functionality preserved
''' 
''' Dependencies:
'''   - Self-referencing for unlimited nesting (SubFolders property)
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System.Collections.Generic

Namespace Models

    ''' <summary>
    ''' Represents a folder within a template with support for unlimited nesting.
    ''' Extracted from FolderDefinition class in TemplateBuilderControl.
    ''' </summary>
    Public Class TemplateFolderDefinition

#Region "Database Properties (New)"

        ''' <summary>
        ''' Unique identifier for database persistence (AutoIncrement Primary Key)
        ''' </summary>
        Public Property FolderID As Integer

        ''' <summary>
        ''' Reference to parent folder ID (NULL for root-level folders)
        ''' Supports unlimited nesting through self-referencing
        ''' </summary>
        Public Property ParentFolderID As Integer?

        ''' <summary>
        ''' Reference to the template this folder belongs to
        ''' </summary>
        Public Property TemplateID As Integer

        ''' <summary>
        ''' Folder description or purpose
        ''' </summary>
        Public Property Description As String

        ''' <summary>
        ''' Date and time the folder definition was created
        ''' </summary>
        Public Property CreatedDate As DateTime = DateTime.Now

        ''' <summary>
        ''' Display order within parent folder (for consistent ordering)
        ''' </summary>
        Public Property DisplayOrder As Integer = 0

#End Region

#Region "Original Properties (Preserved for Backward Compatibility)"

        ''' <summary>
        ''' Folder name (original property from FolderDefinition)
        ''' </summary>
        Public Property Name As String

        ''' <summary>
        ''' Whether the folder is selected for inclusion when creating structure
        ''' (original property from FolderDefinition)
        ''' </summary>
        Public Property IsSelected As Boolean = True

        ''' <summary>
        ''' List of subfolders (original property from FolderDefinition)
        ''' Supports unlimited nesting - each subfolder can have its own SubFolders
        ''' </summary>
        Public Property SubFolders As New List(Of TemplateFolderDefinition)

        ''' <summary>
        ''' List of files within this folder (new for RCH.TemplateStorage)
        ''' </summary>
        Public Property Files As New List(Of TemplateFileDefinition)

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constructor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor with folder name (maintains backward compatibility)
        ''' </summary>
        ''' <param name="name">Folder name</param>
        Public Sub New(name As String)
            Me.Name = name
        End Sub

        ''' <summary>
        ''' Full constructor with all properties
        ''' </summary>
        Public Sub New(name As String, description As String, templateId As Integer)
            Me.Name = name
            Me.Description = description
            Me.TemplateID = templateId
        End Sub

#End Region

#Region "Methods (Original - Preserved for Backward Compatibility)"

        ''' <summary>
        ''' Adds a subfolder to this folder (original method from FolderDefinition)
        ''' Supports unlimited nesting - subfolders can have their own subfolders
        ''' </summary>
        ''' <param name="folderName">Name of the subfolder to add</param>
        ''' <returns>The created TemplateFolderDefinition</returns>
        Public Function AddSubFolder(folderName As String) As TemplateFolderDefinition
            Dim folder As New TemplateFolderDefinition(folderName)
            SubFolders.Add(folder)
            Return folder
        End Function

#End Region

#Region "Methods (New - Enhanced Functionality)"

        ''' <summary>
        ''' Adds a subfolder with full properties
        ''' </summary>
        Public Function AddSubFolder(folderName As String, description As String) As TemplateFolderDefinition
            Dim folder As New TemplateFolderDefinition(folderName) With {
                .Description = description,
                .TemplateID = Me.TemplateID,
                .ParentFolderID = Me.FolderID,
                .DisplayOrder = SubFolders.Count
            }
            SubFolders.Add(folder)
            Return folder
        End Function

        ''' <summary>
        ''' Adds a file to this folder
        ''' </summary>
        Public Function AddFile(fileName As String) As TemplateFileDefinition
            Dim file As New TemplateFileDefinition(fileName) With {
                .FolderID = Me.FolderID
            }
            Files.Add(file)
            Return file
        End Function

        ''' <summary>
        ''' Gets the depth level of this folder in the hierarchy (0 = root)
        ''' </summary>
        Public Function GetDepth() As Integer
            Dim depth = 0
            Dim current = Me

            While current.ParentFolderID.HasValue
                depth += 1
                ' Note: This requires parent folder reference, which would need to be set separately
                ' This is a placeholder for when folder hierarchy is fully loaded
                Exit While
            End While

            Return depth
        End Function

        ''' <summary>
        ''' Gets total count of subfolders (recursively)
        ''' </summary>
        Public Function GetTotalSubFolderCount() As Integer
            Dim count = SubFolders.Count

            For Each subFolder In SubFolders
                count += subFolder.GetTotalSubFolderCount()
            Next

            Return count
        End Function

        ''' <summary>
        ''' Gets total count of files in this folder and all subfolders (recursively)
        ''' </summary>
        Public Function GetTotalFileCount() As Integer
            Dim count = Files.Count

            For Each subFolder In SubFolders
                count += subFolder.GetTotalFileCount()
            Next

            Return count
        End Function

        ''' <summary>
        ''' Finds a subfolder by name (searches only immediate children)
        ''' </summary>
        Public Function FindSubFolder(folderName As String) As TemplateFolderDefinition
            Return SubFolders.FirstOrDefault(Function(f) f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase))
        End Function

        ''' <summary>
        ''' Finds a subfolder by name recursively (searches entire tree)
        ''' </summary>
        Public Function FindSubFolderRecursive(folderName As String) As TemplateFolderDefinition
            ' Check immediate children
            Dim found = FindSubFolder(folderName)
            If found IsNot Nothing Then Return found

            ' Search recursively in subfolders
            For Each subFolder In SubFolders
                found = subFolder.FindSubFolderRecursive(folderName)
                If found IsNot Nothing Then Return found
            Next

            Return Nothing
        End Function

#End Region

    End Class

End Namespace
