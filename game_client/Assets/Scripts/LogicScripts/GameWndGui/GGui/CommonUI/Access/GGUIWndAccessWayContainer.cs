using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取途径列表容器
    /// </summary>
    public class GGUIWndAccessWayContainer : _ANPGGUIBasicSubWnd<GGUIMonoAccessWayContainer>
    {
        //使用背包物品item缓存池
        private NPGAccessWayItemCache<NPGGUIWndAccessBagItem, NPGGUIMonoAccessBagItem> _m_cacheBagItemItem;

        //获取途径item缓存池
        private NPGAccessWayItemCache<NPGGUIWndAccessWayItem, NPGGUIMonoAccessWayItem> _m_cacheAccessWayItem;

        //合成系统item
        private NPGGUIWndAccessCombinedItem _m_accessCombinedItem;

        //附加信息
        private AccessAdditionData _m_additionData;

        public GGUIWndAccessWayContainer(GGUIMonoAccessWayContainer _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //item 缓存池初始化
            if (wnd.goItemParent != null)
            {
                if (wnd.monoBagItem != null)
                {
                    _m_cacheBagItemItem =
                        new NPGAccessWayItemCache<NPGGUIWndAccessBagItem, NPGGUIMonoAccessBagItem>(wnd.goItemParent, 0,
                            5);
                    _m_cacheBagItemItem.init(wnd.monoBagItem);
                }

                if (wnd.monoAccessWay != null)
                {
                    _m_cacheAccessWayItem =
                        new NPGAccessWayItemCache<NPGGUIWndAccessWayItem, NPGGUIMonoAccessWayItem>(wnd.goItemParent, 0,
                            5);
                    _m_cacheAccessWayItem.init(wnd.monoAccessWay);
                }

                if (wnd.monoCombinedItem != null)
                {
                    NPGGUIMonoAccessCombinedItem monoCombined = GameObject.Instantiate(wnd.monoCombinedItem);
                    monoCombined.transform.SetParent(wnd.goItemParent);
                    monoCombined.transform.localPosition = Vector3.zero;
                    monoCombined.transform.localScale = Vector3.one;

                    _m_accessCombinedItem = new NPGGUIWndAccessCombinedItem(monoCombined);
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_cacheBagItemItem != null)
                _m_cacheBagItemItem.discard();
            _m_cacheBagItemItem = null;

            if (_m_cacheAccessWayItem != null)
                _m_cacheAccessWayItem.discard();
            _m_cacheAccessWayItem = null;

            if (_m_accessCombinedItem != null)
                _m_accessCombinedItem.discard();
            _m_accessCombinedItem = null;

            _m_additionData = null;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _resetAllItem();
        }

        protected override void _onReset()
        {
            _m_accessCombinedItem?.resetWnd();
        }

        public void setAccessWayList(List<long> _sureAccessWayIdList, List<long> _possibleAccessIdList, AccessAdditionData _additionData = null)
        {
            List<_INPAccessInfoInterface> accessInfoList = new List<_INPAccessInfoInterface>();
            if (_sureAccessWayIdList != null)
            {
                foreach (long accessWayId in _sureAccessWayIdList)
                {
                    accessInfoList.Add(new NPAccessInfo(accessWayId, true));
                }
            }
            if (_possibleAccessIdList != null)
            {
                foreach (long accessWayId in _possibleAccessIdList)
                {
                    accessInfoList.Add(new NPAccessInfo(accessWayId, false));
                }
            }

            setAccessWayList(accessInfoList, _additionData);
        }
        
        /// <summary>
        /// 设置获取途径信息列表
        /// </summary>
        /// <param name="_infoList">获取途径信息列表</param>
        /// <param name="_additionData">附加信息</param>
        public void setAccessWayList(List<_INPAccessInfoInterface> _infoList, AccessAdditionData _additionData = null)
        {
            //====排序====
            _infoList?.Sort((_a, _b) =>
            {
                if (_a.type.CompareTo(_b.type) != 0)
                    return _a.type.CompareTo(_b.type);
                else if (_a.quality.CompareTo(_b.quality) != 0)
                    return _a.quality.CompareTo(_b.quality);
                return _a.sortId.CompareTo(_b.sortId);
            });
            
            _m_additionData = _additionData;
            _refreshItemList(_infoList);
        }

        /// <summary>
        /// 刷新item列表
        /// </summary>
        /// <param name="_infoList">获取途径信息列表</param>
        private void _refreshItemList(List<_INPAccessInfoInterface> _infoList)
        {
            if (wnd == null)
                return;

            //item总高度
            float itemTotalHeight = 0;
            //先重置item
            _resetAllItem();

            //检查是否有获取途径
            bool hasAccessWay = _infoList != null && _infoList.Count > 0;
            ALUGUICommon.setGameObjEnable(wnd.noAccessWayShowList, !hasAccessWay);

            if (!hasAccessWay)
            {
                _refreshHeight(0);
                return;
            }

            for (int i = 0; i < _infoList.Count; i++)
            {
                if (_infoList[i] == null)
                    continue;

                switch (_infoList[i].type)
                {
                    //背包物品类型
                    case ENPAccessInfoType.BAG_ITEM:
                        if (_m_cacheBagItemItem != null)
                        {
                            NPGGUIWndAccessBagItem bagItem = _m_cacheBagItemItem.popAccessItem();
                            if (bagItem != null)
                            {
                                bagItem.showWnd();
                                bagItem.setInfo((NPAccessBagItemInfo) _infoList[i], _m_additionData);
                                if (bagItem.rectTransform != null)
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
                                wayItem.setInfo((NPAccessInfo) _infoList[i]);
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
                            _m_accessCombinedItem.setInfo((NPAccessCombinedItemInfo) _infoList[i]);
                            if (_m_accessCombinedItem.rectTransform != null)
                                itemTotalHeight += _m_accessCombinedItem.rectTransform.rect.height;
                        }

                        break;
                }
            }

            //再加上item间隙高度
            if (wnd.listLayoutGroup != null && _infoList.Count > 1)
            {
                itemTotalHeight += (wnd.listLayoutGroup.spacing * (_infoList.Count - 1));
            }

            //刷新高度
            _refreshHeight(itemTotalHeight);
        }

        /// <summary>
        /// 刷新容器高度
        /// </summary>
        /// <param name="_itemTotalHeight">item总高度</param>
        private void _refreshHeight(float _itemTotalHeight)
        {
            if (wnd == null || wnd.needChgHeightRectTransform == null)
                return;

            float targetHeight = wnd.noItemDefaultHeight + _itemTotalHeight;
            if (targetHeight > wnd.maxHeight)
                targetHeight = wnd.maxHeight;

            wnd.needChgHeightRectTransform.sizeDelta =
                new Vector2(wnd.needChgHeightRectTransform.sizeDelta.x, targetHeight);
        }

        /// <summary>
        /// 重置item
        /// </summary>
        private void _resetAllItem()
        {
            if (_m_cacheBagItemItem != null)
                _m_cacheBagItemItem.pushBackAllAccessItem();

            if (_m_cacheAccessWayItem != null)
                _m_cacheAccessWayItem.pushBackAllAccessItem();

            _m_accessCombinedItem?.hideWnd();
        }

    }
}