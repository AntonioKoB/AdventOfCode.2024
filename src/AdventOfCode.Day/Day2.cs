using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day2 : AdventCalendarDay
    {
        protected override int Day => 2;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);

            var input = GetInput();

            SolvePuzzleOne(input);
            SolvePuzzleTwo(input);
        }

        protected override Day2Input GetInput()
        {
            var fileName = GetInputFileName(1);
            var input = File.ReadAllText(fileName);
            return new Day2Input(input);
        }

        private void SolvePuzzleTwo(Day2Input input)
        {
            int safeReportsCount = 0;

            foreach (var report in input.Reports)
            {
                if (IsValidReportWithProblemDampener(report))
                {
                    safeReportsCount++;
                }
            }

            Console.WriteLine($"Number of safe reports: {safeReportsCount}");
        }

        private bool IsValidReportWithProblemDampener(int[] report)
        {
            if (IsValidReport(report))
            {
                return true;
            }

            for (int i = 0; i < report.Length; i++)
            {
                var modifiedReport = report.Where((_, index) => index != i).ToArray();
                if (IsValidReport(modifiedReport))
                {
                    return true;
                }
            }

            return false;
        }

        private void SolvePuzzleOne(Day2Input input)
        {
            int validReportsCount = 0;

            foreach (var report in input.Reports)
            {
                if (IsValidReport(report))
                {
                    validReportsCount++;
                }
            }

            Console.WriteLine($"Number of valid reports: {validReportsCount}");
        }

        private bool IsValidReport(int[] report)
        {
            if (report.Length < 2 || report[0] == report[1])
            {
                return false;
            }

            bool increasing = report[0] < report[1];
            bool decreasing = report[0] > report[1];

            for (int i = 1; i < report.Length; i++)
            {
                if (increasing && (report[i] <= report[i - 1] || Math.Abs(report[i] - report[i - 1]) > 3))
                {
                    return false;
                }

                if (decreasing && (report[i] >= report[i - 1] || Math.Abs(report[i] - report[i - 1]) > 3))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
