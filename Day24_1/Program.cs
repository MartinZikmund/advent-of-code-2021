using Tools;

var reachableStates = new Dictionary<(long x, long y, long z), ulong>();
reachableStates.Add((0, 0, 0), 0);
var aluProgram = AluProgram.Read();
foreach (var batch in aluProgram.Batches)
{
    var newReachableStates = new Dictionary<(long x, long y, long z), ulong>();
    for (byte input = 0; input < 10; input++)
    {
        foreach (var state in reachableStates)
        {
            var newValue = state.Value * 10 + input;
            var resultState = batch.ProcessInput(input, state.Key);
            if (!newReachableStates.TryGetValue(resultState, out var bestValue))
            {
                bestValue = 0;
            }

            if (bestValue < newValue)
            {
                newReachableStates[resultState] = newValue;
            }
        }
    }
    reachableStates = newReachableStates;
}

var bestValueOverall = 0UL;
foreach(var state in reachableStates)
{
    if (state.Key.z == 0)
    {
        if(bestValueOverall < state.Value)
        {
            bestValueOverall = state.Value;
        }
    }
}

Console.WriteLine(bestValueOverall);

public class AluProgram
{
    public AluProgram(Batch[] batches)
    {
        Batches = batches;
    }

    public Batch[] Batches { get; }

    public static AluProgram Read()
    {
        var batches = new List<Batch>();
        var instructions = new List<AluInstruction>();
        var lines = InputTools.ReadAllLines();
        foreach (var line in lines)
        {
            var instruction = AluInstruction.Parse(line);
            if (instruction.Method == "inp" && instructions.Count > 0)
            {
                batches.Add(new Batch(instructions.ToArray()));
            }
            instructions.Add(instruction);
        }
        batches.Add(new Batch(instructions.ToArray()));
        return new AluProgram(batches.ToArray());
    }
}

public class AluInstruction
{
    public static AluInstruction Parse(string input)
    {
        var instruction = new AluInstruction();
        var parts = input.Split(' ');
        instruction.Method = parts[0].Trim();
        instruction.Parameter1 = parts[1].Trim();
        if (parts.Length > 2)
        {
            instruction.Parameter2 = parts[2].Trim();
        }
        return instruction;
    }

    public string Method { get; set; }

    public string Parameter1 { get; set; }

    public string? Parameter2 { get; set; }
}

public class Batch
{
    public Batch(AluInstruction[] instructions)
    {
        Instructions = instructions;
    }

    public AluInstruction[] Instructions { get; }

    public (long x, long y, long z) ProcessInput(int input, (string x, string y, string z) state)
    {
        var registry = new Dictionary<char, long>();
        registry['x'] = state.x;
        registry['y'] = state.y;
        registry['z'] = state.z;
        foreach (var instruction in Instructions)
        {
            var val1 = GetParameterValue(instruction.Parameter1);
            var val2 = GetParameterValue(instruction.Parameter2);
            switch (instruction.Method)
            {
                case "inp":
                    registry[instruction.Parameter1[0]] = input;
                    break;
                case "add":
                    registry[instruction.Parameter1[0]] = val1 + val2;
                    break;
                case "mul":
                    registry[instruction.Parameter1[0]] = val1 * val2;
                    break;
                case "div":
                    registry[instruction.Parameter1[0]] = val1 / val2;
                    break;
                case "mod":
                    registry[instruction.Parameter1[0]] = val1 % val2;
                    break;
                case "eql":
                    registry[instruction.Parameter1[0]] = val1 == val2 ? 1 : 0;
                    break;
            }
        }

        return (registry['x'], registry['y'], registry['z']);

        long GetParameterValue(string? parameter)
        {
            if (parameter == null)
            {
                return -1;
            }
            if (char.IsLetter(parameter[0]))
            {
                if (registry.TryGetValue(parameter[0], out var val))
                {
                    return val;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return long.Parse(parameter);
            }
        }
    }
}