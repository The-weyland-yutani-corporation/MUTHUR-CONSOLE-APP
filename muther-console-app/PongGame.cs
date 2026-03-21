namespace muther_console_app;

/// <summary>
/// Hidden ASCII Pong game — a recreational subroutine buried
/// in the MU-TH-UR 6000 mainframe, playable against an AI opponent.
/// </summary>
public static class PongGame
{
    // ── CONSTANTS ─────────────────────────────────────────

    private const int FieldWidth = 60;
    private const int FieldHeight = 20;
    private const int PaddleHeight = 4;
    private const int PlayerPaddleX = 2;
    private const int AiPaddleX = 57;
    private const int WinningScore = 5;
    private const int TickDelay = 60;

    // ── BOX-DRAWING CHARACTERS ────────────────────────────

    private const char BorderHorizontal = '─';
    private const char BorderVertical = '│';
    private const char CornerTopLeft = '┌';
    private const char CornerTopRight = '┐';
    private const char CornerBottomLeft = '└';
    private const char CornerBottomRight = '┘';
    private const char CenterDash = '¦';
    private const char BallChar = '●';
    private const char PaddleChar = '█';

    /// <summary>
    /// Runs a hidden ASCII Pong game against an AI opponent.
    /// </summary>
    public static async Task RunAsync()
    {
        // ── GAME STATE ────────────────────────────────────

        int playerScore = 0;
        int aiScore = 0;
        bool quit = false;

        int ballX = FieldWidth / 2;
        int ballY = FieldHeight / 2;
        int ballDx = Random.Shared.Next(2) == 0 ? 1 : -1;
        int ballDy = Random.Shared.Next(2) == 0 ? 1 : -1;
        int prevBallX = ballX;
        int prevBallY = ballY;

        int playerPaddleY = FieldHeight / 2 - PaddleHeight / 2;
        int aiPaddleY = FieldHeight / 2 - PaddleHeight / 2;
        int prevPlayerPaddleY = playerPaddleY;
        int prevAiPaddleY = aiPaddleY;

        // ── INTRO SEQUENCE ────────────────────────────────

        await ShowIntroAsync();

        // ── PREPARE SCREEN ────────────────────────────────

        bool cursorWasVisible = true;
        try
        {
            cursorWasVisible = Console.CursorVisible;
        }
        catch { /* unsupported platform */ }

        try
        {
            Console.CursorVisible = false;
        }
        catch { /* unsupported platform */ }

        TerminalRenderer.ClearScreen();
        DrawField();
        DrawScore(playerScore, aiScore);
        DrawPaddle(PlayerPaddleX, playerPaddleY);
        DrawPaddle(AiPaddleX, aiPaddleY);
        DrawBall(ballX, ballY);

        // ── MAIN GAME LOOP ────────────────────────────────

        while (playerScore < WinningScore && aiScore < WinningScore && !quit)
        {
            // ── INPUT ─────────────────────────────────────

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (playerPaddleY > 1)
                            playerPaddleY--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (playerPaddleY + PaddleHeight < FieldHeight - 1)
                            playerPaddleY++;
                        break;
                    case ConsoleKey.Q:
                        quit = true;
                        continue;
                }
            }

            // ── UPDATE AI ─────────────────────────────────

            if (ballX > FieldWidth / 2 && Random.Shared.Next(100) >= 30)
            {
                int aiCenter = aiPaddleY + PaddleHeight / 2;
                if (ballY < aiCenter && aiPaddleY > 1)
                    aiPaddleY--;
                else if (ballY > aiCenter && aiPaddleY + PaddleHeight < FieldHeight - 1)
                    aiPaddleY++;
            }

            // ── UPDATE BALL ───────────────────────────────

            prevBallX = ballX;
            prevBallY = ballY;

            int nextBallX = ballX + ballDx;
            int nextBallY = ballY + ballDy;

            // Top / bottom wall bounce
            if (nextBallY <= 0 || nextBallY >= FieldHeight - 1)
            {
                ballDy = -ballDy;
                nextBallY = ballY + ballDy;
                TerminalRenderer.Beep(600, 30);
            }

