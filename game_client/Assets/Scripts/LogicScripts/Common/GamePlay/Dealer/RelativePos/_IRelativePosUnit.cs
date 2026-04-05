using UnityEngine;

namespace GOE
{
    public interface _IRelativePosUnit
    {
        Vector3 position { get; }
        Vector3 relativePosition { set; }
    }
}