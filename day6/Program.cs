using System.Diagnostics;

string[] lines = File.ReadAllLines("input.txt");

char[,] map = new char[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        map[i, j] = lines[i][j];
    }
}

var startPosition = map.IndexOf2D('^') ?? throw new InvalidDataException();

Direction direction = Direction.North;
Coordinate coords = startPosition;

List<Coordinate> part1 = [];

Stopwatch sw = new();
sw.Start();

while(direction != Direction.Exit){
    
    var newPath = Move(ref coords, ref direction, map).Where(n => !part1.Any(p => p == n));

    foreach(var position in newPath){
        map[position.x, position.y] = 'X';
    }

    part1.AddRange(newPath);
    PrintMap(map, coords.x, coords.y);
}

sw.Stop();

Console.WriteLine($"Part 1 : {part1.Count}. Done in {sw.ElapsedMilliseconds} ms");

static void WriteAt(char s, int x, int y)
{
    try
        {
        Console.SetCursorPosition(0+x, 0+y);
        Console.Write(s);
        }
    catch (ArgumentOutOfRangeException e)
        {
        Console.Clear();
        Console.WriteLine(e.Message);
        }
}

void PrintMap(char[,] map, int x, int y){

    int origRow = Console.CursorTop;
    int origCol = Console.CursorLeft;
    
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            if(i == x && j == y){
                Console.ForegroundColor = ConsoleColor.Red;
                WriteAt('&', origRow+i, origCol+j);
                Console.ResetColor();
            }
            else{
                if(map[i, j] == 'X'){
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                WriteAt(map[i, j], origRow+i, origCol+j);
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    // Thread.Sleep(1000);
}

IEnumerable<Coordinate> Move(ref Coordinate coords, ref Direction direction, char[,] map)
{
    bool onCol = direction switch 
    {
        Direction.North => true,
        Direction.South => true,
        _ => false,
    };
    bool reverse = direction switch
    {
        Direction.North => true,
        Direction.West => true,
        _ => false
    };

    int? limiter = onCol ? coords.x : coords.y;

    int? from = reverse ? null : limiter;

    int? to = reverse ? limiter : null;

    var array = onCol ? map.GetColumn(coords.y, from, to) : map.GetRow(coords.x, from, to);

    var obstacleIndex = reverse ? Array.FindIndex(array, c => c == '#') : Array.FindLastIndex(array, c => c == '#');

    int oX = coords.x;
    int oY = coords.y;

    List<Coordinate> result = array.Select( (s, i) => new Coordinate(onCol ? i + (from ?? 0) : oX, onCol ? oY : i + (from ?? 0))).ToList();

    if(reverse){
        result = result.Skip(obstacleIndex + 1).ToList();
    }
    else {
        result = result.Skip(1).SkipLast(obstacleIndex == -1 ? 0 : array.Length - obstacleIndex).ToList();
    }

    Coordinate newCoords = coords;

    if (obstacleIndex == -1)
    {
        newCoords = direction switch
        {
            Direction.North => (0, coords.y),
            Direction.South => (map.GetLength(0) - 1, coords.y),
            Direction.East => (coords.x, map.GetLength(1) - 1),
            Direction.West => (coords.x, 0),
            _ => throw new InvalidOperationException()
        };
        direction = Direction.Exit;
    }
    else
    {
        obstacleIndex += from ?? 0;

        newCoords = new Coordinate(onCol ? reverse ? obstacleIndex + 1 : obstacleIndex - 1 : coords.x,
                  onCol ? coords.y : reverse ? obstacleIndex + 1 : obstacleIndex - 1);

        direction = direction switch
        {
            Direction.North => Direction.East,
            Direction.South => Direction.West,
            Direction.East => Direction.South,
            Direction.West => Direction.North,
            _ => throw new InvalidOperationException()
        };
    }
    
    coords = newCoords;

    return result;
}


enum Direction{
    North,
    South,
    West,
    East,
    Exit
}

internal record struct Coordinate(int x, int y)
{
    public static implicit operator (int x, int y)(Coordinate value)
    {
        return (value.x, value.y);
    }

    public static implicit operator Coordinate((int x, int y) value)
    {
        return new Coordinate(value.x, value.y);
    }
}