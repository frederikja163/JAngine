namespace JAngine.Core;

internal static class LinqExtensions
{
    // Source: https://stackoverflow.com/a/14741288
    public static IEnumerable<T> CatchExceptions<T> (this IEnumerable<T> src, Action<Exception> action = null)
    {
        using var enumerator = src.GetEnumerator();
        bool next = true;

        while (next) {
            try {
                next = enumerator.MoveNext();
            } catch (Exception ex) {
                if (action != null) {
                    action(ex);
                }
                continue;
            }

            if (next) {
                yield return enumerator.Current;
            }
        }
    }
}