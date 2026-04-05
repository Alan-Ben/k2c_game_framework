using UnityEngine;

namespace GOE
{
    public abstract class _APosLimiter
    {
        public abstract Vector3 limitPos(Vector3 _pos, out bool _limited);
        public abstract bool isPosInStableArea(Vector3 _pos, out Vector3 _closestPos);
        public abstract void debugDrawUpdate(Vector3 _originPos, Color _color);
    }
}