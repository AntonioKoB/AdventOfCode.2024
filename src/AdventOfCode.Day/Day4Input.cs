using AdventOfCode.Common;

namespace AdventOfCode.Day
{

    public class Day4Input : IDayInput
    {
        public char[,] xWords { get; private set; }
        public Day4Input(string input)
        {
            var lines = input.Trim().Replace("\r","").Split('\n');
            int rows = lines.Length;
            int cols = lines[0].Length;

            xWords = new char[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    xWords[i, j] = lines[i][j];
        }
    }
}
