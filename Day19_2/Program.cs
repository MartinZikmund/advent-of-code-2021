var scanners = new List<Scanner>();
while (Scanner.Read() is Scanner scanner)
{
    scanner.Id = scanners.Count;
    scanners.Add(scanner);
}

List<(int beaconA, int beaconB, int common)> commonCounts = new();

scanners[0].Recalculated = true;
while (scanners.Any(s => !s.Recalculated))
{
    foreach (var recalculatedScanner in scanners.Where(s => s.Recalculated).ToArray())
    {
        foreach (var unrecalculatedScanner in scanners.Where(s => !s.Recalculated).ToArray())
        {
            var (commonOverlap, sourceBeacon, targetBeacon) = recalculatedScanner.CountCommonByDiff(unrecalculatedScanner);
            if (commonOverlap >= 12)
            {
                // Overlapping scanners!
                // Try axis rotations
                for (int rotation = 1; rotation <= 6; rotation++)
                {
                    for (int signSwap = 0; signSwap < 8; signSwap++)
                    {
                        var coord1Multiply = (signSwap & 4) != 0;
                        var coord2Multiply = (signSwap & 2) != 0;
                        var coord3Multiply = (signSwap & 1) != 0;
                        unrecalculatedScanner.FlipCoords(coord1Multiply, coord2Multiply, coord3Multiply);

                        var matchCommon = recalculatedScanner.CountCommonByMatch(unrecalculatedScanner, sourceBeacon, targetBeacon);
                        if (matchCommon >= 12)
                        {
                            // Exact match!
                            var diff = recalculatedScanner.Beacons[sourceBeacon].DiffTo(unrecalculatedScanner.Beacons[targetBeacon]);
                            foreach(var beacon in unrecalculatedScanner.Beacons)
                            {
                                beacon.Coord1 += diff.d1;
                                beacon.Coord2 += diff.d2;
                                beacon.Coord3 += diff.d3;
                            }
                            unrecalculatedScanner.ScannerCoords = new RelativeCoords(diff.d1, diff.d2, diff.d3);
                            unrecalculatedScanner.Recalculated = true;
                        }

                        if (!unrecalculatedScanner.Recalculated)
                        {
                            unrecalculatedScanner.FlipCoords(coord1Multiply, coord2Multiply, coord3Multiply);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (!unrecalculatedScanner.Recalculated)
                    {
                        if (rotation % 3 == 0)
                        {
                            unrecalculatedScanner.SwapAxes();
                        }
                        else
                        {
                            unrecalculatedScanner.RotateAxes();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}

int maxDistance = 0;
foreach (var scanner in scanners)
{
    foreach (var otherScanner in scanners)
    {
        maxDistance = Math.Max(maxDistance,
            Math.Abs(scanner.ScannerCoords.Coord1 - otherScanner.ScannerCoords.Coord1) +
            Math.Abs(scanner.ScannerCoords.Coord2 - otherScanner.ScannerCoords.Coord2) +
            Math.Abs(scanner.ScannerCoords.Coord3 - otherScanner.ScannerCoords.Coord3));
    }
}
Console.WriteLine(maxDistance);

public class Scanner
{
    public Scanner(RelativeCoords[] beacons)
    {
        Beacons = beacons;
    }

    public int Id { get; set; }

    public RelativeCoords ScannerCoords { get; set; } = new RelativeCoords(0, 0, 0);

    public bool Recalculated { get; set; }

    public static Scanner? Read()
    {
        var line = Console.ReadLine();
        if (line == null)
        {
            return null;
        }

        var beacons = new List<RelativeCoords>();
        while (!string.IsNullOrEmpty(line = Console.ReadLine()))
        {
            beacons.Add(RelativeCoords.Parse(line));
        }
        return new Scanner(beacons.ToArray());
    }

    public RelativeCoords[] Beacons { get; }

    public (int overlap, int sourceBeacon, int targetBeacon) CountCommonByDiff(Scanner other)
    {
        var maxOverlap = 0;
        var sourceBeacon = 0;
        var targetBeacon = 0;
        for (int i = 0; i < Beacons.Length; i++)
        {
            for (int j = 0; j < other.Beacons.Length; j++)
            {
                var myDiffCoords = Beacons.Select(b => b.SquareDistnaceTo(Beacons[i])).ToArray();
                var otherDiffCoords = other.Beacons.Select(b => b.SquareDistnaceTo(other.Beacons[j])).ToArray();
                var overlap = myDiffCoords.Intersect(otherDiffCoords).Count();
                if (overlap > maxOverlap)
                {
                    maxOverlap = overlap;
                    sourceBeacon = i;
                    targetBeacon = j;
                }
            }
        }

        return (maxOverlap, sourceBeacon, targetBeacon);
    }

    public int CountCommonByMatch(Scanner other, int sourceBeacon, int targetBeacon)
    {
        var maxOverlap = 0;
        var myDiffCoords = Beacons.Select(b => b.DiffTo(Beacons[sourceBeacon])).ToArray();
        var otherDiffCoords = other.Beacons.Select(b => b.DiffTo(other.Beacons[targetBeacon])).ToArray();
        var overlap = myDiffCoords.Intersect(otherDiffCoords).Count();
        maxOverlap = Math.Max(overlap, maxOverlap);

        return maxOverlap;
    }

    internal void RotateAxes()
    {
        foreach (var beacon in Beacons)
        {
            beacon.RotateAxes();
        }
    }

    internal void FlipCoords(bool coord1Multiply, bool coord2Multiply, bool coord3Multiply)
    {
        foreach (var beacon in Beacons)
        {
            beacon.FlipCoords(coord1Multiply, coord2Multiply, coord3Multiply);
        }
    }

    internal void SwapAxes()
    {
        foreach (var beacon in Beacons)
        {
            beacon.SwapAxes();
        }
    }
}

public class RelativeCoords : IEquatable<RelativeCoords>
{
    public RelativeCoords(int coord1, int coord2, int coord3)
    {
        Coord1 = coord1;
        Coord2 = coord2;
        Coord3 = coord3;
    }

    public static RelativeCoords Parse(string line)
    {
        var parts = line.Split(',');
        return new RelativeCoords(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
    }

    public int Coord1 { get; set; }

    public int Coord2 { get; set; }

    public int Coord3 { get; set; }

    public void RotateAxes()
    {
        (Coord1, Coord2, Coord3) = (Coord3, Coord1, Coord2);
    }

    public void SwapAxes()
    {
        (Coord1, Coord2, Coord3) = (Coord1, Coord3, Coord2);
    }

    public (int d1, int d2, int d3) DiffTo(RelativeCoords other)
    {
        return (Coord1 - other.Coord1, Coord2 - other.Coord2, Coord3 - other.Coord3);
    }

    public int SquareDistnaceTo(RelativeCoords other)
    {
        var diff1 = Coord1 - other.Coord1;
        var diff2 = Coord2 - other.Coord2;
        var diff3 = Coord3 - other.Coord3;
        return diff1 * diff1 + diff2 * diff2 + diff3 * diff3;
    }

    public bool Equals(RelativeCoords? other)
    {
        return
            Coord1 == other?.Coord1 &&
            Coord2 == other?.Coord2 &&
            Coord3 == other?.Coord3;
    }

    internal void FlipCoords(bool coord1Multiply, bool coord2Multiply, bool coord3Multiply)
    {
        if (coord1Multiply)
        {
            Coord1 *= -1;
        }
        if (coord2Multiply)
        {
            Coord2 *= -1;
        }
        if (coord3Multiply)
        {
            Coord3 *= -1;
        }
    }
}
