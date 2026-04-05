package NPUSServer.NPUSUserMgr.UserComp.DinnerComp;

import Common.DinnerObj.Dinner_JoinerLogList;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerDinnerEachLogBO;

public class DinnerLastEachLog 
{
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	//凭证数据Bo
	private PlayerDinnerEachLogBO _m_bo;
	
	public DinnerLastEachLog(NPUSUserData _userData, PlayerDinnerEachLogBO _bo)
	{
		_m_usUserData = _userData;
		_m_bo = _bo;
	}

	//玩家数据
    public NPUSUserData getUserData() {return _m_usUserData;}
    //US服务器
    public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
    
    //交互数据bo
    public PlayerDinnerEachLogBO getBo() {return _m_bo;}
    //目标玩家CID
    public long getTargetCid() {return _m_bo.getTargetCid();}
    //赴宴次数
    public int getJoinedCount() {return _m_bo.getJoinedCount();}
    //被赴宴次数
    public int getBeJoinedCount() {return _m_bo.getBeJoinedCount();}
    //最后一次更新时间
    public int getLastTs() {return _m_bo.getLastTs();}
    
    /**
     * 构造数据对象
     * @return
     */
    public Dinner_JoinerLogList toProto()
    {
    	Dinner_JoinerLogList proto = new Dinner_JoinerLogList();
    	proto.setCid(getTargetCid());
    	proto.setJoinedCount(getJoinedCount());
    	proto.setBeJoinedCount(getBeJoinedCount());
    	
    	return proto;
    }
    
    /**
     * 超时检查，设置30天
     * @return
     */
    public boolean isExpired()
    {
    	return CommonFunc.getNowTimeSec() > (getLastTs() + RefGeneral.Ref().dinner_deleting_contact_record_deadline * 86400);
    }
    
    /**
     * 设置赴宴次数
     * @param _count
     * @param _context
     */
    public void setJoinedCount(int _count, NPPlayerContext _context)
    {
    	_m_bo.setJoinedCount(getUSServer().getBM(), _count);
    	_m_bo.setLastTs(getUSServer().getBM(), CommonFunc.getNowTimeSec());
    	_m_bo.saveAll(getUSServer().getBM());
    }
    
    /**
     * 增加赴宴次数+1
     * @param _context
     */
    public void incrJoinedCount(NPPlayerContext _context)
    {
    	setJoinedCount(getJoinedCount() + 1, _context);
    }
    
    /**
     * 设置被赴宴次数
     * @param _count
     * @param _context
     */
    public void setBeJoinedCount(int _count, NPPlayerContext _context)
    {
    	_m_bo.setBeJoinedCount(getUSServer().getBM(), _count);
    	_m_bo.setLastTs(getUSServer().getBM(), CommonFunc.getNowTimeSec());
    	_m_bo.saveAll(getUSServer().getBM());
    }
    
    /**
     * 增加被赴宴次数
     * @param _context
     */
    public void incrBeJoinedCount(NPPlayerContext _context)
    {
    	setBeJoinedCount(getBeJoinedCount() + 1, _context);
    }

    /**
     * 销毁数据
     */
    public void discard()
    {
    	_m_bo.del(getUSServer().getBM());
    }
}
