using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day8 : AdventCalendarDay
    {
        private string? _inputCache;

        private Day8Input _input;

        protected override int Day => 8;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            _input = GetInput();
            SolvePuzzleOne();
            SolvePuzzleTwo();
        }

        private void SolvePuzzleOne()
        {
            int rows = _input.Map.GetLength(0);
            int cols = _input.Map.GetLength(1);
            var antinodePositions = new HashSet<(int, int)>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char frequency = _input.Map[i, j];
                    if (frequency == '.')
                        continue;

                    for (int x = 0; x < rows; x++)
                    {
                        for (int y = 0; y < cols; y++)
                        {
                            if ((x != i || y != j) && _input.Map[x, y] == frequency)
                            {
                                int dx = x - i;
                                int dy = y - j;

                                int antinode1X = i - dx;
                                int antinode1Y = j - dy;
                                int antinode2X = x + dx;
                                int antinode2Y = y + dy;

                                if (IsValidPosition(rows, cols, antinode1X, antinode1Y))
                                    antinodePositions.Add((antinode1X, antinode1Y));

                                if (IsValidPosition(rows, cols, antinode2X, antinode2Y))
                                    antinodePositions.Add((antinode2X, antinode2Y));
                            }
                        }
                    }
                }
            }

            int uniqueAntinodes = antinodePositions.Count;
            Console.WriteLine($"Total unique antinode locations: {uniqueAntinodes}");
        }

        private void SolvePuzzleTwo()
        {
            var antinodePositions = new HashSet<(int, int)>();

            // Group antennas by frequency
            var frequencyGroups = new Dictionary<char, List<(int, int)>>();
            for (int i = 0; i < _input.Map.GetLength(0); i++)
            {
                for (int j = 0; j < _input.Map.GetLength(1); j++)
                {
                    char frequency = _input.Map[i, j];
                    if (frequency != '.')
                    {
                        if (!frequencyGroups.ContainsKey(frequency))
                        {
                            frequencyGroups[frequency] = new List<(int, int)>();
                        }
                        frequencyGroups[frequency].Add((i, j));
                    }
                }
            }

            // Process each group
            foreach (var group in frequencyGroups.Values)
            {
                for (int i = 0; i < group.Count; i++)
                {
                    var (x1, y1) = group[i];
                    for (int j = i + 1; j < group.Count; j++)
                    {
                        var (x2, y2) = group[j];

                        // Calculate the slope and intercept of the line connecting the two antennas
                        double slope = (double)(y2 - y1) / (x2 - x1);
                        double intercept = y1 - slope * x1;

                        // Iterate along the line, marking antinode positions, excluding the positions of the two antennas
                        int dx = Math.Sign(x2 - x1);
                        int dy = Math.Sign(y2 - y1);
                        int x = x1 + dx;
                        int y = y1 + dy;

                        // Mark the starting point of the line as an antinode
                        antinodePositions.Add((x1, y1));

                        while (x >= 0 && y >= 0 && x < _input.Map.GetLength(0) && y < _input.Map.GetLength(1))
                        {
                            antinodePositions.Add((x, y));
                            x += dx;
                            y += dy;
                        }
                    }
                }
            }

            Console.WriteLine($"Total unique antinode locations puzzle two.. NOT WORKING!!!!!: {antinodePositions.Count}");
        }

        private bool IsValidPosition(int rows, int cols, int x, int y)
        {
            return x >= 0 && y >= 0 && x < rows && y < cols;
        }

        protected override Day8Input GetInput()
        {
            //Follow same structure as Day6
            if (string.IsNullOrEmpty(_inputCache))
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new(_inputCache);
        }

    }
}


/*

        private void SolvePuzzleTwo()
        {
            var antennas = ParseInput();
            var antinodePositions = new HashSet<(int, int)>();

            AddAntennaPositions(antinodePositions, antennas);
            CalculateAntinodes(antinodePositions, antennas);

            int uniqueAntinodes = antinodePositions.Count;
            Console.WriteLine($"Total unique antinode locations: {uniqueAntinodes}");
        }

        private Dictionary<char, List<(int, int)>> ParseInput()
        {
            int rows = _input.Map.GetLength(0);
            int cols = _input.Map.GetLength(1);
            var antennas = new Dictionary<char, List<(int, int)>>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char frequency = _input.Map[i, j];
                    if (frequency != '.')
                    {
                        if (!antennas.ContainsKey(frequency))
                        {
                            antennas[frequency] = new List<(int, int)>();
                        }
                        antennas[frequency].Add((i, j));
                    }
                }
            }

            return antennas;
        }

        private void AddAntennaPositions(HashSet<(int, int)> antinodePositions, Dictionary<char, List<(int, int)>> antennas)
        {
            foreach (var freqGroup in antennas)
            {
                foreach (var pos in freqGroup.Value)
                {
                    antinodePositions.Add(pos);
                }
            }
        }

        private void CalculateAntinodes(HashSet<(int, int)> antinodePositions, Dictionary<char, List<(int, int)>> antennas)
        {
            foreach (var freqGroup in antennas)
            {
                var positions = freqGroup.Value;

                for (int i = 0; i < positions.Count; i++)
                {
                    for (int j = i + 1; j < positions.Count; j++)
                    {
                        var (x1, y1) = positions[i];
                        var (x2, y2) = positions[j];

                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        int gcd = GCD(Math.Abs(dx), Math.Abs(dy));
                        int stepX = dx / gcd;
                        int stepY = dy / gcd;

                        int newX = x1;
                        int newY = y1;

                        while (true)
                        {
                            antinodePositions.Add((newX, newY));
                            if (newX == x2 && newY == y2)
                            {
                                break;
                            }
                            newX += stepX;
                            newY += stepY;
                        }

                        antinodePositions.Add((x2, y2));
                    }
                }
            }
        }

        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

 */


