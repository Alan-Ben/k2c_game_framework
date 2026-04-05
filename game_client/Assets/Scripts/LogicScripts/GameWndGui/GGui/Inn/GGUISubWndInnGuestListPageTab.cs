using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnGuestListPageTab : GGUISubWndCommonPageTabList<GGUIMonoInnGuestListPageTabType, GGUIMonoInnGuestListPageTab, GGUIMonoInnGuestListPageTabItem>
    {
        public GGUISubWndInnGuestListPageTab(GGUIMonoInnGuestListPageTab _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoInnGuestListPageTabType _type, Transform _wndParent)
        {
            return _type switch
            {
                GGUIMonoInnGuestListPageTabType.NORMAL_GUEST => new GGUIPrefabSubWndInnGuestListNormalPage(_wndParent),
                GGUIMonoInnGuestListPageTabType.SPECIAL_GUEST => new GGUIPrefabSubWndInnGuestListSpecialPage(_wndParent),
            };
        }
    }
}