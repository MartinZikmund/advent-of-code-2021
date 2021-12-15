using Tools;

var input = InputTools.ReadAllLines();

var width = input[0].Length;
var height = input.Length;

int[,] map = new int[width, height];
(int length, Point from)[,] shortestPath = new (int, Point)[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = input[y][x] - '0';
        shortestPath[x, y] = (int.MaxValue, default);
    }
}

shortestPath[0, 0] = (map[0, 0], default);

var activeNodes = new HashSet<Point>();
activeNodes.Add(new Point(0, 0));

while(activeNodes.Count > 0)
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

Console.WriteLine(shortestPath[width - 1, height - 1].length - map[0,0]);