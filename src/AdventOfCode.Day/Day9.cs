using AdventOfCode.Common;

namespace AdventOfCode.Day
{
    public class Day9 : AdventCalendarDay
    {
        protected override int Day => 9;

        private Day9Input _input;
        private string? _inputCache;

        public override void Run(bool isTestMode)
        {
            base.Run(isTestMode);
            _input = GetInput();
            SolvePuzzleOne();
            SolvePuzzleTwo();
        }

        private void SolvePuzzleOne()
        {
            Console.WriteLine("Solving puzzle one");
            if(IsTestMode)
                _input.PrintDisk();
            while (_input.Disk.Contains("."))
            {
                // replace last digit with '.' and bring it to the first occurence of '.'
                var lastDigitIndex = _input.Disk.LastIndexOf(_input.Disk.Last(x => !x.Equals(".")));
                var firstFreeSpaceIndex = _input.Disk.IndexOf(".");
                _input.Disk[firstFreeSpaceIndex] = _input.Disk[lastDigitIndex];
                _input.Disk[lastDigitIndex] = ".";
                _input.TrimEndSpaces();
                if (IsTestMode)
                {
                    Thread.Sleep(100);
                    _input.PrintDisk();
                }
                else
                {
                    if(_input.EmptySpace % 50 == 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Remaining Interpolated Empty Spaces: {_input.EmptySpace}");
                    }
                }
            }

            Console.WriteLine($"Disk Checksum: {_input.CalculateChecksum()}");
        }

        private void SolvePuzzleTwo()
        {
            Console.WriteLine("Solving puzzle 2");
            _input = GetInput();

            if(IsTestMode)
                _input.PrintDisk();

            // NOTE If this doesn't work, then just re-run it again (clear the record list) as we might need to re-run it as new spaces might have been discovered.

            var fileId = "";
            do
            {
                fileId = _input.Disk.Last(x => x != "." && (fileId == string.Empty || int.Parse(x) < int.Parse(fileId)));

                if(fileId != "0")
                {
                    // how many spaces does it uses?
                    var spaces = _input.Disk.Count(x => x == fileId);
                    //get the first index of contiguous spaces represented by '.'
                    var firstSpaceIndex = GetFirstContiguousSpaceIndex(spaces);

                    if (firstSpaceIndex > -1 && firstSpaceIndex < _input.Disk.IndexOf(fileId))
                    {

                        _input.Disk.ReplaceAll(fileId, ".");
                        _input.Disk.ReplaceAll(firstSpaceIndex, spaces, fileId);

                        if (IsTestMode)
                        {
                            _input.PrintDisk();
                            Thread.Sleep(100);
                        }
                        else if (int.Parse(fileId) % 10 == 0)
                        {
                            _input.TrimEndSpaces();
                            Console.Clear();
                            Console.WriteLine($"Current FileId: {fileId}");
                        }
                    }
                }
            }
            while (fileId != "0");

            Console.WriteLine($"Disk Checksum: {_input.CalculateChecksum()}");
        }

        public int GetFirstContiguousSpaceIndex(int spaceAmount)
        {
            int currentCount = 0;

            for (int i = 0; i < _input.Disk.Count; i++)
            {
                if (_input.Disk[i] == ".")
                {
                    currentCount++;
                    if (currentCount == spaceAmount)
                    {
                        return i - spaceAmount + 1;
                    }
                }
                else
                {
                    currentCount = 0;
                }
            }

            return -1; // Return -1 if no such sequence is found
        }


        protected override Day9Input GetInput()
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