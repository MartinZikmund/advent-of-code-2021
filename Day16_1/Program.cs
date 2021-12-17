using System.Security.Authentication.ExtendedProtection;
using Tools;

Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string> {
    { '0', "0000" },
    { '1', "0001" },
    { '2', "0010" },
    { '3', "0011" },
    { '4', "0100" },
    { '5', "0101" },
    { '6', "0110" },
    { '7', "0111" },
    { '8', "1000" },
    { '9', "1001" },
    { 'a', "1010" },
    { 'b', "1011" },
    { 'c', "1100" },
    { 'd', "1101" },
    { 'e', "1110" },
    { 'f', "1111" }
};

var input = string.Join(null,
    Console.ReadLine()!.Trim().Select(c => hexCharacterToBinary[char.ToLowerInvariant(c)]));

var position = 0;
var versionSum = 0;
var packet = ReadPacket(input, ref position);

Console.WriteLine(versionSum);

Packet ReadPacket(string input, ref int position)
{
    var packet = new Packet();
    var firstByte = Convert.ToInt32(input.Substring(position, 8), 2);
    position += 6;
    packet.Version = GetVersion(firstByte);
    versionSum += packet.Version;
    packet.Type = GetPacketType(firstByte);
    if (packet.Type == PacketType.Literal)
    {
        packet.Value = ReadLiteralValue(input, ref position);
    }
    else
    {
        packet.LengthType = input[position++] == '0' ? LengthType.TotalLength : LengthType.NumberOfSubpackets;
        string lengthString;
        if (packet.LengthType == LengthType.TotalLength)
        {
            lengthString = input.Substring(position, 15);
            position += 15;
        }
        else
        {
            lengthString = input.Substring(position, 11);
            position += 11;
        }
        packet.Length = Convert.ToInt32(lengthString, 2);

        if (packet.LengthType == LengthType.NumberOfSubpackets)
        {
            for (int subPacketId = 0; subPacketId < packet.Length; subPacketId++)
            {
                packet.SubPackets.Add(ReadPacket(input, ref position));
            }
        }
        else
        {
            var startingPosition = position;
            while (position < startingPosition + packet.Length)
            {
                packet.SubPackets.Add(ReadPacket(input, ref position));
            }
        }
    }
    return packet;
}

string ReadLiteralValue(string input, ref int position)
{
    var value = "";
    var readLast = false;
    while (!readLast)
    {
        readLast = input[position] == '0';
        position++;
        value += input.Substring(position, 4);
        position += 4;
    }

    return value;
}

int GetVersion(int firstByte)
{
    var versionPattern = 0b11100000;
    var version = (firstByte & versionPattern) >> 5;
    return version;
}

PacketType GetPacketType(int firstByte)
{
    var packetTypePattern = 0b00011100;
    var packetType = (firstByte & packetTypePattern) >> 2;
    return (PacketType)packetType;
}

public enum PacketType
{
    Sum,
    Product,
    Minimum,
    Maximum,
    Literal,
    GreaterThan,
    LessThan,
    EqualTo,
}

public enum LengthType
{
    TotalLength,
    NumberOfSubpackets
}

public class Packet
{
    public int Version { get; set; }

    public PacketType Type { get; set; }

    public LengthType LengthType { get; set; }

    public int Length { get; set; }

    public string Value { get; set; }

    public List<Packet> SubPackets { get; } = new();

    public long Evaluate()
    {
        switch (Type)
        {
            case PacketType.Sum:
                return SubPackets.Select(p => p.Evaluate()).Sum();
            case PacketType.Product:
                return SubPackets.Select(p => p.Evaluate()).Aggregate(1L, (a, b) => a * b);
            case PacketType.Minimum:
                return SubPackets.Min(p => p.Evaluate());
            case PacketType.Maximum:
                return SubPackets.Max(p => p.Evaluate());
            case PacketType.Literal:
                return Convert.ToInt64(Value, 2);
            case PacketType.GreaterThan:
                return SubPackets[0].Evaluate() > SubPackets[1].Evaluate() ? 1 : 0;
            case PacketType.LessThan:
                return SubPackets[0].Evaluate() < SubPackets[1].Evaluate() ? 1 : 0;
            case PacketType.EqualTo:
                return SubPackets[0].Evaluate() == SubPackets[1].Evaluate() ? 1 : 0;
            default:
                throw new InvalidOperationException();
        }
    }
}