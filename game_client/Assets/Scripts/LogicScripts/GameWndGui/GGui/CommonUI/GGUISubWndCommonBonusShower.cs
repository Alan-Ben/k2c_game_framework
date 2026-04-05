using ALPackage;

namespace GOE
{
    public class GGUISubWndCommonBonusShower : _ATALBasicUISubWnd<GGUIMonoCommonBonusShower>
    {
        private UnionBonus _m_bonus;
        private JudgeUnionBonusPart[] _m_bonusParts;
        
        
        public GGUISubWndCommonBonusShower(GGUIMonoCommonBonusShower _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            refreshWnd();
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


        public void refreshWnd(UnionBonus _bonus, JudgeUnionBonusPart[] _bonusParts = null)
        {
            _m_bonus = _bonus;
            _m_bonusParts = _bonusParts;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_bonus == null)
                return;
            
            long value = _m_bonus.getValue(wnd.bonusType, _m_bonusParts);
            string key = wnd.txtBonusKey;
            string valueStr = wnd.isPercentage ? (value / 100f).ToString() : value.ToString();
            ALUGUICommon.setLabelTxt(wnd.txtBonus, string.IsNullOrEmpty(key) ? valueStr : TextTranslate.instance.getLanguage(key, valueStr));
            ALUGUICommon.setGameObjEnable(wnd.goZeroShowList, value == 0);
            ALUGUICommon.setGameObjEnable(wnd.goZeroHideList, value != 0);
        }
    }
}