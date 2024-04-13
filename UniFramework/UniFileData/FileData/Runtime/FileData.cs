using System;
using System.IO;
using UnityEngine;

namespace UniFramework.FileData
{
    /// <summary>
    /// FileBase 将指定类型本地化压缩存储与读取
    /// </summary>
    [Serializable]
    public abstract class FileBase<T>
    {
        private string _pathRoot;
        private bool _dirty = true;         //标记数据对象是否已更改

        public string assetPath;            //文件存储路径的相对路径(相对于 PathRoot)

        [SerializeField]
        private T _data;                    //数据对象

        private string SavePath => Path.Combine(PathRoot, assetPath);       //实际保存地址(PathRoot & assetPath)
        public string PathRoot              //保存根目录
        {
            get {
                if (_pathRoot == null)
                {
                    _pathRoot = Application.persistentDataPath;
                }
                return _pathRoot;
            }
            set {
                if (!Directory.Exists(value))
                {
                    throw new Exception($"该目录不存在 {value}");
                }
                _pathRoot = value;
            }
        }

        public bool Exist => File.Exists(SavePath);     //数据对象是否有本地记录
        public T Data => _data;

        /// <summary>
        /// 检查资产路径是否有效
        /// </summary>
        public bool AssetPathValid(string assetPath_) => File.Exists(Path.Combine(PathRoot, assetPath_));

        public void Init(string assetPath_) {

            assetPath = assetPath_;

            if (Exist) Read();
            else
            {
                _dirty = true;

                T module = Activator.CreateInstance<T>();
                _data = module;
            }
        }

        public void SetDirty(bool value = true)
        {
            _dirty = value;
        }

        public void Read(T data_)
        {
            _dirty = true;
            _data = data_;
        }

        public void Read()
        {
            if (!Exist)
            {
                Debug.LogError($"FileData {typeof(T)} is Valid, file is not exist!");
                return;
            }

            _dirty = false;

            _data = LoadFromNative(SavePath);
        }

        public void Save()
        {
            if (!_dirty || _data == null) return;

            _dirty = false;

            SaveToNative(SavePath);
        }

        public virtual T LoadFromNative(string path)
        {
            return FileHelper.LoadFromBinary<T>(path);
        }
        public virtual void SaveToNative(string path)
        {
            FileHelper.ToBinary(_data, path);
        }
    }

    /// <summary>
    /// 数据对象 二进制存取
    /// </summary>
    [Serializable]
    public class FileData<T> : FileBase<T>
    {
        public override T LoadFromNative(string path) 
        { 
            return FileHelper.LoadFromBinary<T>(path);
        }

        public override void SaveToNative(string path)
        {
            FileHelper.ToBinary(Data, path);
        }
    }
        
    /// <summary>
    /// 数据对象 JsonText存取
    /// </summary>
    [Serializable]
    public class FileJson<T> :FileBase<T>
    {
        public override T LoadFromNative(string path)
        {
            var text = FileHelper.LoadFromText(path);
            return JsonUtility.FromJson<T>(text);
        }

        public override void SaveToNative(string path)
        {
            var text = JsonUtility.ToJson(Data);
            FileHelper.ToText(text, path);
        }
    }
}