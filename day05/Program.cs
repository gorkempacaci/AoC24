/*
    Görkem Paçacı
    AoC24 day 4
*/

part1();
part2();

static void part1()
{
    // Part 1
    var lines = File.ReadAllLines("input.txt").ToArray();
    Dictionary<int, SortedSet<int>> keyFirst = new();
    int lineNumber = 0;
    for(; lineNumber<lines.Length; lineNumber++)
    {
        var line = lines[lineNumber];
        if (line.Length==0)
            break;
        var arr = line.Split("|");
        var first = int.Parse(arr[0]);
        var later = int.Parse(arr[1]);
        // add this order element
        if (keyFirst.TryGetValue(first, out var lst))
            lst.Add(later);
        else keyFirst.Add(first, new SortedSet<int>(){later});
        // redundancy for transitivity
        // foreach(var list in keyFirst.Values)
        // {
        //     if (list.Contains(first))
        //         list.Add(later);
        // }
        // now keyFirst has not only a<b, b<c, c<d, but it looks like a<b,c,d b<c,d c<d
    }
    lineNumber++;
    int middlesTotal = 0;
    for(; lineNumber<lines.Length; lineNumber++)
    {
        var line = lines[lineNumber];
        var pages = line.Split(",").Select(int.Parse).ToArray();
        for(int i=0; i<pages.Length-1; i++)
        {
            for(int j=i+1; j<pages.Length; j++)
            {
                if (keyFirst.TryGetValue(pages[j], out var shouldComeAfterJ))
                    if (shouldComeAfterJ.Contains(pages[i]))
                    {
                        // Console.WriteLine(lineNumber+" bad. "+pages[j]+"<"+pages[i]+"Because:");
                        // Console.WriteLine(pages[j]+" < "+string.Join(",", shouldComeAfterJ));
                        goto thisLineNoGood;
                    }
            }
        }
        var middle = pages[pages.Length/2];
        middlesTotal+=middle;
        thisLineNoGood:;
    }
    Console.WriteLine("Part 1:"+middlesTotal);
}



static void part2()
{
    // Part 1
    var lines = File.ReadAllLines("input.txt").ToArray();
    Dictionary<int, SortedSet<int>> keyFirst = new();
    int lineNumber = 0;
    for(; lineNumber<lines.Length; lineNumber++)
    {
        var line = lines[lineNumber];
        if (line.Length==0)
            break;
        var arr = line.Split("|");
        var first = int.Parse(arr[0]);
        var later = int.Parse(arr[1]);
        // add this order element
        if (keyFirst.TryGetValue(first, out var lst))
            lst.Add(later);
        else keyFirst.Add(first, new SortedSet<int>(){later});
    }
    lineNumber++;
    int middlesTotal = 0;
    for(; lineNumber<lines.Length; lineNumber++)
    {
        var line = lines[lineNumber];
        var pages = line.Split(",").Select(int.Parse).ToArray();
        bool badLine = false;
        tryAgain:
        for(int i=0; i<pages.Length-1; i++)
        {
            for(int j=i+1; j<pages.Length; j++)
            {
                if (keyFirst.TryGetValue(pages[j], out var shouldComeAfterJ))
                    if (shouldComeAfterJ.Contains(pages[i]))
                    {
                        (pages[i], pages[j]) = (pages[j], pages[i]);
                        badLine = true;
                        goto tryAgain;
                    }
            }
        }
        if (badLine)
        {
            var middle = pages[pages.Length/2];
            middlesTotal+=middle;
        }
    }
    Console.WriteLine("Part 1:"+middlesTotal);
}
