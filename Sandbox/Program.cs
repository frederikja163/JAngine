using JAngine.Rendering;

try
{
    using Window window = new Window("Test", 100, 100);
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();