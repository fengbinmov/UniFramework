using UnityEngine;

namespace Uni.GOPool
{
    public class RecyclableObjectPoolViewer : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
