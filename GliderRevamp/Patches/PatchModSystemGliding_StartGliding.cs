namespace GliderRevamp.Patches;

[HarmonyPatch(typeof(ModSystemGliding), "Input_InWorldAction")]
public class PatchModSystemGliding_StartGliding
{
    private static readonly AccessTools.FieldRef<ModSystemGliding, ICoreClientAPI> CapiRef
        = AccessTools.FieldRefAccess<ModSystemGliding, ICoreClientAPI>("capi");    
    public static bool Prefix(ModSystemGliding __instance, EnumEntityAction action, bool on, ref EnumHandling handled)
    {
        var entity = CapiRef(__instance)?.World.Player.Entity;
        if (entity == null)
        {
            return false;
        }
        if(!GliderEvents.InvokeTryStartGlide(__instance, entity))
        {
            return false;
        }
        return true;
    }
}