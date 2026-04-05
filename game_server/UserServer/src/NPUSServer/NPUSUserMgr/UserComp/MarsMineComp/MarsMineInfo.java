package NPUSServer.NPUSUserMgr.UserComp.MarsMineComp;

import Common.MarsObj.Mars_MineIdx;
import NPCommon.DB.BM.BM;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerMarsMineBO;

public class MarsMineInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	//玩家数据
	private PlayerMarsMineBO _m_bo;

	public MarsMineInfo(NPUSUserData _userData, PlayerMarsMineBO _bo)
	{
		_m_udUserData = _userData;
		_m_bo = _bo;
	}
	
	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}
    
    public PlayerMarsMineBO getBo() {return _m_bo;}
    public long getInstanceId() {return getBo().getInstanceId();}
    public long getRefId() {return getBo().getRefId();}
    public long getStartShowMs() {return getBo().getStartShowMs();}
    public long getEndShowMs() {return getBo().getEndShowMs();}
    public long getPos() {return getBo().getPos();}
    
    /**
     * 构造索引数据
     * @return
     */
    public Mars_MineIdx toIdxProto()
    {
    	Mars_MineIdx proto = new Mars_MineIdx();
    	proto.setId(getInstanceId());
    	proto.setRefId(getRefId());
    	proto.setPos(getPos());
    	proto.setEndShowMs(getEndShowMs());
    	proto.setStartShowMs(getStartShowMs());
    	
    	return proto;
    }
    
    /**
     * 移除数据
     */
    protected void _del() 
    {
		getBo().del(getBM());
	}
}
