using System;
using CommonEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 公会协作选择伙伴列表item容器
    /// </summary>
    public class GGUIWndGuildCooperateSelectHeroContainer : _AGGUISubWndPosFixContainer<GGUIMonoGuildCooperateSelectHeroContainerItem, GGUIMonoGuildCooperateSelectHeroContainer, GGUIWndGuildCooperateSelectHeroContainerItem>
    {
        // 伙伴列表
        private List<HeroInfo> _m_lHeroInfoList;
        // 属性据点类型
        private ESpecAttrType _m_ePosAttrType;
        // 当前选中的item
        private GGUIWndGuildCooperateSelectHeroContainerItem _m_wCurSelectedItem;
        // 点击item的检测函数
        private Func<bool> _m_fCheckCanClickItem;

        /// <summary>
        /// 当前选中的item
        /// </summary>
        public GGUIWndGuildCooperateSelectHeroContainerItem curSelectedItem { get { return _m_wCurSelectedItem; } }

        public GGUIWndGuildCooperateSelectHeroContainer(GGUIMonoGuildCooperateSelectHeroContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndGuildCooperateSelectHeroContainerItem _createItemWnd(GGUIMonoGuildCooperateSelectHeroContainerItem _itemMono)
        {
            GGUIWndGuildCooperateSelectHeroContainerItem item = new GGUIWndGuildCooperateSelectHeroContainerItem(_itemMono, _onSelectItem);
            return item;
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
            // WinMsg.RegisterMsg(WinMsgType.ON_GUILD_COOPERATE_HERO_USE_INFO_CHG, _onHeroUseInfoChg);
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            // WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_COOPERATE_HERO_USE_INFO_CHG, _onHeroUseInfoChg);
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

        protected override void _refreshItemWnd(GGUIWndGuildCooperateSelectHeroContainerItem _itemWnd, int _index)
        {
            if (_m_lHeroInfoList == null)
                return;

            if (_index < 0 || _index >= _m_lHeroInfoList.Count)
                return;

            _itemWnd.setInfo(_m_lHeroInfoList[_index], _m_ePosAttrType);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroInfoList"></param>
        public void setInfo(List<HeroInfo> _heroInfoList, ESpecAttrType _posAttrType, int _selectedHeroIndex, Func<bool> _CheckCanClickItem)
        {
            _m_lHeroInfoList = _heroInfoList;
            _m_ePosAttrType = _posAttrType;
            _m_fCheckCanClickItem = _CheckCanClickItem;
            refreshWnd(_m_lHeroInfoList?.Count ?? 0);
            fixToItemIndex(_selectedHeroIndex, false);
            _onSelectChg();
        }

        /// <summary>
        /// 选择的item改变
        /// </summary>
        private void _onSelectChg()
        {
            GGUIWndGuildCooperateSelectHeroContainerItem selectedItem = getItem(currentItemIndex);
            if (selectedItem == _m_wCurSelectedItem)
                return;

            _m_wCurSelectedItem?.setSelect(false);
            _m_wCurSelectedItem = selectedItem;
            _m_wCurSelectedItem?.setSelect(true);
        }

        /// <summary>
        /// 选中某个item事件
        /// </summary>
        /// <param name="_item"></param>
        private void _onSelectItem(GGUIWndGuildCooperateSelectHeroContainerItem _item)
        {
            if (_item == null || _m_lHeroInfoList == null || (_m_fCheckCanClickItem != null && !_m_fCheckCanClickItem.Invoke()))
                return;

            int index = _m_lHeroInfoList.IndexOf(_item.heroInfo);
            fixToItemIndex(index, true);
        }

        /// <summary>
        /// 伙伴使用信息改变
        /// </summary>
        /// <param name="_objects"></param>
        private void _onHeroUseInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long heroId = (long)_objects[0];
            if (heroId <= 0 || _m_lHeroInfoList == null || _m_lHeroInfoList.Count == 0)
                return;

            GGUIWndGuildCooperateSelectHeroContainerItem item = getItem(item => item.heroInfo != null && item.heroInfo.id == heroId);
            item?.refreshWnd();
        }
    }
}
