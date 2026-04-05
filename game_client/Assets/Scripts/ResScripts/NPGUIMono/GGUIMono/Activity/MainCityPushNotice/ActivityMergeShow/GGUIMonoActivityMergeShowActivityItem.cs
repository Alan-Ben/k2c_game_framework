using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动合并展示主城推送弹窗活动Item
    /// </summary>
    public class GGUIMonoActivityMergeShowActivityItem : _TALUGUIMonoGridItem
    {
        [ALHeader("活动名")]
        public TextEx txtActivityName;

        [ALHeader("活动banner")]
        public RawImage banner;

        [ALHeader("活动时间")]
        public TextEx txtActivityTime;

        [ALHeader("在活动结算前多少小时 需要显示距离活动结算倒计时")]
        public int showToSettlingCountDownBeforeSettlingHour;
        [ALHeader("距离活动结算倒计时")]
        public TextEx txtToSettlingCountDown;
        [ALHeader("距离活动结算倒计时Key(一个参数, 倒计时字符串)")]
        public string txtToSettlingCountDownKey;
        [ALHeader("在需要显示距离活动结算倒计时时需要显示的物体列表")]
        public List<GameObject> onShowToSettlingCountDownShow;
        [ALHeader("在需要显示距离活动结算倒计时时需要隐藏的物体列表")]
        public List<GameObject> onShowToSettlingCountDownHide;

        [ALHeader("前往按钮")]
        public GameObject goToBtn;
        
        [ALHeader("不同活动状态显示信息列表")]
        public List<NPCommonEnumStatMutexShowInfo<EActivityState>> activityStateShowInfoList;
    }
}