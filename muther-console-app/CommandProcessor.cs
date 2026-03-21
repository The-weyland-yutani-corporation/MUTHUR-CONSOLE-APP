namespace muther_console_app;

/// <summary>
/// Routes and executes all terminal commands.
/// </summary>
public static class CommandProcessor
{
    /// <summary>
    /// Routes a user command to the appropriate handler.
    /// </summary>
    /// <param name="input">The uppercased, trimmed command string entered by the user.</param>
    /// <returns><c>true</c> to keep the session running; <c>false</c> to exit.</returns>
    public static async Task<bool> ExecuteAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return true; // keep running

        return input switch
        {
            "HELP"              => await HelpAsync(),
            "STATUS"            => await StatusAsync(),
            "CREW"              => await CrewAsync(),
            "COURSE"            => await CourseAsync(),
            "CARGO"             => await CargoAsync(),
            "COMMS"             => await CommsAsync(),
            "DIAGNOSTICS"       => await DiagnosticsAsync(),
            "MOTHER"            => await MotherAsync(),
            "SPECIAL ORDER 937" => await SpecialOrderAsync(),
            "SELF DESTRUCT"     => await SelfDestructAsync(),
            "PONG"              => await PongAsync(),
            "CLEAR" or "CLS"    => ClearCmd(),
            "LOGOUT" or "EXIT"  => await LogoutAsync(),
            _                   => await UnknownAsync(input),
        };
    }

    // ── HELP ──────────────────────────────────────────────

    /// <summary>Displays the list of available terminal commands.</summary>
    private static async Task<bool> HelpAsync()
    {
        string[] lines =
        [
            "",
            "AVAILABLE COMMANDS:",
            "──────────────────────────────────────────",
            "  HELP              DISPLAY THIS LIST",
            "  STATUS            SHIP SYSTEMS STATUS",
            "  CREW              CREW MANIFEST",
            "  COURSE            NAVIGATION AND HEADING",
            "  CARGO             CARGO MANIFEST",
            "  COMMS             COMMUNICATIONS STATUS",
            "  DIAGNOSTICS       RUN FULL DIAGNOSTIC SWEEP",
            "  MOTHER            MU-TH-UR DIRECT INTERFACE",
            "  CLEAR             CLEAR TERMINAL",
            "  LOGOUT            TERMINATE SESSION",
            "──────────────────────────────────────────",
            "",
        ];
        await TerminalRenderer.TypeBlockAsync(lines, bright: true);
        return true;
    }

    // ── STATUS ────────────────────────────────────────────

    /// <summary>Shows the current status of all ship subsystems.</summary>
    private static async Task<bool> StatusAsync()
    {
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            $"  {ShipDatabase.ShipName} — SYSTEMS OVERVIEW", bright: true);
        await TerminalRenderer.DrawRuleAsync();

        foreach (var (system, status) in ShipDatabase.Subsystems)
        {
            string padded = $"  {system,-24} {status}";
            await TerminalRenderer.TypeLineAsync(padded);
        }

        await TerminalRenderer.DrawRuleAsync();
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }

    // ── CREW ──────────────────────────────────────────────

    /// <summary>Displays the full crew manifest for the USCSS Nostromo.</summary>
    private static async Task<bool> CrewAsync()
    {
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync("  CREW MANIFEST — USCSS NOSTROMO", bright: true);
        await TerminalRenderer.DrawRuleAsync();
        await TerminalRenderer.TypeLineAsync(
            $"  {"NAME",-20} {"RANK",-26} {"STATUS"}", bright: true);
        await TerminalRenderer.DrawRuleAsync('─', 60);

        foreach (var (name, rank, status) in ShipDatabase.Crew)
        {
            string line = $"  {name,-20} {rank,-26} {status}";
            await TerminalRenderer.TypeLineAsync(line);
        }

        await TerminalRenderer.DrawRuleAsync();
        await TerminalRenderer.TypeLineAsync(
            $"  TOTAL CREW: {ShipDatabase.Crew.Length}");
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }

    // ── COURSE ────────────────────────────────────────────

    /// <summary>Displays navigation data and current flight plan.</summary>
    private static async Task<bool> CourseAsync()
    {
        string[] lines =
        [
            "",
            "  NAVIGATION — FLIGHT PLAN",
            "────────────────────────────────────────────────────────────",
            $"  ORIGIN:          {ShipDatabase.MissionOrigin}",
            $"  DESTINATION:     {ShipDatabase.MissionDestination}",
            "  ROUTE:           OUTER RIM — ZETA II RETICULI CORRIDOR",
            "  SPEED:           STANDARD CRUISE — WARP FACTOR 0.42c",
            "  ETA:             10 MONTHS (APPROXIMATE)",
            "  COURSE LOCKED:   AFFIRMATIVE",
            "  AUTOPILOT:       ENGAGED",
            "────────────────────────────────────────────────────────────",
            "",
        ];
        await TerminalRenderer.TypeBlockAsync(lines);
        return true;
    }

    // ── CARGO ─────────────────────────────────────────────

    /// <summary>Displays the cargo manifest and integrity status.</summary>
    private static async Task<bool> CargoAsync()
    {
        string[] lines =
        [
            "",
            "  CARGO MANIFEST",
            "────────────────────────────────────────────────────────────",
            $"  PRIMARY:    {ShipDatabase.CargoDescription}",
            $"  MODULE:     {ShipDatabase.RefinerySuffix}",
            "  INTEGRITY:  SEALED — NO BREACHES DETECTED",
            "  TEMP:       -40°C (NOMINAL)",
            "────────────────────────────────────────────────────────────",
            "",
        ];
        await TerminalRenderer.TypeBlockAsync(lines);
        return true;
    }

    // ── COMMS ─────────────────────────────────────────────

    /// <summary>Displays deep-space communications status and relay information.</summary>
    private static async Task<bool> CommsAsync()
    {
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync("  COMMUNICATIONS STATUS", bright: true);
        await TerminalRenderer.DrawRuleAsync();
        string[] lines =
        [
            "  DEEP SPACE RELAY:      STANDBY",
            "  LAST TRANSMISSION:     — NO RECORD —",
            "  SIGNAL STRENGTH:       0.00 dBm (NO ACTIVE LINK)",
            "  EMERGENCY BEACON:      ARMED",
            "  NEAREST RELAY STATION: OUT OF RANGE",
            "",
            "  NOTE: STANDARD COMMS BLACKOUT DURING HYPERSLEEP TRANSIT.",
        ];
        await TerminalRenderer.TypeBlockAsync(lines);
        await TerminalRenderer.DrawRuleAsync();
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }

    // ── DIAGNOSTICS ───────────────────────────────────────

    /// <summary>Runs an animated diagnostic sweep across all ship subsystems.</summary>
    private static async Task<bool> DiagnosticsAsync()
    {
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            "  RUNNING FULL DIAGNOSTIC SWEEP...", bright: true);
        await TerminalRenderer.PauseAsync(600);

        string[] systems =
        [
            "REACTOR CORE", "COOLANT SYSTEM", "OXYGEN RECYCLER",
            "GRAVITY GENERATOR", "HULL PLATING", "NAVIGATION GYRO",
            "SENSOR ARRAY", "FIRE SUPPRESSION", "AIRLOCK SERVOS",
            "HYPERSLEEP MONITORS", "MU-TH-UR CORE LOGIC",
        ];

        var rng = new Random();
        foreach (string sys in systems)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"  SCANNING {sys,-28}");
            await Task.Delay(rng.Next(200, 600));

            // Occasional flicker for atmosphere
            if (rng.Next(100) < 15)
                await TerminalRenderer.FlickerAsync();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PASS");
        }

        await TerminalRenderer.PauseAsync(300);
        await TerminalRenderer.DrawRuleAsync();
        await TerminalRenderer.TypeLineAsync(
            "  ALL SYSTEMS NOMINAL. NO FAULTS DETECTED.", bright: true);
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }

    // ── SPECIAL ORDER 937 ────────────────────────────────

    /// <summary>Reveals the classified Special Order 937 after credential verification.</summary>
    private static async Task<bool> SpecialOrderAsync()
    {
        _ = SoundEffects.AlertToneAsync();
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            "  *** EYES ONLY — SCIENCE OFFICER ***", bright: true);
        await TerminalRenderer.PauseAsync(500);
        await TerminalRenderer.TypeLineAsync(
            "  ACCESS LEVEL: ALPHA-1 REQUIRED.");
        await TerminalRenderer.TypeLineAsync(
            "  VERIFYING CREDENTIALS...");
        await TerminalRenderer.PauseAsync(1200);

        await TerminalRenderer.FlickerAsync(2);

        // Reveal the order
        await TerminalRenderer.DrawRuleAsync('█', 60);
        foreach (string line in ShipDatabase.SpecialOrder937)
        {
            await TerminalRenderer.TypeLineAsync($"  {line}", bright: true, slow: true);
        }
        await TerminalRenderer.DrawRuleAsync('█', 60);

        _ = SoundEffects.AlertToneAsync();
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }

    // ── MOTHER ───────────────────────────────────────────

    /// <summary>Enters the MU-TH-UR direct interface conversational mode.</summary>
    private static async Task<bool> MotherAsync()
    {
        await MotherMode.RunAsync();
        return true;
    }

    // ── SELF DESTRUCT ────────────────────────────────────

    /// <summary>Initiates the emergency scuttle countdown sequence (simulation only).</summary>
    private static async Task<bool> SelfDestructAsync()
    {
        _ = SoundEffects.KlaxonAsync();
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            "  *** WARNING — EMERGENCY SCUTTLE SYSTEM ***", bright: true);
        await TerminalRenderer.TypeLineAsync(
            "  THIS ACTION WILL DESTROY THE NOSTROMO AND ALL ABOARD.");
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            "  TYPE 'CONFIRM' TO ARM THE SELF-DESTRUCT SEQUENCE.", bright: true);

        string confirmation = TerminalRenderer.ReadCommand("  AUTHORIZATION> ");

        if (confirmation != "CONFIRM")
        {
            await TerminalRenderer.TypeLineAsync(
                "  SELF-DESTRUCT ABORTED. RESUMING NORMAL OPERATIONS.");
            await TerminalRenderer.TypeLineAsync("");
            return true;
        }

        _ = SoundEffects.KlaxonAsync();
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            "  SELF-DESTRUCT SEQUENCE ACTIVATED.", bright: true);
        await TerminalRenderer.TypeLineAsync(
            $"  THE SHIP WILL DETONATE IN T-MINUS {ShipDatabase.SelfDestructCountdownSeconds} SECONDS.");
        await TerminalRenderer.TypeLineAsync(
            "  TYPE 'ABORT' TO CANCEL.");
        await TerminalRenderer.TypeLineAsync("");

        for (int i = ShipDatabase.SelfDestructCountdownSeconds; i > 0; i--)
        {
            Console.ForegroundColor = i <= 3 ? ConsoleColor.Green : ConsoleColor.DarkGreen;
            Console.Write($"\r  *** T-MINUS {i,3} SECONDS ***   ");
            _ = SoundEffects.CountdownTickAsync(i);
            await Task.Delay(1000);
        }

        Console.WriteLine();
        await TerminalRenderer.FlickerAsync(5);

        // Instead of actually "exploding," reset the terminal
        TerminalRenderer.ClearScreen();
        await TerminalRenderer.PauseAsync(2000);
        await TerminalRenderer.TypeLineAsync(
            "  ... JUST KIDDING. THIS IS A SIMULATION.", bright: true);
        await TerminalRenderer.TypeLineAsync(
            "  SELF-DESTRUCT OVERRIDE ENGAGED. SYSTEMS RESTORED.");
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }

    // ── CLEAR ─────────────────────────────────────────────

    /// <summary>Clears the terminal display.</summary>
    private static bool ClearCmd()
    {
        TerminalRenderer.ClearScreen();
        return true;
    }

    // ── PONG ──────────────────────────────────────────────

    /// <summary>Launches the hidden Pong easter egg game.</summary>
    private static async Task<bool> PongAsync()
    {
        await PongGame.RunAsync();
        return true;
    }

    // ── LOGOUT ────────────────────────────────────────────

    /// <summary>Terminates the MU-TH-UR session and returns <c>false</c> to exit the loop.</summary>
    private static async Task<bool> LogoutAsync()
    {
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            "  TERMINATING SESSION...", bright: true);
        await TerminalRenderer.PauseAsync(800);
        await TerminalRenderer.TypeLineAsync(
            $"  {ShipDatabase.ComputerName} SIGNING OFF.");
        await TerminalRenderer.TypeLineAsync(
            $"  {ShipDatabase.CompanySlogan}.");
        await TerminalRenderer.PauseAsync(500);
        TerminalRenderer.Beep(600, 200);
        return false; // signal exit
    }

    // ── UNKNOWN ───────────────────────────────────────────

    /// <summary>Handles unrecognized commands with an error message.</summary>
    /// <param name="input">The unrecognized command string.</param>
    private static async Task<bool> UnknownAsync(string input)
    {
        _ = SoundEffects.ErrorBuzzAsync();
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync(
            $"  UNABLE TO COMPLY. UNRECOGNIZED INPUT: '{input}'");
        await TerminalRenderer.TypeLineAsync(
            "  TYPE 'HELP' FOR A LIST OF VALID COMMANDS.");
        await TerminalRenderer.TypeLineAsync("");
        return true;
    }
}
