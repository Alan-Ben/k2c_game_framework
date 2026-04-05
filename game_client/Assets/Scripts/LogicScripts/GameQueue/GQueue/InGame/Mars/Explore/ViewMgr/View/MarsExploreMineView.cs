using ALPackage;
using Common;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MarsExploreMineView : _AALBasicLoadObj
    {
        [NotNull] private readonly _IMarsExploreMineItem _m_mineInfo;

        private GGUIWndMarsExploreMineTipFollowerController _m_tipWnd;
        private GGUICommonFollowTarget _m_followTarget;
        private GTDMonoMarsExploreMine _m_mono;
        private bool _m_needShowLoadedEffect;


        public MarsExploreMineView([NotNull] _IMarsExploreMineItem _mineInfo)
        {
            _m_mineInfo = _mineInfo;
        }


        public _IMarsExploreMineItem mineInfo { get { return _m_mineInfo; } }
        public GGUICommonFollowTarget followTarget { get { return _m_followTarget; } }
        public GTDMonoMarsExploreMine mono { get { return _m_mono; } }
        public Vector3 position
        {
            get
            {
                if (_m_mono == null)
                    return Vector3.zero;
                return _m_mono.transform.position;
            }
        }


        protected override void _loadOp()
        {
            long posId = _m_mineInfo.posId;
            GTDMonoMarsExploreEventPos minePos = MainAdditionMarsExploreTDScene.instance.getEventPos(posId);
            if (minePos == null)
            {
                _setLoadDone();
                return;
            }

            Vector3 position = minePos.transform.position;
            NPGGoIndex goIndex = _m_mineInfo.refObj.scene_go_index;
            MainAdditionMarsExploreTDScene.instance.createUnit<GTDMonoMarsExploreMine>(goIndex, position, _mono =>
            {
                _m_mono = _mono;
                if (_m_mono != null)
                    _onInitDone();
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            _onDiscard();

            if (_m_mono != null)
            {
                NPGGoIndex goIndex = _m_mineInfo.refObj.scene_go_index;
                MainAdditionMarsExploreTDScene.instance.discardUnit(goIndex, _m_mono);
                _m_mono = null;
            }
        }


        public void tick()
        {
            _m_tipWnd?.refreshTime();
        }
        public void updateView(bool _newEventEffect)
        {
            if (_m_mono == null)
                return;

            _m_mono.setState(_m_mineInfo.occupiedCid == 0, _m_mineInfo.isMe);
            if (_m_tipWnd != null)
            {
                _m_tipWnd.refreshWnd();
                if (_newEventEffect)
                    _m_tipWnd.playLoadedEffect();
            }
        }
        public void triggerClick()
        {
            int eventNum = NPPlayer.instance.marsComp.exploreSubComponent.getEventNumAtPos(_m_mineInfo.posId);
            // 如果还有其它事件，打开事件选择窗口
            if (eventNum > 1)
            {
                GGUIWndMarsPosItemSelect.instance.refreshWnd(_m_mineInfo.posId);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsPosItemSelect.instance, GGUIWndMarsPosItemSelect.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_MARS_EXPLORE_POS_ITEM_SELECT, true, false);
                return;
            }
            
            _IMarsExploreMineItem info = _m_mineInfo;
            //设置信息
            GGUIWndMarsExploreMineInfo.instance.refreshWnd(info.instanceId
                , (_teamId) => {
                    NPGSClientListener.sendRequestByLog(new GC2GS_041_010_ReqForwardCollectMine(_teamId, info.instanceId, false, false), 
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
                                    () => NPGSClientListener.sendMsgByLog(new GC2GS_041_010_ReqForwardCollectMine(_teamId, info.instanceId, true, true)));
                            }
                        }));
                });
            //开启窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreMineInfo.instance, GGUIWndMarsExploreMineInfo.instance.showWnd, UINodeTagConst.C_MARS_EXPLORE_MINE_INFO);
        }
        public void setNeedShowLoadedEffect()
        {
            _m_needShowLoadedEffect = true;
        }


        private void _onInitDone()
        {
            if (_m_mono == null)
                return;

            if (_m_mono.followTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(_m_mono.followTarget, Vector3.zero);
                GGUIWndMarsExploreHUDRoot.instance.regInstance(_m_followTarget);
            }

            if (_m_mono.monoClick != null)
                _m_mono.monoClick.onClick += _onClickMine;

            if (_m_followTarget != null)
            {
                _m_tipWnd = new GGUIWndMarsExploreMineTipFollowerController();
                _m_tipWnd.refreshWnd(this);
                if (_m_needShowLoadedEffect)
                    _m_tipWnd.regItemWndLoadDoneDelegate(() => { _m_tipWnd.playLoadedEffect(); });
                GGUIWndMarsExploreHUDRoot.instance.addController(_m_followTarget, _m_tipWnd);
            }

            if (_m_needShowLoadedEffect)
            {
                _m_needShowLoadedEffect = false;
                MainAdditionMarsExploreTDScene.instance.showEventLoadedEffect(position);
            }
        }
        private void _onDiscard()
        {
            _m_tipWnd?.discard();
            _m_tipWnd = null;

            if (_m_followTarget != null)
            {
                GGUIWndMarsExploreHUDRoot.instance.removeInstance(_m_followTarget);
                _m_followTarget.discard();
                _m_followTarget = null;
            }

            if (_m_mono == null)
                return;

            if (_m_mono.monoClick != null)
                _m_mono.monoClick.onClick -= _onClickMine;
        }


        private void _onClickMine()
        {
            triggerClick();
        }
    }
}