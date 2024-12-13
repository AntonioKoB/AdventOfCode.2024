using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day12Input : IDayInput
    {
        public char[][] GardenMap { get; set; }
        public Day12Input(string input)
        {
            GardenMap = ConvertInputToArray(input);
        }

        public static char[][] ConvertInputToArray(string input)
        {
            var lines = input.Trim().Split('\n');
            var array = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                array[i] = lines[i].Trim().ToCharArray();
            }
            return array;
        }

    }

}
