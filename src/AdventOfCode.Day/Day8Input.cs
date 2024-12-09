using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day8Input : IDayInput
    {
        public char[,] Map { get; private set; }

        public Day8Input(string input)
        {
            var lines = input.Replace("\r", "").Split('\n');
            int rows = lines.Length;
            int cols = lines[0].Length;
            Map = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Map[i, j] = lines[i][j];
                }
            }
        }
    }

}
