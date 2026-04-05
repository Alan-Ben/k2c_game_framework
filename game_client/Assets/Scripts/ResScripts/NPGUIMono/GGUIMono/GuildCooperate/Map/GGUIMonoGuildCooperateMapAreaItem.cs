using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 地图区域显示状态枚举
    /// </summary>
    public enum EGuildCooperateMapAreaState
    {
        [InspectorName("UNLOCK_NO_REWARD（已解锁没有奖励）")]
        UNLOCK_NO_REWARD,
        [InspectorName("UNLOCK_HAVE_REWARD（已解锁有奖励）")]
        UNLOCK_HAVE_REWARD,
        [InspectorName("LOCK（未解锁）")]
        LOCK,
    }

    /// <summary>
    /// 公会协作地图预览区域
    /// </summary>
    public class GGUIMonoGuildCooperateMapAreaItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("区域名称")]
        public Text txtName;
        [ALHeader("区域进度")]
        public Text txtProgress;
        [ALHeader("区域状态显示")]
        public MultiStateShow<EGuildCooperateMapAreaState> areaStateShow;
    }
}

