using Tools;

var lines = InputTools.ReadAllLines();

var totalScore = 0;

foreach (var line in lines)
{
    var stack = new Stack<char>();
    for (int i = 0; i < line.Length; i++)
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
                switch (character)
                {
                    case ')':
                        totalScore += 3;
                        break;
                    case ']':
                        totalScore += 57;
                        break;
                    case '}':
                        totalScore += 1197;
                        break;
                    case '>':
                        totalScore += 25137;
                        break;
                }
            }
        }
    }
}

Console.WriteLine(totalScore);