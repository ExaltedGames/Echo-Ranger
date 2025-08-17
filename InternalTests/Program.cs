namespace InternalTests;

public static class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("Running tests...");
		Tests.TestMoveLoading();
		Tests.TestBattle();
	}
}