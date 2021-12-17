using Tools;

var template = Console.ReadLine()!.ToArray();

Console.ReadLine();

Dictionary<string, char> insertionRules = new();
while (Console.ReadLine() is string line)
{
    var parts = line.Split(" -> ");
    insertionRules.Add(parts[0].Trim(), parts[1].Trim()[0]);
}

for (int i = 0; i < 10; i++)
{
    template = PerformInsertion(template);
}

var groups = template.GroupBy(c => c).Select(g => g.Count()).OrderBy(c => c).ToArray();
var leastCommon = groups.First();
var mostCommon = groups.Last();

Console.WriteLine(mostCommon - leastCommon);

char[] PerformInsertion(char[] template)
{
    var output = new List<char>();
    output.Add(template[0]);
    for (int i = 1; i < template.Length; i++)
    {
        if (insertionRules.TryGetValue(template[i-1].ToString() + template[i], out var insertion))
        {
            output.Add(insertion);
        }
        output.Add(template[i]);
    }

    return output.ToArray();
}