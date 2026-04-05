using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱-礼包宝箱页面
    /// </summary>
    public class GGUIWndGuildGiftBoxPage : _AGGUIWndGuildBoxPageBase<GGUIMonoGuildGiftBoxPage>
    {
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonToggleEx _m_toggleAnonymousSendWnd;  // 匿名发送
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildGiftBoxPage(Transform _parent) : base(EGuildBoxType.GUILD_GIFT_BOX, GGUIMonoGuildGiftBoxPage.assetPath, GGUIMonoGuildGiftBoxPage.objName, _parent)
        {
        }
    
        protected override void _onShowWndEx()
        {
        }
    
        protected override void _onHideWndEx()
        {
            _m_toggleAnonymousSendWnd?.hideWnd();
        }
    
        protected override void _onResetEx()
        {
            _m_toggleAnonymousSendWnd?.resetWnd();
        }
    
        protected override void _onDiscardEx()
        {
            _m_toggleAnonymousSendWnd?.discard();
            _m_toggleAnonymousSendWnd = null;
        }
    
        protected override void _onWndInitDoneEx()
        {
            if(null == wnd)
                return;
                
            if (wnd.toggleAnonymousSend != null)
            {
                _m_toggleAnonymousSendWnd = new NPGGUIWndCommonToggleEx(wnd.toggleAnonymousSend);
                _m_toggleAnonymousSendWnd.clickDelegate += _onToggleClicktoggleAnonymousSend;
            }
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        protected override void _onRefreshWnd()
        {
            if (wnd == null)
                return;

            if (_m_toggleAnonymousSendWnd != null)
            {
                _m_toggleAnonymousSendWnd.showWnd();
                _m_toggleAnonymousSendWnd.setSelected(NPPlayer.instance.guildBoxComp.isGuildBoxShareAnonymous);
            }

            //是否可一键领取
            ALUGUICommon.setGameObjEnable(wnd.canCollectAllShowList, NPPlayer.instance.guildBoxComp.getBoxCount(EGuildBoxType.GUILD_GIFT_BOX) >= wnd.giftBoxShowCollectAllCount);
        }

        /// <summary>
        /// 匿名按钮点击
        /// </summary>
        /// <param name="_toggleWnd"></param>
        private void _onToggleClicktoggleAnonymousSend(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if(_toggleWnd == null)
                return;
            _toggleWnd.setSelected(!_toggleWnd.isOn);

            // 记录选中设置
            NPPlayer.instance.guildBoxComp.isGuildBoxShareAnonymous = _toggleWnd.isOn;
            NPPlayer.instance.guildBoxComp.reqSetGuildBoxShareAnonymous(_toggleWnd.isOn);
        }

        /// <summary>
        /// 领取完宝箱奖励推送
        /// </summary>
        protected override void _onGuildBoxGetReward()
        {
            _onRefreshWnd();
        }
    }
}