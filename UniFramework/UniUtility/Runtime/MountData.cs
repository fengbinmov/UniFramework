using System.Collections.Generic;
using UnityEngine;

namespace UniFramework.Utility
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

        /// <summary>
        /// ��ȡָ����ֵ
        /// - �� group ��Ϊ��ʱ�������� ResGroup�в�ѯֵ��ֵΪ���򽫻��� listData �м�����ѯ
        /// - �� group Ϊ�ս�ֻ���� listData �в�ѯֵ
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

                        if (!resGroups[i].data.ContainsKey(key)) Debug.LogError($"[Data,{name}] not is hav key " + key);

                        return resGroups[i].data[key];
                    }
                }
                if (!listData.ContainsKey(key)) Debug.LogError($"[Data,{name}] not is hav key " + key);

                return listData[key];
            }
        }

        /// <summary>
        /// �Ƿ���ڸü�����Ӧ��ֵ
        /// </summary>
        /// <param name="key">��ֵ</param>
        /// <param name="group">��ֵ���ڵ���</param>
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
        /// ���Ի�ȡָ������Ӧ������
        /// </summary>
        /// <param name="key">��ֵ</param>
        /// <param name="obj">���صĶ�Ӧ����</param>
        /// <param name="group">��ֵ���ڵ���</param>
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

        /// <summary>
        /// �������е�ֵ����
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