namespace GOE
{
    /// <summary>
    /// 通用简单的跟随窗口
    /// </summary>
    public class NPGGUIWndCommonItemToolTip :_ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonToolTip>
    {
        public NPGGUIWndCommonItemToolTip(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip));
        }
    }
}