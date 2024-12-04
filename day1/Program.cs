using System.Text.RegularExpressions;

var pattern = @"(?<l1>\d*)   (?<l2>\d*)";

List<int> list1 = new();
List<int> list2 = new();

using (var sr = new StreamReader("input.txt"))
{
    var matches = Regex.Matches(await sr.ReadToEndAsync(), pattern);

    list1 = matches.SelectMany(n => n.Groups["l1"].Captures.Select(c => int.Parse(c.Value))).ToList();
    list2 = matches.SelectMany(n => n.Groups["l2"].Captures.Select(c => int.Parse(c.Value))).ToList();
}

list1 = list1.OrderBy(x => x).ToList();
list2 = list2.OrderBy(x => x).ToList();

List<int> diff = new();

for (int i = 0; i < list1.Count; i++){
    if(list1[i] >= list2[i]){
        diff.Add(list1[i] - list2[i]);
    }
    else{
        diff.Add(list2[i] - list1[i]);
    }
}

Console.WriteLine($"Part 1 : {diff.Sum()}");

//Part 2

var part2 = list1.Sum(id => list2.Count(l2 => l2 == id) * id);

Console.WriteLine($"Part 2 : {part2}");