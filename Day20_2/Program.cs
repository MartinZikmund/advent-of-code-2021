using System.Runtime;

var algorithm = Console.ReadLine();
Console.ReadLine();
var lines = new List<string>();
while (Console.ReadLine() is string line)
{
    lines.Add(line.Trim());
}

var padding = 500;
int steps = 50;

var width = lines[0].Length + padding * 2;
var height = lines.Count + padding * 2;
var map = new bool[width, height];
var scratchMap = new bool[width, height];
for (int y = 0; y < lines.Count; y++)
{
    for (int x = 0; x < lines[0].Length; x++)
    {
        map[x + padding, y + padding] = lines[y][x] == '#';
    }
}

for (int step = 0; step < steps; step++)
{
    Enhance();
}

var counter = 0;
for (int y = 0 + padding - steps; y < height - padding + steps; y++)
{
    for (int x = 0 + padding - steps; x < width - padding + steps; x++)
    {
        if (map[x, y])
        {
            Console.Write('#');
            counter++;
        }
        else
        {
            Console.Write('.');
        }
    }
    Console.WriteLine();
}

Console.WriteLine(counter);

void Enhance()
{
    Array.Clear(scratchMap);
    for (int y = 1; y < height - 1; y++)
    {
        for (int x = 1; x < width - 1; x++)
        {
            var number = Get9Number(x, y);
            scratchMap[x, y] = algorithm![number] == '#';
        }
    }
    (map, scratchMap) = (scratchMap, map);
}

int Get9Number(int x, int y)
{
    var topLeft = map[x - 1, y - 1] ? 1 : 0;
    var top = map[x, y - 1] ? 1 : 0;
    var topRight = map[x + 1, y - 1] ? 1 : 0;
    var left = map[x - 1, y] ? 1 : 0;
    var center = map[x, y] ? 1 : 0;
    var right = map[x + 1, y] ? 1 : 0;
    var bottomLeft = map[x - 1, y + 1] ? 1 : 0;
    var bottom = map[x, y + 1] ? 1 : 0;
    var bottomRight = map[x + 1, y + 1] ? 1 : 0;

    return topLeft << 8 | top << 7 | topRight << 6 | left << 5 | center << 4 | right << 3 | bottomLeft << 2 | bottom << 1 | bottomRight;
}