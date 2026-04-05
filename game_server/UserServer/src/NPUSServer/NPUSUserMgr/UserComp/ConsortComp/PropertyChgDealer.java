package NPUSServer.NPUSUserMgr.UserComp.ConsortComp;

import CommonEnum.EBasicAttrType;
import NPCommon.Util.Delegate.HandlerOne;

/****************
 * 玩家触发大臣属性变化的处理对象
 */
public class PropertyChgDealer extends HandlerOne<EBasicAttrType>
{
	//家人数据
    private ConsortInfo _m_ciConsort;

    public PropertyChgDealer(ConsortInfo _consort)
    {
    	_m_ciConsort = _consort;
    }
    
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}

    @Override
    public void handle(EBasicAttrType _bonusPropertyType)
    {
        //根据不同属性进行处理
        switch (_bonusPropertyType) 
        {
            case TALENT:
            case POWER:
            case POWER_PER:
            	//关联伙伴重新计算国力
            	_m_ciConsort.relationHeroReCal();
                break;
            default:
                break;
        }
    }
}
