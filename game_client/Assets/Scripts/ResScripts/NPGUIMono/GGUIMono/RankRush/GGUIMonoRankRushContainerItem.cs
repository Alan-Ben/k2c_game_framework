using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class GGUIRankRushItemShowState
    {
        [ALHeader("活动状态")]
        public EActivityState activityState;
        [ALHeader("需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("需要隐藏的GO列表")]
        public List<GameObject> goHideList;
        [ALHeader("倒计时文本颜色")]
        public Color cdTextColor;
    }

    /// <summary>
    /// 限时冲榜列表item
    /// </summary>
    public class GGUIMonoRankRushContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("banner图")]
        public RawImage imgBanner;
        [ALHeader("冲榜名称")]
        public Text txtName;
        [ALHeader("冲榜持续时间")]
        public Text txtTime;
        [ALHeader("冲榜倒计时")]
        public Text txtCD;
        [ALHeader("红点")]
        public GameObject goRedTip;
        [ALHeader("展示状态配置列表")]
        public List<GGUIRankRushItemShowState> showStateList;
        [ALHeader("可领取奖励显示GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("可领取奖励隐藏GO列表")]
        public List<GameObject> goCanGetRewardHideList;
        [ALHeader("是跨服冲榜需要显示的GO列表")]
        public List<GameObject> goCrossShowList;
        [ALHeader("是本服冲榜需要显示的GO列表")]
        public List<GameObject> goLocalShowList;

        /// <summary>
        /// 获取CD文本颜色
        /// </summary>
        /// <param name="_state"></param>
        /// <returns></returns>
        public Color getCDTextColor(EActivityState _state)
        {
            if(showStateList == null)
                return Color.white;

            for (int i = 0; i < showStateList.Count; i++)
            {
                if (showStateList[i] != null && showStateList[i].activityState == _state)
                    return showStateList[i].cdTextColor;
            }
            return Color.white;
        }

        /// <summary>
        /// 设置显示状态
        /// </summary>
        /// <param name="_state"></param>
        public void setState(EActivityState _state)
        {
            if (showStateList == null)
                return;

            for (int i = 0; i < showStateList.Count; i++)
            {
                if (showStateList[i] != null && showStateList[i].activityState == _state)
                {
                    ALUGUICommon.setGameObjEnable(showStateList[i].goShowList, true);
                    ALUGUICommon.setGameObjEnable(showStateList[i].goHideList, true);
                }
            }
        }
    }
}
