using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndPlayerHeroGainGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoPlayerHeroGainGridItem, GGUIMonoPlayerHeroGainGrid, GGUISubWndPlayerHeroGainGridItem>
    {
        //配置列表
        private List<PlayerHeroUnlockShowRefObj> _m_heroRefList;
        // 第一个未解锁的索引
        private int _m_iFirstLockIndex;
        
        
        public GGUISubWndPlayerHeroGainGrid(GGUIMonoPlayerHeroGainGrid _wnd) 
            : base(_wnd)
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
        }
        protected override void _onWndInitDone()
        {
        }
        

        protected override GGUISubWndPlayerHeroGainGridItem _createItemWnd(GGUIMonoPlayerHeroGainGridItem _itemMono)
        {
            return new GGUISubWndPlayerHeroGainGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndPlayerHeroGainGridItem _itemMono, int _itemIdx)
        {
            if (_m_heroRefList == null || _itemMono == null || _itemIdx < 0 || _itemIdx >= _m_heroRefList.Count)
                return;
            
            _itemMono.refreshWnd(_m_heroRefList[_itemIdx], _m_iFirstLockIndex == _itemIdx);
        }
        
        
        public void refreshWnd(List<PlayerHeroUnlockShowRefObj> _heroRefList)
        {
            _m_heroRefList = _heroRefList;
            _m_iFirstLockIndex = -1;
            if (_m_heroRefList != null)
            {
                for (int i = 0; i < _m_heroRefList.Count; i++)
                {
                    if (_m_heroRefList[i] != null &&
                        _m_heroRefList[i].unlock_condition != null &&
                        !_m_heroRefList[i].unlock_condition.isEmpty &&
                        !_m_heroRefList[i].unlock_condition.IsEnable(null))
                    {
                        _m_iFirstLockIndex = i;
                        break;
                    }
                }
            }

            setItemCount(_m_heroRefList?.Count ?? 0);
        }
    }
}