# TheForge Project Constitution
**Document Type:** Constitution  
**Version:** 1.0.0  
**Created:** 2025-01-18  
**Status:** Draft - Awaiting Validation  
$118904

---

## Executive Summary

**TheForge** (RCH.Forge.Dashboard) is a WinForms-based modular orchestration platform and test harness for the RCH UI Forge ecosystem. This constitution establishes the foundational principles, constraints, and architectural decisions that govern all development within this project.

---

## 1. Project Identity

### 1.1 Mission Statement
To provide a deterministic, maintainable, and extensible platform for developing, testing, and orchestrating modular VB.NET components within a governed architectural framework that prioritizes clarity, stability, and developer experience.

### 1.2 Core Values
1. **Determinism Over Convenience** - Explicit, predictable behavior always
2. **Documentation-First** - Reduce cognitive load through comprehensive guidance
3. **Modularity & Extensibility** - Self-contained, composable components
4. **Naming Canon** - Names reflect purpose and eliminate ambiguity
5. **Separation of Concerns** - Clear architectural boundaries
6. **Maintainer-Centric Design** - Code serves future developers
7. **Quality Without Compromise** - Clean, intentional, warning-free code
8. **Traceability** - Every change is documented and versioned
9. **UI Consistency** - Predictable, stable user interfaces
10. **Forge Ethos** - Build for clarity, reuse, and elegance

### 1.3 Project Scope

**In Scope:**
- Modular plugin system for VB.NET components
- Service-based architecture (logging, module loading, configuration)
- WinForms dashboard with three-panel layout
- Module discovery, loading, and lifecycle management
- Configuration-driven module behavior (file-based, with future Access database support)
- Comprehensive documentation system
- Optional Microsoft Access database for logging and configuration persistence

**Out of Scope:**
- Web-based interfaces
- Cross-platform support (Windows-only)
- Real-time collaboration features
- Cloud deployment (on-premises/local deployment only)
- Enterprise database systems (SQL Server, PostgreSQL, etc.)

---

## 2. Technology Stack & Constraints

### 2.1 Core Technologies

| Technology | Version | Justification |
|------------|---------|---------------|
| .NET | 8.0 (net8.0-windows) | Latest LTS, modern language features |
| Language | VB.NET | Legacy compatibility, team expertise |
| UI Framework | Windows Forms | Desktop-focused, mature, deterministic |
| Target OS | Windows 10+ | Enterprise desktop environment |
| Data Store (Future) | Microsoft Access 2007+ (.accdb) | Lightweight, single-file, xcopy deployment, Windows-integrated |

### 2.2 Language Constraints

**VB.NET Configuration (Non-Negotiable):**
```xml
<OptionStrict>On</OptionStrict>
<OptionExplicit>On</OptionExplicit>
<OptionInfer>Off</OptionInfer>
```

**Rationale:**
- `Option Strict On` - Prevent implicit type conversions
- `Option Explicit On` - All variables must be declared
- `Option Infer Off` - Explicit type declarations required

### 2.3 Architecture Constraints

**Project Structure (Fixed):**
```
/Documentation/        ? All project documentation (taxonomy-governed)
  /Codex/             ? Technical references
  /Chronicle/         ? Version history
  /Tomes/             ? User guides
  /Lore/              ? Design philosophy
  /Grimoire/          ? Experimental features
  /Scriptorium/       ? Templates and generated docs
/Source/              ? All source code
  /Core/              ? Core abstractions
  /Models/            ? Data models
  /Modules/           ? Module interfaces and implementations
    /Interfaces/      ? Module contracts (IModule, IModuleConfiguration)
  /Services/          ? Business logic
    /Interfaces/      ? Service contracts
    /Implementations/ ? Service implementations
  /UI/                ? All UI code
    /Controls/        ? User controls
/Resources/           ? Static assets
  /Images/            ? Icons and UI assets
  /Data/              ? Configuration files and Access databases
/Prompts/             ? Forge governance files
```

### 2.4 Data Persistence Strategy

**Current Implementation:**
- File-based configuration (`.config` files)
- Text-based logging (via ILoggingService)
- No database dependencies

**Future Enhancement (Planned):**
- Microsoft Access database support for:
  - Centralized configuration management
  - Structured logging and diagnostics
  - Module activity tracking
  - Historical data retention
- Database location: `/Resources/Data/` directory
- Access format: `.accdb` (Access 2007+ format)
- Fallback: File-based configuration remains if database unavailable

