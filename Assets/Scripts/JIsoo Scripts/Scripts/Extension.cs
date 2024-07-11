using System.Collections.Generic;

public static class Extension
{
    /// <summary>
    /// ¼ÅÇÃ ¸Þ¼­µå
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="units"></param>
    public static void Shuffle<T>(this List<T> units)
    {
        System.Random random = new System.Random();

        for (int i = units.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            T temp = units[k];
            units[k] = units[i];
            units[i] = temp;
        }
    }
}
