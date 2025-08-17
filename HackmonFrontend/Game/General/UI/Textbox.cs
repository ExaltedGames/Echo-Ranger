using System.Collections.Generic;

namespace HackmonFrontend.Game.General.UI;

public partial class Textbox : CanvasLayer
{
	public double TypewriterSpeed { get; set; } = 0.05;
	public bool Enabled { get; private set; }
	private readonly Queue<(string, Func<Task>?)> _messageList = new();
	private bool _awaitConfirm;
	private bool _awaitEvent;
	private Func<Task>? _currentEvent;
	private string _currentMessage;

	private TaskCompletionSource<bool> _done = new();
	private double _nextLetterTimer;
	private RichTextLabel _textBox;
	private int _typewriterPosition;

	public override void _Ready()
	{
		_textBox = GetNode<RichTextLabel>("OuterMargin/InnerMargin/Container/RichTextLabel");
		Enabled = true;
	}

	public override void _Input(InputEvent @event)
	{
		if (!Enabled)
			return;

		if (@event.IsActionPressed("ui_accept"))
			OnAccept();
	}

	public void OnAccept()
	{
		if (_typewriterPosition != _currentMessage.Length)
			_typewriterPosition = _currentMessage.Length;
		else if (!_awaitEvent)
		{
			if (_messageList.Count == 0)
			{
				_done.SetResult(true);
				Disable();
				return;
			}

			_typewriterPosition = 1;
			(_currentMessage, _currentEvent) = _messageList.Dequeue();
			if (_currentEvent != null)
			{
				_awaitEvent = true;
				_currentEvent().ContinueWith(t => _awaitEvent = false);
			}
			else
				_awaitEvent = false; //failsafe
		}
	}

	public override void _Process(double delta)
	{
		if (!Enabled)
			return;

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

		if (!Enabled)
			Enable();

		GD.Print($"{_messageList.Count}");
		(_currentMessage, _currentEvent) = _messageList.Dequeue();
		if (_currentEvent != null)
		{
			_awaitEvent = true;
			await _currentEvent().ContinueWith(t => _awaitEvent = false);
		}
		else
			_awaitEvent = false; //failsafe

		await _done.Task;
		await callback();
	}

	public void ShowMessagesSync(Func<Task> callback) => _ = ShowMessages(callback);

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