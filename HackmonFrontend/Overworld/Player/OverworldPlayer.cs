using Godot;

public partial class OverworldPlayer : CharacterBody2D
{
	[Export]
	public bool TilemapSnap = true;

	public Vector2I TilePosition
		=> _tileMap.LocalToMap(Position);

	private Vector2I _targetPosition;
	
	private AnimatedSprite2D _animator;
	private TileMap _tileMap;
	
	public override void _Ready()
	{
		var children = GetChildren();
		foreach (var c in children)
		{
			if (c is AnimatedSprite2D a)
			{
				_animator = a;
				break;
			}
		}

		_tileMap = GetParentOrNull<TileMap>();
		
		_animator.Play();
	}

	public override void _Input(InputEvent @event)
	{
		var input_direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		if (TilemapSnap)
		{
			input_direction = Snap(input_direction.Ceil());
			
			_targetPosition = (Vector2I)(TilePosition + input_direction);

			if (!_tileMap.GetCellTileData(0, _targetPosition).GetCustomData("traversable").AsBool())
			{
				_targetPosition = TilePosition;
			}
		}
		
		Velocity = input_direction * 30;
	}

	public override void _Process(double delta)
	{
		if (Velocity.Length() > 0)
		{
			if (Mathf.Abs(Velocity.X) >= Mathf.Abs(Velocity.Y))
			{
				PlayAnimation("moving_horizontal");
				_animator.FlipH = Mathf.Sign(Velocity.X) == -1;
			}
			else
			{
				PlayAnimation(Mathf.Sign(Velocity.Y) == -1 ? "moving_up" : "moving_down");
			}
		}
		else
		{
			PlayAnimation("idle");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (TilemapSnap)
		{
			GD.Print(_targetPosition);
			var targetLocalPos = _tileMap.MapToLocal(_targetPosition);
			var dir = Position - targetLocalPos;
			if (dir.Length() >= 5)
			{
				MoveAndCollide(dir.Normalized().Ceil() * 30 * (float)delta);
			}
		}
		else
		{
			MoveAndSlide();
		}
	}

	private void PlayAnimation(string animName)
	{
		if (_animator.Animation != animName)
			_animator.Stop();
		_animator.Play(animName);
	}
	
	public Vector2 Snap(Vector2 v) 
	{
		var angle = Mathf.Atan2(v.Y, v.X);
		var hypotenuse = Mathf.Sqrt((v.X * v.X) + (v.Y * v.Y));
		var snapRad = 90 * (Mathf.Pi / 180);
		var snapTo = Mathf.Round(angle / snapRad) * snapRad;
		var snappedVector = new Vector2(Mathf.Cos(snapTo) * hypotenuse, Mathf.Sin(snapTo) * hypotenuse);

		return snappedVector; 
	}
}
