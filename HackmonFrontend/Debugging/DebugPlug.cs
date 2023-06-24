using System;

namespace Hackmon.Debugging;

public static class DebugPlug
{
    public static void Init()
    {
        Console.SetOut(new GDWriter());
        Console.SetError(new GDErrWriter());
    }
}