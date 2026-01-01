# Source/Services

**Purpose:** Non-UI logic that supports the Dashboard.

**Rules:**
- Services must be stateless when possible
- All dependencies must be injected, not created ad-hoc
- Services must be independently testable
- No UI references in services
- Services must expose explicit interfaces

**Examples:**
- ModuleLoaderService.vb
- LoggingService.vb
- DashboardStateService.vb

**Cross-references:**
- See /UI for presentation layer
- See File6.md for Architecture Discipline
