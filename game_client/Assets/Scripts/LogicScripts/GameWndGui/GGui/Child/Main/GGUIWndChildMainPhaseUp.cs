using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GGUIWndChildMain
    {
        public class GGUIWndChildMainPhaseUp
        {
            [NotNull] private readonly GGUIMonoChildMainPhaseUp _m_wnd;
            private bool _m_bIsShow;
            
            
            public GGUIWndChildMainPhaseUp([NotNull] GGUIMonoChildMainPhaseUp _wnd)
            {
                _m_wnd = _wnd;
                initWnd();
            }
            
            
            public void showWnd()
            {
                _m_bIsShow = true;
                
                refreshWnd();
            }  
            public void hideWnd()
            {
                _m_bIsShow = false;
            }
            public void resetWnd()
            {
            }
            public void discard()
            {
            }
            public void initWnd()
            {
            }


            public void refreshWnd()
            {
                
            }
        }
    }
}