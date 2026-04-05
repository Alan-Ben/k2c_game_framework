package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;

public abstract class _AMarsBuildingFunc 
{
	//建筑数据对象
	private MarsBuildingInfo _m_biBuildingInfo;
	
	public _AMarsBuildingFunc(MarsBuildingInfo _info)
	{
		_m_biBuildingInfo = _info;
	}
	
	public MarsBuildingInfo getBuildingInfo() {return _m_biBuildingInfo;}
	
	public NPUSUserData getUserData() {return getBuildingInfo().getUserData();}
    public NPUserServer getUSServer() {return getUserData().getUSServer();}
    public long getCid() {return getUserData().getCid();}
    public BM getBM() {return getUserData().getUSServer().getBM();}

    public long getBuildingId() {return getBuildingInfo().getBuildingId();}
    public int getBuildingLvl() {return getBuildingInfo().getBuildingLvl();}

	protected void _lock() {getUserData().lockUser();}
	protected void _unlock() {getUserData().unlockUser();}
	
	/**
	 * 对应建筑数据加载完成后执行
	 */
	abstract protected void _onBuildingLoad();
	/**
	 * 组件加载完成后执行
	 */
    abstract protected	void _onInited();
	/**
	 * 建筑子数据检查升级
	 * @param _tarLvl
	 * @return
	 */
	abstract protected Result _checkUpdateSub(int _tarLvl);
	/**
	 * 建筑子数据升级
	 * @param _context
	 */
	abstract protected void _doneSub(NPPlayerContext _context);
	/**
	 * 输出数据
	 */
	abstract public String toString();
}
