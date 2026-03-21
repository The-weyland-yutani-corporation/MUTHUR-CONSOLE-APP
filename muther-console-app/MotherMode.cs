namespace muther_console_app;

/// <summary>
/// Provides the MU-TH-UR 6000 direct interface conversational mode,
/// allowing natural-language queries against the mainframe.
/// </summary>
public static class MotherMode
{
    // ── PUBLIC ENTRY POINT ──

    /// <summary>
    /// Enters the MU-TH-UR direct interface conversational mode.
    /// </summary>
    public static async Task RunAsync()
    {
        // ── ENTRY SEQUENCE ──

        await TerminalRenderer.FlickerAsync();
        await TerminalRenderer.TypeLineAsync("ACCESSING MU-TH-UR 6000 DIRECT INTERFACE...", bright: true, slow: true);
        await TerminalRenderer.PauseAsync(800);
        await TerminalRenderer.TypeLineAsync("INTERFACE ACTIVE. ENTER QUERY OR TYPE 'EXIT' TO DISCONNECT.", bright: true);
        await TerminalRenderer.DrawRuleAsync();

        // ── SUB-LOOP ──

        while (true)
        {
            string input = TerminalRenderer.ReadCommand("MOTHER> ");

            if (input is "EXIT" or "LOGOUT" or "BACK")
                break;

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (Random.Shared.Next(100) < 15)
                await TerminalRenderer.FlickerAsync();

            _ = SoundEffects.DataBlipAsync();

            await GetResponseAsync(input);
        }

        // ── EXIT SEQUENCE ──

        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync("DISCONNECTING FROM MU-TH-UR DIRECT INTERFACE...");
        await TerminalRenderer.PauseAsync(600);
        await TerminalRenderer.TypeLineAsync("RETURNED TO STANDARD TERMINAL MODE.");
        await TerminalRenderer.DrawRuleAsync();
    }

    // ── RESPONSE ROUTING ──

