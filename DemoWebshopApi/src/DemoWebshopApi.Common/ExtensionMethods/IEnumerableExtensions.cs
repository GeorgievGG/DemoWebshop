namespace DemoWebshopApi.Common.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null || action == null)
            {
                throw new ArgumentNullException();
            }
            foreach (var item in collection)
            {
                action(item);
            }
        }
    }
}
