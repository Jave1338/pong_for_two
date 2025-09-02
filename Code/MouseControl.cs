using Sandbox;
using System.Linq;

public sealed class MouseControl : Component
{
	[Property, Title("Lock X Axis")] public bool Lock { get; set; } = true;
	[Property] float borderY = 133.5f;
	[Property] bool isBot = false;
	GameObject ball;

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
		if ( Network.IsProxy ) return;

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
}
