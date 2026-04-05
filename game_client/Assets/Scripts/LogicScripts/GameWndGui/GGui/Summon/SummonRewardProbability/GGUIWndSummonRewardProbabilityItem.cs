using ALPackage;

namespace GOE
{
    public class GGUIWndSummonRewardProbabilityItem : _ATALBasicUISubWnd<GGUIMonoSummonRewardProbabilityItem>
    {
        private GachaItemShowInfoRefObj _m_rGachaItemShowInfoRefObj;
        
        private NPGGUIWndCommonItem _m_wItemWnd;
        
        public GGUIWndSummonRewardProbabilityItem(GGUIMonoSummonRewardProbabilityItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.rewardItem != null)
                _m_wItemWnd = new NPGGUIWndCommonItem(wnd.rewardItem);
        }
        
        protected override void _onDiscard()
        {
            _m_wItemWnd?.discard();
            _m_wItemWnd = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItemWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemWnd?.resetWnd();
        }

        public void setData(GachaItemShowInfoRefObj _refObj)
        {
            _m_rGachaItemShowInfoRefObj = _refObj;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_rGachaItemShowInfoRefObj == null)
                return;

            if (_m_wItemWnd != null && _m_rGachaItemShowInfoRefObj.gachaItemRefObj != null)
            {
                _m_wItemWnd.showWnd();
                _m_wItemWnd.setItem(_m_rGachaItemShowInfoRefObj.gachaItemRefObj.item);
            }

            ALUGUICommon.setLabelTxt(wnd.txtProbability, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_rGachaItemShowInfoRefObj.show_prob));
        }
    }
}