**Database Design Principles:**
- Optional, not required for core functionality
- Graceful degradation to file-based if Access not available
- Single-file database (portable, xcopy deployment)
- No external database server dependencies
- Windows-integrated security (no separate credentials)

### 2.5 Naming Convention (Canonical)

**Assembly Names:**
- Format: `RCH.Forge.[ComponentName]`
- Example: `RCH.Forge.Dashboard`

**Namespace Convention:**
- Root: `TheForge`
- Structure: `TheForge.[Layer].[Sublayer]`
- Examples:
  - `TheForge.Services.Interfaces`
  - `TheForge.Modules.Interfaces`
  - `TheForge.UI.Controls`

**File Naming:**
- Descriptive, purpose-driven names
- No default names (Form1, Button1, UserControl1, etc.)
- Match class/interface names exactly
- Designer files: `[ClassName].Designer.vb`
- Access databases: `ForgeData.accdb`, `ForgeLog.accdb` (descriptive, purpose-driven)

**Forbidden Names:**
- Helper, Manager, Utils, Handler (unless contextually justified)
- Generic suffixes without context (Base, Common, Generic)

---

## 3. Architectural Principles

### 3.1 Layered Architecture

**Dependency Flow (Strict):**
```
UI Layer ? Services Layer ? Core/Models Layer
         ?                  ?
         Modules (external, loaded at runtime)
```

**Rules:**
- UI layer may only reference Services and Core
- Services may only reference Core and Models
- Core has no dependencies (foundational contracts)
- Modules may reference Services and Core through interfaces only

### 3.2 Modularity Contract

**Module Lifecycle:**
1. Discovery (filesystem scan)
2. Load (assembly load, type discovery)
3. Initialize (service injection)
4. Configure (optional configuration load)
5. Execute (primary functionality)
6. OnUnload (cleanup)
7. Dispose (resource release)

**Module Requirements:**
- Must implement `IModule` interface
- Must be disposable (`IDisposable`)
- Must accept service dependencies via Initialize()
- Must support optional configuration via LoadConfiguration()
- Must be self-contained (no shared state)

### 3.3 Service-Oriented Design

**Current Services:**
- `ILoggingService` - Centralized logging and diagnostics
- `IModuleLoaderService` - Module discovery and lifecycle management

**Service Characteristics:**
- Interface-first design
- Injected into modules and UI components
- Stateless where possible
- Thread-safe by default

### 3.4 Designer File Governance (Critical)

**Separation of Responsibilities:**
- `.Designer.vb` files - Control declarations, layout, initialization (AI-editable)
- `.vb` files - User logic, event handlers, business logic (human-owned)

**AI Behavior Rules:**
- AI modifies Designer files directly
- AI provides code blocks for main files (user inserts manually)
  - **Applies only in Visual Studio IDE environment**
  - **Only for Designer-based files** (Forms, UserControls with `.Designer.vb` counterparts)
  - **Does not apply** to GitHub, VS Code, or non-Designer files
- Never open Visual Studio Designer UI programmatically (file locking risk)
- Maintain strict separation at all times

**Context-Specific Behavior:**
- **In Visual Studio with Designer files:** AI outputs code as text, user inserts manually
- **In GitHub/VS Code/other editors:** AI can edit files directly (no Designer lock risk)
  - **Must maintain cross-platform IDE compatibility:**
    - Update `.sln` and `.slnx` solution files when projects added/removed
    - Update `.vbproj` project files when files added/removed or references changed
    - Update `.Designer.vb` files when controls added/modified
    - Update `.resx` resource files when UI resources changed
    - Ensure all project structure changes are reflected in project/solution files
- **Non-Designer VB files:** AI edits directly regardless of environment

### 3.5 Cross-Platform IDE Compatibility (Critical)

**Objective:**
Maintain compatibility across Visual Studio, Visual Studio Code, GitHub Codespaces, and other .NET-compatible editors.

**File Synchronization Requirements:**

**When adding/removing files, AI must update:**
1. **Project File (`.vbproj`)** - Add/remove `<Compile>`, `<EmbeddedResource>`, or `<None>` entries
2. **Solution File (`.sln`)** - Update project references if project added/removed
3. **Solution XML (`.slnx`)** - Keep synchronized with `.sln` for cross-platform IDE support

**When modifying Designer-based UI components, AI must update:**
1. **Designer File (`.Designer.vb`)** - Control declarations, initialization, layout
2. **Resource File (`.resx`)** - Embedded resources, strings, images
3. **Project File (`.vbproj`)** - Ensure proper `<DependentUpon>` relationships

