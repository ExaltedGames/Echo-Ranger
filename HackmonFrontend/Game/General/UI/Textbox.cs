using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Textbox : CanvasLayer
{
	private RichTextLabel _textBox;
	private bool _awaitConfirm = false;
	private int _typewriterPosition = 0;
	private double _nextLetterTimer = 0;
	private bool _awaitEvent = false;

	private readonly Queue<(string, Func<Task>?)> _messageList = new();
	private string _currentMessage;
	private Func<Task>? _currentEvent;

	private TaskCompletionSource<bool> _done = new();

	public double TypewriterSpeed { get; set; } = 0.05;
	public bool Enabled { get; private set; }
	
	public override void _Ready()
	{
		_textBox = GetNode<RichTextLabel>("OuterMargin/InnerMargin/Container/RichTextLabel");
		Enabled = true;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!Enabled) return;
		if (@event.IsActionPressed("ui_accept"))
		{
			if (_typewriterPosition != _currentMessage.Length)
			{
				_typewriterPosition = _currentMessage.Length;
			}
			else if(!_awaitEvent)
			{
				if (_messageList.Count == 0)
				{
					_done.SetResult(true);
					Disable();
					return;
				}
				_typewriterPosition = 1;
				(_currentMessage, _currentEvent) =  _messageList.Dequeue();
				if (_currentEvent != null)
				{
					_awaitEvent = true;
					_currentEvent().ContinueWith(t => _awaitEvent = false);
				}
				else _awaitEvent = false; //failsafe
			}
		}
	}

	public override void _Process(double delta)
	{
		if (!Enabled) return;
		if (_typewriterPosition != _currentMessage.Length)
		{
			_nextLetterTimer += delta;
			if (_nextLetterTimer >= TypewriterSpeed)
			{
				_nextLetterTimer = 0;
				_typewriterPosition++;
			}
		}

		_textBox.Text = _currentMessage[.._typewriterPosition];
	}

	public void QueueMessage(string message)
	{
		_messageList.Enqueue((message, null));
	}
	
	public void QueueMessage(string message, Func<Task> @event)
	{
		_messageList.Enqueue((message, @event));	
	}

	public async Task ShowMessages(Func<Task> callback)
	{
		_typewriterPosition = 0;
		_done = new TaskCompletionSource<bool>();
		
		if (!Enabled) Enable();
		GD.Print($"{_messageList.Count}");
		(_currentMessage, _currentEvent) = _messageList.Dequeue();
		if (_currentEvent != null)
		{
			_awaitEvent = true;
			await _currentEvent().ContinueWith(t => _awaitEvent = false);
		}
		else _awaitEvent = false; //failsafe
		
		await _done.Task;
		await callback();
	}

	public void ShowMessagesSync(Func<Task> callback) => ShowMessages(callback).Wait();

	public void SetText(string text)
	{
		_textBox.Text = text;
	}

	public void Disable()
	{
		Enabled = false;
		Visible = false;
	}

	public void Enable()
	{
		Enabled = true;
		Visible = true;
	}
}
