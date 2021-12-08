using System.Globalization;

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
    foreach (var permutation in GetPermutations(letters, letters.Length))
    {
        var permutationArray = permutation.ToArray();
        var translatedDigits = new List<string>();

        foreach (var digit in digitList)
        {
            var digitCopy = "";
            for (int i = 0; i < digit.Length; i++)
            {
                digitCopy += permutationArray[(int)(digit[i] - 'a')];
            }

            translatedDigits.Add(new string(digitCopy.OrderBy(c=>c).ToArray()));
        }

        bool isSolution = true;
        foreach (var templateDigit in digitSegments)
        {
            if (!translatedDigits.Contains(templateDigit))
            {
                isSolution = false;
                break;
            }
        }

        if (isSolution)
        {
            // Translate output digits
            var outputDigits = new List<string>();
            foreach (var digit in outputList)
            {
                var digitCopy = "";
                for (int i = 0; i < digit.Length; i++)
                {
                    digitCopy += permutationArray[(int)(digit[i] - 'a')];
                }

                outputDigits.Add(new string(digitCopy.OrderBy(c => c).ToArray()));
            }

            var currentValue = "";
            foreach (var output in outputDigits)
            {
                currentValue += Array.IndexOf(digitSegments, output).ToString();
            }
            sum += int.Parse(currentValue);
            break;
        }
    }
}

Console.Out.WriteLine(sum.ToString());

static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
{
    if (length == 1) return list.Select(t => new T[] { t });

    return GetPermutations(list, length - 1)
        .SelectMany(t => list.Where(e => !t.Contains(e)),
            (t1, t2) => t1.Concat(new T[] { t2 }));
}