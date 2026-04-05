package NPUSServer.NPUSUserMgr.UserComp.DinnerComp;

import Common.DinnerEnum.EDinnerPermitType;
import Common.DinnerObj.Dinner_Permit;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Dinner.RefDinnerPermit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerDinnerPermitBO;

/**
 * 玩家宴会凭证数据
 * @author mj
 *
 */
public class DinnerPermitInfo 
{
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	//凭证数据Bo
	private PlayerDinnerPermitBO _m_bo;
	//凭证配置数据
	private RefDinnerPermit _m_ref;
	
	public DinnerPermitInfo(NPUSUserData _userData, PlayerDinnerPermitBO _bo)
	{
		_m_usUserData = _userData;
		_m_bo = _bo;
		
		_m_ref = RefDinnerPermit.getMgr().get(_m_bo.getPermitType());
		if(null == _m_ref)
		{
			USLog.error(_m_usUserData.getUSServer(), "player:{} dinner permit:{} init ref fail, not find ref.", _m_usUserData.getCid(), _m_bo.getPermitType());
		}
	}
	public DinnerPermitInfo(NPUSUserData _userData, PlayerDinnerPermitBO _bo, RefDinnerPermit _ref)
	{
		_m_usUserData = _userData;
		_m_bo = _bo;
		_m_ref = _ref;
	}

	//玩家数据
    public NPUSUserData getUserData() {return _m_usUserData;}
    //US服务器
    public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}

    //凭证数据
    public PlayerDinnerPermitBO getBo() {return _m_bo;}
    //凭证实例ID
    public long getId() {return _m_bo.getId();}
    //凭证类型
    public EDinnerPermitType getType() {return EDinnerPermitType.EDinnerPermitType_FromInt(_m_bo.getPermitType());}
    //类型ID
    public long getTypeId() {return _m_bo.getTypeId();}
    //凭证过期时间
    public int getExpiredTs() {return _m_bo.getExpiredTs();}
    
    //配置数据
    public RefDinnerPermit getRef() {return _m_ref;}

    /**
     * 构造凭证数据
     * @return
     */
    public Dinner_Permit toProto()
    {
    	Dinner_Permit proto = new Dinner_Permit();
    	proto.setId(getId());
    	proto.setPermitType(getType());
    	proto.setTypeId(getTypeId());
    	proto.setExpiredTs(getExpiredTs());
    	
    	return proto;
    }
    
    /**
     * 检查是否过期
     * @return
     */
    public boolean isExpired()
    {
    	return CommonFunc.getNowTimeSec() > getExpiredTs();
    }
    
    /**
     * 销毁数据
     */
    public void discard()
    {
    	_m_bo.del(getUSServer().getBM());
    }
}
