namespace JAngine.Rendering.Gui;

public delegate float PositionDelegate(float parentWidth, float selfWidth);

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

    public static Position Percentage(float percentage)
    {
        return new Position((p, _) => p * percentage);
    }
    
    public static Position Center()
    {
        return Percentage(0.5f);
    }

    public static Position LowerMargin(float margin = 0)
    {
        return new Position((p, s) => s/2f + margin);
    }
    
    public static Position UpperMargin(float margin = 0)
    {
        return new Position((p, s) => p - s/2f - margin);
    }
}
