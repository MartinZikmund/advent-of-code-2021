using Day24_1;
using System.Diagnostics;
using Tools;


//var rewrite = new Rewrite();
//rewrite.W6((byte)9, rewrite.W5((byte)2, 0));
//for (int i = 1; i < 10; i++)
//{
//    for (int j = 1; j < 10; j++)
//    {
//        Console.WriteLine($"w5: {j}, w6: {i}: " + rewrite.W6((byte)i, rewrite.W5((byte)j, 0)));

//        //for (int j = 1; j < 10; j++)
//        //{
//        //    for (int k = 1; k < 10; k++)
//        //    {
//        //        if (rewrite.W3((byte)k, rewrite.W2((byte)j, rewrite.W1((byte)i))) < 0)
//        //        {
//        //            Debugger.Break();
//        //        }
//        //    }
//        //}
//    }
//}

var aluProgram = AluProgram.Read();
var states = new List<(string x, string y, string z)>();
for (int batchId = 0; batchId < aluProgram.Batches.Length; batchId++)
{
    var state = ($"x{batchId}", $"y{batchId}", $"z{batchId}");
    var resultState = aluProgram.Batches[batchId].ProcessInput($"w{batchId + 1}", state);
    Console.WriteLine($"x: " + resultState.x);
    Console.WriteLine($"y: " + resultState.y);
    Console.WriteLine($"z: " + resultState.z);
}

//var alu = AluProgram.Read();
//var z = "0";
//var i = 0;
//foreach (byte b in new byte[] { 11931881141161 })
//{
//    z = alu.Batches[i].ProcessInput(b.ToString(), ("0", "0", z.ToString())).z;
//    i++;
//    Console.WriteLine($"After {i}: {z}");
//}
//Console.WriteLine(z);

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
                instructions.Clear();
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

    public (string x, string y, string z) ProcessInput(string input, (string x, string y, string z) state)
    {
        var registry = new Dictionary<char, string>();
        registry['x'] = state.x;
        registry['y'] = state.y;
        registry['z'] = state.z;
        long longVal1 = 0, longVal2 = 0;
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
                    if (long.TryParse(val1, out longVal1) && long.TryParse(val2, out longVal2))
                    {
                        registry[instruction.Parameter1[0]] = (longVal1 + longVal2).ToString();
                    }
                    else if (val1 == "0")
                    {
                        registry[instruction.Parameter1[0]] = val2;
                    }
                    else if (val2 == "0")
                    {
                        registry[instruction.Parameter1[0]] = val1;
                    }
                    else
                    {
                        registry[instruction.Parameter1[0]] = $"({val1} + {val2})";
                    }
                    break;
                case "mul":
                    if (long.TryParse(val1, out longVal1) && long.TryParse(val2, out longVal2))
                    {
                        registry[instruction.Parameter1[0]] = (longVal1 * longVal2).ToString();
                    }
                    else if (val1 == "0" || val2 == "0")
                    {
                        registry[instruction.Parameter1[0]] = "0";
                    }
                    else if (val1 == "1")
                    {
                        registry[instruction.Parameter1[0]] = val2;
                    }
                    else if (val2 == "1")
                    {
                        registry[instruction.Parameter1[0]] = val1;
                    }
                    else
                    {
                        registry[instruction.Parameter1[0]] = $"({val1} * {val2})";
                    }
                    break;
                case "div":
                    if (long.TryParse(val1, out longVal1) && long.TryParse(val2, out longVal2))
                    {
                        registry[instruction.Parameter1[0]] = (longVal1 / longVal2).ToString();
                    }
                    else if (val2 == "1")
                    {
                        registry[instruction.Parameter1[0]] = val1;
                    }
                    else
                    {
                        registry[instruction.Parameter1[0]] = $"({val1} / {val2})";
                    }
                    break;
                case "mod":
                    if (long.TryParse(val1, out longVal1) && long.TryParse(val2, out longVal2))
                    {
                        registry[instruction.Parameter1[0]] = (longVal1 % longVal2).ToString();
                    }
                    else
                    {
                        registry[instruction.Parameter1[0]] = $"({val1} % {val2})";
                    }
                    break;
                case "eql":
                    if (long.TryParse(val1, out longVal1) && long.TryParse(val2, out longVal2))
                    {
                        registry[instruction.Parameter1[0]] = (longVal1 == longVal2 ? 1 : 0).ToString();
                    }
                    else if (val1 == val2)
                    {
                        registry[instruction.Parameter1[0]] = "1";
                    }
                    else if (val1.StartsWith("w") && long.TryParse(val2, out longVal2) && (longVal2 < 0 || longVal2 > 9))
                    {
                        registry[instruction.Parameter1[0]] = "0";
                    }
                    else if (val2.StartsWith("w") && long.TryParse(val1, out longVal1) && (longVal1 < 0 || longVal1 > 9))
                    {
                        registry[instruction.Parameter1[0]] = "0";
                    }
                    else
                    {
                        registry[instruction.Parameter1[0]] = $"({val1} == {val2})";
                    }
                    break;
            }
        }

        return (registry['x'], registry['y'], registry['z']);

        string GetParameterValue(string? parameter)
        {
            if (parameter == null)
            {
                return "";
            }

            if (char.IsLetter(parameter[0]))
            {
                if (registry.TryGetValue(parameter[0], out var val))
                {
                    return val;
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return parameter;
            }
        }
    }
}