using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 卷王子嗣buff
    /// </summary>
    public class GGUIWndConsortInviteBuffItem_GiftedChildBuff : _AGGUIWndConsortInviteBuffItem<GGUIMonoConsortInviteBuffItem_GiftedChildBuff>
    {
        public GGUIWndConsortInviteBuffItem_GiftedChildBuff(GGUIMonoConsortInviteBuffItem_GiftedChildBuff _wnd) : base(_wnd)
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
            
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
        }

        protected override void _onResetSub()
        {
        }

        protected override void _showToolTip(long _assetPathId, RectTransform _targetTransRoot, float _intervalX, float _intervalY)
        {
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_ConsortGiftedChildBuff(_assetPathId, _targetTransRoot, _intervalX, _intervalY));
        }

        private void _refreshWnd()
        {
            setBuffCount(NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.BIRTH_GIFTDE_COUM));
        }

        /// <summary>
        /// 当玩家参数变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onPlayerParamChg(object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is int _paramIndex))
                return;
            
            if(_paramIndex == (int)ENPPlayerParam.BIRTH_GIFTDE_COUM)
                _refreshWnd();
        }
    }
}