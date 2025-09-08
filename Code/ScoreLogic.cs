namespace Sandbox;

public sealed class ScoreLogic : Component, Component.ITriggerListener
{
	[Property] GameManager gameManager;

	protected override void OnEnabled()
	{
		base.OnEnabled();
		gameManager = Scene.Get<GameManager>();
	}

	public void OnTriggerEnter( GameObject other )
	{
		if ( other.Tags.Has("ball"))
		{
			gameManager.ChangeScore( GameObject.Tags.ToString() );
			other.GetComponent<Ball>()?.Start();
		}
	}
}
