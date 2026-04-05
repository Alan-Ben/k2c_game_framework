using NPEnum;

namespace GOE
{
    /// <summary>
    /// 物品详情Node，不带数量
    /// </summary>
    public class NPGAddNodeCommonItemDetail_NoCount : _AGAddNode_SingleWnd
    {
        private ENPItemType _m_eItemType;//物品类型
        private long _m_lItemId;//物品id

        public NPGAddNodeCommonItemDetail_NoCount(ENPItemType _itemType, long _itemId) : base(NPGGUIWndCommonItemDetail_NoCount.instance)
        {
            _m_eItemType = _itemType;
            _m_lItemId = _itemId;
        }

        public override bool NeedRemovePreAutoRemove { get { return false; } }

        protected override void _onWndLoadedDone()
        {
            NPGGUIWndCommonItemDetail_NoCount.instance.showWnd();
            NPGGUIWndCommonItemDetail_NoCount.instance.setShowData(_m_eItemType, _m_lItemId);
        }
    }
}
