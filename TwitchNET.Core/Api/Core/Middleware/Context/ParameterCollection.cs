using System.Collections;
using System.Collections.Generic;

namespace TwitchNET.Core
{
    public class ParameterCollection : IReadOnlyList<object>
    {
        internal object[] Values { get; }
        
        internal ParameterCollection(object[] values)
        {
            Values = values;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>) Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Values.Length;

        public object this[int index] => Values[index];
    }
}