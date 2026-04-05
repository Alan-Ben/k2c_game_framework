
using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 宴会分享
    /// </summary>
    public class NPGGUIWndChatMsgItemDinnerInvite : _ATNPGGUIWndPlayerChatMsgItem_JumpTo<NPGGUIMonoChatMsgItemDinnerInvite, NPChatMsgDinnerInviteInfo>
    {
        
        //刷新操作序列号
        private long _m_lRefreshSerialize;
        private bool _m_canEnterDinner;

        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        public NPGGUIWndChatMsgItemDinnerInvite(NPChatMsgDinnerInviteInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override void _onShowWndEx()
        {
            _refresh();
        }

        protected override void _onHideWndEx()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();

        }

        protected override void _onResetEx()
        {
            
        }

        protected override void _onDiscardEx()
        {

            _m_lRefreshSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onWndInitDoneEx()
        {
            if (null == wnd)
                return;
        }

        /// <inheritdoc/>
        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }

        private void _refresh()
        {
            if (wnd == null || detailInfo == null || detailInfo.content == null)
                return;
            GDinnerTypeRefObj dinnerTypeRefObj = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef((long) detailInfo.content.getDinnerId());
            string dinnerName = TextTranslate.instance.getLanguage(null != dinnerTypeRefObj ? dinnerTypeRefObj.name : null);
            //我的城堡正在举办{0}，期待你的到来
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.dinner_chat_invite_desc, dinnerName));
            // {0}点击进入
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.dinner_chat_invite_title, dinnerName));
            //宾客席位{0}/{1}
            ALUGUICommon.setLabelTxt(wnd.txtSeatCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_chat_seat_count, detailInfo.content.getJoinerCount(),dinnerTypeRefObj.default_seat_num));
            _m_canEnterDinner = false;
            _refreshDinnerInfo();
        }

        private void _refreshDinnerInfo()
        {
            if (null == wnd)
                return;
            long serializeId = _m_lRefreshSerialize = ALSerializeOpMgr.next();

            NPPlayer.instance.dinnerComp.reqCheckDinnerInvite(detailInfo.content.getInstanceId(), (_isSuc, _curCount) =>
            {
                if(serializeId != _m_lRefreshSerialize)
                    return;
                _m_canEnterDinner = _isSuc;
                GDinnerTypeRefObj dinnerTypeRefObj = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef((long) detailInfo.content.getDinnerId());
                bool isEnd = !_m_canEnterDinner || _curCount >= dinnerTypeRefObj.default_seat_num;
                ALUGUICommon.setGameObjEnable(wnd.goEndShowList, isEnd);
                if(isEnd)
                    GGameCommonInfo.grayImage(wnd.goGrayList);
                else
                    GGameCommonInfo.disgrayImage(wnd.goGrayList);

                ALUGUICommon.setGameObjEnable(wnd.goEndHideList, !isEnd);
                //宾客席位{0}/{1}
                ALUGUICommon.setLabelTxt(wnd.txtSeatCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_chat_seat_count, _curCount, dinnerTypeRefObj.default_seat_num));
            });
        }

        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }
        
        protected override ENPFunctionType functionType { get => ENPFunctionType.DINNER; }
        protected override void _onClickJump()
        {
            if (!_m_canEnterDinner)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_is_over_tip));
                return;
            }
            
            GCommon.enterDinner(new GDinnerInfo(detailInfo.content.getInstanceId()), null, _refresh, null,  _refresh);
        }
    }
}