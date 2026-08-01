using GliderRevamp;
using GliderRevamp.Network;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vintagestory.API.Datastructures;

namespace GliderRevamp.Patches;

/// <summary>
/// Synchronize player flying state on touching ground.
/// By Drathon
/// </summary>
[HarmonyPatch(typeof(EntityPlayer),nameof(EntityPlayer.OnFallToGround))]
public class PatchEntityPlayer_TouchGround
{
    public static void Prefix(EntityPlayer __instance, double motionY)
    {
        if (__instance.Controls.Gliding)
        {
            __instance.Controls.IsFlying = false;
            GliderRevampModSystem.ServerChannel.BroadcastPacket(new Packet_ServerChangeFlightControl
            {
                Enabled = false,
                EntityID = __instance.EntityId
            });
        }
    }
}
