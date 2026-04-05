package NPUSServer.NPUSUserMgr.UserComp.ForbidChatComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_ForbidChatInfo;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerForbidChatBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 玩家禁言组件
 * @author mj
 *
 */
public class ForbidChatComponent extends _ANPUserComponent
{
    // 禁言记录列表
    private ArrayList<PlayerForbidChatBO> _m_alForbidChatBoList;

	public ForbidChatComponent(NPUSUserData _userData)
	{
		super(_userData, ENPPlayerCompType.FORBID_CHAT);

        _m_alForbidChatBoList = new ArrayList<>();
	}

	@Override
	protected void _init() 
	{
        getUSServer().getBM().getBM(PlayerForbidChatBO.class).findAll("cid", getUserData().getCid(),
        		new _ASelectCallback<List<PlayerForbidChatBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load forbid chat Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerForbidChatBO> _list)
            {
            	_initFromBoList(_list);

                setInited();
            }
        });
    }

	private void _initFromBoList(List<PlayerForbidChatBO> _boList)
	{
        long nowTime = CommonFunc.getNowTimeMS();

		for(int i = _boList.size() - 1; i >= 0; i--)
		{
            PlayerForbidChatBO bo = _boList.get(i);
			if(null == bo)
				continue;

            // 如果禁言时间已到，则删除记录，只在初始化数据时做一次检查
            if(bo.getEndMs() != -1 && bo.getEndMs() <= nowTime)
            {
                bo.del(getUserData().getUSServer().getBM());
                continue;
            }

            _m_alForbidChatBoList.add(bo);
		}
	}

	@Override
	public ENPPlayerCompType[] getDependCompList() 
	{
		return null;
	}

	@Override
	public void onInited() 
	{
	}

	@Override
	public void dispose() 
	{
	}

    /**
     * 构造协议对象列表
     * @param _list
     */
    public void makeProto(ArrayList<NPCommon_ForbidChatInfo>_list)
    {
        getUserData().lockUser();

        try
        {
            for (int i = 0; i < _m_alForbidChatBoList.size(); i++)
            {
                PlayerForbidChatBO bo = _m_alForbidChatBoList.get(i);
                if (null == bo)
                    continue;

                NPCommon_ForbidChatInfo info = new NPCommon_ForbidChatInfo();
                info.setRoomType(bo.getRoomType());
                info.setEndMs(bo.getEndMs());

                _list.add(info);
            }
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 是否禁言
     * @param _roomType
     * @return
     */
    public boolean isForbidChat(int _roomType)
    {
        getUserData().lockUser();

        try
        {
            long nowTime = CommonFunc.getNowTimeMS();
            for(int i = 0; i < _m_alForbidChatBoList.size(); i++)
            {
                PlayerForbidChatBO bo = _m_alForbidChatBoList.get(i);
                if(null == bo)
                    continue;

                //检查禁言截至时间 -1表示永久
                if(bo.getEndMs() != -1 && bo.getEndMs() <= nowTime)
                    continue;

                // 如果是全禁言或者禁言类型与当前聊天类型相同，则返回true
                if(bo.getRoomType() == 0 || bo.getRoomType() == _roomType)
                    return true;
            }

            return false;
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查询禁言记录
     * @param _roomType
     * @return
     */
    public PlayerForbidChatBO lookup(int _roomType)
    {
        getUserData().lockUser();

        try
        {
            for(int i = 0; i < _m_alForbidChatBoList.size(); i++)
            {
                PlayerForbidChatBO bo = _m_alForbidChatBoList.get(i);
                if(null == bo)
                    continue;

                if(bo.getRoomType() == _roomType)
                    return bo;
            }

            return null;
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置禁言相关配置，房间类型，禁言截至时间
     * @param _roomType
     * @param _endMs
     */
    public void setForbidChat(int _roomType, long _endMs)
    {
        getUserData().lockUser();

        try
        {
            PlayerForbidChatBO bo = lookup(_roomType);
            if(null == bo)
            {
                bo = new PlayerForbidChatBO();
                bo.setCid(getBM(), getUserData().getCid());
                bo.setRoomType(getBM(), _roomType);
                bo.setEndMs(getBM(), _endMs);
                bo.insert(getBM());

                _m_alForbidChatBoList.add(bo);
            }
            else
            {
                bo.saveEndMs(getBM(), _endMs);
            }

            // 推送消息到GC
            getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_068_OnForbidChatChg(bo));
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 解除禁言
     * @param _roomType
     */
    public void unsetForbidChat(int _roomType)
    {
        getUserData().lockUser();

        try
        {
            for(int i = _m_alForbidChatBoList.size() - 1; i >= 0; i--)
            {
                PlayerForbidChatBO bo = _m_alForbidChatBoList.get(i);
                if(null == bo)
                    continue;

                if(_roomType == 0 || bo.getRoomType() == _roomType)
                {
                    bo.del(getBM());
                    _m_alForbidChatBoList.remove(i);

                    // 推送消息到GC
                    getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_069_OnRemoveForbidChat(bo.getRoomType()));
                }
            }
        }
        finally
        {
            getUserData().unlockUser();
        }
    }
}
