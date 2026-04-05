using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意 Tab 列表
    /// 驱动各页签页面（信件 / 求助）。
    /// </summary>
    public class GGUIWndMarsPopularWillTabList : GGUISubWndCommonPageTabList<EMarsPopularWillTabType, GGUIMonoMarsPopularWillTabList, GGUIMonoMarsPopularWillTab>
    {
        public GGUIWndMarsPopularWillTabList(GGUIMonoMarsPopularWillTabList _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(EMarsPopularWillTabType _type, Transform _wndParent)
        {
            // 通过 Mono 中的页签条目查找该页签配置（含 pagePathInfo）
            GGUIMonoMarsPopularWillTab tabMono = _getPageTabItem(_type);
            if (tabMono == null)
                return null;

            switch (_type)
            {
                case EMarsPopularWillTabType.LETTER:
                    return new GGUIWndMarsPopularWillLetterPage(tabMono.pagePathInfo, _wndParent);

                case EMarsPopularWillTabType.HELP:
                    return new GGUIWndMarsPopularWillHelpPage(tabMono.pagePathInfo, _wndParent);

                default:
                    return null;
            }
        }
    }
}
