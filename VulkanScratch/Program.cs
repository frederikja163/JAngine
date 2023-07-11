
using System.Diagnostics;

public static class Program
{
    public const int Width = 800;
    public const int Height = 600;
    
    internal static void Main(string[] args)
    {
        try
        {
            using HelloTriangleApplication app = new HelloTriangleApplication();
            app.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine(e.StackTrace);
            
            Debug.WriteLine(e);
            Debug.WriteLine(e.StackTrace);
        }
    }
}