using UnityEngine;

namespace Uni.Utility
{
    public class AnchorPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool alwaysShow; 
        public float height;
        public Vector3 positionW;
        public Vector3 eulerAngleW;

        private void OnDrawGizmos()
        {
            if (alwaysShow) OnDrawGizmosSelected();
        }

        private void OnDrawGizmosSelected()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10000))
            {
                height = (transform.position - hit.point).magnitude;
                positionW = transform.position;
                eulerAngleW = transform.eulerAngles;

                Gizmos.color = Color.green;
                Gizmos.DrawLine(hit.point,hit.point + transform.forward * 0.25f);
                Gizmos.DrawLine(transform.position, hit.point);
                DrawCirce(hit.point, 0.25f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, Vector3.down * 100);
            }
        }

        [ContextMenu("SetZeroHeight")]
        public void SetZeroHeight()
        {

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
            {
                transform.position = hit.point;
            }
        }

        public static void DrawCirce(Vector3 pos, float radius, int bian = 16)
        {
            bian = Mathf.Max(3, bian);
            for (float i = 0; i < bian; i++)
            {
                Vector3 a = Rotate(Vector2.up, (i / bian) * Mathf.PI * 2f) * radius + pos;
                Vector3 b = Rotate(Vector2.up, ((i + 1) % bian / bian) * Mathf.PI * 2f) * radius + pos;
                Gizmos.DrawLine(a, b);
            }
        }

        private static Vector3 Rotate(Vector2 v, float a)
        {
            Vector2 n = v.normalized;
            a += Mathf.Atan2(n.y, n.x);
            return new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * v.magnitude;
        }
#endif
    }

}
