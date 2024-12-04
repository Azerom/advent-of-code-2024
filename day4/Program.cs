char[] mas = ['M', 'A', 'S'];

string[] lines = File.ReadAllLines("input.txt");

char[,] s = new char[lines.Length, lines[0].Length];

for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        s[i, j] = lines[i][j];
    }
}

int part1WordCount = 0;
int part2XMASCount = 0;
List<(int, int)> coords = new();

for (int i = 0; i < s.GetLength(0); i++){
    for (int y = 0; y < s.GetLength(1); y++)
    {
        if(s[i, y] == 'X')
        {
            //Check horizontal
            if (y < s.GetLength(1) - 3 && ((char[])[s[i, y + 1], s[i, y + 2], s[i, y + 3]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check hrizontal backward
            if (y > 2 && ((char[])[s[i, y - 1], s[i, y - 2], s[i, y - 3]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check vertical
            if (i < s.GetLength(0) - 3 && ((char[])[s[i + 1, y], s[i + 2, y], s[i + 3, y]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check vertical backward
            if (i > 2 && ((char[])[s[i - 1, y], s[i - 2, y], s[i - 3, y]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check diagonal down
            if (y < s.GetLength(1) - 3 && 
                i < s.GetLength(0) - 3 && 
                ((char[])[s[i + 1, y + 1], s[i + 2, y + 2], s[i + 3, y + 3]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check diagonal up
            if (y < s.GetLength(1) - 3 && 
                i > 2 && 
                ((char[])[s[i - 1, y + 1], s[i - 2, y + 2], s[i - 3, y + 3]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check diagonal up backward
            if (y > 2 && 
                i > 2 && 
                ((char[])[s[i - 1, y - 1], s[i - 2, y - 2], s[i - 3, y - 3]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }

            //Check diagonal down backward
            if (y > 2 && 
                i < s.GetLength(0) - 3 && 
                ((char[])[s[i + 1, y - 1], s[i + 2, y - 2], s[i + 3, y - 3]]).SequenceEqual(mas))
            {
                part1WordCount++;
            }
        }
        else if (s[i, y] == 'A' && i > 0 && y > 0 && i < s.GetLength(0) - 1 && y < s.GetLength(1) - 1){
            //Check diagonal down
            char[] diagOne = [s[i - 1, y - 1], s[i, y], s[i + 1, y + 1]];
            if (diagOne.SequenceEqual(mas) || diagOne.Reverse().SequenceEqual(mas))
            {
                //Check diagonal up
                char[] diagTwo = [s[i + 1, y - 1], s[i, y], s[i - 1, y + 1]];
                if (diagTwo.SequenceEqual(mas) || diagTwo.Reverse().SequenceEqual(mas))
                {
                    part2XMASCount++;
                    coords.AddRange([(i - 1, y - 1), (i, y), (i + 1, y + 1)]);
                    coords.AddRange([(i + 1, y - 1), (i, y), (i - 1, y + 1)]);
                }
            }
        }
    }
}

Console.WriteLine($"Part 1 : {part1WordCount}");
Console.WriteLine($"Part 2 : {part2XMASCount}");

for (int i = 0; i < s.GetLength(0); i++){
    for (int y = 0; y < s.GetLength(1); y++)
    {
        if(coords.Any(c => c.Item1 == i && c.Item2 == y))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        Console.Write(s[i, y]);
        Console.ResetColor();
    }
    Console.WriteLine();
}