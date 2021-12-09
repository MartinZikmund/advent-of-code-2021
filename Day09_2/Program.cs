using System.Linq;
using System.Runtime.CompilerServices;

List<string> input = new List<string>();
var directions = new (int x, int y)[]
{
    (-1, 0), (1, 0), (0, -1), (0, 1)
};

while (Console.ReadLine() is string line)
{
    input.Add(line.Trim());
}

var width = input.First().Length;
var height = input.Count;
var map = new int[width, height];
var visited = new bool[width, height];
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = input[y][x] - '0';
    }
}

var basinSizes = new List<int>();
long totalSize = 1;
var totalRisk = 0;

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        if (!visited[x, y] && map[x,y] != 9)
        {
            var basinSize = Visit(x, y);
            if (basinSize != 0)
            {
                basinSizes.Add(basinSize);
            }
        }
    }
}


var largest = basinSizes.OrderByDescending(x => x).Take(3).ToArray();
Console.WriteLine(largest[0] * largest[1] * largest[2]);

int Visit(int x, int y)
{
    if (!visited[x, y] && map[x,y] != 9)
    {
        visited[x, y] = true;
        var counter = 1;
        foreach (var direction in directions)
        {
            (int x, int y) neighbor = (x + direction.x, y + direction.y);
            if (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < input.First().Length && neighbor.y < input.Count)
            {
                counter += Visit(neighbor.x, neighbor.y);
            }
        }

        return counter;
    }
    else
    {
        return 0;
    }
}