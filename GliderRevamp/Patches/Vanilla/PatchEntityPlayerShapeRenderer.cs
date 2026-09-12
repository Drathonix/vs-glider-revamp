using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Vintagestory.API.MathTools;
using static HarmonyLib.Code;

namespace GliderRevamp.Patches.Vanilla;

/// <summary>
/// Fixes incorrect pitch rendering for other players while gliding.
/// </summary>
[HarmonyPatch(typeof(EntityPlayerShapeRenderer), "loadModelMatrixForPlayer")]
public class PatchEntityPlayerShapeRenderer
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var targetMethod = typeof(Mat4f).GetMethod("RotateZ");
        var replMethod = typeof(PatchEntityPlayerShapeRenderer).GetMethod("SpecRotateZ");

        var code = new List<CodeInstruction>(instructions);
        for (int i = 0; i < code.Count; i++)
        {
            var inst = code[i];
            if (inst.Calls(targetMethod))
            {
                code[i] = new CodeInstruction(OpCodes.Ldarg_0);
                code.Insert(i + 1, new CodeInstruction(OpCodes.Ldarg_2));
                code.Insert(i + 2, new CodeInstruction(OpCodes.Call, replMethod));
                break;
            }
        }
        return code;
    }

    public static float[] SpecRotateZ(float[] output, float[] a, float rad, EntityPlayerShapeRenderer instance, bool isSelf)
    {
        if (!isSelf && instance.entity is EntityAgent agent && agent.Controls.Gliding)
        {
            return Mat4f.RotateZ(output, a, rad + (float.Pi + instance.entity.Pos.Pitch));
        } else {
            return Mat4f.RotateZ(output, a, rad);
        }
    }
}
