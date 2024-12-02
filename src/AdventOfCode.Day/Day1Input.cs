using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day1Input : IDayInput
    {
        public Stack<int> Left { get; private set; }
        public Stack<int> Right { get; private set; }

        public IEnumerable<int> LeftList { get; private set; }
        public IEnumerable<int> RightList { get; private set; }

        public Day1Input(string input1)
        {
            Left = new Stack<int>();
            Right = new Stack<int>();

            LeftList = input1.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]));

            RightList = input1.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]));

            LeftList.Order().ToList().ForEach(i => Left.Push(i));
            RightList.Order().ToList().ForEach(i => Right.Push(i));
        }
    }
}
