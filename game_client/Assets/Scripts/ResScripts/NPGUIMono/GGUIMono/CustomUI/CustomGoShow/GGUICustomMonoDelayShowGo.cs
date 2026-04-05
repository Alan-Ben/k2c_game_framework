using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 无操作延迟显示go的自定义脚本
    /// </summary>
    public class GGUICustomMonoDelayShowGo : MonoBehaviour
    {
        [ALHeader("延迟触发的时间")]
        public float delayShowTime = 5;
        [ALHeader("显示多久之后隐藏")]
        public float onShowDelayHideTime = 2;
        [ALHeader("重复几次之后常驻")]
        public int rePlayCount = 3;
        [ALHeader("控制显示的go")]
        public List<GameObject> ctrGoList;
        
        #if NP_GAME

        private int _m_showSerialize;
        private int _m_curShowCount = 0;//当前显示的次数
        private float _m_curRoundTickTime = 0;//当前tick的时间
        private float _m_lastTickTime = 0;//上次tick的时间
        private ALCommonEnableTaskController _m_tickTask;

        private void OnEnable()
        {
            _m_curShowCount = 0;
            _m_curRoundTickTime = 0;
            ALUGUICommon.setGameObjEnable(ctrGoList, false);
            //监听点击等操作
            WinMsg.RegisterMsgAct(WinMsgType.SCREEN_CLICK, _dealReset);
            WinMsg.RegisterMsgAct(WinMsgType.SCREEN_PRESS, _dealReset);
            _m_tickTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_dealTick);
        }

        /// <summary>
        /// tick
        /// </summary>
        private void _dealTick()
        {
            _m_curRoundTickTime += Time.deltaTime;
            if (_m_lastTickTime < delayShowTime + onShowDelayHideTime && _m_curRoundTickTime >= delayShowTime + onShowDelayHideTime)//一轮过去
            {
                _dealRoundEnd();
            }
            else if (_m_lastTickTime < delayShowTime && _m_curRoundTickTime >= delayShowTime)//新一轮的显示
            {
                _dealRoundShow();
            }

            _m_lastTickTime = _m_curRoundTickTime;
        }

        private void _dealReset()
        {
            _m_curShowCount = 0;
            _m_curRoundTickTime = 0;
            ALUGUICommon.setGameObjEnable(ctrGoList, false);
        }

        /// <summary>
        /// 一轮结束
        /// </summary>
        private void _dealRoundEnd()
        {
            //已经达到最大次数，一轮结束不重置
            if (_m_curShowCount >= rePlayCount)
                return;
            _m_curShowCount++;
            _m_curRoundTickTime = 0;
            ALUGUICommon.setGameObjEnable(ctrGoList, false);
        }

        /// <summary>
        /// 进入新的一轮显示
        /// </summary>
        private void _dealRoundShow()
        {
            ALUGUICommon.setGameObjEnable(ctrGoList, true);
        }
        
        private void OnDisable()
        {
            //结束计时
            _m_tickTask.setDisable();
            _dealReset();
            
            //移除监听点击等操作
            WinMsg.UnregisterMsgAct(WinMsgType.SCREEN_CLICK, _dealReset);
            WinMsg.UnregisterMsgAct(WinMsgType.SCREEN_PRESS, _dealReset);
        }
        #endif
    }
}