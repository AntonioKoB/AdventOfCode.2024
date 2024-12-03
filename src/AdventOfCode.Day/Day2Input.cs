using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day2Input : IDayInput
    {
        public IEnumerable<int[]> Reports { get; private set; }

        public Day2Input(string input)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Reports = lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i)).ToArray()).ToList();
        }
    }
}
