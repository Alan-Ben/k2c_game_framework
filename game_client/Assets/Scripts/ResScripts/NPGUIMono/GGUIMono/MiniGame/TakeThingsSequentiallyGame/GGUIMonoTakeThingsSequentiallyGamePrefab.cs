using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 顺序取东西游戏的游戏预制
    /// </summary>
    public class GGUIMonoTakeThingsSequentiallyGamePrefab : _AALBasicUIWndMono
    {
        [ALHeader("物品列表")]
        public List<GGUIMonoTakeThingsSequentiallyGameThing> monoThingList;
        
        [ALHeader("不同状态显示")]
        public MultiStateShow<ETakeThingsSequentiallyGameState> stateShow;
    }
}