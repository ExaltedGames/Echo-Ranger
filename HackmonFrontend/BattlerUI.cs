using Godot;
using HackmonInternals.Models;
using HackmonInternals.StatusEffects;

[Tool]
public partial class BattlerUI : Panel
{
	[Export]
	public bool Flipped
	{
		get => _flipped;
		set
		{
			_flipped = value;
			FlipAilments();
		}
	}

	private HackmonInstance _currentHackmon;
	private RichTextLabel _nameLabel;
	private TextureProgressBar _healthBar;
	private int _currentChange = 0;
	private double _valueBeforeChange;
	private readonly double _tweenTime = 0.5;
	private double _tweenTimePassed = 0.5;

	private Container _ailmentContainer;
	private bool _flipped;

	public void SetCurrentMon(HackmonInstance mon)
	{
		_currentHackmon = mon;
		_nameLabel.Text = mon.Name;
		_healthBar.MaxValue = mon.MaxHp;
		_healthBar.Value = mon.Health;
		//_healthBar.ShowPercentage = true;

		var _primaryTypeImageNode = GetNode<Sprite2D>("Status/PrimarySocket/PrimaryType");
		var _primaryTypeImage = ResourceLoader.Load<Texture2D>($"res://Assets/UI/Icons/Type/{mon.PrimaryType}Icon.png");

		_primaryTypeImageNode.Texture = _primaryTypeImage;

		if (mon.SecondaryType != null)
		{
			GetNode<CanvasItem>("Status/SecondarySocket").Show();
			var _secondaryTypeImageNode = GetNode<Sprite2D>("Status/SecondarySocket/SecondaryType");
			var _secondaryTypeImage = ResourceLoader.Load<Texture2D>($"res://Assets/UI/Icons/Type/{mon.SecondaryType}Icon.png");

			_secondaryTypeImageNode.Texture = _secondaryTypeImage;
		}
		else
			GetNode<CanvasItem>("Status/SecondarySocket").Hide();

		_ailmentContainer = GetNode<Container>("Status/Ailments");
		foreach (var child in _ailmentContainer.GetChildren())
		{
			if (child is CanvasItem c)
				c.Hide();
		}
	}

	public void DoDamageAnim(int damage)
	{
		_valueBeforeChange = _healthBar.Value;
		_currentChange = damage;
		_tweenTimePassed = 0;
	}

	public void AddAilment(Status status)
	{
		var ailment = _ailmentContainer.FindChild(status.Name, false) as AilmentNode;
		ailment!.Stacks = status.Stacks;
		ailment.Show();
	}

	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("Status/Name");
		_healthBar = GetNode<TextureProgressBar>("Status/HealthBar");
		FlipAilments();
	}

	private void FlipAilments()
	{
		GD.Print("Test!!!!!!!");
		foreach (var node in FindChildren("*", nameof(AilmentNode)))
		{
			GD.Print(node.Name);
			var child = ((AilmentNode)node).FindChild("Container") as Control;
			if (child != null)
				child.Scale = new Vector2((Flipped ? -1 : 1) * Mathf.Abs(child.Scale.X), child.Scale.Y);
			GD.Print(child?.Scale);
		}
	}

	public override void _Process(double delta)
	{
		if (_tweenTimePassed < _tweenTime)
		{
			_tweenTimePassed += delta;
			if (_tweenTimePassed >= _tweenTime)
			{
				_healthBar.Value = _valueBeforeChange - _currentChange;
				_tweenTimePassed = _tweenTime;
				_currentChange = 0;
			}
			else
			{
				_healthBar.Value = _valueBeforeChange - (_currentChange * (_tweenTimePassed / _tweenTime));
			}
		}
	}
}
