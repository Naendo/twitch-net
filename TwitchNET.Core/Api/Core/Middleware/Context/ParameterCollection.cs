using System.Collections;
using System.Collections.Generic;

namespace TwitchNET.Core.Middleware
{
    public class ParameterCollection : IReadOnlyList<object>
    {
        internal ParameterCollection(object[] values)
        {
            Values = values;
        }

        internal object[] Values { get; }

        public IEnumerator<object> GetEnumerator()
        {
            return (IEnumerator<object>) Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Values.Length;

        public object this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }
    }
}