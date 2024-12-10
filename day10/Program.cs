/*
    Görkem Paçacı
    AoC24 day 10
*/
using System.Xml.Schema;

part1();

part2();



void part1()
{
    var lines = File.ReadAllLines("input.txt").Select(l => l.Select(c => (int)(c - '0')).ToArray()).ToArray();
    var scores = new HashSet<(int x, int y)>[lines.Length][];
    for (int i = 0; i < lines.Length; i++)
    {
        scores[i] = new HashSet<(int x, int y)>[lines[i].Length];
        for (int j=0; j<lines[i].Length; j++)
            scores[i][j] = new HashSet<(int x, int y)>();
    }
    for (int pass = 0; pass < 9; pass++)
    {
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == 9 - pass)
                {
                    moveScore2(lines, scores, x, y, 0, -1, pass);
                    moveScore2(lines, scores, x, y, 0, +1, pass);
                    moveScore2(lines, scores, x, y, -1, 0, pass);
                    moveScore2(lines, scores, x, y, +1, 0, pass);
                }
            }
        }
    }
    long score = 0;
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[0].Length; x++)
        {
            if (lines[y][x] == 0)
                score += scores[y][x].Count();
        }
    }
    Console.WriteLine("Part 2:" + score);
}


void part2()
{
    var lines = File.ReadAllLines("input.txt").Select(l => l.Select(c => (int)(c - '0')).ToArray()).ToArray();
    var scores = new int[lines.Length][];
    for (int i = 0; i < lines.Length; i++)
        scores[i] = new int[lines[i].Length];
    for (int pass = 0; pass < 9; pass++)
    {
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == 9 - pass)
                {
                    moveScore(lines, scores, x, y, 0, -1, pass);
                    moveScore(lines, scores, x, y, 0, +1, pass);
                    moveScore(lines, scores, x, y, -1, 0, pass);
                    moveScore(lines, scores, x, y, +1, 0, pass);
                }
            }
        }
    }
    long score = 0;
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[0].Length; x++)
        {
            if (lines[y][x] == 0)
                score += scores[y][x];
        }
    }
    Console.WriteLine("Part 1:" + score);
}

void moveScore(int[][] lines, int[][] scores, int x, int y, int xx, int yy, int pass)
{
    if (x + xx >= 0 && x + xx < lines[y].Length && y + yy >= 0 && y + yy < lines.Length)
    {
        if (lines[y + yy][x + xx] == 9 - pass - 1)
        {
            if (pass == 0)
                scores[y + yy][x + xx] += 1;
            else scores[y + yy][x + xx] += scores[y][x];
        }
    }
}


void moveScore2(int[][] lines, HashSet<(int x, int y)>[][] scores, int x, int y, int xx, int yy, int pass)
{
    if (x + xx >= 0 && x + xx < lines[y].Length && y + yy >= 0 && y + yy < lines.Length)
    {
        if (lines[y + yy][x + xx] == 9 - pass - 1)
        {
            if (pass == 0)
                scores[y + yy][x + xx].Add((x, y));
            else foreach(var s in scores[y][x]) scores[y + yy][x + xx].Add(s);
        }
    }
}