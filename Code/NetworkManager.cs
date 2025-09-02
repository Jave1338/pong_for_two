using Sandbox;
using Sandbox.Network;

public sealed class NetworkManager : Component
{
    private static NetworkManager _instance;
    public static NetworkManager Instance => _instance ??= new NetworkManager();
    public int MaxPlayers { get; set; } = 4;
    public LobbyPrivacy LobbyPrivacy { get; set; } = LobbyPrivacy.Public;
    public string GameMode { get; set; } = "Singleplayer";

    //public enum Teams
    //{
    //    Player,
    //    Spectator
    //}
    //public Teams Team { get; set; }

    protected override void OnEnabled()
    {
        _instance = this;

		//foreach (var player in Connection.All)
		//{
		//    
		//}

		Networking.CreateLobby(new LobbyConfig()
        {
            MaxPlayers = GameSettings.MaxPlayers,
            Privacy = GameSettings.LobbyPrivacy,
        });
    }

    protected override void OnFixedUpdate()
    {
        GameMode = Connection.All.Count > 1 ? "Multiplayer" : "Singleplayer";
    }
}
