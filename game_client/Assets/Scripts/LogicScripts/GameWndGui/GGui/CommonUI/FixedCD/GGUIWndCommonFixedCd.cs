using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 仿照FixedCdCustomMono写的
    /// </summary>
    public class GGUIWndCommonFixedCd : _ANPGGUIBasicSubWnd<GGUIMonoCommonFixedCd>
    {
        private long _m_lFixedCdId;
        private NPPlayerFixedCDInfo _m_cdInfo;
        
        private ALCommonEnableTaskController _m_tRefreshTask;//刷新任务
        
        public GGUIWndCommonFixedCd(GGUIMonoCommonFixedCd _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnInfo, _onClickInfo);
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onClickGet);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnInfo, _onClickInfo);
                ALUGUICommon.uncombineBtnClick(wnd.btnGet, _onClickGet);
            }
        }
        
        protected override void _onShowWnd()
        {
            _initRefreshTask();
            
            WinMsg.RegisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCdCountChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_FIXED_CD_COUNT_CHG, _onFixedCdCountChg);

            _discardRefreshTask();
        }

        protected override void _onReset()
        {
        }
        
        private void _initRefreshTask()
        {
            _discardRefreshTask();
            
            if(wnd.refreshInterval > 0)
                _m_tRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshWnd, wnd.refreshInterval);
        }

        private void _discardRefreshTask()
        {
            _m_tRefreshTask.setDisable();
        }
        
        /// <summary>
        /// 设置fixedCdId
        /// </summary>
        /// <param name="_lFixedCdId"></param>
        public void setFixedCdId(long _lFixedCdId)
        {
            _m_lFixedCdId = _lFixedCdId;
            _m_cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_lFixedCdId);

            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if (wnd == null || wnd.gameObject == null || !wnd.gameObject.activeInHierarchy || _m_cdInfo == null)
                return;

            if(wnd.isShowMaxCount)
                ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(TransKeyConst.player_lazyCd_num, _m_cdInfo.getCount(), _m_cdInfo.getMaxCount()));
            else 
                ALUGUICommon.setLabelTxt(wnd.txtNum, _m_cdInfo.getCount());

            for (int i = 0; i < wnd.chgColorConfig.Count; i++)
            {
                wnd.chgColorConfig[i].refreshColor();
            }
            
            NPCommonEnumStatInfo<EFixedCDState>.setStat(wnd.cdStateInfoList, _getFixedCdState());

            // 刷新倒计时文本：次数已满时隐藏，未满时显示下次刷新倒计时
            if (wnd.txtRefreshNumCountDown != null)
            {
                long remainMs = _m_cdInfo.getNextCalcTimeTagMs() - FpsAndPingMgr.instance.serverTimeTag;
                if (remainMs < 0)
                    remainMs = 0;
                
                if(!string.IsNullOrEmpty(wnd.txtRefreshNumCountDownKey))
                    ALUGUICommon.setLabelTxt(wnd.txtRefreshNumCountDown, TextTranslate.instance.getLanguage(wnd.txtRefreshNumCountDownKey, TimeUtil.millisecondsToTime_hms(remainMs)));
                else
                    ALUGUICommon.setLabelTxt(wnd.txtRefreshNumCountDown, TimeUtil.millisecondsToTime_hms(remainMs));
            }
        }

        /// <summary>
        /// 获取fixed CD状态
        /// </summary>
        /// <returns></returns>
        private EFixedCDState _getFixedCdState()
        {
            if (_m_cdInfo == null || _m_cdInfo.getCount() <= 0)
                return EFixedCDState.NO_COUNT;

            if (_m_cdInfo.getCount() >= _m_cdInfo.getMaxCount())
                return EFixedCDState.MAX_COUNT;

            return EFixedCDState.HAS_COUNT_UNMET_MAX;
        }

        private void _onFixedCdCountChg(object[] _objs)
        {
            if(_objs == null || _objs.Length <= 0 || !(_objs[0] is long _fixedId))
                return;
            
            if (_m_lFixedCdId != _fixedId)
                return;
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 点击详情
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickInfo(GameObject _gameObject)
        {
            // NPGNodeCommonToolTip_ItemDetail toolTip = new NPGNodeCommonToolTip_ItemDetail(UIResPathConst.WIN_COMMON_RESOURCES_TIP, ENPItemType.FIXED_CD, fixedCdId, _gameObject.GetComponent<RectTransform>(), toolTipInterval);
            // QueueMgr.instance.AddNode(toolTip);
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                GCommon.getItemName(ENPItemType.FIXED_CD, _m_lFixedCdId),
                GCommon.getItemDesc(ENPItemType.FIXED_CD, _m_lFixedCdId),
                _gameObject.GetComponent<RectTransform>(), 0, wnd.toolTipInterval));
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="_gameObject"></param>
        private void _onClickGet(GameObject _gameObject)
        {
            GCommon.popItemAccessWays(ENPItemType.FIXED_CD, _m_lFixedCdId);
        }
    }
}