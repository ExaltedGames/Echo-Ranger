using HackmonInternals.StatusEffects;

namespace HackmonFrontend.Game.Battle.UI;

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

	private Container _ailmentContainer;

	private HackmonInstance _currentHackmon;
	private bool _flipped;
	private TextureProgressBar _healthBar;
	private int _healthCurrentChange;
	private TaskCompletionSource _healthTween = new();
	private double _healthTweenTimePassed = 0.5;
	private double _healthValueBeforeChange;
	private RichTextLabel _nameLabel;
	private TextureProgressBar _staminaBar;
	private int _staminaCurrentChange;
	private TaskCompletionSource _staminaTween = new();
	private double _staminaTweenTimePassed = 0.5;
	private double _staminaValueBeforeChange;

	[Export]
	private double _tweenTime = 0.5;

	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("Status/Name");
		_healthBar = GetNode<TextureProgressBar>("Status/HealthBar");
		_staminaBar = GetNode<TextureProgressBar>("Status/StaminaBar");
		FlipAilments();
	}

	public void SetCurrentMon(HackmonInstance mon)
	{
		_currentHackmon = mon;
		_nameLabel.Text = mon.Name;
		_healthBar.MaxValue = mon.MaxHp;
		_healthBar.Value = mon.Health;
		_staminaBar.MaxValue = mon.MaxStamina;
		_staminaBar.Value = mon.Stamina;

		//_healthBar.ShowPercentage = true;

		var primaryTypeImageNode = GetNode<Sprite2D>("Status/PrimarySocket/PrimaryType");
		var primaryTypeImage = ResourceLoader.Load<Texture2D>($"res://Assets/UI/Icons/Type/{mon.PrimaryType}Icon.png");

		primaryTypeImageNode.Texture = primaryTypeImage;

		if (mon.SecondaryType != null)
		{
			GetNode<CanvasItem>("Status/SecondarySocket").Show();
			var secondaryTypeImageNode = GetNode<Sprite2D>("Status/SecondarySocket/SecondaryType");
			var secondaryTypeImage =
				ResourceLoader.Load<Texture2D>($"res://Assets/UI/Icons/Type/{mon.SecondaryType}Icon.png");

			secondaryTypeImageNode.Texture = secondaryTypeImage;
		}
		else
			GetNode<CanvasItem>("Status/SecondarySocket").Hide();

		_ailmentContainer = GetNode<Container>("Status/Ailments");
		foreach (var child in _ailmentContainer.GetChildren())
			if (child is CanvasItem c)
				c.Hide();
	}

	public Task DoDamageAnim(int damage)
	{
		if (_healthCurrentChange != 0)
			throw new Exception("There is already a health tween in progress.");

		_healthValueBeforeChange = _healthBar.Value;
		_healthCurrentChange = damage;
		_healthTweenTimePassed = 0;
		_healthTween = new();
		return _healthTween.Task;
	}

	public Task DoHpRegenAnim(int health)
	{
		if (_healthCurrentChange != 0)
			throw new Exception("There is already a health tween in progress.");

		_healthValueBeforeChange = _healthBar.Value;
		_healthCurrentChange = -health;
		_healthTweenTimePassed = 0;
		_healthTween = new();
		return _healthTween.Task;
	}

	public Task DoStaminaAnim(int staminaCost)
	{
		if (_staminaCurrentChange != 0)
			throw new Exception("There is already a stamina tween in progress.");

		_staminaValueBeforeChange = _staminaBar.Value;
		_staminaCurrentChange = staminaCost;
		_staminaTweenTimePassed = 0;
		_staminaTween = new();
		return _staminaTween.Task;
	}

	public Task DoStamRegenAnim(int stamina)
	{
		if (_staminaCurrentChange != 0)
			throw new Exception("There is already a stamina tween in progress.");

		_staminaValueBeforeChange = _staminaBar.Value;
		_staminaCurrentChange = -stamina;
		_staminaTweenTimePassed = 0;
		_staminaTween = new();
		return _staminaTween.Task;
	}

	// Doesn't actually have any animation at the moment
	public Task AddAilment(Status status)
	{
		var ailment = _ailmentContainer.FindChild(status.Name, false) as AilmentNode;
		ailment!.Stacks = status.Stacks;
		ailment.Show();

		return Task.CompletedTask;
	}

	public override void _Process(double delta)
	{
		if (_healthTweenTimePassed < _tweenTime)
		{
			_healthTweenTimePassed += delta;
			if (_healthTweenTimePassed >= _tweenTime)
			{
				_healthBar.Value = _healthValueBeforeChange - _healthCurrentChange;
				_healthTweenTimePassed = _tweenTime;
				_healthCurrentChange = 0;
				_healthTween.SetResult();
			}
			else
			{
				_healthBar.Value = _healthValueBeforeChange -
				                   _healthCurrentChange * (_healthTweenTimePassed / _tweenTime);
			}
		}

		if (_staminaTweenTimePassed < _tweenTime)
		{
			_staminaTweenTimePassed += delta;
			if (_staminaTweenTimePassed >= _tweenTime)
			{
				_staminaBar.Value = _staminaValueBeforeChange - _staminaCurrentChange;
				_staminaTweenTimePassed = _tweenTime;
				_staminaCurrentChange = 0;
				_staminaTween.SetResult();
			}
			else
			{
				_staminaBar.Value = _staminaValueBeforeChange -
				                    _staminaCurrentChange * (_staminaTweenTimePassed / _tweenTime);
			}
		}
	}

	private void FlipAilments()
	{
		foreach (var node in FindChildren("*", nameof(AilmentNode)))
			if (((AilmentNode)node).FindChild("Container") is Control child)
				child.Scale = new Vector2((Flipped ? -1 : 1) * Mathf.Abs(child.Scale.X), child.Scale.Y);
	}
}