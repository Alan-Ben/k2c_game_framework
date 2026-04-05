using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// FirstTeam 主界面
    /// </summary>
    public class GGUIWndFirstTeamActivity : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoFirstTeamActivity>
    {
        private string _m_sAssetPath;
        private string _m_sObjName;

        public GGUIWndFirstTeamActivity(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName   = _assetPathInfo.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName   { get { return _m_sObjName;   } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD,       _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE,    _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE,     _onActivityChg);

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD,       _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_UPDATE,    _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE,     _onActivityChg);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnTeamHall,   _onClickTeamHall);
            ALUGUICommon.uncombineBtnClick(wnd.btnCreateTeam, _onClickCreateTeam);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnTeamHall,   _onClickTeamHall);
            ALUGUICommon.combineBtnClick(wnd.btnCreateTeam, _onClickCreateTeam);
        }

        private void _refreshWnd()
        {
            // TODO: 刷新界面数据
        }

        private void _onActivityChg(params object[] _objects)
        {
            _refreshWnd();
        }

        private void _onClickTeamHall(GameObject _go)
        {
            // TODO: 接入组队大厅逻辑
        }

        private void _onClickCreateTeam(GameObject _go)
        {
            // TODO: 接入创建队伍逻辑
        }
    }
}
