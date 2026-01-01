# Repository-wide Copilot instructions

- This repository follows the Forge rule stack (file1–file6); do not restate rules already present in code or docs.
- Use VB.NET with Option Strict On, Option Explicit On, Option Infer Off.
- For WinForms:
  - Use deterministic layout (TableLayoutPanel, SplitContainer, Dock).
  - Do not use anchoring.
  - Do not create default names like Form1; always use explicit, descriptive names.
- Do not modify project structure or delete files unless explicitly instructed in the prompt.
- Prefer separation of UI, services, and models; no business logic in designer files.