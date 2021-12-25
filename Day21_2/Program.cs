var player1Location = int.Parse(Console.ReadLine()!.Substring("Player X starting position: ".Length));
var player2Location = int.Parse(Console.ReadLine()!.Substring("Player X starting position: ".Length));

Dictionary<(int location1, int location2, int score1, int score2, bool player1Turn), ulong> universeCounters = new();

universeCounters.Add((player1Location, player2Location, 0, 0, true), 1);

for (int score1 = 0; score1 <= 20; score1++)
{
    for (int score2 = 0; score2 <= 20; score2++)
    {
        for (int location1 = 1; location1 <= 10; location1++)
        {
            for (int location2 = 1; location2 <= 10; location2++)
            {
                foreach (var turn in new[] { true, false })
                {
                    if (universeCounters.TryGetValue((location1, location2, score1, score2, turn), out var universes))
                    {
                        for (int roll1 = 1; roll1 <= 3; roll1++)
                        {
                            for (int roll2 = 1; roll2 <= 3; roll2++)
                            {
                                for (int roll3 = 1; roll3 <= 3; roll3++)
                                {
                                    if (turn)
                                    {
                                        var newLocation = location1;
                                        var newScore = score1;
                                        MovePlayer(roll1 + roll2 + roll3, ref newLocation, ref newScore);
                                        AddOrCreateUniverses(newLocation, location2, newScore, score2, !turn, universes);
                                    }
                                    else
                                    {
                                        var newLocation = location2;
                                        var newScore = score2;
                                        MovePlayer(roll1 + roll2 + roll3, ref newLocation, ref newScore);
                                        AddOrCreateUniverses(location1, newLocation, score1, newScore, !turn, universes);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

ulong player1Wins = 0;
ulong player2Wins = 0;
foreach (var item in universeCounters)
{
    if (item.Key.score1 >= 21)
    {
        player1Wins += item.Value;
    }
    if (item.Key.score2 >= 21)
    {
        player2Wins += item.Value;
    }
}

Console.WriteLine(player1Wins);
Console.WriteLine(player2Wins);

void AddOrCreateUniverses(int location1, int location2, int score1, int score2, bool turn, ulong newUniverses)
{
    if (!universeCounters.TryGetValue((location1, location2, score1, score2, turn), out var universes))
    {
        universes = 0;
    }
    universes += newUniverses;
    universeCounters[(location1, location2, score1, score2, turn)] = universes;
}

void MovePlayer(int move, ref int playerLocation, ref int playerScore)
{
    playerLocation += move;
    while (playerLocation > 10)
    {
        playerLocation -= 10;
    }
    playerScore += playerLocation;
}