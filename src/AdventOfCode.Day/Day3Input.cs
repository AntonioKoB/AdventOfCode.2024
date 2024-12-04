using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day
{
    public class Day3Input : IDayInput
    {
        public IEnumerable<Tuple<int, int>> Muls { get; private set; }
        public IEnumerable<Tuple<int, int>> ActiveMuls { get; private set; }


        public Day3Input(string input1, string input2)
        {
            var pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
            var matches = Regex.Matches(input1,pattern);

            var tuples = new List<Tuple<int, int>>();

            foreach (Match match in matches)
            {
                if(match.Groups.Count == 3)
                    tuples.Add(
                        new Tuple<int, int>(
                            int.Parse(match.Groups[1].Value),
                            int.Parse(match.Groups[2].Value)));
            }

            Muls = tuples;


            // parse only active ones
            var isActive = true;
            var patternWithActivators = @"(mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\))";

            matches = Regex.Matches(input2, patternWithActivators);

            tuples = [];

            foreach (Match match in matches)
            {
                if (match.Value.StartsWith("don't()"))
                {
                    isActive = false;
                }
                else if (match.Value.StartsWith("do()"))
                {
                    isActive = true;
                }
                else if (isActive && match.Groups.Count == 4)
                    tuples.Add(
                        new Tuple<int, int>(
                            int.Parse(match.Groups[2].Value),
                            int.Parse(match.Groups[3].Value)));
            }

            ActiveMuls = tuples;
        }
    }
}
