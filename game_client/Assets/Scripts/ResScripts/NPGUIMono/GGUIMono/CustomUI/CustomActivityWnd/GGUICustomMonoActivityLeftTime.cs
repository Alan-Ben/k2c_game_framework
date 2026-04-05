using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class GGUIActivityLeftTimeShowState
    {
        [ALHeader("活动状态")]
        public EActivityState activityState;
        [ALHeader("倒计时文本颜色")]
        public Color cdTextColor = Color.white;
    }

    /// <summary>
    /// 活动剩余时间倒计时展示
    /// </summary>
    public class GGUICustomMonoActivityLeftTime : MonoBehaviour
    {
		[ALHeader("活动id")]
        public long activityId;
		[ALHeader("活动倒计时文本")]
        public Text txtLeftTime;
        [ALHeader("不同状态倒计时文本颜色配置")]
        public List<GGUIActivityLeftTimeShowState> showStateList;

        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_iTickTask.setDisable();
            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_check, 1.0f);
        }

	    private void OnDisable()
        {
            _m_iTickTask.setDisable();
        }

	    private void OnDestroy()
        {
            _m_iTickTask.setDisable();
        }

	    protected void _check()
	    {
#if NP_GAME
			if (null == this || null == gameObject)
	            return;

	        if (gameObject.activeInHierarchy)
            {
                //获取最后一个活动
                _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId);
                if (activityInfo == null)
                    return;

                //当前状态结束时间
                long curStateFinishTimeMs = 0;
                //当前状态文本颜色
                Color curTextColor = getCDTextColor(activityInfo.activityState);
                //翻译key
                string transKey = null;

                switch (activityInfo.activityState)
                {
                    case EActivityState.PLAYING:
                        curStateFinishTimeMs = activityInfo.endTimeMs;
                        transKey = TransKeyConst.activity_playing_str;
                        break;
                    case EActivityState.SETTLING:
                        curStateFinishTimeMs = activityInfo.settleTimeMs;
                        transKey = TransKeyConst.activity_settling_str;
                        break;
                    case EActivityState.REWARDING:
                        curStateFinishTimeMs = activityInfo.closeTimeMs;
                        transKey = TransKeyConst.activity_gettingReward_str;
                        break;
                    default:
                        ALUGUICommon.setLabelTxt(txtLeftTime, "");
                        return;
                }

                long leftTimeMs = curStateFinishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                if (leftTimeMs < 0)
                    leftTimeMs = 0;

                ALUGUICommon.setLabelTxt(txtLeftTime, GCommon.addColorForRichText(TextTranslate.instance.getLanguage(transKey, TimeUtil.millisecondsToTime_dhms(leftTimeMs)), curTextColor));
            }
	        else
            {
                ALUGUICommon.setLabelTxt(txtLeftTime, "");
            }
#endif
		}

        /// <summary>
        /// 获取CD文本颜色
        /// </summary>
        /// <param name="_state"></param>
        /// <returns></returns>
        public Color getCDTextColor(EActivityState _state)
        {
            if (showStateList == null)
                return Color.white;

            for (int i = 0; i < showStateList.Count; i++)
            {
                if (showStateList[i] != null && showStateList[i].activityState == _state)
                    return showStateList[i].cdTextColor;
            }
            return Color.white;
        }
    }
}