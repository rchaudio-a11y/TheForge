# RCH UI Forge — Prime Directives  
**Document Type:** Lore  
**Purpose:** Define universal rules that apply to every RCH UI Forge project  
**Last Updated:** 2025-12-31  
**Related Documents:** file2.md, file4.md, file5.md

---

## Overview
These Prime Directives represent the universal principles that govern all RCH UI Forge projects.  
They define the philosophy, discipline, and expectations that ensure consistency, clarity, and maintainability across the entire ecosystem.

These rules apply to **every** project, regardless of domain, language, UI, or purpose.

---

## 1. Determinism
All systems must behave predictably and consistently.

- No hidden state  
- No implicit behavior  
- No magic strings or magic numbers  
- All behavior must be explicit and discoverable  
- All modules must be independently testable  
- UI must be deterministic and predictable  

---

## 2. Modularity
All components must be self-contained and reusable.

- Each module must have a single, clear purpose  
- Modules must not depend on UI  
- Modules must expose explicit, documented interfaces  
- Modules must not leak internal state  
- Modules must be reusable across projects  
- No circular dependencies  

---

## 3. Onboarding-First Design
Every project must be understandable within minutes.

- Every folder must contain a README.md  
- Every module must include a “How to Use” section  
- Every public class must include a summary of purpose  
- Every project must include a quickstart guide  
- Documentation must reduce cognitive load  

---

## 4. Naming Canon (Global)
Names must reflect purpose and eliminate ambiguity.

- No abbreviations unless industry-standard  
- No overloaded or vague names  
- No “Helper”, “Manager”, “Utils”, or similar catch-all terms  
- Names must be explicit, descriptive, and deterministic  
- Forge-themed naming is encouraged where appropriate  

---

## 5. Architecture Discipline
Architectural clarity is mandatory.

- UI layer must be thin  
- Services must be stateless when possible  
- State must be centralized and explicit  
- All dependencies must be injected, not created ad-hoc  
- No business logic in event handlers  
- No implicit side effects  

---

## 6. Documentation Philosophy
Documentation must serve maintainers and future contributors.

- Documentation must be updated with every change  
- Documentation must be concise, structured, and Forge-themed  
- Documentation must never contradict the code  
- Documentation must explain *why*, not just *what*  
- Documentation must reduce cognitive load  

---

## 7. Code Quality Standards
All code must be clean, readable, and intentional.

- No unused code  
- No commented-out code  
- No TODOs without owners  
- No warnings allowed  
- All public APIs must be documented  
- No horizontal scrolling required to read code  

---

## 8. Versioning & Change Discipline
Every change must be traceable and justified.

- All changes must be logged in Chronicle  
- Every release must include rationale  
- Breaking changes must be documented in advance  
- Deprecated features must move to `/Chronicle/DeprecatedArchive`  

---

## 9. UX/UI Consistency
User interfaces must be predictable and stable.

- All controls must have explicit names  
- No default control names  
- Layout must be grid-aligned  
- UI must not flicker, jump, or resize unexpectedly  
- UI must not contain hidden or implicit behaviors  

---

## 10. Forge Ethos
The guiding philosophy of the RCH UI Forge ecosystem.

- Build for clarity  
- Build for maintainers  
- Build for reuse  
- Build for future-proofing  
- Build for onboarding  
- Build for determinism  
- Build for elegance  

---

## Summary
These Prime Directives define the universal expectations for all RCH UI Forge projects.  
They ensure that every project is consistent, maintainable, and aligned with the Forge ethos.
