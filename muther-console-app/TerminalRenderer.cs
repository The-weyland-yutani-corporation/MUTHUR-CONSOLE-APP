namespace muther_console_app;

/// <summary>
/// Handles all retro CRT-style rendering effects:
/// typewriter text, flicker, scan-line glitches, and cursor blinking.
/// </summary>
public static class TerminalRenderer
{
    private static readonly Random Rng = new();

    // Typing speed range in milliseconds per character
    private const int MinCharDelay = 8;
    private const int MaxCharDelay = 35;
    private const int LineEndPause = 80;

    /// <summary>
    /// Prints text character-by-character with a typewriter effect.
    /// </summary>
    public static async Task TypeLineAsync(string text, bool bright = false, bool slow = false)
    {
        Console.ForegroundColor = bright ? ConsoleColor.Green : ConsoleColor.DarkGreen;

        int min = slow ? MinCharDelay * 3 : MinCharDelay;
        int max = slow ? MaxCharDelay * 3 : MaxCharDelay;

        foreach (char c in text)
        {
            Console.Write(c);
            await Task.Delay(Rng.Next(min, max));
        }

        Console.WriteLine();
        await Task.Delay(LineEndPause);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
    }

    /// <summary>
    /// Prints a block of text line-by-line with the typewriter effect.
    /// </summary>
    public static async Task TypeBlockAsync(string[] lines, bool bright = false)
    {
        foreach (string line in lines)
        {
            await TypeLineAsync(line, bright);
        }
    }

    /// <summary>
    /// Prints text instantly (no animation) in the terminal style.
    /// </summary>
    public static void PrintImmediate(string text, bool bright = false)
    {
        Console.ForegroundColor = bright ? ConsoleColor.Green : ConsoleColor.DarkGreen;
        Console.WriteLine(text);
    }

    /// <summary>
    /// Simulates a brief CRT flicker / scan-line glitch.
    /// </summary>
    public static async Task FlickerAsync(int intensity = 1)
    {
        for (int i = 0; i < intensity; i++)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            await Task.Delay(Rng.Next(30, 80));
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            await Task.Delay(Rng.Next(50, 120));
        }
    }

    /// <summary>
    /// Displays a blinking cursor prompt and waits for user input.
    /// </summary>
    public static string ReadCommand(string prompt = "> ")
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(prompt);
        string? input = Console.ReadLine();
        return input?.Trim().ToUpperInvariant() ?? string.Empty;
    }

    /// <summary>
    /// Clears the screen and resets to the terminal color scheme.
    /// </summary>
    public static void ClearScreen()
    {
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
    }

    /// <summary>
    /// Draws a horizontal rule.
    /// </summary>
    public static async Task DrawRuleAsync(char c = '─', int width = 60)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(new string(c, width));
        await Task.Delay(40);
    }

    /// <summary>
    /// Prints a blank line pause to pace output.
    /// </summary>
    public static async Task PauseAsync(int ms = 400)
    {
        await Task.Delay(ms);
    }

    /// <summary>
    /// Attempts a console beep (swallows exceptions on unsupported platforms).
    /// </summary>
    public static void Beep(int frequency = 800, int duration = 100)
    {
        try { Console.Beep(frequency, duration); }
        catch { /* unsupported platform */ }
    }
}
