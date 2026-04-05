using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendCustomContainer : _AGGUISubWndCommonContainer<GGUIMonoAdultEngageRequestSendCustomContainerItem, GGUIMonoAdultEngageRequestSendCustomContainer, GGUISubWndAdultEngageRequestSendCustomContainerItem>
    {
        private AdultInfo _m_myAdultInfo;
        private List<NPCommonSimplePlayerInfo> _m_simpleInfoList;
        
        
        public GGUISubWndAdultEngageRequestSendCustomContainer(GGUIMonoAdultEngageRequestSendCustomContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        

        protected override GGUISubWndAdultEngageRequestSendCustomContainerItem _createItemWnd(GGUIMonoAdultEngageRequestSendCustomContainerItem _itemMono)
        {
            return new GGUISubWndAdultEngageRequestSendCustomContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndAdultEngageRequestSendCustomContainerItem _itemWnd, int _index)
        {
            if (_m_simpleInfoList == null || _index < 0 || _index >= _m_simpleInfoList.Count)
                return;

            _itemWnd.refreshWnd(_m_myAdultInfo, _m_simpleInfoList[_index]);
        }


        public void refreshWnd(AdultInfo _myAdultInfo, List<NPCommonSimplePlayerInfo> _simpleInfoList)
        {
            _m_myAdultInfo = _myAdultInfo;
            _m_simpleInfoList = _simpleInfoList;
            refreshWnd(_m_simpleInfoList?.Count ?? 0);
            ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, _simpleInfoList == null || _simpleInfoList.Count <= 0);
        }
    }
}