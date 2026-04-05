using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndTreasureHuntSpecimenConvertItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoTreasureHuntSpecimenConvertItem, GGUIMonoTreasureHuntSpecimenConvertItemGrid, GGUIWndTreasureHuntSpecimenConvertItem>
    {
        private List<TreasureHuntCommonOreInfo> _m_lConvertOreList;//转化的矿石列表
        
        public GGUIWndTreasureHuntSpecimenConvertItemGrid(GGUIMonoTreasureHuntSpecimenConvertItemGrid _gridMono) : base(_gridMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_lConvertOreList?.Clear();
            _m_lConvertOreList = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_lConvertOreList?.Clear();
        }

        protected override GGUIWndTreasureHuntSpecimenConvertItem _createItemWnd(GGUIMonoTreasureHuntSpecimenConvertItem _itemMono)
        {
            GGUIWndTreasureHuntSpecimenConvertItem itemWnd = new GGUIWndTreasureHuntSpecimenConvertItem(_itemMono);
            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndTreasureHuntSpecimenConvertItem _itemWnd, int _itemIdx)
        {
            if(_itemWnd == null || _m_lConvertOreList == null || _itemIdx < 0 || _itemIdx >= _m_lConvertOreList.Count)
                return;
            
            TreasureHuntCommonOreInfo oreInfo = _m_lConvertOreList.SafeGet(_itemIdx);
            _itemWnd.setData(oreInfo);
        }

        /// <summary>
        /// 设置转化矿石列表数据
        /// </summary>
        /// <param name="_convertOreList"></param>
        public void setData(List<TreasureHuntCommonOreInfo> _convertOreList)
        {
            _m_lConvertOreList = _convertOreList;

            int itemCount = _m_lConvertOreList?.Count ?? 0;
            setItemCount(itemCount);
        }
    }
}