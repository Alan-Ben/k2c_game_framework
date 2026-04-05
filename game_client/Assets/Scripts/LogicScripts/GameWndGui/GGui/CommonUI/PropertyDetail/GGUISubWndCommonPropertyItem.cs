
using ALPackage;
using GOE.BonusSpace;

namespace GOE
{
    public class GGUISubWndCommonPropertyItem : _ATALBasicUISubWnd<GGUIMonoCommonPropertyItem>
    {
        private JudgeUnionBonusPart[] _m_judgePartData;
        
        
        public GGUISubWndCommonPropertyItem(GGUIMonoCommonPropertyItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }


        public void refreshWnd(JudgeUnionBonusPart[] _judgeData)
        {
            _m_judgePartData = _judgeData;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;

            _AUnionBonusMgr bonusMgr = NPPlayer.instance.playerBonusMgr.findMgrByTag(wnd.tag);
            long value = bonusMgr?.getTotalPropertyBonus(wnd.propertyType, _m_judgePartData) ?? 0;
            string key = wnd.valueKey;
            string valueStr = wnd.isPercentage ? (value / 100f).ToString() : value.ToString();

            ALUGUICommon.setLabelTxt(wnd.txtValue, string.IsNullOrEmpty(key) ? valueStr : TextTranslate.instance.getLanguage(key, valueStr));
        }
    }
}