Imports System
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json.Schema
Imports RCH.TemplateStorage.Models

Namespace Services.Implementations

    ''' <summary>
    ''' Serializes and deserializes template models with backward compatibility
    ''' </summary>
    Public Class TemplateJsonSerializer

#Region "Private Fields"

        Private ReadOnly _settings As JsonSerializerSettings
        Private ReadOnly _schema As JSchema

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constructor with standard JSON settings and schema validation
        ''' </summary>
        Public Sub New()
            _settings = New JsonSerializerSettings() With {
                .Formatting = Formatting.Indented,
                .NullValueHandling = NullValueHandling.Ignore,
                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                .PreserveReferencesHandling = PreserveReferencesHandling.None,
                .DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                .DefaultValueHandling = DefaultValueHandling.Include
            }
            
            ' Load JSON schema
            Try
                Dim schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatabaseSchema", "TemplateDefinition.schema.json")
                If File.Exists(schemaPath) Then
                    Dim schemaJson = File.ReadAllText(schemaPath)
                    _schema = JSchema.Parse(schemaJson)
                End If
            Catch ex As Exception
                ' Schema loading is optional - validation will be skipped if not available
                _schema = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' Constructor with custom serialization settings
        ''' </summary>
        Public Sub New(settings As JsonSerializerSettings)
            _settings = settings
        End Sub

#End Region

#Region "Serialization Methods"

        ''' <summary>
        ''' Serializes a TemplateDefinition to JSON string
        ''' </summary>
        ''' <param name="template">The template to serialize</param>
        ''' <returns>JSON string representation</returns>
        Public Function SerializeTemplate(template As TemplateDefinition) As String
            If template Is Nothing Then
                Throw New ArgumentNullException(NameOf(template))
            End If

            Try
                Return JsonConvert.SerializeObject(template, _settings)
            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to serialize template '{template.Name}': {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Serializes a TemplateDefinition to JSON and saves to file
        ''' </summary>
        ''' <param name="template">The template to serialize</param>
        ''' <param name="filePath">Target file path</param>
        Public Sub SerializeToFile(template As TemplateDefinition, filePath As String)
            If template Is Nothing Then
                Throw New ArgumentNullException(NameOf(template))
            End If

            If String.IsNullOrWhiteSpace(filePath) Then
                Throw New ArgumentNullException(NameOf(filePath))
            End If

            Try
                Dim json = SerializeTemplate(template)
                File.WriteAllText(filePath, json)
            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to serialize template to file '{filePath}': {ex.Message}", ex)
            End Try
        End Sub

#End Region

#Region "Deserialization Methods"

        ''' <summary>
        ''' Deserializes JSON string to TemplateDefinition (new format)
        ''' </summary>
        ''' <param name="json">JSON string</param>
        ''' <returns>Deserialized template</returns>
        Public Function DeserializeTemplate(json As String) As TemplateDefinition
            If String.IsNullOrWhiteSpace(json) Then
                Throw New ArgumentNullException(NameOf(json))
            End If

            Try
                ' Validate against schema (if available)
                If _schema IsNot Nothing Then
                    Dim validationResult = ValidateWithDetails(json)
                    If Not validationResult.IsValid Then
                        ' Log validation errors but continue with lenient deserialization
                        Console.WriteLine($"[WARN] JSON schema validation failed: {validationResult.GetErrorMessage()}")
                        ' Allow deserialization to proceed for backward compatibility
                    End If
                End If
                
                ' Detect format
                Dim format = DetectJsonFormat(json)

                Select Case format
                    Case JsonFormat.Legacy
                        ' Legacy TemplateBuilderControl format - convert
                        Return ImportLegacyTemplate(json)

                    Case JsonFormat.New
                        ' New RCH.TemplateStorage format - direct deserialize
                        Return JsonConvert.DeserializeObject(Of TemplateDefinition)(json, _settings)

                    Case Else
                        Throw New InvalidOperationException("Unknown JSON format")
                End Select

            Catch ex As JsonException
                Throw New InvalidOperationException($"Failed to deserialize template: {ex.Message}", ex)
            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to deserialize template: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Deserializes JSON from file to TemplateDefinition
        ''' </summary>
        ''' <param name="filePath">Source file path</param>
        ''' <returns>Deserialized template</returns>
        Public Function DeserializeFromFile(filePath As String) As TemplateDefinition
            If String.IsNullOrWhiteSpace(filePath) Then
                Throw New ArgumentNullException(NameOf(filePath))
            End If

            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException($"Template file not found: {filePath}")
            End If

            Try
                Dim json = File.ReadAllText(filePath)
                Return DeserializeTemplate(json)
            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to deserialize template from file '{filePath}': {ex.Message}", ex)
            End Try
        End Function

#End Region

#Region "Legacy Migration Methods"

        ''' <summary>
        ''' Imports a legacy TemplateBuilderControl JSON template and converts to new format
        ''' </summary>
        ''' <param name="legacyJson">Legacy JSON string</param>
        ''' <returns>Converted TemplateDefinition</returns>
        Public Function ImportLegacyTemplate(legacyJson As String) As TemplateDefinition
            If String.IsNullOrWhiteSpace(legacyJson) Then
                Throw New ArgumentNullException(NameOf(legacyJson))
            End If

            Try
                ' Parse as JObject for flexible property access
                Dim jObject As JObject = JObject.Parse(legacyJson)

                ' Create new template definition
                Dim template As New TemplateDefinition() With {
                    .Name = If(jObject("Name")?.ToString(), "Imported Template"),
                    .Description = If(jObject("Description")?.ToString(), String.Empty),
                    .Category = "Imported",
                    .Version = "1.0.0",
                    .CreatedDate = DateTime.Now,
                    .ModifiedDate = DateTime.Now,
                    .IsActive = True
                }

                ' Import folders
                Dim foldersArray = TryCast(jObject("Folders"), JArray)
                If foldersArray IsNot Nothing Then
                    For Each folderToken In foldersArray
                        Dim folder = ImportLegacyFolder(CType(folderToken, JObject))
                        If folder IsNot Nothing Then
                            template.Folders.Add(folder)
                        End If
                    Next
                End If

                Return template

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to import legacy template: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' Recursively imports a legacy FolderDefinition and converts to TemplateFolderDefinition
        ''' </summary>
        Private Function ImportLegacyFolder(folderJson As JObject) As TemplateFolderDefinition
            If folderJson Is Nothing Then Return Nothing

            Try
                Dim folder As New TemplateFolderDefinition() With {
                    .Name = If(folderJson("Name")?.ToString(), "Unnamed Folder"),
                    .IsSelected = If(folderJson("IsSelected")?.ToObject(Of Boolean?)(), True),
                    .Description = String.Empty,
                    .CreatedDate = DateTime.Now
                }

                ' Import subfolders recursively (supports unlimited nesting)
                Dim subFoldersArray = TryCast(folderJson("SubFolders"), JArray)
                If subFoldersArray IsNot Nothing Then
                    For Each subFolderToken In subFoldersArray
                        Dim subFolder = ImportLegacyFolder(CType(subFolderToken, JObject))
                        If subFolder IsNot Nothing Then
                            folder.SubFolders.Add(subFolder)
                        End If
                    Next
                End If

                Return folder

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to import legacy folder: {ex.Message}", ex)
            End Try
        End Function

#End Region

#Region "Format Detection"

        ''' <summary>
        ''' Detects whether JSON is in legacy or new format
        ''' </summary>
        Private Function DetectJsonFormat(json As String) As JsonFormat
            Try
                Dim jObject As JObject = JObject.Parse(json)

                ' Check for new format indicators
                If jObject("TemplateID") IsNot Nothing OrElse
                   jObject("CreatedBy") IsNot Nothing OrElse
                   jObject("Category") IsNot Nothing OrElse
                   jObject("Tags") IsNot Nothing Then
                    Return JsonFormat.New
                End If

                ' Check for legacy format indicators
                If jObject("Name") IsNot Nothing AndAlso
                   jObject("Folders") IsNot Nothing Then
                    Return JsonFormat.Legacy
                End If

                ' Default to legacy for compatibility
                Return JsonFormat.Legacy

            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to detect JSON format: {ex.Message}", ex)
            End Try
        End Function

        ''' <summary>
        ''' JSON format enumeration
        ''' </summary>
        Private Enum JsonFormat
            Legacy      ' TemplateBuilderControl DirectoryTemplate format
            [New]       ' RCH.TemplateStorage TemplateDefinition format
        End Enum

#End Region

#Region "Validation Methods"

        ''' <summary>
        ''' Validates JSON string syntax
        ''' </summary>
        ''' <param name="json">JSON string to validate</param>
        ''' <param name="errorMessage">Error message if invalid</param>
        ''' <returns>True if valid JSON</returns>
        Public Function ValidateJsonSyntax(json As String, ByRef errorMessage As String) As Boolean
            If String.IsNullOrWhiteSpace(json) Then
                errorMessage = "JSON string is null or empty"
                Return False
            End If

            Try
                JObject.Parse(json)
                errorMessage = String.Empty
                Return True
            Catch ex As JsonException
                errorMessage = $"Invalid JSON syntax: {ex.Message}"
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Validates that JSON represents a valid template structure
        ''' </summary>
        ''' <param name="json">JSON string to validate</param>
        ''' <param name="errorMessage">Error message if invalid</param>
        ''' <returns>True if valid template structure</returns>
        Public Function ValidateTemplateStructure(json As String, ByRef errorMessage As String) As Boolean
            If String.IsNullOrWhiteSpace(json) Then
                errorMessage = "JSON string is null or empty"
                Return False
            End If

            Try
                Dim jObject As JObject = JObject.Parse(json)

                ' Check required fields
                If jObject("Name") Is Nothing Then
                    errorMessage = "Template must have a Name property"
                    Return False
                End If

                If jObject("Folders") Is Nothing Then
                    errorMessage = "Template must have a Folders property"
                    Return False
                End If

                errorMessage = String.Empty
                Return True

            Catch ex As Exception
                errorMessage = $"Invalid template structure: {ex.Message}"
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Validates JSON against the TemplateDefinition schema
        ''' </summary>
        ''' <param name="json">JSON string to validate</param>
        ''' <param name="errorMessages">List of validation error messages</param>
        ''' <returns>True if valid according to schema</returns>
        Public Function ValidateAgainstSchema(json As String, ByRef errorMessages As List(Of String)) As Boolean
            errorMessages = New List(Of String)
            
            If String.IsNullOrWhiteSpace(json) Then
                errorMessages.Add("JSON string is null or empty")
                Return False
            End If
            
            If _schema Is Nothing Then
                ' Schema not loaded - skip validation
                Return True
            End If
            
            Try
                Dim jObject As JObject = JObject.Parse(json)
                Dim validationErrors As IList(Of String) = Nothing
                
                Dim isValid = jObject.IsValid(_schema, validationErrors)
                
                If Not isValid AndAlso validationErrors IsNot Nothing Then
                    errorMessages.AddRange(validationErrors)
                End If
                
                Return isValid
                
            Catch ex As JsonException
                errorMessages.Add($"Invalid JSON syntax: {ex.Message}")
                Return False
            Catch ex As Exception
                errorMessages.Add($"Schema validation error: {ex.Message}")
                Return False
            End Try
        End Function
        
        ''' <summary>
        ''' Validates JSON against schema and returns detailed validation result
        ''' </summary>
        ''' <param name="json">JSON string to validate</param>
        ''' <returns>Validation result with detailed error information</returns>
        Public Function ValidateWithDetails(json As String) As SchemaValidationResult
            Dim result As New SchemaValidationResult()
            
            If String.IsNullOrWhiteSpace(json) Then
                result.IsValid = False
                result.Errors.Add("JSON string is null or empty")
                Return result
            End If
            
            ' Syntax validation
            Dim syntaxError As String = String.Empty
            If Not ValidateJsonSyntax(json, syntaxError) Then
                result.IsValid = False
                result.Errors.Add(syntaxError)
                Return result
            End If
            
            ' Structure validation
            Dim structureError As String = String.Empty
            If Not ValidateTemplateStructure(json, structureError) Then
                result.IsValid = False
                result.Errors.Add(structureError)
                Return result
            End If
            
            ' Schema validation (if schema available)
            If _schema IsNot Nothing Then
                Dim schemaErrors As New List(Of String)
                If Not ValidateAgainstSchema(json, schemaErrors) Then
                    result.IsValid = False
                    result.Errors.AddRange(schemaErrors)
                    Return result
                End If
            End If
            
            result.IsValid = True
            Return result
        End Function

#End Region

#Region "Export/Import Methods"

        ''' <summary>
        ''' Exports a template to a JSON file
        ''' </summary>
        ''' <param name="template">Template to export</param>
        ''' <param name="filePath">Target file path</param>
        Public Sub ExportTemplate(template As TemplateDefinition, filePath As String)
            SerializeToFile(template, filePath)
        End Sub

        ''' <summary>
        ''' Imports a template from a JSON file (auto-detects format)
        ''' </summary>
        ''' <param name="filePath">Source file path</param>
        ''' <returns>Imported template</returns>
        Public Function ImportTemplate(filePath As String) As TemplateDefinition
            Return DeserializeFromFile(filePath)
        End Function

#End Region

#Region "Utility Methods"

        ''' <summary>
        ''' Gets the current serialization settings
        ''' </summary>
        Public ReadOnly Property Settings As JsonSerializerSettings
            Get
                Return _settings
            End Get
        End Property

        ''' <summary>
        ''' Creates a deep clone of a template using serialization
        ''' </summary>
        Public Function CloneTemplate(template As TemplateDefinition) As TemplateDefinition
            If template Is Nothing Then Return Nothing

            Try
                Dim json = SerializeTemplate(template)
                Return DeserializeTemplate(json)
            Catch ex As Exception
                Throw New InvalidOperationException($"Failed to clone template: {ex.Message}", ex)
            End Try
        End Function

#End Region

    End Class

    ''' <summary>
    ''' Result of schema validation with detailed error information
    ''' </summary>
    Public Class SchemaValidationResult
        
        ''' <summary>
        ''' Whether the JSON is valid according to the schema
        ''' </summary>
        Public Property IsValid As Boolean
        
        ''' <summary>
        ''' List of validation errors (empty if valid)
        ''' </summary>
        Public Property Errors As New List(Of String)
        
        ''' <summary>
        ''' Constructor
        ''' </summary>
        Public Sub New()
            IsValid = False
            Errors = New List(Of String)
        End Sub
        
        ''' <summary>
        ''' Returns a formatted error message
        ''' </summary>
        Public Function GetErrorMessage() As String
            If IsValid Then Return String.Empty
            Return String.Join(Environment.NewLine, Errors)
        End Function
        
    End Class

End Namespace
