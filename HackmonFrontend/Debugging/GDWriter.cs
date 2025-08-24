using System.IO;
using System.Text;

namespace HackmonFrontend.Debugging;

public class GdWriter : StringWriter
{
	public override void Write(char value)
	{
		GD.Print(value);
	}

	public override void Write(char[] buffer, int index, int count)
	{
		GD.Print(new string(buffer, index, count));
	}

	public override void Write(ReadOnlySpan<char> buffer)
	{
		GD.Print(new string(buffer));
	}

	public override void Write(string? value)
	{
		GD.Print(value);
	}

	public override void Write(StringBuilder? value)
	{
		GD.Print(value?.ToString());
	}

	public override void WriteLine(ReadOnlySpan<char> buffer)
	{
		GD.Print(new string(buffer));
	}

	public override void WriteLine(StringBuilder? value)
	{
		GD.Print(value?.ToString());
	}
}