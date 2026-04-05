package NPUSServer.NPUSUserMgr.UserComp.FriendComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.Common_LongList;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.ApplyMgr.FriendApplyMgr;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendMgr.FriendMgr;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.GroupMgr.FriendGroupMgr;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.UserCounterMgr.UserCounterMgr;
import USDB.Bo.PlayerFriendMgrBO;

import java.util.List;

public class FriendComponent extends _ANPUserComponent implements _IHandlerHolder
{
    //管理器数据
    private long _m_mgrBoDbId;
    //好友申请数据管理
    private FriendApplyMgr _m_mgrFriendApplyMgr;
    //好友数据管理
    private FriendMgr _m_mgrFriendMgr;
    //好友数据管理
    private FriendGroupMgr _m_mgrFriendGroupMgr;

    public FriendComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.FRIEND);

        _m_mgrFriendMgr = new FriendMgr(this);
        _m_mgrFriendApplyMgr = new FriendApplyMgr(this);
        _m_mgrFriendGroupMgr = new FriendGroupMgr(this);
    }

    public long getMgrBoDbId()
    {
        return _m_mgrBoDbId;
    }

    public FriendApplyMgr getFriendApplyMgr()
    {
        return _m_mgrFriendApplyMgr;
    }

    public FriendMgr getFriendMgr()
    {
        return _m_mgrFriendMgr;
    }

    public FriendGroupMgr getFriendGroupMgr()
    {
        return _m_mgrFriendGroupMgr;
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("friend_comp_init");
        //1.初始化管理器数据
        process.addResDelegateProcess(_doneAction -> initMgrFromDB(_doneAction::dealAction),
                "friend_mgr_init", null, false);
        //2.初始化好友列表
        process.addResDelegateProcess(_doneAction -> _m_mgrFriendMgr.initFromDB(_doneAction::dealAction),
                "friend_init", null, false);
        //3.初始化好友申请
        process.addResDelegateProcess(_doneAction -> _m_mgrFriendApplyMgr.initFromDB(_doneAction::dealAction),
                "friend_apply_init", null, false);
        //4.初始化好友分组
        process.addResDelegateProcess(_doneAction -> _m_mgrFriendGroupMgr.initFromDB(_doneAction::dealAction),
                "friend_group_init", null, false);

        //开启执行
        process.dealProcess(new _IEZProcessMonitor()
        {
            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
                getUserData().setDataLoadFail();
            }

            //正常结束的事件函数
            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 初始化管理器数据
     * @param _callback 回调
     */
    private void initMgrFromDB(_ICallBackBool _callback)
    {
        getUSServer().getBM().getBM(PlayerFriendMgrBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerFriendMgrBO>>()
        {
            @Override
            public void dealSuc(List<PlayerFriendMgrBO> _boList)
            {
                PlayerFriendMgrBO bo;
                if (_boList.size() > 0)
                {
                    bo = _boList.get(0);
                }else
                {
                    //插入新的数据
                    bo = new PlayerFriendMgrBO();
                    bo.setCid(getUSServer().getBM(), getUserData().getCid());
                    //先把默认分组的id插入
                    Common_LongList longList = new Common_LongList();
                    longList.addValueList(0);
                    bo.setOrderList(getUSServer().getBM(), longList.makePackage().array());
                    bo.insert(getUSServer().getBM());
                }

                //设置管理器数据id
                _m_mgrBoDbId = bo.getId();

                //初始化分组顺序
                _m_mgrFriendGroupMgr.initGroupOrderFromDb(bo);

                _callback.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callback.onRunOver(false);
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
        UserCounterMgr counterMgr = getUserData().getUSServer().getUserCounterMgr();

        //更新好友上限计数
        int friendLimit = (int) getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.FRIEND_NUM);
        counterMgr.setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND_LIMIT, friendLimit);
        //好友申请计数
        counterMgr.setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND_APPLY, getFriendApplyMgr().getApplyCount());
        //更新好友计数
        counterMgr.ensure(getUserData().getCid()).setCounter(EPlayerCounterEnum.FRIEND, getUserData().getFriendComponent().getFriendMgr().getFriendCount());

        //监听好友上限变更
        getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().addHandler(this, new HandlerThree<ENPPlayerPropertyType, Long, Long>()
        {
            @Override
            public void handle(ENPPlayerPropertyType _propertyType, Long _preValue, Long _value)
            {
                if (_propertyType == ENPPlayerPropertyType.FRIEND_NUM)
                {
                    counterMgr.setPlayerCounter(getUserData().getCid(), EPlayerCounterEnum.FRIEND_LIMIT, _value);
                }
            }
        });
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
    }
}
