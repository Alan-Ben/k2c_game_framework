using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 一条伙伴分享信息的msgItem
    /// </summary>
    public class GGUIWndChatMsgItemShareHero : _AGGUIWndChatMsgItemShareCommon<ChatShareHeroMsgDetailInfo>
    {
        public GGUIWndChatMsgItemShareHero(ChatShareHeroMsgDetailInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
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
            QueueMgr.instance.AddNode(new NPGMainQueueChatShareHeroDetailNode(detailInfo.content));
        }

        protected override void _refreshWndEx()
        {
            setColor(EChatShareType.HERO);
        }

        protected override string _getShareContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_hero_str, GCommon.getItemName(ENPItemType.HERO, detailInfo.content.getHeroId()));
        }
    }
}