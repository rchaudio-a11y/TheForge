# Designer File Structure - Optimal Placement Rules

**Document Type:** Technical Reference  
**Created:** 2025-01-02  
**Purpose:** Prevent duplicate declarations in Designer files  
**Related:** file8.md section 7

---

## The Golden Rule

**Partial classes merge ALL declarations from both files.**

If you declare something in BOTH files, VB.NET throws errors:
- `BC30260` - "already declared"
- `BC30269` - "has multiple definitions"
- `BC30663` - "attribute cannot be applied multiple times"

**Solution:** Declare each element in EXACTLY ONE file.

---

## Quick Reference

| Element | .vb (Main) | .Designer.vb |
|---------|-----------|--------------|
| `<DesignerGenerated()>` attribute | ? NO | ? YES |
| `Partial Class` declaration | ? YES | ? YES |
| `Public Sub New()` | ? YES | ? NO |
| `InitializeComponent()` | ? NO | ? YES |
| `Dispose()` override | ? NO | ? YES |
| `Private components` | ? NO | ? YES |
| Control declarations | ? NO | ? YES |
| Event handlers (`Handles`) | ? YES | ? NO |
| Public/Private methods | ? YES | ? NO |

---

## Main File (.vb) - Business Logic Only

```vb
Namespace UI.Controls
    Partial Public Class MyControl
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
        End Sub

        ' Event handlers
        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            ' Business logic
        End Sub

        ' Public methods
        Public Sub LoadData(data As String)
            txtName.Text = data  ' Uses Designer's control
        End Sub
    End Class
End Namespace
```

---

## Designer File (.Designer.vb) - UI Only

```vb
Namespace UI.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class MyControl
        Inherits UserControl

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            ' Cleanup
        End Sub

        Private components As IContainer

        Private Sub InitializeComponent()
            Me.txtName = New TextBox()
            Me.txtName.Location = New Point(10, 10)
            Me.txtName.Size = New Size(200, 27)
            ' ... more UI code ...
        End Sub

        Friend WithEvents txtName As TextBox
        Friend WithEvents btnSave As Button
    End Class
End Namespace
```

---

## Conversion Checklist

**Main .vb file - REMOVE:**
- [ ] All control declarations (`Private txtName As TextBox`)
- [ ] Entire `InitializeComponent()` method
- [ ] `Dispose()` override (if present)
- [ ] `<DesignerGenerated()>` attribute (if present)

**Main .vb file - KEEP:**
- [ ] Change to `Partial Public Class`
- [ ] `Public Sub New()` with `InitializeComponent()` call
- [ ] All event handlers
- [ ] All public/private methods

**Designer .vb file - ADD:**
- [ ] `<DesignerGenerated()>` on `Partial Class`
- [ ] `Dispose()` override
- [ ] `Private components As IContainer`
- [ ] `InitializeComponent()` with ALL UI code
- [ ] ALL control declarations as `Friend` or `Friend WithEvents`

---

## Common Errors

**BC30260 "already declared"**  
? Control in both files. Remove from .vb.

**BC30269 "multiple definitions"**  
? `InitializeComponent()` or `Dispose()` in both files. Remove from .vb.

**BC30663 "attribute applied multiple times"**  
? `<DesignerGenerated()>` in both files. Remove from .vb.

**BC30506 "Handles requires WithEvents"**  
? Change `Friend` to `Friend WithEvents` in .Designer.vb.

---

## Summary

**.vb = Business Logic** (thin, simple)  
**.Designer.vb = UI Layout** (all controls, positioning)  
**Together = One UserControl** (merged at compile time)

---

**Related:** file8.md section 7, DesignerFileShellCommandIssues.md
