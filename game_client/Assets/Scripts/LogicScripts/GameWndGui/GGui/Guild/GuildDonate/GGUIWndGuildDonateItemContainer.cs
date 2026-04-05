using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndGuildDonateItemContainer : _AGGUISubWndCommonContainer<GGUIMonoGuildDonateItem, GGUIMonoGuildDonateItemContainer, GGUIWndGuildDonateItem>
    {
        private List<GuildConstructRefObj> _m_constructRefList;
        
        public GGUIWndGuildDonateItemContainer(GGUIMonoGuildDonateItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onHideWnd()
        {
            _m_constructRefList?.Clear();
            
            base._onHideWnd();
        }

        protected override GGUIWndGuildDonateItem _createItemWnd(GGUIMonoGuildDonateItem _itemMono)
        {
            GGUIWndGuildDonateItem itemWnd = new GGUIWndGuildDonateItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndGuildDonateItem _itemWnd, int _index)
        {
            if(_m_constructRefList == null || _index < 0 || _index >= _m_constructRefList.Count)
                return;
            
            _itemWnd.setInfo(_m_constructRefList[_index]);
        }

        /// <summary>
        /// 
        /// </summary>
        public void setData(List<GuildConstructRefObj> _constructRefList)
        {
            _m_constructRefList = _constructRefList;
         
            refreshWnd(_m_constructRefList?.Count ?? 0);
        }
    }
}