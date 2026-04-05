using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜宝箱分享banner
    /// </summary>
    public class GGUIWndChatMsgItemActivityRankBox : _ATNPGGUIWndPlayerChatMsgItem<GGUIMonoChatMsgItemActivityRankBox, ChatMsgActivityRankBoxInfo>
    {
        private GGUIWndChatMsgItemActivityRankBoxSubPrefab _m_wCommonBoxSubPrefab;//宝箱附加窗口
        public GGUIWndChatMsgItemActivityRankBox(ChatMsgActivityRankBoxInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override long senderPlayerCid { get { return detailInfo == null || detailInfo.sender == null ? 0 : detailInfo.sender.getCid(); } }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
        }

        protected override void _onHideWndEx()
        {
        }

        protected override void _onResetEx()
        {
            if (_m_wCommonBoxSubPrefab != null)
                _m_wCommonBoxSubPrefab.resetWnd();
        }

        protected override void _onDiscardEx()
        {
            if (wnd == null)
                return;

            if (_m_wCommonBoxSubPrefab != null)
                _m_wCommonBoxSubPrefab.discard();
            _m_wCommonBoxSubPrefab = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;


        }

        protected override void _discardAdditionTemplate()
        {

        }

        protected override void _loadAdditionTemplate([NotNull] ALStepCounter _stepCounter)
        {

        }
        
        public override float getHeight()
        {
            if (detailInfo == null || detailInfo.boxRefObj == null)
                return 0;
            
            return detailInfo.boxRefObj.ui_height;
        }
        
        //刷新窗口
        private void _refreshWnd()
        {
            _refreshSubPrefab();
        }

        //刷新附加窗口状态展示
        private void _refreshSubPrefab()
        {
            if (wnd == null || detailInfo == null || detailInfo.content == null)
                return;

            NPSOCommonBoxRefObj boxRef = detailInfo.boxRefObj;
            if (boxRef == null)
                return;

            if (_m_wCommonBoxSubPrefab != null)
            {
                _m_wCommonBoxSubPrefab.showWnd();
                _m_wCommonBoxSubPrefab.setInfo(detailInfo);
            }
            else
            {
                long uiResID = 0;
                if (senderPlayerCid == NPPlayer.instance.playerInfo.CID)
                    uiResID = boxRef.chat_my_sub_ui_res_id;
                else
                    uiResID = boxRef.chat_other_sub_ui_res_id;
                _m_wCommonBoxSubPrefab = new GGUIWndChatMsgItemActivityRankBoxSubPrefab(uiResID, wnd.transParent);
                _m_wCommonBoxSubPrefab.load(() =>
                {
                    if (_m_wCommonBoxSubPrefab == null)
                        return;
                    _m_wCommonBoxSubPrefab.showWnd();
                    _m_wCommonBoxSubPrefab.setInfo(detailInfo);
                });
            }
        }
        
        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }

    }
}
