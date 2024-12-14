using AdventOfCode.Common;
using System.Numerics;

namespace AdventOfCode.Day
{
    public class Day13 : AdventCalendarDay
    {
        protected override int Day => 13;

        private string _inputCache;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            SolvePuzzleOne();
            SolvePuzzleTwo();
        }

        private void SolvePuzzleOne()
        {
            var input = GetInput();

            const int buttonAPriceInTokens = 3;
            const int buttonBPriceInTokens = 1;

            var totalTokensSpent = 0;

            var machineNumber = 0;

            foreach (var machine in input.Machines)
            {
                machineNumber++;
                Console.WriteLine($"Machine {machineNumber}");
                Console.WriteLine("============");
                var (buttonAPress, buttonBPress) = GetCheapestMovements(machine);
                if (buttonAPress < 0 || buttonBPress < 0)
                {
                    Console.WriteLine("Impossible. There is no possible prize in this machine.");
                    Console.WriteLine($"------------{Environment.NewLine}");
                    continue;
                }
                var machineTokensSpent = (buttonAPress * buttonAPriceInTokens) + (buttonBPress * buttonBPriceInTokens);

                Console.WriteLine($"Machine {machineNumber} spent {machineTokensSpent} tokens");
                Console.WriteLine($"------------{Environment.NewLine}");

                totalTokensSpent += machineTokensSpent;
            }

            Console.WriteLine($"Total tokens spent: {totalTokensSpent}");
        }

        private (int, int) GetCheapestMovements(Machine machine)
        {
            var minTokens = int.MaxValue; // Initialize with a very large number
            var bestButtonAPress = -1;
            var bestButtonBPress = -1;

            for (int aPress = 0; aPress <= 100; aPress++)
            {
                for (int bPress = 0; bPress <= 100; bPress++)
                {
                    var totalX = aPress * machine.ButtonA.X + bPress * machine.ButtonB.X;
                    var totalY = aPress * machine.ButtonA.Y + bPress * machine.ButtonB.Y;

                    if (totalX == machine.Prize.X && totalY == machine.Prize.Y)
                    {
                        var tokensSpent = aPress * 3 + bPress;
                        if (tokensSpent < minTokens)
                        {
                            minTokens = tokensSpent; // Update minTokens if a cheaper solution is found
                            bestButtonAPress = aPress;
                            bestButtonBPress = bPress;
                        }
                    }
                }
            }

            if (minTokens == int.MaxValue)
            {
                //Console.WriteLine("Impossible. There is no possible prize in this machine.");
                return (-1, -1);
            }

            Console.WriteLine($"Button A: {bestButtonAPress} Button B: {bestButtonBPress}");
            return (bestButtonAPress, bestButtonBPress);
        }

        private void SolvePuzzleTwo()
        {
            Console.WriteLine("------------============== Puzzle 2 ==============------------");

            var input = GetInput();

            const int buttonAPriceInTokens = 3;
            const int buttonBPriceInTokens = 1;

            var totalTokensSpent = 0L; // Use long type for total tokens

            var machineNumber = 0;

            foreach (var machine in input.MachinesP2)
            {
                machineNumber++;
                Console.WriteLine($"Machine {machineNumber}");
                Console.WriteLine("============");
                var (buttonAPress, buttonBPress) = GetCheapestMovementsLong(machine);
                if (buttonAPress < 0 || buttonBPress < 0)
                {
                    Console.WriteLine("Impossible. There is no possible prize in this machine.");
                    Console.WriteLine($"------------{Environment.NewLine}");
                    continue;
                }
                var machineTokensSpent = (buttonAPress * buttonAPriceInTokens) + (buttonBPress * buttonBPriceInTokens);

                Console.WriteLine($"Machine {machineNumber} spent {machineTokensSpent} tokens");
                Console.WriteLine($"------------{Environment.NewLine}");

                totalTokensSpent += machineTokensSpent;
            }

            Console.WriteLine($"Total tokens spent: {totalTokensSpent}");
        }

        private (long, long) GetCheapestMovementsLong(Machine machine)
        {
            var minTokens = long.MaxValue; // Initialize with a very large number
            var bestButtonAPress = -1L;
            var bestButtonBPress = -1L;

            long maxAPress = GetMaxPress(machine, machine.ButtonA);
            long maxBPress = GetMaxPress(machine, machine.ButtonB);

            var progress = 0L;
            BigInteger totalProgress = (BigInteger)maxAPress * (BigInteger)maxBPress;

            for (long aPress = 0; aPress <= maxAPress - 1; aPress++)
            {
                for (long bPress = 0; bPress <= maxBPress - 1; bPress++)
                {
                    progress++;
                    progress.PrintProgressBar(totalProgress);

                    var totalX = aPress * machine.ButtonA.X + bPress * machine.ButtonB.X;
                    var totalY = aPress * machine.ButtonA.Y + bPress * machine.ButtonB.Y;

                    if (totalX == machine.Prize.X && totalY == machine.Prize.Y)
                    {
                        var tokensSpent = aPress * 3 + bPress;
                        if (tokensSpent < minTokens)
                        {
                            minTokens = tokensSpent; // Update minTokens if a cheaper solution is found
                            bestButtonAPress = aPress;
                            bestButtonBPress = bPress;

                            // Early termination condition
                            if (tokensSpent == 0) return (aPress, bPress);
                        }
                    }
                }
            }

            Console.WriteLine($"Button A: {bestButtonAPress} Button B: {bestButtonBPress}");
            return (bestButtonAPress, bestButtonBPress);
        }

        private long GetMaxPress(Machine machine, LongPoint button)
        {
            long xValue = (long)Math.Ceiling((double)machine.Prize.X / button.X) + 1;
            long yValue = (long)Math.Ceiling((double)machine.Prize.Y / button.Y) + 1;

            return Math.Min(xValue, yValue);
        }


        protected override Day13Input GetInput()
        {
            if (_inputCache == null)
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new Day13Input(_inputCache);
        }
    }
}
