using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUISubWndLoverCollectBtn : _ATALBasicUISubWnd<GGUIMonoLoverCollectBtn>
    {
        private bool _m_isEarningsReached;
        
        
        public GGUISubWndLoverCollectBtn(GGUIMonoLoverCollectBtn _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd(false);
            NPPlayer.instance.specialItemComp.goldData.onEarningsChg += _onEarningsChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.specialItemComp.goldData.onEarningsChg -= _onEarningsChg;
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnEnter, _onBtnEnterClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnEnter, _onBtnEnterClick);
        }


        public void refreshWnd(bool _checkState)
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long curEarnings = NPPlayer.instance.getValue(ENPPlayerValueType.EARNINGS);
            long needEarnings = GRefdataCoreMgr.instance.npGeneral.lover_collect_need_earn_speed;
            bool canRescue = curEarnings >= needEarnings;
            if (_checkState && canRescue == _m_isEarningsReached)
                return;

            _m_isEarningsReached = canRescue;
            wnd.setCanRescueState(canRescue);

            float progress = needEarnings > 0 ? Mathf.Clamp01((float)curEarnings / needEarnings) : 0;
            if (wnd.sliderProgress != null)
                wnd.sliderProgress.value = progress;
            int percent = (int)(progress * 100);
            ALUGUICommon.setLabelTxt(wnd.txtProgressPercent, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, percent));
        }
        
        
        private void _onEarningsChg()
        {
            refreshWnd(true);
        }
        private void _onBtnEnterClick(GameObject _obj)
        {
            GCommon.enterUIMainNodeShow(ESysSceneType.LOVER_COLLECT);
        }
    }
}
