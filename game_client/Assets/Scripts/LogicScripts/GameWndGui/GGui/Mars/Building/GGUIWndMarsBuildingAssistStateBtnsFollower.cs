using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科研所状态常驻HUD按钮控制器
    /// </summary>
    public class GGUIWndMarsBuildingAssistStateBtnsFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsBuildingAssistStateBtns, GGUIWndMarsBuildingAssistStateBtnsFollower>
    {
        private readonly GResPathIndex _m_resIndex;


        public GGUIWndMarsBuildingAssistStateBtnsFollowerController()
        {
            _m_resIndex = new GResPathIndex(7318); // 火星拓展-科研所状态常驻HUD
        }
        public GGUIWndMarsBuildingAssistStateBtnsFollowerController(GResPathIndex _resIndex)
        {
            _m_resIndex = _resIndex;
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }


        protected override GGUIWndMarsBuildingAssistStateBtnsFollower _createItemWnd(GGUIMonoMarsBuildingAssistStateBtns _wndMono)
        {
            GGUIWndMarsBuildingAssistStateBtnsFollower wnd = new GGUIWndMarsBuildingAssistStateBtnsFollower(_wndMono);
            wnd.showWnd();
            return wnd;
        }
        
        public void refreshWnd()
        {
            wnd?.refreshWnd();
        }
    }

    /// <summary>
    /// 科研所状态常驻HUD按钮跟随窗口
    /// </summary>
    public class GGUIWndMarsBuildingAssistStateBtnsFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsBuildingAssistStateBtns>
    {

        public GGUIWndMarsBuildingAssistStateBtnsFollower(GGUIMonoMarsBuildingAssistStateBtns _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, _onCanDealCountChg);
        }


        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, _onCanDealCountChg);
        }
        protected override void _onReset()
        {
           
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnAssist, _onBtnAssistClick);

           
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnAssist, _onBtnAssistClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            bool oneAssist = NPPlayer.instance.guildMarsHelpComp.canDealCount <= 1;
            ALUGUICommon.setLabelTxt(wnd.txtAssistCount, oneAssist ? "" : NPPlayer.instance.guildMarsHelpComp.canDealCount.ToString());
            bool hasAssistToDeal = NPPlayer.instance.guildComp.isJoinGuild() && NPPlayer.instance.guildMarsHelpComp.canDealCount > 0;
            ALUGUICommon.setGameObjEnable(wnd.hasAssistShowList, hasAssistToDeal);
            ALUGUICommon.setGameObjEnable(wnd.oneAssistHideList, !oneAssist);
        }

        private void _onCanDealCountChg()
        {
            refreshWnd();
        }

        /// <summary>
        /// 点击帮助按钮
        /// </summary>
        private void _onBtnAssistClick(GameObject _obj)
        {
            NPPlayer.instance.guildMarsHelpComp.reqDealMarsHelp(_suc =>
            {
                refreshWnd();
            });
        }

    }
}
