/*
    Görkem Paçacı
    AoC24 day 12
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
    var lines = File.ReadAllLines("input.txt");
    var region = makeRegionMap(lines);

    long totalPrice = 0;
    foreach (var gr in region)
    {
        int perimeterOfGroup = 0;
        foreach ((int x, int y) plant in gr.Value)
        {
            int myPerimeter = 4;
            for (int yy = -1; yy < 2; yy++)
            {
                for (int xx = -1; xx < 2; xx++)
                {
                    int tx = plant.x + xx, ty = plant.y + yy;
                    if (Math.Abs(xx) != Math.Abs(yy))
                    {
                        if (gr.Value.Contains((tx, ty)))
                            myPerimeter--;
                    }
                }
            }
            perimeterOfGroup += myPerimeter;
        }
        int area = gr.Value.Count;
        int price = area * perimeterOfGroup;
        totalPrice += price;
    }
    Console.WriteLine("Part 1:" + totalPrice);
}

void discoverAround(int x, int y, string[] lines, Dictionary<(int x, int y), int> regs,
             Dictionary<int, HashSet<(int x, int y)>> plants, int currentReg)
{
    if (x < 0 || x >= lines[0].Length || y < 0 || y >= lines.Length)
        return;
    for (int yy = -1; yy < 2; yy++)
    {
        for (int xx = -1; xx < 2; xx++)
        {
            int tx = x + xx, ty = y + yy;
            if (Math.Abs(xx) != Math.Abs(yy) &&
                tx >= 0 && tx < lines[y].Length && ty >= 0 && ty < lines.Length &&
                lines[y][x] == lines[ty][tx] &&
                !regs.ContainsKey((tx, ty)))
            {
                regs[(tx, ty)] = currentReg;
                plants[currentReg].Add((tx, ty));
                discoverAround(tx, ty, lines, regs, plants, currentReg);
            }
        }
    }
}

Dictionary<int, HashSet<(int x, int y)>> makeRegionMap(string[] lines)
{
    Dictionary<(int x, int y), int> pointToRegion = new();
    Dictionary<int, HashSet<(int x, int y)>> regionToPoint = new();
    int regCounter = 0;
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            if (!pointToRegion.ContainsKey((x, y)))
            {
                int newReg = regCounter++;
                pointToRegion[(x, y)] = newReg;
                regionToPoint[newReg] = new() { (x, y) };
                discoverAround(x, y, lines, pointToRegion, regionToPoint, newReg);
            }
        }
    }
    return regionToPoint;
}


void part2()
{
    var lines = File.ReadAllLines("input.txt");
    Dictionary<int, HashSet<(int x, int y)>> regions = makeRegionMap(lines);
    var totalPrice = 0;
    foreach (var reg in regions)
    {
        var area = reg.Value.GroupBy(p => p.y)
                            .OrderBy(g => g.Key)
                            .Select(g => g.Select(p => p.x).ToImmutableSortedSet())
                            .Sum(r => r.Count());
        int minx = reg.Value.Min(r => r.x);
        int miny = reg.Value.Min(r => r.y);
        int maxx = reg.Value.Max(r => r.x);
        int maxy = reg.Value.Max(r => r.y);
        int corners = 0;
        for (int y = miny; y <= maxy+1; y++)
        {
            for (int x = minx; x <= maxx+1; x++)
            {
                bool uL = reg.Value.Contains((x-1,y-1));
                bool uR = reg.Value.Contains((x, y-1));
                bool lL = reg.Value.Contains((x-1, y));
                bool lR = reg.Value.Contains((x,y));
                if (uL == uR && lL == lR) continue;
                if (uL == lL && uR == lR) continue;
                if (uL == lR && uR == lL) corners++;
                corners++;
            }
        }
        int price = area * corners;
        totalPrice += price;
    }
    Console.WriteLine("Part 2:" + totalPrice);

}


// void part2()
// {
// int lineLength = lines[0].Length;
// Dictionary<int, HashSet<(int x, int y)>> regions = makeRegionMap(lines);
// long totalPrice = 0;
// foreach(var reg in regions)
// {
//     var rows = reg.Value.GroupBy(p => p.y)
//                         .OrderBy(g => g.Key)
//                         .Select(g => g.Select(p => p.x).ToImmutableSortedSet())
//                         .ToArray();
//     var area = rows.Sum(r => r.Count());
//     ImmutableSortedSet<int> prevRow = new List<int>().ToImmutableSortedSet();
//     var theLastRow = rows.Last();
//     var cornersInThisRegion = 0;
//     foreach(var row in rows)
//     {
//         var cornersInThisRow = 0;
//         int prevX = int.MinValue;
//         foreach(int x in row)
//         {
//             if (prevX == x-1) // continue a group from x-1 to x
//             {
//                 if (prevRow.Contains(x-1) && !prevRow.Contains(x))
//                 {
//                     cornersInThisRow++; Console.Write($"[{x}]");
//                 }
//                 if (!prevRow.Contains(x-1) && prevRow.Contains(x))
//                 {
//                     cornersInThisRow++; Console.Write($"[{x}]");
//                 }
//             } else { // beginning a new group
//                 if (prevX == int.MinValue)
//                 {
//                     int aboveCornersUpToHere = countBottomCorners(prevRow, prevX+1, x-1);
//                     Console.Write("[c"+aboveCornersUpToHere+"]");
//                     cornersInThisRow += aboveCornersUpToHere;
//                 } else {
//                     // one for the previous groups end corner
//                     if (prevRow.Contains(prevX+1))
//                     {
//                         cornersInThisRow++;
//                         Console.Write($"[{prevX+1}]");
//                     }
//                 }
//                 if (prevRow.Contains(x-1) && !prevRow.Contains(x)) // [X ], [ X]
//                 {
//                     cornersInThisRow++; // for bottom corner of prev row
//                     Console.Write($"[{x}]");
//                 }
//                 if (!prevRow.Contains(x-1) && prevRow.Contains(x)) // [ X], [ X]
//                 {
//                     // vertical side continues, no corner
//                 } else {                                           // [XX], [ X]
//                     cornersInThisRow++; // for upper last corner of this row
//                     Console.Write($"[{x}]");
//                 }
//             }
//             prevX = x;
//         }
//         if (!prevRow.Contains(prevX))
//         {
//             cornersInThisRow++; // last upper corner of this row
//             Console.Write("[f]");
//         }
//         int cornersPrevRow = countBottomCorners(prevRow, prevX+1, lineLength);
//         if (cornersPrevRow > 0)
//             cornersInThisRow += cornersPrevRow + 1;
//         Console.WriteLine($"Row corners: {cornersInThisRow}");
//         cornersInThisRegion += cornersInThisRow;
//         prevRow = row;
//     }
//     int lastRowBottomCorners = countBottomCorners(prevRow, 0, lineLength) + 1; // bottom corners of last row
//     Console.WriteLine("Lastcorners: " + lastRowBottomCorners);
//     cornersInThisRegion += lastRowBottomCorners;
//     long price = area*cornersInThisRegion;
//     Console.WriteLine($"Area: {area}, Corners:{cornersInThisRegion}, Price:{price}");
//     totalPrice += price;
// }

// Console.WriteLine("Part 2:" + totalPrice);
// }

// int countBottomCorners(ImmutableSortedSet<int> thisRow, int lowBoundIncl, int upBoundIncl)
// {
//     int corners = 0;
//     var row = thisRow.Where(u => u>=lowBoundIncl && u<= upBoundIncl);
//     int prevX = int.MinValue;
//     foreach(var x in row)
//     {
//         if (prevX != x-1)
//             corners++;
//         prevX = x;
//     }
//     return corners;
// }