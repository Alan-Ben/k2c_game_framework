package NPUSServer.NPUSUserMgr.UserComp.LoverCollectComp;

import ALBasicServer.ALProcess.ALProcess;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.LoverCollectErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerLoverCollectBO;

import java.util.List;

/**
 * 情人收集组件
 *
 * 主要功能：
 * 1. 管理玩家选中的目标情人
 * 2. 处理选择目标情人请求（仅可选择一次，不可修改）
 * 3. 达到赚速要求后领取情人
 * 4. 数据变更后推送客户端
 */
public class LoverCollectComponent extends _ANPUserComponent
{
    // 数据库ID，0表示未插入数据库
    private long _m_dbId;

    // 选中的情人配置ID，0=未选择
    private long _m_targetLoverId;

    // 是否已领取当前目标情人
    private boolean _m_isClaimed;

    /**
     * 构造函数
     */
    public LoverCollectComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.LOVER_COLLECT);
        _m_dbId = 0;
        _m_targetLoverId = 0;
        _m_isClaimed = false;
    }

    /**
     * 组件初始化
     * 异步加载数据库数据
     */
    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("lover_collect_init");

        process.addResDelegateProcess(
                action -> _initDataFromDB(action::dealAction),
                "init_data",
                () -> USLog.error(getUSServer(), "LoverCollectComponent._init - load data failed, cid={}", getUserData().getCid()),
                false);

        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "LoverCollectComponent._init - process stopped, cid={}", getUserData().getCid());
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
     * 从数据库加载数据
     */
    private void _initDataFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerLoverCollectBO.class).findAll(
                "cid", getUserData().getCid(),
                new _ASelectCallback<List<PlayerLoverCollectBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerLoverCollectBO> _boList)
                    {
                        if (!_boList.isEmpty())
                        {
                            PlayerLoverCollectBO bo = _boList.get(0);
                            _m_dbId = bo.getId();
                            _m_targetLoverId = bo.getTargetLoverId();
                            _m_isClaimed = bo.getIsClaimed();
                        }
                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    /**
     * 尝试在数据库创建记录（懒加载）
     * @return true=首次创建，false=已存在
     */
    private boolean tryCreateInDB()
    {
        getUserData().lockUser();
        try
        {
            if (_m_dbId != 0)
                return false;

            PlayerLoverCollectBO bo = new PlayerLoverCollectBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setTargetLoverId(getUSServer().getBM(), _m_targetLoverId);
            bo.setIsClaimed(getUSServer().getBM(), _m_isClaimed);
            bo.insert(getUSServer().getBM());

            _m_dbId = bo.getId();
            return true;
        } finally
        {
            getUserData().unlockUser();
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
     * 选择目标情人
     * 情人一旦选择后不可修改，loverId须在可选列表内
     */
    public Result setLoverTarget(long _loverId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 已选择过目标，不可修改
            if (_m_targetLoverId != 0)
                return LoverCollectErr.ALREADY_HAS_TARGET;

            // 校验情人ID是否在可选列表中
            List<Long> loverIds = RefGeneral.Ref().lover_collect_lover_ids;
            if (loverIds == null || !loverIds.contains(_loverId))
                return LoverCollectErr.INVALID_LOVER_ID;

            _m_targetLoverId = _loverId;
            _m_isClaimed = false;

            // 保存数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("target_lover_id", _m_targetLoverId);
                getUSServer().getBM().getBM(PlayerLoverCollectBO.class).update("id", _m_dbId, updateValue);
            }

            pushInfoChange();
            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取情人
     * 校验：已选择目标、未领取、当前赚速达到要求
     */
    public Result claimLover(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 未选择目标
            if (_m_targetLoverId == 0)
                return LoverCollectErr.NO_TARGET;

            // 已领取
            if (_m_isClaimed)
                return LoverCollectErr.ALREADY_CLAIMED;

            // 赚速不足
            long needEarnSpeed = RefGeneral.Ref().lover_collect_need_earn_speed;
            if (getUserData().getPlayerComponent().getEarnings() < needEarnSpeed)
                return LoverCollectErr.EARN_SPEED_NOT_ENOUGH;

            // 发放情人道具
            getUserData().gainItem(ENPItemType.CONSORT, _m_targetLoverId, 1, _context);

            _m_isClaimed = true;

            // 保存数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("is_claimed", _m_isClaimed ? 1 : 0);
                getUSServer().getBM().getBM(PlayerLoverCollectBO.class).update("id", _m_dbId, updateValue);
            }

            pushInfoChange();
            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 推送数据变更给客户端
     */
    private void pushInfoChange()
    {
        getUserData().lockUser();
        try
        {
            getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_076_OnLoverCollectChg(_m_targetLoverId, _m_isClaimed));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public long getTargetLoverId()
    {
        return _m_targetLoverId;
    }

    public boolean isIsClaimed()
    {
        return _m_isClaimed;
    }
}
