# RCH.Forge.Dashboard — Core Models

**Folder Type:** Source/Core  
**Purpose:** Foundational data models and core domain entities  
**Last Updated:** 2025-01-02  
**Architectural Note:** Created in v0.9.7 per file2.md specifications

---

## Overview

This folder contains the core foundational models for RCH.Forge.Dashboard. These are pure data models (POCOs) that represent the fundamental domain concepts of the Forge module system.

**Why /Core vs /Models:**
- `/Core`: Foundational models central to the application's domain
- `/Models`: UI-specific or auxiliary models (future use)

---

## Files in This Folder

### ModuleMetadata.vb
**Purpose:** Represents metadata for a discovered module  
**Namespace:** `Models`  

**Properties:**
- `FileName As String` - The DLL filename
- `DisplayName As String` - Human-readable module name
- `TypeName As String` - Full type name of the IModule implementation
- `LastLoadedTime As DateTime?` - Timestamp of last load
- `IsLoaded As Boolean` - Current load status
- `CachedInstance As IModule` - Cached module instance (if loaded)
- `Dependencies As String()` - Array of module type names this module depends on

**Constructors:**
```vb
' Default constructor
New()

' Basic constructor
New(fileName As String, displayName As String)

' Full constructor
New(fileName As String, displayName As String, typeName As String)
```

**Usage:**
```vb
Dim metadata As New Models.ModuleMetadata("MyModule.dll", "My Module", "MyModule.MyModuleClass")
metadata.Dependencies = New String() {"OtherModule.OtherClass"}
```

**Used By:**
- `ModuleLoaderService` - Creates and manages metadata
- `ModuleListControl` - Displays modules
- `ModuleDetailsControl` - Shows module details
- `DashboardMainForm` - Coordinates module operations

---

### ModuleConfiguration.vb
**Purpose:** Represents configuration data loaded from .config files  
**Namespace:** `Models`

**Properties:**
- Internal dictionary for key-value pairs

**Methods:**
- `SetValue(key As String, value As String)` - Store configuration value
- `GetValue(key As String) As String` - Retrieve configuration value
- `GetValueOrDefault(key As String, defaultValue As String) As String` - Retrieve with fallback

**Configuration File Format:**
```
# Comment line
key1=value1
key2=value2
```

**Usage:**
```vb
' In ModuleLoaderService
Dim config As New Models.ModuleConfiguration()
config.SetValue("ApiUrl", "https://api.example.com")
config.SetValue("Timeout", "30")
moduleInterface.LoadConfiguration(config)

' In Module
Dim apiUrl As String = config.GetValue("ApiUrl")
Dim timeout As Integer = Integer.Parse(config.GetValueOrDefault("Timeout", "10"))
```

**Used By:**
- `ModuleLoaderService` - Loads .config files and creates configuration objects
- `IModule.LoadConfiguration()` - Modules receive configuration

---

## Design Principles

### 1. Pure Data Models (POCOs)
These classes contain **only data**, no business logic:
- Properties for state
- Constructors for initialization
- NO service dependencies
- NO UI references

### 2. Explicit Typing
All properties are explicitly typed (Option Strict On):
```vb
Public Property FileName As String  ' ? Explicit
' NOT: Public Property FileName      ' ? Implicit
```

### 3. Nullable Where Appropriate
Use nullable types for optional data:
```vb
Public Property LastLoadedTime As DateTime?  ' Can be Nothing
Public Property IsLoaded As Boolean          ' Always has value
```

### 4. XML Documentation
All classes and public members have XML comments:
```vb
''' <summary>
''' Represents metadata for a discovered module.
''' </summary>
Public Class ModuleMetadata
    ''' <summary>
    ''' Gets or sets the file name of the module.
    ''' </summary>
    Public Property FileName As String
End Class
```

---

## Adding New Core Models

When creating a new foundational model:

1. **Place in /Core folder**
   - Core models are domain-critical
   - Auxiliary/UI models go in /Models (if needed)

2. **Follow POCO pattern**
   ```vb
   Namespace Models  ' Keep namespace as Models for consistency
       Public Class NewModel
           Public Property Property1 As String
           Public Property Property2 As Integer
           
           Public Sub New()
               ' Initialize defaults
           End Sub
       End Class
   End Namespace
   ```

3. **Add XML documentation**
   - Class summary
   - Property descriptions
   - Constructor explanations

4. **Update this README**
   - Add file description
   - Document properties and methods
   - Provide usage examples

5. **Update project file**
   ```xml
   <Compile Include="Source\Core\NewModel.vb" />
   ```

---

## Namespace Convention

**All Core models use `Models` namespace** regardless of folder location:
```vb
Namespace Models
    Public Class ModuleMetadata
        ' ...
    End Class
End Namespace
```

**Why?**
- Maintains backward compatibility
- Namespace represents logical grouping, not physical location
- Folder structure can change without breaking references

---

## Related Documentation

- **Module Loading:** `/Source/Services/Implementations/ModuleLoaderService.vb`
- **Module Interface:** `/Source/Modules/Interfaces/IModule.vb`
- **Architecture:** `/Documentation/Tomes/ForgeTome.md`
- **Architectural Standards:** `/Prompts/file2.md` (specifies /Core folder)

---

## Migration Notes (v0.9.7)

**Previous Location:** `/Source/Models`  
**New Location:** `/Source/Core` (v0.9.7)  

**Reason for Move:**
- Aligns with file2.md architectural specifications
- Distinguishes foundational models from auxiliary models
- Improves architectural clarity

**Namespace:** Unchanged (`Models`)  
**References:** No updates required (namespace didn't change)

---

**All core domain models that define the fundamental concepts of the Forge module system reside in this folder.**
