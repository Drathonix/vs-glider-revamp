using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace GliderRevamp.Patches.Vanilla;

[HarmonyPatch(typeof(PModulePlayerInAir), "ApplyFlying")]
public class PatchPModulePlayerInAir
{
    public static bool Prefix(PModulePlayerInAir __instance, float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        if (!GliderRevampModSystem.EVENTS.InvokeBeforePhysicsCalculations(__instance,dt,entity,pos,controls))
        {
            ReversePatch.ApplyFlying(__instance, dt, entity, pos, controls);
            return false;
        }
        return false;
    }
}

[HarmonyPatch(typeof(PModuleInAir), "ApplyFlying")]
[HarmonyPatchCategory("reversepatch")]

public class ReversePatch
{
    [HarmonyReversePatch, MethodImpl(MethodImplOptions.NoInlining)]
    public static void ApplyFlying(PModuleInAir __instance, float dt, Entity entity, EntityPos pos, EntityControls controls) { }
}
