using System;

namespace Sandbox;

public sealed class GameManager : Component
{
	[Property] GameObject bot;
	[Property] GameObject gameOver;

	[Sync] public int HostScore { get; set; }
	[Sync] public int GuestScore { get; set; }
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
		if ( !isAlone && Connection.All.Count < 2 )
		{
			ToggleBot(true);
		}
		else if ( isAlone && Connection.All.Count > 1 )
		{
			ToggleBot(false);
		}

		if ( Input.Down( "jump" ) )
		{
			Scene.TimeScale = 0.5f;
		}
		else
		{
			Scene.TimeScale = 1f;
		}
	}

	void ToggleBot(bool enable)
	{
		if ( enable )
		{
			isAlone = true;
			bot.Enabled = true;
		}
		else
		{
			isAlone = false;
			bot.Enabled = false;
		}
		Scene.FindAllWithTag( "ball" ).FirstOrDefault().GetComponent<Ball>().Start();
		HostScore = 0;
		GuestScore = 0;
	}

	public void ChangeScore(string side)
	{
		if ( side == "right" )
		{
			GuestScore++;
		}
		else if ( side == "left" )
		{
			HostScore++;
		}

		if ( HostScore + GuestScore >= GameSettings.MaxRounds )
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
