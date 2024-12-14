using AdventOfCode.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AdventOfCode.Day
{
    public class Day14Input : IDayInput
    {
        public IEnumerable<Robot> Robots { get; set; }

        const int realWidth = 101;
        const int realHeight = 103;
        const int testWidth = 11;
        const int testHeight = 7;

        public bool IsTestRun { get; }

        public Day14Input(string input, bool isTestRun)
        {
            IsTestRun = isTestRun;
            var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var robots = new List<Robot>();
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var positionPart = parts[0].Split("=")[1];
                var movementPart = parts[1].Split("=")[1];

                var initialPosition = new Point(
                    int.Parse(positionPart.Split(',')[0]),
                    int.Parse(positionPart.Split(',')[1]));

                var movement = new Size(int.Parse(movementPart.Split(',')[0]), int.Parse(movementPart.Split(',')[1]));

                var robot = new Robot(initialPosition, movement, isTestRun ? testWidth : realWidth, isTestRun ? testHeight : realHeight);
                robots.Add(robot);
            }

            Robots = robots;
        }

        public void PrintMap(bool removeMiddle = false)
        {
            for (int y = 0; y < (IsTestRun ? testHeight : realHeight); y++)
            {
                for (int x = 0; x < (IsTestRun ? testWidth : realWidth); x++)
                {
                    if(removeMiddle && (x == Robots.First().MapXMiddle || y == Robots.First().MapYMiddle))
                    {
                        Console.Write(" ");
                        continue;
                    }

                    var countRobotsInPosition = Robots.Count(r => r.CurrentPosition == new Point(x, y));
                    Console.Write(countRobotsInPosition == 0 ? "." : countRobotsInPosition.ToString());
                }

                Console.WriteLine();
            }
        }

        public string PrintMapImage(int second)
        {
            int width = IsTestRun ? testWidth : realWidth;
            int height = IsTestRun ? testHeight : realHeight;

            if (!Directory.Exists("Output"))
                Directory.CreateDirectory("Output");

            var filePath = $"Output/Map{second:D5}.jpg";

            using (var bitmap = new Bitmap(width * 10, height * 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            var countRobotsInPosition = Robots.Count(r => r.CurrentPosition == new Point(x, y));
                            string text = countRobotsInPosition == 0 ? "." : "●";
                            g.DrawString(text, SystemFonts.DefaultFont, Brushes.Black, x * 10, y * 10);
                        }
                    }
                }

                bitmap.Save(filePath, ImageFormat.Jpeg);
            }

            return $"Map saved to {filePath}";
        }


    }

    public class Robot {
        public Point InitialPosition { get; }
        public Point CurrentPosition { get; private set; }
        public Size Movement { get; }
        public int MapXSize { get; }
        public int MapYSize { get; }
        public int MapXMiddle { get; }
        public int MapYMiddle { get; }

        public Robot(Point initialPosition, Size movement, int x, int y)
        {
            InitialPosition = initialPosition;
            Movement = movement;
            CurrentPosition = initialPosition;
            MapXSize = x;
            MapYSize = y;
            MapXMiddle = x / 2;
            MapYMiddle = y / 2;
        }

        public void Move()
        {
            CurrentPosition = Point.Add(CurrentPosition, Movement);
            if (CurrentPosition.X < 0)
                CurrentPosition = Point.Add(CurrentPosition, new Size(MapXSize, 0));
            if (CurrentPosition.Y < 0)
                CurrentPosition = Point.Add(CurrentPosition, new Size(0, MapYSize));
            if (CurrentPosition.X >= MapXSize)
                CurrentPosition = Point.Subtract(CurrentPosition, new Size(MapXSize, 0));
            if (CurrentPosition.Y >= MapYSize)
                CurrentPosition = Point.Subtract(CurrentPosition, new Size(0, MapYSize));
        }

        public int GetQuadrant()
        {
            if (CurrentPosition.X < MapXMiddle && CurrentPosition.Y < MapYMiddle)
                return 0;
            if (CurrentPosition.X > MapXMiddle && CurrentPosition.Y < MapYMiddle)
                return 1;
            if (CurrentPosition.X < MapXMiddle && CurrentPosition.Y > MapYMiddle)
                return 2;
            if(CurrentPosition.X == MapXMiddle || CurrentPosition.Y == MapYMiddle)
                return -1; //to ignore
            return 3;
        }
    }


}
