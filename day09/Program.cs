/*
    Görkem Paçacı
    AoC24 day 9
*/

class Program
{
    static void Main(string[] args)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        part1();
        watch.Stop();
        Console.WriteLine("Part1 took " + watch.ElapsedMilliseconds + "ms");
        watch.Restart();
        part2();
        watch.Stop();
        Console.WriteLine("Part2 took " + watch.ElapsedMilliseconds + "ms");
    }
    static void part1()
    {
        var nums = File.ReadAllText("input.txt").Select(c => int.Parse(c.ToString())).ToArray();
        LinkedList<(int id, int len)> blocks = new(); //id, length
        var id = 0;
        var lastBlock = blocks.AddFirst((id++, nums[0]));
        bool isFile = false;
        for (int i = 1; i < nums.Length; i++)
        {
            if (isFile)
                lastBlock = blocks.AddAfter(lastBlock, (id++, nums[i]));
            else
                lastBlock = blocks.AddAfter(lastBlock, (-1, nums[i]));
            isFile = !isFile;
        }
        var firstSpace = blocks.First!.Next!;
        while (true)
        {
            if (firstSpace.Value.len < lastBlock!.Value.len)
            {
                firstSpace.Value = (lastBlock.Value.id, firstSpace.Value.len);
                lastBlock.Value = (lastBlock.Value.id, lastBlock.Value.len - firstSpace.Value.len);
                if (firstSpace.Next is not null && firstSpace.Next.Next is not null)
                    firstSpace = firstSpace.Next.Next;
                else break;
            }
            else if (firstSpace.Value.len > lastBlock.Value.len)
            {
                firstSpace.Value = (firstSpace.Value.id, firstSpace.Value.len - lastBlock.Value.len);
                blocks.AddBefore(firstSpace, (lastBlock.Value.id, lastBlock.Value.len));
                if (lastBlock.Previous != firstSpace && lastBlock.Previous!.Previous != firstSpace)
                    lastBlock = lastBlock.Previous.Previous;
                else break;
                blocks.Remove(lastBlock!.Next!.Next!); // last data
                blocks.Remove(lastBlock!.Next!); // last space
            }
            else if (firstSpace.Value.len == lastBlock.Value.len)
            {
                firstSpace.Value = (lastBlock.Value.id, lastBlock.Value.len);
                if (lastBlock.Previous != firstSpace && lastBlock.Previous!.Previous != firstSpace)
                    lastBlock = lastBlock.Previous.Previous;
                else break;
                blocks.Remove(lastBlock!.Next!.Next!); // last data
                blocks.Remove(lastBlock!.Next!); // last space
                if (firstSpace.Next is not null && firstSpace.Next != lastBlock &&
                    firstSpace.Next.Next is not null)
                    firstSpace = firstSpace.Next.Next;
                else break;
            }
        }
        lastBlock = blocks.Last;
        if (lastBlock!.Value.id == lastBlock.Previous!.Value.id) // group last chunks
        {
            lastBlock.Previous.Value = (lastBlock.Value.id, lastBlock.Value.len + lastBlock.Previous.Value.len);
            blocks.Remove(lastBlock);
        }
        long checksum = 0;
        int position = 0;
        var point = blocks.First;
        while (true)
        {
            for (int i = 0; i < point!.Value.len; i++)
                checksum += position++ * point!.Value.id;
            if (point == blocks.Last)
                break;
            else point = point.Next;
        }
        Console.WriteLine("Part 1:" + checksum);
    }


    static void part2()
    {
        var nums = File.ReadAllText("input.txt").Select(c => int.Parse(c.ToString())).ToArray();
        LinkedList<(int pos, int id, int len)> blocks = new(); //id, length
        List<LinkedListNode<(int pos, int id, int len)>> spaces = new();
        var id = 0;
        var lastBlock = blocks.AddFirst((0, id++, nums[0]));
        bool isFile = false;
        for (int i = 1; i < nums.Length; i++)
        {
            if (isFile)
                lastBlock = blocks.AddAfter(lastBlock, (i, id++, nums[i]));
            else
            {
                lastBlock = blocks.AddAfter(lastBlock, (i, -1, nums[i]));
                spaces.Add(lastBlock);
            }
            isFile = !isFile;
        }
        spaces.Sort((a, b) => a.Value.pos.CompareTo(b.Value.pos));
        //printBlocks2(blocks);
        while (lastBlock is not null)
        {
            //Console.Write(lastBlock.Value.pos+",");
            var space = claimFirstSpace(ref spaces, lastBlock.Value.pos, lastBlock.Value.len);
            var previousFile = getPreviousFile(lastBlock);
            if (space is not null)
            {
                //Console.WriteLine(lastBlock.Value.id + "*" + lastBlock.Value.len + " moves to pos " + space.Value.pos);
                space.Value = (space.Value.pos, lastBlock.Value.id, lastBlock.Value.len);
                deleteAndRespace(lastBlock);
                //printBlocks2(blocks);
            }
            lastBlock = previousFile;
        }
        // Console.WriteLine();
        // if(blocks.Last!.Value.id == -1) // group last chunks
        // {
        //     blocks.Remove(blocks.Last);
        // }
        long checksum = 0;
        int position = 0;
        var point = blocks.First;
        while (point != null)
        {
            if (point.Value.id != -1)
            {
                for (int i = 0; i < point!.Value.len; i++)
                    checksum += position++ * point!.Value.id;
            }
            else position += point.Value.len;
            point = point.Next;
        }
        //printBlocks2(blocks);
        Console.WriteLine("Part 2:" + checksum);
    }

    static Dictionary<int, int> lastSeekPositionByLength = new Dictionary<int, int>();
    static LinkedListNode<(int pos, int id, int len)> claimFirstSpace(ref List<LinkedListNode<(int pos, int id, int len)>> spacesDesc, int maxPos, int len)
    {
        int i = 0;
        if (!lastSeekPositionByLength.TryGetValue(len, out i))
            lastSeekPositionByLength.Add(len, 0);

        for (; i < spacesDesc.Count; i++)
        {
            var s = spacesDesc[i];
            if (s.Value.len >= len && s.Value.id == -1)
            {
                lastSeekPositionByLength[len] = i;
                if (s.Value.pos < maxPos)
                {
                    if (s.Value.len > len)
                    {
                        var unusedSpace = s.List!.AddAfter(s, (s.Value.pos, -1, s.Value.len - len));
                        s.Value = (s.Value.pos, -1, len);
                        spacesDesc[i] = unusedSpace;
                    }
                    else spacesDesc[i].Value = (s.Value.pos, -2, s.Value.len);
                    //spacesDesc = spacesDesc.OrderBy(s => s.Value.pos).ToList();

                    return s;
                }
            }
        }
        lastSeekPositionByLength[len] = spacesDesc.Count;
        return null!;
    }

    static void deleteAndRespace(LinkedListNode<(int pos, int id, int len)> node)
    {
        node.Value = (node.Value.pos, -1, node.Value.len);
        if (node.Next is not null && node.Next.Value.id == -1)
        {
            node.Value = (node.Value.pos, -1, node.Value.len + node.Next.Value.len);
            node.List!.Remove(node.Next);
        }
        if (node.Previous is not null && node.Previous.Value.id == -1)
        {
            node.Previous.Value = (node.Previous.Value.pos, -1, node.Previous.Value.len + node.Value.len);
            node.List!.Remove(node);
        }
    }

    static LinkedListNode<(int pos, int id, int len)> getPreviousFile(LinkedListNode<(int pos, int id, int len)> node)
    {
        if (node.Previous is not null && node.Previous.Value.id == -1)
            return node.Previous.Previous!;
        else
            return node.Previous!;
    }

    static void printBlocks(LinkedList<(int id, int len)> blocks)
    {
        for (var point = blocks.First; point is not null; point = point.Next)
        {
            for (int i = 0; i < point.Value.len; i++)
                if (point.Value.id == -1)
                    Console.Write(".");
                else
                    Console.Write(point.Value.id.ToString());
        }
        Console.WriteLine();
    }

    static void printBlocks2(LinkedList<(int pos, int id, int len)> blocks)
    {
        for (var point = blocks.First; point is not null; point = point.Next)
        {
            //Console.Write("(");
            for (int i = 0; i < point.Value.len; i++)
                if (point.Value.id == -1)
                    Console.Write(".");
                else
                    Console.Write(point.Value.id.ToString());
            //Console.Write(")");        
        }
        Console.WriteLine();
    }
}