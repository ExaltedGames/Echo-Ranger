using Godot;
using System;
using HackmonInternals.Models;

public partial class BattlerUI : Panel
{
	private HackmonInstance _currentHackmon;
	private RichTextLabel _nameLabel;

	public void SetCurrentMon(HackmonInstance mon)
	{
		_currentHackmon = mon;
		_nameLabel.Text = mon.Name;
	}
	
	public override void _Ready()
	{
		_nameLabel = GetNode<RichTextLabel>("Status/Name");
	}

	public override void _Process(double delta)
	{
	}
}
