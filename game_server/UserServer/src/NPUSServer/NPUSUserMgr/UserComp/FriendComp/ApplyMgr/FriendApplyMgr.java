package NPUSServer.NPUSUserMgr.UserComp.FriendComp.ApplyMgr;

import Common.FriendObj.Friend_ApplyInfo;
import Common.ServerObj.ServerObj_FriendApply;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerFriendApplyBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class FriendApplyMgr
{
    //玩家组件
    private FriendComponent _m_comp;
    //申请数据Map，用于查询，key-申请玩家CID
    private HashMap<Long, FriendApplyInfo> _m_hmFriendApplyMap;
    //申请数据，用于遍历
    private ArrayList<FriendApplyInfo> _m_alFriendApplyList;

    public FriendApplyMgr(FriendComponent _comp)
    {
        _m_comp = _comp;
        
        _m_hmFriendApplyMap = new HashMap<>();
        _m_alFriendApplyList = new ArrayList<>();
    }

    public FriendComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return getComp().getUserData();}

    /**
     * 初始化数据
     * @param _handler
     */
    public void initFromDB(final _ICallBackBool _handler)
    {
        BM bmObj = _m_comp.getUSServer().getBM();

        bmObj.getBM(PlayerFriendApplyBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerFriendApplyBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load PlayerFriendApplyBO Data[cid:" + getUserData().getCid() + "]");

                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerFriendApplyBO> _boList)
            {
                for (int i = 0; i < _boList.size(); i++)
                {
                    PlayerFriendApplyBO bo = _boList.get(i);
                    if (null == bo)
                        continue;

                    //重复数据
                    if(_m_hmFriendApplyMap.containsKey(bo.getApplyCid()))
                    {
                    	bo.del(bmObj);
                    	continue;
                    }
                    
                    //构造申请数据
                    FriendApplyInfo info = new FriendApplyInfo(_m_comp, bo);
                    
                    //过期移除
                    if(info.isExpired())
                    {
                    	bo.del(bmObj);
                    	continue;
                    }
                    
                    _m_hmFriendApplyMap.put(bo.getApplyCid(), info);
                    _m_alFriendApplyList.add(info);
                }

                _handler.onRunOver(true);
            }
        });
    }
    
    /**
     * 获取好友申请数量
     * @return
     */
    public int getApplyCount()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		return _m_alFriendApplyList.size();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 构造协议对象
     * @param _list
     */
    public void makeProto(ArrayList<Friend_ApplyInfo> _list)
    {
        getUserData().lockUser();

        try
        {
            for (int i = 0; i < _m_alFriendApplyList.size(); i++)
            {
                FriendApplyInfo info = _m_alFriendApplyList.get(i);
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
     * 查找请求
     * @param _applyCid
     * @return
     */
    public FriendApplyInfo lookupApply(long _applyCid)
    {
        getUserData().lockUser();

        try
        {
            return _m_hmFriendApplyMap.get(_applyCid);
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加申请
     * @param _applyObj
     * @param _context
     * @return
     */
    public FriendApplyInfo addApply(ServerObj_FriendApply _applyObj, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            FriendApplyInfo info = lookupApply(_applyObj.getApplyCid());
            if (null == info)
            {
                BM bmObj = _m_comp.getUSServer().getBM();

                PlayerFriendApplyBO bo = new PlayerFriendApplyBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setApplyCid(bmObj, _applyObj.getApplyCid());
                bo.setApplyTimeS(bmObj, _applyObj.getApplyTs());
                bo.insert(bmObj);

                info = new FriendApplyInfo(getComp(), bo);
                
                _m_hmFriendApplyMap.put(info.getApplyCid(), info);
                _m_alFriendApplyList.add(info);
            }

            //推送协议
            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_060_OnFriendApplyChg(info.toProto()));

            //更新计数
            getUserData().getUSServer().getUserCounterMgr().setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND_APPLY, getApplyCount());
            
            return info;
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 移除好友申请
     * @param _applyCid
     * @param _context
     * @return
     */
    public FriendApplyInfo removeApply(long _applyCid, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            FriendApplyInfo info = _m_hmFriendApplyMap.remove(_applyCid);
            if(null == info)
            	return null;
            
            _m_alFriendApplyList.remove(info);
            info.del();

            //推送协议
            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_061_OnFriendApplyRemove(_applyCid));
            
            //更新好友申请计数
            getUserData().getUSServer().getUserCounterMgr().setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND_APPLY, getApplyCount());

            return info;
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }
}
