using System;
using ALPackage;
using Common.TravelEnum;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历博彩事件结果窗口
    /// </summary>
    public class GGUIWndTravelGambleEventResult : _ATravelResultWnd<GGUIMonoTravelGambleEventResult, TravelGambleEventResultInfo>
    {
        private GGUIWndCommonSimpleItem _m_wBeforeItem;  // 押注前钻石组件
        private GGUIWndCommonSimpleItem _m_wAfterItem;   // 押注后钻石组件

        public GGUIWndTravelGambleEventResult(TravelGambleEventResultInfo _eventResultInfo)
            : base(_eventResultInfo, GGUIMonoTravelGambleEventResult.uiResPathId, EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if (wnd == null)
                return;

            if (wnd.monoBeforeItem != null)
                _m_wBeforeItem = new GGUIWndCommonSimpleItem(wnd.monoBeforeItem);

            if (wnd.monoAfterItem != null)
                _m_wAfterItem = new GGUIWndCommonSimpleItem(wnd.monoAfterItem);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onDiscardSub()
        {
            _m_wBeforeItem?.discard();
            _m_wBeforeItem = null;

            _m_wAfterItem?.discard();
            _m_wAfterItem = null;

            if (wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wBeforeItem?.hideWnd();
            _m_wAfterItem?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wBeforeItem?.resetWnd();
            _m_wAfterItem?.resetWnd();
        }

        protected override void _onRefreshWnd()
        {
            if (wnd == null || _m_eventResultInfo == null || _m_eventResultInfo.eventInfo == null)
                return;

            ETravelGambleResult resultType = _m_eventResultInfo.gambleResult?.getResultType() ?? ETravelGambleResult.NONE;
            long diamondChange = _m_eventResultInfo.diamondChange;
            long diamondChangeAbs = Math.Abs(_m_eventResultInfo.diamondChange);

            ALUGUICommon.setGameObjEnable(wnd.winShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.loseShowList, false);
            ALUGUICommon.setGameObjEnable(wnd.jackpotShowList, false);
            switch (resultType)
            {
                case ETravelGambleResult.WIN:
                    ALUGUICommon.setGameObjEnable(wnd.winShowList, true);
                    ALUGUICommon.setLabelTxt(wnd.txtDiamondChange, TextTranslate.instance.getLanguage(TransKeyConst.travel_gamble_result_win_gain_num, diamondChangeAbs));
                    break;
                
                case ETravelGambleResult.LOSE:
                    ALUGUICommon.setGameObjEnable(wnd.loseShowList, true);
                    ALUGUICommon.setLabelTxt(wnd.txtDiamondChange, TextTranslate.instance.getLanguage(TransKeyConst.travel_gamble_result_lose_loss_num, diamondChangeAbs));
                    break;
                
                case ETravelGambleResult.JACKPOT:
                    ALUGUICommon.setGameObjEnable(wnd.jackpotShowList, true);
                    ALUGUICommon.setLabelTxt(wnd.txtDiamondChange, TextTranslate.instance.getLanguage(TransKeyConst.travel_gamble_result_jakpot_gain_num, diamondChangeAbs));
                    break;
                
                default:
                    _onCloseBtnClick(null);
                    break;
            }
            
            // 押注量（before）：玩家下注的数量
            long betAmount = _m_eventResultInfo?.betAmount ?? 0;
            // 结算返还量（after）：betAmount + diamondChange（WIN=2x/LOSE=0/JACKPOT=jackpotReturn）
            long returnAmount = betAmount + diamondChange;
            if (_m_wBeforeItem != null)
            {
                _m_wBeforeItem.showWnd();
                _m_wBeforeItem.setItem(ENPItemType.CURRENCY, (long) ECurrency.GEM, betAmount);
            }

            if (_m_wAfterItem != null)
            {
                _m_wAfterItem.showWnd();
                _m_wAfterItem.setItem(ENPItemType.CURRENCY, (long) ECurrency.GEM, returnAmount);
            }
        }

        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_GAMBLE_EVENT_RESULT);
        }
    }
}
