using System.Runtime.CompilerServices;
using Tools;

var template = Console.ReadLine()!;

Console.ReadLine();

Dictionary<string, char> insertionRules = new();
while (Console.ReadLine() is string line)
{
    var parts = line.Split(" -> ");
    insertionRules.Add(parts[0].Trim(), parts[1].Trim()[0]);
}

Dictionary<string, long> counters = new();

for (int i = 1; i < template.Length; i++)
{
    var pair = template[i - 1].ToString() + template[i];
    IncreaseCounter(pair, 1);
}

void IncreaseCounter(string pair, long count)
{
    if (!counters.TryGetValue(pair, out var existingCount))
    {
        existingCount = 0;
    }
    counters[pair] = existingCount + count;
}

void DecreaseCounter(string pair, long count)
{
    counters[pair] = counters[pair] - count;
}

for (int i = 0; i < 40; i++)
{
    PerformInsertion();
}

long[] characterCounters = new long['Z' - 'A' + 1];
foreach (var kvp in counters)
{
    characterCounters[kvp.Key[0] - 'A'] += kvp.Value;
    characterCounters[kvp.Key[1] - 'A'] += kvp.Value;
}

// Each is counted twice except of boundaries
characterCounters[template[0] - 'A']++;
characterCounters[template[template.Length - 1] - 'A']++;

var mostCommon = characterCounters.Max() / 2;
var leastCommon = characterCounters.Where(c => c != 0).Min() / 2;

Console.WriteLine(mostCommon - leastCommon);

void PerformInsertion()
{
    List<(string pair, long count)> toAdd = new();
    List<(string pair, long count)> toRemove = new();
    foreach(var insertion in insertionRules)
    {
        if (counters.ContainsKey(insertion.Key))
        {
            toAdd.Add((insertion.Key[0].ToString() + insertion.Value, counters[insertion.Key]));
            toAdd.Add((insertion.Value.ToString() + insertion.Key[1], counters[insertion.Key]));
            toRemove.Add((insertion.Key, counters[insertion.Key]));
        }
    }

    foreach (var removal in toRemove)
    {
        DecreaseCounter(removal.pair, removal.count);
    }
    
    foreach (var addition in toAdd)
    {
        IncreaseCounter(addition.pair, addition.count);
    }
}