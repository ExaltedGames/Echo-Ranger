namespace HackmonFrontend.Game.Battle.UI;

public partial class BattlerStage : Node2D
{
	private AnimatedSprite2D _hackmonSprite = null!;

	public override void _Ready()
	{
		_hackmonSprite = GetNode<AnimatedSprite2D>("BattlerSprite");
	}

	public void LoadHackmon(string hackmonName, bool doFlip = false)
	{
		_hackmonSprite.SpriteFrames.ClearAll();
		var newSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}.png");
		_hackmonSprite.FlipH = doFlip;
		_hackmonSprite.SpriteFrames.AddFrame("default", newSprite);
	}

	public override void _Process(double delta)
	{
	}
}