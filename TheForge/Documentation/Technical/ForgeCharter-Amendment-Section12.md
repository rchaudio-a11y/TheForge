# ForgeCharter Section 12 Amendment Block

**Document Type:** Amendment Block  
**Created:** 2025-01-02  
**Character Count:** TBD  
**Status:** Ready for Insertion  
**Insert Location:** ForgeCharter.md (after Section 11)

---

## Amendment Text (Insert as Section 12)

```markdown
# 12. Reusable Control Versioning Governance

## 12.1 Semantic Versioning Required
All reusable UI controls MUST follow semantic versioning (MAJOR.MINOR.PATCH):
- MAJOR: Breaking changes (incompatible API)
- MINOR: New features (backward-compatible)
- PATCH: Bug fixes (backward-compatible)

## 12.2 Version Metadata Required
Every reusable control MUST include:

**In code:**
```vb
Public Const Version As String = "1.2.0"
```

**In documentation (Codex):**
- Version history
- API documentation
- Compatibility matrix

## 12.3 Breaking Change Management
Breaking changes MUST:
- Increment MAJOR version
- Be deprecated for at least one MINOR version
- Include ObsoleteAttribute with migration path
- Be documented in Chronicle

## 12.4 Forge Enforcement
Forge MUST:
- Prompt for version on control creation (default: 1.0.0)
- Detect breaking changes (require MAJOR bump)
- Update version metadata in code
- Update changelog
- Generate API documentation

ForgeAudit MUST verify:
- Version metadata present
- SemVer format followed
- Changelog current
- Breaking changes properly versioned
- Deprecated members have ObsoleteAttribute

## 12.5 Branch Responsibilities
- **Coding Branch:** Version metadata, ObsoleteAttribute, breaking change detection
- **Architecture Branch:** Documentation folder structure, compatibility matrix
- **Documentation Branch:** Changelog format, API documentation
- **Audit Branch:** Version compliance validation

## 12.6 Scope
Applies to:
- UserControls for reuse across projects
- Custom controls with external dependencies
- Control libraries/packages

Does NOT apply to:
- Project-specific controls (not reusable)
- Internal helper controls
- One-time-use controls

## 12.7 Non-Override
Forge MUST NOT:
- Auto-increment versions without user confirmation
- Modify version numbers implicitly
- Remove version metadata
- Override user version decisions
```

---

## End of Amendment Block
