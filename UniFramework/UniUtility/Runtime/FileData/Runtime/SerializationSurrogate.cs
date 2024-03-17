using System.Runtime.Serialization;
using UnityEngine;

namespace UniFramework.Utility.FileData
{
    public sealed class Vector2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2 vector2 = (Vector2)obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 vector2 = (Vector2)obj;
            vector2.x = info.GetSingle("x");
            vector2.y = info.GetSingle("y");
            return vector2;
        }
    }
    public sealed class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 vector2 = (Vector3)obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
            info.AddValue("z", vector2.z);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 vector2 = (Vector3)obj;
            vector2.x = info.GetSingle("x");
            vector2.y = info.GetSingle("y");
            vector2.z = info.GetSingle("z");
            return vector2;
        }
    }
    public sealed class Vector4SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector4 vector2 = (Vector4)obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
            info.AddValue("z", vector2.z);
            info.AddValue("w", vector2.w);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector4 vector2 = (Vector4)obj;
            vector2.x = info.GetSingle("x");
            vector2.y = info.GetSingle("y");
            vector2.z = info.GetSingle("z");
            vector2.w = info.GetSingle("w");
            return vector2;
        }
    }
    public sealed class Vector2IntSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2Int vector2 = (Vector2Int)obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2Int vector2 = (Vector2Int)obj;
            vector2.x = info.GetInt32("x");
            vector2.y = info.GetInt32("y");
            return vector2;
        }
    }
    public sealed class Vector3IntSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3Int vector2 = (Vector3Int)obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
            info.AddValue("z", vector2.z);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3Int vector2 = (Vector3Int)obj;
            vector2.x = info.GetInt32("x");
            vector2.y = info.GetInt32("y");
            vector2.z = info.GetInt32("z");
            return vector2;
        }
    }
    public sealed class ColorSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color color = (Color)obj;
            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color color = (Color)obj;
            color.r = info.GetSingle("r");
            color.g = info.GetSingle("g");
            color.b = info.GetSingle("b");
            color.a = info.GetSingle("a");
            return color;
        }
    }
    public sealed class BoneWeightSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            BoneWeight weight = (BoneWeight)obj;
            info.AddValue("a", weight.weight0);
            info.AddValue("b", weight.weight1);
            info.AddValue("c", weight.weight2);
            info.AddValue("d", weight.weight3);

            info.AddValue("e", weight.boneIndex0);
            info.AddValue("f", weight.boneIndex1);
            info.AddValue("g", weight.boneIndex2);
            info.AddValue("h", weight.boneIndex3);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            BoneWeight weight = (BoneWeight)obj;

            weight.weight0 = info.GetSingle("a");
            weight.weight1 = info.GetSingle("b");
            weight.weight2 = info.GetSingle("c");
            weight.weight3 = info.GetSingle("d");

            weight.boneIndex0 = info.GetInt32("e");
            weight.boneIndex1 = info.GetInt32("f");
            weight.boneIndex2 = info.GetInt32("g");
            weight.boneIndex3 = info.GetInt32("h");

            return weight;
        }
    }
    public sealed class BoundsSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Bounds bounds = (Bounds)obj;
            info.AddValue("a", bounds.center.x);
            info.AddValue("b", bounds.center.y);
            info.AddValue("c", bounds.center.z);
            info.AddValue("d", bounds.size.x);
            info.AddValue("e", bounds.size.y);
            info.AddValue("f", bounds.size.z);
            info.AddValue("g", bounds.extents.x);
            info.AddValue("h", bounds.extents.y);
            info.AddValue("i", bounds.extents.z);
            info.AddValue("j", bounds.min.x);
            info.AddValue("k", bounds.min.y);
            info.AddValue("l", bounds.min.z);
            info.AddValue("m", bounds.max.x);
            info.AddValue("n", bounds.max.y);
            info.AddValue("o", bounds.max.z);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Bounds bounds = (Bounds)obj;

            bounds.center = new Vector3(info.GetSingle("a"), info.GetSingle("b"), info.GetSingle("c"));
            bounds.size = new Vector3(info.GetSingle("d"), info.GetSingle("e"), info.GetSingle("f"));
            bounds.extents = new Vector3(info.GetSingle("g"), info.GetSingle("h"), info.GetSingle("i"));
            bounds.min = new Vector3(info.GetSingle("j"), info.GetSingle("k"), info.GetSingle("l"));
            bounds.max = new Vector3(info.GetSingle("m"), info.GetSingle("n"), info.GetSingle("o"));

            return bounds;
        }
    }
    public sealed class Matrix4x4SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Matrix4x4 matrix = (Matrix4x4)obj;
            info.AddValue("00", matrix.m00);
            info.AddValue("01", matrix.m01);
            info.AddValue("02", matrix.m02);
            info.AddValue("03", matrix.m03);

            info.AddValue("10", matrix.m10);
            info.AddValue("11", matrix.m11);
            info.AddValue("12", matrix.m12);
            info.AddValue("13", matrix.m13);

            info.AddValue("20", matrix.m20);
            info.AddValue("21", matrix.m21);
            info.AddValue("22", matrix.m22);
            info.AddValue("23", matrix.m23);

            info.AddValue("30", matrix.m30);
            info.AddValue("31", matrix.m31);
            info.AddValue("32", matrix.m32);
            info.AddValue("33", matrix.m33);
        }
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Matrix4x4 matrix = (Matrix4x4)obj;

            Vector4 row0 = new Vector4(info.GetSingle("00"), info.GetSingle("01"), info.GetSingle("02"), info.GetSingle("03"));
            Vector4 row1 = new Vector4(info.GetSingle("10"), info.GetSingle("11"), info.GetSingle("12"), info.GetSingle("13"));
            Vector4 row2 = new Vector4(info.GetSingle("20"), info.GetSingle("21"), info.GetSingle("22"), info.GetSingle("23"));
            Vector4 row3 = new Vector4(info.GetSingle("30"), info.GetSingle("31"), info.GetSingle("32"), info.GetSingle("33"));

            matrix.SetRow(0, row0);
            matrix.SetRow(1, row1);
            matrix.SetRow(2, row2);
            matrix.SetRow(3, row3);

            return matrix;
        }
    }
}