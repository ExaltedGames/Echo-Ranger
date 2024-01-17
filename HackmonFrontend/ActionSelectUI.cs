using Godot;
using System;
using System.Collections.Generic;
using HackmonInternals.Models;

namespace HackmonFrontend;

public partial class ActionSelectUI : Control
{
	private TextEdit Infobox;
	private int numActions = 4;

	public HackmonMove[] SelectableActions = new HackmonMove[4];
	public Button[] Buttons = new Button[4];
	public int CurrentSelection = -1;

	public delegate void ActionSelectHandler(HackmonMove action);
	public event ActionSelectHandler ActionSelected = null!; 
	
	private void _on_move_pressed(int selection)
	{
		GD.Print($"test: {selection}");

		if (CurrentSelection == selection)
		{
			GD.Print("Selection confirmed.");
			ActionSelected?.Invoke(SelectableActions[selection]);
		}
		else
		{
			CurrentSelection = selection;
			var currentMove = SelectableActions[selection];
			Infobox.Text = $"{currentMove.Description}\nType: {currentMove.AttackType}\nDamage: {currentMove.Damage}";
		}
	}

	public void SetEnabled(bool enabled)
	{
		Infobox.Visible = false;
		for (int i = 0; i < numActions; i++)
		{
			Buttons[i].Visible = enabled;
			Buttons[i].Disabled = !enabled;
		}
	}
	
	public void SetActions(HackmonMove[] actions)
	{
		if (actions.Length > 4) throw new Exception("UI currently not built to support more than 4 moves.");
		var totalActions = 0;
		
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] == null) break;
			SelectableActions[i] = actions[i];
			Buttons[i].Text = actions[i].Name;
			Buttons[i].Disabled = false;
			Buttons[i].Visible = true;
			totalActions++;
		}

		for (int i = totalActions; i < 4; i++)
		{
			SelectableActions[i] = null;
			Buttons[i].Disabled = true;
			Buttons[i].Visible = false;
		}

		numActions = totalActions;
	}

	public override void _Ready()
	{
		Infobox = GetNode<TextEdit>("Infobox");
		Buttons[0] = GetNode<Button>("MoveList/TopMoves/Move1");
		Buttons[1] = GetNode<Button>("MoveList/TopMoves/Move2");
		Buttons[2] = GetNode<Button>("MoveList/BottomMoves/Move3");
		Buttons[3] = GetNode<Button>("MoveList/BottomMoves/Move4");
	}

	public override void _Process(double delta)
	{
	}
}
