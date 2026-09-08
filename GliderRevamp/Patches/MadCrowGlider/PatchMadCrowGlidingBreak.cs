using MadCrowGliderMod;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GliderRevamp.Patches.MadCrowGlider;

[HarmonyPatch(typeof(GG_ModSystemGliding_OnClientTick_Fix), "LocalPlayerHasGlider")]
[HarmonyPatchCategory("madcrowglider")]
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