var instructions = new List<Instruction>();
while (Instruction.Read() is Instruction instruction)
{
    instructions.Add(instruction);
}

var cube = new bool[101, 101, 101];

foreach (var instruction in instructions)
{
    for (int x = Math.Max(-50, instruction.XRange.from); x <= Math.Min(50, instruction.XRange.to); x++)
    {
        for (int y = Math.Max(-50, instruction.YRange.from); y <= Math.Min(50, instruction.YRange.to); y++)
        {
            for (int z = Math.Max(-50, instruction.ZRange.from); z <= Math.Min(50, instruction.ZRange.to); z++)
            {
                cube[x + 50, y + 50, z + 50] = instruction.TurnOn;
            }
        }
    }
}

long counter = 0;
for (int x = 0; x < 101; x++)
{
    for (int y = 0; y < 101; y++)
    {
        for (int z = 0; z < 101; z++)
        {
            if (cube[x, y, z])
            {
                counter++;
            }
        }
    }
}

Console.WriteLine(counter);

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