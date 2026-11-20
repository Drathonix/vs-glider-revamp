using GliderRevamp.Util;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Vintagestory.API.Common.Entities;
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

/// <summary>
/// Fixes roll being applied to the wrong rotation axis because it was being applied too early in rotation math (scuffed but it works for the time being)
/// </summary>
[HarmonyPatch(typeof(EntityPlayerShapeRenderer), "loadModelMatrixForPlayer")]
public class PatchEntityPlayerShapeRenderer2
{
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var targetMethod = AccessTools.Method(typeof(Mat4f),"RotateX");
        var replMethod = AccessTools.Method(typeof(PatchEntityPlayerShapeRenderer2), "SpecRotateX");
        var replMethod2 = AccessTools.Method(typeof(PatchEntityPlayerShapeRenderer2), "SpecRotateX2");

        var code = new List<CodeInstruction>(instructions);
        int phase = 0;
        for (int i = 0; i < code.Count; i++)
        {
            var inst = code[i];
            if (phase == 0 && inst.Calls(targetMethod))
            {
                code[i] = new CodeInstruction(OpCodes.Call, replMethod);
                phase = 1;
            } else if(phase == 1 && inst.Calls(targetMethod))
            {
                code[i] = new CodeInstruction(OpCodes.Ldarg_1);
                code.Insert(i+1, new CodeInstruction(OpCodes.Call, replMethod2));
                phase = 2;
            }
        }
        return code;
    }

    public static float[] SpecRotateX(float[] output, float[] a, float rad)
    {
        return output;
    }

    public static float[] SpecRotateX2(float[] output, float[] a, float rad, Entity entity)
    {
        Mat4f.RotateX(output,a,rad);
        //Mat4f.Translate(output, a, 0f, 1f, 1f);
        Mat4f.RotateX(output,a, entity.Pos.Roll * ((float)Math.PI / 180f));
        //Mat4f.Translate(output, a, 0f, -1f, -1f);
        return output;
    }

}
