using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴星辉等级列表，选中会居中显示
    /// </summary>
    public class GGUIWndHeroHaloLevelContainer: _AGGUISubWndPosFixContainer<GGUIMonoHeroHaloLevelContainerItem, GGUIMonoHeroHaloLevelContainer, GGUIWndHeroHaloLevelContainerItem>
    {
        //星辉列表
        private List<HeroHaloLevelRefObj> _m_lHaloLevelRef;
        //最大等级数据
        private HeroHaloLevelRefObj _m_haloMaxLevelRef;
        //伙伴id
        private long _m_lHeroId;
        // 当前选中的item
        private GGUIWndHeroHaloLevelContainerItem _m_wCurSelectedItem;

        /// <summary>
        /// 当前选中的item
        /// </summary>
        public GGUIWndHeroHaloLevelContainerItem curSelectedItem { get { return _m_wCurSelectedItem; } }

        public GGUIWndHeroHaloLevelContainer(GGUIMonoHeroHaloLevelContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndHeroHaloLevelContainerItem _createItemWnd(GGUIMonoHeroHaloLevelContainerItem _itemMono)
        {
            return new GGUIWndHeroHaloLevelContainerItem(_itemMono, _onSelectItem);
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            onItemIndexChanged -= _onSelectChg;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            onItemIndexChanged += _onSelectChg;
            _onSelectChg();
        }

        protected override void _refreshItemWnd(GGUIWndHeroHaloLevelContainerItem _itemWnd, int _index)
        {
            if (_m_lHaloLevelRef == null)
                return;

            if (_index < 0 || _index >= _m_lHaloLevelRef.Count)
                return;

            _itemWnd.setInfo(_m_lHaloLevelRef[_index], _m_haloMaxLevelRef, _m_lHeroId);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_levelRefList"></param>
        /// <param name="_heroId"></param>
        public void setInfo(List<HeroHaloLevelRefObj> _levelRefList, long _heroId)
        {
            if (_levelRefList == null)
                return;

            _m_lHaloLevelRef = _levelRefList;
            _m_lHeroId = _heroId;

            //获取当前星辉等级
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_heroId);
            bool isUnlock = heroInfo != null && heroInfo.heroHaloInfo != null && heroInfo.heroHaloInfo.isUnlock;
            long curLevel = heroInfo != null && heroInfo.heroHaloInfo != null ? heroInfo.heroHaloInfo.level : 0;
            int selectIndex = -1;

            //获取最大等级数据
            HeroHaloLevelRefObj maxLevelRef = null;
            for (int i = 0; i < _levelRefList.Count; i++)
            {
                if (maxLevelRef == null || maxLevelRef.level < _levelRefList[i].level)
                    maxLevelRef = _levelRefList[i];

                //已解锁默认选中当前等级的下一个
                if (isUnlock && curLevel + 1 == _levelRefList[i].level)
                    selectIndex = i;
            }

            //如果没有找到下一个等级，则选中最大等级
            if (selectIndex == -1)
                selectIndex = _m_lHaloLevelRef.Count - 1;

            refreshWnd(_m_lHaloLevelRef?.Count ?? 0);
            fixToItemIndex(selectIndex, false);
            if(!isUnlock)
                fixToItemIndex(0, false);
            _onSelectChg();
        }

        /// <summary>
        /// 播放升级激活动画
        /// </summary>
        public void playUpgradeAni(long _level)
        {
            dealAllWnd(_item =>
            {
                if (_item != null && _item.haloLevelRef != null && _item.haloLevelRef.level == _level)
                    _item.playUpgradeAni();
            });
        }

        /// <summary>
        /// 选中某个item事件
        /// </summary>
        /// <param name="_item"></param>
        private void _onSelectItem(GGUIWndHeroHaloLevelContainerItem _item)
        {
            if (_item == null || _m_lHaloLevelRef == null)
                return;

            int index = _m_lHaloLevelRef.IndexOf(_item.haloLevelRef);
            fixToItemIndex(index, true);
        }

        /// <summary>
        /// 刷新当前选中item显示
        /// </summary>
        public void refreshCurItem()
        {
            GGUIWndHeroHaloLevelContainerItem selectedItem = getItem(currentItemIndex);
            _m_wCurSelectedItem?.setSelect(true);
        }
        
        /// <summary>
        /// 选择的item改变
        /// </summary>
        private void _onSelectChg()
        {
            GGUIWndHeroHaloLevelContainerItem selectedItem = getItem(currentItemIndex);
            if (selectedItem == _m_wCurSelectedItem)
                return;

            _m_wCurSelectedItem?.setSelect(false);
            _m_wCurSelectedItem = selectedItem;
            _m_wCurSelectedItem?.setSelect(true);
        }
    }
}