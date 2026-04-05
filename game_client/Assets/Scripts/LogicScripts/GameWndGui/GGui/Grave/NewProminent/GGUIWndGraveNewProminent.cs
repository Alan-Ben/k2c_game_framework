using System;
using System.Collections.Generic;
using ALPackage;
using Common.GraveObj;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 新晋者列表界面
    /// </summary>
    public class GGUIWndGraveNewProminent : _ATALBasicUIWnd<GGUIMonoGraveNewProminent>
    {
        private static GGUIWndGraveNewProminent _g_instance = new GGUIWndGraveNewProminent();
    
        public static GGUIWndGraveNewProminent instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveNewProminent();
                return _g_instance;
            }
        }
        
        private GGUIWndGraveNewProminentGrid _m_itemGridWnd;
        private List<GraveObj_NewInfo> _m_itemDataList = new List<GraveObj_NewInfo>();

        // <AutoGen:WndDeclaration>
        
        // </AutoGen:WndDeclaration>

        public GGUIWndGraveNewProminent() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGraveNewProminent.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGraveNewProminent.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            _m_itemDataList.Clear();
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            // <AutoGen:_onDiscard>
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnCongrats, _onClickbtnCongrats);
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGraveNewProminentGrid(wnd.itemGrid);
                
            // <AutoGen:_onWndInitDone>
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnCongrats, _onClickbtnCongrats);
            // </AutoGen:_onWndInitDone>
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            ALUGUICommon.setGameObjEnable(wnd.hasNewProminentShowGos, false);
            ALUGUICommon.setGameObjEnable(wnd.noNewProminentShowGos, !true);
            
            NPPlayer.instance.graveComp.reqGraveConNewInfoList(_list =>
            {
                if (wnd == null)
                    return;

                _m_itemDataList = _list;
                NPSORewardRefObj refObj = GRefdataCoreMgr.instance.rewardMap.getRef(GRefdataCoreMgr.instance.npGeneral.grave_congratulate_reward_id);
                long gemCount = 0;
                if (refObj != null && refObj.show_item_list != null)
                    foreach (NPCommonCostItem item in refObj.show_item_list)
                    {
                        if (item != null && item.getCurrencyType() == ECurrency.GEM)
                        {
                            gemCount = item.count;
                            break;
                        }
                    }
                NPPlayerFixedCDInfo cdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.grave_congratulate_reward_gain_fixed_cd);

                int canGetRewardCount = Math.Min(_list != null ? _list.Count : 0, cdInfo != null ? cdInfo.getCount() : 0);
                ALUGUICommon.setLabelTxt(wnd.txtDiamondCount, gemCount * canGetRewardCount);
                bool hasNewProminent = _list != null && _list.Count > 0;
                ALUGUICommon.setGameObjEnable(wnd.hasNewProminentShowGos, hasNewProminent);
                ALUGUICommon.setGameObjEnable(wnd.noNewProminentShowGos, !hasNewProminent);
               
                _m_itemGridWnd?.showWnd();
                _m_itemGridWnd?.showItemList(_m_itemDataList);
            });
            
            // <AutoGen:_refreshWnd>
            
            // </AutoGen:_refreshWnd>
        }
        
        // <AutoGen:Method>
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GRAVE_NEW_PROMINENT);
            
            // <UserCode name="btnClose">
            // </UserCode>
        }
        // 祝贺按钮点击事件
        private void _onClickbtnCongrats(GameObject go)
        {
            // <UserCode name="btnCongrats">
            NPPlayer.instance.graveComp.reqGraveConNewInfo((_cid, _items) =>
            {
                if(_items == null)
                    return;
                List<NPCommonCostItem> gainItemList = new List<NPCommonCostItem>();
                foreach (var item in _items)
                {
                    gainItemList.Add(new NPCommonCostItem(item));
                }
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GRAVE_NEW_PROMINENT);
                NPNoticeDealer_GraveCongrats congratsNotice = new NPNoticeDealer_GraveCongrats(true, _cid, gainItemList);
                NPUINoticeMgr.instance.addDealer(congratsNotice);
                GCommon.dealGainItem(_items);
            });
            // </UserCode>
        }
        // </AutoGen:Method>
    }
}