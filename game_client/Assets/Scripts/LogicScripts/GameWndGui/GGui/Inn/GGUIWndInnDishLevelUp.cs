using System.Collections.Generic;
using ALPackage;
using GC2GS.p034_InnOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnDishLevelUp : _ATALBasicUIWnd<GGUIMonoInnDishLevelUp>
    {
        [NotNull] public static GGUIWndInnDishLevelUp instance { get { return _g_instance ??= new GGUIWndInnDishLevelUp(); } }
        private static GGUIWndInnDishLevelUp _g_instance;


        private InnDishInfo _m_dishInfo;
        private List<InnDishInfo> _m_dishList;
        private InnDishInfo _m_prevDishInfo;
        private InnDishInfo _m_nextDishInfo;
        
        private NPGGuiWndTexture _m_dishIconWnd;
        private NPGGuiWndTexture _m_attrIconWnd;
        private TextUpgradePropertyShow<int> _m_levelChgShow;
        private TextUpgradePropertyShow<string> _m_bonusChgShow;
        private NPGGUIWndProgress _m_upgradeProgressWnd;
        

        public GGUIWndInnDishLevelUp()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoInnDishLevelUp.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnDishLevelUp.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_dishIconWnd?.showWnd();
            _m_attrIconWnd?.showWnd();
            _m_upgradeProgressWnd?.showWnd();
            
            refreshWnd();
            
            NPPlayer.instance.innComp.onDishChg += _onDishChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_DISH_LEVEL_UP_BUTTON, _onSimulateLevelUpClick);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onDishChg -= _onDishChg;
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_DISH_LEVEL_UP_BUTTON, _onSimulateLevelUpClick);
            
            _m_dishIconWnd?.hideWnd();
            _m_attrIconWnd?.hideWnd();
            _m_upgradeProgressWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_dishIconWnd?.discardTexture();
            _m_attrIconWnd?.discardTexture();
            _m_upgradeProgressWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_dishIconWnd?.discard();
            _m_dishIconWnd = null;
            _m_attrIconWnd?.discard();
            _m_attrIconWnd = null;
            _m_upgradeProgressWnd?.discard();
            _m_upgradeProgressWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnLevelUp, _onBtnLevelUpClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextDish, _onBtnNextDishClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevDish, _onBtnPrevDishClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgDishIcon != null)
                _m_dishIconWnd = new NPGGuiWndTexture(wnd.imgDishIcon);
            if (wnd.imgDishAttr != null)
                _m_attrIconWnd = new NPGGuiWndTexture(wnd.imgDishAttr);
            if (wnd.levelUpgradeShow != null)
                _m_levelChgShow = new TextUpgradePropertyShow<int>(wnd.levelUpgradeShow, TransKeyConst.common_level_num);
            if (wnd.bonusUpgradeShow != null)
                _m_bonusChgShow = new TextUpgradePropertyShow<string>(wnd.bonusUpgradeShow, string.Empty);
            if (wnd.monoFinesseProgress != null)
                _m_upgradeProgressWnd = new NPGGUIWndProgress(wnd.monoFinesseProgress);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnLevelUp, _onBtnLevelUpClicked);
            ALUGUICommon.combineBtnClick(wnd.btnNextDish, _onBtnNextDishClicked);
            ALUGUICommon.combineBtnClick(wnd.btnPrevDish, _onBtnPrevDishClicked);
        }


        public void refreshWnd(InnDishInfo _dishInfo, List<InnDishInfo> _dishList = null)
        {
            _m_dishInfo = _dishInfo;
            _m_dishList = _dishList;
            _m_prevDishInfo = null;
            _m_nextDishInfo = null;
            if (_m_dishList is { Count: > 1 })
            {
                int curIndex = _m_dishList.IndexOf(_m_dishInfo);
                if (curIndex >= 0)
                {
                    int nextIndex = NPGameUtility.intRepeat(curIndex + 1, _m_dishList.Count);
                    int prevIndex = NPGameUtility.intRepeat(curIndex - 1, _m_dishList.Count);
                    
                    _m_nextDishInfo = _m_dishList[nextIndex];
                    if (_m_nextDishInfo is not { isUnlock: true })
                        _m_nextDishInfo = null;
                    _m_prevDishInfo = _m_dishList[prevIndex];
                    if (_m_prevDishInfo is not { isUnlock: true })
                        _m_prevDishInfo = null;
                }
            }
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || _m_dishInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDishName, _m_dishInfo.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtDishNum, TextTranslate.instance.getLanguage(TransKeyConst.inn_dishNum_num, _m_dishInfo.num));
            ALUGUICommon.setLabelTxt(wnd.txtDishDesc, _m_dishInfo.descTranslated);
            _m_dishIconWnd?.setTexture(_m_dishInfo.refObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtPopularityAdd, _m_dishInfo.popularityAdd);
            ALUGUICommon.setLabelTxt(wnd.txtAffectionAdd, _m_dishInfo.affectionAdd);
            ALUGUICommon.setLabelTxt(wnd.txtFinesseAdd, _m_dishInfo.finesseAdd);
            _m_attrIconWnd?.setTexture(_m_dishInfo.attrRef?.icon);
            
            InnDishLevelRefObj curLevelRefObj = _m_dishInfo.levelRefObj;
            InnDishLevelRefObj nextLevelRefObj = _m_dishInfo.nextLevelRefObj ?? curLevelRefObj;
            int curLevel = curLevelRefObj?.level ?? 1;
            int nextLevel = nextLevelRefObj?.level ?? 1;
            _m_levelChgShow?.setValue(curLevel, nextLevel);
            ALUGUICommon.setLabelTxt(wnd.txtBonusDesc, TextTranslate.instance.getLanguage(TransKeyConst.inn_dishBuildingtype_name, _m_dishInfo.attrNameTranslated));
            long baseValue = _m_dishInfo.refObj.bonus_prop_modifier_per_level.getPropValue(wnd.bonusPropertyType);
            long curValue = baseValue * curLevel;
            long nextValue = baseValue * nextLevel;
            _m_bonusChgShow?.setValue(_getBonusValueStr(curValue, wnd.isPercentage), _getBonusValueStr(nextValue, wnd.isPercentage));
            _m_upgradeProgressWnd?.setProgress(_m_dishInfo.finesse, curLevelRefObj?.up_need_finesse ?? 0, EValueFormatType.NORMAL_NOT_LARGE_STR);
            wnd.setCanUpgrade(_m_dishInfo.canUpgrade);
            wnd.setLevelMax(_m_dishInfo.isLevelMax);
            wnd.setHasNextDish(_m_nextDishInfo != null);
            wnd.setHasPrevDish(_m_prevDishInfo != null);
        }


        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_DISH_LEVEL_UP);
        }
        private void _onBtnLevelUpClicked(GameObject _)
        {
            if (_m_dishInfo == null)
                return;

            if (!_m_dishInfo.canUpgrade)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.inn_dishCantUpgrade_name, _m_dishInfo.nameTranslated));
                return;
            }
            
            NPGSClientListener.sendRequestByLog(new GC2GS_034_005_ReqInnDishUpgrade(_m_dishInfo.dishId), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() => NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.inn_dishUpgradeSuccess_none)));
        }
        private void _onBtnNextDishClicked(GameObject _)
        {
            refreshWnd(_m_nextDishInfo, _m_dishList);
        }
        private void _onBtnPrevDishClicked(GameObject _)
        {
            refreshWnd(_m_prevDishInfo, _m_dishList);
        }
        private void _onDishChg(InnDishInfo _dishInfo)
        {
            if (_dishInfo != _m_dishInfo)
                return;

            refreshWnd();
        }
        private void _onSimulateLevelUpClick()
        {
            if (wnd == null) return;
            _onBtnLevelUpClicked(wnd.btnLevelUp);
        }
        private string _getBonusValueStr(long _value, bool _isPercentage)
        {
            if (_isPercentage)
                return TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, _value / 100f);
            
            return _value.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT);
        }
    }
}