# RCH UI Forge — Documentation Taxonomy  
**Document Type:** Lore  
**Purpose:** Define the authoritative documentation structure and rules  
**Last Updated:** 2025-12-31  
**Related Documents:** file1.md (Scriptorium Engine), file5.md (Configuration Rules)

---

## Overview
This document defines the complete documentation taxonomy for all RCH UI Forge projects.  
Every project must follow this structure exactly.  
All documentation must be Markdown (.md), deterministic, modular, and non-redundant.

This file is the **single source of truth** for documentation structure.

---

## Folder Structure
/Documentation
    /Codex
        /Controls
        /Layouts
        /Events
        /Schema
        /Glossary
    /Chronicle
        /ReleaseNotes
        /VersionHistory
        /AuditLedger
        /DeprecatedArchive
    /Tomes
        /UserGuide
        /GettingStarted
        /Tutorials
        /QuickReference
    /Lore
        /DesignPhilosophy
        /NamingCanon
        /UIPrecepts
        /ForgeEthos
    /Grimoire
        /Experimental
        /InternalNotes
        /Arcana
        /Research
    /Scriptorium
        /Templates
        /Drafts
        /Generated

---

## Folder Purposes & Rules

### **Codex**
**Purpose:** Developer references and technical specifications.  
**Rules:**  
- Contains API docs, schemas, control definitions, and technical references.  
- No logs, philosophy, or onboarding content.  
- One topic per file.  

**Examples:**  
- ControlDefinition.Button.md  
- LayoutSchema.Grid.md  
- EventReference.ModuleLoaded.md  

---

### **Chronicle**
**Purpose:** Chronological records of project evolution.  
**Rules:**  
- Contains logs, release notes, audits, and version history.  
- All entries must be timestamped.  
- No technical specifications or tutorials.  

**Examples:**  
- VersionHistory.chronicle.md  
- ReleaseNotes.2025-12-31.md  
- AuditLedger.2025-Q4.md  

---

### **Tomes**
**Purpose:** Instructional and onboarding content.  
**Rules:**  
- Must be beginner-friendly.  
- Contains guides, tutorials, walkthroughs, and quick references.  
- No internal or experimental notes.  

**Examples:**  
- GettingStarted.md  
- UserGuide.Dashboard.md  
- Tutorial.CreatingYourFirstModule.md  

---

### **Lore**
**Purpose:** Philosophy, naming, style, and design principles.  
**Rules:**  
- Contains conceptual and stylistic guidance.  
- No code, no logs, no technical specs.  
- Defines the “Forge ethos.”  

**Examples:**  
- NamingCanon.md  
- DesignPhilosophy.md  
- UIPrecepts.md  

---

### **Grimoire**
**Purpose:** Advanced, internal, or experimental knowledge.  
**Rules:**  
- Not intended for onboarding.  
- Contains research, prototypes, risks, and future ideas.  
- May include incomplete or speculative content.  

**Examples:**  
- Experimental.ModuleHotSwap.md  
- Arcana.RenderingPipeline.md  
- Research.AsyncPatterns.md  

---

### **Scriptorium**
**Purpose:** Templates and generated documentation.  
**Rules:**  
- Contains templates, drafts, and auto-generated files.  
- Never hand-edit generated files.  
- Must remain clean and machine-friendly.  

**Examples:**  
- Template.ControlDefinition.md  
- Draft.ModuleManifest.md  
- Generated.APIReference.md  

---

## README.md Requirements
Each folder listed above must contain a README.md describing:

- **Purpose**  
- **Rules**  
- **Examples of valid documents**  
- **Cross-references** (e.g., “See /Lore/NamingCanon.md for naming rules”)  

---

## Summary
This taxonomy defines the structure, purpose, and rules for all documentation in the RCH UI Forge ecosystem.  
All other documentation tools (including the Scriptorium Engine) must reference this file as the authoritative source.