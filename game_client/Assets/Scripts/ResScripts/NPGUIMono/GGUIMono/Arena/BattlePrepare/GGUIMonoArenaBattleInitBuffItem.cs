using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗选择初始增益item
    /// </summary>
    public class GGUIMonoArenaBattleInitBuffItem : _AALBasicUIWndMono
    {
        [ALHeader("道具名称")]
        public Text txtItemName;
        [ALHeader("道具图标")]
        public RawImage imgItemIcon;
        [ALHeader("道具加成描述")]
        public Text txtItemDesc;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("购买消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("选中时需要显示的GO列表")]
        public List<GameObject> goSelectShowList;
        [ALHeader("选中时需要隐藏的GO列表")]
        public List<GameObject> goSelectHideList;
    }
}
