using System;

namespace Sandbox;

public sealed class GameManager : Component
{
	[Property] GameObject bot;
	[Property] GameObject gameOver;

	[Sync] public int hostScore { get; set; }
	[Sync] public int guestScore { get; set; }
	bool isAlone;

	protected override void OnEnabled()
	{
		base.OnEnabled();

		if ( Connection.All.Count < 2 )
		{
			bot.Enabled = true;
			isAlone = true;
		}
	}

	protected override void OnUpdate()
	{
		if ( Input.Down( "jump" ) )
		{
			Scene.TimeScale = 0.5f;
		}
		else
		{
			Scene.TimeScale = 1f;
		}

		if (isAlone && Connection.All.Count > 1 )
		{
			bot.Destroy();
			isAlone = false;
			Scene.FindAllWithTag( "ball" ).FirstOrDefault().GetComponent<Ball>().Start();
			hostScore = 0;
			guestScore = 0;
		}
	}

	public void ChangeScore(string side)
	{
		if ( side == "right" )
		{
			guestScore++;
		}
		else if ( side == "left" )
		{
			hostScore++;
		}

		if ( hostScore + guestScore >= GameSettings.MaxRounds )
		{
			GameOver();
		}
	}

	void GameOver()
	{
		gameOver.Enabled = true;
		Scene.FindAllWithTag( "ball" ).FirstOrDefault().GetComponent<Ball>().Enabled = false;
	}
}
