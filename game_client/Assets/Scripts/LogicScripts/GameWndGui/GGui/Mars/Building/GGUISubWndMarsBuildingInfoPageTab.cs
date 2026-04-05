using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildingInfoPageTab : GGUISubWndCommonPageTabList<GGUIMonoMarsBuildingInfoPageTabType, GGUIMonoMarsBuildingInfoPageTab, GGUIMonoMarsBuildingInfoPageTabItem>
    {
        private MarsBuildingInfo _m_buildingInfo;
        

        public GGUISubWndMarsBuildingInfoPageTab([NotNull] GGUIMonoMarsBuildingInfoPageTab _wnd)
            : base(_wnd)
        {
            initWnd();
        }


        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoMarsBuildingInfoPageTabType _type, Transform _wndParent)
        {
            switch (_type)
            {
                case GGUIMonoMarsBuildingInfoPageTabType.Equipment:
                    GGUIPrefabSubWndMarsBuildingInfoPageEquipment equipmentWnd = new GGUIPrefabSubWndMarsBuildingInfoPageEquipment(_wndParent);
                    equipmentWnd.refreshWnd(_m_buildingInfo);
                    return equipmentWnd;
                case GGUIMonoMarsBuildingInfoPageTabType.Settle:
                    GGUIPrefabSubWndMarsBuildingInfoPageSettle settleWnd = new GGUIPrefabSubWndMarsBuildingInfoPageSettle(_wndParent);
                    settleWnd.refreshWnd(_m_buildingInfo);
                    return settleWnd;
                default:
                    return null;
            }
        }


        public void refreshWnd(MarsBuildingInfo _buildingInfo)
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
                    case GGUIPrefabSubWndMarsBuildingInfoPageEquipment equipmentPage:
                        equipmentPage.refreshWnd(_m_buildingInfo);
                        break;
                    case GGUIPrefabSubWndMarsBuildingInfoPageSettle settlePage:
                        settlePage.refreshWnd(_m_buildingInfo);
                        break;
                }
            });
        }
    }
}