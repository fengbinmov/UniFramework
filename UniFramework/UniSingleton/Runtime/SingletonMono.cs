using UnityEngine;

namespace UniFramework.Singleton {

    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Inst => _instance;

        public virtual void Awake()
        {
            if (_instance == null) _instance = this as T;
            else
            {
                Debug.LogWarning("SingletonMono hav single:[InstanceID:" + Inst.GetInstanceID() + "][name," + Inst.name + "] not use this [InstanceID:" + GetInstanceID() + "][name," + name + "] this will be Destory");

                Destroy(gameObject);
            }
        }
    }
}