# Contributing to MU-TH-UR Console App

> *"Collating — I shall have my report on the||pending clauses within twelve hours."*

Thank you for your interest in contributing to the MU-TH-UR 6000 Interface! Whether you're fixing a bug, adding a new command, or improving documentation, your contributions are welcome aboard the Nostromo.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git
- A terminal that supports ANSI escape codes (Windows Terminal, iTerm2, etc.)

### Local Setup

1. **Fork** the repository on GitHub
2. **Clone** your fork:
   ```bash
   git clone https://github.com/<your-username>/MUTHUR-CONSOLE-APP.git
   cd MUTHUR-CONSOLE-APP
   ```
3. **Build** the project:
   ```bash
   dotnet build
   ```
4. **Run** the application:
   ```bash
   dotnet run --project muther-console-app
   ```

## Development Workflow

1. Create a **feature branch** from `main`:
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. Make your changes (see [Coding Guidelines](#coding-guidelines) below)
3. **Build and test** to verify nothing is broken:
   ```bash
   dotnet build
   dotnet run --project muther-console-app
   ```
4. **Commit** your changes with a descriptive message:
   ```bash
   git commit -m "Add COMMS subcommand for relay diagnostics"
   ```
5. **Push** to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```
6. Open a **Pull Request** against `main`

## Coding Guidelines

### General Style

- **Language:** C# on .NET 10 with implicit usings and nullable reference types enabled
- **Architecture:** Static classes with clear single responsibilities
- **Async/Await:** All I/O and rendering methods must be async (`Task` / `Task<bool>`)
- **Section headers:** Use ASCII-decorated comment blocks to separate logical sections:
  ```csharp
  // ── SECTION NAME ──────────────────────
  ```

### Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Classes | PascalCase | `CommandProcessor` |
| Public methods | PascalCase + `Async` suffix | `StatusAsync()` |
| Constants | UPPER_SNAKE_CASE | `SHIP_NAME` |
| Local variables | camelCase | `scanDelay` |
| Private fields | camelCase with `_` prefix | `_running` |

### XML Documentation

- All public classes **must** have `<summary>` XML doc comments
- All public methods **should** have `<summary>`, `<param>`, and `<returns>` tags
- Keep descriptions concise and informative

### Thematic Consistency

This project is an immersive simulation of the MU-TH-UR 6000 mainframe. All user-facing text should:

- Be written in **UPPERCASE** (as a retro terminal would display)
- Use terminology consistent with the *Alien* universe (Weyland-Yutani, USCSS Nostromo, etc.)
- Maintain the green-phosphor CRT aesthetic (`DarkGreen` / `Green` on black)

## Adding a New Command

1. **Add the handler** in `CommandProcessor.cs` as a new `private static async Task<bool>` method
2. **Wire it into the switch** expression in `ExecuteAsync()`
3. **Update the HELP command** output to list the new command
4. **Add any data** to `ShipDatabase.cs` if the command needs canon-accurate information
5. **Update the README** commands table

See [ARCHITECTURE.md](ARCHITECTURE.md) for the full data flow.

## Commit Messages

- Use the imperative mood: "Add feature" not "Added feature"
- Keep the first line under 72 characters
- Reference issues where applicable: `Fix #42`

## Reporting Bugs

Please use the [Bug Report](https://github.com/The-weyland-yutani-corporation/MUTHUR-CONSOLE-APP/issues/new?template=bug_report.md) issue template.

## Requesting Features

Please use the [Feature Request](https://github.com/The-weyland-yutani-corporation/MUTHUR-CONSOLE-APP/issues/new?template=feature_request.md) issue template.

---

*WEYLAND-YUTANI CORPORATION — BUILDING BETTER WORLDS*
