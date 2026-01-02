# Creating a Test Module for RCH.Forge.Dashboard

## Overview
The Dashboard is now fully functional and ready to load modules. To test it, you need to create a module DLL that implements the `IModule` interface.

## IModule Contract

```vb
Namespace Modules.Interfaces
    Public Interface IModule
        Sub Initialize(loggingService As Services.Interfaces.ILoggingService)
        Sub Execute()
    End Interface
End Namespace
```

## Steps to Create a Test Module

### 1. Create a New Class Library Project

```bash
dotnet new classlib -n SampleForgeModule -lang VB -f net8.0
```

### 2. Add Reference to Dashboard Interfaces

In the `SampleForgeModule.vbproj`, add:

```xml
<ItemGroup>
  <Reference Include="RCH.Forge.Dashboard">
    <HintPath>..\TheForge\bin\Debug\net8.0-windows\RCH.Forge.Dashboard.dll</HintPath>
  </Reference>
</ItemGroup>
```

### 3. Implement IModule

Create `SampleModule.vb`:

```vb
Imports TheForge.Modules.Interfaces
Imports TheForge.Services.Interfaces

Public Class SampleModule
    Implements IModule

    Private _loggingService As ILoggingService

    Public Sub Initialize(loggingService As ILoggingService) Implements IModule.Initialize
        _loggingService = loggingService
        _loggingService.LogInfo("SampleModule initialized")
    End Sub

    Public Sub Execute() Implements IModule.Execute
        _loggingService.LogInfo("SampleModule executing...")
        _loggingService.LogInfo("Hello from the Forge!")
        _loggingService.LogInfo("SampleModule execution complete")
    End Sub
End Class
```

### 4. Build the Module

```bash
dotnet build SampleForgeModule
```

### 5. Deploy to Dashboard

Copy the resulting `SampleForgeModule.dll` to:
```
TheForge\bin\Debug\net8.0-windows\Modules\
```

### 6. Run the Dashboard

1. Start the Dashboard
2. The module should appear in the left panel
3. Select it
4. Click "Run Module"
5. Watch the log output in the bottom panel

## Expected Log Output

```
[INFO] 2025-01-02 12:00:00 - Dashboard started
[INFO] 2025-01-02 12:00:00 - Discovering modules...
[INFO] 2025-01-02 12:00:00 - Modules directory created
[INFO] 2025-01-02 12:00:00 - Found 1 DLL file(s) in Modules directory
[INFO] 2025-01-02 12:00:00 - Discovered module: SampleForgeModule (SampleForgeModule.SampleModule)
[INFO] 2025-01-02 12:00:00 - Discovered 1 module(s)
[INFO] 2025-01-02 12:00:01 - Loading module: SampleForgeModule
[INFO] 2025-01-02 12:00:01 - Loading assembly from: C:\...\Modules\SampleForgeModule.dll
[INFO] 2025-01-02 12:00:01 - Instantiating module type: SampleForgeModule.SampleModule
[INFO] 2025-01-02 12:00:01 - Module loaded successfully: SampleForgeModule.SampleModule
[INFO] 2025-01-02 12:00:01 - Initializing module: SampleForgeModule
[INFO] 2025-01-02 12:00:01 - SampleModule initialized
[INFO] 2025-01-02 12:00:01 - Executing module: SampleForgeModule
[INFO] 2025-01-02 12:00:01 - SampleModule executing...
[INFO] 2025-01-02 12:00:01 - Hello from the Forge!
[INFO] 2025-01-02 12:00:01 - SampleModule execution complete
[INFO] 2025-01-02 12:00:01 - Module execution completed: SampleForgeModule
```

## Troubleshooting

**No modules discovered:**
- Verify the Modules directory exists: `TheForge\bin\Debug\net8.0-windows\Modules\`
- Verify the DLL is present
- Check the log output for errors

**Module doesn't implement IModule:**
- Ensure your module implements `TheForge.Modules.Interfaces.IModule`
- Verify the namespace matches: `TheForge.Modules.Interfaces`
- Ensure you've added a reference to the Dashboard DLL

**Assembly loading errors:**
- Ensure the module targets the same .NET version (net8.0)
- Check for missing dependencies
- Review error messages in the log output

---

**The Dashboard is ready. Create your first module to see it in action!**
