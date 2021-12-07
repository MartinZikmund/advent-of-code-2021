var input = await Console.In.ReadLineAsync();
var positions = input!.Split(',').Select(int.Parse).ToArray();
var intervalStart = (int)positions.Average();
var intervalEnd = intervalStart + 1;
var bestCost = int.MaxValue;
var bestPosition = 0;
for (int i = intervalStart; i <= intervalEnd; i++)
{
    var cost = positions.Sum(p => ArithmeticProgressionSum(Math.Abs(p - i)));
    if (cost < bestCost)
    {
        bestCost = cost;
        bestPosition = i;
    }
}
await Console.Out.WriteLineAsync(bestCost.ToString());

int ArithmeticProgressionSum(int max)
{
    return max * (max + 1) / 2;
}
