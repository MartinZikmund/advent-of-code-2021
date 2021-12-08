var uniqueLengths = new int[]
{
    2, 3, 4, 7
};

var sum = 0;
while (Console.In.ReadLine() is { } line)
{
    var lineParts = line.Split('|');
    var parts = lineParts[1].Split(' ');
    sum += parts.Count(p => uniqueLengths.Contains(p.Trim().Length));
}

Console.Out.WriteLine(sum);