using ALPackage;
using GC2GS.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnMedalInfo : _ATALBasicUIWnd<GGUIMonoInnMedalInfo>
    {
        [NotNull] public static GGUIWndInnMedalInfo instance { get { return _g_instance ??= new GGUIWndInnMedalInfo(); } }
        private static GGUIWndInnMedalInfo _g_instance;

        
        private NPGGuiWndTexture _m_medalIconWnd;
        private GGUISubWndInnLevelIconContainer _m_starIconContainerWnd;


        public GGUIWndInnMedalInfo()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnMedalInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnMedalInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_medalIconWnd?.showWnd();
            _m_starIconContainerWnd?.showWnd();
            
            refreshWnd();
            
            NPPlayer.instance.innComp.onMedalLevelChg += _onMedalChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MEDAL_UPGRADE_BUTTON, _onSimulateClickUpgradeBtn);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onMedalLevelChg -= _onMedalChg;
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MEDAL_UPGRADE_BUTTON, _onSimulateClickUpgradeBtn);
            
            _m_medalIconWnd?.hideWnd();
            _m_starIconContainerWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_medalIconWnd?.discardTexture();
            _m_starIconContainerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_medalIconWnd?.discard();
            _m_medalIconWnd = null;
            _m_starIconContainerWnd?.discard();
            _m_starIconContainerWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnLevelList, _onBtnLevelListClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgMedalIcon != null)
                _m_medalIconWnd = new NPGGuiWndTexture(wnd.imgMedalIcon);
            
            if (wnd.monoStarIconContainer != null)
                _m_starIconContainerWnd = new GGUISubWndInnLevelIconContainer(wnd.monoStarIconContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClicked);
            ALUGUICommon.combineBtnClick(wnd.btnLevelList, _onBtnLevelListClicked);
        }
        

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            InnLevelRefObj innLevelRef = NPPlayer.instance.innComp.levelRef;
            long popularity = NPPlayer.instance.innComp.popularity;
            innLevelRef ??= GRefdataCoreMgr.instance.innLevelRefCore.getRef(1);
            if (innLevelRef == null)
                return;
            
            InnLevelRefObj nextLevelRef = GRefdataCoreMgr.instance.innLevelRefCore.getRef(innLevelRef.level + 1);

            // 设置等级名称
            ALUGUICommon.setLabelTxt(wnd.txtName, innLevelRef.nameTranslated);
            // 设置当前等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, innLevelRef.level));
            // 设置升级进度
            ALUGUICommon.setLabelTxt(wnd.txtLevelProgress,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,
                    popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                    nextLevelRef?.need_popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT) ?? popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            if (wnd.sldLevelProgress != null)
            {
                wnd.sldLevelProgress.minValue = innLevelRef.need_popularity;
                wnd.sldLevelProgress.maxValue = nextLevelRef?.need_popularity ?? innLevelRef.need_popularity;
                wnd.sldLevelProgress.value = popularity;
            }
            
            // 设置当前人气值
            ALUGUICommon.setLabelTxt(wnd.txtCurPopularity, popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            // 设置迎宾次数上限
            ALUGUICommon.setLabelTxt(wnd.txtCurReceiveGuestLimit, innLevelRef.receive_guest_limit);
            // 设置当前解锁客人数量
            ALUGUICommon.setLabelTxt(wnd.txtCurGuestUnlockNum, NPPlayer.instance.innComp.getUnlockedGuestNum());
            // 设置满级状态显示
            wnd.setLevelMax(nextLevelRef == null);
            // 设置等级为0的状态显示
            wnd.setLevelZero(NPPlayer.instance.innComp.levelRef == null || NPPlayer.instance.innComp.levelRef.level <= 0);
            
            // 刷新星级图标容器
            _m_starIconContainerWnd?.refreshWnd(innLevelRef);

            // 刷新奖牌的部分
            refreshMedalPart();
        }
        public void refreshMedalPart()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            InnMedalLevelRefObj medalRef = NPPlayer.instance.innComp.medalLevelRef;
            InnLevelRefObj innLevelRef = NPPlayer.instance.innComp.levelRef;
            bool isNoActivated = medalRef == null;
            medalRef ??= GRefdataCoreMgr.instance.innMedalLevelRefCore.getRef(1);
            innLevelRef ??= GRefdataCoreMgr.instance.innLevelRefCore.getRef(1);
            if (medalRef == null || innLevelRef == null)
                return;
            
            InnMedalLevelRefObj nextMedalRef = GRefdataCoreMgr.instance.innMedalLevelRefCore.getRef(medalRef.level + 1);
            
            ALUGUICommon.setLabelTxt(wnd.txtMedalName, innLevelRef.medalNameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtMedalLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, medalRef.level));
            _m_medalIconWnd?.setTexture(innLevelRef.medal_icon);
            ALUGUICommon.setLabelTxt(wnd.txtMedalDesc, innLevelRef.medalDescTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtFirstObtainTime, TimeUtil.Milliseconds2StringYMD(NPPlayer.instance.innComp.firstTimeUpgradeTimeMs));
            
            long curValue = medalRef.bonus_prop_modifier.getPropValue(wnd.bonusPropertyType);
            long nextValue = nextMedalRef != null ? nextMedalRef.bonus_prop_modifier.getPropValue(wnd.bonusPropertyType) : curValue;
            ALUGUICommon.setLabelTxt(wnd.txtCurrentBonusPlus,
                TextTranslate.instance.getLanguage(TransKeyConst.inn_medalBuildingCurBonusDesc_value,
                    wnd.isPercentage ? curValue / 100f : curValue.ToLargeString(wnd.isGold ? PrimitiveExtension.ELargeStringType.GOLD : PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.txtNextBonusPlus,
                TextTranslate.instance.getLanguage(TransKeyConst.inn_medalBuildingNextBonusDesc_value,
                    wnd.isPercentage ? nextValue / 100f : nextValue.ToLargeString(wnd.isGold ? PrimitiveExtension.ELargeStringType.GOLD : PrimitiveExtension.ELargeStringType.DEFAULT)));
            int medalUpgradeCount = NPPlayer.instance.innComp.getMedalUpgradeCount();
            ALUGUICommon.setLabelTxt(wnd.txtCanUpgradeCount, TextTranslate.instance.getLanguage(TransKeyConst.inn_medalCanUpgradeCount_num, medalUpgradeCount));
            
            wnd.setActivate(!isNoActivated);
            wnd.setCanUpgrade(medalUpgradeCount > 0);
            wnd.setMedalLevelMax(nextMedalRef == null);
        }


        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_MEDAL_INFO);
        }
        private void _onBtnUpgradeClicked(GameObject _)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_034_012_ReqInnMedalUpgrade(), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    if (wnd == null)
                        return;

                    if (wnd.transEffectUpgrade != null)
                        PlaySfxMgr.instance.playUISfx(wnd.effectIdUpgrade, wnd.transEffectUpgrade);
                }));
        }
        private void _onBtnLevelListClicked(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnLevelDetail.instance, GGUIWndInnLevelDetail.instance.showWnd, UINodeTagConst.C_INN_LEVEL_DETAIL);
        }
        private void _onMedalChg()
        {
            refreshMedalPart();
        }
        /// <summary>
        /// 模拟点击升级按钮
        /// </summary>
        private void _onSimulateClickUpgradeBtn()
        {
            _onBtnUpgradeClicked(null);
        }
    }
}