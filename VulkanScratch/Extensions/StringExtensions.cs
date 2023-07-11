namespace VulkanScratch.Extensions;

internal static class StringExtensions
{
    internal static unsafe StringPtr GetPtr(this string str)
    {
        return new StringPtr(str);
    }
}