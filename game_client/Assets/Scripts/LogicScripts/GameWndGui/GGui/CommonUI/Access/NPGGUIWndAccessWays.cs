using System;
using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取途径弹窗
    /// </summary>
    public class NPGGUIWndAccessWays : _ANPGGUIBasicWnd<NPGGUIMonoAccessWays>
    {
        private static NPGGUIWndAccessWays _g_instance;
        public static NPGGUIWndAccessWays instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndAccessWays();
                return _g_instance;
            }
        }

        private NPGGUIWndCommonItem _m_wItemWnd;//物品信息
        private NPCommonItem _m_curItem;//目标物品
        private long _m_lCustomHasItemNum = -1; //自定义拥有数量，-1表示不使用自定义数量

        //使用背包物品item缓存池
        private NPGAccessWayItemCache<NPGGUIWndAccessBagItem, NPGGUIMonoAccessBagItem> _m_cacheBagItemItem;
        //获取途径item缓存池
        private NPGAccessWayItemCache<NPGGUIWndAccessWayItem, NPGGUIMonoAccessWayItem> _m_cacheAccessWayItem;
        //合成系统item
        private NPGGUIWndAccessCombinedItem _m_accessCombinedItem;
        //附加信息
        private AccessAdditionData _m_additionData;

        private NPGGUIWndAccessWays() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return NPGGUIMonoAccessWays.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoAccessWays.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            _resetAllItem();
        }

        protected override void _onReset()
        {
            _m_wItemWnd?.resetWnd();
            _m_accessCombinedItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_curItem = null;

            _m_wItemWnd?.discard();
            _m_wItemWnd = null;

            if (_m_cacheBagItemItem != null)
                _m_cacheBagItemItem.discard();
            _m_cacheBagItemItem = null;

            if (_m_cacheAccessWayItem != null)
                _m_cacheAccessWayItem.discard();
            _m_cacheAccessWayItem = null;

            if (_m_accessCombinedItem != null)
                _m_accessCombinedItem.discard();
            _m_accessCombinedItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickBtnClose);

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //物品信息
            if (wnd.monoItem != null)
                _m_wItemWnd = new NPGGUIWndCommonItem(wnd.monoItem);

            //item 缓存池
            if (wnd.goItemParent != null)
            {
                if (wnd.monoBagItem != null)
                {
                    _m_cacheBagItemItem = new NPGAccessWayItemCache<NPGGUIWndAccessBagItem, NPGGUIMonoAccessBagItem>(wnd.goItemParent, 0, 5);
                    _m_cacheBagItemItem.init(wnd.monoBagItem);
                }

                if (wnd.monoAccessWay != null)
                {
                    _m_cacheAccessWayItem = new NPGAccessWayItemCache<NPGGUIWndAccessWayItem, NPGGUIMonoAccessWayItem>(wnd.goItemParent, 0, 5);
                    _m_cacheAccessWayItem.init(wnd.monoAccessWay);
                }
                if (wnd.monoAccessWay != null)
                {
                    NPGGUIMonoAccessCombinedItem monoCombined = GameObject.Instantiate(wnd.monoCombinedItem);
                    monoCombined.transform.SetParent(wnd.goItemParent);
                    monoCombined.transform.localPosition = Vector3.zero;
                    monoCombined.transform.localScale = Vector3.one;

                    _m_accessCombinedItem = new NPGGUIWndAccessCombinedItem(monoCombined);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickBtnClose);

        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_itemType"></param>
        /// <param name="_subId"></param>
        /// <param name="_additionData">获取途径附加信息</param>
        public void setInfo(ENPItemType _itemType, long _subId, long _customHasItemNum = -1, AccessAdditionData _additionData = null)
        {
            _m_curItem = new NPCommonItem(_itemType, _subId);
            _m_lCustomHasItemNum = _customHasItemNum;
            _m_additionData = _additionData;
        }

        //刷新窗口
        private void _refreshWnd()
        {
            //刷新物品信息
            _refreshItemInfo();
            //刷新item列表
            _refreshItemList();
        }

        //刷新物品信息
        private void _refreshItemInfo()
        {
            if (wnd == null || _m_curItem == null)
                return;

            //物品信息
            _m_wItemWnd?.setItem(new CommonItemData(_m_curItem, _m_lCustomHasItemNum >= 0 ? _m_lCustomHasItemNum : GCommon.getItemCount(_m_curItem)));

            //物品描述
            ALUGUICommon.setLabelTxt(wnd.txtItemDesc, GCommon.getItemDesc(_m_curItem.itemType, _m_curItem.itemId));
        }

        //刷新item列表
        private void _refreshItemList()
        {
            if (wnd == null)
                return;

            //item总高度
            float itemTotalHeight = 0;
            //先重置item
            _resetAllItem();

            List<_INPAccessInfoInterface> infoList = _getInfoList();
            //东西使用完毕再进来会显示空的 这里判断一下关闭弹窗
            if (infoList == null || infoList.Count == 0)
            {
                _refreshHeight(0);
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_ACCESS_WAYS);
                return;
            }
            for (int i = 0; i < infoList.Count; i++)
            {
                switch (infoList[i].type)
                {
                    //背包物品类型
                    case ENPAccessInfoType.BAG_ITEM:
                        if (_m_cacheBagItemItem != null)
                        {
                            NPGGUIWndAccessBagItem bagItem = _m_cacheBagItemItem.popAccessItem();
                            if (bagItem != null)
                            {
                                bagItem.showWnd();
                                bagItem.setInfo((NPAccessBagItemInfo)infoList[i], _m_additionData);
                                if(bagItem.rectTransform != null)
                                    itemTotalHeight += bagItem.rectTransform.rect.height;
                            }
                        }
                        break;
                    //获取途径类型
                    case ENPAccessInfoType.ACCESS:
                        if (_m_cacheAccessWayItem != null)
                        {
                            NPGGUIWndAccessWayItem wayItem = _m_cacheAccessWayItem.popAccessItem();
                            if (wayItem != null)
                            {
                                wayItem.showWnd();
                                wayItem.setInfo((NPAccessInfo)infoList[i]);
                                if (wayItem.rectTransform != null)
                                    itemTotalHeight += wayItem.rectTransform.rect.height;
                            }
                        }
                        break;
                    //合成系统类型
                    case ENPAccessInfoType.COMBIEND:
                        if (_m_accessCombinedItem != null)
                        {
                            //设置一下层级
                            _m_accessCombinedItem.rectTransform.SetSiblingIndex(i);
                            _m_accessCombinedItem.showWnd();
                            _m_accessCombinedItem.setInfo((NPAccessCombinedItemInfo)infoList[i]);
                            if (_m_accessCombinedItem.rectTransform != null)
                                itemTotalHeight += _m_accessCombinedItem.rectTransform.rect.height;
                        }
                        break;
                }
            }

            //再加上item间隙高度
            if (wnd.listLayoutGroup != null)
            {
                itemTotalHeight += (wnd.listLayoutGroup.spacing * (infoList.Count - 1));
            }

            //刷新高度
            _refreshHeight(itemTotalHeight);
        }


        //刷新窗口高度
        private void _refreshHeight(float _itemTotalHeight)
        {
            if (wnd == null || wnd.needChgHeightRectTransform == null)
                return;

            float targetHeight = wnd.noItemDefaultHeight + _itemTotalHeight;
            if (targetHeight > wnd.maxHeight)
                targetHeight = wnd.maxHeight;

            wnd.needChgHeightRectTransform.sizeDelta = new Vector2(wnd.needChgHeightRectTransform.sizeDelta.x, targetHeight);
        }

        //获取数据列表
        private List<_INPAccessInfoInterface> _getInfoList()
        {
            if (_m_curItem == null)
                return null;

            List<_INPAccessInfoInterface> accessInfoList = new List<_INPAccessInfoInterface>();

            //====背包物品列表====
            List<BagItemRefObj> bagItemRefObjList = GRefdataCoreMgr.instance.getCommonItemDefectBagItemRefList(_m_curItem);
            if (bagItemRefObjList != null && bagItemRefObjList.Count > 0)
            {
                foreach (BagItemRefObj bagItemRefObj in bagItemRefObjList)
                {
                    if(bagItemRefObj == null)
                        continue;
                    
                    if(GCommon.getItemCount(ENPItemType.BAG_ITEM, bagItemRefObj.id) > 0)
                        accessInfoList.Add(new NPAccessBagItemInfo(bagItemRefObj));
                }
            }

            //====获取途径信息列表====
            //必然获取途径
            List<long> sureAccessIdList = GCommon.getItemSureAccessWays(_m_curItem.itemType, _m_curItem.itemId);
            if (sureAccessIdList != null)
            {
                for (int i = 0; i < sureAccessIdList.Count; i++)
                {
                    accessInfoList.Add(new NPAccessInfo(sureAccessIdList[i], true));
                }
            }

            //====可能获取途径信息列表====
            List<long> possibleAccessIdList = GCommon.getItemPossibleAccessWays(_m_curItem.itemType, _m_curItem.itemId);
            if (possibleAccessIdList != null)
            {
                for (int i = 0; i < possibleAccessIdList.Count; i++)
                {
                    accessInfoList.Add(new NPAccessInfo(possibleAccessIdList[i], false));
                }
            }

            //====合成信息====
            NPAccessCombinedItemInfo combinedItem = new NPAccessCombinedItemInfo(_m_curItem.itemType, _m_curItem.itemId);
            if (combinedItem.oriItemId != 0)
                accessInfoList.Add(combinedItem);

            //====排序====
            accessInfoList.Sort((_a, _b) =>
            {
                if (_a.type.CompareTo(_b.type) != 0)
                    return _a.type.CompareTo(_b.type);
                else if (_a.quality.CompareTo(_b.quality) != 0)
                    return _a.quality.CompareTo(_b.quality);
                return _a.sortId.CompareTo(_b.sortId);
            });

            return accessInfoList;
        }

        //重置item
        private void _resetAllItem()
        {
            if (_m_cacheBagItemItem != null)
                _m_cacheBagItemItem.pushBackAllAccessItem();

            if (_m_cacheAccessWayItem != null)
                _m_cacheAccessWayItem.pushBackAllAccessItem();

            _m_accessCombinedItem?.hideWnd();
        }

        #region 消息事件

        // //背包信息变更
        // private void _onBagItemRemove(params object[] _objects)
        // {
        //     if (_objects == null || _objects.Length == 0 || _objects[0] == null)
        //         return;
        //
        //     BagItem bagItem = _objects[0] as BagItem;
        //     if (bagItem == null)
        //         return;
        //
        //     if (_m_cacheBagItemItem != null)
        //     {
        //         _m_cacheBagItemItem.pushBackAccessItemByCond((_item) =>
        //         {
        //             return _item != null &&
        //                    _item.bagItem != null &&
        //                    _item.bagItem.itemType == bagItem.itemType &&
        //                    _item.bagItem.itemId == bagItem.itemId;
        //         });
        //     }
        // }

        /// <summary>
        /// 
        /// </summary>
        private void _onCustomReload()
        {
            _refreshItemInfo();
        }
        
        #endregion


        #region 点击事件

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_ACCESS_WAYS);
        }

        #endregion
    }

    /// <summary>
    /// 获取途径附加信息
    /// </summary>
    public class AccessAdditionData
    {
        /// <summary>
        /// 使用道具预选的骑士ID
        /// </summary>
        public long useItemSelectHeroId;

        public AccessAdditionData()
        {
        }
    }
}
