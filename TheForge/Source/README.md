# Source

**Purpose:** Contains all source code for the RCH.Forge.Dashboard project.

**Rules:**
- All code must follow Option Strict On, Option Explicit On, Option Infer Off
- No business logic in UI layer
- Services must be stateless when possible
- All classes must have explicit, descriptive names

**Structure:**
- `/UI` — Forms, controls, and UI-specific logic
- `/Services` — Non-UI logic supporting the Dashboard
- `/Modules` — Dashboard-specific modules or test harness components (optional)

**Cross-references:**
- See file3.md for Dashboard architecture rules
- See file5.md for coding standards
- See File6.md for Prime Directives
