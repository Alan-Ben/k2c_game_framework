
using UnityEngine;

namespace GOE
{
    public class GTDMonoFakePerspectiveItem : MonoBehaviour
    {
        [ALInfo("当相机在标准位置时，这个 item 所在的位置")]
        [ALHeader("这个 item 的标准位置")]
        public Vector3 originPos;
        [ALInfo("xy 分别对应场景的移动方向右和上，倍率是指自己的偏移值和相机的偏移值之间的比值")]
        [ALHeader("插值倍率")]
        public Vector2 offsetScale;

        public Vector2 scaleLogicOffset(Vector2 _originOffset)
        {
            return Vector2.Scale(_originOffset, offsetScale);
        }
        public void setOffset(Vector3 _itemOffset)
        {
            transform.position = originPos + _itemOffset;
        }

#if UNITY_EDITOR
        public void debugDraw(float _debugDrawSize)
        {
            DebugPlus.DrawSphere(originPos, _debugDrawSize, Color.gray);
            DebugPlus.DrawArrow2(originPos, transform.position, Color.gray);
            DebugPlus.DrawSphere(transform.position, _debugDrawSize, Color.red);
        }

        [ContextMenu("Set Current Position As OriginPos")]
        public void setCurrentPositionAsOriginPos()
        {
            originPos = transform.position;
        }
#endif
    }
}