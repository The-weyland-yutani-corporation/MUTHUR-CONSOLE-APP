# MU-TH-UR 6000 — INTERFACE 2037

```
  ██╗    ██╗ ██████╗██╗   ██╗
  ██║    ██║██╔════╝╚██╗ ██╔╝
  ██║ █╗ ██║╚█████╗  ╚████╔╝
  ██║███╗██║ ╚═══██╗  ╚██╔╝
  ╚███╔███╔╝██████╔╝   ██║
   ╚══╝╚══╝ ╚═════╝    ╚═╝
   WEYLAND-YUTANI CORPORATION
     BUILDING BETTER WORLDS
```

> *"Final report of the commercial starship Nostromo, third officer reporting..."*

A fully-functioning retro CRT terminal simulator inspired by the **MU-TH-UR 6000** mainframe computer aboard the **USCSS Nostromo** from Ridley Scott's *Alien* (1979).

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A terminal that supports `Console.Beep` for full audio immersion (Windows recommended)

## Run

```bash
dotnet run --project muther-console-app
```

## Features

| Feature | Description |
|---|---|
| **CRT Boot Sequence** | POST diagnostics, memory check, Weyland-Yutani ASCII logo |
| **Typewriter Effect** | Character-by-character rendering with randomized micro-delays |
| **Green Phosphor Look** | `DarkGreen` / `Green` on black — authentic 1980s CRT |
| **CRT Flicker** | Simulated scan-line glitches for atmosphere |
| **Sound Effects** | Klaxon, alert tones, boot chime, data blips, and error buzz via `Console.Beep` |
| **MOTHER Mode** | Conversational interface — ask MU-TH-UR questions in natural language |
| **Idle Screen** | Animated screensaver with star field, pulsing text, and status ticker after 30s of inactivity |
| **Hidden Easter Egg** | 👀 *Some things are best discovered on your own...* |

## Commands

| Command | Description |
|---|---|
| `HELP` | Display available commands |
| `STATUS` | Ship systems overview (9 subsystems) |
| `CREW` | Crew manifest — all 7 Nostromo crew members |
| `COURSE` | Navigation data — Thedus → Earth |
| `CARGO` | Cargo manifest — 20M tonnes mineral ore |
| `COMMS` | Communications status (deep-space blackout) |
| `DIAGNOSTICS` | Animated system-by-system diagnostic scan |
| `MOTHER` | MU-TH-UR direct interface — natural language queries |
| `SPECIAL ORDER 937` | 🔒 *Eyes only — Science Officer* |
| `SELF DESTRUCT` | Emergency scuttle sequence with countdown |
| `CLEAR` | Clear the terminal |
| `LOGOUT` | Terminate session |

## Project Structure

```
muther-console-app/
├── Program.cs            # Entry point
├── MuthurTerminal.cs     # Console init + main session loop
├── TerminalRenderer.cs   # CRT effects engine (typewriter, flicker, beep)
├── ShipDatabase.cs       # Canon-accurate Nostromo data
├── BootSequence.cs       # POST boot animation
├── CommandProcessor.cs   # Command routing (13 commands)
├── MotherMode.cs         # MU-TH-UR conversational interface
├── SoundEffects.cs       # Console.Beep tone patterns
├── PongGame.cs           # Hidden recreational subroutine
├── IdleScreen.cs         # Animated idle screensaver
└── muther-console-app.csproj
```

## Canon Sources

- *Alien* (1979) — Ridley Scott
- *Aliens* (1986) — James Cameron
- USCSS Nostromo technical specifications
- Weyland-Yutani corporate records

---

*WEYLAND-YUTANI CORPORATION — BUILDING BETTER WORLDS*
