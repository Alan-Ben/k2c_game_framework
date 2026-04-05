package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortTriggeredCallStory;

import Common.Common_LongList;
import Common.ConsortEnum.EConsortStoryType;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Consort.RefConsortStory;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashSet;

/**
 * 家人已触发事件数据管理
 * @author mj
 *
 */
public class ConsortTriggeredCallStoryInfo 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	//已经触发的邀请数据
	private HashSet<Long> _m_hsTriggeredCallStoryIdSet;

	public ConsortTriggeredCallStoryInfo(ConsortInfo _consort)
	{
		_m_ciConsort = _consort;
		
		_m_hsTriggeredCallStoryIdSet = new HashSet<>();
		//解析已经触发的事件列表
		if(null != _consort.getBo().getTriggeredCallStoryIdList())
		{
			ByteBuffer buff = ByteBuffer.wrap(_consort.getBo().getTriggeredCallStoryIdList());
			Common_LongList listObj = new Common_LongList();
			listObj.readPackage(buff);
			
			_m_hsTriggeredCallStoryIdSet.addAll(listObj.getValueList());
		}
	}
	
	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}
	
	/**
	 * 构造数据列表协议
	 * @param _list
	 */
	public void makeProto(ArrayList<Long> _list)
	{
		getUserData().lockUser();
		
		try
		{
			_list.addAll(_m_hsTriggeredCallStoryIdSet);
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 触发事件
	 * @param _hasCg
	 * @param _storyType
	 * @return
	 */
	/**
	 * 优化：GOB-3771 【流程】-【家人】-家人非首次触发CG剧情时不显示CG视频 
	 * https://www.teambition.com/task/686cc7d69bc25355566e860c
	 */
	public ConsortTriggerStoryResult triggerStory(boolean _hasCg, EConsortStoryType _storyType, NPPlayerContext _context)
	{
		getUserData().lockUser();

		try
		{
			ArrayList<RefConsortStory> storyRefList = _m_ciConsort.getRef().callStoryRefListMap.get(_storyType.ordinal());
			if (null == storyRefList || storyRefList.isEmpty())
				return null;

			//获取解锁故事列表
			ArrayList<RefConsortStory> unlockRefList = new ArrayList<>();
			for (RefConsortStory ref : storyRefList)
			{
				if (null == ref)
					continue;

				if (!NPPlayerConditionDealerMgr.IsEnable(ref.unlock_condition, getUserData(), null))
					continue;

				unlockRefList.add(ref);
			}
			if (unlockRefList.isEmpty())
				return null;
			
			RefConsortStory triggerRef;
			
			//获取触发的事件的流程
			do
			{
				//GOB-5124【优化-0】家人--有已解锁未触发的cg剧情的时候优先触发cg剧情 https://www.teambition.com/task/68d906b322608978d3ec5dc4
		    	//补充说明：是针对CALL类型新增的规则，如果不是FIRST_CALL类型，优先执行此规则
                //#################
                //=============== 20260306 该需求被搁置 =================
                //20260303再次优化：GOB-9080【优化-1】家人CG获得概率配置优化 https://www.teambition.com/task/69840f4acb7f75f7ad242b24
                //优化说明1：非首次邀约，需要先确认是否是获得CG的情况
                //优化说明2：需要检查fix_cd，用于限制玩家每日获得新CG的次数，避免玩家过快获取所有CG
                //long fixCd = getUserData().getItemCount(ENPItemType.FIXED_CD, RefGeneral.Ref().consort_call_new_cg_fixed_cd);
				if(EConsortStoryType.FIRST_CALL != _storyType)
				{
					ArrayList<RefConsortStory> priorityList = null;
			    	for(int i = 0; i < unlockRefList.size(); i++)
			    	{
			    		RefConsortStory ref = unlockRefList.get(i);
			    		if(null == ref)
			    			continue;
			    		
			    		//跳过没有cg的事件
			    		if(ref.unlock_cg <= 0)
			    			continue;

			    		//跳过已经触发的事件
			    		if(_m_hsTriggeredCallStoryIdSet.contains(ref.id))
			    			continue;
			    		
			    		if(null == priorityList)
			    			priorityList = new ArrayList<>();
			    		
			    		priorityList.add(ref);
			    	}
			    	if(null != priorityList && !priorityList.isEmpty())
			    	{
			    		int idx = CommonFunc.randomInt(priorityList.size() - 1);
			    		triggerRef = priorityList.get(idx);
			    		break;
			    	}
				}
				
				//如果之前规则没有生效，则执行后续的流程
				//获取符合要求的事件列表
				ArrayList<RefConsortStory> firstRefList = new ArrayList<>();
				for (RefConsortStory ref : unlockRefList)
				{
					if (null == ref)
						continue;

					if (!_hasCg && ref.unlock_cg == 0)
					{
						firstRefList.add(ref);
					} 
					else if (_hasCg && ref.unlock_cg > 0)
					{
						firstRefList.add(ref);
					}
				}
				
				if (!firstRefList.isEmpty())
				{
					triggerRef = _m_ciConsort.getRef().rndCallStory(firstRefList, _m_hsTriggeredCallStoryIdSet);
				} 
				else //没有符合要求的数据，对全部已解锁的数据进行查找
				{
					triggerRef = _m_ciConsort.getRef().rndCallStory(unlockRefList, _m_hsTriggeredCallStoryIdSet);
				}
				
			} while(false);
			
			if (null == triggerRef)
				return null;
			
			ConsortTriggerStoryResult result = new ConsortTriggerStoryResult();
			result.setConsortStoryRef(triggerRef);

			//记录当前已触发事件
			addTriggeredStrory(triggerRef.id);

			//触发同时获得对应的CG
			if (triggerRef.unlock_cg > 0)
			{
				int gainCount = getUserData().getConsortComponent().getCGMgr().gainCG(triggerRef.unlock_cg, _context);
				if(gainCount > 0)
				{
					result.setGainCg(true);
				}
			}

			return result;
		} 
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/*****
	 * 记录已触发事件
	 * @param _stroyId
	 */
	public void addTriggeredStrory(long _stroyId)
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_hsTriggeredCallStoryIdSet.contains(_stroyId))
				return;
			
			_m_hsTriggeredCallStoryIdSet.add(_stroyId);
			
			//保存数据
			Common_LongList listObj = new Common_LongList();
			listObj.getValueList().addAll(_m_hsTriggeredCallStoryIdSet);
			
			BM bmObj = getUSServer().getBM();
			_m_ciConsort.getBo().saveTriggeredCallStoryIdList(bmObj, CommonFunc.ByteBfferToBytes(listObj.makePackage()));
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_054_OnTriggeredCallDlgIdAdd(_m_ciConsort, _stroyId));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
