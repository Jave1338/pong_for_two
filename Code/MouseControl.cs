using Sandbox;
using System.Linq;
using System.Net.NetworkInformation;

public sealed class MouseControl : Component
{
	[Property, Title("Lock X Axis")] public bool Lock { get; set; } = true;
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


	}

	protected override void OnUpdate()
	{
		GameObject.Network.SetOwnerTransfer( OwnerTransfer.Takeover );
		if ( IsProxy ) return;
		GameObject.Network.TakeOwnership();
		SetColor();

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
		}

		PaddleColor = GameSettings.Color;
		GetComponent<ModelRenderer>().Tint = PaddleColor switch
		{
			"blue" => new Color( 0, 0, 170 ),
			"green" => new Color( 0, 170, 0 ),
			"cyan" => new Color( 0, 170, 170 ),
			"red" => new Color( 170, 0, 0 ),
			"magenta" => new Color( 170, 0, 170 ),
			"brown" => new Color( 170, 85, 0 ),
			"gray" => Color.Gray,
			"lightblue" => new Color( 85, 85, 255 ),
			"lightgreen" => new Color( 85, 255, 85 ),
			"lightcyan" => new Color( 85, 255, 255 ),
			"lightred" => new Color( 255, 85, 85 ),
			"lightmagenta" => new Color( 255, 85, 255 ),
			"yellow" => new Color( 255, 255, 85, 1 ),
			"white" => Color.White,
			_ => new Color( 255, 255, 255 )
		};

		Network.Refresh();
		//GetComponent<ModelRenderer>().Tint = PaddleColor;
	}
}
