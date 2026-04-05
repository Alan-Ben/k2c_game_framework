using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    public struct CommonTextItemStruct
    {
        public bool needShowGo;
        public string strOne;
        public string strTwo;
        public NPGTextureIndex texture;
    }

    public class NPGGUIWndCommonTextItemGrid : _ANPGGUIBasicGridSubWnd<NPGGUIMonoCommonTextItem, NPGGUIMonoCommonTextItemGrid, NPGGUIWndCommonTextItem>
    {
        private List<CommonTextItemStruct> _m_itemStructList;
        public NPGGUIWndCommonTextItemGrid(NPGGUIMonoCommonTextItemGrid _containerMono) : base(_containerMono) 
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_itemStructList != null)
                _m_itemStructList.Clear();
            _m_itemStructList = null;
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override NPGGUIWndCommonTextItem _createItemWnd(NPGGUIMonoCommonTextItem _itemMono)
        {
            // 创建对象
            return new NPGGUIWndCommonTextItem(_itemMono);
        }

        // 刷新对象
        protected override void _refreshItemwnd(NPGGUIWndCommonTextItem _itemMono, int _itemIdx)
        {
            if(_itemIdx >= _m_itemStructList.Count)
                return;

            // 刷新物品UI
            CommonTextItemStruct structItem = _m_itemStructList[_itemIdx];
            _itemMono.setTxt(structItem.strOne, structItem.strTwo, structItem.texture, structItem.needShowGo);
        }

        // 初始化物品
        public void setItemList(List<CommonTextItemStruct> _itemStructList)
        {
            if (null == _itemStructList)
                return;

            _m_itemStructList = _itemStructList;
            // 刷新卡牌
            _refreshTextItemList();

            //滚到上边
            scrollMoveToTop();
        }

        // 刷新列表
        private void _refreshTextItemList()
        {
            // 空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_itemStructList.Count <= 0 ? 1 : 0);

            // 刷新grid
            setItemCount(_m_itemStructList.Count);
        }

        public void scrollMoveToTop()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }
    }
}
