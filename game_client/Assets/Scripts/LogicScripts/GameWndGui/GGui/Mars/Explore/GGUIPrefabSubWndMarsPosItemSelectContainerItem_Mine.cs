using System;
using ALPackage;
using Common.MarsObj;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p041_MarsExploreOp;
using NPCommon;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndMarsPosItemSelectContainerItem_Mine : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsPosItemSelectContainerItem_Mine>
    {
        private _IMarsExploreMineItem _m_mineItem;
        private Mars_MineDynamic _m_mineInfo;
        private MarsExploreMineRefObj _m_mineRef;
        private PlayerInfo_IconShow _m_playerInfo;
        private NPGGuiWndTexture _m_iconWnd;
        private long _m_lShowSerialize;


        public GGUIPrefabSubWndMarsPosItemSelectContainerItem_Mine(Transform _parent)
            : base(_parent)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsPosItemSelectContainerItem_Mine.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPosItemSelectContainerItem_Mine.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGo, _onClickGo);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnGo, _onClickGo);
        }


        public void refreshWnd(_IMarsExploreMineItem _mineItem)
        {
            _m_mineItem = _mineItem;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_mineItem == null)
                return;

            _m_lShowSerialize = ALSerializeOpMgr.next();
            long tmpSerialize = _m_lShowSerialize;

            _m_mineInfo = null;
            _m_mineRef = null;
            _m_playerInfo = null;
            wnd.setLoadingShow(true);

            NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(_m_mineItem.instanceId),
                new CommonRequestCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>(
                    (_res) =>
                    {
                        if (_m_lShowSerialize != tmpSerialize)
                            return;

                        _m_mineInfo = _res?.getInfo();
                        if (_m_mineInfo != null)
                            _m_mineRef = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_m_mineInfo.getRefId());

                        _reqPlayerInfo(_m_mineInfo, (_playerInfo) =>
                        {
                            if (_m_lShowSerialize != tmpSerialize)
                                return;

                            _m_playerInfo = _playerInfo;
                            wnd.setLoadingShow(false);
                            _refreshWndInternal();
                        });
                    },
                    (_err) =>
                    {
                        if (_m_lShowSerialize != tmpSerialize)
                            return;

                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_err);
                        wnd.setLoadingShow(false);
                    }, true
                ));
        }


        private void _refreshWndInternal()
        {
            if (wnd == null || _m_mineInfo == null || _m_mineRef == null)
                return;

            _m_iconWnd?.setTexture(_m_mineRef.icon);
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_mineRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_mineRef.mine_lvl));
            ALUGUICommon.setLabelTxt(wnd.txtRemainCount, _m_mineInfo.getRemainNum().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            bool isEmpty = _m_playerInfo == null;
            bool isSelf = !isEmpty && _m_playerInfo.getCid() == NPPlayer.instance.playerInfo.CID;
            bool isGuildMember = !isEmpty && NPPlayer.instance.guildComp.isSameGuild(_m_playerInfo.getGuildId());
            wnd.setState(isEmpty, isSelf, isGuildMember);
        }
        private void _reqPlayerInfo(Mars_MineDynamic _mineInfo, Action<PlayerInfo_IconShow> _complete)
        {
            if (_mineInfo == null || _mineInfo.getOccupiedCid() == 0)
            {
                _complete?.Invoke(null);
                return;
            }

            if (_mineInfo.getOccupiedCid() == NPPlayer.instance.playerInfo.CID)
            {
                _complete?.Invoke(NPPlayer.instance.playerInfo.getPlayerBriefInfo());
                return;
            }

            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_mineInfo.getOccupiedCid()),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                    {
                        _complete?.Invoke(null);
                        return;
                    }

                    _complete?.Invoke(_msg?.getPlayerBrief());
                }));
        }


        private void _onClickGo(GameObject _go)
        {
            if (_m_mineItem == null)
                return;

            _IMarsExploreMineItem mineItem = _m_mineItem;
            GGUIWndMarsExploreMineInfo.instance.refreshWnd(mineItem.instanceId, (_teamId) =>
            {
                NPGSClientListener.sendRequestByLog(new GC2GS_041_010_ReqForwardCollectMine(_teamId, mineItem.instanceId, false, false), 
                    new CommonRequestCallbackProtocolDealer<GS2GC_041_010_RetForwardCollectMine>((_retMsg) => {
                            
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
                                () => NPGSClientListener.sendMsgByLog(new GC2GS_041_010_ReqForwardCollectMine(_teamId, mineItem.instanceId, true, true)));
                        }
                    }));
            });
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreMineInfo.instance, GGUIWndMarsExploreMineInfo.instance.showWnd,
                UINodeTagConst.C_MARS_EXPLORE_MINE_INFO);
        }
    }
}
