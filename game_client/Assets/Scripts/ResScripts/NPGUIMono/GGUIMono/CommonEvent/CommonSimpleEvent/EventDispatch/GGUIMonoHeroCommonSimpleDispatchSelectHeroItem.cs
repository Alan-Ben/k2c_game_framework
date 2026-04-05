using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoHeroCommonSimpleDispatchSelectHeroItem : _ATNPGGUIMonoStateShow<ECommonSelectState, _ATNPGGUIStateShowParam<ECommonSelectState>>
    {
        [ALHeader("所有条件满足时显示物体列表")]
        public List<GameObject> allConditionSatisfyShowList;
        
        [ALHeader("没有条件满足时显示物体列表")]
        public List<GameObject> noConditionSatisfyShowList;
        
        [ALHeader("部分条件满足时显示物体列表")]
        public List<GameObject> partConditionSatisfyShowList;
    }
}