/*
    Görkem Paçacı
    AoC24 day 6
*/
using System.Numerics;

part1();

void part1()
{
    var lines = File.ReadAllLines("input.txt")
        .Select(l => (long.Parse(l.Split(": ")[0]), l.Split(": ")[1].Split(' ').Select(long.Parse)));
    var sum = lines.Where(l => canMakeResult(l.Item1, -1, l.Item2)).Sum(l => l.Item1);
    Console.WriteLine(sum);
}

bool canMakeResult(long result, long currentResult, IEnumerable<long> nums)
{
    if (nums.Count()==0)
        return result==currentResult;
    if (currentResult == -1)
        return canMakeResult(result, nums.First(), nums.Skip(1));
    return canMakeResult(result, currentResult+nums.First(), nums.Skip(1)) ||
        canMakeResult(result, currentResult*nums.First(), nums.Skip(1)) ||
        canMakeResult(result, long.Parse(currentResult.ToString()+nums.First().ToString()), nums.Skip(1));
}
