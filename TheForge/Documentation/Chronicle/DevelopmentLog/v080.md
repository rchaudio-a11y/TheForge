## v0.8.0 — Advanced Features (2025-01-02)

### Description
Added advanced module system features including configuration support, dependency resolution, hot-reload capability, and log filtering/search functionality.

### What It Does
- **Module Configuration Support:**
  - Creates `IModuleConfiguration` interface with GetValue(), SetValue(), HasKey() methods
  - Implements thread-safe `ModuleConfiguration` class
  - Updates `IModule` interface with `LoadConfiguration()` method
  - ModuleLoaderService automatically loads `modulename.config` files from Modules directory
  - Parses simple key=value format (supports # and ; comments)
  - Configuration passed to modules after Initialize() and before Execute()
  - `InitializeAndConfigureModule()` method ensures correct initialization order

- **Module Dependency Resolution:**
  - Creates `ModuleDependencyAttribute` for declaring module dependencies
  - Updates `ModuleMetadata` with Dependencies property
  - ModuleLoaderService scans module attributes during discovery
  - Validates all dependencies exist
  - Detects and prevents circular dependencies
  - Throws clear errors for missing or circular dependencies
  - Logs all dependency resolution steps

- **Hot-Reload Capability:**
  - Adds `ReloadModule()` method to IModuleLoaderService
  - Unloads existing module instance
  - Reloads assembly from disk
  - Recreates module instance with fresh state
  - Calls InitializeAndConfigureModule() automatically
  - Updates metadata and cache
  - Adds btnReloadModule to UI (enabled only when module is loaded)

- **Log Filtering and Search:**
  - Updates `ILoggingService` with FilterLevel and SearchText properties
  - LoggingService respects filters when raising events
  - Adds filter panel to log output area with:
    - cmbLogLevel ComboBox (All, Info, Warning, Error)
    - txtLogSearch TextBox for search term
    - btnApplyFilter button to apply filters
  - RebuildLogOutput() filters existing log entries
  - Level filter: shows messages >= selected level
  - Search filter: case-insensitive substring match
  - Combined filters: must satisfy both criteria
  - Shows filter status in StatusLabel

- **UI Enhancements:**
  - Adds btnReloadModule button to module list panel
  - Adds filter panel with deterministic TableLayoutPanel layout
  - Updates state management: Reload enabled only when module is loaded
  - All existing state rules maintained

### Issues Encountered

#### Issue 1: Assembly Unloading Limitations in .NET
**Problem:** .NET Core/.NET 5+ cannot truly unload assemblies without using AssemblyLoadContext or AppDomains, but v0.8.0 requirements specified hot-reload.

**Root Cause:** Assembly.LoadFrom() loads assemblies into the default context which cannot be unloaded.

**Resolution:** Implemented ReloadModule() as unload-then-reload pattern:
1. Call OnUnload() and Dispose() on existing instance
2. Clear cached instance
3. Re-call Assembly.LoadFrom() (assembly DLL can be read multiple times)
4. Create new instance
5. Re-initialize with services and configuration

**Note:** While not true hot-reload (assembly remains in memory), this provides module refresh functionality for most use cases. True unloading would require AssemblyLoadContext implementation (future enhancement).

#### Issue 2: Configuration File Parsing Edge Cases
**Problem:** Configuration files might contain empty lines, comments, or malformed entries.

**Root Cause:** Simple key=value parsing doesn't handle all edge cases.

**Resolution:** Implemented robust parsing logic:
```vb
For Each line As String In lines
    Dim trimmedLine As String = line.Trim()
    If String.IsNullOrEmpty(trimmedLine) OrElse trimmedLine.StartsWith("#") OrElse trimmedLine.StartsWith(";") Then
        Continue For
    End If
    
    Dim separatorIndex As Integer = trimmedLine.IndexOf("="c)
    If separatorIndex > 0 Then
        Dim key As String = trimmedLine.Substring(0, separatorIndex).Trim()
        Dim value As String = trimmedLine.Substring(separatorIndex + 1).Trim()
        config.SetValue(key, value)
    End If
Next
```

#### Issue 3: Circular Dependency Detection Algorithm
**Problem:** Needed efficient algorithm to detect circular dependencies in module dependency graph.

**Root Cause:** Simple checks wouldn't catch complex circular chains (A?B?C?A).

**Resolution:** Implemented depth-first search with recursion stack tracking:
```vb
Private Function HasCircularDependency(typeName As String, modules As List(Of Models.ModuleMetadata), _
                                       visited As HashSet(Of String), recursionStack As HashSet(Of String)) As Boolean
    If Not visited.Contains(typeName) Then
        visited.Add(typeName)
        recursionStack.Add(typeName)
        
        Dim currentModule As Models.ModuleMetadata = modules.FirstOrDefault(Function(m) m.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase))
        If currentModule IsNot Nothing AndAlso currentModule.Dependencies IsNot Nothing Then
            For Each dep As String In currentModule.Dependencies
                If Not visited.Contains(dep) Then
                    If HasCircularDependency(dep, modules, visited, recursionStack) Then
                        Return True
                    End If
                ElseIf recursionStack.Contains(dep) Then
                    Return True
                End If
            Next
        End If
    End If
    
    recursionStack.Remove(typeName)
    Return False
End Function
```

#### Issue 4: Log Filter Performance with Large Entry Sets
**Problem:** Filtering large log sets (1000+ entries) on every Apply Filter click could be slow.

**Root Cause:** Iterating through all entries and rebuilding TextBox content is O(n).

**Resolution:** 
1. Implemented efficient filtering in RebuildLogOutput() with early termination
2. Used IndexOf() for case-insensitive search (faster than regex for simple substring)
3. Parsed log level from entry prefix once
4. Future optimization: Consider virtualized list control if performance becomes issue

#### Issue 5: Module State Consistency During Reload
**Problem:** If reload fails mid-process, module could be in inconsistent state (unloaded but not reloaded).

**Root Cause:** Reload involves multiple steps that could fail at any point.

**Resolution:** Implemented try-catch in ReloadModule() UI handler with state recovery:
```vb
Try
    _currentModule = _moduleLoaderService.ReloadModule(moduleMetadata)
    _moduleLoaderService.InitializeAndConfigureModule(_currentModule, moduleMetadata.DisplayName)
    UpdateModuleDetails(moduleMetadata)
    btnUnloadModule.Enabled = True
    btnReloadModule.Enabled = True
Catch ex As Exception
    _loggingService.LogError(String.Format("Failed to reload module: {0}", ex.Message))
    UpdateModuleDetails(moduleMetadata)  ' Refresh UI to show actual state
End Try
```

#### Issue 6: Filter Panel Layout Integration
**Problem:** Adding filter panel to existing log output area required careful layout ordering.

**Root Cause:** Panel needs to sit below txtLogOutput but above pnlLogOutput's bottom edge.

**Resolution:** Created pnlLogFilter as child of pnlLogOutput:
```vb
pnlLogFilter.Dock = DockStyle.Bottom
pnlLogFilter.Height = 70

pnlLogOutput.Controls.Add(txtLogOutput)  ' Fill
pnlLogOutput.Controls.Add(pnlLogFilter)  ' Bottom
```

Used TableLayoutPanel inside pnlLogFilter for filter controls (2 columns × 3 rows).

#### Issue 7: Project File Corruption During Updates
**Problem:** Initial attempts to update project file via PowerShell here-strings failed with parsing errors.

**Root Cause:** PowerShell interprets XML angle brackets as operators.

**Resolution:** Created temporary .vbproj file with create_file tool, then moved it to replace original:
```
create_file TheForge/TheForge_fixed.vbproj
Move-Item TheForge_fixed.vbproj TheForge.vbproj -Force
```

#### Issue 8: NullReferenceException in LoadConfiguration
**Problem:** SampleModule crashed with NullReferenceException when LoadConfiguration() was called because `_loggingService` was null.

**Root Cause:** Initial implementation called LoadModuleConfiguration() inside LoadModuleInternal() before the module had been initialized. The call sequence was:
1. Instantiate module
2. LoadConfiguration() ? ? crashes, `_loggingService` is null
3. (Never reached Initialize())

**Resolution:** Refactored module initialization to ensure correct call order:
1. Removed LoadModuleConfiguration() from LoadModuleInternal()
2. Created new `InitializeAndConfigureModule()` method in ModuleLoaderService with `Implements` clause
3. InitializeAndConfigureModule() calls Initialize() first, then LoadModuleConfiguration()
4. Updated UI (ExecuteModule and ReloadModule) to call InitializeAndConfigureModule() instead of Initialize()
5. Added defensive null checks in SampleModule for robustness

Correct sequence now:
```vb
' In IModuleLoaderService:
Sub InitializeAndConfigureModule(moduleInterface As Modules.Interfaces.IModule, moduleName As String)

' In ModuleLoaderService:
Public Sub InitializeAndConfigureModule(moduleInterface As Modules.Interfaces.IModule, moduleName As String) Implements Services.Interfaces.IModuleLoaderService.InitializeAndConfigureModule
    If moduleInterface Is Nothing Then
        Return
    End If
    
    _loggingService.LogInfo(String.Format("Initializing module: {0}", moduleName))
    moduleInterface.Initialize(_loggingService)  ' Set up logging first
    
    LoadModuleConfiguration(moduleInterface, moduleName)  ' Then load config
End Sub

' In DashboardMainForm:
_currentModule = _moduleLoaderService.LoadModule(modulePath)
_moduleLoaderService.InitializeAndConfigureModule(_currentModule, moduleMetadata.DisplayName)
_currentModule.Execute()
```

**Build Error Fixed:** Added missing `Implements Services.Interfaces.IModuleLoaderService.InitializeAndConfigureModule` clause to the method signature in ModuleLoaderService.vb to satisfy the interface contract.

Also added defensive null checks in SampleModule to prevent crashes if methods are called out of order:
```vb
Public Sub LoadConfiguration(config As TheForge.Modules.Interfaces.IModuleConfiguration) Implements TheForge.Modules.Interfaces.IModule.LoadConfiguration
    _config = config
    
    If _loggingService IsNot Nothing Then
        _loggingService.LogInfo("SampleModule configuration loaded")
        ' ... log configuration values
    End If
End Sub
```

---

## Development Patterns & Lessons Learned (Updated for v0.8.0)

### New Patterns in v0.8.0

9. **Configuration File Management**
   - Use simple key=value format for module configuration
   - Support comment lines (# or ;)
   - Trim keys and values to handle whitespace
   - Store configuration in same directory as module DLL
   - Use StringComparer.OrdinalIgnoreCase for case-insensitive keys

10. **Dependency Graph Validation**
    - Always validate dependencies before loading modules
    - Use depth-first search for circular dependency detection
    - Maintain both visited set and recursion stack
    - Provide clear error messages with module names
    - Log all validation steps for debugging

11. **Hot-Reload Pattern**
    - Unload existing instance completely before reload
    - Re-run full initialization sequence after reload
    - Handle reload failures gracefully with state recovery
    - Update UI immediately to reflect actual state
    - Log every step of reload process

12. **Log Filtering Architecture**
    - Store all log entries in service layer
    - Apply filters at presentation layer (UI)
    - Support multiple filter criteria (level + search)
    - Use efficient string operations (IndexOf vs regex)
    - Rebuild display only when filters change

13. **Module Initialization Order** *(Critical Pattern)*
    - Always call Initialize() before LoadConfiguration()
    - LoadConfiguration() depends on logging service being set up
    - Create helper methods (InitializeAndConfigureModule) to enforce correct order
    - Separate instantiation from initialization
    - Add defensive null checks in modules for robustness

### Recurring Issues (Updated)

6. **Project File Management** *(New in v0.8.0)*
   - PowerShell XML editing is error-prone
   - Solution: Use create_file + Move-Item pattern for XML files
   - Always verify project file syntax after updates

7. **Assembly Loading Constraints** *(New in v0.8.0)*
   - .NET Core assemblies can't be truly unloaded from default context
   - Solution: Document limitations, implement best-effort reload
   - Consider AssemblyLoadContext for future true hot-reload

8. **Method Call Order Dependencies** *(New in v0.8.0)*
   - Some methods must be called in specific order (Initialize before LoadConfiguration)
   - Easy to violate order constraints when refactoring
   - Solution: Create wrapper methods that enforce correct sequence
   - Add defensive null checks in implementation code
   - Document call order requirements in XML comments

---

## Build Status Summary (Updated)

| Version | Build Status | Runtime Status | Notes |
|---------|-------------|----------------|-------|
| v0.1.0  | ? Success | ? Runs | Initial restructuring complete |
| v0.2.0  | ? Success | ? Runs | UI controls functional |
| v0.3.0  | ? Success | ? Runs | Module discovery working |
| v0.4.0  | ? Success | ? Runs | Logging integration complete |
| v0.5.0  | ? Success | ? Runs | Full module execution working |
| v0.6.0  | ? Success | ? Runs | Module lifecycle management complete |
| v0.7.0  | ? Success | ? Runs | UI improvements complete |
| v0.8.0  | ? Success | ? Runs | Advanced features complete |

---

## Files Created/Modified in v0.8.0

### New Files
- Source/Modules/Interfaces/IModuleConfiguration.vb
- Source/Modules/Interfaces/ModuleDependencyAttribute.vb
- Source/Models/ModuleConfiguration.vb
- SampleForgeModule/SampleForgeModule.config

### Modified Files
- Source/Modules/Interfaces/IModule.vb (added LoadConfiguration)
- Source/Models/ModuleMetadata.vb (added Dependencies property)
- Source/Services/Interfaces/IModuleLoaderService.vb (added ReloadModule, InitializeAndConfigureModule, FilterLevel, SearchText)
- Source/Services/Interfaces/ILoggingService.vb (added FilterLevel, SearchText)
- Source/Services/Implementations/ModuleLoaderService.vb (config loading, dependency resolution, reload, InitializeAndConfigureModule)
- Source/Services/Implementations/LoggingService.vb (filter support)
- Source/UI/DashboardMainForm.vb (reload button, filter panel, filter logic, InitializeAndConfigureModule calls)
- SampleForgeModule/SampleModule.vb (LoadConfiguration implementation, defensive null checks)
- TheForge.vbproj (added new files)

---

## Key Architectural Decision: Module Initialization Order

To prevent NullReferenceException and ensure modules are properly initialized with all dependencies before configuration is loaded, v0.8.0 establishes the following architectural contract:

**Module Lifecycle Sequence:**
1. **Instantiation** - `Activator.CreateInstance()` creates module instance
2. **Initialization** - `Initialize(loggingService)` sets up required services
3. **Configuration** - `LoadConfiguration(config)` applies module-specific settings
4. **Execution** - `Execute()` runs module functionality
5. **Unload** - `OnUnload()` + `Dispose()` cleanup

**Enforced by:**
- `InitializeAndConfigureModule()` helper method in ModuleLoaderService
- UI code calls this method instead of calling Initialize() and LoadConfiguration() separately
- Defensive null checks in module implementations
- Clear documentation in interface XML comments

**v0.8.0 successfully implemented. The Forge now has production-grade module management capabilities with proper initialization ordering.**
