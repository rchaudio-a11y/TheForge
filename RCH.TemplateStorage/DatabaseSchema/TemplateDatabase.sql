-- ============================================================================
-- File: TemplateDatabase.sql
-- Project: RCH.TemplateStorage
-- Purpose: MS Access database schema for template storage
-- Created: 2026-01-05
-- Author: TheForge
-- Status: Active
-- Version: 1.0.0
-- 
-- Description:
--   Complete database schema for RCH.TemplateStorage supporting TemplateDefinition,
--   TemplateFolderDefinition, and TemplateFileDefinition models. Designed for
--   MS Access (.accdb) with support for unlimited folder nesting through
--   self-referencing ParentFolderID.
-- 
-- Database Engine: MS Access (JET/ACE)
-- Target Version: Access 2016+
-- Character Encoding: UTF-8
-- 
-- Tables:
--   - Template: Main template definitions with metadata
--   - TemplateFolder: Hierarchical folder structure (unlimited nesting)
--   - TemplateFile: File definitions within folders
-- 
-- Relationships:
--   - Template 1:N TemplateFolder (TemplateID)
--   - TemplateFolder 1:N TemplateFolder (ParentFolderID - self-referencing)
--   - TemplateFolder 1:N TemplateFile (FolderID)
--   - Template 1:N TemplateFile (TemplateID - direct reference)
-- 
-- Indexes:
--   - Primary keys on all tables
--   - Foreign key indexes for performance
--   - Search indexes on Name, Category, Tags
-- 
-- Character Count: 8945
-- Last Updated: 2026-01-05
-- ============================================================================

-- ============================================================================
-- DROP EXISTING TABLES (for clean reinstall)
-- ============================================================================

-- Drop in reverse dependency order
DROP TABLE IF EXISTS TemplateFile;
DROP TABLE IF EXISTS TemplateFolder;
DROP TABLE IF EXISTS Template;

-- ============================================================================
-- TABLE: Template
-- Purpose: Main template definitions with comprehensive metadata
-- Maps to: TemplateDefinition.vb model
-- ============================================================================

CREATE TABLE Template (
    -- Primary Key
    TemplateID AUTOINCREMENT PRIMARY KEY,
    
    -- Core Properties (from DirectoryTemplate - backward compatible)
    Name TEXT(255) NOT NULL,
    Description MEMO,
    
    -- Metadata Properties (new)
    Category TEXT(100),
    Tags MEMO,                          -- Comma-separated tags for searching
    Version TEXT(20) DEFAULT '1.0.0',
    
    -- Audit Properties
    CreatedBy TEXT(100),
    CreatedDate DATETIME DEFAULT NOW(),
    ModifiedBy TEXT(100),
    ModifiedDate DATETIME DEFAULT NOW(),
    
    -- Usage Tracking
    UsageCount LONG DEFAULT 0,
    LastUsedDate DATETIME,
    
    -- Status
    IsActive BIT DEFAULT TRUE,
    
    -- Relationships & Dependencies
    Dependencies MEMO,                  -- JSON array of dependent TemplateIDs
    Notes MEMO,                         -- Additional documentation
    
    -- Constraints
    CONSTRAINT CHK_Template_Name CHECK (LEN(Name) > 0)
);

-- ============================================================================
-- TABLE: TemplateFolder
-- Purpose: Hierarchical folder structure with unlimited nesting
-- Maps to: TemplateFolderDefinition.vb model
-- Features: Self-referencing for unlimited depth
-- ============================================================================

CREATE TABLE TemplateFolder (
    -- Primary Key
    FolderID AUTOINCREMENT PRIMARY KEY,
    
    -- Foreign Keys
    TemplateID LONG NOT NULL,           -- Reference to Template
    ParentFolderID LONG,                -- Self-reference (NULL = root folder)
    
    -- Core Properties (from FolderDefinition - backward compatible)
    Name TEXT(255) NOT NULL,
    IsSelected BIT DEFAULT TRUE,        -- Original property preserved
    
    -- Metadata Properties (new)
    Description MEMO,
    CreatedDate DATETIME DEFAULT NOW(),
    DisplayOrder LONG DEFAULT 0,        -- For consistent ordering within parent
    
    -- Relationships
    CONSTRAINT FK_TemplateFolder_Template 
        FOREIGN KEY (TemplateID) REFERENCES Template(TemplateID)
        ON DELETE CASCADE,
        
    CONSTRAINT FK_TemplateFolder_Parent 
        FOREIGN KEY (ParentFolderID) REFERENCES TemplateFolder(FolderID)
        ON DELETE CASCADE,
    
    -- Constraints
    CONSTRAINT CHK_TemplateFolder_Name CHECK (LEN(Name) > 0),
    CONSTRAINT CHK_TemplateFolder_NoSelfRef CHECK (FolderID <> ParentFolderID)
);

