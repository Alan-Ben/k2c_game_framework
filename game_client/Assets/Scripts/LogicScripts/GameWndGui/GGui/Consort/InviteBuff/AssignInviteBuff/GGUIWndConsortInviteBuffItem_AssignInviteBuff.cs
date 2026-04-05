using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortInviteBuffItem_AssignInviteBuff : _AGGUIWndConsortInviteBuffItem<GGUIMonoConsortInviteBuffItem_AssignInviteBuff>
    {
        public GGUIWndConsortInviteBuffItem_AssignInviteBuff(GGUIMonoConsortInviteBuffItem_AssignInviteBuff _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDoneSub()
        {
        }

        protected override void _onDiscardSub()
        {
        }

        protected override void _onShowWndSub()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_ASSIGN_INVITE_CONSORT_CHG, _refreshWnd);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ASSIGN_INVITE_CONSORT_CHG, _refreshWnd);
        }

        protected override void _onResetSub()
        {
        }

        protected override void _showToolTip(long _assetPathId, RectTransform _targetTransRoot, float _intervalX, float _intervalY)
        {
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_ConsortAssignInviteBuff(_assetPathId, _targetTransRoot, _intervalX, _intervalY));
        }

        private void _refreshWnd()
        {
            setBuffCount(NPPlayer.instance.consortComp.randCallConsortIdList?.Count ?? 0);
        }
    }
}