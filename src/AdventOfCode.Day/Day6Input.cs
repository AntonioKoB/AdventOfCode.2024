using AdventOfCode.Common;
using System.Text;

namespace AdventOfCode.Day
{
    public class Day6Input : IDayInput
    {
        public char[][] Room { get; private set; }

        public Day6Input(string input)
        {
            Room = input.Trim().Replace("\r", "").Split('\n').Select(x => x.ToCharArray()).ToArray();
        }

        public void PrintRoom(bool isTest, int sleepTime)
        {
            if (!isTest)
                return;

            var sb = new StringBuilder();

            sb.AppendLine(new string('-', Room[0].Length));

            foreach (var row in Room)
            {
                sb.Append("|");
                foreach (var cell in row)
                {
                    sb.Append(cell);
                }
                sb.AppendLine("|");
            }

            sb.AppendLine(new string('-', Room[0].Length));
            sb.AppendLine();

            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());

            Thread.Sleep(sleepTime);
        }

    }
}
