using Godot;

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

    private int _stacks;
    
    public TextureRect? SpriteRect;

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

    private Texture2D? _sprite;

    private Label? _stacksLabel;

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
