using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 顺序区东西游戏 物品挂载脚本
    /// </summary>
    [Serializable]
    public abstract class _AMonoTakeThingsSequentiallyGameThing : _AALBasicUIWndMono
    {
        [ALHeader("物品id")]
        public long thingId;

        [ALHeader("是否可拿走状态检查的物品列表\n" +
                  "假设这里配置成这样:[[A], [B, C]] 那么当(A物品被拿走) 或 (B和C物品都被拿走时) 该物品为可拿走状态")]
        [Space(15)]
        public List<TakeThingsSequentiallyGameThingLock> thingLockList;
        
        [ALHeader("不同状态显示")]
        public MultiStateShow<ETakeThingsSequentiallyGameThingState> stateShow;

        [ALHeader("物品动画")]
        public Animation thingAnimation;
        
        [ALHeader("不可拿走点击的动画")]
        public string onNotTakeableClickAnimName;
    }
    
    /// <summary>
    /// 物品的锁
    /// </summary>
    [Serializable]
    public class TakeThingsSequentiallyGameThingLock
    {
        [ALHeader("按序取东西游戏物品锁, 这个列表中物品都被取出后, 才能取出该物品")]
        public List<_AMonoTakeThingsSequentiallyGameThing> lockThings;
    }
    
    /// <summary>
    /// 顺序取东西游戏的物品
    /// </summary>
    public class GGUIMonoTakeThingsSequentiallyGameThing : _AMonoTakeThingsSequentiallyGameThing
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}