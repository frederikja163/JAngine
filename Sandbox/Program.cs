using System.Numerics;
using JAngine.Rendering;
using JAngine.Rendering.Gui;
using JAngine.Rendering.OpenGL;

try
{
    using Window window = new Window("Test", 100, 100);

    Size slotSize = Size.Pixels(50);
    Size margin = Size.Pixels(10);
    GuiElement hotbar = new GuiElement(window)
    {
        BackgroundColor = Vector4.One,
        Width = slotSize * 9 + margin * 10,
        Height = slotSize + margin * 2,
        X = Position.Center(),
        Y = Position.LowerMargin(25),
    };
    Position position = Position.Left(hotbar) + margin;
    for (int i = 1; i <= 9; i++)
    {
        GuiElement slot = new GuiElement(hotbar, new Texture(window, "Test", new Vector4[,]{{Vector4.UnitX}}))
        {
            BackgroundColor = Vector4.One * i / 9f,
            Width = slotSize,
            Height = slotSize,
            X = position,
            Y = Position.Center(),
        };
        position = Position.Right(slot) + slotSize + margin;
    }

    // GuiElement minimap = new GuiElement(window)
    // {
    //     BackgroundColor = new Vector4(0, 1, 0, 1),
    //     Width = Size.Pixels(200),
    //     Height = Size.Pixels(200),
    //     X = Position.LowerMargin(25),
    //     Y = Position.LowerMargin(25),
    // };
    //
    // GuiElement healthBar = new GuiElement(minimap)
    // {
    //     BackgroundColor = Vector4.UnitX,
    //     Width = Size.Fill(),
    //     Height = Size.Pixels(25),
    //     Y = Position.UpperMargin(-50),
    //     X = Position.Center(),
    // };
    
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();
