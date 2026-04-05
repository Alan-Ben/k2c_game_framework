using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIMonoTowerChallengeItemContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoTowerChapterItem>
    {
        public GGUIMonoTowerChapterItem barItemPrefab;
        
        [ALHeader("列表为空的时候显示")]
        public List<GameObject> emptyListShow;
        [ALHeader("列表为空的时候隐藏")]
        public List<GameObject> emptyListHide;
    }
}