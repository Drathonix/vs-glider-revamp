using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace GliderRevamp.Network;

/// <summary>
/// Added to properly sync player creative flight states with other players.
/// By Drathon
/// </summary>
public class Packet_ServerChangeFlightControl:
{
    [ProtoMember(1)]
    public long EntityID;

    [ProtoMember(2)]
    public bool Enabled;
}
