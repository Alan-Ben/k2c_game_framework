
using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 一条妃子分享信息的msgItem
    /// </summary>
    public class GGUIWndChatMsgItemShareConsort : _AGGUIWndChatMsgItemShareCommon<ChatShareConsortMsgDetailInfo>
    {


        public GGUIWndChatMsgItemShareConsort(ChatShareConsortMsgDetailInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
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
            
            QueueMgr.instance.AddNode(new NPGMainQueueChatShareConsortDetailNode(detailInfo.content));
            // QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareConsortDetail.instance,
            //     () =>
            //     {
            //         GGUIWndShareConsortDetail.instance.showWnd();
            //         GGUIWndShareConsortDetail.instance.setInfo(detailInfo.content);
            //     });
        }

        protected override void _refreshWndEx()
        {
            setColor(EChatShareType.CONSORT);
        }

        protected override string _getShareContent()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.chat_share_consort_str, GCommon.getItemName(ENPItemType.CONSORT, detailInfo.content.getConsortId()));
        }
    }
}