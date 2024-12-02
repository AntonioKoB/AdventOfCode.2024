using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day1 : AdventCalendarDay
    {
        private bool _isTestMode;
        protected override int Day => 1;

        public Day1() { }

        public override void Run(bool isTestMode)
        {
            _isTestMode = isTestMode;

            var input = GetInput();
            SolvePuzzleOne(input);
            SolvePuzzle2(input);
        }

        private static void SolvePuzzle2(Day1Input input)
        {
            var ocurrences = new Dictionary<int, int>();
            foreach (var number in input.LeftList)
            {
                if (!ocurrences.ContainsKey(number))
                {
                    ocurrences.Add(number, 0);
                }

                ocurrences[number] += input.RightList.Where(x => x == number).Count();
            }

            var similarityScore = ocurrences.Sum(x => x.Key * x.Value);

            Console.WriteLine($"Puzzle2 - {similarityScore}");
        }

        private static void SolvePuzzleOne(Day1Input input)
        {
            var distance = 0;
            do
            {
                distance += Math.Abs(input.Left.Pop() - input.Right.Pop());
            }
            while (input.Left.Count > 0 && input.Right.Count > 0);

            Console.WriteLine($"Puzzle1 - {distance}");
        }

        protected override Day1Input GetInput()
        {
            var fileName = GetInputFileName(1, _isTestMode);
            var input = File.ReadAllText(fileName);
            return new Day1Input(input);
        }
    }
}
