using Tools;

var lines = InputTools.ReadAllLines();

var map = new int[lines.First().Length, lines.Length];
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines.First().Length; x++)
    {
        map[x, y] = lines[y][x] - '0';
    }
}

var totalRisk = 0;

for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines.First().Length; x++)
    {
        var currentValue = map[x, y];
        var allHigher = true;
        foreach (var direction in Directions.WithoutDiagonals)
        {
            (int x, int y) neighbor = (x + direction.X, y + direction.Y);
            if (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < lines.First().Length && neighbor.y < lines.Length)
            {
                if (currentValue >= map[neighbor.x, neighbor.y])
                {
                    allHigher = false;
                    break;
                }
            }
        }

        if (allHigher)
        {
            totalRisk += currentValue + 1;
        }
    }
}

Console.WriteLine(totalRisk);
