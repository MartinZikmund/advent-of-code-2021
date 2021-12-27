using System.Diagnostics;
using System.Runtime.InteropServices;
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
var map = new char[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = x < input[y].Length ? input[y][x] : ' ';
    }
}

var cheapestPaths = new Dictionary<string, long>();
var cheapestCostToSolution = new Dictionary<string, long>();

var solution = "...........ABCDABCD";
var bestSolution = long.MaxValue;

FindCheapestSolution(map, 0, 0);

Console.WriteLine(bestSolution);

string ExtractHashcode(char[,] currentState)
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

long FindCheapestSolution(char[,] currentState, long currentCost, int depth)
{
    var hashcode = ExtractHashcode(currentState);
    if (cheapestPaths!.TryGetValue(hashcode, out var bestCost))
    {
        if (bestCost < currentCost)
        {
            return long.MaxValue;
        }
    }
    cheapestPaths[hashcode] = currentCost;
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

                    // Target position must be empty
                    if (currentState[targetX, targetY] != '.' ||
                        !(currentState[targetX, targetY + 1] == '.' || currentState[targetX, targetY + 1] == letter))
                    {
                        // Can't move
                        continue;
                    }

                    if (CanMove(currentState, new Point(x, y), new Point(targetX, targetY)))
                    {
                        // If possible to move to lower level, do so
                        if (currentState[targetX, targetY + 1] == '.')
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
                    if (desiredX == x &&
                        ((y == 2 && currentState[x, 3] == letter) ||
                         (y == 3)))
                    {
                        // No reason to move this letter anymore
                        continue;
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

void OutputMap(char[,] currentState)
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            Debug.Write(currentState[x, y]);
        }
        Debug.WriteLine("");
    }
}

long GetPathCost(char letter, Point from, Point to)
{
    var diffX = Math.Abs(to.X - from.X);
    var diffY = Math.Abs(to.Y - from.Y);
    return costs[letter] * (diffX + diffY);
}

bool CanMove(char[,] currentState, Point from, Point to)
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