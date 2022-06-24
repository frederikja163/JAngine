using JAngine;

namespace Sandbox;

public class Bootstrap : IBootstrap
{
    public void EngineInitialized()
    {
        Log.Info("Hello world from bootstrapper.");
    }
}