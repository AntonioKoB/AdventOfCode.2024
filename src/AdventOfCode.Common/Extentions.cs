using System.Numerics;
using System.Text;
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

        public static void PrintProgressBar(this long progress, BigInteger total, int barLength = 50)
        {
            if (total <= 0) throw new ArgumentException("Total value must be greater than zero.", nameof(total));

            double percentage = (double)progress / (double)total;
            int filledLength = (int)(barLength * percentage);

            StringBuilder progressBar = new StringBuilder();
            progressBar.Append('[');
            progressBar.Append(new string('█', filledLength));
            progressBar.Append(new string('-', barLength - filledLength));
            progressBar.Append(']');
            progressBar.Append($" {percentage:P1}");

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(progressBar.ToString());
        }


    }

}