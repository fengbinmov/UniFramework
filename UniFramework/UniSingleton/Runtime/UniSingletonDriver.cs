using UnityEngine;

namespace Uni.Singleton
{
    internal class UniSingletonDriver : MonoBehaviour
    {
        void Update()
        {
            UniSingleton.Update();
        }
        private void OnDestroy()
        {
            UniSingleton.Destroy();
        }
    }
}
