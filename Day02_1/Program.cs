Dictionary<string, (int x, int depth)> directions = new()
{
    { "forward", (1, 0) },
    { "up", (0, -1) },
    { "down", (0, 1) },

};

(int x, int depth) position = (0, 0);
while (await Console.In.ReadLineAsync() is { } line)
{
    var command = line.Split(' ');
    var direction = directions[command[0]];
    var multiple = int.Parse(command[1]);
    position = (position.x + multiple * direction.x, position.depth + multiple * direction.depth);
}

await Console.Out.WriteLineAsync((position.x * position.depth).ToString());