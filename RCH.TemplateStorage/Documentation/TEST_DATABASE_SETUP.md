<!-- ============================================================================
File: TEST_DATABASE_SETUP.md
Project: RCH.TemplateStorage
Purpose: Instructions for creating and using test database
Created: 2026-01-05
Author: TheForge
Status: Active
Version: 1.0.0

Description:
  Complete instructions for creating, initializing, and validating the
  test database (Templates_Test.accdb). Includes manual creation steps,
  programmatic creation using DatabaseInitializer, and validation procedures.

Character Count: 5247
Last Updated: 2026-01-05
============================================================================ -->

# Test Database Setup Guide

**Database:** Templates_Test.accdb  
**Purpose:** Testing and validation of RCH.TemplateStorage  
**Schema Version:** 1.0.0  
**Created:** 2026-01-05

---

## Overview

The test database provides a complete environment for testing all RCH.TemplateStorage functionality including:
- Template CRUD operations
- Folder hierarchy (unlimited nesting)
- File template management
- JSON serialization/deserialization
- Legacy TemplateBuilderControl compatibility

---

## Prerequisites

### Required Software
- **MS Access Database Engine** (ACE) 12.0 or higher
  - Included with MS Office 2007+
  - Or download: [Microsoft Access Database Engine Redistributable](https://www.microsoft.com/download)
- **.NET 8.0 SDK** (for programmatic creation)

### Required Permissions
- Write access to output directory
- Ability to create .accdb files

---

## Method 1: Programmatic Creation (Recommended)

### Using CreateTestDatabase Class

```vb
Imports RCH.TemplateStorage.Testing

' Create in default location
Dim creator As New CreateTestDatabase()
creator.CreateDefaultDatabase()

' Or specify custom path
creator.CreateDatabase("C:\Temp\Templates_Test.accdb")
```

### Using DatabaseInitializer Directly

```vb
Imports RCH.TemplateStorage.Database

Dim dbPath = "C:\Temp\Templates_Test.accdb"
Dim initializer As New DatabaseInitializer(dbPath)

' Step 1: Create database file
If initializer.CreateDatabase() Then
    Console.WriteLine("Database created successfully")
End If

' Step 2: Initialize schema with sample data
If initializer.InitializeSchema(includeSampleData:=True) Then
    Console.WriteLine("Schema initialized with sample data")
End If

' Step 3: Validate
If initializer.ValidateDatabaseSchema() Then
    Console.WriteLine("Schema validated successfully")
End If
```

---

## Method 2: Manual Creation

### Step 1: Create Database File

**Option A: Using MS Access**
1. Open Microsoft Access
2. Click **Blank Database**
3. Name: `Templates_Test.accdb`
4. Location: Choose destination folder
5. Click **Create**

**Option B: Using VBA/ADOX**
```vb
' In Access VBA or VB.NET with ADOX reference
Dim cat As New ADOX.Catalog
cat.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Temp\Templates_Test.accdb")
```

### Step 2: Execute Schema Script

1. Open `Templates_Test.accdb` in Access
2. Go to **Database Tools** ? **Query Design**
3. Close the Show Table dialog
4. Switch to **SQL View**
5. Copy SQL from `DatabaseSchema/TemplateDatabase.sql`
6. Execute each CREATE TABLE statement individually
7. Execute each CREATE INDEX statement
8. Execute sample data INSERT statements (optional)

### Step 3: Verify Tables

Expected tables:
- ? **Template** (17 columns)
- ? **TemplateFolder** (8 columns)
- ? **TemplateFile** (15 columns)

---

## Sample Data Structure

The test database includes sample data demonstrating:

### Template: "Sample Programming Template"
- **Category:** Software Development
- **Tags:** programming, automation, vb.net
- **Status:** Active

### Folder Hierarchy (Unlimited Nesting)
```
Root (TemplateID = 1)
??? 01_Input (FolderID = 1, ParentFolderID = NULL)
??? 02_Output (FolderID = 2, ParentFolderID = NULL)
??? 03_Documentation (FolderID = 3, ParentFolderID = NULL)
?   ??? README.md (FileID = 1)
??? 04_Programming (FolderID = 4, ParentFolderID = NULL)
    ??? Robot (FolderID = 5, ParentFolderID = 4)
    ?   ??? Robot_Program.vb (FileID = 2)
    ??? PLC (FolderID = 6, ParentFolderID = 4)
```

### File Templates
1. **README.md**
   - Type: .md (Markdown)
   - Placeholder support: {ProjectName}, {Description}, {Date}, {Author}
   - RequiresMetadataHeader: FALSE

2. **Robot_Program.vb**
   - Type: .vb (VB.NET)
   - Placeholder support: {ProjectName}
   - RequiresMetadataHeader: TRUE

---

## Validation

### Automatic Validation

```vb
Imports RCH.TemplateStorage.Database

Using connection As New DatabaseConnection("C:\Temp\Templates_Test.accdb")
    Dim validator As New DatabaseValidator(connection)
    
    ' Validate schema
    If validator.ValidateSchema() Then
        Console.WriteLine("Schema valid")
    End If
    
    ' Validate referential integrity
    If validator.ValidateReferentialIntegrity() Then
        Console.WriteLine("Integrity valid")
    End If
    
    ' Get detailed report
    Console.WriteLine(validator.GetValidationReport())
End Using
```

### Manual Validation Queries

**Check table existence:**
```sql
SELECT Name FROM MSysObjects 
WHERE Type=1 AND Name IN ('Template', 'TemplateFolder', 'TemplateFile');
```

**Verify folder hierarchy:**
```sql
SELECT f.FolderID, f.ParentFolderID, f.Name, f.DisplayOrder
FROM TemplateFolder f
WHERE f.TemplateID = 1
ORDER BY f.ParentFolderID, f.DisplayOrder;
```

**Check for orphaned records:**
```sql
-- Orphaned folders
SELECT COUNT(*) FROM TemplateFolder f 
WHERE NOT EXISTS (SELECT 1 FROM Template t WHERE t.TemplateID = f.TemplateID);

-- Orphaned files
SELECT COUNT(*) FROM TemplateFile tf 
WHERE NOT EXISTS (SELECT 1 FROM TemplateFolder f WHERE f.FolderID = tf.FolderID);
```

---

## Testing Scenarios

### Scenario 1: CRUD Operations
```vb
' Test template creation, retrieval, update, delete
' (Implemented in Phase 4)
```

### Scenario 2: Unlimited Folder Nesting
```vb
' Create deeply nested folder structure (10+ levels)
Dim root = New TemplateFolderDefinition("Level1")
Dim level2 = root.AddSubFolder("Level2")
Dim level3 = level2.AddSubFolder("Level3")
' ... continue nesting
```

### Scenario 3: Legacy JSON Import
```vb
Imports RCH.TemplateStorage.Services.Implementations

Dim serializer As New TemplateJsonSerializer()
Dim legacyJson = File.ReadAllText("LegacyTemplate.json")
Dim template = serializer.ImportLegacyTemplate(legacyJson)

' Save to database (Phase 4)
```

---

## Troubleshooting

### Issue: "Provider not found"
**Solution:** Install MS Access Database Engine  
- Download: https://www.microsoft.com/download/details.aspx?id=54920
- Install appropriate version (32-bit or 64-bit matching your .NET runtime)

### Issue: "Database is read-only"
**Solution:** Check file permissions  
```powershell
# PowerShell
Get-Acl "C:\Temp\Templates_Test.accdb" | Format-List
```

### Issue: "Cannot create database"
**Solution:** Verify ADOX is available  
```vb
Try
    Dim adoxType = Type.GetTypeFromProgID("ADOX.Catalog")
    If adoxType IsNot Nothing Then
        Console.WriteLine("ADOX available")
    End If
Catch ex As Exception
    Console.WriteLine($"ADOX not available: {ex.Message}")
End Try
```

### Issue: "Schema validation fails"
**Solution:** Re-run schema initialization  
```vb
Dim initializer As New DatabaseInitializer(dbPath)
initializer.InitializeSchema(includeSampleData:=False)
```

---

## Database Maintenance

### Compact Database
```vb
' Using Access: Database Tools ? Compact and Repair Database
' Or programmatically (requires DAO reference):
' DBEngine.CompactDatabase "source.accdb", "destination.accdb"
```

### Backup Database
```vb
' Simple file copy
File.Copy("Templates_Test.accdb", "Templates_Test_Backup.accdb", overwrite:=True)
```

### Reset to Defaults
```vb
' Delete and recreate
If File.Exists(dbPath) Then File.Delete(dbPath)
Dim creator As New CreateTestDatabase()
creator.CreateDatabase(dbPath)
```

---

## Expected Validation Results

### Schema Validation
- ? 3 required tables exist
- ? 40 required columns present
- ? 8 indexes created
- ? Primary keys defined
- ? Foreign keys defined

### Data Validation
- ? 1 sample template
- ? 6 folders (demonstrating hierarchy)
- ? 2 sample files
- ? 0 orphaned records
- ? 0 circular references

---

## Next Steps

After test database creation:
1. ? Verify schema with DatabaseValidator
2. ? Run ModelTests.vb to test models
3. ? Run BackwardCompatibilityTests.vb to test JSON import
4. ? Implement CRUD service (Phase 4)
5. ? Test full round-trip (JSON ? DB ? JSON)

---

**Document Version:** 1.0.0  
**Last Updated:** 2026-01-05  
**Character Count:** 5247  
**Related:** TemplateDatabase.sql, DatabaseInitializer.vb, CreateTestDatabase.vb
