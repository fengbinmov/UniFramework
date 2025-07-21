using Cysharp.Threading.Tasks.Triggers;
using System.Collections.Generic;
using UnityEngine;

namespace Uni.Utility
{
    public class MountData : MonoBehaviour
    {
        [System.Serializable]
        public struct ResGroup
        {
            public string name;
            public StringPairs<Object> data;
        }

        [SerializeField]
        private StringPairs<Object> listData = new StringPairs<Object>();

        [SerializeField]
        private ResGroup[] resGroups = new ResGroup[] { };

        public ResGroup[] ResGroups => resGroups;

        /// <summary>
        /// 获取指定的值
        /// - 当 group 不为空时将会先在 ResGroup中查询值，值为空则将会在 listData 中继续查询
        /// - 当 group 为空将只会在 listData 中查询值
        /// </summary>
        public Object this[string key, string group = null]
        {
            get
            {
                if (!string.IsNullOrEmpty(group))
                {
                    for (int i = 0; i < resGroups.Length; i++)
                    {
                        if (!resGroups[i].name.Equals(group)) continue;

                        if (!resGroups[i].data.ContainsKey(key))
                        {
                            Debug.LogError($"{this.gameObject.scene.name}{(group == null ?string.Empty :($"[group,{group}]"))} not is hav key [key,{key}]");
                            return default;
                        }

                        return resGroups[i].data[key];
                    }
                }
                if (!listData.ContainsKey(key))
                {
                    Debug.LogError($"{this.gameObject.scene.name}{(group == null ? string.Empty : ($"[group,{group}]"))} not is hav key [key,{key}]");
                    return default;
                }

                return listData[key];
            }
        }

        /// <summary>
        /// 是否存在该键所对应的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="group">键值所在的组</param>
        public bool ContainsKey(string key, string group = null)
        {
            if (string.IsNullOrEmpty(key)) return false;

            if (string.IsNullOrEmpty(group))
            {
                return listData.ContainsKey(key);
            }
            else
            {
                for (int i = 0; i < resGroups.Length; i++)
                {
                    if (!resGroups[i].name.Equals(group)) continue;

                    return resGroups[i].data.ContainsKey(key);
                }
                return false;
            }
        }

        /// <summary>
        /// 尝试获取指定键对应的数据
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">返回的对应数据</param>
        /// <param name="group">键值所在的组</param>
        public bool TryGetValue(string key, out UnityEngine.Object obj, string group = null)
        {
            obj = default;

            if (string.IsNullOrEmpty(key)) return false;

            if (string.IsNullOrEmpty(group))
            {
                return listData.TryGetValue(key, out obj);
            }
            else
            {
                for (int i = 0; i < resGroups.Length; i++)
                {
                    if (!resGroups[i].name.Equals(group)) continue;

                    if (resGroups[i].data.ContainsKey(key))
                    {
                        obj = resGroups[i].data[key];
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
                return false;
            }
        }

        public UnityEngine.Object GetValue(string key,string group = null)
        {
            if (string.IsNullOrEmpty(key)) return default;

            if (string.IsNullOrEmpty(group))
            {
                listData.TryGetValue(key, out var obj);
                return obj;
            }
            else
            {
                for (int i = 0; i < resGroups.Length; i++)
                {
                    if (!resGroups[i].name.Equals(group)) continue;

                    if (resGroups[i].data.ContainsKey(key))
                    {
                        return resGroups[i].data[key];
                    }
                    else
                    {
                        break;
                    }
                }
                return default;
            }
        }

        /// <summary>
        /// 返回所有的值数据
        /// </summary>
        public Object[] Values
        {
            get
            {
                List<Object> list = new List<Object>();

                foreach (var item in listData.Values)
                {
                    list.Add(item);
                }

                for (int i = 0; i < resGroups.Length; i++)
                {
                    if (resGroups[i].data == null) continue;

                    for (int j = 0; j < resGroups[i].data.Count; j++)
                    {
                        list.Add(resGroups[i].data[j].value);
                    }
                }

                return list.ToArray();
            }
        }
    }
}
