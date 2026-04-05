using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerMuseumComponent
    {
        public class RedTipDealer
        {
            [NotNull] private readonly PlayerMuseumComponent _m_comp;
            
            
            private _ARedTipNode _m_itemUpgradeOrActiveNode;
            
            
            public RedTipDealer([NotNull] PlayerMuseumComponent _comp)
            {
                _m_comp = _comp;
            }
            
            
            public void init()
            {
                // Initialize red tip nodes
                _m_itemUpgradeOrActiveNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MUSEUM_ITEM_UPGRADE_OR_ACTIVE);
                // Initial refresh
                refreshItemRedTip();
                
                WinMsg.RegisterMsgAct(WinMsgType.ON_INN_GET_SETTLE_REWARD, refreshItemRedTip);
            }
            public void clear()
            {
                // Clear red tip nodes
                _m_itemUpgradeOrActiveNode = null;
                
                WinMsg.UnregisterMsgAct(WinMsgType.ON_INN_GET_SETTLE_REWARD, refreshItemRedTip);
            }
            
            
            public void refreshItemRedTip()
            {
                if (_m_itemUpgradeOrActiveNode == null)
                    return;
                
                bool hasUpgradableOrActivableItem = false;
                foreach (MuseumItemInfo itemInfo in _m_comp._m_itemList)
                {
                    if (!itemInfo.isObtain)
                        continue;
                    
                    if (!itemInfo.isActive || 
                        itemInfo.levelProperty != null && itemInfo.levelProperty.canLevelUp())
                    {
                        hasUpgradableOrActivableItem = true;
                        break;
                    }
                }
                
                _m_itemUpgradeOrActiveNode.setCount(hasUpgradableOrActivableItem ? 1 : 0);
            }
        }
    }
}