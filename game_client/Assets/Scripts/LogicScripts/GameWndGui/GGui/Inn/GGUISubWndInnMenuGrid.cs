using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndInnMenuGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoInnMenuGridItem, GGUIMonoInnMenuGrid, GGUISubWndInnMenuGridItem>
    {
        [ItemNotNull, NotNull] private readonly List<InnDishInfo> _m_dishList;
        
        
        public GGUISubWndInnMenuGrid(GGUIMonoInnMenuGrid _wnd)
            : base(_wnd)
        {
            _m_dishList = new List<InnDishInfo>();
            
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
        protected override GGUISubWndInnMenuGridItem _createItemWnd(GGUIMonoInnMenuGridItem _itemMono)
        {
            return new GGUISubWndInnMenuGridItem(_itemMono, this);
        }
        protected override void _onRefreshItemWnd(GGUISubWndInnMenuGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null)
                return;

            InnDishInfo dishInfo = _m_dishList.SafeGet(_itemIdx);
            if (dishInfo == null)
                return;
            
            _itemMono.refreshWnd(dishInfo, _itemIdx);
        }


        public void refreshWnd()
        {
            NPPlayer.instance.innComp.getDishListNonAlloc(_m_dishList);
            _m_dishList.Sort((_a, _b) =>
            {
                GGUIMonoInnMenuGridItemState aState = _a.uiState;
                GGUIMonoInnMenuGridItemState bState = _b.uiState;
                if (aState == bState)
                    return _a.dishId.CompareTo(_b.dishId);
                
                if (aState == GGUIMonoInnMenuGridItemState.HAS_RECIPE_AND_CONDITION_ENABLE)
                    return -1;
                if (bState == GGUIMonoInnMenuGridItemState.HAS_RECIPE_AND_CONDITION_ENABLE)
                    return 1;
                if (aState == GGUIMonoInnMenuGridItemState.HAS_RECIPE_BUT_CONDITION_DISABLE)
                    return -1;
                if (bState == GGUIMonoInnMenuGridItemState.HAS_RECIPE_BUT_CONDITION_DISABLE)
                    return 1;
                if (aState == GGUIMonoInnMenuGridItemState.UNLOCKED_AND_CAN_UPGRADE)
                    return -1;
                if (bState == GGUIMonoInnMenuGridItemState.UNLOCKED_AND_CAN_UPGRADE)
                    return 1;
                if (aState == GGUIMonoInnMenuGridItemState.UNLOCKED_BUT_CANNOT_UPGRADE)
                    return -1;
                if (bState == GGUIMonoInnMenuGridItemState.UNLOCKED_BUT_CANNOT_UPGRADE)
                    return 1;
                
                return _a.dishId.CompareTo(_b.dishId);
            });
            setItemCount(_m_dishList.Count);
        }


        [NotNull] public List<InnDishInfo> getDishList()
        {
            return _m_dishList;
        }
    }
}