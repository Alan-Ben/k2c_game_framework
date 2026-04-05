using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 用于给策划直接配置倒计时的组件
    /// </summary>
    public class GGUICustomMonoCountdown : _ANPGGUIMonoCustomBasicWnd
    {
        public enum CustomCountDownTimeTxtType
        {
            Default,
            MinuteSecond,
            HourMinuteSecond,
        }
        [Serializable]
        public class CountdownColorEvent
        {
            public int countdown;
            public Color color;
        }
        [Serializable]
        public class CountdownAnimEvent
        {
            public int countdown;
            public string aniName;
        }
        
        [ALHeader("倒计时时长/秒")]
        public int countdown = 180;
        
        [ALHeader("是否循环")]
        public bool loop = false;
        [ALHeader("倒计时文本显示类型")]
        [ALHeader("Default:")]
        [ALHeader("\t大于一天显示：x天x小时")]
        [ALHeader("\t小于一天大于一小时显示：h:m:s")]
        [ALHeader("\t小于一小时大于一分钟显示：m:s")]
        [ALHeader("\t小于一分钟显示：s")]
        [ALHeader("\t显示：m:s")]
        [ALHeader("MinuteSecond: 显示：m:s")]
        [ALHeader("HourMinuteSecond: 显示：h:m:s")]
        [ALHeader("\t显示：h:m:s")]
        public CustomCountDownTimeTxtType timeTxtType = CustomCountDownTimeTxtType.Default;
        [ALHeader("倒计时文本")]
        public Text txtCountDown;
        [ALHeader("倒计时文字颜色事件")]
        public List<CountdownColorEvent> countdownColorEvents;
        
        [ALHeader("倒计时动画事件")]
        public List<CountdownAnimEvent> countdownAnimEvents;
        
        [ALHeader("倒计时为0需要显示的GoList")]
        public List<GameObject> stopTickShowGoList;
        
        [ALHeader("倒计时为0需要隐藏的GoList")]
        public List<GameObject> stopTickHideGoList;
        
        
        private int _m_fCurCountdown;
        private Color _m_initTextColor;
       
        //每秒检测数据状态的任务控制对象
        private ALCommonEnableTaskController _m_tcTickTaskController;

        private void Awake()
        {
            if (txtCountDown != null) _m_initTextColor = txtCountDown.color;
        }

        protected override void _onCustomUIEnable()
        {
            _init();
        }

        protected override void _onCustomUIDisable()
        {
            _m_fCurCountdown = 0;
            _stopTask();
        }

        private void _init()
        {
            _m_fCurCountdown = countdown;
            if (_m_fCurCountdown <= 0)
            {
                ALUGUICommon.setGameObjEnable(stopTickShowGoList, true);
                ALUGUICommon.setGameObjEnable(stopTickHideGoList, false);
                return;
            }

            ALUGUICommon.setGameObjEnable(stopTickShowGoList, false);
            ALUGUICommon.setGameObjEnable(stopTickHideGoList, true);
            ALUGUICommon.setUIObjColor(txtCountDown, _m_initTextColor);
            _refreshCountDownText();
            _startTask();
        }

        //开启限时任务计时
        private void _startTask()
        {
            //停止原先任务
            _m_tcTickTaskController.setDisable();

            //开启任务进行数据逻辑的处理
            // 这里要加delay，不加delay，当帧既会执行，不适合这里每次执行减少1秒的逻辑
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshCountDown, 1f, 1f); 
        }

        //停止倒计时
        private void _stopTask()
        {
            _m_tcTickTaskController.setDisable();
        }

        private void _refreshCountDownText()
        {
#if NP_GAME
            // 更新倒计时文本
            string timeStr;
            switch (timeTxtType)
            {
                case CustomCountDownTimeTxtType.MinuteSecond:
                    timeStr = TimeUtil.millisecondsToTime_ms(_m_fCurCountdown * 1000);
                    break;
                case CustomCountDownTimeTxtType.HourMinuteSecond:
                    timeStr = TimeUtil.millisecondsToTime_hms(_m_fCurCountdown * 1000);
                    break;
                case CustomCountDownTimeTxtType.Default:
                default:
                    timeStr = TimeUtil.millisecondsToTime_DayHourOrHMSOrMSOrS(_m_fCurCountdown * 1000);
                    break;
            }
            ALUGUICommon.setLabelTxt(txtCountDown,  timeStr);
#endif
        }
        
        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshCountDown()
        {
            // 因为这个task创建的时候就会刷新，所以把计数减少放到显示之后这里处理，
            _m_fCurCountdown -= 1;
            
            //处理倒计时文字颜色变化事件
            if (countdownColorEvents != null)
            {
                foreach (var countdownEvent in countdownColorEvents)
                {
                    if (countdownEvent != null && _m_fCurCountdown <= countdownEvent.countdown)
                    {
                        ALUGUICommon.setUIObjColor(txtCountDown, countdownEvent.color);
                    }
                }
            }
            //处理倒计时动画事件
            if (countdownAnimEvents != null)
            {
                foreach (var countdownEvent in countdownAnimEvents)
                {
                    if (countdownEvent != null && _m_fCurCountdown <= countdownEvent.countdown)
                    {
                        if (wndAnimation != null) wndAnimation.Play(countdownEvent.aniName);
                    }
                }
            }

            _refreshCountDownText();
            
            // -1秒的时候，停止任务或执行loop
            if (_m_fCurCountdown < 0)
            {
                ALUGUICommon.setGameObjEnable(stopTickShowGoList, true);
                ALUGUICommon.setGameObjEnable(stopTickHideGoList, false);
                _m_fCurCountdown = 0;
                _stopTask();
                // 如果是循环，则重新初始化
                if (loop)
                    _init();
            }
        }
    }
}