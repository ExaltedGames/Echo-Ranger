using Godot;
using System;
using System.Threading.Tasks;
using HackmonInternals.Models;

public partial class BattlerUI : Panel
{
	private HackmonInstance _currentHackmon;
	private RichTextLabel _nameLabel;
	private TextureProgressBar _healthBar;
	private TextureProgressBar _staminaBar;
	private int _healthCurrentChange = 0;
	private int _staminaCurrentChange = 0;
	private double _healthValueBeforeChange;
	private double _staminaValueBeforeChange;
	private readonly double _tweenTime = 0.5;
	private double _healthTweenTimePassed = 0.5;
	private double _staminaTweenTimePassed = 0.5;
	private TaskCompletionSource _healthTween = new();
	private TaskCompletionSource _staminaTween = new();

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
			var secondaryTypeImage = ResourceLoader.Load<Texture2D>($"res://Assets/UI/Icons/Type/{mon.SecondaryType}Icon.png");

			secondaryTypeImageNode.Texture = secondaryTypeImage;
		}
		else GetNode<CanvasItem>("Status/SecondarySocket").Hide();
	}

	public Task DoDamageAnim(int damage)
	{
		if (_healthCurrentChange != 0) throw new Exception("There is already a health tween in progress.");
		_healthValueBeforeChange = _healthBar.Value;
		_healthCurrentChange = damage;
		_healthTweenTimePassed = 0;
		_healthTween = new();
		return _healthTween.Task;
	}
	
	public Task DoHpRegenAnim(int health)
	{
		if (_healthCurrentChange != 0) throw new Exception("There is already a health tween in progress.");
		_healthValueBeforeChange = _healthBar.Value;
		_healthCurrentChange = -health;
		_healthTweenTimePassed = 0;
		_healthTween = new();
		return _healthTween.Task;
	}
	
	public Task DoStaminaAnim(int staminaCost)
	{
		if (_staminaCurrentChange != 0) throw new Exception("There is already a stamina tween in progress.");		
		_staminaValueBeforeChange = _staminaBar.Value;
		_staminaCurrentChange = staminaCost;
		_staminaTweenTimePassed = 0;
		_staminaTween = new();
		return _staminaTween.Task;
	}
	
	public Task DoStamRegenAnim(int stamina)
	{
		if (_staminaCurrentChange != 0) throw new Exception("There is already a stamina tween in progress.");
		_staminaValueBeforeChange = _staminaBar.Value;
		_staminaCurrentChange = -stamina;
		_staminaTweenTimePassed = 0;
		_staminaTween = new();
		return _staminaTween.Task;
	}

	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("Status/Name");
		_healthBar = GetNode<TextureProgressBar>("Status/HealthBar");
		_staminaBar = GetNode<TextureProgressBar>("Status/StaminaBar");
	}

	public override void _Process(double delta)
	{
		if (_healthTweenTimePassed < _tweenTime)
		{
			_healthTweenTimePassed += delta;
			if (_healthValueBeforeChange >= _tweenTime)
			{
				_healthBar.Value = _healthValueBeforeChange - _healthCurrentChange;
				_healthTweenTimePassed = _tweenTime;
				_healthCurrentChange = 0;
				_healthTween.SetResult();
			}
			else
			{
				_healthBar.Value = _healthValueBeforeChange - (_healthCurrentChange * (_healthTweenTimePassed / _tweenTime));
			}
		}

		if (_staminaTweenTimePassed < _tweenTime)
		{
			_staminaTweenTimePassed += delta;
			if (_staminaValueBeforeChange >= _tweenTime)
			{
				_staminaBar.Value = _staminaValueBeforeChange - _staminaCurrentChange;
				_staminaTweenTimePassed = _tweenTime;
				_staminaCurrentChange = 0;
				_staminaTween.SetResult();
			}
			else
			{
				_staminaBar.Value = _staminaValueBeforeChange - (_staminaCurrentChange * (_staminaTweenTimePassed / _tweenTime));
			}
		}
	}
}
