using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地民意求助项窗口
    /// </summary>
    public class GGUIWndMarsPopularWillHelpItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMarsPopularWillHelpItem>
    {
        private _IMarsPeopleWillHelp _m_iHelpInfo;
        private NPGGuiWndTexture _m_npcHeadIconWnd; // npc头像

        public GGUIWndMarsPopularWillHelpItem(GGUIMonoMarsPopularWillHelpItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建npc头像
            if (wnd.npcHeadIcon != null)
                _m_npcHeadIconWnd = new NPGGuiWndTexture(wnd.npcHeadIcon);

            // 绑定处理按钮
            ALUGUICommon.combineBtnClick(wnd.btnDeal, _onClickDeal);
        }

        protected override void _onDiscard()
        {
            _m_iHelpInfo = null;

            // 销毁npc头像
            _m_npcHeadIconWnd?.discard();
            _m_npcHeadIconWnd = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDeal, _onClickDeal);
            }
        }

        protected override void _onShowWnd()
        {
            _m_npcHeadIconWnd?.showWnd();
            
            refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_HELP_UPDATE, _onHelpInfoChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_HELP_UPDATE, _onHelpInfoChg);
            
            _m_npcHeadIconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_npcHeadIconWnd?.discardTexture();
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 赋值数据
        /// </summary>
        public void setData(_IMarsPeopleWillHelp _help)
        {
            _m_iHelpInfo = _help;
            refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iHelpInfo == null)
                return;

            // 求助名（使用多语言）
            if (!string.IsNullOrEmpty(wnd.txtHelpNameKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtHelpName, TextTranslate.instance.getLanguage(wnd.txtHelpNameKey, _m_iHelpInfo.npcRefObj?.Name));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtHelpName, TextTranslate.instance.getLanguage(_m_iHelpInfo.npcRefObj?.Name));
            }

            // npc头像
            _m_npcHeadIconWnd?.setTexture(_m_iHelpInfo.npcRefObj?.npcIcon);

            // 状态显示（互斥状态组）
            if (wnd.stateShowList != null)
                NPCommonEnumStatMutexShowInfo<EMarsPopularWillHelpState>.setStat(wnd.stateShowList, _m_iHelpInfo.state);
        }

        private void _onClickDeal(GameObject _go)
        {
            _m_iHelpInfo?.deal();
        }

        public void simulateClick()
        {
            _onClickDeal(null);
        }
        
        #region 窗口消息

        /// <summary>
        /// 求助信息变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onHelpInfoChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is _IMarsPeopleWillHelp chgHelpInfo) 
               || _m_iHelpInfo == null || chgHelpInfo.instanceId != _m_iHelpInfo.instanceId)
                return;
            
            refreshWnd();
        }

        #endregion
    }
}
