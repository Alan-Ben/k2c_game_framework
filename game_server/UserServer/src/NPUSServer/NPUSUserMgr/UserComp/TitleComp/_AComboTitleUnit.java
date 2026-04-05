package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import ALBasicProtocolPack._IALProtocolStructure;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerComboTitleBO;

public abstract class _AComboTitleUnit <T extends _IALProtocolStructure>
{
	/**
	 * 获取数据后的处理
	 */
	abstract protected void _onGainUnit();
	/**
	 * 构造数据
	 * @return
	 */
	abstract public T toProto();
	
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	//bo数据
	private PlayerComboTitleBO _m_bo;
	
	public _AComboTitleUnit(NPUSUserData _userData, PlayerComboTitleBO _bo)
	{
		_m_usUserData = _userData;
		
		_m_bo = _bo;
	}

	//玩家数据
    public NPUSUserData getUserData() {return _m_usUserData;}
    //US服务器
    public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
	
    //称号数据
    public PlayerComboTitleBO getBo() {return _m_bo;}
    //组件称号ID
	public long getUnitId() {return _m_bo.getUnitId();}
	//是否查看
	public boolean isViewed() {return _m_bo.getViewed();}
	
	public void setViewed()
	{
		_m_bo.setViewed(getUSServer().getBM(), true);
		_m_bo.saveAllMarked(getUSServer().getBM());
	}
}
