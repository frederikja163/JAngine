namespace JAngine.Rendering.Gui;

public delegate float SizeDelegate(float width);

public sealed class Size
{
    private Size(SizeDelegate size)
    {
        SizeDelegate = size;
    }
    
    internal SizeDelegate SizeDelegate { get; }

    public static Size Function(SizeDelegate sizeDelegate)
    {
        return new Size(sizeDelegate);
    }
    
    public static Size operator +(Size left, Size right)
    {
        return new Size(w => left.SizeDelegate(w) + right.SizeDelegate(w));
    }
    
    public static Size operator +(Size left, float right)
    {
        return new Size(w => left.SizeDelegate(w) + right);
    }
    
    public static Size operator +(float left, Size right)
    {
        return new Size(w => left + right.SizeDelegate(w));
    }
    
    public static Size operator -(Size left, Size right)
    {
        return new Size(w => left.SizeDelegate(w) - right.SizeDelegate(w));
    }
    
    public static Size operator -(Size left, float right)
    {
        return new Size(w => left.SizeDelegate(w) - right);
    }
    
    public static Size operator -(float left, Size right)
    {
        return new Size(w => left + right.SizeDelegate(w));
    }
    
    public static Size operator *(Size left, Size right)
    {
        return new Size(w => left.SizeDelegate(w) * right.SizeDelegate(w));
    }
    
    public static Size operator *(Size left, float right)
    {
        return new Size(w => left.SizeDelegate(w) * right);
    }
    
    public static Size operator *(float left, Size right)
    {
        return new Size(w => left * right.SizeDelegate(w));
    }
    
    public static Size operator /(Size left, Size right)
    {
        return new Size(w => left.SizeDelegate(w) / right.SizeDelegate(w));
    }
    
    public static Size operator /(Size left, float right)
    {
        return new Size(w => left.SizeDelegate(w) / right);
    }
    
    public static Size operator /(float left, Size right)
    {
        return new Size(w => left / right.SizeDelegate(w));
    }
    
    public static Size Pixels(float pixels)
    {
        return new Size(w => pixels);
    }

    public static Size PixelMargin(float margin)
    {
        return new Size(w => w - margin * 2);
    }

    public static Size PercentageMargin(float margin)
    {
        return new Size(w => w - margin * 2);
    }

    public static Size Percentage(float percentage)
    {
        return new Size(w => w * percentage);
    }
    
    public static Size Fill()
    {
        return Percentage(1.0f);
    }
}
