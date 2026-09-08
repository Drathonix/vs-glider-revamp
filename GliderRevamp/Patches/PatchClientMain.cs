using System.Collections.Generic;
using System.Reflection.Emit;
using Vintagestory.Client.NoObf;

namespace GliderRevamp.Patches;

[HarmonyPatch(typeof(ClientMain), nameof(ClientMain.Set3DProjection), typeof(float), typeof(float))]
[HarmonyPatchCategory("vanilla")]
public class PatchGG_ModSystemGliding_OnClientTick_Fix
{
    internal static float FovScale = 1f;
    
    public static float AdjustFov(float fov)
    {
        if (!ModConfig.Instance.EnableFOVEffects)
        {
            return fov;
        }
        return fov * FovScale;
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var output = new List<CodeInstruction>
        {
            // fov = AdjustFov(fov);
            // fov is arg2 (arg0 is this, arg1 is zfar)
            new(OpCodes.Ldarg_2),
            new(
                OpCodes.Call,
                AccessTools.Method(typeof(PatchGG_ModSystemGliding_OnClientTick_Fix), nameof(AdjustFov))
            ),
            new(OpCodes.Starg_S, 2)
        };

        output.AddRange(instructions);
        return output;
    }
}