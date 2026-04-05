using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndInnGuestListNormalPageGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoInnGuestListNormalPageGridItem, GGUIMonoInnGuestListNormalPageGrid, GGUISubWndInnGuestListNormalPageGridItem>
    {
        [ItemNotNull, NotNull] private readonly List<InnNormalGuestHandbookInfo> _m_guestList;
        
        
        public GGUISubWndInnGuestListNormalPageGrid(GGUIMonoInnGuestListNormalPageGrid _wnd) 
            : base(_wnd)
        {
            _m_guestList = new List<InnNormalGuestHandbookInfo>();
            
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
        protected override GGUISubWndInnGuestListNormalPageGridItem _createItemWnd(GGUIMonoInnGuestListNormalPageGridItem _itemMono)
        {
            return new GGUISubWndInnGuestListNormalPageGridItem(_itemMono);
        }
        protected override void _onRefreshItemWnd(GGUISubWndInnGuestListNormalPageGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null)
                return;
            
            InnNormalGuestHandbookInfo guestInfo = _m_guestList.SafeGet(_itemIdx);
            if (guestInfo == null)
                return;
            
            _itemMono.refreshWnd(guestInfo);
        }


        public void refreshWnd()
        {
            NPPlayer.instance.innComp.getNormalGuestHandbookInfoListNonAlloc(_m_guestList);
            _m_guestList.Sort((_a, _b) =>
            {
                bool aHasReward = _a.isUnlock && !_a.hadDrawReward;
                bool bHasReward = _b.isUnlock && !_b.hadDrawReward;
                if (aHasReward != bHasReward)
                    return aHasReward ? -1 : 1;
                
                if (_a.isUnlock != _b.isUnlock)
                    return _a.isUnlock ? -1 : 1;

                return _a.guestId.CompareTo(_b.guestId);
            });
            setItemCount(_m_guestList.Count);
        }
    }
}