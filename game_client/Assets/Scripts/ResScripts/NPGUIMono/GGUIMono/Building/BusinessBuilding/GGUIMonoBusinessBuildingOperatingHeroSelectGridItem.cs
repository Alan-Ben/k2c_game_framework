
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoBusinessBuildingOperatingHeroSelectGridItemState
    {
        [InspectorName("CAPABLE === 可以选择")]
        CAPABLE,
        [InspectorName("SELECTED === 已选择")]
        SELECTED,
        [InspectorName("NOT_JOINED === 未加入")]
        NOT_JOINED,
    }
    public class GGUIMonoBusinessBuildingOperatingHeroSelectGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("各种状态下的显示列表")]
        public MultiStateShow<GGUIMonoBusinessBuildingOperatingHeroSelectGridItemState> stateShow;
        [ALHeader("未加入时置灰的列表")]
        public List<MaskableGraphic> listGrayNotJoined;

        [ALHeader("已经委派到别的建筑中的提示和展示对象")]
        public Text txtAlreadyJoined;
        public List<GameObject> listAlreadyJoinedShow;
        [ALHeader("选中的序号")]
        public Text txtSelectNum;
        [ALHeader("加成值")]
        public Text txtEarningBonus;
        [ALHeader("玩家信息")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;
        [ALHeader("获得途径提示弹窗的偏移量")]
        public Vector2 accessTipOffset;
    }
}