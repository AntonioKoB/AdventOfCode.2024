using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day7 : AdventCalendarDay
    {
        protected override int Day => 7;
        private List<char[]> _combinations;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            var input = GetInput();
            Console.Clear();
            SolvePuzzleOne(input);
            SolvePuzzleTwo(input);
        }

        private void SolvePuzzleOne(Day7Input input)
        {
            char[] elements = { '+', '*' };

            var correctOperations = new List<long>();

            foreach(var equation in input.CalibrationEquation)
            {
                _combinations = new List<char[]>();

                int length = equation.Value.Skip(1).Count();
                char[] combination = new char[length];
                GenerateCombinations(elements, length, combination, 0);

                if(TestEquation(equation.Key, equation.Value))
                {
                    correctOperations.Add(equation.Key);
                }
            }

            Console.WriteLine($"Correct operations: {correctOperations.Sum()}");
        }

        private void GenerateCombinations(char[] elements, int length, char[] combination, int position)
        {
            if (position == length)
            {
                _combinations.Add((char[])combination.Clone());
                return;
            }

            foreach (var element in elements)
            {
                combination[position] = element;
                GenerateCombinations(elements, length, combination, position + 1);
            }
        }


        private bool TestEquation(long test, List<long> items)
        {
            foreach(var combination in _combinations)
            {
                var result = items[0];

                for (int i = 1; i < items.Count; i++)
                {
                    var item = items[i];
                    var op = combination[i - 1];
                    switch (op)
                    {
                        case '+':
                            result += item;
                            break;
                        case '*':
                            result *= item;
                            break;
                        case '|':
                            result = long.Parse($"{result}{item}");
                            break;
                    }
                }
                if(result == test)
                {
                    return true;
                }
            }

            return false;
        }

        private void SolvePuzzleTwo(Day7Input input)
        {
            char[] elements = { '+', '*', '|' };

            var correctOperations = new List<long>();

            foreach (var equation in input.CalibrationEquation)
            {
                _combinations = new List<char[]>();

                int length = equation.Value.Skip(1).Count();
                char[] combination = new char[length];
                GenerateCombinations(elements, length, combination, 0);

                if (TestEquation(equation.Key, equation.Value))
                {
                    correctOperations.Add(equation.Key);
                }
            }

            Console.WriteLine($"Correct operations: {correctOperations.Sum()}");
        }

        private string _inputCache;
        protected override Day7Input GetInput()
        {
            //similar to day 6
            if (string.IsNullOrEmpty(_inputCache))
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new Day7Input(_inputCache);
        }
    }
}



