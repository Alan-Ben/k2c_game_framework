using System;
using ALPackage;
using Common.TravelEnum;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 一键游历 - 博彩事件结果item
    /// </summary>
    public class GGUIWndAkeyTravelGambleEventResultItem : _AGGUIWndAkeyTravelResultItem<GGUIMonoAkeyTravelGambleEventResultItem>
    {
        private TravelGambleEventResultInfo _m_gambleResultInfo;

        private GGUIWndCommonSimpleItem _m_wBeforeItem;
        private GGUIWndCommonSimpleItem _m_wAfterItem;

        public GGUIWndAkeyTravelGambleEventResultItem(GGUIMonoAkeyTravelGambleEventResultItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneSub()
        {
            if (wnd == null)
                return;

            if (wnd.monoBeforeItem != null)
                _m_wBeforeItem = new GGUIWndCommonSimpleItem(wnd.monoBeforeItem);

            if (wnd.monoAfterItem != null)
                _m_wAfterItem = new GGUIWndCommonSimpleItem(wnd.monoAfterItem);
        }

        protected override void _onDiscardSub()
        {
            _m_gambleResultInfo = null;

            _m_wBeforeItem?.discard();
            _m_wBeforeItem = null;

            _m_wAfterItem?.discard();
            _m_wAfterItem = null;
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
            _m_gambleResultInfo = null;
            _m_wBeforeItem?.resetWnd();
            _m_wAfterItem?.resetWnd();
        }

        protected override void _onSetData(_ITravelEventResultInfo _data)
        {
            _m_gambleResultInfo = _data as TravelGambleEventResultInfo;
        }

        protected override void _onRefreshWnd()
        {
            if (wnd == null)
                return;

            ETravelGambleResult resultType = _m_gambleResultInfo?.travelGambleResult ?? ETravelGambleResult.NONE;
            long betAmount = _m_gambleResultInfo?.betAmount ?? 0;
            long diamondChange = _m_gambleResultInfo?.diamondChange ?? 0;
            long returnAmount = betAmount + diamondChange;

            if (resultType == ETravelGambleResult.ABANDON)
            {
                // 放弃：押注量和结算量均显示 0
                _m_wBeforeItem?.showWnd();
                _m_wBeforeItem?.setItem(ENPItemType.CURRENCY, (long)ECurrency.GEM, 0);

                _m_wAfterItem?.showWnd();
                _m_wAfterItem?.setItem(ENPItemType.CURRENCY, (long)ECurrency.GEM, 0);
            }
            else
            {
                // 正常博彩结果：押注量 → 结算量
                _m_wBeforeItem?.showWnd();
                _m_wBeforeItem?.setItem(ENPItemType.CURRENCY, (long)ECurrency.GEM, betAmount);

                _m_wAfterItem?.showWnd();
                _m_wAfterItem?.setItem(ENPItemType.CURRENCY, (long)ECurrency.GEM, returnAmount);

                if (diamondChange < 0)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtDiamondChange, diamondChange);       
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtDiamondChange, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, diamondChange));
                }
            }
            
            NPCommonEnumStatMutexShowInfo<ETravelGambleResult>.setStat(wnd.resultShowList, resultType);
        }
    }
}
