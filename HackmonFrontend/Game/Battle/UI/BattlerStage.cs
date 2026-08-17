namespace HackmonFrontend.Game.Battle.UI;

public partial class BattlerStage : Node2D
{
	private AnimatedSprite2D _hackmonSprite = null!;

	public override void _Ready()
	{
		_hackmonSprite = GetNode<AnimatedSprite2D>("BattlerSprite");
	}

	//Make this into a for loop?
	public void LoadHackmon(string hackmonName, bool doFlip = false)
	{
		_hackmonSprite.SpriteFrames.ClearAll();
		_hackmonSprite.SpriteFrames.AddAnimation("idle");
		_hackmonSprite.SpriteFrames.SetAnimationSpeed("idle", 30);
		_hackmonSprite.SpriteFrames.AddAnimation("physical");
		_hackmonSprite.SpriteFrames.SetAnimationSpeed("physical", 30);
		_hackmonSprite.SpriteFrames.AddAnimation("special");
		_hackmonSprite.SpriteFrames.SetAnimationSpeed("special", 30);
		_hackmonSprite.SpriteFrames.AddAnimation("hurt");
		_hackmonSprite.SpriteFrames.SetAnimationSpeed("hurt", 30);
		_hackmonSprite.SpriteFrames.AddAnimation("defeat");
		_hackmonSprite.SpriteFrames.SetAnimationSpeed("defeat", 30);
		var idleSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Idle.png");
		var walkSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Walk.png");
		var physicalSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Physical.png");
		var specialSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Special.png");
		var hurtSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Hurt.png");
		var defeatSprite = ResourceLoader.Load<Texture2D>($"Assets/BattleSprites/{hackmonName}/{hackmonName}_Defeat.png");
		var frameEdge = 96;
		//TODO make this way cleaner.
		//Also need to make defining vertical and horizontal frames dynamic.s
		for (var v = 0; v < 5; v++)
		{
			for (var h = 0; h < 6; h++)
			{
				var frame = new Rect2(h * frameEdge, v * frameEdge, frameEdge, frameEdge);
				var atlasTexture = new AtlasTexture();
				atlasTexture.Atlas = idleSprite;
				atlasTexture.Region = frame;
				_hackmonSprite.SpriteFrames.AddFrame("idle", atlasTexture);
			}
			for (var h = 0; h < 6; h++)
			{
				var frame = new Rect2(h * frameEdge, v * frameEdge, frameEdge, frameEdge);
				var atlasTexture = new AtlasTexture();
				atlasTexture.Atlas = physicalSprite;
				atlasTexture.Region = frame;
				_hackmonSprite.SpriteFrames.AddFrame("physical", atlasTexture);
			}
			for (var h = 0; h < 6; h++)
			{
				var frame = new Rect2(h * frameEdge, v * frameEdge, frameEdge, frameEdge);
				var atlasTexture = new AtlasTexture();
				atlasTexture.Atlas = specialSprite;
				atlasTexture.Region = frame;
				_hackmonSprite.SpriteFrames.AddFrame("special", atlasTexture);
			}
			for (var h = 0; h < 6; h++)
			{
				var frame = new Rect2(h * frameEdge, v * frameEdge, frameEdge, frameEdge);
				var atlasTexture = new AtlasTexture();
				atlasTexture.Atlas = hurtSprite;
				atlasTexture.Region = frame;
				_hackmonSprite.SpriteFrames.AddFrame("hurt", atlasTexture);
			}
			for (var h = 0; h < 6; h++)
			{
				var frame = new Rect2(h * frameEdge, v * frameEdge, frameEdge, frameEdge);
				var atlasTexture = new AtlasTexture();
				atlasTexture.Atlas = defeatSprite;
				atlasTexture.Region = frame;
				_hackmonSprite.SpriteFrames.AddFrame("defeat", atlasTexture);
			}
		}
		_hackmonSprite.FlipH = doFlip;
		_hackmonSprite.Play("idle");
	}

	public void LoadEchoAnimation(string animName)
	{
		_hackmonSprite.Play($"{animName}");
		if (animName == "defeat")
		{
			
		}
		else
			_hackmonSprite.AnimationLooped += () => _hackmonSprite.Play("idle");
	}

	public override void _Process(double delta)
	{
	}
}
