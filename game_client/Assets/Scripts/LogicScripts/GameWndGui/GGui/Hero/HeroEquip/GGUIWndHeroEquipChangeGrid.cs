using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 藏品列表
    /// </summary>
    public class GGUIWndHeroEquipChangeGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoHeroEquipChangeGridItem, GGUIMonoHeroEquipChangeGrid, GGUIWndHeroEquipChangeGridItem>
    {
        //总的数据列表
        [NotNull] private List<EquipInfo> _m_lInfoList = new List<EquipInfo>();
        //伙伴信息
        private HeroInfo _m_heroInfo;

        public GGUIWndHeroEquipChangeGrid(GGUIMonoHeroEquipChangeGrid _containerMono) : base(_containerMono)
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
            if (_m_lInfoList != null)
                _m_lInfoList.Clear();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndHeroEquipChangeGridItem _createItemWnd(GGUIMonoHeroEquipChangeGridItem _itemMono)
        {
            // 创建对象
            GGUIWndHeroEquipChangeGridItem gridItem = new GGUIWndHeroEquipChangeGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndHeroEquipChangeGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            EquipInfo heroShowInfo = _m_lInfoList[_itemIdx];
            _itemMono.setInfo(heroShowInfo, _m_heroInfo);
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void setInfo(List<EquipInfo> _infoList, HeroInfo _heroInfo)
        {
            if (null == _infoList)
                return;

            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_infoList);

            _m_heroInfo = _heroInfo;
            //刷新卡牌
            _refreshCardList();
            //滚到上边
            scrollMoveToTop();
        }

        /// <summary>
        /// 设置穿戴
        /// </summary>
        /// <param name="_index"></param>
        public void setWearByIndex(int _index)
        {
            if (_index >= _m_lInfoList.Count || _m_heroInfo == null)
                return;

            EquipInfo info = _m_lInfoList[_index];
            if (info == null)
                return;

            NPPlayer.instance.equipComp.reqHeroWearEquip(info.dbId, _m_heroInfo.id, () =>
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_EQUIP_CHANGE);
            });
        }

        // 刷新卡牌列表
        private void _refreshCardList()
        {
            if (wnd == null)
                return;

            //空物品提示
            ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lInfoList.Count <= 0 ? 1 : 0);

            //刷新grid
            setItemCount(_m_lInfoList.Count);
        }

        //移动到顶部
        public void scrollMoveToTop()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                moveToTop();
            });
        }
    }
}