-- ============================================================================
-- TABLE: TemplateFile
-- Purpose: File definitions within template folders
-- Maps to: TemplateFileDefinition.vb model
-- Features: Placeholder support, Forge metadata header flag
-- ============================================================================

CREATE TABLE TemplateFile (
    -- Primary Key
    FileID AUTOINCREMENT PRIMARY KEY,
    
    -- Foreign Keys
    FolderID LONG NOT NULL,             -- Reference to TemplateFolder
    TemplateID LONG NOT NULL,           -- Direct reference to Template
    
    -- Core Properties
    FileName TEXT(255) NOT NULL,
    FileType TEXT(20),                  -- Extension (e.g., ".vb", ".md")
    Description MEMO,
    
    -- Content Template
    ContentTemplate MEMO,               -- Template content with placeholders
    RequiresMetadataHeader BIT DEFAULT FALSE,
    MetadataHeaderTemplate MEMO,        -- Forge header template
    
    -- File Properties
    Encoding TEXT(20) DEFAULT 'UTF-8',
    LineEnding TEXT(10) DEFAULT 'CRLF',
    IsAutoGenerated BIT DEFAULT TRUE,
    
    -- Display & Status
    DisplayOrder LONG DEFAULT 0,
    CreatedDate DATETIME DEFAULT NOW(),
    IsActive BIT DEFAULT TRUE,
    
    -- Relationships
    CONSTRAINT FK_TemplateFile_Folder 
        FOREIGN KEY (FolderID) REFERENCES TemplateFolder(FolderID)
        ON DELETE CASCADE,
        
    CONSTRAINT FK_TemplateFile_Template 
        FOREIGN KEY (TemplateID) REFERENCES Template(TemplateID)
        ON DELETE CASCADE,
    
    -- Constraints
    CONSTRAINT CHK_TemplateFile_FileName CHECK (LEN(FileName) > 0),
    CONSTRAINT CHK_TemplateFile_Extension CHECK (INSTR(FileName, '.') > 0)
);

-- ============================================================================
-- INDEXES FOR PERFORMANCE
-- ============================================================================

-- Template Indexes
CREATE INDEX IDX_Template_Name ON Template(Name);
CREATE INDEX IDX_Template_Category ON Template(Category);
CREATE INDEX IDX_Template_IsActive ON Template(IsActive);
CREATE INDEX IDX_Template_CreatedDate ON Template(CreatedDate);

-- TemplateFolder Indexes
CREATE INDEX IDX_TemplateFolder_TemplateID ON TemplateFolder(TemplateID);
CREATE INDEX IDX_TemplateFolder_ParentFolderID ON TemplateFolder(ParentFolderID);
CREATE INDEX IDX_TemplateFolder_Name ON TemplateFolder(Name);
CREATE INDEX IDX_TemplateFolder_DisplayOrder ON TemplateFolder(TemplateID, ParentFolderID, DisplayOrder);

-- TemplateFile Indexes
CREATE INDEX IDX_TemplateFile_FolderID ON TemplateFile(FolderID);
CREATE INDEX IDX_TemplateFile_TemplateID ON TemplateFile(TemplateID);
CREATE INDEX IDX_TemplateFile_FileName ON TemplateFile(FileName);
CREATE INDEX IDX_TemplateFile_FileType ON TemplateFile(FileType);

-- ============================================================================
-- SAMPLE DATA (Optional - for testing)
-- ============================================================================

-- Insert sample template
INSERT INTO Template (Name, Description, Category, CreatedBy, Tags, IsActive)
VALUES ('Sample Programming Template', 
        'Standard programming project structure', 
        'Software Development',
        'System',
        'programming,automation,vb.net',
        TRUE);

-- Insert root folders (assuming TemplateID = 1)
INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder)
VALUES (1, NULL, '01_Input', TRUE, 'Input files and data', 0);

INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder)
VALUES (1, NULL, '02_Output', TRUE, 'Generated output files', 1);

INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder)
VALUES (1, NULL, '03_Documentation', TRUE, 'Project documentation', 2);

INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder)
VALUES (1, NULL, '04_Programming', TRUE, 'Source code and scripts', 3);

-- Insert subfolder (assuming 04_Programming has FolderID = 4)
INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder)
VALUES (1, 4, 'Robot', TRUE, 'Robot programming files', 0);

INSERT INTO TemplateFolder (TemplateID, ParentFolderID, Name, IsSelected, Description, DisplayOrder)
VALUES (1, 4, 'PLC', TRUE, 'PLC programming files', 1);

-- Insert sample files (assuming Documentation folder has FolderID = 3)
INSERT INTO TemplateFile (FolderID, TemplateID, FileName, FileType, ContentTemplate, RequiresMetadataHeader, DisplayOrder)
VALUES (3, 1, 'README.md', '.md', 
        '# {ProjectName}

## Overview
{Description}

## Created
{Date} by {Author}',
        FALSE, 0);

INSERT INTO TemplateFile (FolderID, TemplateID, FileName, FileType, ContentTemplate, RequiresMetadataHeader, DisplayOrder)
VALUES (5, 1, 'Robot_Program.vb', '.vb',
        ''' <summary>
''' Robot programming logic for {ProjectName}
''' </summary>
Module RobotProgram
    Sub Main()
        '' Implementation here
    End Sub
End Module',
        TRUE, 0);

-- ============================================================================
-- VALIDATION QUERIES
-- ============================================================================

-- Query 1: Get all templates with folder count
-- SELECT t.TemplateID, t.Name, COUNT(f.FolderID) AS FolderCount
-- FROM Template t
-- LEFT JOIN TemplateFolder f ON t.TemplateID = f.TemplateID
-- GROUP BY t.TemplateID, t.Name;

-- Query 2: Get folder hierarchy for a template (with depth)
-- WITH RECURSIVE FolderHierarchy AS (
--     SELECT FolderID, TemplateID, ParentFolderID, Name, 0 AS Depth
--     FROM TemplateFolder
--     WHERE ParentFolderID IS NULL
--     UNION ALL
--     SELECT f.FolderID, f.TemplateID, f.ParentFolderID, f.Name, h.Depth + 1
--     FROM TemplateFolder f
--     INNER JOIN FolderHierarchy h ON f.ParentFolderID = h.FolderID
-- )
-- SELECT * FROM FolderHierarchy
-- WHERE TemplateID = 1
-- ORDER BY Depth, DisplayOrder;

-- Query 3: Get all files in a template
-- SELECT t.Name AS TemplateName, f.Name AS FolderName, tf.FileName, tf.FileType
-- FROM Template t
-- INNER JOIN TemplateFolder f ON t.TemplateID = f.TemplateID
-- INNER JOIN TemplateFile tf ON f.FolderID = tf.FolderID
-- WHERE t.TemplateID = 1
-- ORDER BY f.DisplayOrder, tf.DisplayOrder;

-- Query 4: Search templates by tag
-- SELECT * FROM Template
-- WHERE Tags LIKE '%programming%'
-- OR Tags LIKE '%automation%';

-- Query 5: Get active templates ordered by usage
-- SELECT TemplateID, Name, Category, UsageCount, LastUsedDate
-- FROM Template
-- WHERE IsActive = TRUE
-- ORDER BY UsageCount DESC, LastUsedDate DESC;

-- ============================================================================
-- DATABASE MAINTENANCE
-- ============================================================================

-- Compact and repair database (execute in Access or via ADOX)
-- DBENGINE.COMPACTDATABASE "OldDatabase.accdb", "NewDatabase.accdb"

-- Backup recommendation: Copy .accdb file before major operations

-- ============================================================================
-- SCHEMA VERSION HISTORY
-- ============================================================================

-- v1.0.0 (2026-01-05) - Initial schema
--   - Template table with full metadata
--   - TemplateFolder with unlimited nesting (self-referencing)
--   - TemplateFile with Forge header support
--   - All indexes for performance
--   - Sample data for testing

-- ============================================================================
-- END OF SCHEMA
-- ============================================================================
