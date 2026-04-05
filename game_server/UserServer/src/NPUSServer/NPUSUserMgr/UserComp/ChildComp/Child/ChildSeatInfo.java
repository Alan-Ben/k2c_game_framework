package NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child;

import Common.ChildObj.Child_SeatInfo;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Child.RefChildSeat;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerChildSeatBO;

public class ChildSeatInfo 
{
	//子嗣组件对象
	private ChildComponent _m_comp;
	
	//配置数据
	private RefChildSeat _m_refChildSeat;
	
	//标记是否被使用中
	private boolean _m_bUsing;
	
	//训练位数据
	private int _m_iMaxEnergy;//当前脑力值上限
	private int _m_iEnergy;//当前脑力值
	private long _m_lLastCalMs;//上次计算的时间戳（毫秒）
    private long _m_lFullGetNextRemainMs;//脑力值已满时，获得下一点脑力值所需时间
	
	//bo数据
	private PlayerChildSeatBO _m_boChildSeat;
	
	//是否解锁
	private boolean _m_bUnlock;
	
	public ChildSeatInfo(ChildComponent _comp, RefChildSeat _ref)
	{
		_m_comp = _comp;
		
		_m_refChildSeat = _ref;
	}
	
	//玩家数据
	public ChildComponent getComp() {return _m_comp;}
	public NPUSUserData getUserData() {return _m_comp.getUserData();}
	public NPUserServer getUSServer() {return _m_comp.getUserData().getUSServer();}
	
	//配置数据
	public RefChildSeat getRef() {return _m_refChildSeat;}
	public long getSeatId() {return _m_refChildSeat.seat_id;}
	
	//训练位数据
	public int getEnergy() {return _m_iEnergy;}
	//最大脑力值
	public int getMaxEnergy() {return _m_iMaxEnergy;}
	//回复持续时间
	public int getDurationMs() {return RefGeneral.Ref().child_seat_recover_S * 1000;}
	//每次恢复脑力值（每次回复1点，代码固定）
	public int getAddEnergyPerTime() {return 1;}
	
	//是否使用
	public boolean isUsing() {return _m_bUsing;}
	public void setUsing() {_m_bUsing = true;}
	public void unsetUsing() {_m_bUsing = false;}
	
	protected void _onInited()
	{
		//更新当前最大精力值
		_m_iMaxEnergy = (int) getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.CHILD_SEAT_ENERGY_NUM);
		
