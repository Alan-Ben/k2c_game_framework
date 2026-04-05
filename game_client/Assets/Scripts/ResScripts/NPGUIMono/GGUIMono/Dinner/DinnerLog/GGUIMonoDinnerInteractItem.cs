using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    
    /// <summary>
    /// 宴会参与历史item
    /// </summary>
    public class GGUIMonoDinnerInteractItem : _TALUGUIMonoGridItem
    {
        [ALHeader("玩家信息")]
        public NPGGUIMonoPlayerIcon playerIcon;
        [ALHeader("玩家信息加载中显示的Go")]
        public List<GameObject> playerInfoLoadingShowGos;
        public List<GameObject> playerInfoLoadingHideGos;
        [ALHeader("己方参加玩家宴会次数")]
        public TextEx txtJoinedCount;
        [ALHeader("该玩家参加己方宴会次数")]
        public TextEx txtBeJoinCount;

    }
}
