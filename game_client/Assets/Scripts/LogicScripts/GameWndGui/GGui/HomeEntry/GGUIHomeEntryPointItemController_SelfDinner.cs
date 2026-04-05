using ALPackage;

namespace GOE
{
    public class GGUIHomeEntryPointItemController_SelfDinner : _AGGUIEntryPointFollowItemBaseController<GGUIMonoHomeEntryPointItem_SelfDinner, GGUIWndHomeEntryPointItem_SelfDinner>
    {
        public GGUIHomeEntryPointItemController_SelfDinner():base(2920)
        {            
            
        }
        
        protected override GGUIWndHomeEntryPointItem_SelfDinner _createItemWnd(GGUIMonoHomeEntryPointItem_SelfDinner _wndMono)
        {
            return new GGUIWndHomeEntryPointItem_SelfDinner(_wndMono);
        }
    }
}