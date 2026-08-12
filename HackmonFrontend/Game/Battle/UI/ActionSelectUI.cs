namespace HackmonFrontend.Game.Battle.UI;

public partial class ActionSelectUI : Control
{
	public delegate void ActionSelectHandler(HackmonMove action);

	public event ActionSelectHandler? OnActionSelected;

	public Button[] Buttons = new Button[6];
	public int CurrentSelection = -1;
	public HackmonMove?[] SelectableActions = new HackmonMove[6];
	private TextEdit _infoBox;
	private int _numActions = 6;

	public override void _Ready()
	{
		_infoBox = GetNode<TextEdit>("Infobox");
		Buttons[0] = GetNode<Button>("MoveList/Move1/MoveButton1");
		Buttons[0].Pressed += () => OnMovePressed(0);
		Buttons[0].MouseEntered += () => OnMoveHovered(0);
		Buttons[0].MouseExited += () => OnMoveHoverednt(0);
		Buttons[1] = GetNode<Button>("MoveList/Move2/MoveButton2");
		Buttons[1].Pressed += () => OnMovePressed(1);
		Buttons[1].MouseEntered += () => OnMoveHovered(1);
		Buttons[1].MouseExited += () => OnMoveHoverednt(1);
		Buttons[2] = GetNode<Button>("MoveList/Move3/MoveButton3");
		Buttons[2].Pressed += () => OnMovePressed(2);
		Buttons[2].MouseEntered += () => OnMoveHovered(2);
		Buttons[2].MouseExited += () => OnMoveHoverednt(2);
		Buttons[3] = GetNode<Button>("MoveList/Move4/MoveButton4");
		Buttons[3].Pressed += () => OnMovePressed(3);
		Buttons[3].MouseEntered += () => OnMoveHovered(3);
		Buttons[3].MouseExited += () => OnMoveHoverednt(3);
		Buttons[4] = GetNode<Button>("MoveList/Move5/MoveButton5");
		Buttons[4].Pressed += () => OnMovePressed(4);
		Buttons[4].MouseEntered += () => OnMoveHovered(4);
		Buttons[4].MouseExited += () => OnMoveHoverednt(4);
		Buttons[5] = GetNode<Button>("MoveList/Move6/MoveButton6");
		Buttons[5].Pressed += () => OnMovePressed(5);
		Buttons[5].MouseEntered += () => OnMoveHovered(5);
		Buttons[5].MouseExited += () => OnMoveHoverednt(5);
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
		if (actions.Length > 6)
			throw new Exception("UI currently not built to support more than 6 moves.");

		var totalActions = 0;

		for (var i = 0; i < actions.Length; i++)
		{
			if (actions[i] == null)
				break;

			SelectableActions[i] = actions[i];
			Buttons[i].Text = actions[i]!.Name;
			Buttons[i].Disabled = false;
			Buttons[i].Visible = true;
			totalActions++;
		}

		for (var i = totalActions; i < 6; i++)
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

	public override void _Process(double delta)
	{
	}

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
			_infoBox.Text =
				$"{currentMove!.Description}\nType: {currentMove.AttackType}\nDamage: {currentMove.Damage}      Cost: {currentMove.StaminaCost}";
		}
	}

	private void OnMoveHovered(int hovered)
	{
		Buttons[hovered].SetScale(new(1.1f,1.1f));
	}
	private void OnMoveHoverednt(int hoverednt)
	{
		Buttons[hoverednt].SetScale(new(1f,1f));
	}
}
