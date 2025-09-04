using Sandbox;
using Sandbox.Network;
using System.ComponentModel.Design;

public static class GameSettings
{
	public static LobbyPrivacy LobbyPrivacy { get; set; } = LobbyPrivacy.Public;

	private static int _maxPlayers = 2;
	public static int MaxPlayers 
	{ 
		get => _maxPlayers;
		set { _maxPlayers = value; }
	}

	private static int _maxRounds = 20;
	public static int MaxRounds
	{
		get => _maxRounds;
		set { _maxRounds = value; }
	}

	private static int _speed = 400;
	public static int Speed 
	{ 
		get => _speed;
		set { _speed = value; } 
	}
	
	private static bool _acceleration = true;
	public static bool Acceleration 
	{ 
		get => _acceleration;
		set { _acceleration = value; } 
	}

	private static float _sensitivity = 1.0f;
	public static float Sensitivity
	{
		get => _sensitivity;
		set { _sensitivity = value; }
	}

	private static string _color = "white";
	public static string Color
	{
		get => _color;
		set { _color = value; }
	}
}
