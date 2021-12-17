using Tools;

const int Flashed = 100000;

var lines = InputTools.ReadAllLines();

Dictionary<string, Cave> caves = new();

foreach (var line in lines)
{
    var connection = line.Split('-');
    var cave1 = AddOrCreateCave(connection[0].Trim());
    var cave2 = AddOrCreateCave(connection[1].Trim());
    cave1.Connections.Add(cave2);
    cave2.Connections.Add(cave1);
}

var start = caves["start"];
var pathCount = GetPathCount(start, new HashSet<string>() { "start" });
Console.WriteLine(pathCount);

long GetPathCount(Cave current, HashSet<string> visited)
{
    if (current.Name == "end")
    {
        return 1;
    }

    var totalPaths = 0L;
    foreach (var connection in current.Connections)
    {
        if (char.IsUpper(connection.Name[0]) || !visited.Contains(connection.Name))
        {
            visited.Add(connection.Name);
            totalPaths += GetPathCount(connection, visited);
            visited.Remove(connection.Name);
        }
    }
    return totalPaths;
}

Cave AddOrCreateCave(string name)
{
    if (!caves.TryGetValue(name, out var cave))
    {
        cave = new Cave(name);
        caves.Add(name, cave);
    }

    return cave;
}

public class Cave
{
    public Cave(string name) => Name = name;

    public string Name { get; }

    public List<Cave> Connections { get; } = new List<Cave>();
}