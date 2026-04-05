using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    public class GGUISubWndAdultEngageRequestSendServerContainer : _AGGUISubWndCommonContainer<GGUIMonoAdultEngageRequestSendServerContainerItem, GGUIMonoAdultEngageRequestSendServerContainer, GGUISubWndAdultEngageRequestSendServerContainerItem>
    {
        private AdultInfo _m_myAdultInfo;
        private List<PoolSimpleAdultInfo> _m_simpleInfoList;
        
        
        public GGUISubWndAdultEngageRequestSendServerContainer(GGUIMonoAdultEngageRequestSendServerContainer _containerMono) 
            : base(_containerMono)
        {
            initWnd();
        }
        

        protected override GGUISubWndAdultEngageRequestSendServerContainerItem _createItemWnd(GGUIMonoAdultEngageRequestSendServerContainerItem _itemMono)
        {
            return new GGUISubWndAdultEngageRequestSendServerContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndAdultEngageRequestSendServerContainerItem _itemWnd, int _index)
        {
            if (_m_simpleInfoList == null || _index < 0 || _index >= _m_simpleInfoList.Count)
                return;

            _itemWnd.refreshWnd(_m_myAdultInfo, _m_simpleInfoList[_index]);
        }


        public void refreshWnd(AdultInfo _myAdultInfo, List<PoolSimpleAdultInfo> _simpleInfoList)
        {
            _m_myAdultInfo = _myAdultInfo;
            _m_simpleInfoList = _simpleInfoList;
            _m_simpleInfoList?.Sort((_a, _b) => _b.bonus.CompareTo(_a.bonus));
            refreshWnd(_m_simpleInfoList?.Count ?? 0);
            ALUGUICommon.setGameObjEnable(wnd?.goEmptyShowList, _simpleInfoList == null || _simpleInfoList.Count <= 0);
        }
    }
}