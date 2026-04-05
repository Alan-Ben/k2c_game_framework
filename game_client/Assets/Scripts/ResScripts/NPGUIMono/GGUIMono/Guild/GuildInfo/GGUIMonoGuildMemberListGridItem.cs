using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟成员列表item
    /// </summary>
    public class GGUIMonoGuildMemberListGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("管理按钮")]
        public GameObject btnManage;
        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon playerIcon;
        [ALHeader("半身像")]
        public RawImage imgBody;
        [ALHeader("职位图标")]
        public RawImage imgPosIcon;
        [ALHeader("职位banner图")]
        public Image imgBanner;
        [ALHeader("离线时长文本")]
        public Text txtOffLineTime;
        [ALHeader("七日贡献度")]
        public Text txtSevenDayContribute;
        [ALHeader("总贡献度")]
        public Text txtTotalContribute;
        [ALHeader("在线时需要显示的GO列表")]
        public List<GameObject> goOnlineShowList;
        [ALHeader("在线时需要隐藏的GO列表")]
        public List<GameObject> goOnlineHideList;
        [ALHeader("选择的勾选框")]
        public NPGGUIMonoCommonToggleEx monoCheckToggle;
        [ALHeader("列表选择模式时需要显示的GO列表")]
        public List<GameObject> goSelectModeShowList;
        [ALHeader("列表选择模式时需要隐藏的GO列表")]
        public List<GameObject> goSelectModeHideList;
        [ALHeader("是否需要隐藏自己的选择框")]
        public bool needHideSelfCheckMark = true;
        [ALHeader("内容为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("内容为空时需要隐藏的GO列表")]
        public List<GameObject> goEmptyHideList;
    }
}
