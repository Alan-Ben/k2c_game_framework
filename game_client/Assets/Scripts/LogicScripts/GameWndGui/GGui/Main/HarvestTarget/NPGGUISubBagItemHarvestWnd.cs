
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 背包物品对应的一个资源收集目标对象
    /// </summary>
    public class NPGGUISubBagItemHarvestWnd : _ANPGGUISubNumHarvestWnd
    {
        // 背包物品的 id
        private long _m_bagItemId;

        public NPGGUISubBagItemHarvestWnd(long _bagItemId, Text _txtNum, RectTransform _target, string _transKey, EHarvestType _harvestResType)
            : base(_harvestResType, _txtNum, _target, _transKey)
        {
            _m_bagItemId = _bagItemId;
        }
        public NPGGUISubBagItemHarvestWnd(long _bagItemId, Text _txtNum, string _transKey, EHarvestType _harvestResType)
            : base(_harvestResType, _txtNum, _transKey)
        {
            _m_bagItemId = _bagItemId;
        }

        protected override void _onInit()
        {
            base._onInit();
            
            // 注册玩家资源更新事件
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onUpdateItem);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onUpdateItem);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onUpdateItem);
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            // 注销事件
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onUpdateItem);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onUpdateItem);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onUpdateItem);
        }

        // 刷新物品
        private void _onUpdateItem(params object[] _objs)
        {
            if (!(_objs.SafeGet(0) is BagItem item) || item.itemId != _m_bagItemId)
                return;

            updateNum(GCommon.getItemCount(ENPItemType.BAG_ITEM, _m_bagItemId));
        }
        
        protected override long _getRealCount()
        {
            return GCommon.getItemCount(ENPItemType.BAG_ITEM, _m_bagItemId);
        }
    }
}