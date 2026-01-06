''' ============================================================================
''' File: ITemplateStorageService.vb
''' Project: RCH.TemplateStorage
''' Purpose: Service interface for template CRUD operations
''' Created: 2026-01-05
''' Author: TheForge
''' Status: Active
''' Version: 1.0.0
''' 
''' Description:
'''   Defines the contract for template storage service operations including
'''   CRUD (Create, Read, Update, Delete), search, and JSON import/export.
'''   Provides high-level interface for working with templates, folders, and
'''   files without direct database manipulation.
''' 
''' Operations:
'''   - Template CRUD: Create, Get, Update, Delete templates
'''   - Search: By name, tag, category
'''   - JSON: Import/export with legacy support
'''   - Bulk: Get all, get active templates
''' 
''' Character Count: [Computed by ForgeAudit]
''' Last Updated: 2026-01-05
''' ============================================================================

Imports System
Imports System.Collections.Generic
Imports RCH.TemplateStorage.Models

Namespace Services.Interfaces

    ''' <summary>
    ''' Service interface for template storage operations
    ''' </summary>
    Public Interface ITemplateStorageService
        Inherits IDisposable

#Region "Template CRUD Operations"

        ''' <summary>
        ''' Creates a new template in the database
        ''' </summary>
        ''' <param name="template">Template to create</param>
        ''' <returns>Created template with assigned TemplateID</returns>
        Function CreateTemplate(template As TemplateDefinition) As TemplateDefinition

        ''' <summary>
        ''' Gets a template by ID with all folders and files
        ''' </summary>
        ''' <param name="templateId">Template ID</param>
        ''' <returns>Template with complete hierarchy, or Nothing if not found</returns>
        Function GetTemplate(templateId As Integer) As TemplateDefinition

        ''' <summary>
        ''' Gets all templates (without folder/file details)
        ''' </summary>
        ''' <returns>List of templates</returns>
        Function GetAllTemplates() As List(Of TemplateDefinition)

        ''' <summary>
        ''' Updates an existing template
        ''' </summary>
        ''' <param name="template">Template with updated data</param>
        ''' <returns>True if successful</returns>
        Function UpdateTemplate(template As TemplateDefinition) As Boolean

        ''' <summary>
        ''' Deletes a template by ID (cascade deletes folders and files)
        ''' </summary>
        ''' <param name="templateId">Template ID to delete</param>
        ''' <returns>True if successful</returns>
        Function DeleteTemplate(templateId As Integer) As Boolean

#End Region

#Region "Search Operations"

        ''' <summary>
        ''' Gets a template by exact name
        ''' </summary>
        ''' <param name="name">Template name</param>
        ''' <returns>Template or Nothing if not found</returns>
        Function GetTemplateByName(name As String) As TemplateDefinition

        ''' <summary>
        ''' Searches templates by keyword in name, description, or tags
        ''' </summary>
        ''' <param name="keyword">Search keyword</param>
        ''' <returns>List of matching templates</returns>
        Function SearchTemplates(keyword As String) As List(Of TemplateDefinition)

        ''' <summary>
        ''' Gets templates by category
        ''' </summary>
        ''' <param name="category">Category name</param>
        ''' <returns>List of templates in category</returns>
        Function GetTemplatesByCategory(category As String) As List(Of TemplateDefinition)

        ''' <summary>
        ''' Gets templates by tag
        ''' </summary>
        ''' <param name="tag">Tag to search for</param>
        ''' <returns>List of templates with tag</returns>
        Function GetTemplatesByTag(tag As String) As List(Of TemplateDefinition)

        ''' <summary>
        ''' Gets only active templates
        ''' </summary>
        ''' <returns>List of active templates</returns>
        Function GetActiveTemplates() As List(Of TemplateDefinition)

#End Region

#Region "JSON Import/Export"

        ''' <summary>
        ''' Exports template to JSON file
        ''' </summary>
        ''' <param name="templateId">Template ID to export</param>
        ''' <param name="filePath">Output file path</param>
        ''' <returns>True if successful</returns>
        Function ExportTemplateToJson(templateId As Integer, filePath As String) As Boolean

        ''' <summary>
        ''' Imports template from JSON file (supports legacy format)
        ''' </summary>
        ''' <param name="filePath">JSON file path</param>
        ''' <returns>Imported template with assigned ID</returns>
        Function ImportTemplateFromJson(filePath As String) As TemplateDefinition

        ''' <summary>
        ''' Imports legacy TemplateBuilderControl JSON and saves to database
        ''' </summary>
        ''' <param name="filePath">Legacy JSON file path</param>
        ''' <returns>Imported and saved template</returns>
        Function ImportLegacyTemplate(filePath As String) As TemplateDefinition

#End Region

#Region "Utility Operations"

        ''' <summary>
        ''' Checks if a template with given name exists
        ''' </summary>
        ''' <param name="name">Template name</param>
        ''' <returns>True if exists</returns>
        Function TemplateExists(name As String) As Boolean

        ''' <summary>
        ''' Gets count of all templates
        ''' </summary>
        ''' <returns>Template count</returns>
        Function GetTemplateCount() As Integer

        ''' <summary>
        ''' Increments usage count for a template
        ''' </summary>
        ''' <param name="templateId">Template ID</param>
        ''' <returns>True if successful</returns>
        Function IncrementUsage(templateId As Integer) As Boolean

        ''' <summary>
        ''' Gets all unique categories
        ''' </summary>
        ''' <returns>List of category names</returns>
        Function GetAllCategories() As List(Of String)

        ''' <summary>
        ''' Gets all unique tags
        ''' </summary>
        ''' <returns>List of tag names</returns>
        Function GetAllTags() As List(Of String)

#End Region

#Region "Database Management"

        ''' <summary>
        ''' Gets the database connection string
        ''' </summary>
        ReadOnly Property ConnectionString As String

        ''' <summary>
        ''' Gets the database file path
        ''' </summary>
        ReadOnly Property DatabasePath As String

        ''' <summary>
        ''' Tests the database connection
        ''' </summary>
        ''' <returns>True if connection successful</returns>
        Function TestConnection() As Boolean

        ''' <summary>
        ''' Validates database schema
        ''' </summary>
        ''' <returns>True if schema valid</returns>
        Function ValidateDatabase() As Boolean

#End Region

    End Interface

End Namespace
