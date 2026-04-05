using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOE
{
    [System.Serializable]
    public class RushExchangeLoad
    {
        public long entryPointId;
        public Transform parent;
        public long posId;
    }
    public class GTDMonoRushExchange : MonoBehaviour
    {
        [ALHeader("名字的跟随目标")]
        public Transform hudTarget;
        [ALHeader("点击进入的对象")]
        public GTDCommonPosClickMono clickMono;
        [ALHeader("状态列表")]
        public List<RushExchangeStateInfo> stateInfoList = new List<RushExchangeStateInfo>();
    }
}