		//刷新训练位回复数据
		_apply();
	}
	
	//训练位数据
	public int calAndGetEnergy() 
	{
		getUserData().lockUser();
		
		try
		{
			_apply();
			
			return _m_iEnergy;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	public long getLastCalMs() {return _m_lLastCalMs;}
	
	//是否解锁
	public boolean isUnlock() {return _m_bUnlock;}
	
	//初始化bo数据
	protected void _setBo(PlayerChildSeatBO _bo) 
	{
		_m_boChildSeat = _bo;
		
		_m_iEnergy = _bo.getEnergy();
		_m_lLastCalMs = _bo.getLastCalMs();
		_m_lFullGetNextRemainMs = _bo.getFullGetNextRemainMs();
	}
	
	/**
	 * 检查是否可以解锁
	 * @return
	 */
	public boolean checkUnlock()
	{
		//尚未解锁需要进行检查
		if(!_m_bUnlock && NPPlayerConditionDealerMgr.IsEnable(_m_refChildSeat.seat_unlock_condition, getUserData(), null))
		{
			_m_bUnlock = true;
		}
		
		return _m_bUnlock;
	}
	
	/**
	 * 重新申请数据（根据时长计算）
	 */
	private void _apply()
	{
        int maxEnergy = getMaxEnergy();
        int curEnergy = _m_iEnergy;

        //已经达到上限
        if (curEnergy >= maxEnergy)
        {
            _m_lLastCalMs = CommonFunc.getNowTimeMS();
            _save();
            return;
        }

        long lastCalMs = getLastCalMs();
        long spanMs = CommonFunc.getNowTimeMS() - lastCalMs;
        long durationMs = getDurationMs();
        if (durationMs > 0 && spanMs >= durationMs)
        {
            int addCountPerTime = getAddEnergyPerTime();
            int times = (int) (spanMs / durationMs);
            int finalEnergy = times * addCountPerTime + curEnergy;

            //超过上限但是无法超过，只能取上限值
            if (finalEnergy >= maxEnergy)
            {
            	finalEnergy = maxEnergy;
            	
            	_m_lLastCalMs = CommonFunc.getNowTimeMS();
            } 
            else
            {
                _m_lLastCalMs = lastCalMs + ( times * durationMs );
            }
            _m_iEnergy = finalEnergy;
            
            //保存数据
            _save();
        }
	}
	
	/**
	 * 修改脑力值上限
	 * @param _curMaxEnergy
	 * @param _context
	 */
	public void updateMaxEnergy(int _curMaxEnergy, NPPlayerContext _context) 
	{
		getUserData().lockUser();
		
		try
		{
			//刷新之前的回复数据
			_apply();
			
			//计算应该增加的新数据
			int preMaxEnergy = _m_iMaxEnergy;
			_m_iMaxEnergy = _curMaxEnergy;
			
			//上限增加后，需要补充额外脑力值
			int addValue = _m_iMaxEnergy - preMaxEnergy;
			if(addValue > 0)
			{
				int finalEnergy = _m_iEnergy + addValue;
		        if (finalEnergy >= _m_iMaxEnergy)
		        {
		            //如果玩家使用道具恢复CD到满，则需要记下距离上次计算已经过了多少时间，在下次消耗到低于最大值的时候恢复这个时间
		            long nowTimeMS = CommonFunc.getNowTimeMS();
		            _m_lFullGetNextRemainMs = nowTimeMS - _m_lLastCalMs;
		            _m_lLastCalMs = nowTimeMS;
		        }

		        _m_iEnergy = finalEnergy;
			}

	        //保存数据
	        _save();
	        
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_053_OnSeatChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加精力
	 * @param _value
	 * @param _context
	 */
	public void incrEnergy(int _value, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			_apply();
			
			_m_iEnergy += _value;
			
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_053_OnSeatChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 扣除脑力值
	 * @param _value
	 * @param _context
	 */
	public void descEnergy(int _value, NPPlayerContext _context)
	{
		if(_value <= 0)
			return;
		
		getUserData().lockUser();
		
		try
		{
			_apply();
			
			 int curEnergy = _m_iEnergy - _value;
	        if (curEnergy < 0)
	        {
	        	curEnergy = 0;
	        }

	        //消耗到低于最大值的时候恢复玩家使用道具恢复前等待的时间
	        if (curEnergy < getMaxEnergy() && _m_lFullGetNextRemainMs > 0)
	        {
	            _m_lLastCalMs -= _m_lFullGetNextRemainMs;
	            _m_lFullGetNextRemainMs = 0;
	        }
			
	        _m_iEnergy = curEnergy;
	        
			_save();
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_053_OnSeatChg(this));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 额外增加脑力值
	 * @param _iValue
	 * @param _context
	 * @return
	 */
	public int addEnergyExt(int _iValue, NPPlayerContext _context)
    {
		getUserData().lockUser();
		
		try
		{
			if (_iValue <= 0)
	            return 0;

	        _apply();

	        int maxEnergy = getMaxEnergy();
	        int curEnergy = _m_iEnergy;

	        int finalEnergy = curEnergy + _iValue;
	        if (finalEnergy >= maxEnergy)
	        {
	            //如果玩家使用道具恢复CD到满，则需要记下距离上次计算已经过了多少时间，在下次消耗到低于最大值的时候恢复这个时间
	            long nowTimeMS = CommonFunc.getNowTimeMS();
	            _m_lFullGetNextRemainMs = nowTimeMS - _m_lLastCalMs;
	            _m_lLastCalMs = nowTimeMS;
	        }

	        _m_iEnergy = finalEnergy;
	        
	        //保存数据
	        _save();
	        
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_053_OnSeatChg(this));

	        return finalEnergy - curEnergy;
		}
		finally
		{
			getUserData().unlockUser();
		}
    }
	
	/**
	 * 保存数据
	 */
	private void _save()
	{
		BM bmObj = getUSServer().getBM();
		
		if(null == _m_boChildSeat)
		{
			PlayerChildSeatBO bo = new PlayerChildSeatBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setSeatId(bmObj, getSeatId());
			bo.setEnergy(bmObj, _m_iEnergy);
			bo.setLastCalMs(bmObj, _m_lLastCalMs);
			bo.setFullGetNextRemainMs(bmObj, _m_lFullGetNextRemainMs);
			bo.insert(bmObj);
			
			_m_boChildSeat = bo;
		}
		else
		{
			_m_boChildSeat.setEnergy(bmObj, _m_iEnergy);
			_m_boChildSeat.setLastCalMs(bmObj, _m_lLastCalMs);
			_m_boChildSeat.setFullGetNextRemainMs(bmObj, _m_lFullGetNextRemainMs);
			_m_boChildSeat.saveAll(bmObj);
		}
	}

	/**
	 * 标记为使用中
	 * @return
	 */
	public ChildSeatInfo tryUse()
	{
		getUserData().lockUser();
		try
		{
		    if (_m_bUsing)
    			return null;
    		_m_bUsing = true;
    		return this;
		} finally
		{
		    getUserData().unlockUser();
		}
	}

	/**
	 * 构造协议对象
	 * @return
	 */
	public Child_SeatInfo toProto()
	{
		Child_SeatInfo proto = new Child_SeatInfo();
		proto.setSeatId(getSeatId());
		proto.setMaxEnergy(getMaxEnergy());
		proto.setEnergy(getEnergy());
		proto.setLastCalMs(getLastCalMs());

		return proto;
	}
}
