using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 午间副本宝箱领取详情容器
    /// </summary>
    public class GGUIWndMiddayDungeonBoxRecordGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMiddayDungeonBoxRecordGridItem,GGUIMonoMiddayDungeonBoxRecordGrid,GGUIWndMiddayDungeonBoxRecordGridItem>
    {
        private List<MiddayDungeonBoxRecord> _m_itemDataList = new List<MiddayDungeonBoxRecord>();
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndMiddayDungeonBoxRecordGrid(GGUIMonoMiddayDungeonBoxRecordGrid gridMono) : base(gridMono)
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
            if(null == wnd)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndMiddayDungeonBoxRecordGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            _itemWnd?.setInfo(_m_itemDataList[_itemIdx]);

        }

        protected override GGUIWndMiddayDungeonBoxRecordGridItem _createItemWnd(GGUIMonoMiddayDungeonBoxRecordGridItem _itemMono)
        {
            GGUIWndMiddayDungeonBoxRecordGridItem itemWnd = new GGUIWndMiddayDungeonBoxRecordGridItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<MiddayDungeonBoxRecord> _itemDataList)
        {
            if (_itemDataList == null)
            {
                ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, true);
                return;
            }
            _m_itemDataList.Clear();
            _m_itemDataList.AddRange(_itemDataList);
            setItemCount(_m_itemDataList.Count);
            ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, _m_itemDataList.Count <= 0);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
