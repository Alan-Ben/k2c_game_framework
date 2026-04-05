namespace GOE
{
    /// <summary>
    /// 科技类型tab
    /// </summary>
    public class GGUIWndMarsTechnologyTypeTab : _ATNPGGUIWndCommonTab<EMarsTechnologyType, GGUIMonoMarsTechnologyTypeTab, GGUIWndMarsTechnologyTypeTab>
    {
        public GGUIWndMarsTechnologyTypeTab(GGUIMonoMarsTechnologyTypeTab _mono) : base(_mono, _mono == null ? EMarsTechnologyType.NONE : _mono.technologyType)
        {
        }
    }
}