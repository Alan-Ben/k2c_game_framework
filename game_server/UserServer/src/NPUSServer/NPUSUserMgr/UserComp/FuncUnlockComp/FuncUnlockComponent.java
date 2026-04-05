package NPUSServer.NPUSUserMgr.UserComp.FuncUnlockComp;

import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPFunctionType;
import NPGameRes.Refs.RefFuncUnlock;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerFuncUnlockBO;

import java.util.ArrayList;
import java.util.List;

public class FuncUnlockComponent extends _ANPUserComponent
{
    //解锁信息列表
    private List<FuncUnlockInfo> _m_unlockList;

    public FuncUnlockComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.FUNC_UNLOCK);

        _m_unlockList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerFuncUnlockBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerFuncUnlockBO>>()
        {
            @Override
            public void dealSuc(List<PlayerFuncUnlockBO> _boList)
            {
                for (PlayerFuncUnlockBO bo : _boList)
                {
                    ENPFunctionType type = ENPFunctionType.ENPFunctionType_FromInt(bo.getFuncType());
                    if (type == null)
                    {
                        USLog.error(getUSServer(), "FuncUnlockComponent._init - enum conversion failed: cid={}, funcType={}", getUserData().getCid(), bo.getFuncType());
                        continue;
                    }

                    FuncUnlockInfo info = new FuncUnlockInfo(FuncUnlockComponent.this, bo);
                    _m_unlockList.add(info);
                }

                setInited();
            }

            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "FuncUnlockComponent._init - load data failed: cid={}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }
        });
    }

    @Override
    public NPCommonEnum.ENPPlayerCompType[] getDependCompList()
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
     * 查找解锁信息
     * @param _type
     * @return
     */
    public FuncUnlockInfo lookupUnlockInfo(ENPFunctionType _type)
    {
        getUserData().lockUser();
        try
        {
            for (FuncUnlockInfo info : _m_unlockList)
            {
                if (info.getFuncType() == _type)
                {
                    return info;
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
        return null;
    }

    /**
     * 确保存在解锁信息
     * @param _type
     * @return
     */
    public FuncUnlockInfo ensureUnlockInfo(ENPFunctionType _type)
    {
        getUserData().lockUser();
        try
        {
            FuncUnlockInfo info = lookupUnlockInfo(_type);
            if (info == null)
            {
                PlayerFuncUnlockBO bo = new PlayerFuncUnlockBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setFuncType(getUSServer().getBM(), _type.ordinal());
                bo.setIsClientNotified(getUSServer().getBM(), false);
                bo.setNotDrawReward(getUSServer().getBM(), true);
                bo.insert(getUSServer().getBM());

                info = new FuncUnlockInfo(this, bo);
                _m_unlockList.add(info);
            }

            return info;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 解锁功能
     * @param _type 功能类型
     * @return 解锁结果
     */
    public Result unlock(ENPFunctionType _type, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //查找对应配置
            RefFuncUnlock refFuncUnlock = RefFuncUnlock.getMgr().get(_type.ordinal());
            if (refFuncUnlock == null)
                return CommErr.REF_NOT_FOUND;

            //条件检查
            if (!NPPlayerConditionDealerMgr.IsEnable(refFuncUnlock.simple_unlock_id, getUserData(), null))
                return CommErr.SYSTEM_UNLOCK;

            //获取解锁信息
            FuncUnlockInfo unlockInfo = ensureUnlockInfo(_type);

            //标记奖励已领取
            Result result = unlockInfo.markRewardReceived();
            if (!result.isSucc())
                return result;

            //获取奖励
            getUserData().gainItemList(refFuncUnlock.gain_item_list, _context);

            //推送解锁信息
            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_053_OnFuncUnlockDone(_type));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 移除解锁信息
     * @param _type 功能类型
     */
    public void removeUnlockInfo(ENPFunctionType _type)
    {
        getUserData().lockUser();
        try
        {
            for (FuncUnlockInfo info : _m_unlockList)
            {
                if (info.getFuncType() == _type)
                {
                    _m_unlockList.remove(info);
                    info.discard();
                    break;
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取已经解锁的功能类型列表
     */
    public List<ENPFunctionType> getUnlockTypeList()
    {
        List<ENPFunctionType> list = new ArrayList<>();
        getUserData().lockUser();
        try
        {
            for (FuncUnlockInfo info : _m_unlockList)
            {
                if (info.hadDrawReward())
                {
                    list.add(info.getFuncType());
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
        return list;
    }

    /**
     * 获取客户端通知解锁的功能类型列表
     */
    public List<ENPFunctionType> getClientNotifiedTypeList()
    {
        List<ENPFunctionType> list = new ArrayList<>();
        getUserData().lockUser();
        try
        {
            for (FuncUnlockInfo info : _m_unlockList)
            {
                if (info.isClientNotified())
                {
                    list.add(info.getFuncType());
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
        return list;
    }

    /**
     * 标记功能已通知客户端
     * 仅在功能已解锁时生效
     * @param _type 功能类型
     */
    public Result markClientNotified(ENPFunctionType _type)
    {
        getUserData().lockUser();
        try
        {
            //查找对应配置
            RefFuncUnlock refFuncUnlock = RefFuncUnlock.getMgr().get(_type.ordinal());
            if (refFuncUnlock == null)
                return CommErr.REF_NOT_FOUND;

            //条件检查
            if (!NPPlayerConditionDealerMgr.IsEnable(refFuncUnlock.simple_unlock_id, getUserData(), null))
                return CommErr.SYSTEM_UNLOCK;

            //获取解锁信息
            FuncUnlockInfo unlockInfo = ensureUnlockInfo(_type);
            unlockInfo.markClientNotified();

            //推送解锁信息
            getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_065_OnClientNotifyFuncUnlock(_type));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

}
