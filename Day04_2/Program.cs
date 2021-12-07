var drawnNumbers = (await Console.In.ReadLineAsync())!.Split(',');

var bingoCards = new List<BingoCard>();
while (await Console.In.ReadLineAsync() is { } line)
{
    bingoCards.Add(await BingoCard.ReadFromInputAsync());
}

BingoCard lastWon = null;
int lastWonNumber = 0;

foreach (var draw in drawnNumbers)
{
    var drawnNumber = int.Parse(draw);
    foreach (var card in bingoCards.Where(c => !c.Won))
    {
        if (card.Mark(drawnNumber))
        {
            lastWonNumber = drawnNumber;
            lastWon = card;            
        }
    }
}

var result = lastWonNumber * lastWon!.SumUnmarked();
await Console.Out.WriteLineAsync(result.ToString());

public class BingoCard
{
    private int[,] _numbers = new int[5, 5];
    private bool[,] _marked = new bool[5, 5];

    private BingoCard()
    {
    }

    public bool Won { get; private set; }

    public static async Task<BingoCard> ReadFromInputAsync()
    {
        var bingoCard = new BingoCard();
        for (int y = 0; y < 5; y++)
        {
            var line = (await Console.In.ReadLineAsync())!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < 5; x++)
            {
                bingoCard._numbers[x, y] = int.Parse(line![x]);
            }
        }

        return bingoCard;
    }

    public int SumUnmarked()
    {
        var sum = 0;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (!_marked[x, y])
                {
                    sum += _numbers[x, y];
                }
            }
        }
        return sum;
    }

    public bool Mark(int number)
    {
        bool isWinning = false;
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (_numbers[x, y] == number && !_marked[x, y])
                {
                    _marked[x, y] = true;

                    if (IsRowWinning(y) || IsColumnWinning(x))
                    {
                        Won = true;
                        isWinning = true;
                    }
                }
            }
        }

        return isWinning;
    }

    private bool IsRowWinning(int y)
    {
        for (int x = 0; x < 5; x++)
        {
            if (!_marked[x, y])
            {
                return false;
            }
        }

        return true;
    }

    private bool IsColumnWinning(int x)
    {
        for (int y = 0; y < 5; y++)
        {
            if (!_marked[x, y])
            {
                return false;
            }
        }

        return true;
    }
}

