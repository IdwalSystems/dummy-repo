using System.Collections.Generic;

namespace MSNK.Models.Helper
{
    public class GroupHelper<K, T>
    {
        public K Key;
        public IEnumerable<T> Values;
    }
}
