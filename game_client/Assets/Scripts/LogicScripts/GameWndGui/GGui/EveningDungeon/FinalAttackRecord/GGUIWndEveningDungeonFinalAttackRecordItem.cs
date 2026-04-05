using ALPackage;

namespace GOE
{
    /// <summary>
    /// 晚间副本尾刀记录item
    /// </summary>
    public class GGUIWndEveningDungeonFinalAttackRecordItem : _ATALBasicUISubWnd<GGUIMonoEveningDungeonFinalAttackRecordItem>
    {
        private Common.DungeonObj.EveningDungeon_DefeatInfo _m_defeatInfo;
        
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        
        public GGUIWndEveningDungeonFinalAttackRecordItem(GGUIMonoEveningDungeonFinalAttackRecordItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
        }
        
        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        public void setData(Common.DungeonObj.EveningDungeon_DefeatInfo _defeatInfo)
        {
            _m_defeatInfo = _defeatInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_defeatInfo == null)
                return;

            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.setPlayer(_m_defeatInfo.getCid(), () =>
                {
                    _m_wPlayerIcon?.showWnd();
                });
            }

            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2StringHMS(TimeUtil.FromUTCMilliseconds(_m_defeatInfo.getDefeatTimeMs())));
        }
    }
}