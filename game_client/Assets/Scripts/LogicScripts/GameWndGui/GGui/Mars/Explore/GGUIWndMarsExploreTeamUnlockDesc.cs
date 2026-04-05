using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExploreTeamUnlockDesc : _ANPGGUIBasicWnd<GGUIMonoMarsExploreTeamUnlockDesc>
    {
        private static GGUIWndMarsExploreTeamUnlockDesc _g_instance;
        [NotNull] public static GGUIWndMarsExploreTeamUnlockDesc instance { get { return _g_instance ??= new GGUIWndMarsExploreTeamUnlockDesc(); } }


        protected override string _monoAssetPath { get { return GGUIMonoMarsExploreTeamUnlockDesc.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExploreTeamUnlockDesc.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsExploreTeamUnlockDesc() : base(EALUIWndLayer.ADDITION)
        {
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
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            // TODO: Display team unlock conditions
            // This window likely shows information about how to unlock additional exploration teams
            // Could display:
            // - Required player level
            // - Required building level
            // - Cost to unlock
            // - Benefits of additional teams
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_TEAM_UNLOCK_DESC);
        }
    }
}
