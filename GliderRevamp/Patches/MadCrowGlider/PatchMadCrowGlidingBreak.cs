namespace GliderRevamp.Patches.MadCrowGlider;
public class PatchMadCrowGlidingBreak
{
    public static void Postfix(ICoreClientAPI capi, ref bool __result)
    {
        float canGlide = capi.World.Player.Entity.Stats.GetBlended("canGlide");
        if (canGlide > 1)
        {
            __result = true;
        }
    }
}