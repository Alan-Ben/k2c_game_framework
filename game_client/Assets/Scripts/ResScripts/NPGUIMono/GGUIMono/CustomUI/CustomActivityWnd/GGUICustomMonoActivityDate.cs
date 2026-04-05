using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;


namespace GOE
{
    /// <summary>
    /// 活动日期展示
    /// </summary>
    public class GGUICustomMonoActivityDate : MonoBehaviour
    {
		[ALHeader("活动id")]
        public long activityId;
		[ALHeader("活动日期展示文本")]
        public Text txtDate;

	    //有效和无效的时候分别注册和注销显示对象
	    private void OnEnable()
        {
            _check();
        }

	    private void OnDisable()
        {
        }

	    private void OnDestroy()
	    {
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
                {
                    ALUGUICommon.setLabelTxt(txtDate, "");
                    return;
                }

                DateTime startTime = TimeUtil.FromUTCByTimeZone(activityInfo.startTimeMs);
                DateTime endTime = TimeUtil.FromUTCByTimeZone(activityInfo.endTimeMs);
                ALUGUICommon.setLabelTxt(txtDate,  TimeUtil.DateTime2String_DurationLong_YMD(startTime, endTime));
            }
	        else
            {
                ALUGUICommon.setLabelTxt(txtDate, "");
            }
#endif
		}
	}
}