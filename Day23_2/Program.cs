using System.Diagnostics;
using System.Text;
using Tools;

var costs = new Dictionary<char, long>()
{
    {'A', 1 },
    {'B', 10 },
    {'C', 100 },
    {'D', 1000 },
};

var input = InputTools.ReadAllLines();

var width = input[0].Length;
var height = input.Length;

var map = new Map(width, height);

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = x < input[y].Length ? input[y][x] : ' ';
    }
}

var cheapestPaths = new Dictionary<string, long>();
var cheapestCostToSolution = new Dictionary<string, long>();

var solution = "...........ABCDABCDABCDABCD";
var bestSolution = long.MaxValue;

FindCheapestSolution(map, 0, 0);

Console.WriteLine(bestSolution);

string ExtractHashcode(Map currentState)
{
    var stringBuilder = new StringBuilder();
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (currentState[x, y] == '.' || char.IsLetter(currentState[x, y]))
            {
                stringBuilder.Append(currentState[x, y]);
            }
        }
    }
    return stringBuilder.ToString();
}

long FindCheapestSolution(Map currentState, long currentCost, int depth)
{
    var hashcode = ExtractHashcode(currentState);
    if (cheapestPaths!.TryGetValue(hashcode, out var bestCost))
    {
        if (currentCost < bestCost)
        {
            cheapestPaths[hashcode] = currentCost;
        }
    }

    if (hashcode == solution && currentCost < bestSolution)
    {
        bestSolution = currentCost;
    }

    if (hashcode == solution)
    {
        return currentCost;
    }

    if (cheapestCostToSolution.TryGetValue(hashcode, out var costToSolution))
    {
        if (costToSolution == long.MaxValue)
        {
            // No solution
            return long.MaxValue;
        }

        if (costToSolution + currentCost < bestSolution)
        {
            bestSolution = costToSolution + currentCost;
        }
        return costToSolution + currentCost;
    }

    long bestCostToSolution = long.MaxValue;

    for (int y = 1; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (currentState[x, y] >= 'A' && currentState[x, y] <= 'D')
            {
                var letter = currentState[x, y];
                // Try all possible moves
                if (y == 1)
                {
                    // In the hallway, must move to room
                    var targetRoomIndex = currentState[x, y] - 'A';
                    var targetX = 3 + 2 * targetRoomIndex;
                    var targetY = 2;

                    // Target positions must be empty
                    bool roomClean = true;
                    for (int roomContentY = 2; roomContentY < 6; roomContentY++)
                    {
                        if (currentState[targetX, roomContentY] != letter &&
                            currentState[targetX, roomContentY] != '.')
                        {
                            // There is a foreign letter in the room.
                            roomClean = false;
                        }
                    }

                    if (!roomClean)
                    {
                        continue;
                    }

                    if (currentState[targetX, targetY] != '.')
                    {
                        continue;
                    }

                    if (CanMove(currentState, new Point(x, y), new Point(targetX, targetY)))
                    {
                        // If possible to move to lower level, do so
                        while (currentState[targetX, targetY + 1] == '.')
                        {
                            targetY++;
                        }

                        var cost = GetPathCost(letter, new Point(x, y), new Point(targetX, targetY));
                        currentState[x, y] = '.';
                        currentState[targetX, targetY] = letter;
                        var result = FindCheapestSolution(currentState, currentCost + cost, depth + 1);
                        if (result < bestCostToSolution)
                        {
                            bestCostToSolution = result;
                        }
                        currentState[targetX, targetY] = '.';
                        currentState[x, y] = letter;
                    }
                }
                else
                {
                    var targetRoomIndex = currentState[x, y] - 'A';
                    var desiredX = 3 + 2 * targetRoomIndex;

                    if (desiredX == x)
                    {
                        bool roomClean = true;
                        for (int belowY = y; belowY < height - 1; belowY++)
                        {
                            var belowState = currentState[x, belowY];
                            if (belowState != letter)
                            {
                                roomClean = false;
                            }
                        }
                        if (roomClean)
                        {
                            continue;
                        }
                    }

                    for (int targetX = 1; targetX < width - 1; targetX++)
                    {
                        // Cannot stop above room
                        if (currentState[targetX, 2] != '#')
                        {
                            continue;
                        }

                        if (CanMove(currentState, new Point(x, y), new Point(targetX, 1)))
                        {
                            var cost = GetPathCost(letter, new Point(x, y), new Point(targetX, 1));
                            currentState[x, y] = '.';
                            currentState[targetX, 1] = letter;
                            var result = FindCheapestSolution(currentState, currentCost + cost, depth + 1);
                            if (result < bestCostToSolution)
                            {
                                bestCostToSolution = result;
                            }
                            currentState[targetX, 1] = '.';
                            currentState[x, y] = letter;
                        }
                    }
                }
            }
        }
    }

    if (bestCostToSolution == long.MaxValue)
    {
        cheapestCostToSolution[hashcode] = long.MaxValue;
    }
    else
    {
        var costDiff = bestCostToSolution - currentCost;
        cheapestCostToSolution[hashcode] = costDiff;
    }
    return bestCostToSolution;
}

long GetPathCost(char letter, Point from, Point to)
{
    var diffX = Math.Abs(to.X - from.X);
    var diffY = Math.Abs(to.Y - from.Y);
    return costs[letter] * (diffX + diffY);
}

bool CanMove(Map currentState, Point from, Point to)
{
    var currentPosition = from;
    var directionX = to.X - from.X;
    if (directionX != 0)
    {
        directionX /= Math.Abs(directionX);
    }
    var directionY = to.Y - from.Y;
    if (directionY != 0)
    {
        directionY /= Math.Abs(directionY);
    }

    var moved = true;
    while (moved)
    {
        if (currentPosition == to)
        {
            return true;
        }

        if (currentPosition.X != to.X && currentState[currentPosition.X + directionX, currentPosition.Y] == '.')
        {
            currentPosition.X += directionX;
            moved = true;
            continue;
        }

        if (currentPosition.Y != to.Y && currentState[currentPosition.X, currentPosition.Y + directionY] == '.')
        {
            currentPosition.Y += directionY;
            moved = true;
            continue;
        }

        moved = false;
    }
    return false;
}

public class Map
{
    private readonly char[,] _map;
    private readonly int _width;
    private readonly int _height;

    public Map(int width, int height)
    {
        _map = new char[width, height];
        _width = width;
        _height = height;
    }

    public char this[int x, int y]
    {
        get => _map[x, y];
        set => _map[x, y] = value;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                stringBuilder.Append(this[x, y]);
            }
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}