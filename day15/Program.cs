/*
    Görkem Paçacı
    AoC24 day 15
*/
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;


Stopwatch sp = new Stopwatch();
sp.Start();
//part1();
sp.Stop();
Console.WriteLine("time: " + sp.ToString());
sp.Restart();
part2();
sp.Stop();
Console.WriteLine("time:" + sp.ToString());

// void part1()
// {
//     var lines = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
//     int linesLength = -1;
//     (int x, int y) robPos = (-1, -1);
//     for(int i=0; i<lines.Length; i++)
//     {
//         int pos = Array.IndexOf(lines[i], '@');
//         if (pos > -1)
//             robPos = (pos, i);
//         if (lines[i].Length==0)
//         {
//             linesLength = i;
//             break;
//         }
//     }
//     string moves = lines.Skip(linesLength).Select(cs => new string(cs)).Aggregate(string.Concat);
//     foreach(char move in moves)
//         push(lines, ref robPos, move);
//     long totDistance = 0;
//     for(int i=0; i<linesLength; i++)
//     {
//         for(int x=0; x<lines[i].Length; x++)
//         {
//             if (lines[i][x]=='O')
//                 totDistance += i*100+x;
//         }
//     }
//     Console.WriteLine("Part 1:"+totDistance);
// }

// bool push(char[][] lines, ref (int x, int y) pos, char op)
// {
//     (int x, int y) dir = op switch
//     {
//         '<' => (-1, 0),
//         '>' => (1, 0),
//         'v' => (0, 1),
//         '^' => (0, -1),
//         _ => throw new Exception("Not known move:"+op)
//     };
//     int xx=pos.x + dir.x, yy=pos.y+dir.y;
//     if (lines[yy][xx] == '#')
//         return false;
//     if (lines[yy][xx] == '.')
//     {
//         lines[yy][xx] = lines[pos.y][pos.x];
//         lines[pos.y][pos.x] = '.';
//         pos = (xx, yy);
//         return true;
//     }
//     if (lines[yy][xx] == 'O')
//     {
//         (int x, int y) dummy = (xx,yy);
//         if (push(lines, ref dummy, op))
//         {
//             lines[yy][xx] = lines[pos.y][pos.x];
//             lines[pos.y][pos.x] = '.';
//             pos = (xx, yy);
//             return true;
//         } else return false;
//     }
//     throw new NotImplementedException("Unhandled target box:" + lines[yy][xx]);
// }

void part2()
{
    var linesI = File.ReadAllLines("input.txt").ToArray();
    int linesLength = -1;
    (int x, int y) robPos = (-1, -1);
    for(int i=0; i<linesI.Length; i++)
    {
        if (linesLength == -1)
        {
            linesI[i] = linesI[i].Replace(".", "..").Replace("@", "@.").Replace("#","##").Replace("O", "[]");
        }
        int pos = linesI[i].IndexOf('@');
        if (pos > -1)
            robPos = (pos, i);
        if (linesI[i].Length==0)
        {
            linesLength = i;
            break;
        }
    }
    char[][] lines = linesI.Select(l => l.ToCharArray()).ToArray();
    // foreach(var line in lines)
    //     Console.WriteLine(line);
    string moves = lines.Skip(linesLength).Select(cs => new string(cs)).Aggregate(string.Concat);
    printLines(lines, robPos);
    foreach(char move in moves)
    {
        evacuate(lines, ref robPos, move);
        printLines(lines, robPos);
    }
    long totDistance = 0;
    for(int i=0; i<linesLength; i++)
    {
        for(int x=0; x<lines[i].Length; x++)
        {
            if (lines[i][x]=='[')
                totDistance += i*100+x;
        }
    }
    printLines(lines, (0,0));
    Console.WriteLine("Part 2:"+totDistance);
}


// move whatever in pos in op direction
bool push2(char[][] lines, ref (int x, int y) pos, char op)
{
    char[][] linesBackup = lines.Select(l => new string(l).ToCharArray()).ToArray();
    (int x, int y) dir = op switch
    {
        '<' => (-1, 0),
        '>' => (1, 0),
        'v' => (0, 1),
        '^' => (0, -1),
        _ => throw new Exception("Not known move:"+op)
    };
    int mySO = lines[pos.y][pos.x] switch
    {
        '[' => 1,
        ']' => -1,
        _ => 0
    };

    int xx=pos.x + dir.x, yy=pos.y+dir.y;
    int targetSO = lines[yy][xx]==']'?-1:(lines[yy][xx]=='['?+1:0);

    if (lines[yy][xx] == '.' && lines[yy][xx+mySO] == '.')
    {
        lines[yy][xx] = lines[pos.y][pos.x];
        lines[yy][xx+mySO] = lines[pos.y][pos.x+mySO];
        lines[pos.y][pos.x] = '.';
        lines[pos.y][pos.x+mySO] = '.';
        pos = (xx, yy);
        return true;
    }

    if (lines[yy][xx] == '#' || lines[yy][xx+mySO] == '#')
        return false;



    if (mySO == 0)
    {
        if (evacuate(lines, ref pos, op))
        {
            lines[yy][xx] = lines[pos.y][pos.x];
            return true;
        } else return false;
    }
    else
    {
        (int x, int y) dummy = (xx,yy), dummy2 = (xx+mySO,yy);
        if (evacuate(lines, ref dummy2, op) && evacuate(lines, ref dummy, op))
        {
            lines[yy][xx+mySO] = lines[pos.y][pos.x+mySO];
            lines[yy][xx] = lines[pos.y][pos.x];
            //lines[pos.y][pos.x+pushSO] = '.';
            lines[pos.y][pos.x] = '.';
            pos = (xx, yy);
            checkValid(lines, linesBackup, dir);   
            return true;
        } else {
            for(int i=0; i<lines.Length; i++)
                lines[i]=linesBackup[i]; 
            return false;
        }
    }
    
    if ((lines[yy][xx] == '[' && lines[yy][xx+targetSO] == ']') || (lines[yy][xx] == ']' && lines[yy][xx+targetSO] == '['))
    {
        if (targetSO==0)
        {
            for(int i=0; i<lines.Length; i++)
                Console.WriteLine(lines[i]);
            Console.WriteLine("Error. pushSO=0.");
        }
        (int x, int y) dummy = (xx,yy), dummy2 = (xx+mySO,yy);

    }
    return false;
    //lines[pos.y][pos.x]='!';
    //printLines(lines, dir);
    //throw new NotImplementedException($"Unhandled {pos.y},{pos.x} myso:{mySO} dir:{dir} target box:" + lines[yy][xx] + lines[yy][xx+mySO]);
}

void checkValid(char[][] lines, char[][] backupLines, (int x, int y) dir)
{
    for(int y=0; y<lines.Length; y++)
    {
        if (lines[y].Length==0)
            break;
        for(int x=0; x<lines[y].Length; x++)
        {
            if ((lines[y][x]=='[' && lines[y][x+1]!=']') || (lines[y][x]==']' && lines[y][x-1]!='['))
            {
                for(int yy=0; yy<lines.Length; yy++) {
                    Console.Write(lines[yy]); Console.WriteLine(backupLines[yy]);
                    if (lines[yy].Length==0)
                        break;
                }
                Console.WriteLine("Validation error. Dir:" + dir);
                Environment.Exit(-1);
            }
        }
    }
}


void printLines(char[][] lines, (int x, int y) dir)
{
    for(int y=0; y<lines.Length; y++)
    {
        if (lines[y].Length==0)
            break;
        Console.WriteLine(lines[y]);
    }
}