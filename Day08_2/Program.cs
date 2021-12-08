long sum = 0;

var digitSegments = new string[]
{
    "abcefg",
    "cf",
    "acdeg",
    "acdfg",
    "bcdf",
    "abdfg",
    "abdefg",
    "acf",
    "abcdefg",
    "abcdfg"
};

var letters = "abcdefg";

while (await Console.In.ReadLineAsync() is { } line)
{
    var inputParts = line.Split('|');
    var digitList = inputParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToList();
    var outputList = inputParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToList();

    var number1 = digitList.Single(d => d.Length == 2);
    var number7 = digitList.Single(d => d.Length == 3);
    var number4 = digitList.Single(d => d.Length == 4);
    var number8 = digitList.Single(d => d.Length == 7);
    var number6 = digitList.Single(d => d.Length == 6 && d.Intersect(number1).Count() == 1);
    var number0 = digitList.Single(d => d.Length == 6 && d.Intersect(number4).Count() == 3 && !d.SequenceEqual(number6));

    var segmentA = number7.Except(number1).Single();
    var segmentC = number1.Except(number6).Single();
    var segmentF = number1.Except(new[] { segmentC }).Single();
    var segmentB = number4.Intersect(number0).Except(new[] { segmentC, segmentF }).Single();
    var segmentD = number4.Except(number0).Single();

    var number3 = digitList.Single(d => d.Length == 5 && d.Intersect(new[] { segmentC, segmentD, segmentF }).Count() == 3);

    var segmentG = number3.Except(number4).Except(number7).Single();
    var segmentE = number8.Except(number4).Except(number3).Single();

    var actualMapping = new char[] {
        segmentA,
        segmentB,
        segmentC,
        segmentD,
        segmentE,
        segmentF,
        segmentG
    };

    var outputDigits = "";
    foreach (var outputNumber in outputList)
    {
        var translated = new List<char>();
        for (int i = 0; i < outputNumber.Length; i++)
        {
            var segmentIndex = Array.IndexOf(actualMapping.ToArray(), outputNumber[i]);
            translated.Add(letters[segmentIndex]);
        }

        var translatedString = new string(translated.OrderBy(c => c).ToArray());

        outputDigits += Array.IndexOf(digitSegments, translatedString).ToString();
    }

    sum += int.Parse(outputDigits);
}

Console.Out.WriteLine(sum.ToString());