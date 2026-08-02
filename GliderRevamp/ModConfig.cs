
namespace GliderRevamp;

[UsedImplicitly]
public sealed class ModConfig
{
    public static ModConfig Instance { get; set; } = new();

    // ============================================================
    // Core toggles
    // ============================================================

    public bool DisableGlider { get; set; } = false;
    public bool ShowSpeed { get; set; } = false;
    public bool EnableSoundModulation { get; set; } = true;
    public bool EnableFOVEffects { get; set; } = true;

    // ============================================================
    // Flight Physics
    // ============================================================

    public float ClimbCoefficiency { get; set; } = 0.2f;

    public float TurnRate { get; set; } = 90f;  // degrees per second

    public float DragCoefficiency { get; set; } = 0.1f;

    public float StallSpeedMs { get; set; } = 6f;
    
    public float ActivationSpeedMs { get; set; } = 8f;

    public float TerminalVelocityMs { get; set; } = 40f;  // m/s

    
}
