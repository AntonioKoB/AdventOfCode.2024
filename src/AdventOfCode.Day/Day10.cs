using AdventOfCode.Common;
using AdventOfCode.Day.Structures;
using System.Drawing;

namespace AdventOfCode.Day
{
    public class Day10 : AdventCalendarDay
    {
        public class KvPPointIntComparer : IEqualityComparer<KeyValuePair<Point,int>>
        {
            public bool Equals(KeyValuePair<Point, int> x, KeyValuePair<Point, int> y)
            {
                // Define how to determine if x and y are equal
                return x.Key.Equals(y.Key) && x.Value == y.Value;
            }

            public int GetHashCode(KeyValuePair<Point, int> obj)
            {
                // Define how to calculate the hash code for obj
                return obj.Key.GetHashCode() ^ obj.Value.GetHashCode();
            }
        }

        private static class MovingPoints
        {
            internal static Point UP = new Point(0, -1);
            internal static Point DOWN = new Point(0, 1);
            internal static Point LEFT = new Point(-1, 0);
            internal static Point RIGHT = new Point(1, 0);

            public static Point? GetNextPoint(Point currentPoint, Point direction, Point maxPoint)
            {
                Point nextPoint = new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y);

                if (nextPoint.X < 0 || nextPoint.X > maxPoint.X || nextPoint.Y < 0 || nextPoint.Y > maxPoint.Y)
                {
                    return null;
                }

                return nextPoint;
            }
        }

        protected override int Day => 10;

        public string _inputCache;

        private List<Task> _tasks;



        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            SolvePuzzleOne();
            SolvePuzzleTwo();
        }

        private void SolvePuzzleOne()
        {
            var input = GetInput();
            var totalScore = 0;
            foreach (var line in input.TrailingMap)
            {
                if (line.Value == 0)
                {
                    //let's create a trail
                    totalScore += GetTrailScore(line, input);
                }
            }

            Console.WriteLine($"Total Score puzzle 1: {totalScore}");
        }

        private int GetTrailScore(KeyValuePair<Point, int> line, Day10Input input)
        {
            var nary = new NaryTree<KeyValuePair<Point, int>>(line);
            _tasks = new List<Task>();
            _tasks.Add(NavigateAndGenerateTree(nary.Root, input));
            do
            {
                Task.WhenAll(_tasks).Wait();
                _tasks.RemoveAll(t => t.IsCompleted);
                Thread.Sleep(100);
            } while (_tasks.Count > 0);

            if(IsTestMode)
                nary.PrintTree(nary.Root, x => Console.WriteLine($"{x.Key} - {x.Value}"));

            return nary.FindPathsToLeaf(x => x.Value == 9).Distinct(new KvPPointIntComparer()).Count();
        }

        private Task NavigateAndGenerateTree(NaryTreeNode<KeyValuePair<Point, int>> node, Day10Input input)
        {
            var currentPoint = node.Value.Key;
            if (node.Value.Value == 9)
            {
                return Task.CompletedTask; // we reach the leaf
            }

            //from this point we need every direction that is an increment of our current node value
            var upBranch = MovingPoints.GetNextPoint(currentPoint, MovingPoints.UP, input.MaxPoint);
            CheckAndGenerateNode(node, input, upBranch);

            var downBranch = MovingPoints.GetNextPoint(currentPoint, MovingPoints.DOWN, input.MaxPoint);
            CheckAndGenerateNode(node, input, downBranch);

            var leftBranch = MovingPoints.GetNextPoint(currentPoint, MovingPoints.LEFT, input.MaxPoint);
            CheckAndGenerateNode(node, input, leftBranch);

            var rightBranch = MovingPoints.GetNextPoint(currentPoint, MovingPoints.RIGHT, input.MaxPoint);
            CheckAndGenerateNode(node, input, rightBranch);

            return Task.CompletedTask;
        }

        private void CheckAndGenerateNode(NaryTreeNode<KeyValuePair<Point, int>> node, Day10Input input, Point? branchCandidate)
        {
            if (branchCandidate != null && input.TrailingMap[branchCandidate.Value] == node.Value.Value + 1)
            {
                var newDirectionNode = new NaryTreeNode<KeyValuePair<Point, int>>(
                    input.TrailingMap.First(p => p.Key == branchCandidate.Value));

                node.Children ??= new List<NaryTreeNode<KeyValuePair<Point, int>>>();

                node.Children.Add(newDirectionNode);
                _tasks.Add(NavigateAndGenerateTree(newDirectionNode, input));
            }
        }

        private void SolvePuzzleTwo()
        {
            var input = GetInput();
            var totalRating = 0;
            foreach (var line in input.TrailingMap)
            {
                if (line.Value == 0)
                {
                    //let's create a trail
                    totalRating += GetTrailRating(line, input);
                }
            }

            Console.WriteLine($"Total Rating puzzle 2: {totalRating}");
        }

        private int GetTrailRating(KeyValuePair<Point, int> line, Day10Input input)
        {
            var nary = new NaryTree<KeyValuePair<Point, int>>(line);
            _tasks = new List<Task>();
            _tasks.Add(NavigateAndGenerateTree(nary.Root, input));
            do
            {
                Task.WhenAll(_tasks).Wait();
                _tasks.RemoveAll(t => t.IsCompleted);
                Thread.Sleep(100);
            } while (_tasks.Count > 0);

            if (IsTestMode)
                nary.PrintTree(nary.Root, x => Console.WriteLine($"{x.Key} - {x.Value}"));

            var groupsOfLeafs = nary.FindPathsToLeaf(x => x.Value == 9).GroupBy(x => x.Value);
            var totalRate = 0;
            foreach (var group in groupsOfLeafs)
            {
                totalRate += group.Count();
            }

            return totalRate;
        }

        protected override Day10Input GetInput()
        {
            if(_inputCache == null)
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }

            return new(_inputCache);
        }
    }
}