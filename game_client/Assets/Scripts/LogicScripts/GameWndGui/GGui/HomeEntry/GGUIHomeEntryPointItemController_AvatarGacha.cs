using ALPackage;

namespace GOE
{
    public class GGUIHomeEntryPointItemController_AvatarGacha : _AGGUIEntryPointFollowItemBaseController<GGUIMonoHomeEntryPointItem_AvatarGacha, GGUIWndHomeEntryPointItem_AvatarGacha>
    {
        public GGUIHomeEntryPointItemController_AvatarGacha():base(2015)
        {            
            
        }
        
        protected override GGUIWndHomeEntryPointItem_AvatarGacha _createItemWnd(GGUIMonoHomeEntryPointItem_AvatarGacha _wndMono)
        {
            return new GGUIWndHomeEntryPointItem_AvatarGacha(_wndMono);
        }
    }
}