namespace HackmonFrontend.Debugging;

public static class DebugPlug
{
	public static void Init()
	{
		Console.SetOut(new GdWriter());
		Console.SetError(new GdErrWriter());
	}
}