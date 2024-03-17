using System;
using System.IO;
using UnityEngine;

namespace UniFramework.Utility.FileData
{
    [Serializable]
    public class FileData<T>
    {
        private string _crc;                //CRC hash 缓存
        private bool _needsave;             //

        private string _relativePath;       //文件存储的根路径；为空则为 Application.persistentDataPath

        public string assetPath = "db.dat"; //文件存储的相对路径路径；

        [SerializeField]
        private T _data;

        /// <summary>
        /// 序列化数据
        /// </summary>
        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                _needsave = true;
            }
        }

        /// <summary>
        /// 该FileData 是否有效并能正常本地存储
        /// </summary>
        public bool Valid => File.Exists(Path.Combine(Application.persistentDataPath, assetPath));

        /// <summary>
        /// 是否需要保存
        /// </summary>
        public bool Needsave
        {
            get
            {
                return _data == null ? false : (_needsave || _crc != FileHelper.CacheRCR(_data));
            }
        }

        public FileData(string assetPath_)
        {
            assetPath = assetPath_;
            if (Valid) Read();
        }

        public FileData(string assetPath_,string relativePath_)
        {
            assetPath = assetPath_;
            _relativePath = relativePath_;

            if (Valid) Read();
        }

        /// <summary>
        /// 表明该数据已经改动，NeedSave 将为 true
        /// </summary>
        public void Dirty()
        {
            _needsave = true;
        }

        /// <summary>
        /// 从本地读取数据到 Data 当中
        /// </summary>
        [ContextMenu("Read")]
        public void Read()
        {
            string relativePath_ = string.IsNullOrEmpty(_relativePath) ? Application.persistentDataPath : _relativePath;
            _data = FileHelper.LoadFromBinary(_data, Path.Combine(relativePath_, assetPath), true, out _crc);
            _needsave = false;
        }

        /// <summary>
        /// 保存 Data 的序列化数据到本地
        /// </summary>
        [ContextMenu("Save")]
        public void Save()
        {
            string _crc_t = null;

            byte[] bytes = FileHelper.GetBinary(_data, true, out _crc_t);

            if (_crc != _crc_t)
            {
                _crc = _crc_t;
                string relativePath_ = string.IsNullOrEmpty(_relativePath) ? Application.persistentDataPath : _relativePath;
                FileHelper.ToBinary(bytes, Path.Combine(relativePath_, assetPath));
            }
            _needsave = false;
        }
    }
}