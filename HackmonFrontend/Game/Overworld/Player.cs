namespace HackmonFrontend.Game.Overworld;

public partial class Player : CharacterBody2D
{
	private int _moveSpeed;
	private int _runSpeed = 500;
	private int _walkSpeed = 250;

	public void GetInput()
	{
		_moveSpeed = _walkSpeed;
		if (Input.IsActionPressed("sprint"))
			_moveSpeed = _runSpeed;

		var inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection * _moveSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}