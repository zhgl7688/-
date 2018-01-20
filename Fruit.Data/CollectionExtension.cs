using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public static class CollectionExtension
    {
        public static bool Remove<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            foreach (var item in collection)
            {
                if (condition(item))
                {
                    collection.Remove(item);
                    return true;
                }
            }
            return false;
        }


        public static int Removes<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            var delList = new List<T>();
            foreach (var item in collection)
            {
                if (condition(item))
                {
                    delList.Add(item);
                }
            }
            int count = 0;
            foreach (var delItem in delList)
            {
                if (collection.Remove(delItem))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
