using JAngine;

namespace Sandbox;

internal static class Program
{
    internal static void Main()
    {
        using var game = new Game(new GameSettings());
        
        game.Run();
    }
}

