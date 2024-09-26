public static class LinQ
{
    public static Linq<T> From<T>(List<T> elements)
    {
        return new Linq<T>(elements);
    }
}

public class Linq<T>
{
    public List<T> elements;

    public Linq(List<T> elements)
    {
        this.elements = elements;
    }

    public Linq<T> Where(Func<T, bool> predicate)
    {
        List<T> filteredElements = new List<T>();
        foreach (var element in elements)
        {
            if (predicate(element))
            {
                filteredElements.Add(element);
            }
        }
        return new Linq<T>(filteredElements);
    }

    public List<T> ToList()
    {
        return new List<T>(elements);
    }

    public Linq<T> OrderBy<TKey>(Func<T, TKey> keySelector) where TKey : IComparable<TKey>
    {
        var sortedList = new List<T>(elements);

        sortedList.Sort((x, y) =>
        {
            TKey keyX = keySelector(x);
            TKey keyY = keySelector(y);
            return keyX.CompareTo(keyY);
        });
        return new Linq<T>(sortedList);
    }

    public Linq<T> OrderByDescending<TKey>(Func<T, TKey> keySelector) where TKey : IComparable<TKey>
    {
        var sortedList = new List<T>(elements);
        sortedList.Sort((x, y) => keySelector(y).CompareTo(keySelector(x)));
        return new Linq<T>(sortedList);
    }

}