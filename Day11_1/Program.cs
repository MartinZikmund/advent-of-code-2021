using Tools;

const int Flashed = 100000;

var lines = InputTools.ReadAllLines();

var width = lines[0].Length;
var height = lines.Length;

var map = new int[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = lines[y][x] - '0';
    }
}

var totalFlashes = 0;
for (int i = 0; i < 100; i++)
{
    totalFlashes += PerformStep();
}

Console.WriteLine(totalFlashes);

int PerformStep()
{
    var flashCount = 0;
    IncreaseAllBy1();
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            flashCount += TryFlash(x, y);
        }
    }
    ResetToZero();
    return flashCount;
}

void IncreaseAllBy1()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            map![x, y]++;
        }
    }
}

void ResetToZero()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (map![x, y] >= Flashed)
            {
                map![x, y] = 0;
            }
        }
    }
}

int TryFlash(int x, int y)
{
    if (map[x, y] <= 9 || map[x, y] >= Flashed)
    {
        return 0;
    }

    var flashCount = 1;
    map[x, y] = Flashed;

    foreach (var direction in Directions.WithDiagonals)
    {
        var neighborX = x + direction.X;
        var neighborY = y + direction.Y;
        if (neighborX >= 0 && neighborY >= 0 && neighborX < width && neighborY < height)
        {
            map[neighborX, neighborY]++;
            flashCount += TryFlash(neighborX, neighborY);
        }
    }

    return flashCount;
}