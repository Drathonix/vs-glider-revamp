using ConfigLib;
using GliderRevamp.HudElements;
using GliderRevamp.Network;
using System.Reflection;
using Vintagestory.API.Server;

namespace GliderRevamp;

public sealed class GliderRevampModSystem : ModSystem
{
    private const string HarmonyId = "gliderrevamp";
    private const string ConfigLibId = "configlib";

    public static IServerNetworkChannel ServerChannel;

    private Harmony _harmony;
    private GliderSpeedHudElement _gliderSpeedHud;

    public override void Start(ICoreAPI api)
    {
        if (api.ModLoader.IsModEnabled(ConfigLibId))
        {
            SubscribeToConfigChange(api);
        }
        _harmony = new Harmony(HarmonyId);
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
        
        GliderEvents.Init();
    }

    public override void StartClientSide(ICoreClientAPI capi)
    {
        base.StartClientSide(capi);
        
        _gliderSpeedHud = new GliderSpeedHudElement(capi);
        capi.Gui.RegisterDialog(_gliderSpeedHud);
        ClientNetworkHandler.Init(capi);
    }

    public override void Dispose()
    {
        base.Dispose();

        _harmony?.UnpatchAll(HarmonyId);
        _gliderSpeedHud?.Dispose();
    }

    private static void SubscribeToConfigChange(ICoreAPI api)
    {
        var system = api.ModLoader.GetModSystem<ConfigLibModSystem>();

        system.SettingChanged += (domain, _, setting) =>
        {
            if (domain != HarmonyId)
            {
                return;
            }

            setting.AssignSettingValue(ModConfig.Instance);
        };
        
        system.ConfigsLoaded += () =>
        {
            system.GetConfig(HarmonyId)?.AssignSettingsValues(ModConfig.Instance);
        };
    }

    public override void StartServerSide(ICoreServerAPI sapi)
    {
        ServerChannel = sapi.Network.RegisterChannel("gliderrevamp");
        ServerChannel.RegisterMessageType(typeof(Packet_ServerChangeFlightControl));
        sapi.Event.PlayerSwitchGameMode += PlayerSwitchGamemode;
    }

    public void PlayerSwitchGamemode(IServerPlayer player)
    {
        ServerChannel.BroadcastPacket(new Packet_ServerChangeFlightControl
        {
            Enabled = player.WorldData.EntityControls.IsFlying,
            EntityID = player.Entity.EntityId
        });
    }
}
