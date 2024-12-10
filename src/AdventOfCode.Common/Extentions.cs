using System.Text.Json;

namespace AdventOfCode.Common
{
    public static class Extensions
    {
        public static T Clone<T>(this T source) where T : IDayInput
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source cannot be null");
            }

            var JsonString = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(JsonString);
        }

        public static int TrimEnd<T>(this List<T> source, T item)
        {
            var count = 0;
            while (source.Last().Equals(item))
            {
                source.RemoveAt(source.Count - 1);
                count++;
            }

            return count;
        }

        public static void ReplaceAll<T>(this List<T> list, T oldValue, T newValue)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(oldValue))
                {
                    list[i] = newValue;
                }
            }
        }

        public static void ReplaceAll<T>(this List<T> list, int firstIndex, int amount, T newValue)
        {
            for (int i = firstIndex; i < firstIndex + amount && i < list.Count; i++)
            {
                list[i] = newValue;
            }
        }


    }

}
