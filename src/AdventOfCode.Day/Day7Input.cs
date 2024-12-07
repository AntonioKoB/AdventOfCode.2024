using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day7Dic
    {
        public long Key { get; set; }
        public List<long> Value { get; set; }


        public Day7Dic(long key, List<long> value)
        {
            Key = key;
            Value = value;
        }
    }
    public class Day7Input : IDayInput
    {
        public List<Day7Dic> CalibrationEquation { get; private set; }

        public Day7Input(string input)
        {
            CalibrationEquation = new();
            var rows = input.Trim().Replace("\r", "").Split('\n');
            foreach (var row in rows)
            {
                var splitArray = row.Trim().Split(":");

                var testOperation = splitArray[0].Trim();

                var equationItems =
                    splitArray[1]
                    .Trim()
                    .Split(" ")
                    .Select(x => long.Parse(x))
                    .ToList();

                CalibrationEquation.Add(new(long.Parse(testOperation), equationItems));
            }
        }
    }
}
