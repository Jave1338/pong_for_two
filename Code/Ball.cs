using Sandbox;
using Sandbox.Network;
using Sandbox.Services;
using System;
using System.Numerics;

public sealed class Ball : Component, Component.ICollisionListener
{
	GameObject gameManager;

	[Property, Sync] public int Speed { get; set; }
	[Property] public bool CollisionSound { get; set; } = true;
	[Property] float pushDelay = 1.5f;
	bool pushTimerEnabled = false;
	[Sync] public Vector3 Direction { get; set; }
	float paddleTouchCount = 0;
	float phantomTouchCount = 0;

	protected override void OnEnabled()
	{
		base.OnEnabled();
		gameManager = Scene.FindAllWithTag( "GameManager" ).FirstOrDefault();

		Start();
	}

	protected override void OnUpdate()
    {		
		if ( !GameSettings.Acceleration )
		{
			Speed = GameSettings.Speed;
		}

		LocalRotation = Rotation.Identity;

		if ( pushTimerEnabled )
		{
			pushDelay -= Time.Delta;
			if ( pushDelay <= 0 )
			{
				pushTimerEnabled = false;
				pushDelay = 1.5f;
				PushBall();
			}
		}

		//DebugOverlay.Line(0, Direction * 100);
		//DebugOverlay.Text( new Vector3(0, -10), $"{MathF.Atan2( Direction.y, Direction.x )}" );
	}

	public void Start()
	{
		GetComponent<Rigidbody>().Velocity = Vector3.Zero;
		Speed = GameSettings.Speed;

		var random = new Random();
		WorldPosition = new Vector3( 0, random.Next( -120, 120 ), 7 );
		
		paddleTouchCount = 0;
		phantomTouchCount = 0;
		pushTimerEnabled = true;
	}

	void PushBall()
	{
		if ( pushTimerEnabled ) return;
		for (int i = 0; i < 10;)
		{
			Direction = Vector3.Random.WithZ(0);
			if ( MathF.Atan2( Direction.Abs().y, Direction.Abs().x ) > 1.2 && MathF.Atan2( Direction.Abs().y, Direction.Abs().x ) < 1.9 )
			{
				i++;
			}
			else
			{
				break;
			}
		}

		GetComponent<Rigidbody>().Velocity = (Direction.Normal * Speed).WithZ( 0 );
	}

	public void OnCollisionStart(Collision collision)
	{
		collision.Self.Body.EnableCollisionSounds = CollisionSound;

		Direction = Vector3.Reflect( Direction, collision.Contact.Normal ).Normal.WithZ( 0 );

		collision.Self.Body.Velocity = Direction.Normal * Speed;
		collision.Self.Body.AngularVelocity = 0;

		if ( collision.Other.GameObject.Tags.Has( "paddle" ) && GameSettings.Acceleration )
		{
			paddleTouchCount++;
			phantomTouchCount = 0;
			Speed += Convert.ToInt32( paddleTouchCount / 50f * GameSettings.Speed );
			GetComponent<ModelRenderer>().Tint = collision.Other.GameObject.GetComponent<ModelRenderer>().Tint;
		}
		else if ( collision.Other.GameObject.Tags.Has( "top" ) || collision.Other.GameObject.Tags.Has( "bottom" ) )
		{
			phantomTouchCount++;
		}

		if ( phantomTouchCount >= 8 )
		{
			Start();
		}
	}

	public void OnCollisionStop(CollisionStop collision)
	{
		collision.Self.Body.Velocity = Direction.Normal * Speed;
		collision.Self.Body.AngularVelocity = 0;
	}

	void AddAngle( ref Vector3 vector )
	{
		float angle = MathF.Atan2( vector.y, vector.x );

		var random = new Random();
		float newAngle = angle + (random.Next( -5, 5 ) / 10);

		vector = new Vector3( MathF.Cos(newAngle), MathF.Sin(newAngle));
	}
}
