using ALPackage;

namespace GOE
{
    public class GGUIHomeEntryPointItemController_DinnerList : _AGGUIEntryPointFollowItemBaseController<GGUIMonoHomeEntryPointItem_DinnerList, GGUIWndHomeEntryPointItem_DinnerList>
    {
        public GGUIHomeEntryPointItemController_DinnerList():base(2921)
        {            
            
        }
        
        protected override GGUIWndHomeEntryPointItem_DinnerList _createItemWnd(GGUIMonoHomeEntryPointItem_DinnerList _wndMono)
        {
            return new GGUIWndHomeEntryPointItem_DinnerList(_wndMono);
        }
    }
}