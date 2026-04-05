package NPUSServer.NPUSUserMgr.UserComp.Common;

import CommonEnum.EBasicAttrType;
import NPCommon.Util.Delegate.HandlerOne;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.SuitMgr.HeroSuitInfo;

/****************
 * 玩家触发大臣套件属性变化的处理对象
 */
public class HeroSuitPropertyChgDealer extends HandlerOne<EBasicAttrType>
{
    private HeroSuitInfo _m_heroSuitInfo;

    public HeroSuitPropertyChgDealer(HeroSuitInfo _suitInfo)
    {
        _m_heroSuitInfo = _suitInfo;
    }

    @Override
    public void handle(EBasicAttrType _bonusPropertyType)
    {
        //根据不同属性进行处理
        switch (_bonusPropertyType) {
            //实力变化，大臣需要重新处理
            case TALENT:
            case POWER:
            case POWER_PER:
                _m_heroSuitInfo.getSuitMgr().getComp().recalAllSuitHero(_m_heroSuitInfo.getSuitRef().id);
                break;
            default:
                break;
        }
    }
}
