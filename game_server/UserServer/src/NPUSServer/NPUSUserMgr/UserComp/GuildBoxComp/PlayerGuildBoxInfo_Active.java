package NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.GuildEnum.EGuildBoxType;
import Common.ServerObj.ServerObj_GuildBoxList;
import NPCommon.DB._ASelectCallback;
import NPCommon.DB._AUpdateCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerGuildBoxActivityBO;

/**
 * 联盟宝箱 - 活跃宝箱
 * @author mj
 *
 */
public class PlayerGuildBoxInfo_Active extends _APlayerGuildBoxInfo
{
	//活跃宝箱数据
	private PlayerGuildBoxActivityBO _m_bo;
	
	public PlayerGuildBoxInfo_Active(NPUSUserData _userData) 
	{
		super(_userData);
	}
	
	public PlayerGuildBoxActivityBO getBo() {return _m_bo;}
	
	@Override
	protected void _initFromDB(_ICallBackBool _handler)
    {
        //本服玩家，从数据库加载
        getUSServer().getBM().getBM(PlayerGuildBoxActivityBO.class).findOne("cid", getUserData().getCid(), 
        		new _ASelectCallback<PlayerGuildBoxActivityBO>()
        {
            @Override
            public void dealSuc(PlayerGuildBoxActivityBO _bo)
            {
            	_loadBo(_bo);
            	
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
            	
                _handler.onRunOver(true);
            }
        });
    }

	/**
	 * 加载bo数据，加载可领取宝箱列表数据
	 * @param _bo
	 */
	protected void _loadBo(PlayerGuildBoxActivityBO _bo) 
	{
		_m_bo = _bo;
		
		_loadCanGainBoxData(_m_bo.getBoxData());
	}
	
	@Override
	public EGuildBoxType getType() 
	{
		return EGuildBoxType.GUILD_ACTIVE_BOX;
	}

	@Override
	public int getCanLimit() 
	{
		return -1;
	}

	@Override
	public void afterGained(int _count, NPPlayerContext _context) 
	{
	}

	@Override
	protected void _saveBoxData(long _dataSerial, ServerObj_GuildBoxList _boxData) 
	{
		if(null == _m_bo)
		{
			PlayerGuildBoxActivityBO bo = new PlayerGuildBoxActivityBO();
			bo.setCid(getBM(), getUserData().getCid());
			bo.setBoxData(getBM(), CommonFunc.ByteBfferToBytes(_boxData.makePackage()));
			bo.insert(getBM());
			
			_m_bo = bo;
		}
		else
		{
			final PlayerGuildBoxInfo_Active info = this;
			
			ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
	        updateValue.addValueObj("box_data", CommonFunc.ByteBfferToBytes(_boxData.makePackage()));
			
			getBM().getBM(PlayerGuildBoxActivityBO.class).update("id", _m_bo.getId(), updateValue, 
					new _AUpdateCallback<Integer>() 
			{
				@Override
				public void dealSuc(Integer _dealCount) 
				{
				}

				@Override
				public void dealFail() 
				{
					//1秒后重新发起存储
					ALSynTaskManager.getInstance().regTask(new GuildBoxSaveTask(_dataSerial, info), 1000);
				}
			});
		}
	}

	@Override
	protected void _del() 
	{
		if(null == _m_bo)
			return;
		
		_m_bo.del(getBM());
		_m_bo = null;
	}
}
