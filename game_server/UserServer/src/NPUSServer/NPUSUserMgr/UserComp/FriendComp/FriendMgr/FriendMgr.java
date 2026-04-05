package NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendMgr;

import Common.FriendObj.Friend_Info;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.ENpLogType;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerFriendBO;
import USLOGDB.Bo.LogFriendBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class FriendMgr
{
    //玩家组件
    private FriendComponent _m_comp;
    //好友数据 key-好友玩家CID
    private HashMap<Long, FriendInfo> _m_hmFriendMap;
    //好友列表
    private ArrayList<FriendInfo> _m_alFriendList;

    public FriendMgr(FriendComponent _comp)
    {
        _m_comp = _comp;
        
        _m_hmFriendMap = new HashMap<>();
        _m_alFriendList = new ArrayList<>();
    }

    public FriendComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return getComp().getUserData();}

    /**
     * 初始化数据
     * @param _handler
     */
    public void initFromDB(final _ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerFriendBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerFriendBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load PlayerFriendBO Data[cid:" + getUserData().getCid() + "]");

                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerFriendBO> _boList)
            {
                for (int i = 0; i < _boList.size(); i++)
                {
                    PlayerFriendBO bo = _boList.get(i);
                    if (null == bo)
                        continue;

                    //已经存在的数据，不再重复加入
                    if (_m_hmFriendMap.containsKey(bo.getFcid()))
                    {
                        USLog.error(_m_comp.getUSServer(), "Repeat PlayerFriendBO, cid:{} id:{} fcid:{}"
                                , getUserData().getCid(), bo.getId(), bo.getFcid());
                        continue;
                    }

                    FriendInfo info = new FriendInfo(_m_comp, bo);
                    _m_hmFriendMap.put(bo.getFcid(), info);
                    _m_alFriendList.add(info);
                }
                
                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 构造协议对象
     * @param _list
     */
    public void makeProto(ArrayList<Friend_Info> _list)
    {
        getUserData().lockUser();

        try
        {
            for (int i = 0; i < _m_alFriendList.size(); i++)
            {
            	FriendInfo info = _m_alFriendList.get(i);
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
     * 好友数量
     * @return
     */
    public int getFriendCount()
    {
        getUserData().lockUser();

        try
        {
            return _m_alFriendList.size();
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 根据好友cid查询数据
     * @param _fcid
     * @return
     */
    public FriendInfo lookupByFCid(long _fcid)
    {
        getUserData().lockUser();

        try
        {
            return _m_hmFriendMap.get(_fcid);
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加好友
     * @param _fcid
     * @param _isAgree
     * @param _context
     */
    public void addFriend(long _fcid, boolean _isAgree, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            FriendInfo info = _m_hmFriendMap.get(_fcid);
            if (null == info)
            {
                BM bmObj = _m_comp.getUSServer().getBM();

                //增加好友
                PlayerFriendBO bo = new PlayerFriendBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setFcid(bmObj, _fcid);
                bo.insert(bmObj);

                info = new FriendInfo(_m_comp, bo);
                _m_hmFriendMap.put(_fcid, info);
                _m_alFriendList.add(info);
            }

            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_063_OnFriendChg(info.toProto(), _isAgree));
            
            //更新好友计数
            getUserData().getUSServer().getUserCounterMgr().setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND, getFriendCount());

            //记录玩家好友数量历史记录最高值
            getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.FRIEND_NUM_MAX_RECORD, 1, _context);
            
            //日志数据
            LogFriendBO logBo = new LogFriendBO();
            logBo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
            logBo.setLogType(getUserData().getUSServer().getBM(), ENpLogType.ADD.ordinal());
            logBo.setFriendCid(getUserData().getUSServer().getBM(), _fcid);
            logBo.setIsAgree(getUserData().getUSServer().getBM(), _isAgree);
            CommLogDB.log(getUserData().getUSServer().getBM(), logBo, _context);
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 移除好友
     * @param _fcid
     * @param _context
     * @return
     */
    public boolean removeFriend(long _fcid, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
        	FriendInfo removeInfo = _m_hmFriendMap.remove(_fcid);
        	if(null == removeInfo)
        		return false;
        	
        	_m_alFriendList.remove(removeInfo);

            HashMap<String, Object> condMap = new HashMap<>();
            condMap.put("cid", getUserData().getCid());
            condMap.put("fcid", _fcid);
            _m_comp.getUSServer().getBM().getBM(PlayerFriendBO.class).delAll(condMap);

            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_064_OnFriendRemove(_fcid));
            
            //更新玩家好友计数
            getUserData().getUSServer().getUserCounterMgr().setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND, getFriendCount());

            //日志数据
            LogFriendBO logBo = new LogFriendBO();
            logBo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
            logBo.setLogType(getUserData().getUSServer().getBM(), ENpLogType.DEL.ordinal());
            logBo.setFriendCid(getUserData().getUSServer().getBM(), _fcid);
            CommLogDB.log(getUserData().getUSServer().getBM(), logBo, _context);
        } 
        finally
        {
            getUserData().unlockUser();
        }

        //从好友分组中移除
        _m_comp.getFriendGroupMgr().onFriendRemove(_fcid);

        return true;
    }

    /**
     * 获取所有好友的cid列表
     * @return
     */
    public List<Long> getAllFriendCidList()
    {
        getUserData().lockUser();
        try{
            List<Long> friendCidList = new ArrayList<>();
            for (FriendInfo friendInfo : _m_alFriendList)
            {
                friendCidList.add(friendInfo.getFCid());
            }
            return friendCidList;
        }finally
        {
            getUserData().unlockUser();
        }
    }
}
