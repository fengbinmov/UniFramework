using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uni.Utility
{
    public class BoxColliderMat : MonoBehaviour
    {
#if UNITY_EDITOR
#if UNITY_EDITOR && ODIN_INSPECTOR
        [ToggleGroup("editMatrix")]
#endif
        public bool editMatrix;
#if UNITY_EDITOR && ODIN_INSPECTOR
        [ToggleGroup("editMatrix")]
#endif
        public Matrix4x4 matrix4X4;
#if UNITY_EDITOR && ODIN_INSPECTOR
        [ToggleGroup("editMatrix")]
#endif
        public Vector4[] vector4s = new Vector4[4];
#if UNITY_EDITOR && ODIN_INSPECTOR
        [ToggleGroup("editMatrix"), TextArea(7, 7)]
#endif
        public string matString;

        private BoxCollider _boxCollider;
        private BoxCollider Collider
        {
            get
            {
                if (_boxCollider == null) _boxCollider = GetComponent<BoxCollider>();
                return _boxCollider;
            }
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        [ToggleGroup("editMatrix"), Button()]
#endif
        void ReadFromString()
        {

            if (Collider == null) return;

            string[] m0 = matString.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

            float[] float16 = new float[16];
            int n = 0;

            for (int i = 1; i < m0.Length - 1; i++)
            {
                string m1 = m0[i].Replace("new Vector4(", string.Empty).Replace(" ", string.Empty).Replace("\r", string.Empty);
                m1 = m1.Remove(m1.Length - 1, 1);
                if (m1[m1.Length - 1] == ')') m1 = m1.Remove(m1.Length - 1, 1);

                string[] m2 = m1.Split(",", System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in m2)
                {
                    string m3 = item;
                    if (m3[m3.Length - 1] == 'f') m3 = m3.Remove(m3.Length - 1, 1);

                    float16[n++] = float.Parse(m3);
                }
            }

            Vector4[] vector4s = new Vector4[4];

            n = 0;
            for (int i = 0; i < vector4s.Length; i++)
            {
                vector4s[i] = new Vector4(float16[n++], float16[n++], float16[n++], float16[n++]);
            }

            n = 0;
            Matrix4x4 matrix = new Matrix4x4(vector4s[n++], vector4s[n++], vector4s[n++], vector4s[n++]);

            Collider.center = Vector3.zero;
            Collider.size = matrix.lossyScale;
            transform.position = matrix.GetPosition();
            transform.rotation = matrix.rotation;
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        [ToggleGroup("editMatrix"), Button()]
#endif
        void Matrix2String()
        {
            matString = $"matrix4X4 = new Matrix4x4(\n" +
                $"new Vector4({vector4s[0].x}f,{vector4s[1].x}f,{vector4s[2].x}f,{vector4s[3].x}f),\n" +
                $"new Vector4({vector4s[0].y}f,{vector4s[1].y}f,{vector4s[2].y}f,{vector4s[3].y}f),\n" +
                $"new Vector4({vector4s[0].z}f,{vector4s[1].z}f,{vector4s[2].z}f,{vector4s[3].z}f),\n" +
                $"new Vector4({vector4s[0].w}f,{vector4s[1].w}f,{vector4s[2].w}f,{vector4s[3].w}f)\n" +
                $");";
        }


        [ContextMenu("matrix4X4")]
        public void matrix4X4s()
        {

            Debug.Log($"matrix4X4 = new Matrix4x4(\n" +
                $"new Vector4({vector4s[0].x}f,{vector4s[1].x}f,{vector4s[2].x}f,{vector4s[3].x}f),\n" +
                $"new Vector4({vector4s[0].y}f,{vector4s[1].y}f,{vector4s[2].y}f,{vector4s[3].y}f),\n" +
                $"new Vector4({vector4s[0].z}f,{vector4s[1].z}f,{vector4s[2].z}f,{vector4s[3].z}f),\n" +
                $"new Vector4({vector4s[0].w}f,{vector4s[1].w}f,{vector4s[2].w}f,{vector4s[3].w}f)\n" +
                $");");
        }

        private void OnDrawGizmos()
        {
            if (editMatrix && Collider != null)
            {
                Vector3 postion = transform.TransformPoint(Collider.center);
                Vector3 euler = transform.eulerAngles;
                Vector3 size = Collider.size;

                matrix4X4 = Matrix4x4.TRS(postion, Quaternion.Euler(euler), size);

                if (vector4s == null || vector4s.Length != 4) vector4s = new Vector4[4];

                for (int i = 0; i < 4; i++)
                {
                    vector4s[i] = matrix4X4.GetRow(i);
                }

                Gizmoss.DrawCube(postion, size, Quaternion.Euler(euler));
            }
        }
#endif
    }

}
