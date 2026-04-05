using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣item容器
    /// </summary>
    public class GGUIWndGuildDispatchHeroCardContainer : _AGGUISubWndCommonContainer<GGUIMonoGuildDispatchHeroCard, GGUIMonoGuildDispatchHeroCardContainer, GGUIWndGuildDispatchHeroCard>
    {
        private List<GuildDispatchGottenHeroInfo> _m_lDispatchGottenHeroInfoList;//派遣获得的大臣信息列表
        private GGUIWndGuildDispatchHeroCard _m_wSelectedHeroCardWnd;//选中的大臣item
        
        public GGUIWndGuildDispatchHeroCardContainer(GGUIMonoGuildDispatchHeroCardContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        /// <summary>
        /// 选中的大臣信息
        /// </summary>
        public GuildDispatchGottenHeroInfo selectedDispatchGottenHeroInfo
        {
            get
            {
                return _m_wSelectedHeroCardWnd?.dispatchGottenHeroInfo;
            }
        }

        public event Action onSelectItemChg;//当选中item变化时

        protected override void _onDiscard()
        {
            onSelectItemChg = null;
            
            base._onDiscard();
        }

        protected override void _onHideWnd()
        {
            _m_lDispatchGottenHeroInfoList?.Clear();

            base._onHideWnd();
        }

        protected override GGUIWndGuildDispatchHeroCard _createItemWnd(GGUIMonoGuildDispatchHeroCard _itemMono)
        {
            GGUIWndGuildDispatchHeroCard itemWnd = new GGUIWndGuildDispatchHeroCard(_itemMono);
            itemWnd.onItemClick += _onItemClick;
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndGuildDispatchHeroCard _itemWnd, int _index)
        {
            if(_m_lDispatchGottenHeroInfoList == null || _index < 0 || _index >= _m_lDispatchGottenHeroInfoList.Count)
                return;
            
            _itemWnd.setData(_m_lDispatchGottenHeroInfoList[_index]);
            _itemWnd.setSelected(_m_wSelectedHeroCardWnd == _itemWnd);
        }

        public void setData(List<GuildDispatchGottenHeroInfo> _dispatchGottenHeroInfoList)
        {
            _m_lDispatchGottenHeroInfoList = _dispatchGottenHeroInfoList;
            _m_lDispatchGottenHeroInfoList.Sort(_sortList);
            _m_wSelectedHeroCardWnd = null;

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, _m_lDispatchGottenHeroInfoList == null || _m_lDispatchGottenHeroInfoList.Count <= 0);
            }
            
            refreshWnd(_m_lDispatchGottenHeroInfoList?.Count ?? 0);
        }

        /// <summary>
        /// 设置选中某一相性大臣
        /// </summary>
        public void setSelectAttrHero(ESpecAttrType _specAttrType)
        {
            // 获取指定相性的大臣item
            GGUIWndGuildDispatchHeroCard specAttrHeroCard = getItem((_item) =>
            {
                return _item != null && _item.dispatchGottenHeroInfo != null &&
                       _item.dispatchGottenHeroInfo.specAttrType == _specAttrType;
            });
            
            if(_m_wSelectedHeroCardWnd != null && _m_wSelectedHeroCardWnd == specAttrHeroCard)
                return;
            
            _m_wSelectedHeroCardWnd?.setSelected(false);
            _m_wSelectedHeroCardWnd = specAttrHeroCard;
            _m_wSelectedHeroCardWnd?.setSelected(true);
            
            onSelectItemChg?.Invoke();
        }
        
        /// <summary>
        /// 当item被点击
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onItemClick(GGUIWndGuildDispatchHeroCard _itemWnd)
        {
            if (_itemWnd == null || _m_wSelectedHeroCardWnd == _itemWnd)
                return;

            _m_wSelectedHeroCardWnd?.setSelected(false);
            _m_wSelectedHeroCardWnd = _itemWnd;
            _m_wSelectedHeroCardWnd?.setSelected(true);
            
            onSelectItemChg?.Invoke();
        }

        //排序：委任加成值从大到小，等级从大到小，ID从小到大
        private int _sortList(GuildDispatchGottenHeroInfo _a, GuildDispatchGottenHeroInfo _b)
        {
            if (_a == null || _b == null || _a.heroInfo == null || _b.heroInfo == null)
                return 0;

            if (_a.specAttrAddPro != _b.specAttrAddPro)
                return -(_a.specAttrAddPro.CompareTo(_b.specAttrAddPro));

            if(_a.heroInfo.level != _b.heroInfo.level)
                return -(_a.heroInfo.level.CompareTo(_b.heroInfo.level));

            return _a.heroInfo.id.CompareTo(_b.heroInfo.id);
        }
    }
}