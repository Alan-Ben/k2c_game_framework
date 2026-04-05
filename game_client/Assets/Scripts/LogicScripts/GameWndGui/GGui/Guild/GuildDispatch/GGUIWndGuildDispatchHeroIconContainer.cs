using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣item容器
    /// </summary>
    public class GGUIWndGuildDispatchHeroIconContainer : _AGGUISubWndCommonContainer<GGUIMonoGuildDispatchHeroIcon, GGUIMonoGuildDispatchHeroIconContainer, GGUIWndGuildDispatchHeroIcon>
    {
        private List<Common.GuildObj.Guild_DispatchHeroInfo> _m_lDispatchHeroInfoList;//派遣大臣信息列表
        
        public GGUIWndGuildDispatchHeroIconContainer(GGUIMonoGuildDispatchHeroIconContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndGuildDispatchHeroIcon _createItemWnd(GGUIMonoGuildDispatchHeroIcon _itemMono)
        {
            GGUIWndGuildDispatchHeroIcon itemWnd = new GGUIWndGuildDispatchHeroIcon(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndGuildDispatchHeroIcon _itemWnd, int _index)
        {
            _itemWnd.setData(_m_lDispatchHeroInfoList?.SafeGet(_index));
        }

        public void setData(List<Common.GuildObj.Guild_DispatchHeroInfo> _dispatchHeroInfoList)
        {
            _m_lDispatchHeroInfoList = _dispatchHeroInfoList;
            
            refreshWnd(GRefdataCoreMgr.instance.npGeneral.each_attr_can_dispatch_hero_num);
        }
    }
}