    /// <summary>
    /// Matches keywords in the input and displays the appropriate response.
    /// </summary>
    private static async Task GetResponseAsync(string input)
    {
        await TerminalRenderer.TypeLineAsync("");

        // 1. Special Order / Classified
        if (input.Contains("937") || input.Contains("SPECIAL ORDER") ||
            input.Contains("CLASSIFIED") || input.Contains("PRIORITY ONE"))
        {
            await TerminalRenderer.TypeLineAsync("UNABLE TO COMPLY. REQUESTED DATA IS CLASSIFIED.");
            await TerminalRenderer.TypeLineAsync("AUTHORIZATION LEVEL: ALPHA-1 REQUIRED.");
            await TerminalRenderer.TypeLineAsync("ACCESS DENIED.");
        }

        // 2. Xenomorph / Unknown organism
        else if (input.Contains("ALIEN") || input.Contains("CREATURE") ||
                 input.Contains("ORGANISM") || input.Contains("XENOMORPH") ||
                 input.Contains("SPECIMEN") || input.Contains("LIFEFORM"))
        {
            await TerminalRenderer.TypeLineAsync("NO DATA ON FILE MATCHING QUERY PARAMETERS.");
            await TerminalRenderer.TypeLineAsync("RECOMMEND FILING REQUEST WITH SCIENCE DIVISION.");
        }

        // 3. Self-destruct
        else if (input.Contains("DESTRUCT") || input.Contains("SCUTTLE") ||
                 input.Contains("DETONATE") || input.Contains("EXPLODE"))
        {
            await TerminalRenderer.TypeLineAsync("EMERGENCY SCUTTLE SYSTEM: ARMED AND AVAILABLE.");
            await TerminalRenderer.TypeLineAsync("AUTHORIZATION REQUIRES SENIOR OFFICER CONFIRMATION.");
            await TerminalRenderer.TypeLineAsync("THIS ACTION IS IRREVERSIBLE.");
        }

        // 4. Crew — specific names
        else if (input.Contains("DALLAS") || input.Contains("KANE") ||
                 input.Contains("RIPLEY") || input.Contains("ASH") ||
                 input.Contains("LAMBERT") || input.Contains("PARKER") ||
                 input.Contains("BRETT"))
        {
            await DisplayCrewMemberAsync(input);
        }

        // 5. Crew — general
        else if (input.Contains("CREW") || input.Contains("MANIFEST") ||
                 input.Contains("PERSONNEL"))
        {
            await TerminalRenderer.TypeLineAsync("CREW COMPLEMENT: 7 REGISTERED.");
            await TerminalRenderer.TypeLineAsync("6 IN HYPERSLEEP. 1 ACTIVE.");
            await TerminalRenderer.TypeLineAsync("ACCESS CREW MANIFEST VIA 'CREW' COMMAND.");
        }

        // 6. Ship
        else if (input.Contains("NOSTROMO") || input.Contains("SHIP") ||
                 input.Contains("VESSEL"))
        {
            await TerminalRenderer.TypeLineAsync("USCSS NOSTROMO — REG 180924609");
            await TerminalRenderer.TypeLineAsync("LOCKMART CM 88B BISON M-CLASS STARFREIGHTER");
            await TerminalRenderer.TypeLineAsync("ALL PRIMARY SYSTEMS: NOMINAL");
        }

        // 7. Mission / Cargo
        else if (input.Contains("MISSION") || input.Contains("CARGO") ||
                 input.Contains("ORE") || input.Contains("REFINERY"))
        {
            await TerminalRenderer.TypeLineAsync("PRIMARY MISSION: COMMERCIAL FREIGHT TRANSPORT");
            await TerminalRenderer.TypeLineAsync("CARGO: 20,000,000 TONNES MINERAL ORE");
            await TerminalRenderer.TypeLineAsync("ORIGIN: THEDUS — DESTINATION: EARTH");
        }

        // 8. Course / Navigation
        else if (input.Contains("COURSE") || input.Contains("ROUTE") ||
                 input.Contains("DESTINATION") || input.Contains("HEADING") ||
                 input.Contains("ETA") || input.Contains("NAVIGATION"))
        {
            await TerminalRenderer.TypeLineAsync("CURRENT HEADING: EARTH — OUTER RIM CORRIDOR");
            await TerminalRenderer.TypeLineAsync("SPEED: WARP FACTOR 0.42C");
            await TerminalRenderer.TypeLineAsync("ETA: APPROXIMATELY 10 MONTHS");
            await TerminalRenderer.TypeLineAsync("AUTOPILOT: ENGAGED");
        }

        // 9. Hypersleep
        else if (input.Contains("HYPERSLEEP") || input.Contains("CRYO") ||
                 input.Contains("SLEEP") || input.Contains("POD") ||
                 input.Contains("CHAMBER"))
        {
            await TerminalRenderer.TypeLineAsync("HYPERSLEEP VAULTS: 6 OF 7 OCCUPIED");
            await TerminalRenderer.TypeLineAsync("ALL PODS FUNCTIONING WITHIN NORMAL PARAMETERS.");
            await TerminalRenderer.TypeLineAsync("LIFE SIGNS: STABLE");
        }

        // 10. Life support
        else if (input.Contains("LIFE SUPPORT") || input.Contains("OXYGEN") ||
                 input.Contains("AIR") || input.Contains("ATMOSPHERE") ||
                 input.Contains("BREATHE"))
        {
            await TerminalRenderer.TypeLineAsync("LIFE SUPPORT SYSTEMS: NOMINAL");
            await TerminalRenderer.TypeLineAsync("OXYGEN RECYCLER: OPERATING AT 98.7% EFFICIENCY");
            await TerminalRenderer.TypeLineAsync("ATMOSPHERIC COMPOSITION: WITHIN ACCEPTABLE RANGE");
        }

        // 11. Reactor
        else if (input.Contains("REACTOR") || input.Contains("ENGINE") ||
                 input.Contains("POWER") || input.Contains("ENERGY"))
        {
            await TerminalRenderer.TypeLineAsync("REACTOR CORE: STABLE — OUTPUT 94.2%");
            await TerminalRenderer.TypeLineAsync("COOLANT SYSTEM: NOMINAL");
            await TerminalRenderer.TypeLineAsync("NO ANOMALIES DETECTED");
        }

        // 12. Communications
        else if (input.Contains("COMMS") || input.Contains("COMM") ||
                 input.Contains("SIGNAL") || input.Contains("RELAY") ||
                 input.Contains("TRANSMISSION") || input.Contains("BEACON"))
        {
            await TerminalRenderer.TypeLineAsync("DEEP SPACE RELAY: STANDBY");
            await TerminalRenderer.TypeLineAsync("NO ACTIVE COMMUNICATIONS LINK");
            await TerminalRenderer.TypeLineAsync("STANDARD BLACKOUT DURING HYPERSLEEP TRANSIT");
        }

        // 13. Company
        else if (input.Contains("WEYLAND") || input.Contains("YUTANI") ||
                 input.Contains("COMPANY") || input.Contains("CORPORATION"))
        {
            await TerminalRenderer.TypeLineAsync("WEYLAND-YUTANI CORPORATION");
            await TerminalRenderer.TypeLineAsync("BUILDING BETTER WORLDS");
            await TerminalRenderer.TypeLineAsync("ALL OPERATIONS CONDUCTED PER CORPORATE PROTOCOL.");
        }

        // 14. MU-TH-UR Identity
        else if (input.Contains("MOTHER") || input.Contains("MUTHUR") ||
                 input.Contains("WHO ARE YOU") || input.Contains("IDENTIFY") ||
                 input.Contains("COMPUTER"))
        {
            await TerminalRenderer.TypeLineAsync("I AM MU-TH-UR 6000.");
            await TerminalRenderer.TypeLineAsync("CENTRAL AI MAINFRAME — USCSS NOSTROMO.");
            await TerminalRenderer.TypeLineAsync("INTERFACE VERSION: 2037.");
            await TerminalRenderer.TypeLineAsync("HOW MAY I ASSIST YOU?");
        }

        // 15. Help
        else if (input.Contains("HELP") || input.Contains("COMMAND") ||
                 input.Contains("WHAT CAN"))
        {
            await TerminalRenderer.TypeLineAsync("DIRECT INTERFACE MODE: NATURAL LANGUAGE QUERIES ACCEPTED.");
            await TerminalRenderer.TypeLineAsync("TOPICS: CREW, SHIP, MISSION, COURSE, SYSTEMS, NAVIGATION.");
            await TerminalRenderer.TypeLineAsync("TYPE 'EXIT' TO RETURN TO STANDARD TERMINAL.");
        }

        // 16. Default
        else
        {
            await TerminalRenderer.TypeLineAsync("UNABLE TO PROCESS QUERY.");
            await TerminalRenderer.TypeLineAsync("REPHRASE OR SPECIFY TOPIC: CREW, SHIP, MISSION, SYSTEMS.");
        }

        await TerminalRenderer.TypeLineAsync("");
    }

