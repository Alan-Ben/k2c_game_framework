using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 商店入口
    /// </summary>
    public class GGUICustomMonoShopEntrance : MonoBehaviour
    {
        [ALHeader("商店id")]
        public long shopId;
        
        [ALHeader("红点显示脚本")]
        public NPGGUIMonoCommonRedTip monoRedTip;
        
        [ALHeader("打开商店按钮")]
        public GameObject btnOpen;
        
        [ALHeader("锁定状态显示信息列表")]
        public List<NPCommonEnumStatMutexShowInfo<ECommonLockState>> lockStateShowInfoList;

        [ALHeader("条件不满足时是否可以打开商店(若为true, 不做拦截还是可以打开商店 ;若为false, 点击时会进行拦截并弹出解锁条件描述tip)")]
        public bool onConditionDisableCanOpen;

#if NP_GAME
        private NPGGUIWndCommonRedTip _m_wRedTip;
#endif
        
        private ShopMainRefObj _m_shopMainRefObj;

        private ShopMainRefObj shopMainRefObj
        {
            get
            {
#if NP_GAME
                if(_m_shopMainRefObj == null || _m_shopMainRefObj.id != shopId)
                    _m_shopMainRefObj = GRefdataCoreMgr.instance.shopMainRefCore.getRef(shopId);
                return _m_shopMainRefObj;
#else
                return null;
#endif
                
            }
        }

        private void Awake()
        {
            ALUGUICommon.combineBtnClick(btnOpen, _onOpenBtnClick);
            
#if NP_GAME
            if (monoRedTip != null)
                _m_wRedTip = new NPGGUIWndCommonRedTip(monoRedTip);
#endif
        }

        private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(btnOpen, _onOpenBtnClick);

#if NP_GAME
            _m_wRedTip?.discard();
            _m_wRedTip = null;
#endif
        }

        private void OnEnable()
        {
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refresh);
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshRedTip);
            
#if NP_GAME
            _m_wRedTip?.showWnd();
#endif
            
            _refresh();
        }

        private void OnDisable()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _refresh);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshRedTip);
            
#if NP_GAME
            _m_wRedTip?.hideWnd();
#endif
        }

        private void _refresh()
        {
            if (lockStateShowInfoList != null)
            {
                NPCommonEnumStatMutexShowInfo<ECommonLockState>.setStat(lockStateShowInfoList, getShopUnlockState());
            }

            _refreshRedTip();
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
#if NP_GAME
            if(shopMainRefObj == null || shopMainRefObj.shop_id_list == null || _m_wRedTip == null)
                return;

            bool needShowRedTip = false;
            foreach (var shopId in shopMainRefObj.shop_id_list)
            {
                NPShopRefObj shopRefObj = GRefdataCoreMgr.instance.shopMap.getRef(shopId);
                if(shopRefObj == null)
                    continue;

                _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(shopRefObj.red_tip_id);
                if (redTipNode != null && redTipNode.needShow())
                {
                    needShowRedTip = true;
                    break;
                }
            }

            _m_wRedTip.showRedTipNum(needShowRedTip ? 1 : 0);
#endif
        }
        
        /// <summary>
        /// 获取商家的解锁状态
        /// </summary>
        /// <returns></returns>
        private ECommonLockState getShopUnlockState()
        {
            ShopMainRefObj showMainRefObj = shopMainRefObj;
            bool isUnlock = showMainRefObj != null && showMainRefObj.unlock_cond != null && showMainRefObj.unlock_cond.isNoConditionOrEnable(null);
            return isUnlock ? ECommonLockState.UNLOCKED : ECommonLockState.LOCKED;
        }

        /// <summary>
        /// 打开商店按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onOpenBtnClick(GameObject _go)
        {
#if NP_GAME
            if (getShopUnlockState() == ECommonLockState.LOCKED && !onConditionDisableCanOpen)
            {
                ShopMainRefObj showMainRefObj = shopMainRefObj;
                if(showMainRefObj != null)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(showMainRefObj.unlock_cond_desc, showMainRefObj.unlock_cond_desc_args));
                
                return;
            }
            
            QueueMgr.instance.AddNode(new GShopMainNode(shopId));      
#endif
        }
    }
}