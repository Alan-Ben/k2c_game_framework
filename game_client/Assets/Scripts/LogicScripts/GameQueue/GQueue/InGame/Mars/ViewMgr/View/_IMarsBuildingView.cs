using UnityEngine;

namespace GOE
{
    public interface _IMarsBuildingView
    {
        void setSelected(bool _select);
        Vector3 position { get; }
        MarsBuildingInfo buildingInfo { get; }
        void triggerClick();
    }
}