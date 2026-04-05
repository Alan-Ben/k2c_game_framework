using ALPackage;

namespace GOE
{
    public class GGUIWndEveningDungeonRankTopPlayerInfo : _AGGUIWndBaseSubRankPlayerInfo<GGUIMonoEveningDungeonRankTopPlayerInfo, EveningDungeonRankInfo>
    {
        public GGUIWndEveningDungeonRankTopPlayerInfo(GGUIMonoEveningDungeonRankTopPlayerInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        public override void setInfo(EveningDungeonRankInfo _info)
        {
            if (wnd == null)
                return;

            if (_info == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, false);
                base.setInfo(_info);
            }
        }
    }
}