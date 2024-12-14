using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day14 : AdventCalendarDay
    {
        protected override int Day => 14;

        private string _inputCache;

        override public void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            SolvePuzzleOne();
            SolvePuzzleTwo();
        }

        private void SolvePuzzleOne()
        {
            var input = GetInput();

            Thread.Sleep(1000);
            Console.Clear();

            Console.WriteLine("Initial Position");
            input.PrintMap();

            for (int i = 0; i < 100; i++)
            {
                if(IsTestMode)
                    Console.WriteLine($"After {i + 1} second{(i == 0 ? "" : "s")}");

                foreach (var robot in input.Robots)
                {
                    robot.Move();
                }

                if(IsTestMode)
                    input.PrintMap();
            }

            Console.WriteLine("Final map");
            input.PrintMap(true);

            int[] quadrants = new int[4];

            foreach (var robot in input.Robots)
            {
                var quadrant = robot.GetQuadrant();
                if(quadrant >= 0)
                {
                    quadrants[quadrant]++;
                }
            }

            Console.WriteLine($"Quadrants: {string.Join(", ", quadrants)}");
            Console.WriteLine($"Quadrants product: {quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3]}");
        }

        private async void SolvePuzzleTwo()
        {
            var input = GetInput();
            for (int i = 0; i < 10000; i++)
            {
                foreach (var robot in input.Robots)
                {
                    robot.Move();
                }
                var message = "                                ";

                if (ThereIsRobotsInARowOf(10, input.Robots))
                    message = input.PrintMapImage(i+1);

                ((long)i).PrintProgressBar(10000 - 1, message: $"[{message}]");
            }
        }

        private bool ThereIsRobotsInARowOf(int countInRow, IEnumerable<Robot> robots)
        {
            foreach (var robot in robots)
            {
                var count = 0;
                var currentX = robot.CurrentPosition.X;

                var otherRobotsInLine = robots.Where(r => r.CurrentPosition.X > currentX && r.CurrentPosition.Y == robot.CurrentPosition.Y).OrderBy(r => r.CurrentPosition.X);

                foreach (var otherRobot in otherRobotsInLine)
                {
                    if(otherRobot.CurrentPosition.X == currentX + 1 && robot.CurrentPosition.Y == otherRobot.CurrentPosition.Y)
                    {
                        count++;
                        currentX++;

                        if (count >= countInRow)
                            return true;
                    }
                }
            }

            return false;
        }

        protected override Day14Input GetInput()
        {
            if (_inputCache == null)
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new Day14Input(_inputCache, IsTestMode);
        }
    }
}
