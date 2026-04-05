package NPUSServer.NPUSUserMgr.UserComp.Common;

import CommonEnum.EBasicAttrType;
import NPCommon.Util.Delegate.HandlerOne;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;

/****************
 * 玩家触发大臣属性变化的处理对象
 */
public class HeroPropertyChgDealer extends HandlerOne<EBasicAttrType>
{
    private HeroInfo _m_heroInfo;

    public HeroPropertyChgDealer(HeroInfo _heroInfo)
    {
        _m_heroInfo = _heroInfo;
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
                _m_heroInfo.recalHero();
                break;
            default:
                break;
        }
    }
}