**When adding/removing project references, AI must update:**
1. **Project File (`.vbproj`)** - Add/remove `<ProjectReference>` or `<Reference>` entries
2. **Solution File (`.sln`)** - Update project dependency hierarchy if needed

**File Relationship Patterns (Must be maintained):**

```xml
<!-- Code files -->
<Compile Include="Source\UI\DashboardMainForm.vb">
  <SubType>Form</SubType>
</Compile>
<Compile Include="Source\UI\DashboardMainForm.Designer.vb">
  <DependentUpon>DashboardMainForm.vb</DependentUpon>
</Compile>

<!-- Resource files -->
<EmbeddedResource Include="Source\UI\DashboardMainForm.resx">
  <DependentUpon>DashboardMainForm.vb</DependentUpon>
</EmbeddedResource>
```

**Validation Requirements:**
- After any structural change, verify project builds in multiple editors
- Ensure `.sln` and `.slnx` remain synchronized
- Validate `<DependentUpon>` relationships are correct
- Confirm resource files are properly embedded

**Common Compatibility Issues to Avoid:**
- ? Adding files to filesystem without updating `.vbproj`
- ? Modifying `.sln` without updating `.slnx` (or vice versa)
- ? Breaking `<DependentUpon>` relationships (Designer/resource files orphaned)
- ? Adding project references without updating solution dependencies
- ? Changing file paths without updating all referencing files

---

## 4. Code Quality Standards

### 4.1 Compilation Requirements
- Zero warnings on build (treat warnings as errors in spirit)
- All Option Strict/Explicit/Infer rules enforced
- No unused imports
- No unreachable code

