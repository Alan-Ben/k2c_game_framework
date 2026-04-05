using ALPackage;

namespace GOE
{
    public class GGUIHomeEntryPointItemController : _AGGUIEntryPointFollowItemBaseController<GGUIMonoHomeEntryPointItem, GGUIWndHomeEntryPointItem>
    {
        public GGUIHomeEntryPointItemController():base(1130)
        {            
            
        }
        
        protected override GGUIWndHomeEntryPointItem _createItemWnd(GGUIMonoHomeEntryPointItem _wndMono)
        {
            return new GGUIWndHomeEntryPointItem(_wndMono);
        }
    }
}