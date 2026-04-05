using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣item
    /// </summary>
    public class GGUIMonoGuildDispatchHeroIcon : _AALBasicUIWndMono
    {
        [ALHeader("伙伴基础信息item")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;

        [ALHeader("玩家名")]
        public TextEx txtPlayerName;
        
        [ALHeader("加成百分比")]
        public TextEx txtAddPro;
        
        [ALHeader("没有大臣时显示")]
        public List<GameObject> noHeroShow;

        [ALHeader("派遣成功特效父节点")]
        public Transform dispatchSfxParent;
        [ALHeader("派遣成功特效id")]
        public long dispatchSfxId;
    }
}