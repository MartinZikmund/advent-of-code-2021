using System;
using System.Drawing;

const string prefix = "target area: ";
var line = Console.ReadLine()!.Substring(prefix.Length);
var xy = line.Split(',');
var targetX = ExtractCoords(xy[0]);
var targetY = ExtractCoords(xy[1]);

var xMinTester = 0;
var minStartX = 0;
for (; xMinTester < targetX.from; minStartX++)
{
    xMinTester += minStartX;
}
minStartX--;

var bestStartY = 0;
for (int x = minStartX; x < targetX.to; x++)
{
    for (int y = bestStartY; y < Math.Abs(targetY.from); y++)
    {
        var maxHeight = TestTargetHit(x, y);
        if (maxHeight > bestStartY)
        {
            bestStartY = maxHeight;
        }
    }
}

Console.WriteLine(bestStartY);

int TestTargetHit(int vx, int vy)
{
    var topHeight = 0;
    Point current = new Point();
    Point speed = new Point(vx, vy);
    while (current.X <= targetX.to && current.Y >= targetY.to)
    {
        // Step
        current.X += speed.X;
        current.Y += speed.Y;
        speed.X = Math.Max(0, speed.X - 1);
        speed.Y -= 1;
        topHeight = Math.Max(topHeight, current.Y);
        if (current.X >= targetX.from && current.X <= targetX.to &&
            current.Y <= targetY.to && current.Y >= targetY.from)
        {
            // Hit
            return topHeight;
        }
    }
    return -1;
}

(int from, int to) ExtractCoords(string part)
{
    var trimmed = part.Trim().Substring(2);
    var fromTo = trimmed.Split("..");
    var from = int.Parse(fromTo[0]);
    var to = int.Parse(fromTo[1]);
    return (from, to);
}