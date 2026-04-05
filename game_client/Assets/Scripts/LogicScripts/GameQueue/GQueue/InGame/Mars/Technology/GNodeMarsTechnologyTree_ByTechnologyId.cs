namespace GOE
{
    public class GNodeMarsTechnologyTree_ByTechnologyId : _AGNodeMarsTechnologyTree
    {
        private long _m_lFocusTechID;
        
        public GNodeMarsTechnologyTree_ByTechnologyId(long _techID) : base()
        {
            _m_lFocusTechID = _techID;
        }
        
        protected override void _dealShowWnd()
        {
            GGUIWndMarsTechnologyTree.instance.showWnd();
            if(_m_bIsFirstEnter)
                GGUIWndMarsTechnologyTree.instance.focusTechnology(_m_lFocusTechID);
        }
    }
}