    // ── CREW MEMBER LOOKUP ──

    /// <summary>
    /// Displays the file for the first crew member whose name matches the input.
    /// </summary>
    private static async Task DisplayCrewMemberAsync(string input)
    {
        string[] keywords = ["DALLAS", "KANE", "RIPLEY", "ASH", "LAMBERT", "PARKER", "BRETT"];

        foreach (string keyword in keywords)
        {
            if (!input.Contains(keyword))
                continue;

            var match = ShipDatabase.Crew.FirstOrDefault(c => c.Name.Contains(keyword));
            if (match == default)
                continue;

            await TerminalRenderer.TypeLineAsync($"CREW FILE: {match.Name}");
            await TerminalRenderer.TypeLineAsync($"RANK: {match.Rank}");
            await TerminalRenderer.TypeLineAsync($"STATUS: {match.Status}");

            string detail = keyword switch
            {
                "DALLAS"  => "COMMANDING OFFICER. FINAL AUTHORITY ON ALL SHIP OPERATIONS.",
                "KANE"    => "SECOND IN COMMAND. DESIGNATED SURVEY TEAM LEAD.",
                "RIPLEY"  => "CURRENTLY ACTIVE. MONITORING BRIDGE SYSTEMS.",
                "ASH"     => "ASSIGNED BY WEYLAND-YUTANI. SCIENCE DIVISION LIAISON.",
                "LAMBERT" => "PRIMARY NAVIGATOR. CERTIFIED FOR DEEP SPACE ROUTES.",
                "PARKER"  => "SUPERVISES ALL ENGINE AND MECHANICAL OPERATIONS.",
                "BRETT"   => "ENGINEERING SUPPORT. REPORTS TO CHIEF ENGINEER PARKER.",
                _         => "NO ADDITIONAL DATA ON FILE.",
            };

            await TerminalRenderer.TypeLineAsync(detail);
            return;
        }

        await TerminalRenderer.TypeLineAsync("CREW MEMBER NOT FOUND IN DATABASE.");
    }
}
