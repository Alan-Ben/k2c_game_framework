using ALPackage;
using CommonEnum;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用游历事件结果预制体子窗口
    /// </summary>
    /// <typeparam name="T_Mono"></typeparam>
    /// <typeparam name="T_Data"></typeparam>
    public abstract class _ATravelResultWnd<T_Mono, T_EventResultInfo> : _ANPGGUIBasicWnd<T_Mono>, _ITravelResultWnd 
        where T_Mono : _ATravelResultMono
        where T_EventResultInfo : _ITravelEventResultInfo
    {
        protected T_EventResultInfo _m_eventResultInfo;
        private long _m_lUiResPathId;
        
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;//奖励列表
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;//玩家icon
        private NPGGUIWndProgress _m_wExpProgress;//当前经验进度条
        private NPGGUIWndProgress _m_wEarningsProgress;//当前赚速进度条
        
        protected _ATravelResultWnd(T_EventResultInfo _eventResultInfo, long _uiResPathId, EALUIWndLayer _layer) : base(_layer)
        {
            _m_eventResultInfo = _eventResultInfo;
            _m_lUiResPathId = _uiResPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUiResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUiResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        public T_EventResultInfo eventResultInfo { get { return _m_eventResultInfo; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);

            if (wnd.playerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);

            if (wnd.expProgress != null)
                _m_wExpProgress = new NPGGUIWndProgress(wnd.expProgress);

            if (wnd.earningsProgress != null)
                _m_wEarningsProgress = new NPGGUIWndProgress(wnd.earningsProgress);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            
            _onWndInitDoneSub();
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
            
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
            
            _m_wExpProgress?.discard();
            _m_wExpProgress = null;
            
            _m_wEarningsProgress?.discard();
            _m_wEarningsProgress = null;
            
            _onDiscardSub();
        }
        
        protected override void _onShowWnd()
        {
            _onShowWndSub();

            _refreshWnd();

            NPPlayer.instance.specialItemComp.goldData.onEarningsChg += _refreshEarnings;
        }

        protected override void _onHideWnd()
        {
            NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _refreshEarnings;
            
            _m_wRewardContainer?.hideWnd();

            _m_wPlayerIcon?.hideWnd();

            _m_wExpProgress?.hideWnd();

            _m_wEarningsProgress?.hideWnd();

            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();

            _m_wPlayerIcon?.resetWnd();

            _m_wExpProgress?.resetWnd();

            _m_wEarningsProgress?.resetWnd();
            
            _onResetSub();
        }

        public _AALBasicLoadUIWndBasicClass getBasicLoadUIWndBasicClass()
        {
            return this;
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_eventResultInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtEventResultDesc, TextTranslate.instance.getLanguage(_m_eventResultInfo.eventResultDesc));

            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(_m_eventResultInfo.showRewardItemList);
            }

            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.showWnd();
                _m_wPlayerIcon.setSelfInfo();
            }

            _refreshExp();
            _refreshEarnings();
            
            _onRefreshWnd();
        }

        /// <summary>
        /// 刷新经验
        /// </summary>
        private void _refreshExp()
        {
            if(wnd == null || _m_eventResultInfo == null)
                return;
            
            //设置玩家经验条，用经验值减去当前等级的初始经验
            if (null != _m_wExpProgress)
            {
                long showExp = NPPlayer.instance.rescourceComp.getValue(ECurrency.P_EXP) - NPPlayer.instance.playerInfo.curLevelRef.exp;
                if (showExp < 0)
                    showExp = 0;

                _m_wExpProgress.showWnd();
                if(NPPlayer.instance.playerInfo.nextLevelRef != null && NPPlayer.instance.playerInfo.curLevelRef != null)
                    _m_wExpProgress.setProgress(showExp, (NPPlayer.instance.playerInfo.nextLevelRef.exp - NPPlayer.instance.playerInfo.curLevelRef.exp), EValueFormatType.NORMAL_NOT_LARGE_STR);
                else
                    _m_wExpProgress.setProgress(showExp, showExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }
            string key = string.IsNullOrEmpty(wnd.txtAddExpKey) ? TransKeyConst.common_add_num : wnd.txtAddExpKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddExp, TextTranslate.instance.getLanguage(key, _m_eventResultInfo.addExp));
            
            ALUGUICommon.setGameObjEnable(wnd.hasAddExpShow, _m_eventResultInfo.addExp > 0);
        }
        
        /// <summary>
        /// 刷新赚速
        /// </summary>
        private void _refreshEarnings()
        {
            if(wnd == null || _m_eventResultInfo == null)
                return;
            
            long nowEarnings = NPPlayer.instance.specialItemComp.goldData.earnings;//获取当前赚速
            //设置玩家赚速条，用经验值减去当前等级的初始经验
            if (null != _m_wEarningsProgress)
            {
                _m_wEarningsProgress.showWnd();
                
                if(NPPlayer.instance.playerInfo.nextLevelRef != null)
                    _m_wEarningsProgress.setProgress(nowEarnings, NPPlayer.instance.playerInfo.nextLevelRef.earnings, EValueFormatType.GOLD);
                else
                    _m_wEarningsProgress.setProgress(nowEarnings, nowEarnings, EValueFormatType.GOLD);
            }
            string key = string.IsNullOrEmpty(wnd.txtAddEarningsKey) ? TransKeyConst.common_add_num : wnd.txtAddEarningsKey;
            ALUGUICommon.setLabelTxt(wnd.txtAddEarnings, TextTranslate.instance.getLanguage(key, (nowEarnings - _m_eventResultInfo.oldEarnings).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            
            ALUGUICommon.setGameObjEnable(wnd.hasAddEarningsShow, nowEarnings > _m_eventResultInfo.oldEarnings);
        }
        
        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();
        
        protected abstract void _onRefreshWnd();

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}