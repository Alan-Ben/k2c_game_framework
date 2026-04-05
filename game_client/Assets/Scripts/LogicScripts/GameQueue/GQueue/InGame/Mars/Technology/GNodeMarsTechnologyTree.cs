namespace GOE
{
    /// <summary>
    /// 火星科技树
    /// </summary>
    public class GNodeMarsTechnologyTree : _AGNodeMarsTechnologyTree
    {
        //页签类型
        private EMarsTechnologyType _m_eSelectTechType = EMarsTechnologyType.NONE;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_technologyType">若这个参数传NONE, 进入窗口默认配置的页签</param>
        public GNodeMarsTechnologyTree(EMarsTechnologyType _technologyType) : base()
        {
            _m_eSelectTechType = _technologyType;
        }


        protected override void _dealShowWnd()
        {
            if(_m_bIsFirstEnter)
                GGUIWndMarsTechnologyTree.instance.setSelectTab(_m_eSelectTechType, true, true);
                
            GGUIWndMarsTechnologyTree.instance.showWnd();
        }
    }
}