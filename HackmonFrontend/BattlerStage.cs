using Godot;

public partial class BattlerStage : Node2D
{
	private AnimatedSprite2D hackmonSprite;
	
	public override void _Ready()
	{
		hackmonSprite = GetNode<AnimatedSprite2D>("BattlerSprite");
	}

	public void LoadHackmon(string hackmonName, bool doFlip = false)
	{
		hackmonSprite.SpriteFrames.ClearAll();
		var newSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}.png");
		hackmonSprite.FlipH = doFlip;	
		hackmonSprite.SpriteFrames.AddFrame("default", newSprite);	
	}

	public override void _Process(double delta)
	{
	}
}
