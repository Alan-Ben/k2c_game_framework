using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{    
    /// <summary>
    /// item
    /// </summary>
    public class GGUIMonoMiddayDungeonHeroSelectItem : _TALUGUIMonoGridItem
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("大臣所属联盟成员名")]
        public Text txtGuildPlayerName;
        [ALHeader("剩余出战次数")]
        public Text txtLeftFightTimes;
        [ALHeader("有战斗次数时颜色")]
        public Color hasFightTimesCol = Color.green;
        [ALHeader("无战斗次数时颜色")]
        public Color noFightTimesCol = Color.red;
        [ALHeader("选中的序号")]
        public Text txtSelectNum;
        [ALHeader("选中时的显隐列表")]
        public List<GameObject> listSelectShow;
        public List<GameObject> listSelectHide;
        [ALHeader("不可用时的显示列表")]
        public List<GameObject> listInValidShow;
        [ALHeader("不可用时的隐藏列表")]
        public List<GameObject> listInValidHide;
        [ALHeader("大臣信息")]
        public GGUIMonoHeroIconItem monoHeroCard;
        public List<NPCommonEnumStatInfo<EMiddayDungeonSelectHeroType>> statInfos;
    }
}
