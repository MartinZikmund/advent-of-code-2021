using Tools;

var input = InputTools.ReadAllLines();

var originalWidth = input[0].Length;
var width = input[0].Length * 5;
var originalHeight = input.Length;
var height = input.Length * 5;

int[,] map = new int[width, height];
(long length, Point from)[,] shortestPath = new (long, Point)[width, height];

for (int y = 0; y < originalHeight; y++)
{
    for (int x = 0; x < originalWidth; x++)
    {
        var value = input[y][x] - '0';
        for (int iy = 0; iy < 5; iy++)
        {
            for (int ix = 0; ix < 5; ix++)
            {
                var adjustedValue = value + iy + ix;
                if (adjustedValue > 9)
                {
                    adjustedValue -= 9;
                }
                map[x + (ix * originalWidth), y + (iy * originalHeight)] = adjustedValue;
                shortestPath[x + (ix * originalWidth), y + (iy * originalHeight)] = (long.MaxValue, default);
            }
        }
    }
}

shortestPath[0, 0] = (map[0, 0], default);

var activeNodes = new HashSet<Point>();
activeNodes.Add(new Point(0, 0));

while (activeNodes.Count > 0)
{
    var bestNode = activeNodes.First();
    foreach (var node in activeNodes)
    {
        if (shortestPath[node.X, node.Y].length < shortestPath[bestNode.X, bestNode.Y].length)
        {
            bestNode = node;
        }
    }

    var bestPoint = bestNode;
    activeNodes.Remove(bestNode);

    var bestLength = shortestPath[bestPoint.X, bestPoint.Y].length;
    foreach (var direction in Directions.WithoutDiagonals)
    {
        var neighbor = new Point(bestPoint.X + direction.X, bestPoint.Y + direction.Y);
        if (neighbor.X >= 0 && neighbor.Y >= 0 && neighbor.X < width && neighbor.Y < height)
        {
            if (shortestPath[neighbor.X, neighbor.Y].length > bestLength + map[neighbor.X, neighbor.Y])
            {
                shortestPath[neighbor.X, neighbor.Y] = (bestLength + map[neighbor.X, neighbor.Y], bestPoint);
                activeNodes.Add(neighbor);
            }
        }
    }
}

Console.WriteLine(shortestPath[width - 1, height - 1].length - map[0, 0]);