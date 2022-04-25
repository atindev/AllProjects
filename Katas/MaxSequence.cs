using System.Linq;

public static class KataMaxSequence
{
    public static int MaxSequence(int[] arr)
    {
        int max = 0;
        if (arr?.Any() == true)
        {
            //TODO : create code
            foreach (int item in Enumerable.Range(1, arr.Length))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    int sums = arr.Take(item).Skip(i).Sum();
                    max = sums > max ? sums : max;
                }
            }
        }
        return max;
    }
}