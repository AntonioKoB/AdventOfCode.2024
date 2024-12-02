namespace AdventOfCode.Common
{
    public abstract class AdventCalendarDay
    {
        protected abstract int Day { get; }
        public abstract void Run(bool isTestMode);
        protected abstract IDayInput GetInput();

        protected string GetInputFileName(int inputNumber, bool isTestMode)
        {
            return $"Inputs/{(isTestMode ? "Test" : "")}Day{Day}Input{inputNumber}.txt";
        }
    }
}
