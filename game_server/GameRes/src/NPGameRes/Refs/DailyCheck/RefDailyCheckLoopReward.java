package NPGameRes.Refs.DailyCheck;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

@RefTable(tableName = "daily_check_loop_reward")
public class RefDailyCheckLoopReward extends RefBase
{
    private static RefDailyCheckLoopRewardMgr _g_mgr = new RefDailyCheckLoopRewardMgr();
    public static RefDailyCheckLoopRewardMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefDailyCheckLoopRewardMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefDailyCheckLoopRewardMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefDailyCheckLoopReward newRef = (RefDailyCheckLoopReward) _newRef;
        id = newRef.id;
        is_loop = newRef.is_loop;
        loop_add_day = newRef.loop_add_day;
        gain_item_list = newRef.gain_item_list;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id;
    }
    
    public static class RefDailyCheckLoopRewardMgr extends RefTableContainer<RefDailyCheckLoopReward>
    {
    	//签到阶段奖励数据对象整理
    	private DailyCheckLoopRewardObj _m_doRewardObj;
    	
    	@Override
    	protected void _onTableLoaded()
    	{
    		DailyCheckLoopRewardObj obj = new DailyCheckLoopRewardObj();
    		RefDailyCheckLoopReward preRef = null;
    		int curDay = 0;
    		RefDailyCheckLoopReward firstLoopRef = null;
    		RefDailyCheckLoopReward preLoopRef = null;
    		ArrayList<RefDailyCheckLoopReward> refList = new ArrayList<>(getList());
    		for(int i = 0; i < refList.size(); i++)
    		{
    			RefDailyCheckLoopReward ref = refList.get(i);
    			if(null == ref)
    				continue;
    			
    			curDay += ref.loop_add_day;
    			ref.curDay = curDay;
    			
    			obj.refList.add(ref);
    			obj.roundDays += ref.loop_add_day;
    			
    			if(ref.is_loop)
    			{
    				obj.loopRefList.add(ref);
    				obj.loopDays += ref.loop_add_day;

    				//首条循环周期数据
    				if(null == firstLoopRef)
    				{
    					firstLoopRef = ref;
    				}
    				//如果上一条也是循环数据
    				ref.loopSumDays = ref.loop_add_day;
    				if(null != preLoopRef)
    				{
    					ref.loopSumDays += preLoopRef.loopSumDays;
    				}
    				
    				preLoopRef = ref;
    			}
    			
    			//前后配置
    			if(null != preRef)
    			{
    				ref.preRef = preRef;
    				preRef.nextRef = ref;
    			}
    			//最后一条配置的处理
    			int nextIdx = i + 1;
    			if(nextIdx >= refList.size())
    			{
    				//如果是循环周期的配置，则下一条配置接入第一条循环配置
    				if(ref.is_loop)
    				{
    					ref.nextRef = firstLoopRef;
    				}
    			}

    			preRef = ref;
    		}
    		
    		_m_doRewardObj = obj;
    	}
    	
    	/**
    	 * 根据传递的天数（可以领取奖励的天数），指定当前可以看到的领取奖励的一组数据
    	 * 
    	 * 处理方案：
    	 * 1. 定位到指定天数所在的位置
    	 * 2. 根据位置推算所在的分组
    	 * 
    	 * @param _days
    	 * @param _groupSize
    	 * @return
    	 */
    	public DailyCheckLoopRewardShowResult getGroupList(int _days, int _groupSize)
    	{
    		//参数检查
    		if(_days < 0 || _groupSize <= 0)
    		{
    			CommLog.error("RefDailyCheckLoopReward getGroupList params[{}, {}] error.", _days, _groupSize);
    			return null;
    		}
    		//检查当前数据
    		DailyCheckLoopRewardObj obj = _m_doRewardObj;
    		if(null == obj || obj.refList.isEmpty())
    		{
    			CommLog.error("RefDailyCheckLoopReward getGroupList ref error.");
    			return null;
    		}
    		
    		//结果数据
    		DailyCheckLoopRewardShowResult showResult = new DailyCheckLoopRewardShowResult();

    		//预设配置拍成一个符合队列，计算当前数据所处的位置，从1开始
    		int idxCount = 0;
    		
    		//一轮周期后剩余的天数，这部分天数是循环周期里的天数
    		int remainDays = _days - obj.roundDays;
    		//进入循环次数
    		int loopCount = 0;
    		if(remainDays > 0) //进入循环
    		{
    			//进入循环，需要确保有可以循环的基数
    			if(obj.loopDays <= 0)
    			{
        			CommLog.error("RefDailyCheckLoopReward getGroupList enter loop loop-days:{} error.", obj.loopDays);
        			return null;
        		}

    			//计算循环次数
    			loopCount = remainDays / obj.loopDays;
    			//当前已经计算好的位置
    			idxCount = obj.refList.size() + loopCount * obj.loopRefList.size();

    			//计算最后剩余天数（一个循环周期内的位置）
    			int calRemainDays = remainDays - loopCount * obj.loopDays;
    			//如果有多出的天数，则需要计算下一个循环
    			if(calRemainDays > 0)
    			{
    				//计算循环次数，结果是去除中间周期循环，直接计算最后一个循环
        			for(int i = 0; i < obj.loopRefList.size(); i++)
        			{
        				RefDailyCheckLoopReward ref = obj.loopRefList.get(i);
            			if(null == ref)
            				continue;
            			
            			if(calRemainDays <= ref.loopSumDays)
            			{
            				idxCount += (i + 1);
            				break;
            			}
        			}
    			}
    		}
    		else //不满足一个周期的处理
    		{
    			for(int i = 0; i < obj.refList.size(); i++)
    			{
    				RefDailyCheckLoopReward ref = obj.refList.get(i);
        			if(null == ref)
        				continue;
        			
        			if(_days <= ref.curDay)
        			{
        				idxCount = i + 1;
        				break;
        			}
    			}
    		}
    		
    		//根据计算得到的队列位置，计算该位置所处的分组，再计算分组里其他配置数据
    		if(0 >= idxCount)
    		{
    			CommLog.error("RefDailyCheckLoopReward getGroupList cal days:{} groupSize:{} idx:{} error.", _days, _groupSize, idxCount);
    			return null;
    		}
    		
    		//根据队列所在的位置，计算所在位置的分组数据
    		if(idxCount > obj.refList.size()) //当前所处位置大于一轮周期
    		{
    			//计算当前所在的位置的数据
    			int curLoopRemain = (idxCount - obj.refList.size()) % obj.loopRefList.size();
    			if(curLoopRemain == 0)
    				curLoopRemain = obj.loopRefList.size();
    			int curLoopIdx = curLoopRemain  - 1;
    			
    			//剩余天数的循环次数，用于计算总天数
    			int curLoopCount = (idxCount - 1 - obj.refList.size()) / obj.loopRefList.size();
    			
    			RefDailyCheckLoopReward curRef = obj.loopRefList.get(curLoopIdx);
    			int curDay = obj.roundDays + curLoopCount * obj.loopDays + curRef.loopSumDays;
    			DailyCheckLoopRewardShow curResult = new DailyCheckLoopRewardShow(curRef, curDay);
				
    			showResult.addCurShow(curResult);
    			
				//根据展示余数确定从当前的配置往前推或者往后推
				int curRemain = idxCount % _groupSize;
    			if(curRemain == 0)
    				curRemain = _groupSize;
				//向前推
    			RefDailyCheckLoopReward tmpPreRef = curRef.preRef;
    			int tmpReduceDays = curRef.loop_add_day;
				int tmpLastLoopCount = 0;
				int tmpLastCuryDay = curDay;
				boolean isLoopEnd = false;
				for(int i = (curRemain - 1); i > 0; i--)
				{
					if(null == tmpPreRef)
						break;

					//已经跳出循环
					if(isLoopEnd)
					{
						tmpLastCuryDay -= tmpReduceDays;
	    				
						RefDailyCheckLoopReward tmpRef = tmpPreRef;
						int tmpCurDay = tmpLastCuryDay;
	    				DailyCheckLoopRewardShow tmpResult = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
	    				
	    				showResult.addCurShow(tmpResult);

	    				tmpReduceDays = tmpPreRef.loop_add_day;
	    				tmpPreRef = tmpPreRef.preRef;
	    				
	    				continue;
					}
					
					//尚在循环中
					if(tmpPreRef.is_loop)
					{
						tmpLastCuryDay -= tmpReduceDays;
	    				
						RefDailyCheckLoopReward tmpRef = tmpPreRef;
						int tmpCurDay = tmpLastCuryDay;
	    				DailyCheckLoopRewardShow tmpResult = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
	    				
	    				showResult.addCurShow(tmpResult);

	    				tmpReduceDays = tmpPreRef.loop_add_day;
	    				tmpPreRef = tmpPreRef.preRef;
	    				
	    				continue;
					}
					
					//前一个配置不是循环周期，需要检查是否超过循环次数，超过表示循环终止，讲进入单次排期
					tmpLastLoopCount++;
					if(tmpLastLoopCount < loopCount)
					{
						tmpLastCuryDay -= tmpReduceDays;
	    				
						RefDailyCheckLoopReward tmpRef = tmpPreRef;
						int tmpCurDay = tmpLastCuryDay;
	    				DailyCheckLoopRewardShow tmpResult = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
	    				
	    				showResult.addCurShow(tmpResult);
	    				
	    				//从循环周期的最后一个配置重新开始
	    				tmpReduceDays = tmpPreRef.loop_add_day;
	    				tmpPreRef = obj.loopRefList.get(obj.loopRefList.size() - 1);
					}
					else //终止循环，进入单次周期
					{
						tmpLastCuryDay -= tmpReduceDays;
	    				
						RefDailyCheckLoopReward tmpRef = tmpPreRef;
						int tmpCurDay = tmpLastCuryDay;
	    				DailyCheckLoopRewardShow tmpResult = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
	    				
	    				showResult.addCurShow(tmpResult);

	    				tmpReduceDays = tmpPreRef.loop_add_day;
	    				tmpPreRef = tmpPreRef.preRef;
	    				
	    				//循环已经结束
	    				isLoopEnd = true;
					}
				}
				
    			//向后推
				RefDailyCheckLoopReward tmpNextRef = curResult.getRef().nextRef;
				int tmpNextCurDay = curResult.getCurDay();
    			for(int i = curRemain; i < _groupSize; i++)
    			{
    				if(null == tmpNextRef)
    					break;
    				
    				tmpNextCurDay += tmpNextRef.loop_add_day;

					RefDailyCheckLoopReward tmpRef = tmpNextRef;
					int tmpCurDay = tmpNextCurDay;
    				DailyCheckLoopRewardShow tmpResult = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
    				
    				showResult.addCurShow(tmpResult);
    				
    				tmpNextRef = tmpNextRef.nextRef;
    			}
    			//根据天数进行排序
    			CommonFunc.sortAscList(showResult.getShowList(), new Comparator<DailyCheckLoopRewardShow>() {

					@Override
					public int compare(DailyCheckLoopRewardShow o1, DailyCheckLoopRewardShow o2) 
					{
						return Integer.compare(o1.getCurDay(), o2.getCurDay());
					}
				});
    		}
    		else //当前所处位置小于等于一轮周期
    		{
    			int count = (idxCount - 1) / _groupSize;
    			for(int i = 0; i < _groupSize; i++)
    			{
    				int resultIdx = count * _groupSize + i;
    				if(resultIdx < obj.refList.size()) //还在周期之内
    				{
    					RefDailyCheckLoopReward tmpRef = obj.refList.get(resultIdx);
    					int tmpCurDay = tmpRef.curDay;
    					DailyCheckLoopRewardShow result = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
    					
    					showResult.addCurShow(result);
    				}
    				else //这部分数据进入了循环
    				{
    					if(obj.loopRefList.isEmpty())
    					{
    						break;
    					}
    					
    					int tmpLoopIdx = resultIdx - obj.refList.size();
    					int resultLoopCount = (tmpLoopIdx + 1) / obj.loopRefList.size();
    					int resultLoopIdx = tmpLoopIdx - resultLoopCount * obj.loopRefList.size();
    					
    					RefDailyCheckLoopReward tmpRef = obj.loopRefList.get(resultLoopIdx);
    					int tmpCurDay = obj.roundDays + resultLoopCount * obj.loopDays + tmpRef.loopSumDays;
    					DailyCheckLoopRewardShow result = new DailyCheckLoopRewardShow(tmpRef, tmpCurDay);
    					
    					showResult.addCurShow(result);
    				}
    			}
    		}
    		
    		//计算前后的节点数据
    		if(!showResult.getShowList().isEmpty())
    		{
    			//向前-1的节点数据
    			DailyCheckLoopRewardShow firstShow = showResult.getShowList().get(0);
    			RefDailyCheckLoopReward preRef = firstShow.getRef().preRef;
				if(null != preRef)
				{
					if(firstShow.getCurDay() <= obj.roundDays  //如果第一个节点尚未超过总周期节点，则直接设置该配置的前一个节点
							|| preRef.is_loop //如果是上一个节点还在循环周期里，也可以直接设置
							)
	    			{
	    				DailyCheckLoopRewardShow preShow = new DailyCheckLoopRewardShow(preRef, firstShow.getCurDay() - firstShow.getRef().loop_add_day);
	    				showResult.setPreShow(preShow);
	    			}
	    			else //大于总周期且上一个节点不是循环节点，所以必然还在循环周期里，上一个节点必然是循环周期里的最后一个节点
	    			{
	    				RefDailyCheckLoopReward lastLoopRef = obj.loopRefList.get(obj.loopRefList.size() - 1);
	    				if(null != lastLoopRef)
	    				{
	    					DailyCheckLoopRewardShow preShow = new DailyCheckLoopRewardShow(lastLoopRef, firstShow.getCurDay() - firstShow.getRef().loop_add_day);
		    				showResult.setPreShow(preShow);
	    				}
	    			}
				}
    			
    			//计算向后+1的节点数据
    			DailyCheckLoopRewardShow lastShow = showResult.getShowList().get(showResult.getShowList().size() - 1);
    			RefDailyCheckLoopReward nextRef = lastShow.getRef().nextRef;
    			if(null != nextRef)
    			{
    				DailyCheckLoopRewardShow nextShow = new DailyCheckLoopRewardShow(nextRef, lastShow.getCurDay() + nextRef.loop_add_day);
    				showResult.setNextShow(nextShow);
    			}
    		}
    		
    		return showResult;
    	}
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public boolean is_loop;//是否循环
    public int loop_add_day;//循环增加的天数
    public List<NPCommonCostItem> gain_item_list;//奖励列表
    
    @RefField(isIgnore = true)
    public RefDailyCheckLoopReward preRef;//上一条数据配置
    @RefField(isIgnore = true)
    public RefDailyCheckLoopReward nextRef;//下一条数据配置
    @RefField(isIgnore = true)
    public int curDay;//当前天数
    @RefField(isIgnore = true)
    public int loopSumDays;//循环周期里，累计到当前配置的天数
    
}