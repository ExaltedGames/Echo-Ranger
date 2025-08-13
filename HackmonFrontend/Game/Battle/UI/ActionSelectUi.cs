using Godot;
using System;
using HackmonInternals.Models;

namespace HackmonFrontend;

public partial class ActionSelectUi : Control
{
	public HackmonMove?[] SelectableActions = new HackmonMove[4];
	public Button[] Buttons = new Button[4];
	public int CurrentSelection = -1;

	public delegate void ActionSelectHandler(HackmonMove action);
	public event ActionSelectHandler? OnActionSelected; 
	
	private TextEdit _infoBox;
	private int _numActions = 4;
	
	private void OnMovePressed(int selection)
	{
		GD.Print($"test: {selection}");

		if (CurrentSelection == selection)
		{
			GD.Print("Selection confirmed.");
			OnActionSelected?.Invoke(SelectableActions[selection]!);
			CurrentSelection = -1;
		}
		else
		{
			CurrentSelection = selection;
			var currentMove = SelectableActions[selection];
			_infoBox.Text = $"{currentMove!.Description}\nType: {currentMove.AttackType}\nDamage: {currentMove.Damage}      Cost: {currentMove.StaminaCost}";
		}
	}

	public void SetEnabled(bool enabled)
	{
		_infoBox.Visible = enabled;
		for (var i = 0; i < _numActions; i++)
		{
			Buttons[i].Visible = enabled;
			Buttons[i].Disabled = !enabled;
		}
	}
	
	public void SetActions(HackmonMove?[] actions)
	{
		if (actions.Length > 4) throw new Exception("UI currently not built to support more than 4 moves.");
		var totalActions = 0;
		
		for (var i = 0; i < actions.Length; i++)
		{
			if (actions[i] == null) break;
			SelectableActions[i] = actions[i];
			Buttons[i].Text = actions[i]!.Name;
			Buttons[i].Disabled = false;
			Buttons[i].Visible = true;
			totalActions++;
		}

		for (var i = totalActions; i < 4; i++)
		{
			SelectableActions[i] = null;
			Buttons[i].Disabled = true;
			Buttons[i].Visible = false;
		}

		_numActions = totalActions;
	}

	public void ResetHandler()
	{
		OnActionSelected = null;
	}

	public override void _Ready()
	{
		_infoBox = GetNode<TextEdit>("Infobox");
		Buttons[0] = GetNode<Button>("MoveList/TopMoves/Move1");
		Buttons[0].Pressed += () => OnMovePressed(0);
		Buttons[1] = GetNode<Button>("MoveList/TopMoves/Move2");
		Buttons[1].Pressed += () => OnMovePressed(1);
		Buttons[2] = GetNode<Button>("MoveList/BottomMoves/Move3");
		Buttons[2].Pressed += () => OnMovePressed(2);
		Buttons[3] = GetNode<Button>("MoveList/BottomMoves/Move4");
		Buttons[3].Pressed += () => OnMovePressed(3);
	}

	public override void _Process(double delta)
	{
	}
}
