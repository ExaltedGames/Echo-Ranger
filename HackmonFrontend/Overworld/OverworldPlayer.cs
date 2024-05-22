using Godot;

namespace HackmonFrontend.Overworld;

public partial class OverworldPlayer : Node2D
{
	[Export]
	public float MoveTimeInSeconds = 0.25f;

	public bool Moving => _animateTime <= MoveTimeInSeconds;
	
	public Vector2I VirtualPosition;
	
    private TileMap _tileMap;
    private AnimatedSprite2D _sprite;
    
    private Vector2 _newPosition;
    private Vector2 _previousPosition;
    private float _animateTime = float.MaxValue;

    private StringName IdleUp = new("idle_up");
    private StringName IdleDown = new("idle_down");
    private StringName IdleLeft = new("idle_left");
    private StringName IdleRight = new("idle_right");
    private StringName WalkUp = new("walk_up");
    private StringName WalkDown = new("walk_down");
    private StringName WalkLeft = new("walk_left");
    private StringName WalkRight = new("walk_right");
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tileMap = GetParent<TileMap>();
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
		_sprite.Animation = IdleDown;
		_sprite.Play();
		VirtualPosition = _tileMap.LocalToMap(Position);
		Position = _tileMap.MapToLocal(VirtualPosition);
	}

	private void MoveToTile(Vector2I newMapPosition)
	{
		if (Moving) return;

		var relPosition = newMapPosition - VirtualPosition;
		if (relPosition.Y < 0)
		{
			_sprite.Animation = WalkUp;
		}
		else if (relPosition.Y > 0)
		{
			_sprite.Animation = WalkDown;
		}
		else if (relPosition.X < 0)
		{
			_sprite.Animation = WalkLeft;
		}
		else if (relPosition.X > 0)
		{
			_sprite.Animation = WalkRight;
		}
		
		_previousPosition = _tileMap.MapToLocal(VirtualPosition);
		_newPosition = _tileMap.MapToLocal(newMapPosition);
		VirtualPosition = newMapPosition;
		_animateTime = 0;
	}

	private void Animate(float delta)
	{
		if (Moving)
		{
			var t = _animateTime / MoveTimeInSeconds;
			Position = _previousPosition + ((_newPosition - _previousPosition) * t);
			_animateTime += delta;
		}
		else
		{
			if (_sprite.Animation == WalkUp)
			{
				_sprite.Animation = IdleUp;
			}
			else if (_sprite.Animation == WalkDown)
			{
				_sprite.Animation = IdleDown;
			}
			else if (_sprite.Animation == WalkLeft)
			{
				_sprite.Animation = IdleLeft;
			}
			else if (_sprite.Animation == WalkRight)
			{
				_sprite.Animation = IdleRight;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Animate((float)delta);
		
		if (Input.IsActionPressed("overworld_up"))
		{
			MoveToTile(VirtualPosition + new Vector2I(0, -1));
		}
		else if (Input.IsActionPressed("overworld_down"))
		{
			MoveToTile(VirtualPosition + new Vector2I(0, 1));
		}
		else if (Input.IsActionPressed("overworld_left"))
		{
			MoveToTile(VirtualPosition + new Vector2I(-1, 0));
		}
		else if (Input.IsActionPressed("overworld_right"))
		{
			MoveToTile(VirtualPosition + new Vector2I(1, 0));
		}
	}
}