using UnityEngine;

namespace GOE
{
    public class _AGTDMonoBuildingFunction : MonoBehaviour
    {
        // todo: 目前功能还不需要这个值给策划配置，由代码内部自动填值，如果需要的话再改为序列化成员
        public long buildingId { get; set; }
        public Transform parentTrans;
    }
}