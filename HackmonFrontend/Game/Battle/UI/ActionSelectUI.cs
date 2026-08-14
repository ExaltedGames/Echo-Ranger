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
		for (var i = 0; i < _numActions; i++)
		{
			var moveIndex = i; //make a copy of i so it's safe to pass to functions
			Buttons[i] = GetNode<Button>($"MoveList/Move{i+1}/MoveButton{i+1}");
			Buttons[i].Pressed += () => OnMovePressed(moveIndex);
			Buttons[i].MouseEntered += () => OnMoveHovered(moveIndex);
			Buttons[i].MouseExited += () => OnMoveHoverednt(moveIndex);
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
		if (actions.Length > 6)
			throw new Exception("UI currently not built to support more than 6 moves.");

		var totalActions = 0;

		for (var i = 0; i < actions.Length; i++)
		{
			if (actions[i] == null)
				break;

			SelectableActions[i] = actions[i];
			Buttons[i].Text = actions[i]!.Name;
			Buttons[i].Icon =
				ResourceLoader.Load<DpiTexture>($"res://Assets/UI/Icons/Type/{actions[i]!.MoveType}Icon.svg");
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
