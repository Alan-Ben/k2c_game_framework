using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 邀约结果item列表
    /// </summary>
    public class GGUIWndConsortInviteRewardItemContainer : _AGGUISubWndCommonContainer<GGUIMonoConsortInviteRewardItem, GGUIMonoConsortInviteRewardItemContainer, GGUIWndConsortInviteRewardItem>
    {
        private List<Common.ConsortObj.Consort_CallRes> _m_lResultList;//结果列表
        
        public GGUIWndConsortInviteRewardItemContainer(GGUIMonoConsortInviteRewardItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortInviteRewardItem _createItemWnd(GGUIMonoConsortInviteRewardItem _itemMono)
        {
            GGUIWndConsortInviteRewardItem itemWnd = new GGUIWndConsortInviteRewardItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndConsortInviteRewardItem _itemWnd, int _index)
        {
            if(_m_lResultList == null|| _index < 0 || _index >= _m_lResultList.Count)
                return;
            
            _itemWnd.setData(_m_lResultList[_index]);
        }
        
        public void setData(List<Common.ConsortObj.Consort_CallRes> _lResultList)
        {
            _m_lResultList = _lResultList;
         
            refreshWnd(_m_lResultList?.Count ?? 0);
        }
    }
}