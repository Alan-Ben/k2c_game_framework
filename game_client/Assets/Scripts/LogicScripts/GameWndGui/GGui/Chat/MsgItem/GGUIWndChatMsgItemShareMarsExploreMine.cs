using System;
using ALPackage;
using ChatPackage;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一条火星探索矿分享信息的msgItem
    /// </summary>
    public class GGUIWndChatMsgItemShareMarsExploreMine : _ATNPGGUIWndPlayerChatMsgItem<GGUIMonoChatMsgItemMarsExploreMine, ChatShareMarsExploreMineMsgDetailInfo>
    {
        private long _m_lReqSerialize;
        private Mars_MineDynamic _m_mineInfo;

        public GGUIWndChatMsgItemShareMarsExploreMine(
            ChatShareMarsExploreMineMsgDetailInfo _detailInfo,
            Transform _parent,
            [NotNull] GUICacheMgrChatMsgItem _cacheMgr)
            : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override long senderPlayerCid
        {
            get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); }
        }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
        }

        protected override void _onHideWndEx()
        {
            _m_lReqSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnInfo, _onClickInfo);
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnInfo, _onClickInfo);
        }

        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }

        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }

        private void _refreshWnd()
        {
            if (wnd == null || detailInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(detailInfo.getMineNameTranslated()));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, detailInfo.getMineLevel()));

            _m_lReqSerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lReqSerialize;

            wnd.setLoadingShow(true);
            _reqServerInfo(detailInfo.content.getGuildShareMineMsgId(), (_mineInfo, _playerInfo) =>
            {
                if (tmpSerialize != _m_lReqSerialize)
                    return;

                wnd.setLoadingShow(false);

                _m_mineInfo = _mineInfo;
                bool isEmpty = _playerInfo == null;
                bool isSelf = !isEmpty && _playerInfo.getCid() == NPPlayer.instance.playerInfo.CID;
                bool isGuildMember = !isEmpty && NPPlayer.instance.guildComp.isSameGuild(_playerInfo.getGuildId());

                if (!isEmpty)
                {
                    string guildPlayerName;
                    if (string.IsNullOrEmpty(_playerInfo.getGuildSimpleName()))
                        guildPlayerName = _playerInfo.getPlayerName();
                    else
                        guildPlayerName = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOccupantName, _playerInfo.getGuildSimpleName(), _playerInfo.getPlayerName());
                    ALUGUICommon.setLabelTxt(wnd.txtOccupied, guildPlayerName);
                }

                wnd.setOccupiedState(!isEmpty, isSelf, isGuildMember);
                wnd.setDisableShow(_m_mineInfo == null);
            });
        }

        private void _reqServerInfo(long _guildShareMineMsgId, Action<Mars_MineDynamic, PlayerInfo_IconShow> _complete)
        {
            if (_guildShareMineMsgId <= 0)
            {
                _complete?.Invoke(null, null);
                return;
            }

            NPGSClientListener.sendRequestByLog(
                new GC2GS_041_019_ReqGuildShareMineInfo(_guildShareMineMsgId),
                new CommonRequestCallbackProtocolDealer<GS2GC_041_019_RetGuildShareMineInfo>(
                    (_res) =>
                    {
                        Mars_MineDynamic mineInfo = _res?.getInfo();
                        _reqPlayerInfo(mineInfo, _playerInfo => _complete?.Invoke(mineInfo, _playerInfo));
                    },
                    (_err) =>
                    {
                        _complete?.Invoke(null, null);
                    }, true));
        }

        private void _reqPlayerInfo(Mars_MineDynamic _mineInfo, Action<PlayerInfo_IconShow> _complete)
        {
            if (_mineInfo == null)
            {
                _complete?.Invoke(null);
                return;
            }

            if (_mineInfo.getOccupiedCid() == 0)
            {
                _complete?.Invoke(null);
                return;
            }

            if (_mineInfo.getOccupiedCid() == NPPlayer.instance.playerInfo.CID)
            {
                PlayerInfo_IconShow selfInfo = NPPlayer.instance.playerInfo.getPlayerBriefInfo();
                _complete?.Invoke(selfInfo);
                return;
            }

            NPGSClientListener.sendRequestByLog(
                NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_mineInfo.getOccupiedCid()),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>(
                    (_isSuc, _msg) =>
                    {
                        if (!_isSuc)
                        {
                            _complete?.Invoke(null);
                            return;
                        }

                        PlayerInfo_IconShow briefInfo = _msg?.getPlayerBrief();
                        _complete?.Invoke(briefInfo);
                    }));
        }

        private void _onClickInfo(GameObject _go)
        {
            if (detailInfo == null || _m_mineInfo == null)
                return;

            long guildShareId = detailInfo.content.getGuildShareMineMsgId();
            long mineInstanceId = _m_mineInfo.getId();
            GNodeMarsExplore.openOrQuitToNode(() =>
            {
                GGUIWndMarsExploreMineInfo.instance.refreshWnd(mineInstanceId
                    , (_teamId) =>
                    {
                        NPGSClientListener.sendRequestByLog(new GC2GS_041_018_ReqGuildMateForwardCollectMine(guildShareId, _teamId, false, false),
                            new CommonRequestCallbackProtocolDealer<GS2GC_041_018_RetGuildMateForwardCollectMine>((_retMsg) =>
                            {

                            }, (_errCode) =>
                            {
                                string tips = string.Empty;
                                bool needShowConfirm = false;
                                if (_errCode == ErrorCodeConst.MARS_MINE_OTHER_PLAYER_OCCUPY)
                                {
                                    tips = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOtherPlayerOccupyTip_none);
                                    needShowConfirm = true;
                                }
                                else if (_errCode == ErrorCodeConst.MARS_MINE_OTHER_PLAYER_FORWARD)
                                {
                                    tips = TextTranslate.instance.getLanguage(TransKeyConst.mars_exploreMineOtherPlayerForwardTip_none);
                                    needShowConfirm = true;
                                }

                                if (needShowConfirm)
                                {
                                    NPMesMgr.instance.showTwoBtnMes(tips,
                                        TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                                        null,
                                        TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                                        () => NPGSClientListener.sendMsgByLog(new GC2GS_041_018_ReqGuildMateForwardCollectMine(guildShareId, _teamId, true, true)));
                                }
                            }));
                    });
                //开启窗口
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreMineInfo.instance, GGUIWndMarsExploreMineInfo.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_MINE_INFO);
            });
        }
    }
}
