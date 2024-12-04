using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day3 : AdventCalendarDay
    {
        protected override int Day => 3;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            var input = GetInput();
            SolvePuzzleOne(input);
            SolvePuzzleTwo(input);
        }

        private void SolvePuzzleTwo(Day3Input input)
        {
            var total = 0;
            foreach (var mul in input.ActiveMuls)
            {
                total += mul.Item1 * mul.Item2;
            }

            Console.WriteLine($"Total Active muls: {total}");
        }

        private void SolvePuzzleOne(Day3Input input)
        {
            var total = 0;
            foreach (var mul in input.Muls)
            {
                total += mul.Item1 * mul.Item2;
            }

            Console.WriteLine($"Total muls: {total}");
        }

        protected override Day3Input GetInput()
        {
            var fileName = GetInputFileName(1);
            var input1 = File.ReadAllText(fileName);
            var input2 = IsTestMode ? File.ReadAllText(GetInputFileName(2)) : input1;
            return new Day3Input(input1, input2);
        }
    }
}
