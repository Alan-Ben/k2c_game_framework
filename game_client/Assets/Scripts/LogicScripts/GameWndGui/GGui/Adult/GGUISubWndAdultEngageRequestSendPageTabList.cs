using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendPageTabList : GGUISubWndCommonPageTabList<GGUIMonoAdultEngageRequestSendTabType, GGUIMonoAdultEngageRequestSendPageTabList, GGUIMonoAdultEngageRequestSendPageTabListItem>
    {
        private AdultInfo _m_myAdultInfo;
        
        
        public GGUISubWndAdultEngageRequestSendPageTabList(GGUIMonoAdultEngageRequestSendPageTabList _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoAdultEngageRequestSendTabType _type, Transform _wndParent)
        {
            switch (_type)
            {
                case GGUIMonoAdultEngageRequestSendTabType.Server:
                {
                    GGUIPrefabSubWndAdultEngageRequestSendServer pageWnd = new GGUIPrefabSubWndAdultEngageRequestSendServer(_wndParent);
                    pageWnd.refreshWnd(_m_myAdultInfo);
                    return pageWnd;
                }
                case GGUIMonoAdultEngageRequestSendTabType.Guild:
                {
                    GGUIPrefabSubWndAdultEngageRequestSendGuild pageWnd = new GGUIPrefabSubWndAdultEngageRequestSendGuild(_wndParent);
                    pageWnd.refreshWnd(_m_myAdultInfo);
                    return pageWnd;
                }
                case GGUIMonoAdultEngageRequestSendTabType.Custom:
                {
                    GGUIPrefabSubWndAdultEngageRequestSendCustom pageWnd = new GGUIPrefabSubWndAdultEngageRequestSendCustom(_wndParent);
                    pageWnd.refreshWnd(_m_myAdultInfo);
                    return pageWnd;
                }
                default:
                    return null;
            }
        }


        public void refreshWnd(AdultInfo _adultInfo)
        {
            _m_myAdultInfo = _adultInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            actionForAllLoadedPageWndSync(_wnd =>
            {
                switch (_wnd)
                {
                    case GGUIPrefabSubWndAdultEngageRequestSendServer server:
                        server.refreshWnd(_m_myAdultInfo);
                        break;
                    case GGUIPrefabSubWndAdultEngageRequestSendGuild guild:
                        guild.refreshWnd(_m_myAdultInfo);
                        break;
                    case GGUIPrefabSubWndAdultEngageRequestSendCustom custom:
                        custom.refreshWnd(_m_myAdultInfo);
                        break;
                }
            });
        }
    }
}