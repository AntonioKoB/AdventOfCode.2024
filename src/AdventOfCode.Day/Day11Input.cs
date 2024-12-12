using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day11Input : IDayInput
    {
        public IEnumerable<long> Stones { get; set; }

        public Day11Input(string input)
        {
            Stones =
                input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        }

        internal void PrintStones()
        {
            Console.WriteLine(string.Join(" ", Stones));
        }
    }

}
