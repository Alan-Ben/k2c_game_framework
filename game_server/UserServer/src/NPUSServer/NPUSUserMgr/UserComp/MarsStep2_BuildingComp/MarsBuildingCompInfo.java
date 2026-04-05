package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerMarsBuildingCompBO;

public class MarsBuildingCompInfo 
{
	//玩家数据对象
	private NPUSUserData _m_udUserData;
	
	//bo数据
	private PlayerMarsBuildingCompBO _m_bo;
	
	public MarsBuildingCompInfo(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
	}

	public NPUSUserData getUserData() {return _m_udUserData;}
    public NPUserServer getUSServer() {return _m_udUserData.getUSServer();}
    public long getCid() {return _m_udUserData.getCid();}
    public BM getBM() {return _m_udUserData.getUSServer().getBM();}
    
    public PlayerMarsBuildingCompBO getBo() {return _m_bo;}
    public long getExtMoodIndex() {return getBo().getExtMoodIndex();}
    
    /**
     * 数据库加载
     * @param _handler
     */
	protected void _initFromDB(_ICallBackBool _handler)
    {
		getUSServer().getBM().getBM(PlayerMarsBuildingCompBO.class).findOne("cid", getCid(), 
				new _ASelectCallback<PlayerMarsBuildingCompBO>() 
		{
			@Override
			public void dealSuc(PlayerMarsBuildingCompBO _bo) 
			{	
				_m_bo = _bo;
				
				_handler.onRunOver(true);
			}

			@Override
			public void dealFail() 
			{
				if(getHasErr())
				{
					_handler.onRunOver(false);
					return;
				}
				
				PlayerMarsBuildingCompBO bo = new PlayerMarsBuildingCompBO();
				bo.setCid(getBM(), getCid());
				bo.insert(getBM());
				
				_m_bo = bo;
				
				_handler.onRunOver(true);
			}
		});
    }
	
	/**
	 * 设置心情指数
	 * @param _value
	 * @param _context
	 */
	public void setMoodIndex(long _value, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(_value == getBo().getExtMoodIndex())
				return;
			
			getBo().saveExtMoodIndex(getBM(), _value);
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_039_MarsBuildingOp.make_061_OnExtMoodIndexChg(_value));
			
			//发起计算快乐值
			getUserData().getMarsBuildingComponent().doLazyCalHappyIndex();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 调整心情指数
	 * @param _value
	 * @param _context
	 */
	public void chgMoodIndex(long _value, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			long newValue = getBo().getExtMoodIndex() + _value;
			
			setMoodIndex(newValue, _context);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
