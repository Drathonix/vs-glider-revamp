using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GliderRevamp.Network;

public class ClientNetworkHandler
{
    private static ICoreClientAPI api;

    public static void Init(ICoreClientAPI capi)
    {
        api = capi;
        var channel = capi.Network.RegisterChannel("gliderrevamp");
        //channel.RegisterMessageType(typeof(Packet_ServerChangeFlightControl));
       // channel.SetMessageHandler<Packet_ServerChangeFlightControl>(HandleServerChangeFlightControl);
    }
    /*public static void HandleServerChangeFlightControl(Packet_ServerChangeFlightControl packet)
    {
        if(api.World.GetEntityById(packet.EntityID) is EntityAgent agent)
        {
            agent.Controls.IsFlying = packet.Enabled;
        }
    }*/
}
