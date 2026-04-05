using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 打捞收获物item列表容器
    /// </summary>
    public class GGUIWndTreasureHuntCaptureHarvestItemContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntCaptureHarvestItem, GGUIMonoTreasureHuntCaptureHarvestItemContainer, GGUIWndTreasureHuntCaptureHarvestItem>
    {
        private List<_ATreasureHuntCaptureHarvestItemInfo> _m_lHarvestItemInfoList;


        public GGUIWndTreasureHuntCaptureHarvestItemContainer(GGUIMonoTreasureHuntCaptureHarvestItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }


        protected override GGUIWndTreasureHuntCaptureHarvestItem _createItemWnd(GGUIMonoTreasureHuntCaptureHarvestItem _itemMono)
        {
            GGUIWndTreasureHuntCaptureHarvestItem itemWnd = new GGUIWndTreasureHuntCaptureHarvestItem(_itemMono);

            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndTreasureHuntCaptureHarvestItem _itemWnd, int _index)
        {
            if (_m_lHarvestItemInfoList == null || _index < 0 || _index >= _m_lHarvestItemInfoList.Count)
                return;

            _ATreasureHuntCaptureHarvestItemInfo harvestItemInfo = _m_lHarvestItemInfoList.SafeGet(_index);
            if (harvestItemInfo != null)
            {
                _itemWnd.setData(harvestItemInfo);
            }
        }


        public void setData(List<_ATreasureHuntCaptureHarvestItemInfo> _harvestItemInfoList)
        {
            _m_lHarvestItemInfoList = _harvestItemInfoList;
            _m_lHarvestItemInfoList?.Sort((_a, _b) =>
            {
                if(_b == null) return -1;
                if(_a == null) return 1;
                if(ReferenceEquals(_a, _b)) return 0;
                return _a.compareTo(_b);
            });

            int itemCount = _m_lHarvestItemInfoList?.Count ?? 0;
            
            // 控制无item时的显示
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }

            refreshWnd(itemCount);
        }
    }
}