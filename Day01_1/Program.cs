int increases = 0;
var previous = int.MaxValue;
while (int.TryParse(await Console.In.ReadLineAsync(), out var current))
{
    if (current > previous)
    {
        increases++;
    }
    previous = current;
}
await Console.Out.WriteLineAsync(increases.ToString());