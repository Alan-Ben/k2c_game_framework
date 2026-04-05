package NPUSServer.NPUSUserMgr.UserComp.Common;

import CommonEnum.EBonusPropertyType;
import NPCommon.Util.Delegate.HandlerOne;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;

/****************
 * 玩家触发大臣属性变化的处理对象
 */
public class BuildingPropertyChgDealer extends HandlerOne<EBonusPropertyType>
{
    private BuildingInfo _m_buildingInfo;

    public BuildingPropertyChgDealer(BuildingInfo _buildingInfo)
    {
        _m_buildingInfo = _buildingInfo;
    }

    @Override
    public void handle(EBonusPropertyType _bonusPropertyType)
    {
        //根据不同属性进行处理
        switch (_bonusPropertyType) {
            //实力变化，大臣需要重新处理
            case BUILDING_PROFIT_ADD_PER:
                _m_buildingInfo.calOutputSpeed();
                break;
            case BONUS:
                _m_buildingInfo.calOutputSpeed();
                break;
            default:
                break;
        }
    }
}
