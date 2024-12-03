/*
    Görkem Paçacı
    AoC24 day 2
*/

var lines = File.ReadAllLines("input.txt").Select(l => l.Split(' ').Select(int.Parse));
var linesOfPairs = lines.Select(levels => levels.Zip(levels.Skip(1)));
var linesOfDeltas = linesOfPairs.Select(pairs => pairs.Select(pair => pair.Second - pair.First));
var linesOfGoodDeltas = linesOfDeltas.Where(deltas => deltas.All(d => 1<=Math.Abs(d) && Math.Abs(d)<=3));
var linesOfSigns = linesOfGoodDeltas.Select(deltas => deltas.Select(d => Math.Sign(d)));
var linesMonotonic = linesOfSigns.Where(signs => signs.Distinct().Count()==2);

Console.WriteLine(linesMonotonic.Count());

