package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.PlayerSkinErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPPlayerComboTitleType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerComboTitleBO;

import java.util.ArrayList;

/**
 * 组合称号的数据基类
 * @author mj
 *
 */
public abstract class _AComboTitleUnitMgr <A extends _AComboTitleUnit<B>, B extends _IALProtocolStructure> implements _IUserItemBasicDealer
{
	/**
	 * 组合称号类型
	 * @return
	 */
	abstract public ENPPlayerComboTitleType getType();
	/**
	 * 初始化检查，对于满足条件的部件进行自动获取解锁
	 */
	abstract protected void _initCheck();
	/**
	 * 创建组合称号数据
	 * @param _bo
	 * @return
	 */
	abstract protected _AComboTitleUnit<B> _createUnit(PlayerComboTitleBO _bo);
	
	//玩家数据对象
	private NPUSUserData _m_usUserData;
	//组合称号单位数据列表
	protected ArrayList<_AComboTitleUnit<B>> _m_alComboTitleUnitList;
	
	public _AComboTitleUnitMgr(NPUSUserData _userData)
	{
		_m_usUserData = _userData;
		
		_m_alComboTitleUnitList = new ArrayList<>();
	}

	//玩家数据
    public NPUSUserData getUserData() {return _m_usUserData;}
    //US服务器
    public NPUserServer getUSServer() {return _m_usUserData.getUSServer();}
    
    /**
     * 初始化组合称号单位
     * @param _unit
     */
	protected void _initUnit(_AComboTitleUnit<B> _unit)
    {
    	_m_alComboTitleUnitList.add(_unit);
    }

	@Override
	public long getItemCount(long _itemId) 
	{
		return hasItem(_itemId, 1) ? 1 : 0;
	}

	@Override
	public boolean hasItem(long _itemId, long _count) 
	{
		getUserData().lockUser();
		
		try
		{
			return null != lookupItem(_itemId);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public void initGainItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		gainItem(_itemId, _count, false, _context);
	}

	@Override
	public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context) 
	{
		getUserData().lockUser();
		
		try
		{
			//不再重复获得
			if(hasItem(_itemId, _count))
				return;
			
			PlayerComboTitleBO bo = new PlayerComboTitleBO();
			bo.setCid(getUSServer().getBM(), getUserData().getCid());
			bo.setUnitType(getUSServer().getBM(), getType().ordinal());
			bo.setUnitId(getUSServer().getBM(), _itemId);
			bo.insert(getUSServer().getBM());
		
			_AComboTitleUnit<B> unit = _createUnit(bo);
			_m_alComboTitleUnitList.add(unit);
			
			//获取后处理
			unit._onGainUnit();
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	@Override
	public boolean spendItem(long _itemId, long _count, NPPlayerContext _context) 
	{
		return false;
	}
	
	/**
	 * 构造数据对象
	 * @param _unitList
	 */
	public void makeProto(ArrayList<B> _unitList) 
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alComboTitleUnitList.size(); i++)
			{
				_AComboTitleUnit<B> unit = _m_alComboTitleUnitList.get(i);
				if(null == unit)
					continue;
				
				_unitList.add(unit.toProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取对应的组合参数部件
	 * @param _unitId
	 * @return
	 */
	public _AComboTitleUnit<B> lookupItem(long _unitId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alComboTitleUnitList.size(); i++)
			{
				_AComboTitleUnit<B> unit = _m_alComboTitleUnitList.get(i);
				if(null == unit)
					continue;
				
				if(unit.getUnitId() == _unitId)
					return unit;
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 更新称号查看状态
	 * @param _unitId
	 * @return
	 */
	public Result setViewed(long _unitId)
	{
		getUserData().lockUser();
		
		try
		{
			_AComboTitleUnit<B> unit = lookupItem(_unitId);
			if(null == unit)
				return Result.failed(PlayerSkinErr.PLAYER_TITLE_NOT_FOUND.getCode());
			
			unit.setViewed();
			
			return Result.SUCC;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
