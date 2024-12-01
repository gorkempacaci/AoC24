/*
    Görkem Paçacı
    2024-12-01 AoC day 1
*/
using System.Collections;
List<int> left = new(), right = new();
foreach(var line in File.ReadAllLines("input.txt"))
{
    var lineArr = line.Split("   ").Select(int.Parse).ToArray();
    left.Add(lineArr[0]);
    right.Add(lineArr[1]);
}
left.Sort();
right.Sort();
var sum = left.Select(l => l * right.Count(r => r==l)).Sum();
Console.WriteLine(sum);