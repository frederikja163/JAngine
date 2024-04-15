namespace JAngine.Rendering.Gui;

public delegate float PositionDelegate(float parentPosition, float parentWidth, float selfWidth);

public sealed class Position
{
    private Position(PositionDelegate pos)
    {
        PositionDelegate = pos;
    }
    
    internal PositionDelegate PositionDelegate { get; }

    public static Position Function(PositionDelegate positionDelegate)
    {
        return new Position(positionDelegate);
    }
    
    public static Position operator +(Position left, float right)
    {
        return new Position((pp, pw, w) => left.PositionDelegate(pp, pw, w) + right);
    }
    
    public static Position operator +(float left, Position right)
    {
        return new Position((pp, pw, w) => left + right.PositionDelegate(pp, pw, w));
    }
    
    public static Position operator +(Position left, Size right)
    {
        return new Position((pp, pw, w) => left.PositionDelegate(pp, pw, w) + right.SizeDelegate(pw));
    }
    
    public static Position operator +(Size left, Position right)
    {
        return new Position((pp, pw, w) => left.SizeDelegate(pw) + right.PositionDelegate(pp, pw, w));
    }

    public static Position operator -(Position left, float right)
    {
        return new Position((pp, pw, w) => left.PositionDelegate(pp, pw, w) - right);
    }

    public static Position operator -(float left, Position right)
    {
        return new Position((pp, pw, w) => left - right.PositionDelegate(pp, pw, w));
    }

    public static Position operator -(Position left, Size right)
    {
        return new Position((pp, pw, w) => left.PositionDelegate(pp, pw, w) - right.SizeDelegate(pw));
    }

    public static Position operator -(Size left, Position right)
    {
        return new Position((pp, pw, w) => left.SizeDelegate(pw) - right.PositionDelegate(pp, pw, w));
    }

    public static Position Absolute(float position)
    {
        return new Position((pp, pw, w) => position);
    }

    public static Position Percentage(float percentage)
    {
        return new Position((pp, pw, w) => pp + pw * percentage - w * 0.5f);
    }
    
    public static Position Center()
    {
        return Percentage(0.5f);
    }

    public static Position LowerMargin(float margin = 0)
    {
        return new Position((pp, pw, w) => 
            pp + margin);
    }

    public static Position Left(float margin = 0) => LowerMargin(margin);

    public static Position Left(GuiElement element)
    {
        IGuiElement elem = (IGuiElement)element;
        return new Position((pp, pw, w) => elem.X);
    }
    public static Position Bottom(float margin = 0) => LowerMargin(margin);
    public static Position Bottom(GuiElement element)
    {
        IGuiElement elem = (IGuiElement)element;
        return new Position((pp, pw, w) => elem.Y);
    }
    
    public static Position UpperMargin(float margin = 0)
    {
        return new Position((pp, pw, w) => pp + pw - margin - w);
    }

    public static Position Right(float margin = 0) => UpperMargin(margin);
    public static Position Right(GuiElement element)
    {
        IGuiElement elem = (IGuiElement)element;
        return new Position((pp, pw, w) => elem.X + elem.Width - w);
    }
    public static Position Top(float margin = 0) => UpperMargin(margin);
    public static Position Top(GuiElement element)
    {
        IGuiElement elem = (IGuiElement)element;
        return new Position((pp, pw, w) => elem.Y + elem.Height - w);
    }
}
