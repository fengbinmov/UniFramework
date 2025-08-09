using UnityEngine;

namespace Uni.Utility
{
    public class GizmosMeshHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum GizmeType
        {
            Close,
            Always,
            Select,
        }

        public Mesh mesh;
        public GizmeType isShow = GizmeType.Always;
        public Color color = Color.white;
        public Vector3 center = Vector3.zero;
        public Vector3 euler = Vector3.zero;
        public Vector3 scale = Vector3.one;

        private void Draw()
        {
            if (mesh == null) return;

            Gizmos.color = color;
            var mat = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawMesh(mesh, center, Quaternion.Euler(euler), scale);
            Gizmos.matrix = mat;
        }

        private void OnDrawGizmos()
        {
            if (isShow == GizmeType.Always) Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (isShow == GizmeType.Select) Draw();
        }
#endif
    }
}
