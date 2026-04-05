using System.Collections.Generic;
using ALPackage;
using GC2GS.p035_MuseumOp;
using GS2GC.p035_MuseumOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMuseumItemInfo : _ATALBasicUIWnd<GGUIMonoMuseumItemInfo>
    {
        [NotNull] public static GGUIWndMuseumItemInfo instance { get { return _g_instance ??= new GGUIWndMuseumItemInfo(); } }
        private static GGUIWndMuseumItemInfo _g_instance;


        private MuseumItemInfo _m_itemInfo;
        private List<MuseumItemInfo> _m_itemList;
        private MuseumItemInfo _m_prevItemInfo;
        private MuseumItemInfo _m_nextItemInfo;
        
        private NPGGuiWndTexture _m_iconTexture;
        private NPGGuiWndTexture _m_skillIconTexture;
        private NPGGuiWndTexture _m_qualityIconTexture;
        private GGUISubWndQualityShowGo _m_qualityShowGoWnd;
        private NPGGUIWndCommonItem _m_upgradeCostWnd;
        private TextUpgradePropertyShow<int> _m_levelUpgradeShow;
        private List<CommonUISfxObj> _m_lUpgradeSfxObjList;


        public GGUIWndMuseumItemInfo()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMuseumItemInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMuseumItemInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconTexture?.showWnd();
            _m_skillIconTexture?.showWnd();
            _m_qualityIconTexture?.showWnd();
            _m_qualityShowGoWnd?.showWnd();
            _m_upgradeCostWnd?.showWnd();
            
            refreshWnd();
            
            NPPlayer.instance.museumComp.onItemChg += _onItemChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.museumComp.onItemChg -= _onItemChg;
            
            _m_iconTexture?.hideWnd();
            _m_skillIconTexture?.hideWnd();
            _m_qualityIconTexture?.hideWnd();
            _m_qualityShowGoWnd?.hideWnd();
            _m_upgradeCostWnd?.hideWnd();
            _discardUpgradeSfx();
        }
        protected override void _onReset()
        {
            _m_iconTexture?.discardTexture();
            _m_skillIconTexture?.discardTexture();
            _m_qualityIconTexture?.discardTexture();
            _m_qualityShowGoWnd?.resetWnd();
            _m_upgradeCostWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_iconTexture?.discard();
            _m_iconTexture = null;
            _m_skillIconTexture?.discard();
            _m_skillIconTexture = null;
            _m_qualityIconTexture?.discard();
            _m_qualityIconTexture = null;
            _m_qualityShowGoWnd?.discard();
            _m_qualityShowGoWnd = null;
            _m_upgradeCostWnd?.discard();
            _m_upgradeCostWnd = null;
            _discardUpgradeSfx();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnActivate, _onBtnActivateClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextItem, _onBtnNextItemClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevItem, _onBtnPrevItemClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconTexture = new NPGGuiWndTexture(wnd.imgIcon);
            if (wnd.imgSkillIcon != null)
                _m_skillIconTexture = new NPGGuiWndTexture(wnd.imgSkillIcon);
            if (wnd.imgQualityIcon != null)
                _m_qualityIconTexture = new NPGGuiWndTexture(wnd.imgQualityIcon);
            if (wnd.monoQualityShowGo != null)
                _m_qualityShowGoWnd = new GGUISubWndQualityShowGo(wnd.monoQualityShowGo);
            if (wnd.monoUpgradeCose != null)
                _m_upgradeCostWnd = new NPGGUIWndCommonItem(wnd.monoUpgradeCose);
            if (wnd.txtLevel != null)
                _m_levelUpgradeShow = new TextUpgradePropertyShow<int>(wnd.txtLevel, TransKeyConst.common_level_num);

            ALUGUICommon.combineBtnClick(wnd.btnActivate, _onBtnActivateClicked);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onBtnUpgradeClicked);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnNextItem, _onBtnNextItemClicked);
            ALUGUICommon.combineBtnClick(wnd.btnPrevItem, _onBtnPrevItemClicked);
        }

        
        public void refreshWnd(MuseumItemInfo _itemInfo, List<MuseumItemInfo> _itemList = null)
        {
            _m_itemInfo = _itemInfo;
            _m_itemList = _itemList;
            _m_prevItemInfo = null;
            _m_nextItemInfo = null;
            if (_m_itemList is { Count: > 1 })
            {
                int curIndex = _m_itemList.IndexOf(_m_itemInfo);
                if (curIndex >= 0)
                {
                    int nextIndex = NPGameUtility.intRepeat(curIndex + 1, _m_itemList.Count);
                    int prevIndex = NPGameUtility.intRepeat(curIndex - 1, _m_itemList.Count);
                    _m_nextItemInfo = _m_itemList[nextIndex];
                    _m_prevItemInfo = _m_itemList[prevIndex];
                }
            }
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_itemInfo?.levelProperty == null)
                return;

            wnd.setState(_m_itemInfo.isObtain, _m_itemInfo.isActive, _m_itemInfo.levelProperty.isLevelMax);
            wnd.setHasNextItem(_m_nextItemInfo != null);
            wnd.setHasPrevItem(_m_prevItemInfo != null);

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.desc));
            ALUGUICommon.setLabelTxt(wnd.txtQuality, TextTranslate.instance.getLanguage(_m_itemInfo.refObj.quality_ref?.name));
            ALUGUICommon.setLabelTxt(wnd.txtObtainTime, TextTranslate.instance.getLanguage(TransKeyConst.museum_itemObtainTime_date, TimeUtil.Milliseconds2StringYMD(_m_itemInfo.obtainTimeMS)));
            _m_iconTexture?.setTexture(_m_itemInfo.refObj.icon);
            _m_skillIconTexture?.setTexture(_m_itemInfo.refObj.skill_icon);
            _m_qualityIconTexture?.setTexture(_m_itemInfo.refObj.quality_ref?.icon);
            _m_qualityShowGoWnd?.setData(_m_itemInfo.refObj.quality_ext_ref);
            
            MuseumItemLevelRefObj curLevelRefObj = _m_itemInfo.levelProperty.levelRefObj;
            MuseumItemLevelRefObj nextLevelRefObj = _m_itemInfo.levelProperty.nextLevelRefObj ?? curLevelRefObj;
            MuseumItemUpgradeCostRefObj curLevelUpgradeCostRefObj = _m_itemInfo.levelProperty.costRefObj;
            
            _m_levelUpgradeShow?.setValue(curLevelRefObj.level, nextLevelRefObj.level);
            ALUGUICommon.setLabelTxt(wnd.txtBonusLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, curLevelRefObj.level));
            ALUGUICommon.setLabelTxt(wnd.txtCurrentBonusPlus, TextTranslate.instance.getLanguage(curLevelRefObj.bonus_desc_key, curLevelRefObj.bonus_desc_params));
            ALUGUICommon.setLabelTxt(wnd.txtNextBonusPlus, TextTranslate.instance.getLanguage(curLevelRefObj.next_bonus_desc_key, curLevelRefObj.next_bonus_desc_params));
            _m_upgradeCostWnd?.setItem(curLevelUpgradeCostRefObj?.upgrade_cost_item);
        }

        /// <summary>
        /// 播放升级成功特效
        /// </summary>
        private void _playUpgradeSfx()
        {
            if (wnd == null)
                return;

            if (_m_lUpgradeSfxObjList == null)
                _m_lUpgradeSfxObjList = new List<CommonUISfxObj>();

            if (wnd.upgradeSfxId > 0 && wnd.transUpgradeSfx != null)
            {
                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxId, wnd.transUpgradeSfx);
                _m_lUpgradeSfxObjList.Add(sfxObj);
            }

            if (wnd.upgradeSfxId2 > 0 && wnd.transUpgradeSfx2 != null)
            {
                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxId2, wnd.transUpgradeSfx2);
                _m_lUpgradeSfxObjList.Add(sfxObj);
            }
        }

        /// <summary>
        /// 消耗升级特效
        /// </summary>
        private void _discardUpgradeSfx()
        {
            if (_m_lUpgradeSfxObjList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lUpgradeSfxObjList)
                {
                    sfxObj?.forceDiscard();
                }
            }
            _m_lUpgradeSfxObjList = null;
        }

        private void _onBtnActivateClicked(GameObject _)
        {
            if (_m_itemInfo == null)
                return;
            
            NPGSClientListener.sendRequestByLog(new GC2GS_035_002_ReqMuseumItemActive(_m_itemInfo.itemId), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    if (wnd != null && wnd.transEffectActivate != null)
                        PlaySfxMgr.instance.playUISfx(wnd.effectIdActivate, wnd.transEffectActivate);
                }));
        }
        private void _onBtnUpgradeClicked(GameObject _)
        {
            if (_m_itemInfo?.levelProperty?.costRefObj?.upgrade_cost_item == null)
                return;

            if (!GCommon.isItemEnough(_m_itemInfo.levelProperty.costRefObj.upgrade_cost_item, true))
                return;
            
            NPGSClientListener.sendRequestByLog(new GC2GS_035_001_ReqMuseumItemUpgrade(_m_itemInfo.itemId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_035_001_RetMuseumItemUpgrade>((_isSuc,_msg) =>
                {
                    if (_isSuc)
                    {
                        // 播放升级成功特效
                        _playUpgradeSfx();
                        // 提示升级成功
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.inn_museumItemUpgradeSuc_name, _m_itemInfo?.refObj?.name));
                    }
                }));
        }
        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MUSEUM_ITEM_INFO);
        }
        private void _onItemChg(MuseumItemInfo _itemInfo)
        {
            if (_itemInfo != _m_itemInfo)
                return;

            refreshWnd();
        }
        private void _onBtnNextItemClicked(GameObject _)
        {
            _discardUpgradeSfx();
            refreshWnd(_m_nextItemInfo, _m_itemList);
        }
        private void _onBtnPrevItemClicked(GameObject _)
        {
            _discardUpgradeSfx();
            refreshWnd(_m_prevItemInfo, _m_itemList);
        }
    }
}