package NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ChildObj.Adult_ToMeApplyBaseInfo;
import Common.ServerObj.ServerObj_AdultMarryApplyInfo;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ChildComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerAdultToMeApplyBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class ToMeMarryApplyMgr 
{
	//子嗣组件
	private ChildComponent _m_comp;
	
	//请求数据列表
	private ArrayList<ToMeMarryApplyInfo> _m_alToMeMarryApplyList;

	public ToMeMarryApplyMgr(ChildComponent _comp)
	{
		_m_comp = _comp;
		
		_m_alToMeMarryApplyList = new ArrayList<>();
	}
	
	public ChildComponent getComp() {return _m_comp;}
	public NPUSUserData getUserData() {return _m_comp.getUserData();}
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	public void _initFromDB(_ICallBackBool _handler)
	{
		getUSServer().getBM().getBM(PlayerAdultToMeApplyBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerAdultToMeApplyBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(_m_comp.getUSServer(), "player:{} load toMeApply bo fail.", getUserData().getCid());
                
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerAdultToMeApplyBO> _list)
            {
            	_initFromDB(_list);
                
                _handler.onRunOver(true);
            }
        });
	}
	private void _initFromDB(List<PlayerAdultToMeApplyBO> _list)
	{
		for(int i = 0; i < _list.size(); i++)
		{
			PlayerAdultToMeApplyBO bo = _list.get(i);
			if(null == bo)
				continue;
			
			ToMeMarryApplyInfo info = new ToMeMarryApplyInfo(getComp(), bo);
			_m_alToMeMarryApplyList.add(info);
		}
		
		//进行一次排序，根据截至时间顺序
		CommonFunc.sortAscList(_m_alToMeMarryApplyList, new Comparator<ToMeMarryApplyInfo>() 
		{
			@Override
			public int compare(ToMeMarryApplyInfo o1, ToMeMarryApplyInfo o2) 
			{
				return Integer.compare(o1.getApplyExpiredTs(), o2.getApplyExpiredTs());
			}
		});
	}
	
	/**
	 * 刷新移除过期数据
	 */
	private void _refreshExpired()
	{
		for(int i = _m_alToMeMarryApplyList.size() - 1; i >= 0; i--)
		{
			ToMeMarryApplyInfo info = _m_alToMeMarryApplyList.get(i);
			if(null == info)
				continue;
			
			if(!info.isExpired())
				break;
			
			_m_alToMeMarryApplyList.remove(i);
			info.discard();
		}
	}
	
	/**
	 * 构造数据列表数据
	 * @param _list
	 */
	public void makeProto(ArrayList<Adult_ToMeApplyBaseInfo> _list)
	{
		getUserData().lockUser();
		
		try
		{
			//刷新移除过期数据
			_refreshExpired();
			
			for(int i = 0; i < _m_alToMeMarryApplyList.size(); i++)
			{
				ToMeMarryApplyInfo info = _m_alToMeMarryApplyList.get(i);
				if(null == info)
					continue;
				
				_list.add(info.toBaseProto());
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 查找指定子嗣的请求数据
	 * @param _applyInstanceId
	 * @return
	 */
	public ToMeMarryApplyInfo checkExpiredAndLookup(long _applyAdultId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alToMeMarryApplyList.size(); i++)
			{
				ToMeMarryApplyInfo info = _m_alToMeMarryApplyList.get(i);
				if(null == info)
					continue;
				
				if(info.getApplyAdultId() == _applyAdultId)
				{
					//检查是否过期，如果过期，直接移除数据
					if(info.isExpired())
					{
						info.discard();
						return null;
					}
					
					return info;
				}
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 增加向玩家的请求记录
	 * @param _applyAdult
	 * @param _context
	 */
	public void add(ServerObj_AdultMarryApplyInfo _apply, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			//移除旧数据，保证一个申请子嗣只有一条记录
			getAndDel(_apply.getApplyAdult().getId(), _context);
			
			//创建新数据
			BM bmObj = getUSServer().getBM();
			
			PlayerAdultToMeApplyBO bo = new PlayerAdultToMeApplyBO();
			bo.setCid(bmObj, getUserData().getCid());
			bo.setApplyCid(bmObj, _apply.getApplyCid());
			bo.setApplyCname(bmObj, _apply.getApplyCname());
			bo.setApplyAdultId(bmObj, _apply.getApplyAdult().getId());
			bo.setInitResId(bmObj, _apply.getApplyAdult().getInitResId());
			bo.setQuality(getUSServer().getBM(), _apply.getApplyAdult().getQuality());
			bo.setAttrType(getUSServer().getBM(), _apply.getApplyAdult().getAttrType().ordinal());
			bo.setCareer(getUSServer().getBM(), _apply.getApplyAdult().getCareerId());
			bo.setIsGiftde(bmObj, _apply.getApplyAdult().getIsGiftde());
			bo.setName(bmObj, _apply.getApplyAdult().getName());
			bo.setBonus(bmObj, _apply.getApplyAdult().getBonus());
			bo.setMarriedItem(bmObj, CommonFunc.ByteBfferToBytes(_apply.getMarriedItem().makePackage()));
			bo.setApplyExpiredTs(bmObj, _apply.getApplyExpiredTs());
			bo.insert(bmObj);
			
			ToMeMarryApplyInfo toMeApply = new ToMeMarryApplyInfo(_m_comp, bo);
			_m_alToMeMarryApplyList.add(toMeApply);
			
			//推送数据
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_056_OnToMeApplyAdd(toMeApply));
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 获取并移除请求数据
	 * @param _applyAdultId
	 * @param _context
	 * @return
	 */
	public ToMeMarryApplyInfo getAndDel(long _applyAdultId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alToMeMarryApplyList.size(); i++)
			{
				ToMeMarryApplyInfo toMeApply = _m_alToMeMarryApplyList.get(i);
				if(null == toMeApply)
					continue;
				
				if(toMeApply.getApplyAdultId() == _applyAdultId)
				{
					//删除数据
					_m_alToMeMarryApplyList.remove(i);
					toMeApply.discard();
					//推送删除数据
					getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_058_OnToMeApplyDel(toMeApply.getApplyAdultId()));
					
					//如果已经过期，则返回null
					if(toMeApply.isExpired())
						return null;
					
					return toMeApply;
				}
			}
			
			return null;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 拒绝联姻请求
	 * @param _applyAdultId
	 * @param _context
	 * @return
	 */
	public ToMeMarryApplyInfo refuse(long _applyAdultId, NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			ToMeMarryApplyInfo toMeApply = getAndDel(_applyAdultId, _context);
			if(null == toMeApply)
				return null;
			
			//同步申请方数据
			ALSynTaskManager.getInstance().regTask(()->
			{
				AdultMarrySystem.SendRefusePlayerApply(toMeApply, _context);
			});
			
			return toMeApply;
		}
		finally
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 拒绝所有联姻请求
	 * @param _context
	 */
	public void allRefuse(NPPlayerContext _context)
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_alToMeMarryApplyList.isEmpty())
				return;
			
			//移除所有请求数据
			ArrayList<ToMeMarryApplyInfo> delToMeApplyList = new ArrayList<>(_m_alToMeMarryApplyList);
			_m_alToMeMarryApplyList.clear();
			
			BM bmObj = getUSServer().getBM();
			bmObj.getBM(PlayerAdultToMeApplyBO.class).delAll("cid", getUserData().getCid());
			
			//清空数据推送
			getUserData().sendMsgToGC(US2GCWriter_014_ChildOp.make_060_OnToMeApplyClear());
			
			//同步申请方数据
			for(int i = 0; i < delToMeApplyList.size(); i++)
			{
				ToMeMarryApplyInfo delToMeApply = delToMeApplyList.get(i);
				if(null == delToMeApply)
					continue;
				
				AdultMarrySystem.SendRefusePlayerApply(delToMeApply, _context);
			}
		}
		finally
		{
			getUserData().unlockUser();
		}
	}
}
