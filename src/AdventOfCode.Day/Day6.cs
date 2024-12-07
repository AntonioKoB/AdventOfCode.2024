using AdventOfCode.Common;
using System;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Day
{
    public class Day6 : AdventCalendarDay
    {
        protected override int Day => 6;

        public int _obstaclesCount = 0;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            var input = GetInput();
            Console.Clear();

            SolvePuzzleOne(input);
            Task.Run(() => SolvePuzzleTwo()).Wait();
        }

        private void SolvePuzzleOne(Day6Input input)
        {
            var limitX = input.Room[0].Length;
            var limitY = input.Room.Length;

            var currentLocation = new Tuple<int, int>(0, 0);
            currentLocation = GetInitialLocation(input, limitX, limitY);

            //Commands
            var goUp = new Tuple<int, int>(-1, 0); //because first is row and second is col, so it will looc swapped
            var goDown = new Tuple<int, int>(1, 0);
            var goLeft = new Tuple<int, int>(0, -1);
            var goRight = new Tuple<int, int>(0, 1);

            var currentDirection = goUp;

            var countSteps = 0;

            input.PrintRoom(IsTestMode, 50);

            do
            {
                input.Room[currentLocation.Item2][currentLocation.Item1] = 'X';
                currentLocation = MoveOneStep(currentLocation, currentDirection, limitX, limitY);

                if (IsInBound(currentLocation))
                {
                    if (input.Room[currentLocation.Item2][currentLocation.Item1] == 'X')
                        countSteps--;

                    switch (currentDirection)
                    {
                        case var x when x == goUp:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = '^';
                            break;
                        case var x when x == goRight:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = '>';
                            break;
                        case var x when x == goDown:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = 'V';
                            break;
                        case var x when x == goLeft:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = '<';
                            break;
                    }
                }
                var nextStep = MoveOneStep(currentLocation, currentDirection, limitX, limitY);

                if (IsInBound(nextStep) && input.Room[nextStep.Item2][nextStep.Item1] == '#')
                {
                    switch (currentDirection)
                    {
                        case var x when x == goUp:
                            currentDirection = goRight;
                            break;
                        case var x when x == goRight:
                            currentDirection = goDown;
                            break;
                        case var x when x == goDown:
                            currentDirection = goLeft;
                            break;
                        case var x when x == goLeft:
                            currentDirection = goUp;
                            break;
                    }
                }
                countSteps++;

                input.PrintRoom(IsTestMode, 30);

            } while (IsInBound(currentLocation));

            Console.WriteLine($"Steps: {countSteps}");

            Thread.Sleep(3000);
            File.WriteAllText("Day6Output1.txt", countSteps.ToString());
        }

        private Tuple<int, int> _initialLocationCache = new Tuple<int, int>(-1, -1);

        private Tuple<int, int> GetInitialLocation(Day6Input input, int limitX, int limitY)
        {
            if(_initialLocationCache.Item1 != -1 && _initialLocationCache.Item2 != -1)
                return _initialLocationCache;
            for (int i = 0; i < limitX; i++)
            {
                for (int j = 0; j < limitY; j++)
                {
                    if (input.Room[i][j] == '^')
                    {
                        _initialLocationCache = new Tuple<int, int>(j, i);
                        return _initialLocationCache;
                    }
                }
            }

            return _initialLocationCache; //This should never happen
        }

        private bool IsInBound(Tuple<int, int> position)
        {
            return position.Item1 >= 0 && position.Item1 >= 0;
        }

        private static Tuple<int, int> MoveOneStep(Tuple<int, int> position, Tuple<int, int> currentDirection, int maxX, int maxY)
        {
            var nextPosition = new Tuple<int, int>(position.Item1 + currentDirection.Item2, position.Item2 + currentDirection.Item1);
            if (nextPosition.Item1 < 0 || nextPosition.Item2 < 0 || nextPosition.Item1 >= maxY || nextPosition.Item2 >= maxX)
                return new(-1,-1);

            return nextPosition;
        }

        private async Task SolvePuzzleTwo()
        {
            _obstaclesCount = 0; //because we know the first run will not be a loop
            var currentObstacle = new Tuple<int, int>(-1, -1); // no obstacles
            var input = GetInput(); //refresh the input

            var iteration = 1;

            var tasks = new List<Task>();

            var totalIterations = input.Room[0].Length * input.Room.Length; // 16900

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var maxParallelTasks = 20;//IsTestMode ? 1 : 15;

            PrintCalculationRemainingTime(stopWatch, totalIterations, iteration, 0);
            do
            {
                iteration++;

                tasks.Add(IsLooping(currentObstacle));

                while (tasks.Count >= maxParallelTasks)
                {
                    await Task.Delay(500);
                    tasks.RemoveAll(x => x.IsCompleted);
                    PrintCalculationRemainingTime(stopWatch, totalIterations, iteration, tasks.Count);
                }

                currentObstacle = FindNextObstacle(currentObstacle);

            }
            while(currentObstacle.Item1 >=0 && currentObstacle.Item2 >=0); //we will get there eventually

            await Task.WhenAll(tasks);

            Console.WriteLine($"Total Obstacles: {_obstaclesCount}");
            // save the result into a file
            File.WriteAllText("Day6Output2.txt", _obstaclesCount.ToString());
        }

        private void PrintCalculationRemainingTime(Stopwatch stopWatch, int totalIterations, int totalIterationsDone, int parallelProcessCount)
        {
            var remainingIterations = totalIterations - totalIterationsDone;
            var timeSpentSoFar = stopWatch.Elapsed.TotalSeconds;
            var percentageDone = (double)totalIterationsDone / totalIterations * 100;
            var estimatedRemainingTime =
                TimeSpan.FromSeconds(remainingIterations * timeSpentSoFar / totalIterationsDone);

            int barWidth = 50; // Width of the progress bar
            var buffer = new StringBuilder();

            buffer.Append("["); // Start of the progress bar

            int progressWidth = (int)(percentageDone * barWidth) / 100;
            for (int i = 0; i < barWidth; i++)
            {
                if (i < progressWidth)
                    buffer.Append("■");
                else
                    buffer.Append(" ");
            }

            buffer.Append($"] {percentageDone:F2}%"); // End of the progress bar

            Console.Clear(); // Clear the current console line

            Console.Write(buffer.ToString());
            Console.WriteLine(); // Move to the next line
            Console.WriteLine($"Estimated remaining time: {estimatedRemainingTime:hh\\:mm\\:ss}");
            Console.WriteLine($"FOUND SO FAR: {_obstaclesCount}"); // Print the message below the progress bar
            Console.WriteLine($"Currently processing {parallelProcessCount} parallel processes"); // Print the message below the progress bar
        }

        private Tuple<int, int> FindNextObstacle(Tuple<int, int> currentObstacle)
        {
            var input = GetInput();
            if (currentObstacle.Item1 == -1 && currentObstacle.Item2 == -1)
            {
                return new (0, 0);
            }
            var initLocation = GetInitialLocation(input, input.Room[0].Length, input.Room.Length);

            input.Room[initLocation.Item2][initLocation.Item1] = '^';
            input.Room[currentObstacle.Item1][currentObstacle.Item2] = '#';

            for (int i = currentObstacle.Item1; i < input.Room[0].Length; i++)
            {
                for (int j = currentObstacle.Item2; j < input.Room.Length; j++)
                {
                    if (input.Room[i][j] == '.' || input.Room[i][j] == 'X')
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
                currentObstacle = new Tuple<int, int>(currentObstacle.Item1, 0);
            }

            return new(-1, -1);
        }

        private async Task IsLooping(Tuple<int, int> currentObstacle)
        {
            var input = GetInput();
            if (currentObstacle.Item1 >= 0 && currentObstacle.Item2 >= 0)
                input.Room[currentObstacle.Item1][currentObstacle.Item2] = '#';

            await Task.Delay(100); // to force the thread to release, but not complete the task
            var stepsDone = 0;

            //var stepsToCalculateLoop = IsTestMode ? 10 : 100;
            var numberOfCells = input.Room[0].Length * input.Room.Length;
            var stepsToConsiderItALoop = numberOfCells + (numberOfCells / 2); // 25350

            var historyOfTravel = new List<Tuple<int, int>>();

            var limitX = input.Room[0].Length;
            var limitY = input.Room.Length;

            var currentLocation = GetInitialLocation(input, limitX, limitY);

            //Commands
            var goUp = new Tuple<int, int>(-1, 0); //because first is row and second is col, so it will looc swapped
            var goDown = new Tuple<int, int>(1, 0);
            var goLeft = new Tuple<int, int>(0, -1);
            var goRight = new Tuple<int, int>(0, 1);

            Tuple<int, int> nextStep;

            var currentDirection = goUp;

            do
            {
                nextStep = MoveOneStep(currentLocation, currentDirection, limitX, limitY);

                if (IsInBound(nextStep) && input.Room[nextStep.Item2][nextStep.Item1] == '#')
                {
                    switch (currentDirection)
                    {
                        case var x when x == goUp:
                            currentDirection = goRight;
                            break;
                        case var x when x == goRight:
                            currentDirection = goDown;
                            break;
                        case var x when x == goDown:
                            currentDirection = goLeft;
                            break;
                        case var x when x == goLeft:
                            currentDirection = goUp;
                            break;
                    }
                }
            } while (IsInBound(nextStep) && input.Room[nextStep.Item2][nextStep.Item1] == '#');

            input.PrintRoom(IsTestMode, 50);

            do
            {
                historyOfTravel.Add(currentLocation);
                input.Room[currentLocation.Item2][currentLocation.Item1] = 'X';
                currentLocation = MoveOneStep(currentLocation, currentDirection, limitX, limitY);

                if (IsInBound(currentLocation))
                {
                    switch (currentDirection)
                    {
                        case var x when x == goUp:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = '^';
                            break;
                        case var x when x == goRight:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = '>';
                            break;
                        case var x when x == goDown:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = 'V';
                            break;
                        case var x when x == goLeft:
                            input.Room[currentLocation.Item2][currentLocation.Item1] = '<';
                            break;
                    }
                }

                do
                {
                    nextStep = MoveOneStep(currentLocation, currentDirection, limitX, limitY);

                    if (IsInBound(nextStep) && input.Room[nextStep.Item2][nextStep.Item1] == '#')
                    {
                        switch (currentDirection)
                        {
                            case var x when x == goUp:
                                currentDirection = goRight;
                                break;
                            case var x when x == goRight:
                                currentDirection = goDown;
                                break;
                            case var x when x == goDown:
                                currentDirection = goLeft;
                                break;
                            case var x when x == goLeft:
                                currentDirection = goUp;
                                break;
                        }
                    }
                } while (IsInBound(nextStep) && input.Room[nextStep.Item2][nextStep.Item1] == '#');

                input.PrintRoom(IsTestMode, 50);
                stepsDone++;
                //if (stepsDone >= stepsToCalculateLoop &&
                //    IsLoopDetected((Tuple<int,int>[]) historyOfTravel.ToArray().Clone()))
                if(stepsDone >= stepsToConsiderItALoop)
                    _obstaclesCount++;

            } while (IsInBound(currentLocation) && stepsDone < stepsToConsiderItALoop);
        }

        private bool IsLoopDetected(Tuple<int, int>[] history)
        {
            var searcheableList = history.Reverse().ToArray();

            // Check if the list has fewer than 2 elements
            if (searcheableList.Length < 2)
                return false;

            var firstElement = searcheableList.First();
            var candidateOfRepeating = Array.IndexOf(searcheableList, firstElement, 1);

            if (candidateOfRepeating == -1)
                return false;

            // Check if candidateOfRepeating represents more than half of the list
            if (candidateOfRepeating > searcheableList.Length / 2)
                return false;

            candidateOfRepeating++; // because we skip the first

            for (var i = 0; i < candidateOfRepeating; i++)
            {
                if (!searcheableList[i].Equals(searcheableList[candidateOfRepeating + i]))
                    return false;
            }

            return true;
        }

        //private bool IsLoopDetected(Tuple<int, int>[] history)
        //if (history.Count() <= 1)
        //{
        //    return false;
        //}
        //if (history.Count() % 2 != 0)
        //{
        //    return IsLoopDetected(history.Skip(1).ToArray());
        //}

        //var list1 = history.Take(history.Count() / 2);
        //var list2 = history.Skip(history.Count() / 2);

        //if (list1.First().Equals(list2.First()) && list1.Last().Equals(list2.Last()))
        //    if (list1.SequenceEqual(list2))
        //        return true;

        //return IsLoopDetected(history.Skip(2).ToArray());



        //}

        private string? _inputCache;

        protected override Day6Input GetInput()
        {
            if (string.IsNullOrEmpty(_inputCache))
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new Day6Input(_inputCache);
        }
    }
}



