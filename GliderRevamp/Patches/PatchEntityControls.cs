using System;
using System.Collections.Generic;
using System.Text;

namespace GliderRevamp.Patches;

/// <summary>
/// By Drathon
/// Fixes a vanilla bug where the glide animation does not start when for other players in multiplayer.
/// </summary>
[HarmonyPatch(typeof(EntityControls), "FromInt")]
public class PatchEntityControls
{
    public static void Postfix(EntityControls __instance, int flagsInt)
    {
        if (__instance.Gliding)
        {
            __instance.IsFlying = true;
        }
    }
}
