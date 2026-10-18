using GliderRevamp.Util;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Vintagestory.Client.NoObf;

namespace GliderRevamp.Patches.Vanilla;

/// <summary>
/// Fixes a vanilla bug which incorrectly renders the player's model while gliding in first person - This bug may be more extensive than just gliding animations but its not too important to me.
/// </summary>
[HarmonyPatch(typeof(EntityPlayer), "OnGameTick")]
public class PatchEntityPlayer
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // IL_009a: ldfld        bool Vintagestory.API.Common.EntityPlayer/*02000389*/::selfNowShadowPass/*04001159*/

        return Harmixin.Splice(instructions,
            [new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(EntityPlayer),"selfNowShadowPass"))],
            [new CodeInstruction(OpCodes.Call,AccessTools.Method(typeof(PatchEntityPlayer), "SpecShadowPass"))],
            1);
    }
    // Result is notted
    internal static bool SpecShadowPass(EntityPlayer __instance)
    {
        return __instance.selfNowShadowPass || __instance.Controls.Gliding;
    }
}
