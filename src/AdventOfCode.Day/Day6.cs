using AdventOfCode.Common;
using System.Diagnostics;

namespace AdventOfCode.Day
{
    public class Day6 : AdventCalendarDay
    {
        protected override int Day => 6;

        private List<Tuple<int, int>> _historyOfTravel;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            var input = GetInput();
            Console.Clear();

            SolvePuzzleOne(input);
            SolvePuzzleTwo();
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

        private void SolvePuzzleTwo()
        {
            var obstaclesCount = 0; //because we know the first run will not be a loop
            var currentObstacle = new Tuple<int, int>(-1, -1); // no obstacles
            var input = GetInput(); //refresh the input

            var iteration = 1;
            var currentRow = -1;
            var stopwatch = new Stopwatch();

            var timeProcessingLastRow = 0;

            do
            {
                Console.WriteLine($">>> ITERATION: {iteration++}");
                if (currentObstacle.Item1 >= 0 && currentObstacle.Item2 >= 0)
                {
                    if(currentObstacle.Item1 > currentRow)
                    {
                        currentRow = currentObstacle.Item1;
                        if(stopwatch.IsRunning)
                        {
                            stopwatch.Stop();
                            if(timeProcessingLastRow == 0)
                                timeProcessingLastRow = (int)stopwatch.ElapsedMilliseconds / 1000;
                            else
                                timeProcessingLastRow = (((int)stopwatch.ElapsedMilliseconds / 1000) + timeProcessingLastRow) / 2; //average
                            stopwatch.Reset();
                        }
                        stopwatch.Start();
                    }
                    Console.WriteLine($"Current Obstacle: [{currentObstacle.Item1}, {currentObstacle.Item2}]");
                }
                var isDebugPoint = false; // currentObstacle.Item1 == 59 && currentObstacle.Item2 == 70;
                var currentSleep =  isDebugPoint ? 1000 : 0;
                if (IsLooping(input, currentSleep))
                {
                    if (!IsTestMode)
                        Console.Clear();
                    Console.WriteLine($"Obstacle good.     Total Obstacles: {obstaclesCount}");
                    obstaclesCount++;
                }
                else
                {
                    if (!IsTestMode)
                        Console.Clear();
                    Console.WriteLine($"Obstacle NOT good. Total Obstacles: {obstaclesCount}");
                }

                Console.WriteLine(GetRemainingTime(input.Room[0].Length - currentRow, timeProcessingLastRow));

                currentObstacle = FindNextObstacle(input, currentObstacle);
                if (currentObstacle.Item1 >= 0 && currentObstacle.Item2 >= 0)
                {
                    input = GetInput(); //refresh the input
                    input.Room[currentObstacle.Item1][currentObstacle.Item2] = '#';
                }

            }
            while(currentObstacle.Item1 >=0 && currentObstacle.Item2 >=0); //we will get there eventually

            Console.WriteLine($"Total Obstacles: {obstaclesCount}");
            // save the result into a file
            File.WriteAllText("Day6Output2.txt", obstaclesCount.ToString());
        }

        private string GetRemainingTime(int length, int timeProcessingLastRow)
        {
            var progress = "Estimated remaining time: ";
            if (timeProcessingLastRow == 0)
                return progress + "[Calculating...]";

            var remainingTime = TimeSpan.FromSeconds(length * timeProcessingLastRow);
            return progress + remainingTime.ToString(@"hh\:mm\:ss") + " hh:mm:ss";
        }

        private Tuple<int, int> FindNextObstacle(Day6Input input, Tuple<int, int> currentObstacle)
        {
            if (currentObstacle.Item1 == -1 && currentObstacle.Item2 == -1)
                return new Tuple<int, int>(0, 0);

            var initLocation = GetInitialLocation(input, input.Room[0].Length, input.Room.Length);

            input.Room[initLocation.Item2][initLocation.Item1] = '^';

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

        private bool IsLooping(Day6Input input, int sleepAmount = 0)
        {
            var stepsDone = 0;

            var stepsToCalculateLoop = 10;

            _historyOfTravel = new List<Tuple<int, int>>();

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

            input.PrintRoom(IsTestMode, 50 + sleepAmount);

            do
            {
                _historyOfTravel.Add(currentLocation);
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

                input.PrintRoom(IsTestMode, 10 + sleepAmount);
                stepsDone++; // to avoid wasting time on the begining of the algorithm // Non test should at least do 50 steps as it is on X 70
                if (stepsDone >= stepsToCalculateLoop && IsLoopDetected(_historyOfTravel))
                    return true;

            } while (IsInBound(currentLocation));

            return false;
        }

        private bool IsLoopDetected(IEnumerable<Tuple<int, int>> history)
        {
            if (history.Count() <= 1)
            {
                return false;
            }
            if (history.Count() % 2 != 0)
            {
                return IsLoopDetected(history.Skip(1));
            }

            int mid = history.Count() / 2;
            for (int i = 0; i < mid; i++)
            {
                if (!history.ElementAt(i).Equals(history.ElementAt(i + mid)))
                {
                    return IsLoopDetected(history.Skip(2));
                }
            }
            return true;
        }

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



