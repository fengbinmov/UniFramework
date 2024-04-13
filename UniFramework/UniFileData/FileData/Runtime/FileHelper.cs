using System;
using System.IO;
using UnityEngine;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;

namespace UniFramework.FileData
{
    /// <summary>
    /// 文件操作过程自动压缩
    /// </summary>
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
        public static void ToBinary<T>(T target, string filePath)
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormat.Serialize(stream, target);

            string direct = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);

            FileStream fileStream = File.Create(filePath);

            CompressStream(stream, fileStream);

            fileStream.Close();

            stream.Dispose();

            if (!File.Exists(filePath))
            {
                Debug.Log($"Failed to save data to the local directory, [filePath,{filePath}] not exist!");
            }
        }

        /// <summary>
        /// 将 bytes 解析为目标对象 target
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="crcCode">是否记录 该文件的Crc16CcittFalse 值（crcCode == null 则不记录）</param>
        /// <returns>解析成功的目标对象</returns>
        public static T LoadFromBinary<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    Stream fileStream = File.OpenRead(filePath);
                    MemoryStream stream = new MemoryStream();
                    DecompressStream(fileStream, stream);
                    T target = (T)BinaryFormat.Deserialize(stream);
                    fileStream.Close();

                    stream.Dispose();

                    return target;
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to read the data. The input file format is incorrect\n{e.ToString()}");
                    return default;
                }
            }
            else
            {
                Debug.Log($"Failed to load data to the local directory, [filePath,{filePath}] not exist!");
                return default;
            }
        }

        /// <summary>
        /// 将target 存储到指定位置
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="crcCode">是否记录 该文件的Crc16CcittFalse 值（crcCode == null 则不记录）</param>
        /// <returns>文件地址 存储失败则返回string.Empty</returns>
        public static void ToText(string text, string filePath)
        {
            MemoryStream stream = new MemoryStream();

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes,0, bytes.Length);

            string direct = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(direct)) Directory.CreateDirectory(direct);

            FileStream fileStream = File.Create(filePath);

            CompressStream(stream, fileStream);

            fileStream.Close();

            stream.Dispose();

            if (!File.Exists(filePath))
            {
                Debug.Log($"Failed to save data to the local directory, [filePath,{filePath}] not exist!");
            }
        }

        /// <summary>
        /// 将 bytes 解析为目标对象 target
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="filePath">文件地址</param>
        /// <param name="crcCode">是否记录 该文件的Crc16CcittFalse 值（crcCode == null 则不记录）</param>
        /// <returns>解析成功的目标对象</returns>
        public static string LoadFromText(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    Stream fileStream = File.OpenRead(filePath);
                    MemoryStream stream = new MemoryStream();
                    DecompressStream(fileStream, stream);

                    string text = Encoding.UTF8.GetString(stream.GetBuffer());

                    fileStream.Close();

                    stream.Dispose();

                    return text;
                }
                catch (Exception e)
                {
                    Debug.Log($"Failed to read the data. The input file format is incorrect\n{e.ToString()}");
                    return default;
                }
            }
            else
            {
                Debug.Log($"Failed to load data to the local directory, [filePath,{filePath}] not exist!");
                return default;
            }
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