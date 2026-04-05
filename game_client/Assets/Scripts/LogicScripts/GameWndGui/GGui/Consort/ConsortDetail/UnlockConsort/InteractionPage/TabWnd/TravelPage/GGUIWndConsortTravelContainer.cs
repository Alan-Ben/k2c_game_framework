using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndConsortTravelContainer : _AGGUISubWndCommonContainer<GGUIMonoConsortTravelContainerItem, GGUIMonoConsortTravelContainer, GGUIWndConsortTravelContainerItem>
    {
        private GGottenConsortInfo _m_iConsortInfo;//妃子信息
        
        public GGUIWndConsortTravelContainer(GGUIMonoConsortTravelContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortTravelContainerItem _createItemWnd(GGUIMonoConsortTravelContainerItem _itemMono)
        {
            GGUIWndConsortTravelContainerItem itemWnd = new GGUIWndConsortTravelContainerItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndConsortTravelContainerItem _itemWnd, int _index)
        {
            List<ConsortTravelRefObj> travelRefObjList = GRefdataCoreMgr.instance.consortTravelRefCore.refList;
            if(travelRefObjList == null || _index < 0 || _index >= travelRefObjList.Count)
                return;
            
            _itemWnd.setData(_m_iConsortInfo, travelRefObjList[_index]);
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();

            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_TRAVEL_ITEM_BY_INDEX, _onSimulateClickTravelItemByIndex);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_TRAVEL_ITEM_BY_INDEX, _onSimulateClickTravelItemByIndex);

            base._onHideWnd();
        }

        public void setData(GGottenConsortInfo _consortInfo)
        {
            _m_iConsortInfo = _consortInfo;
            
            refreshWnd(GRefdataCoreMgr.instance.consortTravelRefCore.refList.Count);
        }

        /// <summary>
        /// 模拟点击出游列表 item
        /// 参数：_objs[0] 为 long 类型 item下标（从0开始）
        /// </summary>
        private void _onSimulateClickTravelItemByIndex(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || !(_objs[0] is long _targetIndex))
                return;

            if (_targetIndex < 0 || _targetIndex > int.MaxValue)
                return;

            GGUIWndConsortTravelContainerItem targetItem = getItem((int)_targetIndex);
            if (targetItem == null)
                return;

            targetItem.simulateClickTravelBtn();
        }
    }
}