            // Player paddle collision
            if (nextBallX <= PlayerPaddleX + 1 &&
                ballX > PlayerPaddleX + 1 &&
                nextBallY >= playerPaddleY &&
                nextBallY < playerPaddleY + PaddleHeight)
            {
                ballDx = -ballDx;
                nextBallX = PlayerPaddleX + 2;
                ballDy = ComputeBounceDy(nextBallY, playerPaddleY);
                TerminalRenderer.Beep(800, 30);
            }
            // AI paddle collision
            else if (nextBallX >= AiPaddleX - 1 &&
                     ballX < AiPaddleX - 1 &&
                     nextBallY >= aiPaddleY &&
                     nextBallY < aiPaddleY + PaddleHeight)
            {
                ballDx = -ballDx;
                nextBallX = AiPaddleX - 2;
                ballDy = ComputeBounceDy(nextBallY, aiPaddleY);
                TerminalRenderer.Beep(800, 30);
            }

            ballX = nextBallX;
            ballY = nextBallY;

            // ── SCORING ───────────────────────────────────

            bool scored = false;

            if (ballX <= 0)
            {
                aiScore++;
                scored = true;
            }
            else if (ballX >= FieldWidth - 1)
            {
                playerScore++;
                scored = true;
            }

            if (scored)
            {
                TerminalRenderer.Beep(400, 200);
                DrawScore(playerScore, aiScore);
                await Task.Delay(800);

                // Reset ball to center with random direction
                ballX = FieldWidth / 2;
                ballY = FieldHeight / 2;
                ballDx = Random.Shared.Next(2) == 0 ? 1 : -1;
                ballDy = Random.Shared.Next(2) == 0 ? 1 : -1;
                prevBallX = ballX;
                prevBallY = ballY;

                // Flush any keys pressed during the pause
                while (Console.KeyAvailable) Console.ReadKey(true);
            }

            // ── RENDER ────────────────────────────────────

            // Clear old positions
            ClearBall(prevBallX, prevBallY);

            if (prevPlayerPaddleY != playerPaddleY)
            {
                ClearPaddle(PlayerPaddleX, prevPlayerPaddleY);
            }

            if (prevAiPaddleY != aiPaddleY)
            {
                ClearPaddle(AiPaddleX, prevAiPaddleY);
            }

            // Redraw center line cells that the ball may have erased
            RepairCenterLine(prevBallX, prevBallY);

            // Draw new positions
            DrawPaddle(PlayerPaddleX, playerPaddleY);
            DrawPaddle(AiPaddleX, aiPaddleY);
            DrawBall(ballX, ballY);

            prevPlayerPaddleY = playerPaddleY;
            prevAiPaddleY = aiPaddleY;

