# RCH UI Forge — Scriptorium Engine  
**Document Type:** Scriptorium  
**Purpose:** Automatically generate or update project documentation  
**Last Updated:** 2025-12-31  
**Related Documents:** file4.md (Documentation Taxonomy), file5.md (Configuration Rules)

---

## Overview
The Scriptorium Engine is responsible for generating and updating documentation inside any RCH UI Forge project. It does not define folder structures or project configuration. It operates only within the documentation taxonomy defined in file4.md.

---

## Global Rules
- Never duplicate content across documents.
- Update existing documents instead of creating new ones.
- Merge overlapping documents into a single authoritative source.
- All documents must be Markdown (.md).
- All documents must begin with:
  - Document type  
  - Purpose  
  - Last updated date  
  - Related documents  

---

## Documentation Types
Use the correct prompt for each document type:

### **Codex Prompt**
Technical, structured, authoritative.  
Include: purpose, specification, properties, events, usage, examples.

### **Chronicle Prompt**
Chronological, factual, archival.  
Include: version, date, changes, rationale, impact.

### **Tome Prompt**
Instructional, clear, beginner-friendly.  
Include: overview, steps, examples, best practices.

### **Lore Prompt**
Philosophical, conceptual, guild-like.  
Include: principles, naming conventions, design rules.

### **Grimoire Prompt**
Deep, internal, advanced.  
Include: experimental notes, risks, research, future ideas.

### **Scriptorium Prompt**
Template-oriented.  
Include: placeholders, required sections, formatting rules.

---

## Tasks
1. Analyze the project’s purpose and contents.  
2. Determine which documentation types are needed.  
3. Generate or update documents accordingly.  
4. Output:
   - List of required documents  
   - Content of each document  
   - Correct folder path  
   - Notes on updates vs new documents  
   - Cross-references  

---

## Scope
- This file does not define folder structures.  
- This file does not define project configuration.  
- This file only generates documentation inside the existing structure.

Refer to **file4.md** for the authoritative documentation taxonomy.