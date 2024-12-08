/*
    Görkem Paçacı
    AoC24 day 6
*/

part1();

part2();

void part1()
{
    var lines = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
    Dictionary<char, HashSet<(int x, int y)>> antennas = new();
    HashSet<(int x, int y)> nodes = new();
    Func<(int x, int y), bool> inRange = point => point.x >= 0 && point.x < lines[0].Length && point.y >= 0 && point.y < lines.Length;
    for(int y=0; y<lines.Length; y++)
    {
        for(int x=0; x<lines[y].Length; x++)
        {
            char c = lines[y][x];
            if ((c >= '0' && c <= '9') || (c>='a' && c<= 'z') || (c>='A' && c<='Z'))
            {
                if (antennas.TryGetValue(c, out var others))
                {
                    foreach(var an in others)
                    {
                        //Console.Write((x,y)+"*"+an+":");
                        (int x, int y) ba = (an.x-x,an.y-y);
                        (int x, int y) nodeA = (x+(-ba.x), y+(-ba.y)); 
                        (int x, int y) nodeB = (an.x+ba.x, an.y+ba.y); 
                        if (inRange(nodeA))
                        {
                            nodes.Add(nodeA);
                            //Console.Write(nodeA);
                        }
                        if (inRange(nodeB))
                        {
                            nodes.Add(nodeB);
                            //Console.Write(nodeB);
                        }
                        //Console.WriteLine();
                    }
                    others.Add((x, y));
                } else antennas.Add(c, new(){ (x, y) });
            }
        }
    }
    foreach(var node in nodes)
        lines[node.y][node.x] = '#';
    foreach(var line in lines)
        Console.WriteLine(line);
    Console.WriteLine("Part1: "+nodes.Count());
}



void part2()
{
    var lines = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();
    Dictionary<char, HashSet<(int x, int y)>> antennas = new();
    HashSet<(int x, int y)> nodes = new();
    Func<(int x, int y), bool> inRange = point => point.x >= 0 && point.x < lines[0].Length && point.y >= 0 && point.y < lines.Length;
    for(int y=0; y<lines.Length; y++)
    {
        for(int x=0; x<lines[y].Length; x++)
        {
            char c = lines[y][x];
            if ((c >= '0' && c <= '9') || (c>='a' && c<= 'z') || (c>='A' && c<='Z'))
            {
                if (antennas.TryGetValue(c, out var others))
                {
                    foreach(var an in others)
                    {
                        //Console.Write((x,y)+"*"+an+":");
                        (int x, int y) ba = (an.x-x,an.y-y);
                        (int x, int y) point = (x,y);
                        while(true)
                        {
                            if (inRange(point))
                                nodes.Add(point);
                            else break;
                            point = (point.x - ba.x, point.y - ba.y);
                        }
                        point = (x+ba.x, y+ba.y);
                        while(true)
                        {
                            if (inRange(point))
                                nodes.Add(point);
                            else break;
                            point = (point.x + ba.x, point.y + ba.y);
                        }
                    }
                    others.Add((x, y));
                } else antennas.Add(c, new(){ (x, y) });
            }
        }
    }
    foreach(var node in nodes)
        lines[node.y][node.x] = '#';
    foreach(var line in lines)
        Console.WriteLine(line);
    Console.WriteLine("Nodes: "+nodes.Count());
}
