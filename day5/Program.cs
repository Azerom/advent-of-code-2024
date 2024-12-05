using System.Diagnostics;
using System.Text.RegularExpressions;

var pattern = @"(?>(?'before'\d+)\|(?'after'\d+))|(?'update'(?>(?'page'\d+),?)+)";

List<(int before, int after)> rules = [];
List<int[]> updates = [];

var sw = new Stopwatch();
sw.Start();

using (var sr = new StreamReader("input.txt"))
{
    var matches = Regex.Matches(await sr.ReadToEndAsync(), pattern);

    rules.AddRange(matches.Where(m => m.Groups["before"].Success)
                          .Select(m => (int.Parse(m.Groups["before"].Value), int.Parse(m.Groups["after"].Value))));
    updates.AddRange(matches.Where(m => m.Groups["update"].Success)
                          .Select(m => m.Groups["page"].Captures.Select(c => int.Parse(c.Value)).ToArray()));
}

sw.Stop();
Console.WriteLine($"Took {sw.ElapsedMilliseconds} ms to read input");

sw.Restart();

var validUpdatesMiddlePageSum = 0;
var invalidUpdatesMiddlePageSum = 0;

foreach(var update in updates)
{
    var errorCount = 0;
    bool validUpdate = true;
    foreach(var (before, after) in rules)
    {
        var beforeIndex = Array.FindIndex(update, p => p == before);
        var afterIndex = Array.FindIndex(update, p => p == after);
        if(beforeIndex != -1 && afterIndex != -1 && beforeIndex > afterIndex)
        {
            validUpdate = false;
            errorCount++;
            
        }
    }
    if(validUpdate){
        validUpdatesMiddlePageSum += update[update.Length / 2];

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" \u2714 ");
        Console.ResetColor();
        Console.WriteLine($"{string.Join(" ", update)}");
    }
    else{
        List<(int before, int after)> updateRules = update.SelectMany(p => rules.Where(r => r.before == p && update.Contains(r.after))).ToList();

        List<int> pages = [.. update];

        List<int> inOrder = [];

        while(pages.Count > 0)
        {
            if(!updateRules.Exists(r => r.after == pages[0])){
                inOrder.Add(pages[0]);
                updateRules.RemoveAll(r => r.before == pages[0]);
                pages.RemoveAt(0);
            }
            else{
                pages.Add(pages[0]);
                pages.RemoveAt(0);
            }
        }

        invalidUpdatesMiddlePageSum += inOrder[inOrder.Count / 2];
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("\u274C ");
        Console.ResetColor();
        for(int i = 0; i < inOrder.Count; i++){
            if(inOrder[i] != update[i])
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.Write($"{inOrder[i]} ");
            Console.ResetColor();
        } 
        Console.WriteLine();
    }
}

sw.Stop();
Console.WriteLine($"Took {sw.ElapsedMilliseconds} ms to compute");

Console.WriteLine($"Part 1 : {validUpdatesMiddlePageSum}");
Console.WriteLine($"Part 2 : {invalidUpdatesMiddlePageSum}");