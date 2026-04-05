using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{    
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoHeroCommonSelectItem : _TALUGUIMonoGridItem
    {
        [ALHeader("选中的序号")]
        public Text txtSelectNum;
        [ALHeader("选中时的显隐列表")]
        public List<GameObject> listSelectShow;
        public List<GameObject> listSelectHide;
        [ALHeader("大臣信息")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;
    }
}
