namespace Sandbox;

public sealed class ScoreLogic : Component, Component.ITriggerListener
{
	GameObject gameManager;

	protected override void OnEnabled()
	{
		base.OnEnabled();
		gameManager = Scene.FindAllWithTag( "GameManager" ).FirstOrDefault();
	}

	public void OnTriggerEnter( GameObject other )
	{
		if ( other.Tags.Has("ball"))
		{
			gameManager.GetComponent<GameManager>().ChangeScore( GameObject.Tags.ToString() );
			other.GetComponent<Ball>().Start();
		}
	}
}
