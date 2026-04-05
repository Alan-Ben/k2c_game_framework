using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 联盟派遣itemContainer
    /// </summary>
    public class GGUIWndGuildDispatchItemContainer : _AGGUISubWndCommonContainer<GGUIMonoGuildDispatchItem, GGUIMonoGuildDispatchItemContainer, GGUIWndGuildDispatchItem>
    {
        private List<GuildDispatchInfo> _m_lDispatchInfoList;//派遣信息列表

        public GGUIWndGuildDispatchItemContainer(GGUIMonoGuildDispatchItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndGuildDispatchItem _createItemWnd(GGUIMonoGuildDispatchItem _itemMono)
        {
            GGUIWndGuildDispatchItem itemWnd = new GGUIWndGuildDispatchItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndGuildDispatchItem _itemWnd, int _index)
        {
            if(_m_lDispatchInfoList == null || _index < 0 || _index >= _m_lDispatchInfoList.Count)
                return;
            
            _itemWnd.setData(_m_lDispatchInfoList[_index]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dispatchInfoList"></param>
        public void setData(List<GuildDispatchInfo> _dispatchInfoList)
        {
            _m_lDispatchInfoList = _dispatchInfoList;
            
            refreshWnd(_m_lDispatchInfoList?.Count ?? 0);
        }
    }
}