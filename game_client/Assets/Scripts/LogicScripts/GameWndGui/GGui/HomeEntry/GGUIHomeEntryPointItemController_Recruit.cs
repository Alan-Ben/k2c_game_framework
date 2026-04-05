namespace GOE
{
    public class GGUIHomeEntryPointItemController_Recruit : _AGGUIEntryPointFollowItemBaseController<GGUIMonoHomeEntryPointItem_Recruit, GGUIWndHomeEntryPointItem_Recruit>
    {
        public GGUIHomeEntryPointItemController_Recruit() : base(0)
        {
        }

        protected override GGUIWndHomeEntryPointItem_Recruit _createItemWnd(GGUIMonoHomeEntryPointItem_Recruit _wndMono)
        {
            return new GGUIWndHomeEntryPointItem_Recruit(_wndMono);
        }
    }
}