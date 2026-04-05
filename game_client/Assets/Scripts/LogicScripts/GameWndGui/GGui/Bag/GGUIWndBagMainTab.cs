namespace GOE
{
    /// <summary>
    /// 背包主界面页签
    /// </summary>
    public class GGUIWndBagMainTab : _ATNPGGUIWndCommonTab<EBagMainTabMonoType, GGUIWndBagMainTab>
    {
        private long _m_resPathId;
        public long resPathId
        {
            get { return _m_resPathId; }
        }

        public GGUIWndBagMainTab(NPGGUIMonoCommonTab _wnd, EBagMainTabMonoType _bagItemType,long _resPathId) : base(_wnd, _bagItemType)
        {
            _m_resPathId = _resPathId;
        }
    }
}
