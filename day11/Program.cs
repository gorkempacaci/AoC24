/*
    Görkem Paçacı
    AoC24 day 11
*/
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Xml.Schema;

Stopwatch sp = new Stopwatch();
sp.Start();
part1();
sp.Stop();
Console.WriteLine("time: "+sp.ToString());
sp.Restart();
part2();
sp.Stop();
Console.WriteLine("time:"+sp.ToString());


void part1()
{
    var nums = File.ReadAllText("input.txt").Split(' ').Select(decimal.Parse).ToList();
    var lst = new List<decimal>();
    for (int i = 0; i < 25; i++)
    {
        foreach (var n in nums)
        {
            if (n == 0)
                lst.Add(1);
            else if (n.ToString().Length % 2 == 0)
            {
                string s = n.ToString();
                var s1 = s.Substring(0, s.Length/2);
                var s2 = s.Substring(s.Length/2, s.Length/2);
                lst.Add(decimal.Parse(s1));
                lst.Add(decimal.Parse(s2));
            }
            else lst.Add(n * 2024);
        }
        nums = lst;
        lst = new();
    }
    Console.WriteLine("Part 1:" + nums.Count);
}


void part2()
{
    List<long> nums = File.ReadAllText("input.txt").Split(' ').Select(s => long.Parse(s)).ToList();
    SafeDictionary<long,long> dict = new();
    foreach(var n in nums)
        dict[n] = 1;

    for (int i = 0; i < 75; i++)
    {
        SafeDictionary<long,long> newDict = new();
        foreach (var n in dict.Keys)
        {
            if (n == 0)
                newDict[1] += dict[0];
            else
            {
                int digitsN = (int)Math.Floor(Math.Log10(n))+1;
                if (digitsN%2==0)
                {
                    long halfWayZeroed = (long)Math.Pow(10, digitsN/2);
                    long number2 = n % halfWayZeroed;
                    long number1 = (n - number2)/halfWayZeroed;
                    newDict[number1] += dict[n];
                    newDict[number2] += dict[n];
                }
                else newDict[n*2024] += dict[n];
            }
        }
        dict=newDict;
        //Console.WriteLine("Step "+i+", Count:"+logs.Count);
    }
    Console.WriteLine("Part 2:" + dict.Values.Sum());
}

public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey:notnull where TValue:notnull 
{
    public new TValue this[TKey key]
    {
        get
        {
            if (base.TryGetValue(key, out TValue val))
                return val;
            else return default!;
        }
        set
        {
            base[key] = value;
        }
    }
}