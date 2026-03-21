namespace muther_console_app;

/// <summary>
/// Plays the MU-TH-UR 6000 POST / boot animation.
/// </summary>
public static class BootSequence
{
    /// <summary>
    /// Plays the full POST boot animation: CRT warm-up, logo, memory check, subsystem verification, and ready prompt.
    /// </summary>
    public static async Task PlayAsync()
    {
        TerminalRenderer.ClearScreen();

        // ── CRT warm-up flicker ──
        await TerminalRenderer.FlickerAsync(3);
        await TerminalRenderer.PauseAsync(600);

        // ── Weyland-Yutani logo ──
        foreach (string line in ShipDatabase.Logo)
        {
            TerminalRenderer.PrintImmediate(line, bright: true);
            await Task.Delay(90);
        }

        await TerminalRenderer.PauseAsync(1200);
        await TerminalRenderer.DrawRuleAsync();

        // ── POST sequence ──
        string[] postLines =
        [
            $"{ShipDatabase.CompanyName}",
            $"{ShipDatabase.ComputerName} — {ShipDatabase.InterfaceVersion}",
            "",
            "PERFORMING SYSTEM SELF-TEST...",
        ];
        await TerminalRenderer.TypeBlockAsync(postLines);
        await TerminalRenderer.PauseAsync(400);

        // Memory check with incrementing counter
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        string memPrefix = "  MEMORY CHECK: ";
        Console.Write(memPrefix);
        int[] memSteps = [4096, 16384, 65536, 262144, 524288, 1048576];
        foreach (int kb in memSteps)
        {
            Console.Write($"\r{memPrefix}{kb,10} KB OK");
            await Task.Delay(120);
        }
        Console.WriteLine();

        // Subsystem checks
        string[] checks =
        [
            "  CPU CORE ........................ OK",
            "  NAVIGATION ARRAY ............... OK",
            "  LIFE SUPPORT ................... OK",
            "  REACTOR CORE ................... OK",
            "  HYPERSLEEP VAULTS .............. OK",
            "  COMM RELAY ..................... STANDBY",
            "  HULL SENSORS ................... OK",
            "  AIRLOCK CONTROL ................ SECURED",
        ];

        foreach (string check in checks)
        {
            await TerminalRenderer.TypeLineAsync(check);
        }

        await TerminalRenderer.PauseAsync(500);
        await TerminalRenderer.DrawRuleAsync();

        // ── Ready ──
        string[] ready =
        [
            "",
            $"  SHIP:        {ShipDatabase.ShipName}",
            $"  REG:         {ShipDatabase.ShipRegistration}",
            $"  CLASS:       {ShipDatabase.ShipClass}",
            $"  STATUS:      {ShipDatabase.MissionStatus}",
            "",
        ];
        await TerminalRenderer.TypeBlockAsync(ready);

        await SoundEffects.BootChimeAsync();

        await TerminalRenderer.TypeLineAsync(
            $"{ShipDatabase.InterfaceVersion} READY FOR INQUIRY.", bright: true);
        await TerminalRenderer.TypeLineAsync(
            "TYPE 'HELP' FOR AVAILABLE COMMANDS.", bright: true);

        await TerminalRenderer.DrawRuleAsync();
        Console.WriteLine();
    }
}
