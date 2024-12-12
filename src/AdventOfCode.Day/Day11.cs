using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day11 : AdventCalendarDay
    {
        protected override int Day => 11;

        private string _inputCache;

        override public void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            SolvePuzzleOne();
            SolvePuzzleTwo();
        }

        private void SolvePuzzleOne()
        {
            Blink(25);
        }

        private void Blink(int blinkAmount)
        {
            var input = GetInput();
            var stoneCounts = new Dictionary<long, long>();

            // Initialize stone counts
            foreach (var stone in input.Stones)
            {
                if (!stoneCounts.ContainsKey(stone))
                {
                    stoneCounts[stone] = 0;
                }
                stoneCounts[stone]++;
            }

            Console.WriteLine("Initial arrangement:");
            PrintStones(stoneCounts);

            for (int i = 0; i < blinkAmount; i++)
            {
                var newStoneCounts = new Dictionary<long, long>();

                foreach (var entry in stoneCounts)
                {
                    var stones = TransformStones(entry.Key);
                    foreach (var stone in stones)
                    {
                        if (!newStoneCounts.ContainsKey(stone))
                        {
                            newStoneCounts[stone] = 0;
                        }
                        newStoneCounts[stone] += entry.Value;
                    }
                }

                stoneCounts = newStoneCounts;

                Console.WriteLine($"After {i + 1} blink{(i == 0 ? "" : "s")}:");
                if (IsTestMode && blinkAmount <= 30)
                {
                    PrintStones(stoneCounts);
                }
            }

            Console.WriteLine($"After {blinkAmount} blinks, there are {stoneCounts.Values.Sum()} stones.");
            Thread.Sleep(3000);
        }

        private IEnumerable<long> TransformStones(long stone)
        {
            if (stone == 0)
            {
                return new[] { 1L };
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                var stoneStr = stone.ToString();
                var halfLength = stoneStr.Length / 2;
                var leftStone = long.Parse(stoneStr.Substring(0, halfLength));
                var rightStone = long.Parse(stoneStr.Substring(halfLength));
                return new[] { leftStone, rightStone };
            }
            else
            {
                return new[] { stone * 2024 };
            }
        }

        private void PrintStones(Dictionary<long, long> stoneCounts)
        {
            foreach (var entry in stoneCounts)
            {
                Console.WriteLine($"Stone {entry.Key}: {entry.Value} times");
            }
        }


        //private void Blink(int blinkAmount)
        //{
        //    var newStones = new List<long>();
        //    var input = GetInput();

        //    Console.WriteLine("Initial arrangement:");
        //    input.PrintStones();

        //    for (int i = 0; i < blinkAmount; i++)
        //    {
        //        newStones = new();
        //        foreach (var stone in input.Stones)
        //        {
        //            newStones.AddRange(TransformStones(stone));
        //        }
        //        input.Stones = newStones;

        //        Console.WriteLine($"After {i + 1} blink{(i == 0 ? "" : "s")}:");
        //        if (IsTestMode && blinkAmount <= 30)
        //        {
        //            input.PrintStones();
        //        }
        //    }

        //    Console.WriteLine($"After {blinkAmount} blinks, there are {input.Stones.Count()} stones.");

        //    Thread.Sleep(3000);
        //}

        //private IEnumerable<long> TransformStones(long stone)
        //{
        //    // Rules:
        //    // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
        //    if(stone == 0)
        //    {
        //        return [1];
        //    }
        //    // If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
        //    else if (stone.ToString().Length % 2 == 0)
        //    {
        //        var stoneStr = stone.ToString();
        //        var halfLength = stoneStr.Length / 2;
        //        var leftStone = long.Parse(stoneStr.Substring(0, halfLength));
        //        var rightStone = long.Parse(stoneStr.Substring(halfLength));
        //        return [leftStone, rightStone];
        //    }
        //    // If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
        //    else
        //    {
        //        return [stone * 2024];
        //    }
        //}

        private void SolvePuzzleTwo()
        {
            Blink(75);
        }

        protected override Day11Input GetInput()
        {
            if (_inputCache == null)
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new(_inputCache);
        }
    }
}