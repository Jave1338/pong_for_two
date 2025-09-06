using Sandbox;
using System.Windows;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System;
using System.Globalization;

public sealed class MouseControl : Component
{
	[Property] float borderY = 133.5f;
	[Property] bool isBot = false;
	GameObject ball;
	[Sync(SyncFlags.Query)] string PaddleColor { get; set; } = "white";

    protected override void OnEnabled()
    {
        base.OnEnabled();
		ball = Scene.FindAllWithTag( "ball" ).FirstOrDefault();

		if (Networking.IsHost)
		{
			var spawnPoints = Scene.FindAllWithTags( ["spawnpoint", "right"] ).FirstOrDefault();
			WorldPosition = spawnPoints.WorldPosition;
		}
		else if (!Networking.IsHost)
		{
			var spawnPoints = Scene.FindAllWithTags( ["spawnpoint", "left"] ).FirstOrDefault();
			WorldPosition = spawnPoints.WorldPosition;
		}

		SetColor();
	}

	protected override void OnUpdate()
	{
		//GameObject.Network.SetOwnerTransfer( OwnerTransfer.Takeover );
		if ( IsProxy ) return;
		//GameObject.Network.TakeOwnership();

		if (!isBot)
		{
			WorldPosition += new Vector3( 0, -Input.MouseDelta.y * GameSettings.Sensitivity, 0 );
		}
		else
		{
			WorldPosition = new Vector3( -275, ball.WorldPosition.y, WorldPosition.z );
		}

		if ( WorldPosition.y >= borderY )
		{
			WorldPosition = new Vector3( WorldPosition.x, borderY, WorldPosition.z );
		}
		else if ( WorldPosition.y <= -borderY )
		{
			WorldPosition = new Vector3( WorldPosition.x, -borderY, WorldPosition.z );
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

		PaddleColor = GameSettings.Color;
		GetComponent<ModelRenderer>().Tint = PaddleColor switch
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
