namespace muther_console_app;

/// <summary>
/// Animated idle screensaver that activates after terminal inactivity.
/// Displays a star field, pulsing ship name, and subsystem status ticker.
/// </summary>
public static class IdleScreen
{
    private struct Star
    {
        public int X;
        public int Y;
        public char Glyph;
    }

    private static readonly char[] StarGlyphs = ['·', '*'];

    /// <summary>
    /// Runs the idle screensaver animation until a key is pressed.
    /// </summary>
    public static async Task RunAsync()
    {
        var (width, height) = GetConsoleDimensions();

        // Save state and prepare screen
        Console.CursorVisible = false;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Clear();

        // ── IDLE HUM ──

        var humCts = new CancellationTokenSource();
        _ = SoundEffects.IdleHumAsync(humCts.Token);

        // ── HEADER ──

        string header = "MU-TH-UR 6000 — STANDBY MODE";
        int headerCol = Math.Max(0, (width - header.Length) / 2);
        try
        {
            Console.SetCursorPosition(headerCol, 0);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(header);
        }
        catch { /* cursor position out of bounds */ }

        // ── STAR FIELD INIT ──

        const int starCount = 30;
        var stars = new Star[starCount];
        for (int i = 0; i < starCount; i++)
        {
            stars[i] = new Star
            {
                X = Random.Shared.Next(0, width),
                Y = Random.Shared.Next(2, height - 1),
                Glyph = StarGlyphs[Random.Shared.Next(StarGlyphs.Length)]
            };
        }

        int tick = 0;
        int subsystemIndex = 0;
        bool nostromoBright = false;

        // ── MAIN ANIMATION LOOP ──

        while (!Console.KeyAvailable)
        {
            (width, height) = GetConsoleDimensions();

            // ── STAR FIELD ──

            for (int i = 0; i < starCount; i++)
            {
                // Erase old position
                try
                {
                    Console.SetCursorPosition(stars[i].X, stars[i].Y);
                    Console.Write(' ');
                }
                catch { /* out of bounds */ }

                // Move star left
                stars[i].X--;

                // Respawn at right edge if off-screen
                if (stars[i].X < 0)
                {
                    stars[i].X = width - 1;
                    stars[i].Y = Random.Shared.Next(2, Math.Max(3, height - 1));
                    stars[i].Glyph = StarGlyphs[Random.Shared.Next(StarGlyphs.Length)];
                }

                // Draw at new position
                try
                {
                    Console.SetCursorPosition(stars[i].X, stars[i].Y);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(stars[i].Glyph);
                }
                catch { /* out of bounds */ }
            }

            // ── PULSING TEXT ──

            if (tick % 40 == 0)
            {
                nostromoBright = !nostromoBright;
            }

            string shipText = "NOSTROMO";
            int shipCol = Math.Max(0, (width - shipText.Length) / 2);
            int shipRow = Math.Min(10, height - 2);
            try
            {
                Console.SetCursorPosition(shipCol, shipRow);
                Console.ForegroundColor = nostromoBright ? ConsoleColor.Green : ConsoleColor.DarkGreen;
                Console.Write(shipText);
            }
            catch { /* out of bounds */ }

            // ── STATUS TICKER ──

            if (tick % 60 == 0 && ShipDatabase.Subsystems.Length > 0)
            {
                subsystemIndex = (subsystemIndex + 1) % ShipDatabase.Subsystems.Length;
            }

            if (ShipDatabase.Subsystems.Length > 0)
            {
                var (system, status) = ShipDatabase.Subsystems[subsystemIndex];
                string ticker = $"  {system}: {status}  ";
                int tickerRow = height - 1;
                try
                {
                    Console.SetCursorPosition(0, tickerRow);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    // Pad to clear previous ticker text
                    Console.Write(ticker.PadRight(width - 1));
                }
                catch { /* out of bounds */ }
            }

            // ── RANDOM FLICKER ──

            if (Random.Shared.Next(100) < 2)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                await Task.Delay(30);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }

            tick++;
            await Task.Delay(80);
        }

        // ── CLEANUP ──

        Console.ReadKey(true);
        humCts.Cancel();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Clear();
        Console.CursorVisible = true;
    }

    private static (int Width, int Height) GetConsoleDimensions()
    {
        try { return (Console.WindowWidth, Console.WindowHeight); }
        catch { return (80, 25); }
    }
}
