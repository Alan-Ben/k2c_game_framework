using System.Collections.Generic;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 委派列表item容器
    /// </summary>
    public class GGUIWndWeekCardAssignSubItemContainer : _AGUISubWndLoadPrefabContainer<_AGGUIMonoWeekCardAssignSubItemBase,GGUIMonoWeekCardAssignSubItemContainer,_AGGUIWndWeekCardAssignSubItemBase>
    {
        private List<_IWeekCardAssignItemShowInfo> _m_itemDataList;

        public GGUIWndWeekCardAssignSubItemContainer(GGUIMonoWeekCardAssignSubItemContainer _containerMono) : base(_containerMono)
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
            
        }

        protected override long _getItemUIPathId(int _itemDataIdx)
        {
            _IWeekCardAssignItemShowInfo _itemInfo = _getItemInfo(_itemDataIdx);
            if (null != _itemInfo)
                return _itemInfo.getUIPathId();
            return 0;
        }

        protected override _AGGUIWndWeekCardAssignSubItemBase _createItemWnd(string assetPath, string objName, Transform _parent, int _itemIdx)
        {
            _IWeekCardAssignItemShowInfo _itemInfo = _getItemInfo(_itemIdx);
            if (null != _itemInfo)
            {
                switch (_itemInfo.getAssignType())
                {
                    case EWeekCardSettleType.COLLEGE_STUDY :
                        return new GGUIWndWeekCardAssignSubItem_College(assetPath,objName, _parent);
                    
                    default:
                        return new GGUIWndWeekCardAssignSubItem_Normal(assetPath,objName, _parent);
                }
            }

            return null;
        }
        

        protected override void _onAddItemWnd(_AGGUIWndWeekCardAssignSubItemBase _itemWnd, int _itemDataIdx)
        {
            _IWeekCardAssignItemShowInfo _itemInfo = _getItemInfo(_itemDataIdx);
            if (null != _itemInfo)
            {
                _itemWnd.setInfo(_itemInfo);
            }
        }

        private _IWeekCardAssignItemShowInfo _getItemInfo(int _itemDataIdx)
        {
            if (_itemDataIdx >= 0 && null != _m_itemDataList && _m_itemDataList.Count > _itemDataIdx)
                return _m_itemDataList[_itemDataIdx];
            return null;
        }
        
        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void setInfo(List<_IWeekCardAssignItemShowInfo> _itemDataList)
        {
            if(null == _itemDataList)
                return;
            _m_itemDataList = _itemDataList;
            _setItemCount(_m_itemDataList.Count);
        }
    }
}
