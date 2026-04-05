using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndBusinessBuildingRnDPageTabList : GGUISubWndCommonPageTabList<GGUIMonoBusinessBuildingRnDPageTabType, GGUIMonoBusinessBuildingRnDPageTabList, GGUIMonoBusinessBuildingRnDPageTabListItem>
    {
        private BusinessBuildingInfo _m_buildingInfo;
        
        
        public GGUISubWndBusinessBuildingRnDPageTabList(GGUIMonoBusinessBuildingRnDPageTabList _wnd) 
            : base(_wnd)
        {
            initWnd();
        }


        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoBusinessBuildingRnDPageTabType _type, Transform _wndParent)
        {
            switch (_type)
            {
                case GGUIMonoBusinessBuildingRnDPageTabType.Develop:
                {
                    GGUIPrefabSubWndBusinessBuildingRnDPageDevelop pageWnd = new GGUIPrefabSubWndBusinessBuildingRnDPageDevelop(_wndParent);
                    pageWnd.refreshWnd(_m_buildingInfo);
                    return pageWnd;
                }
                case GGUIMonoBusinessBuildingRnDPageTabType.Product:
                {
                    GGUIPrefabSubWndBusinessBuildingRnDPageProduct pageWnd = new GGUIPrefabSubWndBusinessBuildingRnDPageProduct(_wndParent);
                    pageWnd.refreshWnd(_m_buildingInfo);
                    return pageWnd;
                }
                default:
                    return null;
            }
        }


        public void refreshWnd(BusinessBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            actionForAllLoadedPageWndSync(_wnd =>
            {
                switch (_wnd)
                {
                    case GGUIPrefabSubWndBusinessBuildingRnDPageDevelop server:
                        server.refreshWnd(_m_buildingInfo);
                        break;
                    case GGUIPrefabSubWndBusinessBuildingRnDPageProduct guild:
                        guild.refreshWnd(_m_buildingInfo);
                        break;
                }
            });
        }
    }
}