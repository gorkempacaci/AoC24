/*
    Görkem Paçacı
    AoC24 day 4
*/
using System.Data.Common;

part1();
part2();

static void part1()
{
    // Part 1
    var lines = File.ReadAllLines("input.txt").ToArray();
    int width = lines[0].Length;
    int height = lines.Length;
    int cnt = 0;
    for (int y = 0; y < lines.Length; y++)
        for (int x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == 'X')
            {
                char[] xmas = new char[] { 'M', 'A', 'S' };
                cnt += count(lines, x, y, 1, 0, width, height, xmas);
                cnt += count(lines, x, y, 1, 1, width, height, xmas);
                cnt += count(lines, x, y, 0, 1, width, height, xmas);
                cnt += count(lines, x, y, -1, 1, width, height, xmas);
                cnt += count(lines, x, y, -1, 0, width, height, xmas);
                cnt += count(lines, x, y, -1, -1, width, height, xmas);
                cnt += count(lines, x, y, 0, -1, width, height, xmas);
                cnt += count(lines, x, y, 1, -1, width, height, xmas);
            }
        }
    Console.WriteLine("Part 1:" + cnt);
}

static void part2()
{
    // Part 2
    var lines = File.ReadAllLines("input.txt").ToArray();
    int width = lines[0].Length;
    int height = lines.Length;
    int cnt = 0;
    for (int y = 1; y < lines.Length; y++)
        for (int x = 1; x < lines[y].Length; x++)
        {
            if (lines[y][x] == 'A')
            {
                int legCnt = 0;
                char[] xmas = new char[] { 'M', 'A', 'S' };
                legCnt += count(lines, x-2, y-2, +1, +1, width, height, xmas);
                legCnt += count(lines, x+2, y-2, -1, +1, width, height, xmas);
                legCnt += count(lines, x+2, y+2, -1, -1, width, height, xmas);
                legCnt += count(lines, x-2, y+2, +1, -1, width, height, xmas);
                if (legCnt == 2)
                    cnt++;
            }
        }
    Console.WriteLine("Part 2:" + cnt);
}

static int count(string[] lines, int x0, int y0, int xoffset, int yoffset, int width, int height, Span<char> chars)
{
    if (chars.Length == 0)
        return 1;
    int x = x0 + xoffset;
    int y = y0 + yoffset;
    if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
        return 0;
    if (lines[y][x] == chars[0])
        return count(lines, x, y, xoffset, yoffset, width, height, chars.Slice(start: 1));
    else return 0;
}