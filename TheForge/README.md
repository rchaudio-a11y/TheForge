# RCH.Forge.Dashboard

**A WinForms test harness and orchestration platform for the RCH UI Forge ecosystem.**

---

## Quick Start

1. Open the solution in Visual Studio 2022 or later
2. Build the solution (Ctrl+Shift+B)
3. Run the Dashboard (F5)

---

## Project Structure

```
/Documentation          — All project documentation (Codex, Chronicle, Tomes, Lore, Grimoire, Scriptorium)
/Source                 — All source code
  /UI                   — Forms and controls
  /Services             — Business logic and services
/Resources              — Static assets
  /Images               — Icons and UI assets
  /Data                 — Configuration and sample data
/Prompts                — Forge Orchestrator prompt files
```

---

## Documentation

All documentation follows the **RCH UI Forge Documentation Taxonomy** (file4.md).

**Start here:**
- [ForgeTome.md](Documentation/Tomes/ForgeTome.md) — Project overview and onboarding
- [NamingCanon.md](Documentation/Lore/NamingCanon.md) — Naming conventions
- [VersionHistory.chronicle.md](Documentation/Chronicle/VersionHistory.chronicle.md) — Change log

**Browse by category:**
- `/Documentation/Codex` — Technical references
- `/Documentation/Chronicle` — Version history and logs
- `/Documentation/Tomes` — User guides and tutorials
- `/Documentation/Lore` — Design philosophy and principles
- `/Documentation/Grimoire` — Experimental features and research
- `/Documentation/Scriptorium` — Templates and generated docs

---

## Standards & Configuration

This project follows:
- **file2.md** — Project File Layout Template
- **file3.md** — Dashboard Project Template
- **file4.md** — Documentation Taxonomy
- **file5.md** — Standard Configuration Rules
- **File6.md** — Forge Prime Directives

**Key standards:**
- VB.NET with Option Strict On, Option Explicit On, Option Infer Off
- RCH.Forge.[ModuleName] naming convention
- No default names (Form1, Button1, etc.)
- Clear separation of UI and Services
- Documentation-first approach

---

## Architecture

### UI Layer (`/Source/UI`)
- `DashboardMainForm.vb` — Main application window with three-panel layout:
  - **Left Panel:** Module list
  - **Bottom Panel:** Log output
  - **Center Panel:** Test area

### Services Layer (`/Source/Services`)
- `IModuleLoaderService` / `ModuleLoaderService` — Module discovery and loading
- `ILoggingService` / `LoggingService` — Centralized logging and diagnostics

---

## Forge Prime Directives

This project adheres to the Forge ethos:
1. **Determinism** — All behavior is explicit and predictable
2. **Modularity** — Components are self-contained and reusable
3. **Onboarding-First** — Documentation reduces cognitive load
4. **Naming Canon** — Names reflect purpose and eliminate ambiguity
5. **Architecture Discipline** — Clear separation of concerns
6. **Documentation Philosophy** — Documentation serves maintainers
7. **Code Quality** — Clean, readable, intentional code
8. **Versioning Discipline** — Every change is traceable
9. **UX/UI Consistency** — Predictable and stable interfaces
10. **Forge Ethos** — Build for clarity, maintainers, reuse, and elegance

---

## Contributing

Before making changes:
1. Review the **NamingCanon.md** for naming conventions
2. Review **File6.md** for Prime Directives
3. Update **VersionHistory.chronicle.md** with your changes
4. Ensure all code compiles without warnings

---

## Technology Stack

- **.NET 8.0** (net8.0-windows)
- **WinForms** (Windows Forms)
- **VB.NET** (Visual Basic .NET)

---

## License

[Specify license here]

---

## Contact

[Specify contact information here]

---

**Welcome to the Forge.**
