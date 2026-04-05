using ALPackage;
using System;
using System.Collections.Generic;
using Common.BagItemUseEnum;

namespace GOE
{
    /// <summary>
    /// 使用物品-情人列表
    /// </summary>
    public class GGUIWndBagItemUseConsortGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoBagItemUseConsortGridItem, GGUIMonoBagItemUseConsortGrid, GGUIWndBagItemUseConsortGridItem>
    {
        //骑士列表数据
        private List<GGottenConsortInfo> _m_consortInfoList;
        //使用道具
        private BagItem _m_bagItem;
        //使用回调
        private Action<BagItem,long> _m_useAction;

        public GGUIWndBagItemUseConsortGrid(GGUIMonoBagItemUseConsortGrid _containerMono) : base(_containerMono)
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
            _m_consortInfoList?.Clear();
            _m_consortInfoList = null;
        }

        protected override void _onWndInitDone()
        {
        }

        protected override GGUIWndBagItemUseConsortGridItem _createItemWnd(GGUIMonoBagItemUseConsortGridItem _itemMono)
        {
            // 创建对象
            return new GGUIWndBagItemUseConsortGridItem(_itemMono, _m_useAction);
        }

        protected override void _refreshItemwnd(GGUIWndBagItemUseConsortGridItem _itemWnd, int _itemIdx)
        {
            if (_m_consortInfoList == null || _itemIdx >= _m_consortInfoList.Count)
                return;

            //获取数据对象
            GGottenConsortInfo consortInfo = _m_consortInfoList[_itemIdx];
            if (null == consortInfo)
                return;

            _itemWnd.setItem(_m_bagItem, consortInfo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_bagItem"></param>
        /// <param name="_useAction"></param>
        public void setInfo(BagItem _bagItem, Action<BagItem,long> _useAction)
        {
            if (wnd == null || _bagItem == null)
                return;

            _m_bagItem = _bagItem;
            _m_useAction = _useAction;

            //获取当前拥有的骑士数据
            if(_m_consortInfoList == null)
                _m_consortInfoList = new List<GGottenConsortInfo>();
            _m_consortInfoList.Clear();

            //获取伙伴列表
            _m_consortInfoList = NPPlayer.instance.consortComp.getConsortList();
            //按照类型，取对应的值从大到小排序
            BagItemConsortRefObj bagItemConsortRef = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_m_bagItem.itemId);
            switch (bagItemConsortRef.show_type)
            {
                case EBagItemUse_ConsortType.INTIMACY:
                    _m_consortInfoList.Sort((_a, _b) => -(_a.intimacy.CompareTo(_b.intimacy)));
                    break;
                case EBagItemUse_ConsortType.CHARM:
                    _m_consortInfoList.Sort((_a, _b) => -(_a.charm.CompareTo(_b.charm)));
                    break;
                case EBagItemUse_ConsortType.CHARM_POINT:
                    _m_consortInfoList.Sort((_a, _b)=> -(_a.charmPoint.CompareTo(_b.charmPoint)));
                    break;
            }
            //刷新grid
            setItemCount(_m_consortInfoList.Count);
            //显示无数据提示
            ALUGUICommon.setGameObjEnable(wnd.zeroShowGoList, _m_consortInfoList.Count == 0);
        }
    }
}
