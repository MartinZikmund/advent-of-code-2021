using Tools;

var lines = InputTools.ReadAllLines();
var width = lines[0].Trim().Length;
var height = lines.Length;

var map = new char[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = lines[y][x];
    }
}

var steps = 0;
while (Step())
{
    steps++;
}

Console.WriteLine(++steps);

bool Step()
{
    bool movement = false;
    for (int y = 0; y < height; y++)
    {
        var minX = 0;
        for (int x = width - 1; x >= minX; x--)
        {
            if (map![x, y] == '>')
            {
                var targetX = x + 1;
                if (targetX >= width)
                {
                    targetX = 0;
                }
                if (map[targetX, y] == '.')
                {
                    map[targetX, y] = '>';
                    map[x, y] = '.';
                    x--;
                    if (targetX == 0)
                    {
                        minX = 1;
                    }
                    movement = true;
                }
            }
        }
    }

    for (int x = 0; x < width; x++)
    {
        var minY = 0;
        for (int y = height - 1; y >= minY; y--)
        {
            if (map![x, y] == 'v')
            {
                var targetY = y + 1;
                if (targetY >= height)
                {
                    targetY = 0;
                }
                if (map[x, targetY] == '.')
                {
                    map[x, targetY] = 'v';
                    map[x, y] = '.';
                    y--;
                    if (targetY == 0)
                    {
                        minY = 1;
                    }
                    movement = true;
                }
            }
        }
    }

    return movement;
}