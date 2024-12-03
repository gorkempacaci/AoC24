/*
    Görkem Paçacı
    AoC24 day 2b
*/
public static class Program
{
    public static void Main()
    {
        var lines = File.ReadAllLines("input.txt").Select(l => l.Split(' ').Select(int.Parse).ToArray());
        int goodReports = lines.Count(l => isGoodReport(l, 1, 1));
        Console.WriteLine(goodReports);
    }
    /// <summary>
    /// Evaluates the levels rule around index j, considering the index triple (j-1, j, j+1)
    /// </summary>
    static bool isGoodReport(int[] levels, int j, int errorBudget)
    {
        if (errorBudget < 0)
            return false;
        if (j == levels.Length-1)
            return true;
        int deltaLeg1 = levels[j] - levels[j-1];
        int deltaLeg2 = levels[j+1] - levels[j];
        int deltaLegLong = levels[j+1] - levels[j-1];
        Func<int,bool> good = (d => d >= -3 && d <= 3 && d != 0);
        bool goodDeltaLeg1 = good(deltaLeg1), goodDeltaLeg2 = good(deltaLeg2), goodDeltaLongLeg = good(deltaLegLong);
        bool signsMatch = Math.Sign(deltaLeg1)==Math.Sign(deltaLeg2);
        Func<bool> proceed = () => isGoodReport(levels, j+1, errorBudget);
        Func<bool> popLeft = () => isGoodReport(levels, j+1, errorBudget-1);
        Func<bool> popMiddle = () => isGoodReport(shiftRight(levels, j-1, 1), j+1, errorBudget-1);
        Func<bool> popRight = () => isGoodReport(shiftRight(levels, j-1, 2), j+1, errorBudget-1);
        return (goodDeltaLeg1, goodDeltaLeg2, goodDeltaLongLeg, signsMatch) switch
        {
            /* 1, 2, 3 */ ( true,  true,     _, true ) => proceed(),
            /* 1, 3, 0 */ ( true,  true,  true, false) => popLeft() || popMiddle() || popRight(),
            /* 1, 4, 1 */ ( true,  true, false, false) => popLeft() || popRight(),
            /* 1, 4, 0 */ ( true, false,  true, _    ) => popMiddle() || popRight(),
            /* 1, 2, 9 */ ( true, false, false, _    ) => popRight(),
            /* 6, 2, 3 */ (false,  true,  true, _    ) => popLeft() || popMiddle(),
            /* 6, 2, 1 */ (false,  true, false, _    ) => popLeft(),
            /* 1, 9, 3 */ (false, false,  true, _    ) => popMiddle(),
            /* 1, 9, 5 */ (false, false, false, _    ) => false,
        };
    }
    /// <summary>
    /// In a new array, shift n elements starting at i by one to the right
    /// </summary>
    static int[] shiftRight(int[] ints, int i, int n)
    {
        int[] newInts = (int[])ints.Clone();
        for(;n>0;n--)
            newInts[i+n] = newInts[i+n-1];
        return newInts;
    }
}
