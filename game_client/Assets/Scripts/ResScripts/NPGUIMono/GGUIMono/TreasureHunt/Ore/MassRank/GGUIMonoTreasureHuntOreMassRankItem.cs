using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石质量排行item
    /// </summary>
    public class GGUIMonoTreasureHuntOreMassRankItem : _AALBasicUIWndMono
    {
        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;

        [ALHeader("质量")]
        public TextEx txtMass;
        [ALHeader("质量key(一个参数, 质量)")]
        public string txtMassKey;
        
        [ALHeader("有玩家信息时显示的列表")]
        public List<GameObject> hasPlayerInfoShowList;
        [ALHeader("没有玩家信息时显示的列表")]
        public List<GameObject> noPlayerInfoShowList;
    }
}