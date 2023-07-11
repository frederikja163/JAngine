using System.Runtime.InteropServices;

namespace VulkanScratch.Extensions;

internal readonly unsafe struct StringPtr : IDisposable
{
    internal readonly unsafe byte* _value;
        
    public StringPtr(string str)
    {
        _value = (byte*)Marshal.StringToCoTaskMemAnsi(str);
    }
        
    public static implicit operator byte*(StringPtr strPtr)
    {
        return strPtr._value;
    }

    public void Dispose()
    {
        Marshal.FreeCoTaskMem((IntPtr)_value);
    }
}