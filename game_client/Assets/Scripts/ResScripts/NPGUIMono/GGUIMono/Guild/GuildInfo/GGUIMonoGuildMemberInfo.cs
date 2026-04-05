using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟成员信息子窗口
    /// </summary>
    public class GGUIMonoGuildMemberInfo : _AALBasicUIWndMono
    {
        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon playerIcon;
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
        [ALHeader("正在获取玩家信息时需要显示的GO列表")]
        public List<GameObject> goGettingPlayerInfoShowList;
        [ALHeader("正在获取玩家信息时需要隐藏的GO列表")]
        public List<GameObject> goGettingPlayerInfoHideList;
    }
}