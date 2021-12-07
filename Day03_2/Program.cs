using System.Runtime.CompilerServices;

List<string> validMostCommon = new();
List<string> validLeastCommon = new();
(int zeroes, int ones)[]? counters = null;

while (await Console.In.ReadLineAsync() is { } line)
{
    validLeastCommon.Add(line);
    validMostCommon.Add(line);
}

var lineLength = validLeastCommon[0].Length;

for (int position = 0; position < lineLength; position++)
{
    var mostCommonCounts = GetCountsForPosition(position, validMostCommon);
    var leastCommonCounts = GetCountsForPosition(position, validLeastCommon);

    var mostCommonExpectedValue = mostCommonCounts.ones >= mostCommonCounts.zeroes ? '1' : '0';
    validMostCommon = validMostCommon.Where(i => i[position] == mostCommonExpectedValue).ToList();

    if (validLeastCommon.Count > 1)
    {
        var leastCommonExpectedValue = leastCommonCounts.zeroes <= leastCommonCounts.ones ? '0' : '1';
        validLeastCommon = validLeastCommon.Where(i => i[position] == leastCommonExpectedValue).ToList();
    }
}

var leastCommonNumber = Convert.ToInt32(validLeastCommon[0], 2);
var mostCommonNumber = Convert.ToInt32(validMostCommon[0], 2);
await Console.Out.WriteLineAsync((leastCommonNumber * mostCommonNumber).ToString());

(int zeroes, int ones) GetCountsForPosition(int position, List<string> input)
{
    (int zeroes, int ones) counter = (0, 0);
    foreach (var item in input)
    {
        if (item[position] == '0')
        {
            counter.zeroes++;
        }
        else
        {
            counter.ones++;
        }
    }
    return counter;
}