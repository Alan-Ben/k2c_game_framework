package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.OfflineReward_Info;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer.OfflineDataDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer._AOfflineDataDealer;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerOfflineRewardBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;


/****************************
 * 用户延迟消息组件
 * @author mark
 *
 */
public class OfflineRewardComponent extends _ANPUserComponent implements _IHandlerHolder
{
    private ArrayList<OfflineRewardInfo> _m_alOfflineRewardList;

    public OfflineRewardComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.OFFLINE_REWARD);

        _m_alOfflineRewardList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerOfflineRewardBO.class).findAll("cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerOfflineRewardBO>>()
        {
            @Override
            public void dealFail()
            {
            	getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerOfflineRewardBO> _boList)
            {
            	_initFromBoList(_boList);
            	
            	setInited();
            }
        });    	
    }
    //初始化离线数据
    private void _initFromBoList(List<PlayerOfflineRewardBO> _boList)
    {
    	for(int i = 0; i < _boList.size(); i++)
    	{
    		PlayerOfflineRewardBO bo = _boList.get(i);
    		if(null == bo)
    			continue;
    		
    		//检查离线数据枚举
    		EOfflineRewardEnum offlineEnum = EOfflineRewardEnum.EOfflineRewardEnum_FromInt(bo.getRewardType());
    		if(null == offlineEnum)
    		{
    			USLog.error(getUSServer(), "player:{} init offline data fail, offline enum:{} error", getUserData().getCid(), bo.getRewardType());
    			continue;
    		}
    		//无法找到对应的dealer
    		_AOfflineDataDealer dealer = OfflineDataDealerMgr.getInstance().getDealer(offlineEnum);
    		if(null == dealer)
    		{
    			USLog.error(getUSServer(), "player:{} init offline data fail, lost dealer:{}", getUserData().getCid(), offlineEnum);
    			continue;
    		}
    		
    		OfflineRewardInfo info = new OfflineRewardInfo(this, bo, dealer);
    		_m_alOfflineRewardList.add(info);
    	}

    	//按数据实例ID自增顺序排序，用于依次处理
    	CommonFunc.sortAscList(_m_alOfflineRewardList, new Comparator<OfflineRewardInfo>() {

			@Override
			public int compare(OfflineRewardInfo o1, OfflineRewardInfo o2) 
			{
				return Long.compare(o1.getId(), o2.getId());
			}
		});
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    	if(_m_alOfflineRewardList.isEmpty())
    		return;
    	
    	ArrayList<OfflineRewardInfo> offlineList = new ArrayList<>(_m_alOfflineRewardList);
    	_m_alOfflineRewardList.clear();
    	
    	for(int i = 0; i < offlineList.size(); i++)
    	{
    		OfflineRewardInfo info = offlineList.get(i);
    		if(null == info)
    			continue;
    		
    		//已经无效，可以移除
    		if(!info.getDealer().isValid())
    		{
    			info.del();
    			continue;
    		}
    		
    		//预处理
    		info.getDealer().preDeal(info, getUserData().getPlayerInitContext());
    		
    		//如果需要同步客户端，重新加入列表
    		if(info.getDealer().syncToClient()) //需要同步
    		{
    			_m_alOfflineRewardList.add(info);
    		}
    		else //不需要同步，直接移除
    		{
    			info.del();
    		}
    	}
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
    }

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    /**
     * 构造离线奖励结构数据列表
     * @param _list
     */
    public void makeProtoList(ArrayList<OfflineReward_Info> _list)
    {
        getUserData().lockUser();

        try
        {
            for (int i = 0; i < _m_alOfflineRewardList.size(); i++)
            {
                OfflineRewardInfo info = _m_alOfflineRewardList.get(i);
                if (null == info)
                    continue;

                _list.add(info.toProto());
            }
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找离线奖励
     * @return
     */
    public OfflineRewardInfo lookupReward(long _id)
    {
        getUserData().lockUser();

        try
        {
            for (int i = 0; i < _m_alOfflineRewardList.size(); i++)
            {
                OfflineRewardInfo info = _m_alOfflineRewardList.get(i);
                if (null == info)
                    continue;

                if (info.getId() == _id)
                    return info;
            }

            return null;
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加奖励记录
     * @param _rewardType
     * @param _offlineData
     * @param _itemList
     * @param _rewardShow
     * @param _context
     */
    public void addReward(EOfflineRewardEnum _rewardType, ByteBuffer _offlineData, ByteBuffer _itemList, ByteBuffer _rewardShow, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
        	//没有找到对应dealer，不处理
            _AOfflineDataDealer dealer = OfflineDataDealerMgr.getInstance().getDealer(_rewardType);
            if(null == dealer)
            {
            	USLog.error(getUSServer(), "player:{} add offline reward:{} fail, not find dealer.", getUserData().getCid(), _rewardType);
            	return;
            }
            
            //创建离线数据
            if(dealer.syncToClient()) //需要同步客户端
            {
            	PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setRewardType(getUSServer().getBM(), _rewardType.ordinal());
                
                if(null != _offlineData)
                	bo.setOfflineData(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_offlineData));
                if(null != _itemList)
                	bo.setItemList(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_itemList));
                if(null != _rewardShow)
                	bo.setRewardShow(getUSServer().getBM(), CommonFunc.ByteBfferToBytes(_rewardShow));
                
                bo.insert(getUSServer().getBM());
            	
            	OfflineRewardInfo info = new OfflineRewardInfo(this, bo, dealer);
                //进行预处理
                dealer.preDeal(info, _context);
                //放入队列并推送客户端
            	_m_alOfflineRewardList.add(info);
                //推送协议
                getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_055_OnOfflineRewardAdd(info));
            }
            else //无需同步客户端，无需创建BO，直接处理完成
            {
            	OfflineRewardInfo info = new OfflineRewardInfo(this, _rewardType.ordinal(), _offlineData, _itemList, _rewardShow, dealer);
                //进行预处理
                dealer.preDeal(info, _context);
            }
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取离线奖励
     * @param _id
     * @param _context
     * @return
     */
    public OfflineRewardTakeResult takeReward(long _id, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
        	OfflineRewardTakeResult result = new OfflineRewardTakeResult();
        	
            for (int i = 0; i < _m_alOfflineRewardList.size(); i++)
            {
                OfflineRewardInfo info = _m_alOfflineRewardList.get(i);
                if (null == info)
                    continue;

                //移除并领取奖励
                if (info.getId() == _id)
                {
                    //移除数据
                    _m_alOfflineRewardList.remove(i);
                    info.del();
                    //推送客户端移除离线奖励消息
                    getUserData().sendMsgToGC(US2GCWriter_007_CommOp.make_054_OnOfflineRewardDel(_id));

                    //设置领取奖励成功
                    result.setResult(Result.SUCC);
                    //领取奖励
                    info.getDealer().takeReward(info, _context, result);
                    
                    return result;
                }
            }

            //设置领取奖励失败
            result.setResult(PlayerErr.OFFLINE_REWARD_TAKE_FAIL);
            
            return result;
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }
}
