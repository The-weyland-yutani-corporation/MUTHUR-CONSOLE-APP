# MU-TH-UR 6000 Console App — Architecture

## Overview

A .NET 10 console application simulating the **MU-TH-UR 6000** mainframe computer from *Alien* (1979). The app recreates the green phosphor CRT terminal experience with a boot sequence, typewriter rendering effects, and an interactive command loop providing access to canon-accurate ship data from the USCSS Nostromo.

All classes are static (no dependency injection). Async/await is used throughout to drive rendering timing and visual effects. The console uses a green-on-black color scheme (`DarkGreen`/`Green` on `Black`) to emulate a period-accurate CRT aesthetic.

## Architecture Diagram

```
Program.cs
    │
    ▼
MuthurTerminal.RunAsync()
    │
    ├── BootSequence.PlayAsync()
    │       ├── TerminalRenderer (effects)
    │       └── ShipDatabase (logo, subsystems)
    │
    └── Loop:
            ├── TerminalRenderer.ReadCommand()
            └── CommandProcessor.ExecuteAsync()
                    ├── TerminalRenderer (effects)
                    └── ShipDatabase (data)
```

## Component Descriptions

### Program.cs

**Purpose:** Entry point (~4 lines). Creates a `MuthurTerminal` instance and calls `RunAsync()`.

### MuthurTerminal.cs

**Purpose:** Main terminal session loop (~46 lines).

| Method | Description |
|---|---|
| `InitConsole()` | Sets up the console — title, foreground/background colors, window size. |
| `RunAsync()` | Runs `BootSequence.PlayAsync()`, then enters the command loop reading input and dispatching to `CommandProcessor`. |

### BootSequence.cs

**Purpose:** Static class (~94 lines). `PlayAsync()` plays the full POST (Power-On Self-Test) animation sequence.

| Step | Detail |
|---|---|
| CRT warm-up | Flicker effect simulating phosphor warm-up |
| Logo | Weyland-Yutani ASCII logo from `ShipDatabase` |
| Memory check | Animated memory count |
| Subsystem checks | Iterates `ShipDatabase.Subsystems` with status output |
| Ready message | Final ready prompt with console beeps |

### CommandProcessor.cs

**Purpose:** Static class (~318 lines). `ExecuteAsync()` routes user input to command handlers via a switch expression. Returns `Task<bool>` — `true` to continue the loop, `false` to exit (on `LOGOUT`).

| Command | Description |
|---|---|
| `HELP` | Lists available commands |
| `STATUS` | Ship status overview |
| `CREW` | Crew manifest with rank and status |
| `COURSE` | Current navigation/course data |
| `CARGO` | Cargo manifest |
| `COMMS` | Communications system status |
| `DIAGNOSTICS` | Full system diagnostics |
| `SPECIAL ORDER 937` | Classified directive |
| `SELF DESTRUCT` | Self-destruct sequence |
| `CLEAR` | Clears the terminal |
| `LOGOUT` | Ends the session (returns `false`) |

### TerminalRenderer.cs

**Purpose:** Static class (~119 lines) providing all rendering and visual effects. Every method has XML documentation.

| Method | Description |
|---|---|
| `TypeLineAsync` | Typewriter effect, character-by-character (8–35 ms delays) |
| `TypeBlockAsync` | Renders a multi-line block with typewriter pacing |
| `PrintImmediate` | Instant text output (no animation) |
| `FlickerAsync` | CRT flicker/glitch effect |
| `ReadCommand` | Reads user input from the prompt |
| `ClearScreen` | Clears the console buffer |
| `DrawRuleAsync` | Draws a horizontal rule/divider |
| `PauseAsync` | Timed pause between output sections |
| `Beep` | Platform-safe beep wrapped in try-catch for `NotSupportedException` |

### ShipDatabase.cs

**Purpose:** Static class (~84 lines) holding all canon-accurate data as constants and static arrays.

| Member | Description |
|---|---|
| Ship info | USCSS Nostromo identification and specs |
| Crew array | 7 crew members with rank and status |
| Subsystems array | 9 ship subsystems |
| Special Order 937 | Full classified directive text |
| ASCII logo | Weyland-Yutani corporate logo |

## How to Add a New Command

1. **Choose a command name.** Pick an uppercase string (e.g., `"AIRLOCK"`).

2. **Add a handler method** in `CommandProcessor.cs`:

   ```csharp
   // -- AIRLOCK --
   private static async Task HandleAirlockAsync()
   {
       await TerminalRenderer.TypeLineAsync("AIRLOCK STATUS:");
       // ... command output using TerminalRenderer and ShipDatabase
   }
   ```

3. **Register the command** in the switch expression inside `ExecuteAsync()`:

   ```csharp
   "AIRLOCK" => await HandleAndContinue(HandleAirlockAsync),
   ```

   Use the existing pattern — return `true` to continue the loop.

4. **Add it to the HELP listing** so it appears when the user types `HELP`.

5. **Add any new data** to `ShipDatabase.cs` if the command needs ship data that doesn't exist yet.

## How to Add New Ship Data

1. **Open `ShipDatabase.cs`.**

2. **Add a new constant, array, or static property** following the existing conventions:

   ```csharp
   // -- AIRLOCK DATA --
   public static readonly string[] Airlocks =
   [
       "AIRLOCK 1 — PORT SIDE — SEALED",
       "AIRLOCK 2 — STARBOARD — SEALED",
       "AIRLOCK 3 — AFT — SEALED",
   ];
   ```

3. **Reference it** from `CommandProcessor.cs` or `BootSequence.cs` as needed. All data access is direct static member access — no lookup layer required.

## How to Add New Visual Effects

1. **Open `TerminalRenderer.cs`.**

2. **Add a new static async method** with XML documentation:

   ```csharp
   /// <summary>
   /// Simulates a screen tear / static burst effect.
   /// </summary>
   public static async Task StaticBurstAsync()
   {
       // Implementation using Console.Write, Task.Delay, etc.
   }
   ```

3. **Follow the existing conventions:**
   - Use `async Task` return types for anything with timing.
   - Use `Console.ForegroundColor` (`DarkGreen`/`Green`) and `Console.BackgroundColor` (`Black`) to stay within the CRT palette.
   - Wrap any platform-dependent calls (e.g., `Console.Beep`) in a try-catch for `NotSupportedException`.
   - Add `// -- SECTION --` comment headers for logical grouping.

4. **Call the new effect** from `BootSequence.cs`, `CommandProcessor.cs`, or `MuthurTerminal.cs` as appropriate.

---

*WEYLAND-YUTANI CORPORATION — BUILDING BETTER WORLDS*
