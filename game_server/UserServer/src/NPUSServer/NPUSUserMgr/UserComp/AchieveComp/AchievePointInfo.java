package NPUSServer.NPUSUserMgr.UserComp.AchieveComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.AchieveObj.Achieve_AchievePointInfo;
import Common.PlayerEnum.EPlayerEventRecordType;
import NPCommon.ErrMain.AchieveErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENCounterDealType;
import NPGameRes.Refs.Achieve.RefAchievePointStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import USDB.Bo.PlayerAchievePointBO;
import USLOGDB.Bo.LogAchievePointBO;

public class AchievePointInfo implements _IHandlerHolder
{
    //组件对象
    private AchieveComponent _m_comp;
    //成就数据
    private long _m_dbId;
    private int _m_type;
    private long _m_count;
    private int _m_hadDrawMaxStep;

    public AchievePointInfo(AchieveComponent _comp, PlayerAchievePointBO _bo)
    {
        _m_comp = _comp;

        _m_dbId = _bo.getId();
        _m_type = _bo.getType();
        _m_count = _bo.getCount();
        _m_hadDrawMaxStep = _bo.getHadDrawMaxStep();
    }

    public AchieveComponent getComp()
    {
        return _m_comp;
    }

    public int getType()
    {
        return _m_type;
    }

    public long getCount()
    {
        return _m_count;
    }

    public int getHadDrawMaxStep()
    {
        return _m_hadDrawMaxStep;
    }

    /**
     * 增加成就点
     * @param _count   增加的成就点数量
     * @param _context
     */
    public void addCount(long _count, NPPlayerContext _context)
    {
        if (_count <= 0)
            return;

        long oriCount = _m_count;
        _m_count += _count;
        _onCountChg();
        
        _log(oriCount, _context);
    }

    /**
     * 减少成就点
     * @param _count 减少的成就点数量
     */
    public void reduceCount(long _count, NPPlayerContext _context)
    {
        if (_count <= 0)
            return;

        long oriCount = _m_count;
        _m_count -= _count;
        _onCountChg();
        
        _log(oriCount, _context);
    }

    /**
     * 成就点变更时处理
     */
    private void _onCountChg()
    {
        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_067_OnAchievePointChg(toProto()));

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("count", _m_count);
        getComp().getUSServer().getBM().getBM(PlayerAchievePointBO.class).update("id", _m_dbId, updateValue);
    }
    
    /**
     * 成就点数据变化日志
     * @param _oriCount
     * @param _context
     */
    private void _log(long _oriCount, NPPlayerContext _context)
    {
    	LogAchievePointBO logBo = new LogAchievePointBO();
    	logBo.setAchieveType(getComp().getUSServer().getBM(), _m_type);
    	logBo.setOriCount(getComp().getUSServer().getBM(), _oriCount);
    	logBo.setCurValue(getComp().getUSServer().getBM(), _m_count);
    	CommLogDB.log(getComp().getUSServer().getBM(), logBo, _context);
    }

    /**
     * 是否有领取过成就点阶段奖励
     * @param _step 阶段
     * @return true:已领取过 false:未领取过
     */
    public boolean hasDrawStep(int _step)
    {
        return _m_hadDrawMaxStep >= _step;
    }

    /**
     * 领取成就点阶段奖励
     * @param _ref     配置数据
     * @param _context 玩家上下文
     * @return 成功返回suc, 失败返回错误码
     */
    public Result drawStepReward(RefAchievePointStep _ref, NPPlayerContext _context)
    {
        //检查是否已经领取过
        if (hasDrawStep(_ref.step_id))
            return AchieveErr.ACHIEVE_POINT_STEP_REWARD_HAD_DRAW;

        //保存已经领取过的步骤
        saveHadDrawStep(_ref.step_id);

        getComp().getUserData().gainItemList(_ref.reward_item_list, _context);

        //记录领取成就点数奖励次数
        NPSynPlayerEvnetRecordTask.syncRecord(getComp().getUserData(), ENCounterDealType.ADD,
                EPlayerEventRecordType.ACHIEVE_POINT_REWARDED.ordinal(), getType(), 1);

        return Result.SUCC;
    }

    /**
     * 领取成就点阶段奖励
     * @param _step 阶段
     */
    public void saveHadDrawStep(int _step)
    {
        _m_hadDrawMaxStep = _step;

        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_067_OnAchievePointChg(toProto()));

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("had_draw_max_step", _m_hadDrawMaxStep);
        getComp().getUSServer().getBM().getBM(PlayerAchievePointBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 构造成就点信息协议
     */
    public Achieve_AchievePointInfo toProto()
    {
        Achieve_AchievePointInfo proto = new Achieve_AchievePointInfo();
        proto.setType(_m_type);
        proto.setCount(_m_count);
        proto.setHadDrawMaxStep(_m_hadDrawMaxStep);
        return proto;
    }

    public void reset()
    {
        _m_hadDrawMaxStep = 0;

        //推送协议
        getComp().getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_067_OnAchievePointChg(toProto()));

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("had_draw_max_step", 0);
        getComp().getUSServer().getBM().getBM(PlayerAchievePointBO.class).update("id", _m_dbId, updateValue);
    }
}
