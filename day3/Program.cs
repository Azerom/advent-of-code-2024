using System.Text.RegularExpressions;

var part1Pattern = @"mul\((?'input1'\d+),(?'input2'\d+)\)";
var part2Pattern = @"(?>mul\((?'input1'\d+),(?'input2'\d+)\))|(?'do'do\(\))|(?'dont'don\'t\(\))";

int part1Sum = 0;

using (var sr = new StreamReader("input.txt"))
{
    var matches = Regex.Matches(await sr.ReadToEndAsync(), part1Pattern);

    bool doMul = true;

    part1Sum = matches.Sum(m => Mul(int.Parse(m.Groups["input1"].Value), int.Parse(m.Groups["input2"].Value)));
}

Console.WriteLine($"Part 1 : {part1Sum}");

int part2Sum = 0;

using (var sr = new StreamReader("input.txt"))
{
    var matches = Regex.Matches(await sr.ReadToEndAsync(), part2Pattern);

    bool doMul = true;

    part2Sum = matches.Sum(m =>
    {
        if(m.Groups["do"].Success){
            doMul = true;
        }
        else if(m.Groups["dont"].Success){
            doMul = false;
        }
        else if(doMul){
            return Mul(int.Parse(m.Groups["input1"].Value), int.Parse(m.Groups["input2"].Value));
        }
        return 0;
    });
}

Console.WriteLine($"Part 2 : {part2Sum}");

static int Mul(int input1, int input2){
    return input1 * input2;
}