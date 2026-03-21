# MU-TH-UR Console App — Copilot Coding Instructions

## Project Overview

This is a .NET 10 console application simulating the **MU-TH-UR 6000** mainframe computer from *Alien* (1979). The app recreates the retro CRT terminal experience with typewriter effects, screen flicker, and canon-accurate data from the Alien franchise.

- **Framework:** .NET 10, C# (no external NuGet packages)
- **ImplicitUsings:** enabled
- **Nullable:** enabled
- **Namespace:** `muther_console_app`
- **Solution file:** `muther-console-app.slnx`
- **Run command:** `dotnet run --project muther-console-app`

## Architecture

```
Program.cs
  └─ MuthurTerminal.RunAsync()
       ├─ BootSequence.PlayAsync()
       └─ Command loop
            └─ CommandProcessor.ExecuteAsync()
                 └─ Routes commands via switch expression → returns Task<bool>
```

- **MuthurTerminal** — Entry point; orchestrates boot sequence and the main command loop.
- **CommandProcessor** — Routes user commands via a `switch` expression; each handler returns `Task<bool>`.
- **TerminalRenderer** — All CRT visual effects (`TypeLineAsync`, `FlickerAsync`, `Beep`, etc.).
- **ShipDatabase** — Canon-accurate Nostromo data stored as `static` constants and arrays.
- **All classes are static** — there is no dependency injection.

## Code Style

### Naming Conventions

| Element       | Convention          | Example                        |
|---------------|---------------------|--------------------------------|
| Methods       | PascalCase          | `ExecuteAsync()`               |
| Constants     | UPPER_SNAKE_CASE    | `SHIP_NAME`                    |
| Local vars    | camelCase           | `commandInput`                 |
| Async methods | Suffix with `Async` | `TypeLineAsync()`, `RunAsync()`|

### Async/Await

All I/O and rendering must use `async`/`await`. Every asynchronous method name must end with `Async`.

```csharp
public static async Task DisplayCrewAsync()
{
    await TerminalRenderer.TypeLineAsync("CREW MANIFEST:", bright: true);
    // ...
}
```

### Section Headers

Organize code regions with section header comments:

```csharp
// ── CREW DATA ──

// ── COMMAND ROUTING ──
```

### Documentation

- Use XML doc comments on all `public` classes and methods.
- Keep inline comments minimal — code should be self-documenting.

```csharp
/// <summary>
/// Displays the USCSS Nostromo cargo manifest.
/// </summary>
public static async Task<bool> CargoAsync()
{
    // ...
}
```

## Thematic Guidelines

### CRT Aesthetic

The terminal uses a green phosphor CRT look: `DarkGreen` and `Green` on a `Black` background. Use `TerminalRenderer` methods to maintain the immersive feel:

- **`TypeLineAsync(text, bright, slow)`** — Typewriter effect with optional bright color and slow speed for emphasis.
- **`FlickerAsync()`** — Simulates CRT screen flicker.
- **`Beep()`** — Platform-safe terminal beep.
- **`Random.Shared`** — Use random delays to add atmosphere.

### Uppercase Output

All user-facing terminal output **must be UPPERCASE**:

```csharp
await TerminalRenderer.TypeLineAsync("ACCESS DENIED. SPECIAL ORDER 937 CLASSIFIED.");
```

### Alien-Universe Terminology

Always use in-universe names and terminology:

- **Weyland-Yutani** (not "the company")
- **USCSS Nostromo** (not "the ship")
- **MU-TH-UR 6000** (not "the computer")

### Canon Accuracy

All data must be canon-accurate to the Alien franchise, primarily sourced from:

- *Alien* (1979)
- *Aliens* (1986)

## Adding a New Command

Follow these steps when adding a new terminal command:

1. **Add a handler** in `CommandProcessor.cs`:

    ```csharp
    /// <summary>
    /// Displays the hypersleep chamber status.
    /// </summary>
    private static async Task<bool> HypersleepAsync()
    {
        await TerminalRenderer.TypeLineAsync("HYPERSLEEP CHAMBERS: ALL NOMINAL", bright: true);
        // ...
        return true;
    }
    ```

2. **Wire it into the `ExecuteAsync()` switch expression:**

    ```csharp
    "HYPERSLEEP" => HypersleepAsync(),
    ```

3. **Update `HelpAsync()`** to list the new command.

4. **Add any supporting data** to `ShipDatabase.cs` as static constants or arrays.

5. **Update the README** commands table.

## Key Patterns

### Platform-Safe Beep

`Console.Beep` is not supported on all platforms. Always wrap it in a try-catch:

```csharp
try
{
    Console.Beep();
}
catch (NotSupportedException)
{
    // Beep not supported on this platform
}
```

### Input Normalization

`ReadCommand` normalizes all user input with `Trim().ToUpperInvariant()` before routing, so command matching is always against uppercase strings.

### Atmospheric Delays

Use `Random.Shared` for subtle random delays that enhance the retro terminal feel:

```csharp
await Task.Delay(Random.Shared.Next(30, 80));
```

### Emphasized Output

Use the `bright` and `slow` parameters on `TypeLineAsync` for important lines:

```csharp
await TerminalRenderer.TypeLineAsync("PRIORITY ONE — ENSURE RETURN OF ORGANISM.", bright: true, slow: true);
```
