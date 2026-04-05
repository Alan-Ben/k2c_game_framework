using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 居民tab列表
    /// </summary>
    public class GGUIWndMarsResidentTabList : GGUISubWndCommonPageTabList<EMarsResidentTabType, GGUIMonoMarsResidentTabList, GGUIMonoMarsResidentTab>
    {
        public GGUIWndMarsResidentTabList(GGUIMonoMarsResidentTabList _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(EMarsResidentTabType _type, Transform _wndParent)
        {
            GGUIMonoMarsResidentTab tabMono = _getPageTabItem(_type);
            if (tabMono == null)
                return null;
            
            switch (_type)
            {
                case EMarsResidentTabType.STATE:
                    return new GGUIWndMarsResidentStatePage(_wndParent, tabMono.pagePathInfo);
                
                case EMarsResidentTabType.ATTRIBUTE:
                    return new GGUIWndMarsResidentAttributePage(_wndParent, tabMono.pagePathInfo);
                
                default:
                    return null;
            }
        }
    }
}