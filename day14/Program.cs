/*
    Görkem Paçacı
    AoC24 day 14
*/
using System.Collections.Immutable;
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
    // 47658600 too low
    var lines = File.ReadAllLines("input.txt");
    List<(int x, int y)> pos = new();
    List<(int x, int y)> vel = new();
    foreach(string line in lines)
    {
        var poss = line.Split(' ')[0].Split('=')[1].Split(',').Select(int.Parse).ToArray();
        var vels = line.Split(' ')[1].Split('=')[1].Split(',').Select(int.Parse).ToArray();
        pos.Add((poss[0], poss[1]));
        vel.Add((vels[0], vels[1]));
    }
    int width=101, height=103;
    for(int i=0; i<pos.Count; i++)
    {
        pos[i] = (((pos[i].x+vel[i].x*100+width*100) % width), ((pos[i].y+height+vel[i].y*100+height*100) % height));
    }
    int q1 = pos.Where(p => p.x < width/2 && p.y < height/2).Count();
    int q2 = pos.Where(p => p.x > width/2 && p.y < height/2).Count();
    int q3 = pos.Where(p => p.x < width/2 && p.y > height/2).Count();
    int q4 = pos.Where(p => p.x > width/2 && p.y > height/2).Count();
    Console.WriteLine($"Q1={q1}, Q2={q2}, Q3={q3}, Q4={q4}");
    Console.WriteLine($"Part 1:" + (q1*q2*q3*q4));
}



void part2()
{
    // 47658600 too low
    var lines = File.ReadAllLines("input.txt");
    List<(int x, int y)> pos = new();
    List<(int x, int y)> vel = new();
    foreach(string line in lines)
    {
        var poss = line.Split(' ')[0].Split('=')[1].Split(',').Select(int.Parse).ToArray();
        var vels = line.Split(' ')[1].Split('=')[1].Split(',').Select(int.Parse).ToArray();
        pos.Add((poss[0], poss[1]));
        vel.Add((vels[0], vels[1]));
    }
    int width=101, height=103;
    File.WriteAllText("easter.txt", "");
    for(int s=0; s<20000; s++)
    {
        char[][] screen = new char[height+1][];
        for(int h=0; h<height; h++)
            screen[h]= new string(' ',width).ToArray();
        for(int i=0; i<pos.Count; i++)
        {
            pos[i] = ((pos[i].x+width+vel[i].x) % width, (pos[i].y+height+vel[i].y) % height);
            screen[pos[i].y][pos[i].x] = '*';
        }
        screen[height-1] = ("Second " + (s+1) + " passed (result above).").ToArray();
        if (screen.Any(s => (new string(s)).Contains("********")))
            File.AppendAllLines("easter.txt", screen.Select(cs => new string(cs)));
    }
    int q1 = pos.Where(p => p.x < width/2 && p.y < height/2).Count();
    int q2 = pos.Where(p => p.x > width/2 && p.y < height/2).Count();
    int q3 = pos.Where(p => p.x < width/2 && p.y > height/2).Count();
    int q4 = pos.Where(p => p.x > width/2 && p.y > height/2).Count();
    Console.WriteLine($"Q1={q1}, Q2={q2}, Q3={q3}, Q4={q4}");
    Console.WriteLine($"Part 1:" + (q1*q2*q3*q4));
}


