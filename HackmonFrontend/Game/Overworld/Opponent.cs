using System;
using Godot;
using HackmonFrontend;

public partial class Opponent : CharacterBody2D
{
	private enum State
	{
		Idle,
		Wander,
		Chase,
		Search,
	}
	
	private State _state = State.Wander;
	private double _stateChangeTimer;
	private Vector2 _wanderDirection;
	private int _moveSpeed = 100;

	public override void _PhysicsProcess(double delta)
	{
		switch (_state)
		{
			case State.Idle:
				_stateChangeTimer -= delta;
				if (_stateChangeTimer <= 0)
				{
					_state = State.Wander;
					_stateChangeTimer = GD.RandRange(0.5, 1.5);
					_wanderDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1));
				}
				break;
			case State.Wander:
				_stateChangeTimer -= delta;
				if (_stateChangeTimer <= 0)
				{
					_state = State.Idle;
					_stateChangeTimer = GD.RandRange(1, 4);
					_wanderDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1));
				}

				Velocity = _wanderDirection * _moveSpeed;
				break;
			case State.Chase:
				// TODO Add chase state
				break;
			case State.Search:
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
