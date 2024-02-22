using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Textbox : CanvasLayer
{
	private RichTextLabel TextBox;
	private bool AwaitConfirm = false;
	private int typewriterPosition = 0;
	private double nextLetterTimer = 0;

	private List<string> messageList = new();
	private int messageIndex = 0;
	private string currentMessage => messageList[messageIndex];

	private TaskCompletionSource<bool> done = new();

	public double TypewriterSpeed { get; set; } = 0.05;
	public bool Enabled { get; private set; }
	
	public override void _Ready()
	{
		TextBox = GetNode<RichTextLabel>("OuterMargin/InnerMargin/Container/RichTextLabel");
		Enabled = true;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			if (typewriterPosition != currentMessage.Length)
			{
				typewriterPosition = currentMessage.Length;
			}
			else
			{
				if (messageIndex == messageList.Count - 1)
				{
					done.SetResult(true);
					return;
				}
				typewriterPosition = 1;
				messageIndex++;
			}
		}
	}

	public override void _Process(double delta)
	{
		if (!Enabled) return;
		if (messageList.Count <= 0)
		{
			Disable();
			return;
		}
		if (typewriterPosition != currentMessage.Length)
		{
			nextLetterTimer += delta;
			if (nextLetterTimer >= TypewriterSpeed)
			{
				nextLetterTimer = 0;
				typewriterPosition++;
			}
		}

		TextBox.Text = currentMessage.Substring(0, typewriterPosition);
	}

	public async Task ShowMessages(List<string> messages, Action callback)
	{
		messageList = messages;
		messageIndex = 0;
		if (!Enabled) Enable();
		
		GD.Print($"Loaded {messages.Count} messages");
		
		await done.Task;
		callback();
	}

	public void SetText(string text)
	{
		TextBox.Text = text;
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
