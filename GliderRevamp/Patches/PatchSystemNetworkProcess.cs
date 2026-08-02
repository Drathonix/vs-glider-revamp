using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Vintagestory.Client.NoObf;

namespace GliderRevamp.Patches;

[HarmonyPatch(typeof(SystemNetworkProcess), "HandleSinglePacket", [typeof(Packet_EntityPosition)])]
public class PatchSystemNetworkProcess
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var targetMethod = typeof(EntityControls).GetMethod("FromInt");
        var replMethod = typeof(PatchSystemNetworkProcess).GetMethod("FromInt");

        var code = new List<CodeInstruction>(instructions);
        for (int i = 0; i < code.Count; i++)
        {
            var inst = code[i];
            if (inst.Calls(targetMethod))
            {
                code[i] = new CodeInstruction(OpCodes.Ldloc_2);
                code.Insert(i + 1, new CodeInstruction(OpCodes.Call, replMethod));
            }
        }
        return code;
    }

    public static void FromInt(EntityControls instance, int i, EntityAgent agent)
    {
        instance.FromInt(i);
        if(agent is EntityPlayer player)
        {
            IPlayer plr = player.Player;
            if (!plr.WorldData.FreeMove && !instance.Gliding)
            {
                instance.IsFlying = false;
            } else if(plr.WorldData.FreeMove || instance.Gliding)
            {
                instance.IsFlying = true;
            }
        }
    }
}
