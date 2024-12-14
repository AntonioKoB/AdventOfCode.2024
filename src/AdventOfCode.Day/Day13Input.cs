using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day13Input : IDayInput
    {
        public List<Machine> Machines { get; private set; }
        public List<Machine> MachinesP2 { get; private set; }

        public Day13Input(string input)
        {
            Machines = new List<Machine>();
            var lines = input.Split(Environment.NewLine);

            int ax = 0, ay = 0, bx = 0, by = 0, px = 0, py = 0;

            var isLastIterationAMachineDone = false; // to consider scenarios where we have an empty line at the end of the file or not
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    isLastIterationAMachineDone = true;
                    Machines.Add(new Machine(ax, ay, bx, by, px, py));
                    continue;
                }

                isLastIterationAMachineDone = false;

                var parts = line.Split(':');

                if (parts[0].ToUpper().StartsWith("BUTTON"))
                {
                    int x = 0, y = 0;
                    var movement = parts[1].Trim().Split(',');
                    foreach (var coordenates in movement)
                    {
                        var coord = coordenates.Trim().Split('+');
                        if (coord[0].ToUpper() == "X")
                            x = int.Parse(coord[1]);
                        else
                            y = int.Parse(coord[1]);
                    }

                    if (parts[0].ToUpper().EndsWith("A"))
                    {
                        ax = x;
                        ay = y;
                    }
                    else
                    {
                        bx = x;
                        by = y;
                    }
                }
                else
                {
                    var movement = parts[1].Trim().Split(',');
                    foreach (var coordenates in movement)
                    {
                        var coord = coordenates.Trim().Split('=');
                        if (coord[0].ToUpper() == "X")
                            px = int.Parse(coord[1]);
                        else
                            py = int.Parse(coord[1]);
                    }
                }
            }

            if (!isLastIterationAMachineDone)
                Machines.Add(new Machine(ax, ay, bx, by, px, py));

            MachinesP2 = Machines.Select(m => m.CloneForPuzzle2()).ToList();
        }
    }

    public class Machine
    {
        public LongPoint ButtonA { get; private set; }
        public LongPoint ButtonB { get; private set; }
        public LongPoint Prize { get; private set; }
        public Machine(long ax, long ay, long bx, long by, long px, long py)
        {
            ButtonA = new LongPoint(ax, ay);
            ButtonB = new LongPoint(bx, by);
            Prize = new LongPoint(px, py);
        }

        public Machine CloneForPuzzle2()
        {
            return new Machine(ButtonA.X, ButtonA.Y, ButtonB.X, ButtonB.Y, Prize.X + 10000000000000, Prize.Y + 10000000000000);
        }
    }

    public struct LongPoint
    {
        public long X { get; set; }
        public long Y { get; set; }

        public LongPoint(long x, long y)
        {
            X = x;
            Y = y;
        }

        public static LongPoint Add(LongPoint point1, LongPoint point2)
        {
            return new LongPoint(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static LongPoint Subtract(LongPoint point1, LongPoint point2)
        {
            return new LongPoint(point1.X - point2.X, point1.Y - point2.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

}
