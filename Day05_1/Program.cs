var lines = new List<Line>();
while (await Line.ReadFromInputAsync() is Line line)
{
    lines.Add(line);
}

Dictionary<(int x, int y), int> intersections = new();
foreach (var line in lines.Where(l => l.IsHorizontalOrVertical))
{
    foreach (var point in line.EnumeratePoints())
    {
        if (!intersections.TryGetValue(point, out var value))
        {
            value = 0;
        }
        value++;
        intersections[point] = value;
    }
}

var result = intersections.Count(i => i.Value >= 2).ToString();
await Console.Out.WriteLineAsync(result);

public class Line
{
    public Line(int x1, int y1, int x2, int y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public int X1 { get; }

    public int Y1 { get; }

    public int X2 { get; }

    public int Y2 { get; }

    public bool IsHorizontalOrVertical => X1 == X2 || Y1 == Y2;

    public IEnumerable<(int x, int y)> EnumeratePoints()
    {
        if (IsHorizontalOrVertical)
        {
            (int x, int y) currentPoint = (Math.Min(X1, X2), Math.Min(Y1, Y2));
            (int x, int y) endPoint = (Math.Max(X1, X2), Math.Max(Y1, Y2));
            yield return currentPoint;
            while (currentPoint != endPoint)
            {
                if (endPoint.x == currentPoint.x)
                {
                    currentPoint.y++;
                }
                else
                {
                    currentPoint.x++;
                }
                yield return currentPoint;
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public static async Task<Line?> ReadFromInputAsync()
    {
        var line = await Console.In.ReadLineAsync();
        if (line == null)
        {
            return null;
        }

        var points = line!.Split("->");
        var startPoint = points[0].Split(',');
        var endPoint = points[1].Split(',');
        var x1 = int.Parse(startPoint[0]);
        var y1 = int.Parse(startPoint[1]);
        var x2 = int.Parse(endPoint[0]);
        var y2 = int.Parse(endPoint[1]);
        return new Line(x1, y1, x2, y2);
    }
}
