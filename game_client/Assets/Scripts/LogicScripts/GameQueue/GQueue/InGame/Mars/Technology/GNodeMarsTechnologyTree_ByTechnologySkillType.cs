namespace GOE
{
    public class GNodeMarsTechnologyTree_ByTechnologySkillType : _AGNodeMarsTechnologyTree
    {
        private int _m_lSkillTypeID;
        
        public GNodeMarsTechnologyTree_ByTechnologySkillType(int _skillTypeId) : base()
        {
            _m_lSkillTypeID = _skillTypeId;
        }
        
        protected override void _dealShowWnd()
        {
            GGUIWndMarsTechnologyTree.instance.showWnd();
            if(_m_bIsFirstEnter)
                GGUIWndMarsTechnologyTree.instance.focusTechnologyBySkillType(_m_lSkillTypeID);
        }
    }
}