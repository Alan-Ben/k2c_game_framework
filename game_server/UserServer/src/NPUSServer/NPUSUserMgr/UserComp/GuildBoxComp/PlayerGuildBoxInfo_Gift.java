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
import USDB.Bo.PlayerGuildBoxGiftBO;

/**
 * 联盟宝箱 - 等级宝箱
 * @author mj
 *
 */
public class PlayerGuildBoxInfo_Gift extends _APlayerGuildBoxInfo
{
	//活跃宝箱数据
	private PlayerGuildBoxGiftBO _m_bo;
	
	public PlayerGuildBoxInfo_Gift(NPUSUserData _userData) 
	{
		super(_userData);
	}
	
	public PlayerGuildBoxGiftBO getBo() {return _m_bo;}
	
	@Override
	protected void _initFromDB(_ICallBackBool _handler)
    {
        //本服玩家，从数据库加载
        getUSServer().getBM().getBM(PlayerGuildBoxGiftBO.class).findOne("cid", getUserData().getCid(), 
        		new _ASelectCallback<PlayerGuildBoxGiftBO>()
        {
            @Override
            public void dealSuc(PlayerGuildBoxGiftBO _bo)
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
	protected void _loadBo(PlayerGuildBoxGiftBO _bo) 
	{
		_m_bo = _bo;
		
		_loadCanGainBoxData(_m_bo.getBoxData());
	}
	
	@Override
	public EGuildBoxType getType() 
	{
		return EGuildBoxType.GUILD_GIFT_BOX;
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
			PlayerGuildBoxGiftBO bo = new PlayerGuildBoxGiftBO();
			bo.setCid(getBM(), getUserData().getCid());
			bo.setBoxData(getBM(), CommonFunc.ByteBfferToBytes(_boxData.makePackage()));
			bo.insert(getBM());
			
			_m_bo = bo;
		}
		else
		{
			final PlayerGuildBoxInfo_Gift info = this;
			
			ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
	        updateValue.addValueObj("box_data", CommonFunc.ByteBfferToBytes(_boxData.makePackage()));
			
			getBM().getBM(PlayerGuildBoxGiftBO.class).update("id", _m_bo.getId(), updateValue, 
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
