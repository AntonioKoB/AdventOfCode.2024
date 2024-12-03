namespace AdventOfCode.Common
{
    public abstract class AdventCalendarDay
    {
        protected abstract int Day { get; }
        public virtual void Run(bool isTestMode)
        {
            IsTestMode = isTestMode;
        }
        protected abstract IDayInput GetInput();

        protected bool IsTestMode { get; private set; }

        protected string GetInputFileName(int inputNumber)
        {
            return $"Inputs/{(IsTestMode ? "Test" : "")}Day{Day}Input{inputNumber}.txt";
        }
    }
}
