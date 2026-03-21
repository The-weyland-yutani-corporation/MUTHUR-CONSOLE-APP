namespace muther_console_app;

/// <summary>
/// Provides platform-safe Console.Beep tone patterns for the MU-TH-UR 6000 terminal.
/// </summary>
public static class SoundEffects
{
    // ── PLATFORM HELPER ──

    /// <summary>
    /// Wraps Console.Beep in a try-catch so unsupported platforms fail silently.
    /// </summary>
    private static void SafeBeep(int frequency, int duration)
    {
        try { Console.Beep(frequency, duration); }
        catch { /* unsupported platform or terminal */ }
    }

    // ── EMERGENCY TONES ──

    /// <summary>
    /// Emergency klaxon for self-destruct and critical alerts.
    /// Alternating 400 Hz / 800 Hz tones, 3 cycles.
    /// </summary>
    public static Task KlaxonAsync()
    {
        return Task.Run(() =>
        {
            for (int i = 0; i < 3; i++)
            {
                SafeBeep(400, 150);
                SafeBeep(800, 150);
            }
        });
    }

    /// <summary>
    /// Warning alert tone for Special Order 937 and sensitive data access.
    /// Rising 3-tone sweep: 400 Hz → 600 Hz → 800 Hz.
    /// </summary>
    public static Task AlertToneAsync()
    {
        return Task.Run(() =>
        {
            SafeBeep(400, 120);
            SafeBeep(600, 120);
            SafeBeep(800, 120);
        });
    }

    /// <summary>
    /// Self-destruct countdown tick with escalating pitch as time decreases.
    /// </summary>
    /// <param name="secondsRemaining">Seconds left on the countdown timer.</param>
    public static Task CountdownTickAsync(int secondsRemaining)
    {
        return Task.Run(() =>
        {
            int frequency = 600 + (10 - secondsRemaining) * 80;
            SafeBeep(frequency, 80);
        });
    }

    // ── FEEDBACK TONES ──

    /// <summary>
    /// Quick high-pitched blip for MOTHER mode response acknowledgement.
    /// </summary>
    public static Task DataBlipAsync()
    {
        return Task.Run(() =>
        {
            SafeBeep(1200, 30);
        });
    }

    /// <summary>
    /// Ascending 4-tone chime played at the end of the boot sequence.
    /// </summary>
    public static Task BootChimeAsync()
    {
        return Task.Run(() =>
        {
            SafeBeep(400, 100);
            Thread.Sleep(30);
            SafeBeep(600, 100);
            Thread.Sleep(30);
            SafeBeep(800, 100);
            Thread.Sleep(30);
            SafeBeep(1000, 100);
        });
    }

    /// <summary>
    /// Low harsh buzz for unknown or invalid commands.
    /// </summary>
    public static Task ErrorBuzzAsync()
    {
        return Task.Run(() =>
        {
            SafeBeep(150, 200);
        });
    }

    // ── AMBIENT TONES ──

    /// <summary>
    /// Repeating low-frequency hum for the idle terminal screen.
    /// Loops until the cancellation token is triggered.
    /// </summary>
    /// <param name="token">Token to stop the idle hum loop.</param>
    public static async Task IdleHumAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            SafeBeep(200, 100);
            try
            {
                await Task.Delay(2000, token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
