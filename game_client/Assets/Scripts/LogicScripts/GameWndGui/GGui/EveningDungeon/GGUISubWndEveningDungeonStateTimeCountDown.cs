using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动状态时间窗口
    /// </summary>
    public class GGUISubWndEveningDungeonStateTimeCountDown : _ANPGGUIBasicSubWnd<GGUISubMonoEveningDungeonStateTimeCountDown>
    {
        public GGUISubWndEveningDungeonStateTimeCountDown(GGUISubMonoEveningDungeonStateTimeCountDown _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onWndInitDone()
        {
            // 无需额外初始化
        }

        protected override void _onShowWnd()
        {
            _refreshActivityStateShow();
            _refreshCountDown();

            WinMsg.RegisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.RegisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_SEC_TICK, _onSecTick);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EVENING_DUNGEON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EVENING_DUNGEON_SEC_TICK, _onSecTick);
        }

        protected override void _onReset()
        {
            // 无需重置
        }

        protected override void _onDiscard()
        {
            // 无需额外销毁
        }


        #region 消息回调

        /// <summary>
        /// 活动状态变更回调
        /// 参数：_objs[0] 为旧状态，_objs[1] 为新状态
        /// </summary>
        private void _onActivityStateChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 2)
                return;

            _refreshActivityStateShow();
            _refreshCountDown();
        }

        /// <summary>
        /// 秒级tick回调（无参数）
        /// </summary>
        private void _onSecTick()
        {
            _refreshCountDown();
        }

        #endregion


        #region 刷新方法

        /// <summary>
        /// 刷新活动状态显示
        /// </summary>
        private void _refreshActivityStateShow()
        {
            if (wnd == null || wnd.showStateList == null)
                return;

            EEveningDungeonActivityState activityState = NPPlayer.instance.eveningDungeonComp.activityState;
            GGUIEveningDungeonActivityStateShow targetState = null;

            foreach (GGUIEveningDungeonActivityStateShow state in wnd.showStateList)
            {
                if (state == null)
                    continue;

                if (state.activityState != activityState)
                    ALUGUICommon.setGameObjEnable(state.goShowList, false);
                else
                    targetState = state;
            }

            if (targetState != null)
                ALUGUICommon.setGameObjEnable(targetState.goShowList, true);
        }

        /// <summary>
        /// 刷新倒计时文本
        /// </summary>
        private void _refreshCountDown()
        {
            if (wnd == null || (wnd.txtActivityTimeCountDownList == null && wnd.txtMeshProActivityTimeCountDownList == null))
                return;

            EEveningDungeonActivityState activityState = NPPlayer.instance.eveningDungeonComp.activityState;
            _getCurrentStateFinishTimeMsAndTransKey(activityState, out long finishTimeMs, out string transKey);

            if (finishTimeMs <= 0)
            {
                _setLabelTxt(wnd.txtActivityTimeCountDownList, string.Empty);
                _setLabelTxt(wnd.txtMeshProActivityTimeCountDownList, string.Empty);
                return;
            }

            long leftTimeMs = finishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs < 0)
                leftTimeMs = 0;

            string timeText = TimeUtil.millisecondsToTime_hms(leftTimeMs);
            Color cdColor = _getCDTextColor(activityState);

            if (string.IsNullOrEmpty(transKey) || !wnd.needUseShowActivityStateDescKey)
            {
                timeText = GCommon.addColorForRichText(timeText, cdColor);
            }
            else
            {
                timeText = GCommon.addColorForRichText(TextTranslate.instance.getLanguage(transKey, timeText), cdColor);
            }
            
            _setLabelTxt(wnd.txtActivityTimeCountDownList, timeText);
            _setLabelTxt(wnd.txtMeshProActivityTimeCountDownList, timeText);
        }

        private void _setLabelTxt(List<TextEx> txtList, string _str)
        {
            if(txtList == null)
                return;

            foreach (var txt in txtList)
            {
                ALUGUICommon.setLabelTxt(txt, _str);
            }
        }
        
        private void _setLabelTxt(List<TMP_Text> tmpTextList, string _str)
        {
            if(tmpTextList == null)
                return;

            foreach (var tmpText in tmpTextList)
            {
                ALUGUICommon.setLabelTxt(tmpText, _str);
            }
        }
        
        /// <summary>
        /// 根据活动状态获取对应的结束时间以及翻译key
        /// </summary>
        private void _getCurrentStateFinishTimeMsAndTransKey(EEveningDungeonActivityState _state, out long _finishTimeMs, out string _transKey)
        {
            _finishTimeMs = 0;
            _transKey = string.Empty;

            switch (_state)
            {
                case EEveningDungeonActivityState.ONGOING:
                    _finishTimeMs = NPPlayer.instance.eveningDungeonComp.endTimeMs;
                    _transKey = TransKeyConst.eveningDungeon_playing_str;
                    break;
                case EEveningDungeonActivityState.END:
                    _finishTimeMs = NPPlayer.instance.eveningDungeonComp.preCloseTimeMs;
                    _transKey = TransKeyConst.eveningDungeon_settling_str;
                    break;
                case EEveningDungeonActivityState.CLOSE:
                    _finishTimeMs = NPPlayer.instance.eveningDungeonComp.previewTimeMs;
                    _transKey = TransKeyConst.eveningDungeon_closing_str;
                    break;
                case EEveningDungeonActivityState.PREVIEW:
                    _finishTimeMs = NPPlayer.instance.eveningDungeonComp.startTimeMs;
                    _transKey = TransKeyConst.eveningDungeon_preparing_str;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获取对应状态的倒计时文本颜色
        /// </summary>
        private Color _getCDTextColor(EEveningDungeonActivityState _state)
        {
            if (wnd == null || wnd.showStateList == null)
                return Color.white;

            for (int i = 0; i < wnd.showStateList.Count; i++)
            {
                GGUIEveningDungeonActivityStateShow state = wnd.showStateList[i];
                if (state != null && state.activityState == _state)
                    return state.cdTextColor;
            }

            return Color.white;
        }

        #endregion
    }
}