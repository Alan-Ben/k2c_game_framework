using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum ENPTempActivityStat
    {
        NONE,
        READYING,//即将开始
        START,//活动已开始
        END,//活动结束
    }

    [System.Serializable]
    public class NPTempActivityTime
    {
        [ALHeader("时")]
        public int hour;
        [ALHeader("分")]
        public int minute;
        [ALHeader("秒")]
        public int second;
    }
    
    /// <summary>
    /// 临时活动自定义入口
    /// </summary>
    public class NPGGUITempActivityCustomMono: UnityEngine.MonoBehaviour
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        
        [ALHeader("活动即将开始时间")]
        public NPTempActivityTime readyTime;
        [ALHeader("活动开始时间")]
        public NPTempActivityTime startTime;
        [ALHeader("活动结束时间")]
        public NPTempActivityTime endTime;

        [ALHeader("即将开始文本")]
        public Text txtWaitingTime;
        [ALHeader("即将开始倒计时key，参数：时间")]
        public string waitingTimeTxtKey;
        [ALHeader("即将开始点击提示文本，参数：时间")]
        public string waitingTimeTipKey;
        
        [ALHeader("结束倒计时文本")]
        public Text txtTimeDown;
        [ALHeader("结束倒计时key，参数：时间")]
        public string timeDownKey;
        [ALHeader("活动进行中点击执行的effect")]
        public string clickEffect;
        
        [ALHeader("活动结束点击提示文本,无参数")]
        public string endTipKey;

        [ALHeader("不同状态的显示配置")]
        public List<NPCommonEnumStatInfo<ENPTempActivityStat>> statInfos;


#if NP_GAME
        [System.NonSerialized]
        private ALCommonEnableTaskController _m_task;
        [System.NonSerialized]
        private ENPTempActivityStat _m_curStat = ENPTempActivityStat.NONE;
        [System.NonSerialized]
        private long _m_timeDiff = 0;

        private void OnEnable()
        {
            _m_task.setDisable();
            _m_task = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refresh,1f);
            ALUGUICommon.combineBtnClick(btnClick,_onClick);
        }

        private void OnDisable()
        {            
            _m_task.setDisable();
            ALUGUICommon.uncombineBtnClick(btnClick,_onClick);
        }
        
        private void _onClick(GameObject _obj)
        {
            if (_m_curStat == ENPTempActivityStat.NONE)
                return;
            if (_m_curStat == ENPTempActivityStat.READYING)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(waitingTimeTipKey,TimeUtil.millisecondsToTime_Two(_m_timeDiff)));
                return;
            }

            if (_m_curStat == ENPTempActivityStat.END)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(endTipKey));
                return;
            }
            
            if (_m_curStat == ENPTempActivityStat.START)
            {
                _NPPlayerEffectSerializeInfo effect = _NPPlayerEffectSerializeInfo.ReadFromString(clickEffect);
                if (effect.isEmpty)
                {
                    #if UNITY_EDITOR
                    ALLog.Error($"----配置的执行clickEffect不合法：{clickEffect}");
                    #endif
                }
                else
                {
                    effect.dealEffect();
                }
                return;
            }
        }

        private void _refresh()
        {
            DateTime now = TimeUtil.FromUTCSeconds(FpsAndPingMgr.instance.serverTimeTagS);
            ENPTempActivityStat stat = ENPTempActivityStat.NONE;
            if (_isTimeLess(now, readyTime)) //还没到准备时间 = 活动结束
            {
                stat = ENPTempActivityStat.END;
            }
            else if (_isTimeLess(now, startTime)) //还没到开始时间 = 活动即将开始
            {
                stat = ENPTempActivityStat.READYING;
                DateTime startTimeDate = new DateTime(now.Year,now.Month,now.Day);
                startTimeDate = startTimeDate.AddHours(startTime.hour);
                startTimeDate = startTimeDate.AddMinutes(startTime.minute);
                startTimeDate = startTimeDate.AddSeconds(startTime.second);
                _m_timeDiff = (long)(startTimeDate - now).TotalMilliseconds;
                ALUGUICommon.setLabelTxt(this.txtWaitingTime, TextTranslate.instance.getLanguage(this.waitingTimeTxtKey, TimeUtil.millisecondsToTime_Two(_m_timeDiff)));
            }
            else if (_isTimeLess(now, endTime)) //还没到结束时间 = 活动进行中
            {
                stat = ENPTempActivityStat.START;
                
                DateTime endTimeDate = new DateTime(now.Year,now.Month,now.Day);
                endTimeDate = endTimeDate.AddHours(endTime.hour);
                endTimeDate = endTimeDate.AddMinutes(endTime.minute);
                endTimeDate = endTimeDate.AddSeconds(endTime.second);
                _m_timeDiff = (long)(endTimeDate - now).TotalMilliseconds;
                ALUGUICommon.setLabelTxt(this.txtTimeDown, TextTranslate.instance.getLanguage(this.timeDownKey, TimeUtil.millisecondsToTime_Two(_m_timeDiff)));
            }
            else //超过活动结束时间时间 = 活动结束
            {
                stat = ENPTempActivityStat.END;
            }
            
            if(_m_curStat == stat)
                return;
            _m_curStat = stat;
            
            NPCommonEnumStatInfo<ENPTempActivityStat>.setStat(statInfos, _m_curStat);
        }

        /// <summary>
        /// 是否小于目标时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="_targetTime"></param>
        /// <returns></returns>
        private bool _isTimeLess(DateTime time, NPTempActivityTime _targetTime)
        {
            if (time.Hour < _targetTime.hour)
                return true;
            if (time.Hour == _targetTime.hour && time.Minute < _targetTime.minute)
                return true;
            if (time.Hour == _targetTime.hour && time.Minute == _targetTime.minute && time.Second < _targetTime.second)
                return true;
            return false;
        }

#endif
        
        
    }
}