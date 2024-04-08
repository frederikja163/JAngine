using System.Numerics;
using JAngine.Rendering;
using JAngine.Rendering.Gui;

try
{
    using Window window = new Window("Test", 100, 100);

    Size slotSize = Size.Pixels(50);
    GuiElement hotbar = new GuiElement(window)
    {
        BackgroundColor = Vector4.One,
        Width = Size.Pixels(400),
        Height = slotSize,
        X = Position.Center(),
        Y = Position.LowerMargin(25),
    };
    for (int i = 0; i < 9; i++)
    {
        GuiElement slot = new GuiElement(hotbar)
        {
            BackgroundColor = Vector4.UnitX * i / 9f,
            Width = slotSize,
            Height = slotSize,
            X = Position.Percentage((i + 0.5f) / 9f),
            Y = Position.Center(),
        };
    }
    
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();
