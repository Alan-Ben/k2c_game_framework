package NPUSServer.NPUSUserMgr.UserComp.PushGiftPackComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.PushGiftObj.PushGift_GroupInfo;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.PushGift.RefPushGiftGroup;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerPushGiftGroupBO;

import java.util.ArrayList;
import java.util.List;

public class PushGiftComponent extends _ANPUserComponent
{
    private List<PushGiftGroupInfo> _m_groupList;

    public PushGiftComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.PUSH_GIFT_PACK);
        _m_groupList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("push_gift_pack_component_init");

        // 步骤1: 加在礼包组数据
        process.addResDelegateProcess(action -> _initGroupInfoFromDB(action::dealAction),
                "init_group_info", null, false);

        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "PushGiftPackComponent._init - component init failed: cid={}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 加载礼包组状态数据
     * <p>
     * 执行流程：
     * 1. 从数据库查询所有group_state记录
     * 2. 为每个记录创建GroupState对象
     * 3. 放入_m_groupStateMap映射中
     * @param _callback 加载完成回调
     */
    private void _initGroupInfoFromDB(_ICallBackBool _callback)
    {
        getBM().getBM(PlayerPushGiftGroupBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerPushGiftGroupBO>>()
        {
            @Override
            public void dealSuc(List<PlayerPushGiftGroupBO> _boList)
            {
                for (PlayerPushGiftGroupBO bo : _boList)
                {
                    // 获取配置
                    RefPushGiftGroup refGroup = RefPushGiftGroup.getMgr().get(bo.getGroupId());
                    if (refGroup == null)
                    {
                        USLog.error(getUSServer(),
                                "PushGiftPackComponent._initGroupStateFromDB - ref not found: cid={}, groupId={}",
                                getCid(), bo.getGroupId());
                        continue;
                    }

                    _m_groupList.add(new PushGiftGroupInfo(PushGiftComponent.this, refGroup, bo));
                }

                _callback.onRunOver(true);
            }

            @Override
            public void dealFail()
            {
                _callback.onRunOver(false);
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        // 组件初始化完成后的处理
    }

    @Override
    public void dispose()
    {

    }

    /**
     * 查找礼包组信息
     * @param _groupId
     * @return
     */
    public PushGiftGroupInfo lookupGroupInfo(long _groupId)
    {
        getUserData().lockUser();
        try
        {
            for (PushGiftGroupInfo groupInfo : _m_groupList)
            {
                if (groupInfo.getGroupId() == _groupId)
                {
                    return groupInfo;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 确保礼包组状态存在，不存在则创建
     * @param _ref 礼包组配置
     * @return 礼包组状态
     */
    public PushGiftGroupInfo ensureGroupInfo(RefPushGiftGroup _ref)
    {
        getUserData().lockUser();
        try
        {
            PushGiftGroupInfo groupInfo = lookupGroupInfo(_ref.Id());
            if (groupInfo == null)
            {
                PlayerPushGiftGroupBO bo = new PlayerPushGiftGroupBO();
                bo.setCid(getBM(), getCid());
                bo.setGroupId(getBM(), _ref.Id());
                bo.insert(getBM());

                groupInfo = new PushGiftGroupInfo(this, _ref, bo);
                _m_groupList.add(groupInfo);
            }
            return groupInfo;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 触发礼包组
     * @param _groupId 礼包组ID
     * @param _context 操作上下文
     * @return 成功返回礼包组信息，失败返回错误码
     */
    public Result triggerGroup(long _groupId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            RefPushGiftGroup refGroup = RefPushGiftGroup.getMgr().get(_groupId);
            if (refGroup == null)
                return CommErr.REF_NOT_FOUND;

            return ensureGroupInfo(refGroup).tryTrigger(false, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 标记推送礼包为已读
     * @param _groupId    礼包组ID
     * @param _pushGiftId 推送礼包ID
     * @return 成功返回null，失败返回错误码
     */
    public Result markAsRead(long _groupId, long _pushGiftId)
    {
        getUserData().lockUser();
        try
        {
            PushGiftGroupInfo groupInfo = lookupGroupInfo(_groupId);
            if (groupInfo == null)
                return CommErr.REF_NOT_FOUND;

            return groupInfo.markAsRead(_pushGiftId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造礼包组协议对象列表
     * @return
     */
    public List<PushGift_GroupInfo> makeGroupProtoList()
    {
        getUserData().lockUser();
        try
        {
            List<PushGift_GroupInfo> groupList = new ArrayList<>();
            for (PushGiftGroupInfo groupInfo : _m_groupList)
            {
                groupList.add(groupInfo.makeProto());
            }
            return groupList;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否可以购买礼包
     * @param _giftPackId
     * @return
     */
    public boolean canBuyGiftPack(long _giftPackId)
    {
        getUserData().lockUser();
        try
        {
            for (PushGiftGroupInfo groupInfo : _m_groupList)
            {
                if (groupInfo.canBuyGiftPack(_giftPackId))
                    return true;
            }
            return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 购买礼包后的处理
     * @param _giftPackId
     * @param _context
     */
    public void onOrderDelivery(long _giftPackId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            for (PushGiftGroupInfo groupInfo : _m_groupList)
            {
                groupInfo.onOrderDelivery(_giftPackId, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }
}