            await Task.Delay(TickDelay);
        }

        // ── GAME OVER ─────────────────────────────────────

        if (!quit)
        {
            TerminalRenderer.Beep(1000, 300);
        }

        try
        {
            Console.CursorVisible = cursorWasVisible;
        }
        catch { /* unsupported platform */ }

        await ShowExitAsync(playerScore, aiScore, quit);
    }

    // ── INTRO / EXIT ──────────────────────────────────────

    private static async Task ShowIntroAsync()
    {
        TerminalRenderer.ClearScreen();
        await TerminalRenderer.TypeLineAsync("  *** RECREATIONAL SUBROUTINE DETECTED ***", bright: true);
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync("  LOADING PONG.EXE FROM ARCHIVE...");
        await TerminalRenderer.PauseAsync(800);
        await TerminalRenderer.TypeLineAsync("  CONTROLS: W/S OR ARROW KEYS — Q TO QUIT");
        await TerminalRenderer.TypeLineAsync("  FIRST TO 5 WINS.");
        await TerminalRenderer.TypeLineAsync("");
        await TerminalRenderer.TypeLineAsync("  PRESS ANY KEY TO START.", bright: true);
        Console.ReadKey(true);
    }

    private static async Task ShowExitAsync(int playerScore, int aiScore, bool quit)
    {
        TerminalRenderer.ClearScreen();

        if (!quit)
        {
            if (playerScore >= WinningScore)
            {
                await TerminalRenderer.TypeLineAsync(
                    "  IMPRESSIVE. YOUR REFLEXES EXCEED EXPECTED PARAMETERS.", bright: true);
            }
            else
            {
                await TerminalRenderer.TypeLineAsync(
                    "  MU-TH-UR 6000 REMAINS UNDEFEATED. BETTER LUCK NEXT CYCLE.", bright: true);
            }

            await TerminalRenderer.TypeLineAsync($"  FINAL SCORE — PLAYER: {playerScore}  MU-TH-UR: {aiScore}");
            await TerminalRenderer.PauseAsync(1500);
        }

        TerminalRenderer.ClearScreen();
        await TerminalRenderer.TypeLineAsync("  RECREATIONAL SUBROUTINE TERMINATED.", bright: true);
        await TerminalRenderer.TypeLineAsync("  RETURNING TO MAIN INTERFACE.");
        await TerminalRenderer.PauseAsync(500);
        TerminalRenderer.ClearScreen();
    }

    // ── BALL PHYSICS ──────────────────────────────────────

    private static int ComputeBounceDy(int ballY, int paddleY)
    {
        int hitOffset = ballY - paddleY;

        if (hitOffset <= 0)
            return -1;
        else if (hitOffset >= PaddleHeight - 1)
            return 1;
        else
            return 0;
    }

    // ── DRAWING HELPERS ───────────────────────────────────

    private static void DrawField()
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;

        // Top border
        SafeSetCursor(0, 0);
        Console.Write(CornerTopLeft);
        Console.Write(new string(BorderHorizontal, FieldWidth - 2));
        Console.Write(CornerTopRight);

        // Side borders and center line
        for (int y = 1; y < FieldHeight - 1; y++)
        {
            SafeSetCursor(0, y);
            Console.Write(BorderVertical);

            SafeSetCursor(FieldWidth / 2, y);
            Console.Write(y % 2 == 0 ? CenterDash : ' ');

            SafeSetCursor(FieldWidth - 1, y);
            Console.Write(BorderVertical);
        }

        // Bottom border
        SafeSetCursor(0, FieldHeight - 1);
        Console.Write(CornerBottomLeft);
        Console.Write(new string(BorderHorizontal, FieldWidth - 2));
        Console.Write(CornerBottomRight);
    }

    private static void DrawScore(int playerScore, int aiScore)
    {
        string scoreText = $"PLAYER: {playerScore}    MU-TH-UR: {aiScore}";
        int x = (FieldWidth - scoreText.Length) / 2;

        Console.ForegroundColor = ConsoleColor.Green;
        SafeSetCursor(x, 0);
        Console.Write(scoreText);
    }

    private static void DrawBall(int x, int y)
    {
        if (x <= 0 || x >= FieldWidth - 1 || y <= 0 || y >= FieldHeight - 1)
            return;

        Console.ForegroundColor = ConsoleColor.Green;
        SafeSetCursor(x, y);
        Console.Write(BallChar);
    }

    private static void ClearBall(int x, int y)
    {
        if (x <= 0 || x >= FieldWidth - 1 || y <= 0 || y >= FieldHeight - 1)
            return;

        SafeSetCursor(x, y);
        Console.Write(' ');
    }

    private static void DrawPaddle(int x, int y)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < PaddleHeight; i++)
        {
            int py = y + i;
            if (py > 0 && py < FieldHeight - 1)
            {
                SafeSetCursor(x, py);
                Console.Write(PaddleChar);
            }
        }
    }

    private static void ClearPaddle(int x, int y)
    {
        for (int i = 0; i < PaddleHeight; i++)
        {
            int py = y + i;
            if (py > 0 && py < FieldHeight - 1)
            {
                SafeSetCursor(x, py);
                Console.Write(' ');
            }
        }
    }

    private static void RepairCenterLine(int ballX, int ballY)
    {
        if (ballX == FieldWidth / 2 && ballY > 0 && ballY < FieldHeight - 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            SafeSetCursor(FieldWidth / 2, ballY);
            Console.Write(ballY % 2 == 0 ? CenterDash : ' ');
        }
    }

    private static void SafeSetCursor(int x, int y)
    {
        try
        {
            Console.SetCursorPosition(x, y);
        }
        catch
        {
            // Console buffer too small or unsupported platform
        }
    }
}
