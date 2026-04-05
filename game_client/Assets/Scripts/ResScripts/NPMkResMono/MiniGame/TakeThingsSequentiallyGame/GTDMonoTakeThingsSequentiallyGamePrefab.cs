using System.Collections.Generic;
using UnityEngine;

namespace GOE.MiniGame.TakeThingsSequentiallyGame
{
    public class GTDMonoTakeThingsSequentiallyGamePrefab : MonoBehaviour
    {
        [ALHeader("物品列表")]
        public List<GTDMonoTakeThingsSequentiallyGameThing> monoThingList;
        
        [ALHeader("不同状态显示")]
        public MultiStateShow<ETakeThingsSequentiallyGameState> stateShow;
    }
}