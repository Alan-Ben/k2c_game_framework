using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndBusinessBuildingRnDPageProductGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoBusinessBuildingRnDPageProductGridItem>
    {
        private BusinessBuildingProductRefObj _m_productRef;
        private BusinessBuildingInfo _m_buildingInfo;

        private NPGGuiWndTexture _m_productIcon;
        private GGUISubWndCommonBonusShower _m_productBonus;
        private NPGGUIWndProgress _m_wUnlockProgress;//解锁进度条


        public GGUISubWndBusinessBuildingRnDPageProductGridItem(GGUIMonoBusinessBuildingRnDPageProductGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_BUILDING_PRODUCT_UNLOCK_BY_INDEX, _onSimulateClickUnlock);//模拟点击解锁
            _m_productIcon?.showWnd();
            _m_productBonus?.showWnd();
            _m_wUnlockProgress?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_BUILDING_PRODUCT_UNLOCK_BY_INDEX, _onSimulateClickUnlock);//模拟点击解锁
            _m_productIcon?.hideWnd();
            _m_productBonus?.hideWnd();
            _m_wUnlockProgress?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_productIcon?.discardTexture();
            _m_productBonus?.resetWnd();
            _m_wUnlockProgress?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_productIcon?.discard();
            _m_productBonus?.discard();
            _m_wUnlockProgress?.discard();
            _m_productIcon = null;
            _m_productBonus = null;
            _m_wUnlockProgress = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnUnlock, _onBtnUnlockClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgProductIcon != null)
                _m_productIcon = new NPGGuiWndTexture(wnd.imgProductIcon);
            if (wnd.monoProductBonus != null)
                _m_productBonus = new GGUISubWndCommonBonusShower(wnd.monoProductBonus);
            if (wnd.monoUnlockProgress != null)
                _m_wUnlockProgress = new NPGGUIWndProgress(wnd.monoUnlockProgress);

            ALUGUICommon.combineBtnClick(wnd.btnUnlock, _onBtnUnlockClick);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(BusinessBuildingProductRefObj _productRef, BusinessBuildingInfo _buildingInfo)
        {
            _m_productRef = _productRef;
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_productRef == null || _m_buildingInfo == null)
                return;
            
            _m_productIcon?.setTexture(_m_productRef.icon);
            ALUGUICommon.setLabelTxt(wnd.txtProductName, TextTranslate.instance.getLanguage(_m_productRef.name));
            _m_productBonus?.refreshWnd(_m_productRef.add_bonus.unionBonus, _m_buildingInfo.bonusJudgeParts);
            ALUGUICommon.setLabelTxt(wnd.txtUnlockRequire, _m_productRef.employee_required);

            //是否是下个待解锁的产品
            bool isNextUnlock = _getIsNextUnlock();
            if (isNextUnlock)
            {
                //是下个待解锁的产品，显示解锁进度条
                _m_wUnlockProgress?.showWnd();
                _m_wUnlockProgress?.setProgress(_m_buildingInfo.employeeNum, _m_productRef.employee_required, EValueFormatType.NORMAL_NOT_LARGE_STR);
            }
            else
                _m_wUnlockProgress?.hideWnd();
            wnd.setStateShow(_m_buildingInfo.employeeNum >= _m_productRef.employee_required, _m_buildingInfo.isProductUnlocked(_m_productRef.id), isNextUnlock);
        }

        /// <summary>
        /// 是否是下一个即将可解锁的产品
        /// </summary>
        /// <returns></returns>
        private bool _getIsNextUnlock()
        {
            if (_m_buildingInfo == null || _m_productRef == null)
                return false;

            List<BusinessBuildingProductRefObj> productList = GRefdataCoreMgr.instance.getBusinessBuildingProductRefList(_m_buildingInfo.id);
            if(productList == null)
                return false;

            for (int i = 0; i < productList.Count; i++)
            {
                // 找到第一个未解锁的产品
                if (productList[i] != null && 
                    productList[i].employee_required > _m_buildingInfo.employeeNum && 
                    !_m_buildingInfo.isProductUnlocked(productList[i].id))
                    return productList[i].id == _m_productRef.id;
            }
            return false;
        }
        
        private void _onBtnUnlockClick(GameObject _)
        {
            if (_m_productRef == null || _m_buildingInfo == null)
                return;

            BusinessBuildingProductRefObj productRef = _m_productRef;
            BusinessBuildingInfo buildingInfo = _m_buildingInfo;
            NPGSClientListener.sendRequestByLog(GSWriter_010_BuildingOp.make_007_ReqBusinessUnlockProduct(productRef.id), 
                new CommonErrCodeRequestCallbackDispatherTriggerDealer(() =>
                {
                    GGUIWndBusinessBuildingProductUnlock.instance.refreshWnd(productRef, buildingInfo);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBusinessBuildingProductUnlock.instance,
                        GGUIWndBusinessBuildingProductUnlock.instance.showWnd,
                        UINodeTagConst.C_BUSINESS_BUILDING_PRODUCT_UNLOCK);
                }));
        }

        //模拟点击解锁
        private void _onSimulateClickUnlock(params object[] _objects)
        {
            if(_objects == null || _objects.Length == 0) 
                return;

            long targetIndex = (long)_objects[0];

            if (targetIndex == itemIdx)
                _onBtnUnlockClick(null);
        }
    }
}