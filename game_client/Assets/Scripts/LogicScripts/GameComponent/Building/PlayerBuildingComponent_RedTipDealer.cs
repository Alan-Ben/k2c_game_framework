using JetBrains.Annotations;

namespace GOE
{
    public partial class PlayerBuildingComponent
    {
        public class RedTipDealer
        {
            [NotNull] private readonly PlayerBuildingComponent _m_comp;

            private _ARedTipNode _m_buildableNode;


            public RedTipDealer([NotNull] PlayerBuildingComponent _comp)
            {
                _m_comp = _comp;
            }


            public void init()
            {
                _m_buildableNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_BUILDING_BUILD);

                refreshBuildableRedTip();

                WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, refreshBuildableRedTip);
            }
            public void clear()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, refreshBuildableRedTip);

                _m_buildableNode = null;
            }


            public void refreshBuildableRedTip()
            {
                if (_m_buildableNode == null)
                    return;

                bool hasBuildable = false;
                foreach (BuildingInfo buildingInfo in _m_comp._m_buildingList)
                {
                    if (!buildingInfo.isBuilt &&
                        (buildingInfo.baseRef.build_order < 0 || buildingInfo == _m_comp.nextOrderedBuilding) && 
                        buildingInfo.baseRef.build_condition.IsEnable(null))
                    {
                        hasBuildable = true;
                        break;
                    }
                }

                _m_buildableNode.setCount(hasBuildable ? 1 : 0);
            }
        }
    }
}
