int increases = 0;
int[] window = new int[3];
window[0] = ReadNumber();
window[1] = ReadNumber();
window[2] = ReadNumber();
int windowEnd = 0;
var previousSum = window.Sum();

while (int.TryParse(await Console.In.ReadLineAsync(), out var currentNumber))
{
    var currentSum = previousSum - window[windowEnd] + currentNumber;
    if (currentSum > previousSum)
    {
        increases++;
    }
    previousSum = currentSum;
    window[windowEnd] = currentNumber;
    windowEnd = (windowEnd + 1) % window.Length;
}

await Console.Out.WriteLineAsync(increases.ToString());

int ReadNumber()
{
    return int.Parse(Console.In.ReadLine()!);
}

