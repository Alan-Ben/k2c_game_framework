using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using GS2GC.p004_PlayerOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带玩家头像的聊天信息窗体基类
    /// </summary>
    public abstract class _ATNPGGUIWndPlayerChatMsgItem<T_MONO, T_DATA> : _ANPGUISubWndChatMsgListItem<T_MONO, T_DATA>
        where T_MONO : _ANPGGUIMonoPlayerChatMsgItem
        where T_DATA : _IMsgItemData
    {
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;//玩家头像
        private long _m_lShowSerialize;//显示序列号

        protected _ATNPGGUIWndPlayerChatMsgItem(T_DATA _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {

        }

        protected abstract long senderPlayerCid { get; }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if (_m_playerInfoWnd != null)
            {
                _refreshPlayerInfo(_m_playerInfoWnd);
                _m_playerInfoWnd.showWnd();
            }

            _onShowWndEx();
        }

        protected abstract void _onShowWndEx();

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_playerInfoWnd?.hideWnd();
            _onHideWndEx();
        }

        protected abstract void _onHideWndEx();

        protected override void _onReset()
        {
            _m_playerInfoWnd?.resetWnd();
            _onResetEx();
        }

        protected abstract void _onResetEx();

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.onClickAction -= _onClickPlayerInfo;
                _m_playerInfoWnd.discard();
            }
            _m_playerInfoWnd = null;
            _onDiscardEx();
        }

        protected abstract void _onDiscardEx();

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            base._onWndInitDone();

            //玩家信息
            if (wnd.monoPlayer != null)
            {
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.monoPlayer);
                _m_playerInfoWnd.onClickAction += _onClickPlayerInfo;
            }

            _onWndInitDoneEx();
        }

        protected abstract void _onWndInitDoneEx();

        public override float getHeight()
        {
            if (_originMono == null)
                return 0f;

            RectTransform wndRect = _originMono.GetComponent<RectTransform>();
            return wndRect == null ? 0 : wndRect.rect.height;
        }


        #region 点击事件

        /// <summary>
        /// 点击玩家头像
        /// </summary>
        private void _onClickPlayerInfo()
        {
            long serialize = _m_lShowSerialize;
            //打开玩家详情
            GCommon.reqPlayerInfo(senderPlayerCid, (_info) =>
            {
                if (wnd == null || null == getGameObj() || !getGameObj().activeInHierarchy || _m_playerInfoWnd == null || _m_playerInfoWnd.wnd == null || _info == null || _m_lShowSerialize != serialize)
                    return;

                RectTransform targetRectTransform = (null != _m_playerInfoWnd.wnd.locateRectTrans) ? _m_playerInfoWnd.wnd.locateRectTrans : this.rectTransform;
                
                WinMsg.SendMsg(WinMsgType.ON_TO_SHOW_ALL_ITEM, targetRectTransform);

                RectTransform rangTrans = GGUIWndChat.instance.getChatInfoListViewport();
                GCommon.showPlayerInfoWndTip(_info,targetRectTransform,0,rangTrans);
            });
        }

        #endregion

        protected abstract void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd);
    }
}
