using System;
using System.IO;
using UnityEngine;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace UniFramework.Utility.FileData
{
    public class FileHelper
    {
        private static BinaryFormatter _binaryFormatter;
        private static BinaryFormatter BinaryFormat
        {
            get
            {
                if (_binaryFormatter == null)
                {
                    SurrogateSelector surrogate = new SurrogateSelector();
                    StreamingContext context = new StreamingContext(StreamingContextStates.All);
                    surrogate.AddSurrogate(typeof(Vector2), context, new Vector2SerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Vector3), context, new Vector3SerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Vector4), context, new Vector4SerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Vector2Int), context, new Vector2IntSerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Vector3Int), context, new Vector3IntSerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Color), context, new ColorSerializationSurrogate());
                    surrogate.AddSurrogate(typeof(BoneWeight), context, new BoneWeightSerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Bounds), context, new BoundsSerializationSurrogate());
                    surrogate.AddSurrogate(typeof(Matrix4x4), context, new Matrix4x4SerializationSurrogate());

                    _binaryFormatter = new BinaryFormatter(surrogate, context);
                }
                return _binaryFormatter;
            }
        }

        /// <summary>
        /// 将target 存储到指定位置
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="crcCode">是否记录 该文件的Crc16CcittFalse 值（crcCode == null 则不记录）</param>
        /// <returns>文件地址 存储失败则返回string.Empty</returns>
        public static string ToBinary<T>(T target, string filePath)
        {
            return ToBinary(target, filePath);
        }

        public static string ToBinary<T>(T target, string filePath, bool outCRC, out string Crc16HEx)
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormat.Serialize(stream, target);

            return StreamToBinary(stream, filePath, outCRC, out Crc16HEx);
        }

        public static string ToBinary(byte[] bytes, string filePath)
        {
            return StreamToBinary(new MemoryStream(bytes), filePath);
        }

        public static string StreamToBinary(Stream stream, string filePath, bool outCRC = false)
        {
            return StreamToBinary(stream, filePath, false, out var _);
        }

        public static string StreamToBinary(Stream stream, string filePath, bool outCRC, out string Crc16HEx)
        {
            string direct = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);

            FileStream fileStream = File.Create(filePath);

            Crc16HEx = default;

            if (outCRC)
            {
                byte[] buffur = new byte[stream.Length];

                stream.Read(buffur, 0, (int)stream.Length);

                Crc16HEx = YooAsset.HashUtility.BytesCRC32(buffur);
            }

            CompressStream(stream, fileStream);

            fileStream.Close();

            stream.Dispose();

            if (File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                Debug.Log("数据保存失败");
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取类的二进制数组
        /// </summary>
        /// <returns>二进制数组</returns>
        public static byte[] GetBinary<T>(T target)
        {
            return GetBinary(target, true, out var _);
        }

        public static byte[] GetBinary<T>(T target, bool OutCRC, out string Crc16HEx)
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormat.Serialize(stream, target);

            Crc16HEx = default;
            if (OutCRC)
            {
                Crc16HEx = YooAsset.HashUtility.BytesCRC32(stream.GetBuffer());
            }

            var data = stream.GetBuffer();

            stream.Dispose();

            return data;
        }

        /// <summary>
        /// 获取目标文件的二进制数组(目标已经过压缩的)
        /// </summary>
        /// <returns>二进制数组</returns>
        public static byte[] LoadBinary(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    FileStream fileStream = File.OpenRead(filePath);
                    MemoryStream stream = new MemoryStream();
                    DecompressStream(fileStream, stream);

                    fileStream.Close();

                    byte[] bytes = stream.GetBuffer();

                    stream.Dispose();

                    return bytes;
                }
                catch (Exception e)
                {
                    Debug.Log("数据读取失败，输入文件格式不正确\n" + e.ToString());
                    return null;
                }

            }
            else
            {
                Debug.Log("数据读取失败[" + filePath + "]");
                return null;
            }
        }

        public static string CacheRCR<T>(T target)
        {

            return CacheRCR(target, false, out var _);
        }

        public static string CacheRCR<T>(T target, bool OutCRC, out string Crc16HEx)
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormat.Serialize(stream, target);

            Crc16HEx = default;
            if (OutCRC)
            {
                Crc16HEx = YooAsset.HashUtility.BytesCRC32(stream.GetBuffer());
            }

            stream.Dispose();

            return Crc16HEx;
        }

        /// <summary>
        /// 将 bytes 解析为目标对象 target
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="bytes">字节流</param>
        /// <param name="crcCode">是否记录 该文件的Crc16CcittFalse 值（crcCode == null 则不记录）</param>
        /// <returns>解析成功的目标对象</returns>
        public static T LoadFromBinary<T>(T target, byte[] bytes)
        {
            return LoadFromBinary(target, bytes, false, out var _);
        }

        public static T LoadFromBinary<T>(T target, byte[] bytes, bool OutCRC, out string CacheRCR)
        {
            if (bytes != null)
            {
                return LoadFromBinary<T>(target, new MemoryStream(bytes), OutCRC, out CacheRCR);
            }
            else
            {
                CacheRCR = default;
                Debug.Log("数据读取失败bytes == null");
                return target;
            }
        }

        /// <summary>
        /// 将 bytes 解析为目标对象 target
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="crcCode">是否记录 该文件的Crc16CcittFalse 值（crcCode == null 则不记录）</param>
        /// <returns>解析成功的目标对象</returns>
        public static T LoadFromBinary<T>(T target, string filePath)
        {
            return LoadFromBinary(target, filePath, false, out var _);
        }

        public static T LoadFromBinary<T>(T target, string filePath, bool OutCRC, out string Crc16HEx)
        {
            if (File.Exists(filePath))
            {
                return LoadFromBinary<T>(target, File.OpenRead(filePath), OutCRC, out Crc16HEx);
            }
            else
            {
                Crc16HEx = default;
                Debug.Log("数据读取失败[" + filePath + "]");
                return target;
            }
        }

        public static T LoadFromBinary<T>(T target, Stream fileStream)
        {
            return LoadFromBinary(target, fileStream, false, out var _);
        }

        public static T LoadFromBinary<T>(T target, Stream fileStream, bool OutCRC, out string Crc16HEx)
        {
            Crc16HEx = default;

            try
            {
                MemoryStream stream = new MemoryStream();
                DecompressStream(fileStream, stream);
                target = (T)BinaryFormat.Deserialize(stream);
                fileStream.Close();

                if (OutCRC)
                {
                    Crc16HEx = YooAsset.HashUtility.BytesCRC32(stream.GetBuffer());
                }

                stream.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log("数据读取失败，输入文件格式不正确\n" + e.ToString());
                return target;
            }

            return target;
        }

        internal static void CompressStream(Stream sources, Stream target)
        {
            GZipStream compressionStream = new GZipStream(target, CompressionMode.Compress);
            sources.Seek(0, SeekOrigin.Begin);
            sources.CopyTo(compressionStream);
            compressionStream.Close();
        }

        internal static void DecompressStream(Stream sources, Stream target)
        {
            var zipStream = new GZipStream(sources, CompressionMode.Decompress);
            zipStream.CopyTo(target);
            target.Seek(0, SeekOrigin.Begin);
            zipStream.Close();
        }
    }
}