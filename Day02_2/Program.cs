Dictionary<string, Func<(int x, int depth, int aim), int, (int x, int depth, int aim)>> commands = new()
{
    { "forward", (state, multiple) => (state.x + multiple, state.depth + state.aim * multiple, state.aim) },
    { "up", (state, multiple) => (state.x, state.depth, state.aim - multiple) },
    { "down", (state, multiple) => (state.x, state.depth, state.aim + multiple) },
};

(int x, int depth, int aim) position = (0, 0, 0);
while (await Console.In.ReadLineAsync() is { } line)
{
    var input = line.Split(' ');
    var command = commands[input[0]];
    var multiple = int.Parse(input[1]);
    position = command(position, multiple);
}

await Console.Out.WriteLineAsync((position.x * position.depth).ToString());