using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day9Input : IDayInput
    {
        public List<string> Disk { get; set; }

        public int EmptySpace { get; set; }

        public Day9Input()
        {
            Disk = new List<string>();
        }
        public Day9Input(string input)
        {
            Disk = new List<string>();
            bool isFile = true;

            var fileId = 0;

            foreach (var c in input)
            {
                var number = int.Parse(c.ToString());
                if (isFile)
                {
                    Disk.AddRange(Enumerable.Repeat(fileId.ToString(), number));
                    fileId++;
                }
                else
                {
                    Disk.AddRange(Enumerable.Repeat(".", number));
                }

                isFile = !isFile;
            }
            EmptySpace = Disk.Count(x => x == ".");
        }

        public void PrintDisk()
        {
            Console.WriteLine(string.Concat(Disk));
        }

        public long CalculateChecksum()
        {
            long checksum = 0;
            Disk.Select((item, index) => new { Item = item, Index = index })
                .ToList()
                .ForEach(sector =>
                {
                    var item = sector.Item.ToString();
                    var index = sector.Index;
                    if (item != ".")
                        checksum += index * long.Parse(item);
                });

            return checksum;
        }

        public void TrimEndSpaces()
        {
            EmptySpace -= Disk.TrimEnd(".");
        }
    }

}
