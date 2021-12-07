(int zeroes, int ones)[]? counters = null;

while (await Console.In.ReadLineAsync() is { } line)
{
    counters ??= new (int, int)[line.Length];
    for (int character = 0; character < line.Length; character++)
    {
        if (line[character] == '0')
        {
            counters[character].zeroes++;
        }
        else
        {
            counters[character].ones++;
        }
    }
}

string mostCommon = "";
string leastCommon = "";
foreach (var counter in counters!)
{
    if (counter.zeroes < counter.ones)
    {
        mostCommon += '1';
        leastCommon += '0';
    }
    else
    {
        mostCommon += '0';
        leastCommon += '1';
    }
}

var leastCommonNumber = Convert.ToInt32(leastCommon, 2);
var mostCommonNumber = Convert.ToInt32(mostCommon, 2);
await Console.Out.WriteLineAsync((leastCommonNumber * mostCommonNumber).ToString());