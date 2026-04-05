using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultMainPageTabList : GGUISubWndCommonPageTabList<GGUIMonoAdultMainTabType, GGUIMonoAdultMainPageTabList, GGUIMonoAdultMainPageTabListItem>
    {
        public GGUISubWndAdultMainPageTabList(GGUIMonoAdultMainPageTabList _wnd) 
            : base(_wnd)
        {
            initWnd();
        }

        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoAdultMainTabType _type, Transform _wndParent)
        {
            return _type switch {
                GGUIMonoAdultMainTabType.Unmarried => new GGUIPrefabSubWndAdultMainUnmarried(_wndParent),
                GGUIMonoAdultMainTabType.Married => new GGUIPrefabSubWndAdultMainMarried(_wndParent),
                _ => null
            };
        }
    }
}