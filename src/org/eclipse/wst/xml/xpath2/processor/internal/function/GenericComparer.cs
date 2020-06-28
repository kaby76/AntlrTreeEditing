using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace xpath.org.eclipse.wst.xml.xpath2.processor.@internal.function
{
    public class GenericIComparer<T> : IComparer<T>, System.Collections.IComparer
    {
        private readonly static Dictionary<Type, Dictionary<Tuple<string, Type[]>, RuntimeMethodHandle>> comparers =
            new Dictionary<Type, Dictionary<Tuple<string, Type[]>, RuntimeMethodHandle>>();

        private MethodBase _handle;

        private GenericIComparer(MethodBase handle)
        {
            _handle = handle;
        }

        public static GenericIComparer<T> GetComparer<T>(string propertyName)
        {
            if (!comparers.ContainsKey(typeof(T)))
                comparers.Add(typeof(T), new Dictionary<Tuple<string, Type[]>, RuntimeMethodHandle>());
            if (!comparers[typeof(T)].ContainsKey(new Tuple<string, Type[]>(propertyName, new Type[0])))
                comparers[typeof(T)].Add(new Tuple<string, Type[]>(propertyName, new Type[0]),
                    typeof(T).GetProperty(propertyName).GetGetMethod().MethodHandle);
            return (GenericIComparer<T>)
                new GenericIComparer<T>(MethodInfo.GetMethodFromHandle(comparers[typeof(T)][new Tuple<string, Type[]>(propertyName, new Type[0])]));
        }

        public static GenericIComparer<T> GetComparer<T>(string propertyName, Type[] args)
        {
            var type_of_t = typeof(T);
            if (!comparers.ContainsKey(type_of_t))
                comparers.Add(type_of_t, new Dictionary<Tuple<string, Type[]>, RuntimeMethodHandle>());
            var x = comparers.TryGetValue(type_of_t, out Dictionary<Tuple<string, Type[]>, RuntimeMethodHandle> foo);
            if (foo == null)
                throw new Exception();
            if (!foo.ContainsKey(new Tuple<string, Type[]>(propertyName, args)))
                foo.Add(new Tuple<string, Type[]>(propertyName, args),
                    typeof(T).GetMethod(propertyName, args).MethodHandle);
            return (GenericIComparer<T>)
                new GenericIComparer<T>(MethodInfo.GetMethodFromHandle(comparers[typeof(T)][new Tuple<string, Type[]>(propertyName, args)]));
        }

        public int Compare(T x, T y)
        {
            object[] args = new object[2] { x, y };
            var result = (int)_handle.Invoke(this, args);
            return result;
        }


        public int Compare(object x, object y)
        {
            object[] args = new object[2] { x, y };
            var result = (int)_handle.Invoke(this, args);
            return result;
        }
    }
}

