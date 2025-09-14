using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Core.SharpDX
{
    public struct Half3
    {
        public Half X;
        public Half Y;
        public Half Z;
        public Half3(float x, float y, float z)
        {
            X = new Half(x);
            Y = new Half(y);
            Z = new Half(z);
        }
        public Half3(ushort x, ushort y, ushort z)
        {
            X = new Half(x);
            Y = new Half(y);
            Z = new Half(z);
        }
        public static implicit operator Half3(Vector3 value)
        {
            return new Half3(value.X, value.Y, value.Z);
        }
        public static implicit operator Vector3(Half3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }
    }
}
