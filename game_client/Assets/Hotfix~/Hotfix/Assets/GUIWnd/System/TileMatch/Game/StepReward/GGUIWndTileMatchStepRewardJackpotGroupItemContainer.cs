using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    public class GGUIWndTileMatchStepRewardJackpotGroupItemContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoTileMatchStepRewardJackpotGroupItemContainer, GGUIWndTileMatchStepRewardJackpotGroupItem>
    {
        private List<TileMatchJackpotGroupRefObj> _m_lJackpotGroupList;
        private int _m_iTotalWeight; // 奖池组总权重
        
        private List<GGUIWndTileMatchStepRewardJackpotGroupItem> _m_lSubWndList; // 子窗口列表
        
        public GGUIWndTileMatchStepRewardJackpotGroupItemContainer(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
        }

        protected override void _onDiscard()
        {
            if (_m_lSubWndList != null)
            {
                GGUIWndTileMatchStepRewardJackpotGroupItem itemWnd = null;
                for (int i = 0, count = _m_lSubWndList.Count; i < count; i++)
                {
                    itemWnd = _m_lSubWndList[i];
                    if(itemWnd != null)
                        itemWnd.resetWnd();
                }    
            }
            _m_lSubWndList = null;
            
            _m_lJackpotGroupList = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lJackpotGroupList = null;

            _hideAllSubWnd();
        }

        protected override void _onReset()
        {
            _m_lJackpotGroupList = null;

            if (_m_lSubWndList != null)
            {
                GGUIWndTileMatchStepRewardJackpotGroupItem itemWnd = null;
                for (int i = 0, count = _m_lSubWndList.Count; i < count; i++)
                {
                    itemWnd = _m_lSubWndList[i];
                    if(itemWnd != null)
                        itemWnd.resetWnd();
                }    
            }
        }
        
        protected override GGUIWndTileMatchStepRewardJackpotGroupItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            GGUIWndTileMatchStepRewardJackpotGroupItem itemWnd = new GGUIWndTileMatchStepRewardJackpotGroupItem(_itemMono);
            return itemWnd;
        }

        public void setData(List<TileMatchJackpotGroupRefObj> _jackpotGroupRefObjList, int _totalWeight)
        {
            _m_lJackpotGroupList = _jackpotGroupRefObjList;
            _m_iTotalWeight = _totalWeight;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (_m_lJackpotGroupList == null || _m_lJackpotGroupList.Count <= 0)
            {
                _hideAllSubWnd();
                return;
            }

            if (_m_lSubWndList == null)
                _m_lSubWndList = new List<GGUIWndTileMatchStepRewardJackpotGroupItem>();

            GGUIWndTileMatchStepRewardJackpotGroupItem itemWnd = null;
            TileMatchJackpotGroupRefObj jackpotGroupRefObj = null;
            int showWndCount = 0;
            for (int i = 0, count = _m_lJackpotGroupList.Count; i < count; i++)
            {
                jackpotGroupRefObj = _m_lJackpotGroupList[i];
                if(jackpotGroupRefObj == null)
                    continue;
                
                if (showWndCount >= _m_lSubWndList.Count)
                {
                    itemWnd = addItemWnd();
                    if(itemWnd != null)
                        _m_lSubWndList.Add(itemWnd);
                }
                else
                {
                    itemWnd = _m_lSubWndList[showWndCount];
                }

                if (itemWnd != null)
                {
                    itemWnd.showWnd();
                    itemWnd.setData(jackpotGroupRefObj, _m_iTotalWeight);
                    showWndCount++;
                }
            }

            for (int i = showWndCount, count = _m_lSubWndList.Count; i < count; i++)
            {
                itemWnd = _m_lSubWndList[i];
                if(itemWnd != null)
                    itemWnd.hideWnd();
            }
        }
        
        /// <summary>
        /// 隐藏所有子窗口
        /// </summary>
        private void _hideAllSubWnd()
        {
            if(_m_lSubWndList == null)
                return;

            GGUIWndTileMatchStepRewardJackpotGroupItem itemWnd = null;
            for (int i = 0, count = _m_lSubWndList.Count; i < count; i++)
            {
                itemWnd = _m_lSubWndList[i];
                if(itemWnd != null)
                    itemWnd.hideWnd();
            }
        }
    }
}