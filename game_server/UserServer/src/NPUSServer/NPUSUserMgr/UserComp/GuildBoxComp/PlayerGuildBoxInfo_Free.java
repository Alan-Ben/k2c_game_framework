package NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.GuildEnum.EGuildBoxType;
import Common.ServerObj.ServerObj_GuildBoxList;
import NPCommon.DB._ASelectCallback;
import NPCommon.DB._AUpdateCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerGuildBoxFreeBO;

/**
 * 联盟宝箱 - 免费宝箱
 * @author mj
 *
 */
public class PlayerGuildBoxInfo_Free extends _APlayerGuildBoxInfo
{
	//活跃宝箱数据
	private PlayerGuildBoxFreeBO _m_bo;
	
	public PlayerGuildBoxInfo_Free(NPUSUserData _userData) 
	{
		super(_userData);
	}
	
	public PlayerGuildBoxFreeBO getBo() {return _m_bo;}
	
	@Override
	protected void _initFromDB(_ICallBackBool _handler)
    {
        //本服玩家，从数据库加载
        getUSServer().getBM().getBM(PlayerGuildBoxFreeBO.class).findOne("cid", getUserData().getCid(), 
        		new _ASelectCallback<PlayerGuildBoxFreeBO>()
        {
            @Override
            public void dealSuc(PlayerGuildBoxFreeBO _bo)
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
	protected void _loadBo(PlayerGuildBoxFreeBO _bo) 
	{
		_m_bo = _bo;
		
		_loadCanGainBoxData(_m_bo.getBoxData());
	}
	
	@Override
	public EGuildBoxType getType() 
	{
		return EGuildBoxType.GUILD_FREE_BOX;
	}

	@Override
	public int getCanLimit() 
	{
		return (int) getUserData().getFixedCdComponent().getItemCount(RefGeneral.Ref().guild_box_claim_fixed_cd_id);
	}

	@Override
	public void afterGained(int _count, NPPlayerContext _context) 
	{
		if(_count <= 0)
			return;

		long costCount = _count;
		//需要扣除对应的fixed_cd，因为添加宝箱时已经检查了cd的数量，理论上cd数值不会少于宝箱数量
		long cdId = RefGeneral.Ref().guild_box_claim_fixed_cd_id;
		long cdCount = getUserData().getFixedCdComponent().getItemCount(cdId);
		//如果出现了该情况，日志输出错误
		if(cdCount < _count)
		{
			USLog.error(getUSServer(), "player:{} guild box reward:{} > cd:{} count:{}, need check."
					, getUserData().getCid(), _count, cdId, cdCount);
			
			costCount = cdCount;
		}
		
		getUserData().getFixedCdComponent().spendItem(cdId, costCount, _context);
	}

	@Override
	protected void _saveBoxData(long _dataSerial, ServerObj_GuildBoxList _boxData) 
	{
		if(null == _m_bo)
		{
			PlayerGuildBoxFreeBO bo = new PlayerGuildBoxFreeBO();
			bo.setCid(getBM(), getUserData().getCid());
			bo.setBoxData(getBM(), CommonFunc.ByteBfferToBytes(_boxData.makePackage()));
			bo.insert(getBM());
			
			_m_bo = bo;
		}
		else
		{
			final PlayerGuildBoxInfo_Free info = this;
			
			ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
	        updateValue.addValueObj("box_data", CommonFunc.ByteBfferToBytes(_boxData.makePackage()));
			
			getBM().getBM(PlayerGuildBoxFreeBO.class).update("id", _m_bo.getId(), updateValue, 
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
