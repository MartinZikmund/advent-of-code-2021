using Tools;

var lines = InputTools.ReadAllLines();

var dotPositions = (
    from line in lines
    where !line.StartsWith("fold") && line != ""
    let split = line.Split(',')
    select new Point(int.Parse(split[0]), int.Parse(split[1]))
).ToArray();

var folds = (
    from line in lines
    where line.StartsWith("fold")
    select line.Substring("fold along ".Length)
).ToArray();

var currentWidth = dotPositions.Max(p => p.X) + 1;
var currentHeight = dotPositions.Max(p => p.Y) + 1;

var map = new bool[currentWidth, currentHeight];

foreach (var position in dotPositions)
{
    map[position.X, position.Y] = true;
}

foreach (var fold in folds)
{
    PerformFold(fold);
}

var counter = 0;
for (int y = 0; y < currentHeight; y++)
{
    for (int x = 0; x < currentWidth; x++)
    {
        Console.Write(map[x, y] ? "X" : " ");
    }
    Console.WriteLine();

}

void PerformFold(string fold)
{
    var foldInstruction = fold.Split('=');
    var axis = foldInstruction[0][0];
    var distance = int.Parse(foldInstruction[1]);

    // Perform the fold
    var startX = axis == 'x' ? distance : 0;
    var startY = axis == 'y' ? distance : 0;

    for (int y = startY; y < currentHeight; y++)
    {
        for (int x = startX; x < currentWidth; x++)
        {
            if (axis == 'x')
            {
                map[distance - (x - distance), y] |= map[x, y];
            }
            else
            {
                map[x, distance - (y - distance)] |= map[x, y];
            }
        }
    }

    // Adjust actual size
    currentWidth = axis == 'x' ? distance : currentWidth;
    currentHeight = axis == 'y' ? distance : currentHeight;
}