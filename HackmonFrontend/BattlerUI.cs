using Godot;
using System;
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
	private double _tweenTimePassed = 0.5;

	public void SetCurrentMon(HackmonInstance mon)
	{
		_currentHackmon = mon;
		_nameLabel.Text = mon.Name;
		_healthBar.MaxValue = mon.MaxHp;
		_healthBar.Value = mon.Health;
		_staminaBar.MaxValue = mon.MaxStamina;
		_staminaBar.Value = mon.Stamina;
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
		else GetNode<CanvasItem>("Status/SecondarySocket").Hide();


	}

	public void DoDamageAnim(int damage)
	{
		_healthValueBeforeChange = _healthBar.Value;
		_healthCurrentChange = damage;
		_tweenTimePassed = 0;
	}
	public void DoHpRegenAnim(int health)
	{
		_healthValueBeforeChange = _healthBar.Value;
		_healthCurrentChange = -health;
		_tweenTimePassed = 0;
	}
	public void DoStaminaAnim(int staminaCost)
	{
		_staminaValueBeforeChange = _staminaBar.Value;
		_staminaCurrentChange = staminaCost;
		_tweenTimePassed = 0;
	}
	public void DoStamRegenAnim(int stamina)
	{
		_staminaValueBeforeChange = _staminaBar.Value;
		_staminaCurrentChange = -stamina;
		_tweenTimePassed = 0;
	}

	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("Status/Name");
		_healthBar = GetNode<TextureProgressBar>("Status/HealthBar");
		_staminaBar = GetNode<TextureProgressBar>("Status/StaminaBar");
	}

	public override void _Process(double delta)
	{
		if (_tweenTimePassed < _tweenTime)
		{
			_tweenTimePassed += delta;
			if (_tweenTimePassed >= _tweenTime)
			{
				_healthBar.Value = _healthValueBeforeChange - _healthCurrentChange;
				_staminaBar.Value = _staminaValueBeforeChange - _staminaCurrentChange;
				_tweenTimePassed = _tweenTime;
				_healthCurrentChange = 0;
				_staminaCurrentChange = 0;
			}
			else
			{
				_healthBar.Value = _healthValueBeforeChange - (_healthCurrentChange * (_tweenTimePassed / _tweenTime));
				_staminaBar.Value = _staminaValueBeforeChange - (_staminaCurrentChange * (_tweenTimePassed / _tweenTime));
			}
		}
	}
}
