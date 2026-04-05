using ALPackage;
using GC2GS.p034_InnOp;
using GS2GC.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnStationList : _ATALBasicUIWnd<GGUIMonoInnStationList>
    {
        [NotNull] public static GGUIWndInnStationList instance { get { return _g_instance ??= new GGUIWndInnStationList(); } }
        private static GGUIWndInnStationList _g_instance;


        private GGUISubWndInnStationListContainer _m_containerWnd;
        private InnViewMgr _m_viewMgr;
        private InnStationInfo _m_curStationInfo;
        private InnStationInfo _m_defaultStationInfo;
        private NPGGuiWndTexture _m_wStationTex;
        private NPGGUIWndCommonItem _m_buildCostItemWnd;
        private NPGGUIWndCommonItem _m_levelUpCostItemWnd;
        private TextUpgradePropertyShow<long> _m_levelChgShow;
        private TextUpgradePropertyShow<long> _m_popularityGainChgShow;
        private TextUpgradePropertyShow<long> _m_finesseGainChgShow;
        private GGUISubWndInnStationDishGrid _m_dishGridWnd;


        public GGUIWndInnStationList() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnStationList.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnStationList.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_STATION_BUILD_BUTTON, _onSimulateBuildClick);
            _m_containerWnd?.showWnd();
            _m_buildCostItemWnd?.showWnd();
            _m_levelUpCostItemWnd?.showWnd();
            _m_wStationTex?.showWnd();
            _m_dishGridWnd?.showWnd();

            refreshWnd();

            // NPPlayer.instance.innComp.onStationChg += _onStationChg;
        }
        protected override void _onHideWnd()
        {
            // NPPlayer.instance.innComp.onStationChg -= _onStationChg;

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_STATION_BUILD_BUTTON, _onSimulateBuildClick);
            _m_containerWnd?.hideWnd();
            _m_wStationTex?.hideWnd();
            _m_buildCostItemWnd?.hideWnd();
            _m_levelUpCostItemWnd?.hideWnd();
            _m_dishGridWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_containerWnd?.resetWnd();
            _m_wStationTex?.discardTexture();
            _m_buildCostItemWnd?.resetWnd();
            _m_levelUpCostItemWnd?.resetWnd();
            _m_dishGridWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_containerWnd?.discard();
            _m_containerWnd = null;
            _m_wStationTex?.discard();
            _m_wStationTex = null;
            _m_buildCostItemWnd?.discard();
            _m_buildCostItemWnd = null;
            _m_levelUpCostItemWnd?.discard();
            _m_levelUpCostItemWnd = null;
            _m_dishGridWnd?.discard();
            _m_dishGridWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBuild, _onClickBuild);
            ALUGUICommon.uncombineBtnClick(wnd.btnLevelUp, _onClickLevelUp);
            ALUGUICommon.uncombineBtnClick(wnd.btnLockLevelUp, _onClickLockLevelUp);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoContainer != null)
            {
                _m_containerWnd = new GGUISubWndInnStationListContainer(wnd.monoContainer);
                _m_containerWnd.onSelectItem += _onClickSelectItem;
            }

            if (wnd.imgStationTex != null)
                _m_wStationTex = new NPGGuiWndTexture(wnd.imgStationTex);

            if (wnd.monoBuildCostItem != null)
                _m_buildCostItemWnd = new NPGGUIWndCommonItem(wnd.monoBuildCostItem);

            if (wnd.monoUpgradeCostItem != null)
                _m_levelUpCostItemWnd = new NPGGUIWndCommonItem(wnd.monoUpgradeCostItem); 

            if (wnd.txtLevelChg != null)
                _m_levelChgShow = new TextUpgradePropertyShow<long>(wnd.txtLevelChg, TransKeyConst.common_level_num);

            if (wnd.txtPopularityGainChg != null)
                _m_popularityGainChgShow = new TextUpgradePropertyShow<long>(wnd.txtPopularityGainChg, string.Empty);

            if (wnd.txtFinesseGainChg != null)
                _m_finesseGainChgShow = new TextUpgradePropertyShow<long>(wnd.txtFinesseGainChg, string.Empty);

            if (wnd.monoStationDishGrid != null)
                _m_dishGridWnd = new GGUISubWndInnStationDishGrid(wnd.monoStationDishGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBuild, _onClickBuild);
            ALUGUICommon.combineBtnClick(wnd.btnLevelUp, _onClickLevelUp);
            ALUGUICommon.combineBtnClick(wnd.btnLockLevelUp, _onClickLockLevelUp);
        }

        public void refreshWnd(InnViewMgr _viewMgr, InnStationInfo _selectStationInfo = null)
        {
            _m_viewMgr = _viewMgr;
            _m_defaultStationInfo = _selectStationInfo;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_containerWnd?.refreshWnd(_m_viewMgr, _m_defaultStationInfo);
        }

        /// <summary>
        /// 刷新设施详情展示
        /// </summary>
        private void _refreshDetail()
        {
            if (wnd == null || _m_curStationInfo == null || _m_curStationInfo.refObj == null)
                return;

            bool isBuild = _m_curStationInfo.isBuilt;

            // 设施大图
            _m_wStationTex?.setTexture(_m_curStationInfo.refObj.station_tex);
            // 等级名称
            ALUGUICommon.setLabelTxt(wnd.txtNameLevel, isBuild ? 
                TextTranslate.instance.getLanguage(TransKeyConst.inn_stationLevelAndName_level_name, _m_curStationInfo.level, _m_curStationInfo.nameTranslated) : 
                _m_curStationInfo.nameTranslated);
            // 设施描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_curStationInfo.descTranslated);
            // 提示文本
            ALUGUICommon.setLabelTxt(wnd.txtTip, _m_curStationInfo.getCurrentTipTranslated());
            // 已建造时显隐GO列表
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyBuildShowList, isBuild);
            ALUGUICommon.setGameObjEnable(wnd.goAlreadyBuildHideList, !isBuild);

            _m_dishGridWnd?.refreshWnd(_m_curStationInfo.uiDishInfos);

            if (isBuild)
                _refreshBuildShow();
            else
                _refreshNoBuildShow();
        }

        /// <summary>
        /// 刷新已建造设施显示
        /// </summary>
        private void _refreshBuildShow()
        {
            if (wnd == null || _m_viewMgr == null || _m_curStationInfo == null || _m_curStationInfo.refObj == null)
                return;

            InnStationLevelRefObj curLevelRefObj = _m_curStationInfo.levelRefObj;
            InnStationLevelRefObj nextLevelRefObj = _m_curStationInfo.nextLevelRefObj ?? curLevelRefObj;
            // 等级属性变化
            _m_levelChgShow?.setValue(curLevelRefObj?.level ?? 0, nextLevelRefObj?.level ?? 0);
            _m_popularityGainChgShow?.setValue(curLevelRefObj?.popularity_add ?? 0, nextLevelRefObj?.popularity_add ?? 0);
            _m_finesseGainChgShow?.setValue(curLevelRefObj?.finesse_add ?? 0, nextLevelRefObj?.finesse_add ?? 0);
            // 升级按钮显隐
            bool canUpgradeStation = NPPlayer.instance.innComp.canUpgradeStation();
            wnd.setIsUpgradeUnlock(canUpgradeStation);
            wnd.setIsLevelMax(_m_curStationInfo.isLevelMax);
            //升级消耗
            _m_levelUpCostItemWnd?.setItem(curLevelRefObj?.upgrade_cost);
        }

        /// <summary>
        /// 刷新未建造设施显示
        /// </summary>
        private void _refreshNoBuildShow()
        {
            if (wnd == null || _m_viewMgr == null || _m_curStationInfo == null || _m_curStationInfo.refObj == null)
                return;

            // 属性值
            ALUGUICommon.setLabelTxt(wnd.txtPopularityGain, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationPopularityGain_num, _m_curStationInfo.levelRefObj?.popularity_add ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtFinesseGain, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationFinesseGain_num, _m_curStationInfo.levelRefObj?.finesse_add ?? 0));

            InnStationInfo requireStationInfo = NPPlayer.instance.innComp.getRequireStationInfo(_m_curStationInfo);
            bool isNextBuilding = requireStationInfo is { isBuilt: true } or null;
            long currentGuestNum = _m_viewMgr.getHadSettleGuestsCount();
            long requireGuestNum = _m_curStationInfo.refObj.need_receive_guest_num;
            // 解锁提示
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(TransKeyConst.inn_stationUnlockTip_name, requireStationInfo?.nameTranslated));
            // 客人接待要求
            if(requireGuestNum == 0)
                ALUGUICommon.setLabelTxt(wnd.txtGuestServeRequire, "");
            else if (currentGuestNum >= requireGuestNum)
                ALUGUICommon.setLabelTxt(wnd.txtGuestServeRequire, TextTranslate.instance.getLanguage(TransKeyConst.inn_currentGuestNumAndTarget_num_num, currentGuestNum, requireGuestNum));
            else
                ALUGUICommon.setLabelTxt(wnd.txtGuestServeRequire,
                    TextTranslate.instance.getLanguage(TransKeyConst.inn_currentGuestNumAndTarget_num_num, GCommon.addColorForRichText(currentGuestNum.ToString(), wnd.notEnoughColor), requireGuestNum));
            // 建造消耗
            _m_buildCostItemWnd?.setItem(_m_curStationInfo.refObj.build_cost);
            wnd.setIsNextBuilding(isNextBuilding, isNextBuilding && currentGuestNum >= requireGuestNum && !_m_curStationInfo.isBuilt);
        }

        /// <summary>
        /// 点击设施
        /// </summary>
        /// <param name="_item"></param>
        private void _onClickSelectItem(GGUISubWndInnStationListContainerItem _item)
        {
            if (_item == null)
                return;

            _m_curStationInfo = _item.stationInfo;
            _refreshDetail();
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_STATION_LIST);
        }

        /// <summary>
        /// 点击建造
        /// </summary>
        /// <param name="_"></param>
        private void _onClickBuild(GameObject _)
        {
            if (_m_curStationInfo == null)
                return;

            InnStationInfo requireStationInfo = NPPlayer.instance.innComp.getRequireStationInfo(_m_curStationInfo);
            bool isNextBuilding = requireStationInfo is { isBuilt: true } or null;
            // 前置设施还没建造，上浮提示
            if (!isNextBuilding)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.inn_stationUnlockTip_name, requireStationInfo?.nameTranslated));
                return;
            }

            // 客人数量是否足够
            long currentGuestNum = _m_viewMgr.getHadSettleGuestsCount();
            long requireGuestNum = _m_curStationInfo.refObj.need_receive_guest_num;
            if (currentGuestNum < requireGuestNum)
            {
                //迎宾更多人后可解锁
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_needMoreGuestTip_none);
                return;
            }

            // 道具是否足够
            if (!GCommon.isItemEnough(_m_curStationInfo.refObj.build_cost, true))
                return;

            NPGSClientListener.sendRequestByLog(new GC2GS_034_002_ReqInnStationUnlock(_m_curStationInfo.stationId),
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    _m_containerWnd?.refreshAllItem();
                    _refreshDetail();
                    GGUIWndInnStationBuildSuccess.instance.refreshWnd(_m_curStationInfo);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnStationBuildSuccess.instance, GGUIWndInnStationBuildSuccess.instance.showWnd, UINodeTagConst.C_INN_STATION_BUILD_SUCCESS);
                }));

        }

        /// <summary>
        /// 点击升级
        /// </summary>
        /// <param name="_"></param>
        public void _onClickLevelUp(GameObject _)
        {
            if (_m_curStationInfo == null)
                return;

            bool canUpgradeStation = NPPlayer.instance.innComp.canUpgradeStation();
            if (!canUpgradeStation)
            {
                //建造所有设施后可升级
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_unlockStationUpgradeTip_none);
                return;
            }

            if (_m_curStationInfo.levelRefObj == null || !GCommon.isItemEnough(_m_curStationInfo.levelRefObj.upgrade_cost, true))
                return;

            NPGSClientListener.sendRequestByLog(new GC2GS_034_003_ReqInnStationUpgrade(_m_curStationInfo.stationId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_034_003_RetInnStationUpgrade>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        _m_containerWnd?.refreshAllItem();
                        _refreshDetail();
                        GGUIWndInnStationLevelUpSuccess.instance.refreshWnd(_m_curStationInfo);
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnStationLevelUpSuccess.instance, GGUIWndInnStationLevelUpSuccess.instance.showWnd, UINodeTagConst.C_INN_STATION_LEVEL_UP_SUCCESS);
                    }
                }));
        }

        /// <summary>
        /// 点击未升级
        /// </summary>
        /// <param name="_"></param>
        public void _onClickLockLevelUp(GameObject _)
        {
            if (_m_curStationInfo == null)
                return;

            bool canUpgradeStation = NPPlayer.instance.innComp.canUpgradeStation();
            if (!canUpgradeStation)
            {
                //建造所有设施后可升级
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_unlockStationUpgradeTip_none);
                return;
            }
        }

        /// <summary>
        /// 设施变更
        /// </summary>
        /// <param name="_info"></param>
        private void _onStationChg(InnStationInfo _info)
        {
            refreshWnd();
        }

        private void _onSimulateBuildClick()
        {
            if (wnd == null)
                return;

            _onClickBuild(wnd.btnBuild);
        }
    }
}