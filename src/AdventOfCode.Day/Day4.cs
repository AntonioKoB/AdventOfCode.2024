using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day4 : AdventCalendarDay
    {
        protected override int Day => 4;
        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            var input = GetInput();
            SolvePuzzleOne(input);
            SolvePuzzleTwo(input);
        }
        private void SolvePuzzleOne(Day4Input input)
        {
            var word = "XMAS";
            int count = SearchWord(input.xWords, word);
            Console.WriteLine($"Number of times {word} appears: {count}");
        }

        private void SolvePuzzleTwo(Day4Input input)
        {
            int count = FindMASCrosses(input.xWords);
            Console.WriteLine($"MAS crosses found: {count}");
        }

        protected override Day4Input GetInput()
        {
            var fileName = GetInputFileName(1);
            var input1 = File.ReadAllText(fileName);
            return new Day4Input(input1);
        }

        private static int SearchWord(char[,] matrix, string word)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int wordLen = word.Length;
            int count = 0;
            // All possible directions (right, left, down, up, and diagonals)
            int[][] directions = new int[][]
            {
                new int[] { 0, 1 },
                new int[] { 0, -1 },
                new int[] { 1, 0 },
                new int[] { -1, 0 },
                new int[] { 1, 1 },
                new int[] { -1, -1 },
                new int[] { 1, -1 },
                new int[] { -1, 1 }
            };

            bool SearchFrom(int x, int y, int dx, int dy)
            {
                for (int i = 0; i < wordLen; i++)
                {
                    int nx = x + i * dx;
                    int ny = y + i * dy;
                    if (nx < 0 || nx >= rows || ny < 0 || ny >= cols || matrix[nx, ny] != word[i])
                        return false;
                }

                return true;
            }

            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    foreach (var dir in directions)
                        if (SearchFrom(x, y, dir[0], dir[1]))
                            count += 1;

            return count;
        }

        private int FindMASCrosses(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int count = 0;

            // Check for "MAS" and "SAM" cross patterns around each 'A'
            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < cols - 1; j++)
                {
                    if (matrix[i, j] == 'A')
                    {
                        /*
                         * M M
                         *  A
                         * S S
                         */
                        if (matrix[i - 1, j - 1] == 'M' && matrix[i + 1, j + 1] == 'S' &&
                            matrix[i + 1, j - 1] == 'M' && matrix[i - 1, j + 1] == 'S')
                        {
                            count++;
                        }

                        /*
                         * S S
                         *  A
                         * M M
                         */
                        if (matrix[i - 1, j - 1] == 'S' && matrix[i + 1, j + 1] == 'M' &&
                            matrix[i + 1, j - 1] == 'S' && matrix[i - 1, j + 1] == 'M')
                        {
                            count++;
                        }

                        /*
                         * M S
                         *  A
                         * M S
                         */
                        if (matrix[i - 1, j - 1] == 'M' && matrix[i + 1, j + 1] == 'S' &&
                            matrix[i + 1, j - 1] == 'S' && matrix[i - 1, j + 1] == 'M')
                        {
                            count++;
                        }

                        /*
                         * S M
                         *  A
                         * S M
                         */
                        if (matrix[i - 1, j - 1] == 'S' && matrix[i + 1, j + 1] == 'M' &&
                            matrix[i + 1, j - 1] == 'M' && matrix[i - 1, j + 1] == 'S')
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }
    }
}



