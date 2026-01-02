# Source/UI

**Purpose:** Forms, controls, views, and UI-specific logic.

**Rules:**
- No business logic in UI layer
- All controls must have explicit names (no Form1, Button1, etc.)
- UI must be deterministic and predictable
- Delegate all logic to Services
- Layout must be grid-aligned

**Examples:**
- DashboardMainForm.vb
- ModuleListControl.vb
- LogPanelControl.vb

**Cross-references:**
- See /Services for business logic
- See /Documentation/Lore/UIPrecepts.md for UI design principles
