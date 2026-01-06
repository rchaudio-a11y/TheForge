# TemplateStorageModule - Troubleshooting Guide

## Issue: "Run Module" Button Disabled

### Root Cause Analysis

The "Run Module" button stays disabled because:

1. **Button Logic in DashboardMainForm.vb (line 70-77):**
   ```vb
   If moduleMetadata.IsLoaded Then
       UpdateButtonStates(False, True, True)  ' Run = DISABLED
   Else
       UpdateButtonStates(True, False, False)  ' Run = ENABLED
   End If
   ```

2. **Module Discovery Check (ModuleLoaderService.vb line 185):**
   ```vb
   Private Function FindModuleType(assembly As Assembly) As Type
       Dim moduleInterfaceType As Type = GetType(Modules.Interfaces.IModule)
       For Each type In assembly.GetTypes()
           If moduleInterfaceType.IsAssignableFrom(type) Then
               Return type  ' ? Checks for IModule interface
           End If
       Next
       Return Nothing  ' ? If not found, TypeName stays empty!
   End Function
   ```

3. **The Problem:**
   - TemplateStorageModule uses duck typing (no direct IModule reference)
   - `IsAssignableFrom()` returns False
   - `TypeName` is set to empty string
   - Dashboard sees empty TypeName and logs warning
   - Module isn't considered valid

### Check Your Logs

Look for this warning:
```
[WARN] Module RCH.TemplateStorage does not implement IModule
```

If you see that, it means `TypeName` is empty.

---

## Solutions

### Solution 1: Quick Fix - Copy IModule Interface (5 min)

Create a local copy of IModule in RCH.TemplateStorage that matches TheForge's signature:

**File:** `RCH.TemplateStorage\Modules\Interfaces\IModule.vb`
```vb
Namespace Modules.Interfaces
    Public Interface IModule
        Inherits IDisposable
        
        Sub Initialize(loggingService As Object)
        Sub LoadConfiguration(config As Object)
        Sub Execute()
        Sub OnUnload()
    End Interface
End Namespace
```

Then update TemplateStorageModule:
```vb
Public Class TemplateStorageModule
    Implements Modules.Interfaces.IModule  ' Local interface
```

This works because:
- Interface matching is by method signature, not namespace
- TheForge sees methods match IModule contract
- No circular reference issue

---

### Solution 2: Fix ModuleLoaderService (15 min)

Update `FindModuleType()` to check method signatures instead of interface inheritance:

```vb
Private Function FindModuleType(assembly As Assembly) As Type
    For Each type As Type In assembly.GetTypes()
        If Not type.IsClass OrElse type.IsAbstract Then Continue For
        
        ' Check for required methods (duck typing)
        Dim hasInit = type.GetMethod("Initialize") IsNot Nothing
        Dim hasLoad = type.GetMethod("LoadConfiguration") IsNot Nothing
        Dim hasExec = type.GetMethod("Execute") IsNot Nothing
        Dim hasUnload = type.GetMethod("OnUnload") IsNot Nothing
        
        If hasInit AndAlso hasLoad AndAlso hasExec AndAlso hasUnload Then
            Return type  ' Found valid module via duck typing
        End If
    Next
    
    Return Nothing
End Function
```

This properly supports duck-typed modules.

---

### Solution 3: Use Assembly Attributes (10 min)

Add a marker attribute to identify modules:

```vb
' In RCH.TemplateStorage
<ForgeModule("Template Storage", "1.0.0")>
Public Class TemplateStorageModule
    ' ...
End Class
```

Then update FindModuleType to check for attribute.

---

## Recommended Approach

**Use Solution 1** - It's the quickest and follows established plugin patterns.

### Steps:

1. **Create local IModule interface** (already done - see IModuleContract.vb)
2. **Update TemplateStorageModule to implement it** (already done)
3. **Rebuild RCH.TemplateStorage**
4. **Test in dashboard**

---

## Expected Result After Fix

When you select the module:
```
StatusLabel: "Selected: RCH.TemplateStorage (Not Loaded)"
btnRunModule.Enabled: True
TypeName: "RCH.TemplateStorage.Modules.TemplateStorageModule"
```

Click "Run Module":
```
[INFO] Loading module: RCH.TemplateStorage
[INFO] Instantiating module type: RCH.TemplateStorage.Modules.TemplateStorageModule
[INFO] Module loaded successfully
[INFO] Initializing module: RCH.TemplateStorage
[INFO] Template Storage Module initializing...
[INFO] Executing Template Storage Module...
[INFO] === Template Storage Module ===
[INFO] Database: C:\Users\...\Templates.accdb
[INFO] Connection: OK
[INFO] Schema: Valid
[INFO] Templates: 0
```

After execution:
```
StatusLabel: "Executed: RCH.TemplateStorage"
btnRunModule.Enabled: False (already loaded)
btnUnloadModule.Enabled: True
btnReloadModule.Enabled: True
```

---

## Testing Checklist

After applying fix:

- [ ] Rebuild RCH.TemplateStorage
- [ ] Check module copied to Modules folder
- [ ] Run TheForge dashboard
- [ ] Verify log shows: "Discovered module: RCH.TemplateStorage (RCH.TemplateStorage.Modules.TemplateStorageModule)"
- [ ] Select module - Run button should be ENABLED
- [ ] Click Run - module should execute
- [ ] After execution - Run button should be DISABLED, Unload/Reload ENABLED

---

## Debug Commands

### Check if module has IModule methods:
```powershell
$dll = [System.Reflection.Assembly]::LoadFile("C:\...\Modules\RCH.TemplateStorage.dll")
$type = $dll.GetType("RCH.TemplateStorage.Modules.TemplateStorageModule")
$type.GetMethods() | Where-Object { $_.Name -in @("Initialize","Execute","OnUnload","LoadConfiguration") }
```

Should show all 4 methods if duck typing is correct.

---

**Status:** Solution 1 implemented, needs rebuild and test  
**Next:** Rebuild and verify "Run Module" button enables
