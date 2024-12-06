using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day5 : AdventCalendarDay
    {
        protected override int Day => 5;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            var input = GetInput();
            SolvePuzzleOne(input);
            SolvePuzzleTwo(input);
        }

        private void SolvePuzzleOne(Day5Input input)
        {
            var middlepageaddiction = 0;
            foreach (var update in input.Updates)
            {
                if (IsValid(update, input.Rules))
                {
                    middlepageaddiction += update[update.Length / 2];
                }
            }

            Console.WriteLine($"Middle page SUM: {middlepageaddiction}");
        }

        private bool IsValid(int[] update, List<Tuple<int, int>> rules)
        {
            foreach (var rule in rules)
            {
                int pageA = rule.Item1;
                int pageB = rule.Item2;

                int indexA = Array.IndexOf(update, pageA);
                int indexB = Array.IndexOf(update, pageB);

                if (indexA != -1 && indexB != -1) // Both pages are present
                {
                    if (indexA > indexB) // Page A must appear before Page B
                    {
                        return false;
                    }
                }
                // If one or neither of the pages is present, the rule is considered valid
            }
            return true;
        }


        private void SolvePuzzleTwo(Day5Input input)
        {
            var middlepageaddiction = 0;
            foreach (var update in input.Updates)
            {
                if (!IsValid(update, input.Rules))
                {
                    var orderedUpdate = Reorder(update, input.Rules);
                    while (!IsValid(orderedUpdate, input.Rules))
                    {
                        orderedUpdate = Reorder(orderedUpdate, input.Rules);
                    }

                    middlepageaddiction += orderedUpdate[update.Length / 2];
                }
            }

            Console.WriteLine($"Middle page SUM: {middlepageaddiction}");
        }
        private int[] Reorder(int[] update, List<Tuple<int, int>> rules)
        {
            List<int> orderedUpdates = new List<int>(update);

            foreach (var rule in rules)
            {
                int pageA = rule.Item1;
                int pageB = rule.Item2;

                int indexA = orderedUpdates.IndexOf(pageA);
                int indexB = orderedUpdates.IndexOf(pageB);

                if (indexA != -1 && indexB != -1 && indexA > indexB)
                {
                    orderedUpdates.RemoveAt(indexA);
                    orderedUpdates.Insert(indexB, pageA);
                }
            }

            return orderedUpdates.ToArray();
        }

        protected override Day5Input GetInput()
        {
            var fileName = GetInputFileName(1);
            var input1 = File.ReadAllText(fileName);
            return new Day5Input(input1);
        }
    }
}



