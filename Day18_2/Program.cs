using Tools;

var numbers = InputTools.ReadAllLines().Select(n => SnailfishNumber.Parse(n)).ToArray();

var bestMagnitude = 0L;

for (int i = 0; i < numbers.Length; i++)
{
    for (int j = 0; j < numbers.Length; j++)
    {
        if (i == j)
        {
            continue;
        }
        bestMagnitude = Math.Max(bestMagnitude, (numbers[i] + numbers[j]).Magnitude);
        bestMagnitude = Math.Max(bestMagnitude, (numbers[j] + numbers[i]).Magnitude);
    }
}

Console.WriteLine(bestMagnitude);

public class SnailfishNumber
{
    private SnailfishNumber? _left;
    private SnailfishNumber? _right;

    public SnailfishNumber(int value)
    {
        Number = value;
    }

    public SnailfishNumber(int left, int right) :
        this(new SnailfishNumber(left), new SnailfishNumber(right))
    {
    }

    public SnailfishNumber(SnailfishNumber left, SnailfishNumber right)
    {
        Left = left;
        Right = right;
    }

    public static SnailfishNumber Parse(string input)
    {
        static SnailfishNumber ParseAt(string input, ref int position)
        {
            if (input[position] == '[')
            {
                position++;
                // Reading a pair
                var left = ParseAt(input, ref position);
                // Comma
                position++;
                var right = ParseAt(input, ref position);
                // ]
                position++;
                return new SnailfishNumber(left, right);
            }
            else
            {
                var number = "";
                while (char.IsDigit(input[position]))
                {
                    number += input[position];
                    position++;
                }
                return new SnailfishNumber(int.Parse(number));
            }
        }

        var startPosition = 0;
        return ParseAt(input, ref startPosition);
    }

    public bool IsNumber => Left == null && Right == null;

    public SnailfishNumber? Parent { get; private set; }

    public int Depth { get; private set; }

    public int Number { get; private set; }

    public SnailfishNumber? Left
    {
        get => _left;
        private set
        {
            _left = value;
            if (_left != null)
            {
                _left.Parent = this;
                _left.SetDepth(Depth + 1);
            }
        }
    }

    public SnailfishNumber? Right
    {
        get => _right;
        private set
        {
            _right = value;
            if (_right != null)
            {
                _right.Parent = this;
                _right.SetDepth(Depth + 1);
            }
        }
    }

    public static SnailfishNumber operator +(SnailfishNumber left, SnailfishNumber right)
    {
        var pair = new SnailfishNumber(left.Clone(), right.Clone());
        var changeOcurred = true;
        while (changeOcurred)
        {
            changeOcurred = pair.ExplodeLeftmost() || pair.SplitLeftmost();
        }
        return pair;
    }

    public SnailfishNumber Clone() => SnailfishNumber.Parse(ToString());

    public long Magnitude
    {
        get
        {
            if (IsNumber)
            {
                return Number;
            }
            else
            {
                return 3 * Left!.Magnitude + 2 * Right!.Magnitude;
            }
        }
    }

    public bool ExplodeLeftmost()
    {
        if (Left?.ExplodeLeftmost() == true)
        {
            return true;
        }
        else if (this.Explode())
        {
            return true;
        }
        else
        {
            return Right?.ExplodeLeftmost() == true;
        }
    }

    public bool SplitLeftmost()
    {
        if (Left?.SplitLeftmost() == true)
        {
            return true;
        }
        else if (this.Split())
        {
            return true;
        }
        else
        {
            return Right?.SplitLeftmost() == true;
        }
    }

    public override string ToString()
    {
        if (IsNumber)
        {
            return Number.ToString();
        }

        return $"[{Left},{Right}]";
    }

    public void SetDepth(int depth)
    {
        Depth = depth;
        Left?.SetDepth(depth + 1);
        Right?.SetDepth(depth + 1);
    }

    public void AddToNumber(int value)
    {
        Number += value;
    }

    public bool Split()
    {
        if (!IsNumber || Number < 10)
        {
            return false;
        }

        var pair = new SnailfishNumber(Number / 2, (Number + 1) / 2);
        if (Parent!.Left == this)
        {
            Parent.Left = pair;
        }
        else
        {
            Parent.Right = pair;
        }

        return true;
    }

    public bool Explode()
    {
        if (Depth < 4 || IsNumber)
        {
            return false;
        }

        if (!Left!.IsNumber || !Right!.IsNumber)
        {
            return false;
        }

        AddClosestLeft();
        AddClosestRight();

        if (Parent!.Left == this)
        {
            Parent.Left = new SnailfishNumber(0);
        }
        else
        {
            Parent.Right = new SnailfishNumber(0);
        }

        return true;
    }

    private SnailfishNumber? AddClosestLeft()
    {
        var current = this;
        while (current.Parent?.Left == current)
        {
            current = current.Parent;
        }

        if (current.Parent == null)
        {
            return null;
        }

        var source = current.Parent.Left;
        while (!source!.IsNumber)
        {
            source = source.Right;
        }

        source.AddToNumber(Left!.Number);
        return source;
    }

    private SnailfishNumber? AddClosestRight()
    {
        var current = this;
        while (current.Parent?.Right == current)
        {
            current = current.Parent;
        }

        if (current.Parent == null)
        {
            return null;
        }

        var source = current.Parent.Right;
        while (!source!.IsNumber)
        {
            source = source.Left;
        }

        source.AddToNumber(Right!.Number);
        return source;
    }
}