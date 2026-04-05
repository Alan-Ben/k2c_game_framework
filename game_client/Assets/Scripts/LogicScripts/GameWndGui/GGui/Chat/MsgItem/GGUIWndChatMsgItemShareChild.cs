using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 一条子嗣分享信息的msgItem
    /// </summary>
    public class GGUIWndChatMsgItemShareChild : _AGGUIWndChatMsgItemShareCommon<ChatShareChildMsgDetailInfo>
    {
        public GGUIWndChatMsgItemShareChild(ChatShareChildMsgDetailInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }
        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }


        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }

        protected override void _clickInfoBtn(GameObject _)
        {
            QueueMgr.instance.AddNode(new NPGMainQueueChatShareChildDetailNode(detailInfo.content));
        }

        protected override void _refreshWndEx()
        {
            setColor(EChatShareType.CHILD);
        }

        protected override string _getShareContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_child_str, detailInfo?.content.getChildName());
        }
    }
}