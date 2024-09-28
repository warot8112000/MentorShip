using System.Collections.Generic;

namespace LinqTest
{
    /*
     LinQ là một lớp tĩnh (static class), nghĩa là lớp này chỉ chứa các phương thức tĩnh 
     và không thể tạo đối tượng từ nó.Lớp này giống như một tiện ích (utility) để 
     giúp khởi tạo các đối tượng của lớp Linq<T>.
    */
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

       public Linq<TResult> Select<TResult>(Func<T, TResult> slelector)
        {
            List<TResult> result = new List<TResult>();
            foreach (var item in elements)
            {
                result.Add(slelector(item));
            }
            return new Linq<TResult>(result);
        }

        public T First(Func<T, bool> predicate)
        {
            foreach(var item in elements)
            {
                if(predicate(item))
                {
                    return item;
                }
            }
            throw new InvalidOperationException("No element satisfies the condition.");
        }

    }
}