**Build Verification Requirements:**
- **Code changes:** Build verification required after modifications
- **Documentation changes:** Build verification not required (documentation-only changes don't affect compilation)
- **Mixed changes:** If both code and documentation modified, build verification required

### 4.2 Documentation Standards
- XML documentation comments for all public interfaces
- Summary tags required for all public methods
- Parameter descriptions required
- Return value descriptions required

### 4.3 Error Handling
- No silent exception swallowing
- Logging required for all errors
- Graceful degradation where possible
- User-friendly error messages in UI

### 4.4 Testing Philosophy
- Dashboard serves as test harness
- Modules tested in isolation
- Integration testing via UI
- No automated test suite (manual testing via dashboard)

---

## 5. Governance System

### 5.1 Forge Charter Integration

This constitution integrates with the existing **ForgeCharter** governance system:

**Precedence Hierarchy:**
1. **CONSTITUTION.md** (this document) - Project-level principles
2. **ForgeCharter.md** - Universal Forge rules
3. **Branch Files** (Coding, Architecture, Documentation)
4. **Audit Branch** - Compliance evaluation

**Integration Rules:**
- Constitution defines "what and why"
- ForgeCharter defines "how and when"
- Branch files define domain-specific rules
- No conflicts permitted (constitution wins)

### 5.2 Change Management

**Constitution Amendments:**
- Require explicit user approval
- Increment version (semantic versioning)
- Document in Amendment Block (Section 8)
- Update character count
- Review ForgeCharter impact

**Code Changes:**
- Follow Coding Branch rules
- Maintain architectural boundaries
- Update documentation accordingly
- Update VersionHistory.chronicle.md

### 5.3 Drift Prevention

**Pre-Flight Checks (Required):**
- Verify alignment with constitution principles
- Check naming canon compliance
- Validate architectural boundaries
- Review documentation requirements
- Confirm no rule conflicts

---

## 6. Development Workflow

### 6.1 Standard Development Cycle

1. **Specification** - Define feature/change using Spec-Kit
2. **Architecture Review** - Validate against constitution
3. **Implementation** - Follow Coding Branch rules
4. **Documentation** - Update relevant documentation
5. **Testing** - Manual testing via dashboard
6. **Versioning** - Update VersionHistory.chronicle.md
7. **Build Verification** - Ensure clean build (required for code changes only, skip for documentation-only changes)

### 6.2 Module Development Workflow

1. Create new class library project
2. Implement `IModule` interface
3. Add logging via `ILoggingService`
4. Create `.config` file (if configuration needed)
5. Test via Dashboard module loader
6. Document in module-specific documentation

### 6.3 UI Development Workflow

1. Create control in `/Source/UI/Controls/`
2. Use descriptive naming (no default names)
3. Separate Designer and logic concerns
4. Inject services via constructor or properties
5. Test in Dashboard TestAreaControl
6. Document control purpose and usage

### 6.4 Documentation Generation Rules

Documentation is generated using a tiered model:

1. Script-Generated (mechanical, templated)
2. AI-Generated (structured reasoning)
3. Human-Crafted (governance and philosophy)
4. Hybrid (script + AI + human)

All documentation must follow:
- The 5W&H rule for event and error logs
- The Documentation Taxonomy
- Metadata requirements
- Validation workflows
---

## 7. Documentation Requirements

### 7.1 Mandatory Documentation

**Every Feature Must Have:**
- Entry in ForgeTome.md (if user-facing)
- Technical reference in Codex (if API/interface)
- Version history entry (VersionHistory.chronicle.md)

**Every File Must Have:**
- Metadata header with character count
- Document type, created date, last updated date
- Related files references

### 7.2 Documentation Taxonomy (Enforced)

| Category | Purpose | Examples |
|----------|---------|----------|
| Codex | Technical references | API docs, interface specs |
| Chronicle | Version history | VersionHistory.chronicle.md |
| Tomes | User guides | ForgeTome.md, tutorials |
| Lore | Design philosophy | NamingCanon.md, principles |
| Grimoire | Experimental | Research, prototypes |
| Scriptorium | Templates | Generated docs, scaffolds |

### 7.3 Character Count Enforcement

**All files must include:**
```
' **Character Count:** TBD
```

**Rules:**
- Field must be present in all Forge-managed files
- Must be updated after every edit
- Never approximate or estimate
- Never leave as TBD after modification

---

## 8. Amendment Block

### Version 1.0.0 - 2025-01-18
- Initial constitution created
- Integrated with ForgeCharter governance
- Established core principles and constraints
- Defined architecture and technology stack
- Set code quality and documentation standards
- Added data persistence strategy (file-based with future Access database support)
- Clarified scope boundaries for database technology (Access, not enterprise RDBMS)
- Clarified Designer file governance (Visual Studio vs GitHub/VS Code context)
- Added cross-platform IDE compatibility requirements (Section 3.5)
- Added build verification efficiency rule (documentation-only changes don't require build)

---

## 9. Validation Checklist

Before accepting this constitution, verify:

- [ ] Mission statement aligns with project goals
- [ ] Technology stack matches current implementation
- [ ] Architecture constraints are accurate and enforceable
- [ ] Naming conventions match existing codebase
- [ ] Governance integration with ForgeCharter is clear
- [ ] Development workflows are practical and realistic
- [ ] Documentation requirements are achievable
- [ ] No conflicts with existing ForgeCharter rules
- [ ] Core values reflect team/project culture
- [ ] Scope boundaries are clearly defined

---

## 10. Open Questions for Validation

**Please review and confirm:**

1. **License & Contact Information** - Need to be specified in README.md
2. **Automated Testing Strategy** - Currently manual-only, is this acceptable?
3. **Module Distribution Model** - How are third-party modules distributed?
4. **Configuration Management** - Currently file-based; future Microsoft Access database support planned for configuration and logging
5. **Error Reporting Mechanism** - Currently logging-only, sufficient?
6. **UI Theme/Styling Governance** - Should standardized styling be enforced?
7. **Performance Requirements** - Any specific performance benchmarks?
8. **Security Considerations** - Module sandboxing, code signing requirements?
9. **Localization/Internationalization** - English-only or multi-language support?
10. **Backward Compatibility Policy** - How are breaking changes handled?

---

## 11. Next Steps After Validation

Once this constitution is validated and accepted:

1. Move from Draft to **Active** status
2. Update README.md with constitution reference
3. Create architecture decision records (ADRs) for major decisions
4. Establish review process for constitution amendments
5. Train team/contributors on constitutional principles
6. Integrate constitution checks into development workflow
7. Create constitution compliance audit in ForgeAudit.md

---

## Glossary

- **Forge** - The RCH UI ecosystem and governance framework
- **Module** - A self-contained component implementing IModule
- **Dashboard** - The main orchestration and test harness application
- **Charter** - ForgeCharter.md, the universal governance document
- **Branch** - Domain-specific rule files (Coding, Architecture, Documentation)
- **Constitution** - This document, project-level foundational principles
- **Drift** - Deviation from established rules and patterns
- **Canon** - Authoritative naming and structural conventions

---

**End of Constitution - Awaiting Validation**
