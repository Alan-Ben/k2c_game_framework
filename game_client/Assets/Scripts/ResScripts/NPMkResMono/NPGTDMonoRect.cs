using UnityEditor;
using UnityEngine;

namespace GOE
{
    public class NPGTDMonoRect : MonoBehaviour
    {
        [ALHeader("用什么颜色显示范围")]
        public Color color = Color.magenta;
        [ALHeader("显示范围的高度")]
        public float cubeY = 10;
        [ALHeader("范围")]
        public Rect rect;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color beforeColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawWireCube(new Vector3(rect.center.x, 0, rect.center.y), new Vector3(rect.size.x, cubeY, rect.size.y));
            Gizmos.color = beforeColor;
        }
#endif
    }
}