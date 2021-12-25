var player1Location = int.Parse(Console.ReadLine()!.Substring("Player X starting position: ".Length));
var player2Location = int.Parse(Console.ReadLine()!.Substring("Player X starting position: ".Length));

var score1 = 0;
var score2 = 0;

bool player1Turn = true;
var dieState = 1;
var rolls = 0;
while (score1 < 1000 && score2 < 1000)
{
    var move = Roll() + Roll() + Roll();
    if (player1Turn)
    {
        MovePlayer(move, ref player1Location, ref score1);
    }
    else
    {
        MovePlayer(move, ref player2Location, ref score2);
    }
    player1Turn = !player1Turn;
}

Console.WriteLine(rolls * Math.Min(score1, score2));

void MovePlayer(int move, ref int playerLocation, ref int playerScore)
{
    playerLocation += move;
    while (playerLocation > 10)
    {
        playerLocation -= 10;
    }
    playerScore += playerLocation;
}

int Roll()
{
    var number = dieState++;
    if (dieState == 101)
    {
        dieState = 1;
    }
    rolls++;
    return number;
}