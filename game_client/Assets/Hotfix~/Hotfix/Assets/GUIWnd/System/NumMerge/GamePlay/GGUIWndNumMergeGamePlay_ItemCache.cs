// 

using System;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private class ItemCache : _ATHotfixUIWndUnsafeCache<GGUIWndNumMergeGamePlayBlockItem, GGUIHotfixCommonMono>
        {
            private readonly Action<Vector2Int> _m_itemClickDelegate;
            
            
            public ItemCache(Transform _parentTrans, Action<Vector2Int> _itemClickDelegate) 
                : base(_parentTrans, 16, 32, 1)
            {
                _m_itemClickDelegate = _itemClickDelegate;
            }
            
            
            protected override string _warningTxt { get { return "NumMergeItemCache"; } }


            public new GGUIWndNumMergeGamePlayBlockItem popItem()
            {
                GGUIWndNumMergeGamePlayBlockItem item = base.popItem();
                item?.showWnd();
                return item;
            }
            
           
            protected override void _onInit(GGUIHotfixCommonMono _template)
            {
            }
            protected override void _resetItem(GGUIWndNumMergeGamePlayBlockItem _item)
            {
                _item?.hideWnd();
            }
            protected override GGUIWndNumMergeGamePlayBlockItem _createWndByMono(GGUIHotfixCommonMono _mono)
            {
                return new GGUIWndNumMergeGamePlayBlockItem(_mono, _m_itemClickDelegate);
            }
            protected override void _initNewWnd(GGUIWndNumMergeGamePlayBlockItem _wnd)
            {
            }
        }
    }
}