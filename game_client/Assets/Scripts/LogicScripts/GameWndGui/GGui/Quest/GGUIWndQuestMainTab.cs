namespace GOE
{
    /// <summary>
    /// 主线任务界面页签
    /// </summary>
    public class GGUIWndQuestMainTab : _ATNPGGUIWndCommonTab<EQuestMainTab, GGUIWndQuestMainTab>
    {
        private long _m_lSubWndAssetId;
        public long subWndAssetId { get { return _m_lSubWndAssetId; } }

        public GGUIWndQuestMainTab(NPGGUIMonoCommonTab _wnd, EQuestMainTab _tabType,long _subWndAssetId) : base(_wnd, _tabType)
        {
            _m_lSubWndAssetId = _subWndAssetId;
        }
    }
}
