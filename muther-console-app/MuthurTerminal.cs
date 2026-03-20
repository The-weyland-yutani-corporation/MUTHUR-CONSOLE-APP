namespace muther_console_app;

/// <summary>
/// Main terminal session loop for the MU-TH-UR 6000 interface.
/// </summary>
public class MuthurTerminal
{
    /// <summary>
    /// Initializes the console, runs the boot sequence, and enters the main command loop.
    /// </summary>
    public async Task RunAsync()
    {
        InitConsole();
        await BootSequence.PlayAsync();

        bool running = true;
        while (running)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string input = TerminalRenderer.ReadCommand($"{ShipDatabase.ComputerName}> ");
            running = await CommandProcessor.ExecuteAsync(input);
        }

        // Final cleanup
        Console.ResetColor();
        Console.Clear();
    }

    /// <summary>
    /// Configures console title, colors, cursor, and window dimensions for the CRT experience.
    /// </summary>
    private static void InitConsole()
    {
        Console.Title = $"{ShipDatabase.ComputerName} — {ShipDatabase.InterfaceVersion}";
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Clear();
        Console.CursorVisible = true;

        // Maximize the window for immersion if possible
        try
        {
            Console.WindowWidth = Math.Min(100, Console.LargestWindowWidth);
            Console.WindowHeight = Math.Min(40, Console.LargestWindowHeight);
        }
        catch
        {
            // Not all terminals support resizing — safe to ignore
        }
    }
}
