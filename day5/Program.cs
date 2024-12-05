using System.Diagnostics;
using System.Text.RegularExpressions;
using day5;

var pattern = @"(?>(?'before'\d+)\|(?'after'\d+))|(?'update'(?>(?'page'\d+),?)+)";

List<Rule> rules = [];
List<Update> updates = [];

var sw = new Stopwatch();
sw.Start();

using (var sr = new StreamReader("input.txt"))
{
    var matches = Regex.Matches(await sr.ReadToEndAsync(), pattern);

    rules.AddRange(matches.Where(m => m.Groups["before"].Success)
                          .Select(m => new Rule(int.Parse(m.Groups["before"].Value), int.Parse(m.Groups["after"].Value))));
    updates.AddRange(matches.Where(m => m.Groups["update"].Success)
                          .Select(m => new Update(m.Groups["page"].Captures.Select(c => int.Parse(c.Value)))));
}

sw.Stop();
Console.WriteLine($"Took {sw.ElapsedMilliseconds} ms to read input");

sw.Restart();

var validUpdatesMiddlePageSum = 0;
var invalidUpdatesMiddlePageSum = 0;

foreach(var update in updates)
{

    if(update.IsCorrectOrder(rules))
    {
        validUpdatesMiddlePageSum += update.MiddlePage;

        Console.WriteLine($"{update.ToPrettyString()}");
    }
    else{
        Update correctedUpdate = update.CorrectOrder(rules);

        invalidUpdatesMiddlePageSum += correctedUpdate.MiddlePage;
        
        Console.WriteLine(correctedUpdate.ToPrettyString(update));
    }
}

sw.Stop();
Console.WriteLine($"Took {sw.ElapsedMilliseconds} ms to compute");

Console.WriteLine($"Part 1 : {validUpdatesMiddlePageSum}");
Console.WriteLine($"Part 2 : {invalidUpdatesMiddlePageSum}");
