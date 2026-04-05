using System;
using System.Collections.Generic;
using ALPackage;
using ClientEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会举办item容器
    /// </summary>
    public class GGUIWndDinnerCreateItemContainer : _AGUISubWndLoadPrefabContainer<_AGGUIMonoDinnerCreateItemBase,GGUIMonoDinnerCreateItemContainer,_AGGUIWndDinnerCreateItemBase>
    {
        private List<DinnerCreateItemShowInfo> _m_itemDataList;//数据列表
        public GGUIWndDinnerCreateItemContainer(GGUIMonoDinnerCreateItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override long _getItemUIPathId(int _itemDataIdx)
        {
            DinnerCreateItemShowInfo _itemInfo = _getItemInfo(_itemDataIdx);
            if (null != _itemInfo)
            {
                if (_itemInfo.itemType == EDinnerCreateItemType.Item)
                {
                    if (_itemInfo.dinnerTypeRef != null) 
                        return _itemInfo.dinnerTypeRef.ui_path_id;
                    return 2919;
                }
                // Bar
                else
                {
                    return 2925;
                }
            }
            return 2919;
        }

        protected override _AGGUIWndDinnerCreateItemBase _createItemWnd(string assetPath, string objName, Transform _parent, int _itemIdx)
        {
            DinnerCreateItemShowInfo _itemInfo = _getItemInfo(_itemIdx);
            if (null != _itemInfo)
            {
                if (_itemInfo.itemType == EDinnerCreateItemType.Item)
                {
                    switch (_itemInfo.getDinnerCreateItemType())
                    {
                        case EDinnerShowType.FAMILY:
                            return new GGUIWndDinnerCreateItem_Consort(assetPath, objName, _parent);
                        case EDinnerShowType.CELEBRATION:
                            return new GGUIWndDinnerCreateItem_Celebration(assetPath, objName, _parent);
                        case EDinnerShowType.HIGH_PARTY:
                            return new GGUIWndDinnerCreateItem_Props(assetPath, objName, _parent);
                        case EDinnerShowType.GIFTDE_CHILD_CELE:
                            return new GGUIWndDinnerCreateItem_GiftdeChildCele(assetPath, objName, _parent);
                        default:
                            return new GGUIWndDinnerCreateItem_Props(assetPath, objName, _parent);
                    }
                }
                else
                {
                    return new GGUIWndDinnerCreateItem_Bar(assetPath, objName, _parent);
                }
             
            }
            return null;
        }

        protected override void _onAddItemWnd(_AGGUIWndDinnerCreateItemBase _itemWnd, int _itemDataIdx)
        {
            DinnerCreateItemShowInfo _itemInfo = _getItemInfo(_itemDataIdx);
            if (null != _itemInfo && null != _itemWnd)
            {
                _itemWnd.setInfo(_itemInfo);
            }
        }
        private DinnerCreateItemShowInfo _getItemInfo(int _itemDataIdx)
        {
            if (_itemDataIdx >= 0 && null != _m_itemDataList && _m_itemDataList.Count > _itemDataIdx)
                return _m_itemDataList[_itemDataIdx];
            return null;
        }
        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void setInfo(List<DinnerCreateItemShowInfo> _itemDataList)
        {
            if(null == _itemDataList)
                return;
            _m_itemDataList = _itemDataList;
            _setItemCount(_m_itemDataList.Count);
        }

        /// <summary>
        /// 根据下标创建宴会
        /// </summary>
        /// <param name="_index"></param>
        public void createDinnerByIndex(int _index)
        {
            _AGGUIWndDinnerCreateItemBase item = getItemWnd(_index);
            if(null != item)
            {
                item.setCreatDinner();
            }
        }
    }
}
