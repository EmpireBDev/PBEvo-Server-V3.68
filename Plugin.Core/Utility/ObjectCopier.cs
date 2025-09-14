using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Plugin.Core.Utility
{
    public static class ObjectCopier
    {
        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }
            return (type.IsValueType & type.IsPrimitive);
        }
        public static object Copy(this object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<object, object>(new REComparer()));
        }
        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null)
            {
                return null;
            }
            Type typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect))
            {
                return originalObject;
            }
            if (visited.ContainsKey(originalObject))
            {
                return visited[originalObject];
            }
            if (typeof(Delegate).IsAssignableFrom(typeToReflect))
            {
                return null;
            }
            object cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                Type arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }
        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }
        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false)
                {
                    continue;
                }
                if (IsPrimitive(fieldInfo.FieldType))
                {
                    continue;
                }
                object originalFieldValue = fieldInfo.GetValue(originalObject);
                object clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        public static T Copy<T>(this T original)
        {
            return (T)Copy((object)original);
        }
        public static T Clone<T>(T Source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }
            if (Source == null)
            {
                return default;
            }
            IFormatter Formatter = new BinaryFormatter();
            Stream Stream = new MemoryStream();
            using (Stream)
            {
                Formatter.Serialize(Stream, Source);
                Stream.Seek(0, SeekOrigin.Begin);
                return (T)Formatter.Deserialize(Stream);
            }
        }
        private static object Process(object OBJ)
        {
            if (OBJ == null)
            {
                return null;
            }
            Type MType = OBJ.GetType();
            if (MType.IsValueType || MType == typeof(string))
            {
                return OBJ;
            }
            else if (MType.IsArray)
            {
                Type ElementType = Type.GetType(MType.FullName.Replace("[]", string.Empty));
                Array ArrayOBJ = OBJ as Array;
                Array CopiedArray = Array.CreateInstance(ElementType, ArrayOBJ.Length);
                for (int i = 0; i < ArrayOBJ.Length; i++)
                {
                    CopiedArray.SetValue(Process(ArrayOBJ.GetValue(i)), i);
                }
                return Convert.ChangeType(CopiedArray, OBJ.GetType());
            }
            else if (MType.IsClass)
            {
                object Toret = Activator.CreateInstance(OBJ.GetType());
                FieldInfo[] Fields = MType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo Field in Fields)
                {
                    object FieldValue = Field.GetValue(OBJ);
                    if (FieldValue == null)
                    {
                        continue;
                    }
                    Field.SetValue(Toret, Process(FieldValue));
                }
                return Toret;
            }
            else
            {
                throw new ArgumentException("Unknown type");
            }
        }
        public static T DeepCopy<T>(T Source)
        {
            if (Source == null)
            {
                throw new ArgumentNullException("Object cannot be null");
            }
            return (T)Process(Source);
        }
    }
    public class REComparer : EqualityComparer<object>
    {
        public override bool Equals(object X, object Y)
        {
            return ReferenceEquals(X, Y);
        }
        public override int GetHashCode(object OBJ)
        {
            if (OBJ == null)
            {
                return 0;
            }
            return OBJ.GetHashCode();
        }
    }
    public static class ArrayEXT
    {
        public static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0)
            {
                return;
            }
            ArrayTRV walker = new ArrayTRV(array);
            do action(array, walker.Position);
            while (walker.Step());
        }
    }
    internal class ArrayTRV
    {
        public int[] Position;
        private readonly int[] maxLengths;
        public ArrayTRV(Array array)
        {
            maxLengths = new int[array.Rank];
            for (int i = 0; i < array.Rank; ++i)
            {
                maxLengths[i] = array.GetLength(i) - 1;
            }
            Position = new int[array.Rank];
        }
        public bool Step()
        {
            for (int i = 0; i < Position.Length; ++i)
            {
                if (Position[i] < maxLengths[i])
                {
                    Position[i]++;
                    for (int j = 0; j < i; j++)
                    {
                        Position[j] = 0;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
