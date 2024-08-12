using UnityEngine;

namespace UniFramework.Utility
{
    public class GizmosMeshHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        public Mesh mesh;
        public bool isShow = true;
        public Color color = Color.white;
        public Vector3 scale = Vector3.one;

        private void OnDrawGizmos()
        {
            if (mesh == null || !isShow) return;

            Gizmos.color = color;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation, scale);
        }
#endif
    }
}