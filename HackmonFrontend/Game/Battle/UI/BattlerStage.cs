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
		_hackmonSprite.SpriteFrames.AddAnimation("idle");
		var newSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Idle.png");
		var frameEdge = 96;
		for (var v = 0; v < 5; v++)
		{
			for (var h = 0; h < 6; h++)
			{
				var frame = new Rect2(h * frameEdge, v * frameEdge, frameEdge, frameEdge);
				var atlasTexture = new AtlasTexture();
				atlasTexture.Atlas = newSprite;
				atlasTexture.Region = frame;
				_hackmonSprite.SpriteFrames.AddFrame("idle", atlasTexture);
			}
		}
		_hackmonSprite.FlipH = doFlip;
		_hackmonSprite.SpriteFrames.SetAnimationSpeed("idle", 30);
		_hackmonSprite.Play("idle");
	}

	public override void _Process(double delta)
	{
	}
}
