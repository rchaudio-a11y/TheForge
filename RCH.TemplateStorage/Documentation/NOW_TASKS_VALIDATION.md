# NOW Tasks - Validation Guide

**Status:** ? COMPLETE  
**Date:** 2026-01-05  
**Time to Complete:** < 5 minutes

---

## What Was Done

### ? Task 1: Post-Build Copy Implemented

**File Modified:** `RCH.TemplateStorage\RCH.TemplateStorage.vbproj`

**What it does:**
- Automatically copies `RCH.TemplateStorage.dll` to `TheForge\bin\Debug\net8.0-windows\Modules\`
- Copies required dependencies (Newtonsoft.Json, System.Data.OleDb)
- Creates Modules directory if missing
- Shows confirmation message after build

**Result:** RCH.TemplateStorage is now discoverable by TheForge!

---

### ? Task 2: ModuleLoaderService Restored

**File Restored:** `TheForge\Source\Services\Implementations\ModuleLoaderService.vb`

**Action Taken:** `git checkout HEAD -- TheForge/Source/Services/Implementations/ModuleLoaderService.vb`

**Result:** 
- ModuleLoaderService back to working state
- All interface methods present
- Build successful with no errors

---

## How to Test NOW

### Step 1: Rebuild Solution

```powershell
# In Visual Studio
Build ? Rebuild Solution

# Or from command line
dotnet build TheForge.sln
```

**Expected Output:**
```
Build succeeded.
...
Copied RCH.TemplateStorage.dll to TheForge Modules directory
```

---

### Step 2: Verify Module Copied

Check that this file exists:
```
TheForge\bin\Debug\net8.0-windows\Modules\RCH.TemplateStorage.dll
```

**PowerShell Check:**
```powershell
Test-Path "TheForge\bin\Debug\net8.0-windows\Modules\RCH.TemplateStorage.dll"
# Should return: True
```

---

### Step 3: Run TheForge Dashboard

**From Visual Studio:**
1. Set TheForge as startup project
2. Press F5 (Start Debugging)

**Expected Behavior:**
1. Dashboard opens with three panels
2. Log shows: "Module Loader Service initialized"
3. Log shows: "Found X DLL file(s) in Modules directory"
4. Log shows: "Discovered module: RCH.TemplateStorage" or similar

---

### Step 4: Verify Module Discovery

**In Dashboard UI:**
1. Look at **Module List** panel (left side)
2. You should see: "TemplateStorage" or "RCH.TemplateStorage"

**In Log Output:**
```
Module Loader Service initialized
Found 2 DLL file(s) in Modules directory
Discovered module: SampleForgeModule (if present)
Discovered module: RCH.TemplateStorage
```

---

### Step 5: Load and Test Module

**In Dashboard:**
1. Select "RCH.TemplateStorage" from Module List
2. Click **Load** button
3. Watch Log Output

**Expected Log Messages:**
```
Loading assembly from: [path]\RCH.TemplateStorage.dll
Instantiating module type: RCH.TemplateStorage.Modules.TemplateStorageModule
Initializing module: RCH.TemplateStorage
Template Storage Module initializing...
Database: [path]\Templates.accdb
Template Storage Service initialized successfully
Module loaded successfully
```

---

### Step 6: Execute Module

**In Dashboard:**
1. With module selected, click **Execute** button

**Expected Log Output:**
```
Executing Template Storage Module...
=== Template Storage Module ===
Database: [path]\Templates.accdb
Connection: OK
Schema: Valid
Templates: 0 (or count if any exist)
Module execution complete.
```

---

## Troubleshooting

### Issue: Module not copied after build

**Check:**
1. Build output window for copy message
2. Verify project built successfully
3. Check if Modules directory was created

**Fix:**
```powershell
# Manually create directory and copy
New-Item -Path "TheForge\bin\Debug\net8.0-windows\Modules" -ItemType Directory -Force
Copy-Item "RCH.TemplateStorage\bin\Debug\net8.0-windows\RCH.TemplateStorage.dll" -Destination "TheForge\bin\Debug\net8.0-windows\Modules\"
```

---

### Issue: Module discovered but won't load

**Check Log for:**
- "No type implementing IModule found"
- "Failed to cast module instance to IModule"

**Likely Cause:** TemplateStorageModule doesn't properly implement IModule

**Fix:** Check that TemplateStorageModule.vb has correct interface signature

---

### Issue: ADOX not available error

**Error Message:** "ADOX not available. Install MS Access Database Engine."

**Solution:**
1. Install MS Access Database Engine: https://www.microsoft.com/download/details.aspx?id=54920
2. Choose 32-bit or 64-bit to match your runtime
3. Restart Visual Studio after installation

**Workaround:**
Module will still load, but database creation will fail. Use existing database file.

---

### Issue: Database file locked

**Error:** "Database is in use by another process"

**Fix:**
1. Close all Access instances
2. Delete lock file: `Templates.laccdb`
3. Restart TheForge

---

## Success Criteria

You can declare success when:

? Build shows "Copied RCH.TemplateStorage.dll to TheForge Modules directory"  
? RCH.TemplateStorage appears in Module List  
? Module loads without errors  
? Module executes and shows "Connection: OK"  
? Log shows template count and statistics  

---

## What You Can Test Now

With module loaded in dashboard, you can:

1. **View Module Metadata**
   - Name: "Template Storage Service"
   - Version: "1.0.0"
   - Author: "RCH Audio"
   - Category: "Data Management"

2. **Execute Module**
   - Shows database status
   - Lists templates (if any)
   - Shows categories and tags

3. **Test Integration**
   - Load/unload module multiple times
   - Check for memory leaks
   - Verify logging works

4. **Access Service Directly (Advanced)**
   ```vb
   ' In Test Area Control or custom code
   Dim module = LoadedModules.Find("TemplateStorageModule")
   Dim service = DirectCast(module, TemplateStorageModule).Service
   
   ' Now use service directly
   Dim templates = service.GetAllTemplates()
   ```

---

## Next Steps (SOON)

After validating basic discovery works:

1. **Test with multiple modules**
   - Add RCHAutomation.Controls modules
   - Verify multi-module loading
   - Test module isolation

2. **Implement proper discovery (Option 2)**
   - Remove post-build hack
   - Add recursive scanning
   - Add solution root detection

3. **Add discovery tests**
   - Unit tests for scanning
   - Unit tests for filtering
   - Integration tests for loading

---

## Files Modified

| File | Change | Status |
|------|--------|--------|
| RCH.TemplateStorage.vbproj | Added post-build event | ? Complete |
| ModuleLoaderService.vb | Restored from git | ? Complete |

---

## Build Status

? **Build Successful**  
? **No Errors**  
? **No Warnings**  
? **Post-build event working**

---

## Time Investment

- Task 1 (Post-build): 2 minutes
- Task 2 (Git restore): 1 minute
- Validation: 5 minutes
- **Total: < 10 minutes**

---

**Status:** Ready for immediate testing  
**Next Action:** Rebuild solution and run TheForge dashboard  
**Blocker:** None - all NOW tasks complete
