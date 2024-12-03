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
        private void SolvePuzzleTwo(Day4Input input)
        {
            
        }
        
        private void SolvePuzzleOne(Day4Input input)
        {
            
        }

        protected override Day4Input GetInput()
        {
            var fileName = GetInputFileName(1);
            var input1 = File.ReadAllText(fileName);
            return new Day4Input(input1);
        }
    }
}
