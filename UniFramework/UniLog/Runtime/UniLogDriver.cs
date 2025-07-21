using UnityEngine;

namespace Uni.Log {

    /// <summary>
    /// 当需要手动刷新时可创建此脚本
    /// </summary>
    public class UniLogDriver : MonoBehaviour
    {
#if INTERNAL_CLIENT || UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void Initalize() {

            UniLog.Initalize();
        }
#if INTERNAL_CLIENT || UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
#endif
        static void ManualFlush()
        {
            UniLog.Instance.IsManualFlush = true;

            if (UniLog.Instance.driver == null)
            {
                GameObject obj = new UnityEngine.GameObject($"[{nameof(UniLog)}]");
                obj.hideFlags = HideFlags.HideInHierarchy;
                obj.AddComponent<UniLogDriver>();
                UnityEngine.Object.DontDestroyOnLoad(obj);
                UniLog.Instance.driver = obj;
            }
        }

        private void FixedUpdate()
        {
            UniLog.Instance.ManualFlush();
        }

        private void OnDestroy()
        {
            UniLog.Destroy();
        }
    }
}
