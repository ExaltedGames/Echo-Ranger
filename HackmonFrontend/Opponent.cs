using System;
using Godot;
using HackmonFrontend;

public partial class Opponent : CharacterBody2D
{
	enum State
	{
		IDLE,
		WANDER,
		CHASE,
		SEARCH,
	}
	
	private State _state = State.WANDER;
	private double _stateChangeTimer;
	private Vector2 _wanderDirection;
	private int _moveSpeed = 100;

	public override void _PhysicsProcess(double delta)
	{
		switch (_state)
		{
			case State.IDLE:
				_stateChangeTimer -= delta;
				if (_stateChangeTimer <= 0)
				{
					_state = State.WANDER;
					_stateChangeTimer = GD.RandRange(0.5, 1.5);
					_wanderDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1));
				}
				break;
			case State.WANDER:
				_stateChangeTimer -= delta;
				if (_stateChangeTimer <= 0)
				{
					_state = State.IDLE;
					_stateChangeTimer = GD.RandRange(1, 4);
					_wanderDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1));
				}

				Velocity = _wanderDirection * _moveSpeed;
				break;
			case State.CHASE:
				// TODO Add chase state
				break;
			case State.SEARCH:
				// TODO Add search state
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
		MoveAndSlide();
	}

	private void OnAreaEntered(PhysicsBody2D body)
	{
		if (body is Player player)
		{
			GD.Print("Player has entered ENGAGEMENT ZONE.");
			GameManager.Instance.LoadTestBattleData();
		}
	}
}
