using AdventOfCode.Common;
using System.Drawing;

namespace AdventOfCode.Day
{
    public class Day10Input : IDayInput
    {
        public Dictionary<Point, int> TrailingMap { get; set; }

        public Point MaxPoint { get; private set; }
        public Day10Input(string input)
        {
            var x = 0;
            var y = 0;
            TrailingMap = new Dictionary<Point, int>();
            foreach (var line in input.Split(Environment.NewLine))
            {
                x = 0;
                foreach (var c in line)
                {
                    TrailingMap.Add(new Point(x, y), int.Parse(c.ToString()));
                    x++;
                }
                y++;
            }

            PrintMap();
            MaxPoint = new Point(x-1, y-1);

            Console.WriteLine($"MaxPoint: {MaxPoint}");
        }

        public void PrintMap()
        {
            var maxX = TrailingMap.Keys.Max(x => x.X);
            var maxY = TrailingMap.Keys.Max(x => x.Y);
            Console.WriteLine(string.Join("",Enumerable.Repeat("-", maxX + 2)));

            for (var y = 0; y <= maxY; y++)
            {
                Console.Write("|");
                for (var x = 0; x <= maxX; x++)
                {
                    Console.Write(TrailingMap[new Point(x, y)]);
                }
                Console.WriteLine("|");
            }

            Console.WriteLine(string.Join("", Enumerable.Repeat("-", maxX + 2)));
        }
    }

}
