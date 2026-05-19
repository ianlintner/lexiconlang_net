# Agent instructions

This repository uses `.github/copilot-instructions.md` as the primary coding-agent guidance.

## Quick rules

- Preserve deterministic generation behavior.
- Prefer additive, backward-compatible API changes.
- Keep `dotnet build -warnaserror` green.
- Keep formatting/analyzers clean with `dotnet format`.
- Ensure library projects remain NuGet-packable.
