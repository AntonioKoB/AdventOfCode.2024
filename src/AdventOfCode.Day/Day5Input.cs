using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day5Input : IDayInput
    {
        public List<Tuple<int, int>> Rules { get; private set; }
        public List<int[]> Updates { get; private set; }
        public Day5Input(string input)
        {
            Updates = new List<int[]>();
            Rules = new List<Tuple<int, int>>();

            var lines = input.Trim().Replace("\r", "").Split('\n');
            foreach (var line in lines)
            {
                if(line.Contains("|")) //it is a Rule
                {
                    var rules = line.Split('|');
                    Rules.Add(new Tuple<int, int>(int.Parse(rules[0]), int.Parse(rules[1])));
                }
                else if (line.Contains(",")) //it is an Update
                {
                    Updates.Add(Array.ConvertAll(line.Split(','), int.Parse));
                } //else it is an empty line, ignore it
            }
        }
    }
}
