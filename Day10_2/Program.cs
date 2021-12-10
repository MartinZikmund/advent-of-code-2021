using Tools;

Dictionary<char, int> scoring = new()
{
    { ')', 1 },
    { ']', 2 },
    { '}', 3 },
    { '>', 4 },
};

var lines = InputTools.ReadAllLines();

List<long> scores = new();

foreach (var line in lines)
{
    bool isCorrupted = false;
    var stack = new Stack<char>();
    for (int i = 0; i < line.Length && !isCorrupted; i++)
    {
        var character = line[i];
        bool isClosing = false;
        char expectedOpening = '?';
        switch (character)
        {
            case '(':
            case '[':
            case '{':
            case '<':
                stack.Push(character);
                break;
            case ')':
                expectedOpening = '(';
                isClosing = true;
                break;
            case ']':
                expectedOpening = '[';
                isClosing = true;
                break;
            case '}':
                expectedOpening = '{';
                isClosing = true;
                break;
            case '>':
                expectedOpening = '<';
                isClosing = true;
                break;
        }

        if (isClosing)
        {
            if (stack.Count == 0 || stack.Pop() != expectedOpening)
            {
                // Corrupted
                isCorrupted = true;
            }
        }
    }

    if (!isCorrupted)
    {
        if (stack.Count > 0)
        {
            // Incomplete
            var remaining = "";
            while (stack.Count > 0)
            {
                var opening = stack.Pop();
                switch (opening)
                {
                    case '(':
                        remaining += ')';
                        break;
                    case '[':
                        remaining += ']';
                        break;
                    case '{':
                        remaining += '}';
                        break;
                    case '<':
                        remaining += '>';
                        break;
                }
            }

            var score = 0l;
            for (int i = 0; i < remaining.Length; i++)
            {
                score *= 5;
                score += scoring[remaining[i]];
            }
            scores.Add(score);
        }
    }
}

Console.WriteLine(scores.OrderBy(s => s).ToArray()[scores.Count / 2]);