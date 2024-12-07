/*
    Görkem Paçacı
    AoC24 day 6
*/
using System.Numerics;

Console.WriteLine("Part 1: "+part1(null));
Console.WriteLine("Part 2: "+part2());

Vector2 rotate90(Vector2 dir) => Vector2.Transform(dir, Matrix3x2.CreateRotation((float)Math.PI/2));
(int x, int y) advance((int x, int y) pos, Vector2 dir) => (pos.x + (int)dir.X, pos.y + (int)dir.Y);


int part1(char[][]? lines)
{
    int visited = 1;
    if (lines is null)
        lines = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
    var directions = lines.Select(l => l.Select(c => new HashSet<Vector2>()).ToArray()).ToArray();
    (int x, int y) guard = (-1, -1);
    for(int y=0; y<lines.Length; y++)
        for(int x=0; x<lines[y].Length; x++)
            if (lines[y][x]=='^')
            {
                guard = (x, y);
            }
    Vector2 direction = new(0, -1);
    while(true)
    {
        var nextPos = advance(guard, direction);
        if (nextPos.x < 0 || nextPos.x >= lines[0].Length || nextPos.y < 0 || nextPos.y >= lines.Length)
            break;
        if (directions[guard.y][guard.x].Contains(direction))
            return -1; // looped
        if (lines[nextPos.y][nextPos.x] == '#' || lines[nextPos.y][nextPos.x] == 'O')
        {
            direction = rotate90(direction);
        } else
        {
            if (lines[nextPos.y][nextPos.x] == '.')
            {
                lines[nextPos.y][nextPos.x] = 'X';
                visited++;
            }
            directions[guard.y][guard.x].Add(direction);
            guard = nextPos;
        }
    }
    return visited;
}


int part2()
{
    // Part 1
    var lines = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
    (int x, int y) guard = (-1, -1);
    for(int y=0; y<lines.Length; y++)
    {
        for(int x=0; x<lines[y].Length; x++)
            if (lines[y][x]=='^')
            {
                guard = (x, y);
                break;
            }
    }
    var guardStartPosition = guard;
    Vector2 direction = new(0, -1);
    HashSet<(int x, int y)> newObstacles = new();
    while(true)
    {
        var nextPos = advance(guard, direction);
        if (nextPos.x < 0 || nextPos.x >= lines[0].Length || nextPos.y < 0 || nextPos.y >= lines.Length)
            break;
        lines[guard.y][guard.x] = 'X';
        if (lines[nextPos.y][nextPos.x] == '#')
        {
            direction = rotate90(direction);
        } else
        {
            // speculate new obstacle
            var specDirection = rotate90(direction);
            var newLines = lines.Select(l => l.Select(c =>c).ToArray()).ToArray();
            newLines[nextPos.y][nextPos.x] = 'O';
            newLines[guardStartPosition.y][guardStartPosition.x] = '^';
            if (newObstacles.Count()==45)
                for(int i=0; i<newLines.Length; i++)
                    Console.WriteLine(newLines[i]);
            if (part1(newLines)==-1)//hitsAnX(newLines, newDirections, guard, specDirection, 130*130*100))
            {
                if (nextPos != guardStartPosition)
                {
                    newObstacles.Add(nextPos);
                    Console.WriteLine("Obstacles: " + newObstacles.Count());
                }
            }
            guard = nextPos;
        }
    }
    return newObstacles.Count();
}

bool hitsAnX(char[][] lines, Vector2[][] directions, (int x, int y) pos, Vector2 direction, int maxpath)
{
    if (maxpath <= 0)
        return false;
    if (pos.x <0 || pos.x >= lines[0].Length || pos.y <0 || pos.y >= lines.Length)
        return false;
    if (lines[pos.y][pos.x] == 'X' && direction == directions[pos.y][pos.x])
        return true;
    var nextPos = advance(pos, direction);
    if (nextPos.x <0 || nextPos.x >= lines[0].Length || nextPos.y <0 || nextPos.y >= lines.Length)
        return false;
    lines[pos.y][pos.x] = 'X';
    if (lines[nextPos.y][nextPos.x] == '#')
    {
        directions[pos.y][pos.x] = rotate90(direction);
        return hitsAnX(lines, directions, pos, rotate90(direction), maxpath-1);
    }
    directions[pos.y][pos.x] = direction;
    return hitsAnX(lines, directions, nextPos, direction, maxpath-1);
}