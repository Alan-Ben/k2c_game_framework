using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的热更工程wnd基类
    /// </summary>
    public abstract class _AGGUIHotfixBasicWnd : _ANPGGUIBasicWnd<GGUIHotfixCommonMono>
    {
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public _AGGUIHotfixBasicWnd(EALUIWndLayer _layer) : base(_layer)
        {
        }
    }
}