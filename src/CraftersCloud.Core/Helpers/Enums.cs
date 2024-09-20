using JetBrains.Annotations;

namespace CraftersCloud.Core.Helpers
{
    [PublicAPI]
    public static class Enums
    {
        public static T ValueFrom<T>(string value) where T : System.Enum => (T)System.Enum.Parse(typeof(T), value);

        public static IEnumerable<T> GetAll<T>() =>
            System.Enum.GetValues(typeof(T))
                .Cast<T>();

        public static IEnumerable<T> GetAllExcept<T>(T one) where T : System.Enum
        {
            if (one == null)
            {
                throw new ArgumentNullException(nameof(one));
            }

            return GetAll<T>().Except(new List<T> { one });
        }

        public static IEnumerable<T> GetAllExcept<T>(IEnumerable<T> exclusions) where T : System.Enum
        {
            if (exclusions == null)
            {
                throw new ArgumentNullException(nameof(exclusions));
            }

            return GetAll<T>().Except(exclusions);
        }

        public static IList<T> GetAllOrderedAlphabetically<T>() where T : System.Enum =>
            GetAll<T>().OrderBy(value => value.GetDescription().ToUpperInvariant()).ToList();
    }
}
