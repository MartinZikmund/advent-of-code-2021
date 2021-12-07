var input = await Console.In.ReadLineAsync();
var positions = input!.Split(',').Select(int.Parse).OrderBy(i => i).ToArray();
var intervalStart = positions[positions.Length / 2];
var intervalEnd = positions[positions.Length / 2 + 1];
var bestCost = int.MaxValue;
var bestPosition = 0;
for (int i = intervalStart; i <= intervalEnd; i++)
{
    var cost = positions.Sum(p => Math.Abs(p - i));
    if (cost < bestCost)
    {
        bestCost = cost;
        bestPosition = i;
    }
}
await Console.Out.WriteLineAsync(bestCost.ToString());
