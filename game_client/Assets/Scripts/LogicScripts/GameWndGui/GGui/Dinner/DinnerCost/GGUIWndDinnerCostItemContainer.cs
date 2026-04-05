using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会赴宴消耗item容器
    /// </summary>
    public class GGUIWndDinnerCostItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoDinnerCostItem,GGUIMonoDinnerCostItemContainer,GGUIWndDinnerCostItem>
    {
        private bool _m_isQuickJoin;//是否是快速赴宴
        public List<GGUIWndDinnerCostItem> _m_lItemGroupList;//子控件列表
        
        public event Action<GDinnerJoinCostRefObj> onClickJoin;
        public event Action<GDinnerJoinCostRefObj> onClickSave;
        public GGUIWndDinnerCostItemContainer(GGUIMonoDinnerCostItemContainer _containerMono) : base(_containerMono)
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
            
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscard()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
            onClickJoin = null;
            onClickSave = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndDinnerCostItem>();
        }

        protected override GGUIWndDinnerCostItem _createItemWnd(GGUIMonoDinnerCostItem _itemMono)
        {
            GGUIWndDinnerCostItem itemWnd = new GGUIWndDinnerCostItem(_itemMono);
            itemWnd.onClickJoin += _onClickJoin;
            itemWnd.onClickSave += _onClickSave;
            return itemWnd;
        }

        /// <summary>
        /// 使用消耗加入
        /// </summary>
        /// <param name="_costRef"></param>
        private void _onClickJoin(GDinnerJoinCostRefObj _costRef)
        {
            onClickJoin?.Invoke(_costRef);
        }
        /// <summary>
        /// 使用消耗加入
        /// </summary>
        /// <param name="_costRef"></param>
        private void _onClickSave(GDinnerJoinCostRefObj _costRef)
        {
            onClickSave?.Invoke(_costRef);
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        /// <param name="_canCostList"></param>
        public void showItemList(bool _isQuickJoin)
        {
            _m_isQuickJoin = _isQuickJoin;
            
            GDinnerJoinCostRefObj tempData = null;
            GGUIWndDinnerCostItem tempItemWnd = null;
            EGameCommonUnlockType itemUnlockType = EGameCommonUnlockType.LOCK;
            int count = 0;
            var _itemDataList = GRefdataCoreMgr.instance.dinnerJoinCostRefCore.refList;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[i];
                }

                tempItemWnd.setInfo(tempData, _m_isQuickJoin);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            _refreshContentLayout();
        }
        
        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }
    }
}
