using Godot;
using System;

public partial class Textbox : CanvasLayer
{
	private RichTextLabel TextBox;
	
	public bool Enabled { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextBox = GetNode<RichTextLabel>("OuterMargin/InnerMargin/Container/RichTextLabel");
		Enabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
