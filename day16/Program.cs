/*
    Görkem Paçacı
    AoC24 day 16
*/
using System.ComponentModel;
using System.Diagnostics;


Stopwatch sp = new Stopwatch();
sp.Start();
part1();
sp.Stop();
Console.WriteLine("time: " + sp.ToString());
sp.Restart();
part2();
sp.Stop();
Console.WriteLine("time:" + sp.ToString());

void part1()
{
    var lines = File.ReadAllLines("input.txt");
    var lines2 = lines.Select(l => l.ToCharArray()).ToArray();
    (int x, int y) E = (-1, -1); bool foundE = false;
    (int x, int y) S = (-1, -1); bool foundS = false;
    for (int y = 0; y < lines.Length; y++)
    {

        if (!foundE)
        {
            int ee = lines[y].IndexOf('E');
            if (ee > -1)
            {
                E = (ee, y);
                foundE = true;
            }
        }
        if (!foundS)
        {
            int ss = lines[y].IndexOf('S');
            if (ss > -1)
            {
                S = (ss, y);
                foundS = true;
            }
        }
        if (foundE&&foundS)
            break;
    }
    Stack<((int x, int y) pos, (int x, int y) dir, int costSoFar)> heads = new();
    Dictionary<(int x, int y), long> cache = new();
    heads.Push((S, (1,0), 0));
    cache.Add(S, 0);
    List<long> successes=new();
    while(heads.Count>0)
    {
        var head = heads.Pop();
        if (head.pos==E)
        {
            successes.Add(head.costSoFar);
            continue;
        }
        var around = new (int x, int y)[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
        foreach(var move in around)
        {
            int xx=head.pos.x+move.x;
            int yy=head.pos.y+move.y;
            if (!(lines[yy][xx] == '.' || lines[yy][xx] == 'E'))
            {
                continue;
            }
            int newCost=head.costSoFar+1+turnCost(head.dir,move);
            if (cache.TryGetValue((xx, yy), out var oldCost) && oldCost < newCost)
            {
                continue;
            } else cache[(xx,yy)] = newCost;
            heads.Push((pos:(xx, yy), dir:move, costSoFar:newCost));
        }
        
        lines2[head.pos.y][head.pos.x] = 'O';
    }
    // for(int i=0; i<lines2.Length; i++)
    //     Console.WriteLine(lines2[i]);
    long min = successes.Min();
    Console.WriteLine("Part 1:"+min);
}


void part2()
{
    // 5158 too high
    var lines = File.ReadAllLines("input.txt");
    var lines2 = lines.Select(l => l.ToCharArray()).ToArray();
    (int x, int y) E = (-1, -1); bool foundE = false;
    (int x, int y) S = (-1, -1); bool foundS = false;
    for (int y = 0; y < lines.Length; y++)
    {

        if (!foundE)
        {
            int ee = lines[y].IndexOf('E');
            if (ee > -1)
            {
                E = (ee, y);
                foundE = true;
            }
        }
        if (!foundS)
        {
            int ss = lines[y].IndexOf('S');
            if (ss > -1)
            {
                S = (ss, y);
                foundS = true;
            }
        }
        if (foundE&&foundS)
            break;
    }
    Stack<((int x, int y) pos, (int x, int y) dir, int costSoFar, List<(int x, int y)> visited)> heads = new();
    Dictionary<(int x, int y), long> cache = new();
    heads.Push((S, (1,0), 0, new()));
    cache.Add(S, 0);
    var bestSoFar = int.MaxValue;
    List<(int x, int y)> onBestPathList = new();
    while(heads.Count>0)
    {
        var head = heads.Pop();
        if (head.pos==E)
        {
            if (head.costSoFar < bestSoFar)
            {
                bestSoFar = head.costSoFar;
                onBestPathList = new(head.visited);
            } else if (head.costSoFar == bestSoFar)
            {
                onBestPathList.AddRange(head.visited);
            }
            continue;
        }
        var around = new (int x, int y)[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
        foreach(var move in around)
        {
            int xx=head.pos.x+move.x;
            int yy=head.pos.y+move.y;
            if (!(lines[yy][xx] == '.' || lines[yy][xx] == 'E'))
            {
                continue;
            }
            int newCost=head.costSoFar+1+turnCost(head.dir,move);
            if (cache.TryGetValue((xx, yy), out var oldCost) && oldCost < newCost - 1000)
            {
                continue;
            } else cache[(xx,yy)] = newCost;
            var newList = new List<(int x, int y)>(head.visited);
            newList.Add((xx,yy));
            heads.Push((pos:(xx, yy), dir:move, costSoFar:newCost, visited:newList));
        }
        
        //lines2[head.pos.y][head.pos.x] = 'O';
    }
    for(int i=0; i<lines2.Length; i++)
        Console.WriteLine(lines2[i]);
    Console.WriteLine("Part 2:"+(onBestPathList.ToHashSet().Count+1));
}

int turnCost((int x, int y) dir1, (int x, int y) dir2)
{
    if (dir1==dir2)
        return 0;
    (int x, int y)[] directions = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
    const int up=0, right=1, down=2, left=3;
    int dirMin = Math.Min(Array.IndexOf(directions, dir1), Array.IndexOf(directions, dir2));
    int dirMax = Math.Max(Array.IndexOf(directions, dir1), Array.IndexOf(directions, dir2));
    return (dirMin, dirMax) switch
    {
        (up, right) => 1000,
        (up, left) => 1000,
        (up, down) => 2000,
        (right, down) => 1000,
        (right, left) => 2000,
        (down, left) => 1000,
        _ => throw new ArgumentException("turnCost")
    };
    // var dirs = new (int x, int y)[] {(0, -1), (1, 0), (0, 1), (-1, 0)};
    // var dir1Index = Array.IndexOf(dirs, dir1);
    // var dir2Index = Array.IndexOf(dirs, dir2);
    // if (dir1Index < 0 || dir2Index < 0)
    //     throw new ArgumentException("turnCost");
    // var lo = Math.Min(dir1Index, dir2Index);
    // var hi = Math.Max(dir1Index, dir2Index);
    // var d = hi-lo % 3;
    // return d*1000;
}

