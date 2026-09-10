using GliderRevamp.Patches.MadCrowGlider;
using MadCrowGliderMod;
using System;
using System.Collections.Generic;
using System.Text;

namespace GliderRevamp;

internal class MadCrowPatches
{
    internal static void Run(Harmony _harmony)
    {
        _harmony.Patch(AccessTools.Method(typeof(GG_ModSystemGliding_OnClientTick_Fix), "LocalPlayerHasGlider"), null, AccessTools.Method(typeof(PatchMadCrowGlidingBreak), "Postfix"));
    }
}
