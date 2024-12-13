using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day12 : AdventCalendarDay
    {
        protected override int Day => 12;

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
            var totalCost = CalculateTotalCost(input.GardenMap);
            Console.WriteLine($"Total cost P1: {totalCost}");
        }

        private static int CalculateTotalCost(char[][] grid)
        {
            int rows = grid.Length;
            int cols = grid[0].Length;
            bool[,] visited = new bool[rows, cols];
            int totalCost = 0;

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (!visited[r, c])
                    {
                        char plantType = grid[r][c];
                        int area = 0;
                        int perimeter = 0;
                        DFS1(grid, visited, r, c, plantType, ref area, ref perimeter, dx, dy);
                        totalCost += area * perimeter;
                    }
                }
            }

            return totalCost;
        }

        private static void DFS1(char[][] grid, bool[,] visited, int x, int y, char plantType, ref int area, ref int perimeter, int[] dx, int[] dy)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((x, y));

            while (stack.Count > 0)
            {
                var (cx, cy) = stack.Pop();
                if (visited[cx, cy])
                    continue;

                visited[cx, cy] = true;
                area++;
                int borders = 0;

                for (int i = 0; i < 4; i++)
                {
                    int nx = cx + dx[i];
                    int ny = cy + dy[i];

                    if (nx >= 0 && nx < grid.Length && ny >= 0 && ny < grid[0].Length)
                    {
                        if (grid[nx][ny] == plantType)
                        {
                            if (!visited[nx, ny])
                                stack.Push((nx, ny));
                        }
                        else
                        {
                            borders++;
                        }
                    }
                    else
                    {
                        borders++;
                    }
                }

                perimeter += borders;
            }
        }

        private void SolvePuzzleTwo()
        {
            var total = CalculateTotalFenceCost(GetInput().GardenMap);
            Console.WriteLine($"Total cost P2: {total}");
        }

        public static int CalculateTotalFenceCost(char[][] grid)
        {
            int rows = grid.Length;
            int cols = grid[0].Length;
            bool[,] visited = new bool[rows, cols];

            int totalCost = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!visited[i, j])
                    {
                        int area = 0;
                        int sideCount = 0;

                        Console.WriteLine($"Starting new region at ({i}, {j})");
                        DFS(grid, visited, i, j, grid[i][j], ref area, ref sideCount);

                        Console.WriteLine($"Region: Area = {area}, Side Count = {sideCount}, Price = {area * sideCount}");
                        totalCost += area * (sideCount-4);
                    }
                }
            }

            return totalCost;
        }

        private static void DFS(char[][] grid, bool[,] visited, int row, int col, char currentChar, ref int area, ref int sideCount)
        {
            if (row < 0 || row >= grid.Length || col < 0 || col >= grid[row].Length || visited[row, col] || grid[row][col] != currentChar)
            {
                return;
            }

            visited[row, col] = true;
            area++;

            // Count sides based on neighboring cells
            if (row > 0 && grid[row - 1][col] != currentChar) { sideCount++; }
            if (col > 0 && grid[row][col - 1] != currentChar) { sideCount++; }
            if (row < grid.Length - 1 && grid[row + 1][col] != currentChar) { sideCount++; }
            if (col < grid[row].Length - 1 && grid[row][col + 1] != currentChar) { sideCount++; }

            DFS(grid, visited, row - 1, col, currentChar, ref area, ref sideCount);
            DFS(grid, visited, row + 1, col, currentChar, ref area, ref sideCount);
            DFS(grid, visited, row, col - 1, currentChar, ref area, ref sideCount);
            DFS(grid, visited, row, col + 1, currentChar, ref area, ref sideCount);
        }

        protected override Day12Input GetInput()
        {
            if (_inputCache == null)
            {
                var fileName = GetInputFileName(1);
                _inputCache = File.ReadAllText(fileName);
            }
            return new Day12Input(_inputCache);
        }
    }
}
