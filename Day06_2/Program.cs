long[] counters = new long[9];
var input = await Console.In.ReadLineAsync();
foreach (var latternfish in input!.Split(',').Select(int.Parse))
{
    counters[latternfish]++;
}

for (int i = 0; i < 256; i++)
{
    var zeroDays = counters[0];
    for (int days = 1; days < 9; days++)
    {
        counters[days - 1] = counters[days];
    }
    counters[8] = 0;

    counters[6] += zeroDays;
    counters[8] += zeroDays;
}

await Console.Out.WriteLineAsync(counters.Sum().ToString());