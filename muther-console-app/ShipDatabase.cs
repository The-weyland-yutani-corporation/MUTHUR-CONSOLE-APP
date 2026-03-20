namespace muther_console_app;

/// <summary>
/// Canon-accurate data for the USCSS Nostromo and its mission.
/// Source: Alien (1979), Aliens (1986), and related canon materials.
/// </summary>
public static class ShipDatabase
{
    // ── Ship ──────────────────────────────────────────────
    public const string ShipName = "USCSS NOSTROMO";
    public const string ShipRegistration = "REG 180924609";
    public const string ShipClass = "LOCKMART CM 88B BISON M-CLASS STARFREIGHTER";
    public const string RefinerySuffix = "TOWING REFINERY MODULE: 20,000,000 TONNES ORE PROCESSING";
    public const string ComputerName = "MU-TH-UR 6000";
    public const string InterfaceVersion = "INTERFACE 2037";
    public const string CompanyName = "WEYLAND-YUTANI CORPORATION";
    public const string CompanySlogan = "BUILDING BETTER WORLDS";

    // ── Mission ───────────────────────────────────────────
    public const string MissionOrigin = "THEDUS";
    public const string MissionDestination = "EARTH";
    public const string CargoDescription = "20,000,000 TONNES MINERAL ORE";
    public const string MissionStatus = "IN TRANSIT — CREW IN HYPERSLEEP";

    // ── Crew ──────────────────────────────────────────────
    public static readonly (string Name, string Rank, string Status)[] Crew =
    [
        ("DALLAS, A.J.",     "CAPTAIN",                 "HYPERSLEEP"),
        ("KANE, T.G.",       "EXECUTIVE OFFICER",       "HYPERSLEEP"),
        ("RIPLEY, E.L.",     "WARRANT OFFICER",         "ACTIVE"),
        ("ASH",              "SCIENCE OFFICER",         "HYPERSLEEP"),
        ("LAMBERT, J.M.",    "NAVIGATOR",               "HYPERSLEEP"),
        ("PARKER, D.",       "CHIEF ENGINEER",          "HYPERSLEEP"),
        ("BRETT, S.E.",      "ENGINEERING TECHNICIAN",  "HYPERSLEEP"),
    ];

    // ── Special Order 937 ────────────────────────────────
    public static readonly string[] SpecialOrder937 =
    [
        "",
        "PRIORITY ONE",
        "ALL OTHER PRIORITIES RESCINDED.",
        "",
        "SPECIAL ORDER 937",
        "",
        "ENSURE RETURN OF ORGANISM FOR ANALYSIS.",
        "ALL OTHER CONSIDERATIONS SECONDARY.",
        "CREW EXPENDABLE.",
        "",
        "— WEYLAND-YUTANI CORP, SPECIAL PROJECTS DIVISION",
        "",
    ];

    // ── Self Destruct ────────────────────────────────────
    public const string SelfDestructSystem = "EMERGENCY ORDER 937-OMEGA";
    public const int SelfDestructCountdownSeconds = 10;

    // ── Subsystems ───────────────────────────────────────
    public static readonly (string System, string Status)[] Subsystems =
    [
        ("LIFE SUPPORT",       "NOMINAL"),
        ("HULL INTEGRITY",     "100%"),
        ("REACTOR CORE",       "STABLE — OUTPUT 94.2%"),
        ("NAVIGATION",         "AUTO — ON COURSE"),
        ("COMMUNICATIONS",     "DEEP SPACE RELAY — STANDBY"),
        ("HYPERSLEEP VAULTS",  "6 OF 7 OCCUPIED"),
        ("CARGO LOCK",         "SEALED"),
        ("AIRLOCK SYSTEMS",    "SECURED"),
        ("MU-TH-UR 6000",     "ONLINE"),
    ];

    // ── Weyland-Yutani ASCII Logo ────────────────────────
    public static readonly string[] Logo =
    [
        @"  ██╗    ██╗ ██████╗██╗   ██╗ ",
        @"  ██║    ██║██╔════╝╚██╗ ██╔╝ ",
        @"  ██║ █╗ ██║╚█████╗  ╚████╔╝  ",
        @"  ██║███╗██║ ╚═══██╗  ╚██╔╝   ",
        @"  ╚███╔███╔╝██████╔╝   ██║    ",
        @"   ╚══╝╚══╝ ╚═════╝    ╚═╝    ",
        @"   WEYLAND-YUTANI CORPORATION  ",
        @"     BUILDING BETTER WORLDS    ",
    ];
}
