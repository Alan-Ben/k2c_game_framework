package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import Common.MarsObj.Mars_PeopleNum;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_040_MarsPeopleOp;

public class MarsPeopleNumInfo 
{
	//火星居民组件
	private MarsPeopleComponent _m_comp;
	
	public MarsPeopleNumInfo(MarsPeopleComponent _comp)
	{
		_m_comp = _comp;
	}
	
	private void _lock() {_m_comp._lock();}
	private void _unlock() {_m_comp._unlock();}
	
	public MarsPeopleComponent getComp() {return _m_comp;}
	
	public long getIdleNum() {return getComp().getBo().getIdleNum();}
	public long getSickNum() {return getComp().getBo().getSickNum();}
	//派遣居民数量（来自火星建筑组件）
	public int getWorkingNum() {return getComp().getUserData().getMarsBuildingComponent().getPeopleBuildingFuncMgr().dispatchedSum();}
	
	//火星居民总数（不包括移民状态中的居民）
	public long getPeopleSum() {return getIdleNum() + getWorkingNum() + getSickNum();}
	
	public Mars_PeopleNum toProto()
	{
		Mars_PeopleNum proto = new Mars_PeopleNum();
		proto.setIdle(getIdleNum());
		proto.setSick(getSickNum());
		
		return proto;
	}
	
	/**
	 * 设置休闲中居民数量
	 * @param _num
	 * @param _context
	 */
	public void setIdleNum(long _num, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			//增加准备居民
			getComp().getBo().saveIdleNum(getComp().getBM(), _num);
			
			//推送数据
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_050_OnMarsPeopleNumChg(this));

			//触发重新计算健康/幸福指数
			getComp().getUserData().getMarsBuildingComponent().doLazyCalHealthIndex();
			getComp().getUserData().getMarsBuildingComponent().doLazyCalHappyIndex();
		}
		finally
		{
			_unlock();
		}
	}
	/**
	 * 增加空闲人数
	 * @param _num
	 * @param _context
	 */
	public void incrIdleNum(long _num, NPPlayerContext _context)
	{
		if(_num <= 0)
			return;
		
		_lock();
		
		try
		{
			setIdleNum(getIdleNum() + _num, _context);
		}
		finally
		{
			_unlock();
		}
	}
	/**
	 * 减少空闲人数
	 * @param _num
	 * @param _context
	 */
	public void reduceIdleNum(long _num, NPPlayerContext _context)
	{
		if(_num <= 0)
			return;
		
		_lock();
		
		try
		{
			long newNum = Math.max(getIdleNum() - _num, 0);
			
			setIdleNum(newNum, _context);
		}
		finally
		{
			_unlock();
		}
	}

	/**
	 * 设置生病中居民数量
	 * @param _num
	 * @param _context
	 */
	public void setSickNum(long _num, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			getComp().getBo().setSickNum(getComp().getBM(), _num);
			getComp().getBo().saveAll(getComp().getBM());
			
			//推送数据
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_050_OnMarsPeopleNumChg(this));

			//触发重新计算健康/幸福指数
			getComp().getUserData().getMarsBuildingComponent().doLazyCalHealthIndex();
			getComp().getUserData().getMarsBuildingComponent().doLazyCalHappyIndex();
		}
		finally
		{
			_unlock();
		}
	}
	/**
	 * 减少空闲人数
	 * @param _num
	 * @param _context
	 */
	public void incrSickNum(long _num, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			long newNum = getSickNum() + _num;
			
			setSickNum(newNum, _context);
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 治愈居民：生病 -> 空闲 
	 * @param _num
	 * @param _context
	 */
	public void transSickToIdle(long _num, NPPlayerContext _context)
	{
		if(_num == 0)
			return;
		
		_lock();
		
		try
		{
			//没有生病居民
			long preSickNum = getSickNum();
			if(preSickNum <= 0)
				return;
			
			long newSickNum = preSickNum;
			if(-1 == _num) //治疗全部生病居民
			{
				newSickNum = 0;
			}
			else //治疗指定数量的生病居民
			{
				newSickNum = Math.max(preSickNum - _num, 0);
			}
			//计算转变数量
			long realChgNum = preSickNum - newSickNum;

			getComp().getBo().setSickNum(getComp().getBM(), newSickNum);
			getComp().getBo().setIdleNum(getComp().getBM(), getIdleNum() + realChgNum);
			getComp().getBo().saveAll(getComp().getBM());
			
			//推送数据
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_050_OnMarsPeopleNumChg(this));

			//触发重新计算健康/幸福指数
			getComp().getUserData().getMarsBuildingComponent().doLazyCalHealthIndex();
			getComp().getUserData().getMarsBuildingComponent().doLazyCalHappyIndex();
		}
		finally
		{
			_unlock();
		}
	}
	
	/**
	 * 治愈生病居民
	 * @param _num
	 * @param _context
	 */
	public void cureSickNum(long _num, NPPlayerContext _context)
	{
		_lock();
		
		try
		{
			long cureNum = Math.min(_num, getSickNum());
			if(cureNum <= 0)
				return;
			
			long newIdleNum = getIdleNum() + cureNum;
			long newSickNum = getSickNum() - cureNum;
			
			//更新数据
			getComp().getBo().setIdleNum(getComp().getBM(), newIdleNum);
			getComp().getBo().setSickNum(getComp().getBM(), newSickNum);
			getComp().getBo().saveAll(getComp().getBM());
			
			//推送数据
			getComp().getUserData().sendMsgToGC(US2GCWriter_040_MarsPeopleOp.make_050_OnMarsPeopleNumChg(this));
		}
		finally
		{
			_unlock();
		}
	}
}
