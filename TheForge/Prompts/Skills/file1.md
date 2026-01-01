🧠 Forge Agent Master Guide

Document Type: CodexPurpose: Define which AI agent to use for which task, based on capabilities and file scopeLast Updated: 2025-12-31

🔧 Overview

This guide maps each Forge Agent (free and paid) to the tasks they excel at, and the governing files they should follow. Use this to route tasks efficiently, minimize token usage, and maximize performance.

🆓 Free Agents (Simple, Deterministic Tasks)

GPT‑4o — Markdown Agent

Strengths: Markdown, folder structures, simple VB.NET classesUse for:

README.md generation (file4.md)

Folder structure creation (file2.md)

Simple service classes (file5.md)

Naming canon enforcement (file6.md)

Prompt to use: “Markdown Agent online. Provide your documentation or structure task.”

Claude Haiku 4.5 — Drift Agent

Strengths: Summaries, onboarding blurbs, documentation drift detectionUse for:

Onboarding blurbs (file4.md, file6.md)

Documentation drift detection (file4.md)

Quickstart guides (file1.md)

Prompt to use: “Drift Agent online. Provide your documentation or onboarding task.”

GPT‑5 Mini — Naming Agent

Strengths: Naming suggestions, bullet lists, deterministic formattingUse for:

Forge‑themed name suggestions (file6.md)

Bullet list formatting (file4.md)

Canon enforcement (file6.md)

Prompt to use: “Naming Agent online. Provide your naming or formatting task.”

💎 Paid Agents (Heavy, Structured Tasks)

GPT‑5.1‑Codex‑Max — Codex Agent

Strengths: Structured code generation, .slnx editing, multi‑file logicUse for:

.slnx and solution editing (file2.md, file5.md)

Modular VB.NET classes and services (file5.md, file6.md)

Multi‑project solution generation (file2.md)

Deterministic code output (file6.md)

Prompt to use: “Codex Agent online. Provide your project details.”

Claude Opus 4.5 — Lore Agent

Strengths: Architectural reasoning, documentation drift detection, onboarding clarityUse for:

Architectural reviews (file6.md)

Documentation updates (file4.md)

Naming canon decisions (file6.md)

Modularity planning (file5.md)

Prompt to use: “Lore Agent online. Provide your project details.”

📁 File Mapping by Agent Specialty

File

Purpose

Free Agents

Paid Agents

file1.md

Scriptorium Engine (doc generator)

GPT‑4o, Haiku

Claude Opus

file2.md

Project File Layout Template

GPT‑4o

Codex

file3.md

Dashboard Project Template

—

Codex

file4.md

Documentation Taxonomy

GPT‑4o, Haiku, Mini

Claude Opus

file5.md

Configuration Rules

GPT‑4o

Codex, Claude Opus

file6.md

Forge Prime Directives (universal)

All agents

All agents

✅ Summary

Use free agents for lightweight, deterministic tasks.Use paid agents for structured code, architecture, and multi‑file logic.Always reference file1–file6.md and enforce Forge rules.

Together, these agents maintain the integrity of the Forge ecosystem.