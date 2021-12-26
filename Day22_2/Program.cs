var instructions = new List<Instruction>();
while (Instruction.Read() is Instruction instruction)
{
    instructions.Add(instruction);
}

var cube = new bool[101, 101, 101];

var rootNode = new Node((0, 0));

foreach (var instruction in instructions)
{
    ProcessInstruction(instruction);
}

var totalVolume = CalculateTotalVolume(rootNode, 1);
Console.WriteLine(totalVolume);

ulong CalculateTotalVolume(Node node, ulong currentMultiple)
{
    ulong sum = 0;

    currentMultiple *= (ulong)(node.Range.To - node.Range.From + 1);

    if (node.IsLeaf)
    {
        return node.IsOn ? currentMultiple : 0;
    }

    foreach (var child in node.Children)
    {
        sum += CalculateTotalVolume(child, currentMultiple);
    }

    return sum;
}

void ProcessInstruction(Instruction instruction)
{
    rootNode.InsertRanges(new[] { instruction.XRange, instruction.YRange, instruction.ZRange }, 0, instruction.TurnOn);
}

bool CheckOn(int x, int y, int z)
{
    var xNode = rootNode.Children.FirstOrDefault(c => c.Range.From <= x && x <= c.Range.To);
    var yNode = xNode?.Children.FirstOrDefault(c => c.Range.From <= y && y <= c.Range.To);
    var zNode = yNode?.Children.FirstOrDefault(c => c.Range.From <= z && z <= c.Range.To);
    return zNode?.IsOn ?? false;
}

public class Node
{
    public Node((int From, int To) range)
    {
        Range = range;
    }

    public override string ToString()
    {
        return $"<{Range.From},{Range.To}>";
    }

    public bool IsOn { get; set; }

    public bool IsLeaf => Children.Count == 0;

    public (int From, int To) Range { get; set; }

    public List<Node> Children { get; private set; } = new List<Node>();

    public Node Clone((int From, int To) newRange)
    {
        var copy = new Node(newRange)
        {
            IsOn = IsOn
        };
        foreach (var child in Children)
        {
            copy.Children.Add(child.Clone(child.Range));
        }
        return copy;
    }

    public void InsertRanges((int From, int To)[] ranges, int depth, bool isOn)
    {
        if (depth == 3)
        {
            IsOn = isOn;
            return;
        }

        var currentRange = ranges[depth];
        var lastCovered = currentRange.To + 1;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            if (lastCovered <= currentRange.From)
            {
                // Fully covered
                break;
            }

            var currentChild = Children[i];

            //Empty space to be covered to the right
            if (lastCovered > currentChild.Range.To + 1)
            {
                // Add node to the right
                var rightNode = new Node((Math.Max(currentRange.From, currentChild.Range.To + 1), lastCovered - 1)) { IsOn = isOn };
                Children.Insert(i + 1, rightNode);
                rightNode.InsertRanges(ranges, depth + 1, isOn);
            }

            if (currentChild.Range.To < currentRange.From)
            {
                // The full range is already covered on the right.
                return;
            }

            if (currentChild.Range.From > currentRange.To)
            {
                // Range starts before current child
                lastCovered = Math.Min(Children[i].Range.From, currentRange.To + 1);
                continue;
            }

            // Range may overlap with current node
            if (currentRange.From <= currentChild.Range.From && currentChild.Range.To <= currentRange.To)
            {
                // Fully covered by the range ->  just nest
                currentChild.InsertRanges(ranges, depth + 1, isOn);
            }
            else
            {
                // Partial overlap, need to split
                // Right split
                if (currentRange.To < currentChild.Range.To)
                {
                    var rightSplit = currentChild.Clone((currentRange.To + 1, currentChild.Range.To));
                    Children.Insert(i + 1, rightSplit);
                }
                // Left split
                if (currentChild.Range.From < currentRange.From)
                {
                    var leftSplit = currentChild.Clone((currentChild.Range.From, currentRange.From - 1));
                    Children.Insert(i, leftSplit);
                }

                currentChild.Range = (Math.Max(currentChild.Range.From, currentRange.From), Math.Min(currentChild.Range.To, currentRange.To));
                currentChild.InsertRanges(ranges, depth + 1, isOn);
            }

            lastCovered = Math.Min(Children[i].Range.From, currentRange.To + 1);
        }

        // Potential leftmost child
        if (currentRange.From < lastCovered)
        {
            var leftmostNode = new Node((currentRange.From, Math.Min(lastCovered - 1, currentRange.To))) { IsOn = isOn };
            Children.Insert(0, leftmostNode);
            leftmostNode.InsertRanges(ranges, depth + 1, isOn);
        }
    }
}

public class Instruction
{
    public static Instruction? Read()
    {
        (int from, int to) GetRange(string coordPart)
        {
            var fromTo = coordPart.Substring(2).Split("..");
            return (int.Parse(fromTo[0]), int.Parse(fromTo[1]));
        }
        if (Console.ReadLine() is not { } line)
        {
            return null;
        }

        var parts = line.Split(' ');
        var on = parts[0] == "on";
        var coordParts = parts[1].Split(',');
        var xRange = GetRange(coordParts[0]);
        var yRange = GetRange(coordParts[1]);
        var zRange = GetRange(coordParts[2]);
        return new Instruction()
        {
            TurnOn = on,
            XRange = xRange,
            YRange = yRange,
            ZRange = zRange,
        };
    }

    public bool TurnOn { get; set; }

    public (int from, int to) XRange { get; set; }

    public (int from, int to) YRange { get; set; }

    public (int from, int to) ZRange { get; set; }
}