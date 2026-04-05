using ALPackage;
using CommonEnum;
using NPEnum;
using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历博彩押注窗口
    /// </summary>
    public class GGUIWndTravelGambleAnte : _ANPGGUIBasicWnd<GGUIMonoTravelGambleAnte>
    {
        private static GGUIWndTravelGambleAnte _g_instance;
        public static GGUIWndTravelGambleAnte instance { get { return _g_instance ??= new GGUIWndTravelGambleAnte(); } }

        private TravelGambleEventInfo _m_eventInfo;
        private long _m_lAnteNum;//下注数量
        
        private NPGGUIWndCommonItem _m_wCommonItem;
        private GGUIWndBagPopCounterEx _m_wAnteCounter;
        
        public GGUIWndTravelGambleAnte() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelGambleAnte.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelGambleAnte.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCommonItem != null)
                _m_wCommonItem = new NPGGUIWndCommonItem(wnd.monoCommonItem);

            if (wnd.anteNumCounter != null)
            {
                _m_wAnteCounter = new GGUIWndBagPopCounterEx(wnd.anteNumCounter);
                _m_wAnteCounter.regCounterChangedEvent(_onAnteChg);
            }

            ALUGUICommon.combineBtnClick(wnd.anteBtn, _onAnteBtnClick);
            ALUGUICommon.combineBtnClick(wnd.abandonBtn, _onAbandonBtnClick);
            ALUGUICommon.combineBtnClick(wnd.laterBtn, _onLaterBtnClick);
        }
        
        protected override void _onDiscard()
        {
            _m_wCommonItem?.discard();
            _m_wCommonItem = null;

            _m_wAnteCounter?.discard();
            _m_wAnteCounter = null;

            _m_eventInfo = null;
            
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.anteBtn, _onAnteBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.abandonBtn, _onAbandonBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.laterBtn, _onLaterBtnClick);
            }
        }

        protected override void _onShowWnd()
        {
            _m_wCommonItem?.showWnd();
            _m_wAnteCounter?.showWnd();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wCommonItem?.hideWnd();
            _m_wAnteCounter?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonItem?.resetWnd();
            _m_wAnteCounter?.resetWnd();
        }

        /// <summary>
        /// 传入博彩事件数据并刷新窗口
        /// </summary>
        public void setData(TravelGambleEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            _m_lAnteNum = _m_eventInfo?.selectedAnteCount ?? 0;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_eventInfo == null || !isShow)
                return;

            TravelEventGambleRefObj refObj = _m_eventInfo.gambleEventRefObj;
            if (refObj == null)
                return;

            // 显示押注道具（钻石）
            long playerGems = GCommon.getItemCount(ENPItemType.CURRENCY, (long)ECurrency.GEM);
            NPCommonCostItem gemItem = new NPCommonCostItem(ENPItemType.CURRENCY, (long)ECurrency.GEM, playerGems);
            _m_wCommonItem?.showWnd();
            _m_wCommonItem?.setItem(gemItem);

            long canSelectMaxValue = Math.Min(playerGems, refObj.max_ante);//可下注的最大值:玩家钻石数和下注事件最大值中较小的一个
            _m_wAnteCounter?.init(canSelectMaxValue, _m_lAnteNum, refObj.min_ante);

            _refreshCanAnteShow();
        }

        private void _refreshCanAnteShow()
        {
            if (wnd == null || _m_eventInfo == null || !isShow)
                return;
            
            bool canAnte = _checkCanAnte(false);
            ALUGUICommon.setGameObjEnable(wnd.canAnteShowList, canAnte);
            ALUGUICommon.setGameObjEnable(wnd.cannotAnteShowList, !canAnte);
        }
        
        /// <summary>
        /// 检查是否可下注
        /// </summary>
        /// <param name="_showTip"></param>
        /// <returns></returns>
        private bool _checkCanAnte(bool _showTip)
        {
            if (_m_eventInfo == null || _m_eventInfo.gambleEventRefObj == null)
                return false;

            if (_m_lAnteNum < _m_eventInfo.gambleEventRefObj.min_ante)//下注金额小于最小下注金额时
            {
                if(_showTip)
                    GCommon.dealItemNotEnough(ENPItemType.CURRENCY, (long) ECurrency.GEM);
                return false;
            }
            
            if (!GCommon.isItemEnough(ENPItemType.CURRENCY, (long) ECurrency.GEM, _m_lAnteNum, _showTip))
                return false;

            return true;
        }

        /// <summary>
        /// 当下注金额变化时
        /// </summary>
        /// <param name="_value"></param>
        private void _onAnteChg(long _value)
        {
            if(_m_eventInfo == null || _m_eventInfo.gambleEventRefObj == null)
                return;

            _m_lAnteNum = _value;
            _refreshCanAnteShow();
        }
        
        /// <summary>
        /// 点击下注按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onAnteBtnClick(GameObject _go)
        {
            if (_m_eventInfo == null)
                return;

            if (!_checkCanAnte(true))
                return;

            _m_eventInfo.selectedAnteCount = (int)_m_lAnteNum;
            _m_eventInfo.anteState = ETravelGambleAnteState.ANTED;
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.travel_gambleEventAnteTip_num, _m_eventInfo.selectedAnteCount));
            _dealClose();
        }

        /// <summary>
        /// 点击放弃下注
        /// </summary>
        /// <param name="_go"></param>
        private void _onAbandonBtnClick(GameObject _go)
        {
            if (_m_eventInfo == null)
                return;

            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.travel_gambleEventWaivePopWndTip), 
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    _m_eventInfo.anteState = ETravelGambleAnteState.WAIVE;
                    _dealClose();
                });
        }

        /// <summary>
        /// 点击稍后继续
        /// </summary>
        /// <param name="_go"></param>
        private void _onLaterBtnClick(GameObject _go)
        {
            if (_m_eventInfo == null)
                return;

            _dealClose();
        }

        private void _dealClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_GAMBLE_ANTE_DEAL_WND);
        }
    }
}