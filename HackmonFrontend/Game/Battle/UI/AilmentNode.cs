namespace HackmonFrontend.Game.Battle.UI;

[Tool]
[GlobalClass]
public partial class AilmentNode : Control
{
	public int Stacks
	{
		get => _stacks;
		set
		{
			_stacks = value;
			SetStacks(value);
		}
	}

	public TextureRect? SpriteRect;

	private Texture2D? _sprite;

	private int _stacks;

	private Label? _stacksLabel;

	[Export]
	private Texture2D? Sprite
	{
		get => _sprite;
		set
		{
			_sprite = value;
			SetSprite(value);
		}
	}

	public override void _Ready()
	{
		SpriteRect ??= GetNode<TextureRect>("Container/Sprite");
		_stacksLabel ??= GetNode<Label>("Container/Sprite/Stacks");
		SetSprite(Sprite);
		SetStacks(Stacks);
	}

	private void SetStacks(int amount)
	{
		if (_stacksLabel != null)
			_stacksLabel.Text = amount.ToString();
	}

	private void SetSprite(Texture2D? texture)
	{
		if (SpriteRect != null)
			SpriteRect.Texture = texture;
	}
}