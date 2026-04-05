package NPUSServer.NPUSUserMgr.UserComp.Common;

import CommonEnum.EBonusPropertyType;
import NPCommon.Util.Delegate.HandlerOne;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/****************
 * 玩家触发Union属性变化的处理对象
 */
public class UserUnionBonusPropertyChgDealer extends HandlerOne<EBonusPropertyType>
{
    private NPUSUserData _m_userData;

    public UserUnionBonusPropertyChgDealer(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    @Override
    public void handle(EBonusPropertyType _bonusPropertyType)
    {
        //根据不同属性进行处理
        switch (_bonusPropertyType) {
            //实力变化，所有大臣需要重新处理
            case TALENT:
            case POWER:
            case POWER_PER:
                _m_userData.getHeroComponent().recalAllHero();
                break;

            //收益加成需要所有建筑重新处理
            case BONUS:
            case BUILDING_PROFIT_ADD_PER:
            case BUILDING_EMPLOYEE_PROFIT_ADD:
                _m_userData.getBuildingComponent().recalAllBuilding();
                break;

            //收益加成需要所有建筑重新处理
            case INTIMACY:
                _m_userData.getConsortComponent().recalAllConsortIntimacy();
                break;
            case CHARM:
                _m_userData.getConsortComponent().recalAllConsortCharm();
                break;

            //太空寻宝-奇物产出加成
            case TREASURE_HUNT_TREASURE_OUTPUT_ADD:
                _m_userData.getTreasureHuntComponent().getTreasureMgr().checkAllTreasureOutputChange();
                break;

            default:
                break;
        }
    }
}
