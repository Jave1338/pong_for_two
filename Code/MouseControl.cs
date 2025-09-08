using Sandbox;
using System.Windows;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System;
using System.Globalization;

public sealed class MouseControl : Component
{
	[Property] bool lockXAxis;
	[Property] bool lockYAxis;
	[Property] float border = 130;
	[Property] bool isBot = false;
	GameObject ball;
	GameManager gameManager;

    protected override void OnEnabled()
    {
        base.OnEnabled();
		ball = Scene.FindAllWithTag( "ball" ).FirstOrDefault();
		gameManager = Scene.Get<GameManager>();

		SetColor();
	}

	protected override void OnStart()
	{
		base.OnStart();

		if ( gameManager.sceneName == "game" )
		{
			lockXAxis = true;
			lockYAxis = false;
			
			if (Networking.IsHost)
			{
				var spawnPoint = Scene.FindAllWithTags( ["spawnpoint", "right"] ).FirstOrDefault();
				WorldPosition = spawnPoint.WorldPosition;
				WorldRotation = spawnPoint.WorldRotation;
			}
			else if (!Networking.IsHost)
			{
				var spawnPoint = Scene.FindAllWithTags( ["spawnpoint", "left"] ).FirstOrDefault();
				WorldPosition = spawnPoint.WorldPosition;
				WorldRotation = spawnPoint.WorldRotation;
			}
		}
		else if ( Scene.Name == "game4" )
		{
			if ( Networking.IsHost )
			{
				lockXAxis = false;
				lockYAxis = true;
				var spawnPoint = Scene.FindAllWithTags( ["spawnpoint", "bottom"] ).FirstOrDefault();
				WorldPosition = spawnPoint.WorldPosition;
			}
		}
	}

	protected override void OnUpdate()
	{
		if ( IsProxy ) return;



		if (!isBot)
		{
			WorldPosition += new Vector3( 
				lockXAxis ? 0 : Input.MouseDelta.x * GameSettings.Sensitivity, 
				lockYAxis ? 0 : -Input.MouseDelta.y * GameSettings.Sensitivity, 
				0 );
		}
		else
		{
			WorldPosition = new Vector3( -275, ball.WorldPosition.y, WorldPosition.z );
		}

		if ( lockXAxis && WorldPosition.y >= border )
		{
			WorldPosition = new Vector3( WorldPosition.x, border, WorldPosition.z );
		}
		if ( lockXAxis && WorldPosition.y <= -border )
		{
			WorldPosition = new Vector3( WorldPosition.x, -border, WorldPosition.z );
		}
		if ( lockYAxis && WorldPosition.x >= border )
		{
			WorldPosition = new Vector3( border, WorldPosition.y, WorldPosition.z );
		}
		if ( lockYAxis && WorldPosition.x <= -border )
		{
			WorldPosition = new Vector3( -border, WorldPosition.y, WorldPosition.z );
		}
	}

	void SetColor()
	{
		if ( IsProxy ) return;

		if ( isBot )
		{
			GetComponent<ModelRenderer>().Tint = Color.White;
			return;
		}

		GetComponent<ModelRenderer>().Tint = GameSettings.Color switch
		{
			"blue" => new Color( 0, 0, 170 ),
			"green" => new Color( 0, 170, 0 ),
			"darkcyan" => new Color( 0, 170, 170 ),
			"red" => new Color( 20, 0, 0 ),
			"purple" => new Color( 8388736 ).WithAlpha(1),
			"saddlebrown" => new Color( 3607296 ).WithAlpha(1).AdjustHue(-220),
			"gray" => Color.Gray,
			"lightblue" => Color.FromRgba( UInt32.Parse( "F47EFff", NumberStyles.HexNumber ) ),
			"lightgreen" => Color.FromRgba(UInt32.Parse( "4bd14eff", NumberStyles.HexNumber ) ),
			"cyan" => Color.FromRgba(UInt32.Parse( "00FFFFff", NumberStyles.HexNumber ) ),
			"lightcoral" => Color.FromRgba( UInt32.Parse( "EA3434ff", NumberStyles.HexNumber ) ),
			"violet" => Color.FromRgba( UInt32.Parse( "EB3DEBff", NumberStyles.HexNumber ) ),
			"yellow" => Color.FromRgba( UInt32.Parse( "FFFF00ff", NumberStyles.HexNumber ) ),
			"white" => Color.White,
			_ => new Color( 255, 255, 255 )
		};

		Network.Refresh();
	}
}
