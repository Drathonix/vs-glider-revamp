using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common.Entities;

namespace GliderRevamp.Patches.Vanilla;

[HarmonyPatch(typeof(PModuleGravity),"DoApply")]
public class PatchPModuleGravity
{
    public static bool Prefix(PModuleGravity __instance, float dt, Entity entity, EntityPos pos, EntityControls controls)
    {
        return GliderRevampModSystem.EVENTS.InvokeShouldApplyGravity(dt, entity, pos, controls);
    }
}
