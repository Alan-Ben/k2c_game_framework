using NPEnum;

namespace GOE
{
    /// <summary>
    /// 物品详情Node
    /// </summary>
    public class NPGAddNodeCommonItemDetail : _AGAddNode_SingleWnd
    {
        private ENPItemType _m_eItemType;//物品类型
        private long _m_lItemId;//物品id

        public NPGAddNodeCommonItemDetail(ENPItemType _itemType, long _itemId) : base(NPGGUIWndCommonItemDetail.instance)
        {
            _m_eItemType = _itemType;
            _m_lItemId = _itemId;
        }

        public override bool NeedRemovePreAutoRemove { get { return false; } }

        protected override void _onWndLoadedDone()
        {
            NPGGUIWndCommonItemDetail.instance.showWnd();
            NPGGUIWndCommonItemDetail.instance.setShowData(_m_eItemType, _m_lItemId);
        }
    }
}
