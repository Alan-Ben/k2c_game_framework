package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Treasure;

import Common.TreasureHuntObj.TreasureHunt_CaptureResult_Treasure;
import Common.TreasureHuntObj.TreasureHunt_TreasureInfo;
import Common.TreasureHuntObj.TreasureHunt_TreasureOutputInfo;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntArea;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasure;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntTreasureOutput;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent.SkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerTreasureHuntTreasureBO;
import USDB.Bo.PlayerTreasureHuntTreasureOutputBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class TreasureHuntTreasureMgr
{
    private TreasureHuntComponent _m_comp;
    private HashMap<Long, TreasureHuntTreasureInfo> _m_treasureMap;
    private HashMap<Long, TreasureHuntTreasureOutputInfo> _m_treasureOutputMap;

    public TreasureHuntTreasureMgr(TreasureHuntComponent _comp)
    {
        _m_comp = _comp;
        _m_treasureMap = new HashMap<>();
        _m_treasureOutputMap = new HashMap<>();
    }

    public TreasureHuntComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 初始化奇物数据
     * @param _handler
     */
    public void _initTreasureFromDB(_ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerTreasureHuntTreasureBO.class).findAll("cid", _m_comp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerTreasureHuntTreasureBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerTreasureHuntTreasureBO> _boList)
                    {
                        for (PlayerTreasureHuntTreasureBO bo : _boList)
                        {
                            RefTreasureHuntTreasure refTreasure = RefTreasureHuntTreasure.getMgr().get(bo.getTreasureId());
                            if (refTreasure == null)
                            {
                                USLog.error(_m_comp.getUSServer(), "TreasureHuntTreasureMgr _initTreasureFromDB refTreasure is null， cid:{} treasureId:{}",
                                        _m_comp.getUserData().getCid(), bo.getTreasureId());
                                continue;
                            }

                            TreasureHuntTreasureInfo treasureInfo = new TreasureHuntTreasureInfo(TreasureHuntTreasureMgr.this, bo, refTreasure);
                            _m_treasureMap.put(bo.getTreasureId(), treasureInfo);
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
     * 初始化奇物产出数据
     * @param _handler
     */
    public void _initTreasureOutputFromDB(_ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerTreasureHuntTreasureOutputBO.class).findAll("cid", _m_comp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerTreasureHuntTreasureOutputBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerTreasureHuntTreasureOutputBO> _boList)
                    {
                        for (PlayerTreasureHuntTreasureOutputBO bo : _boList)
                        {
                            RefTreasureHuntTreasureOutput refTreasureOutput = RefTreasureHuntTreasureOutput.getMgr().get(bo.getTreasureId());
                            if (refTreasureOutput == null)
                            {
                                USLog.error(_m_comp.getUSServer(), "TreasureHuntTreasureMgr _initTreasureOutputFromDB refTreasureOutput is null， cid:{} treasureId:{}",
                                        _m_comp.getUserData().getCid(), bo.getTreasureId());
                                continue;
                            }

                            TreasureHuntTreasureOutputInfo outputInfo = new TreasureHuntTreasureOutputInfo(refTreasureOutput, bo, TreasureHuntTreasureMgr.this);
                            _m_treasureOutputMap.put(bo.getTreasureId(), outputInfo);
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

    public long getTreasureNum()
    {
        return _m_treasureMap.size();
    }

    /**
     * 获取奇物信息
     * @param _treasureId
     * @return
     */
    public TreasureHuntTreasureInfo lookupTreasure(long _treasureId)
    {
        getUserData().lockUser();
        try
        {
            return _m_treasureMap.get(_treasureId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取奇物产出信息
     * @param _treasureId
     * @return
     */
    public TreasureHuntTreasureOutputInfo lookupTreasureOutput(long _treasureId)
    {
        getUserData().lockUser();
        try
        {
            return _m_treasureOutputMap.get(_treasureId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加奇物
     * @param _refTreasure
     * @param _context
     */
    public void addTreasure(RefTreasureHuntTreasure _refTreasure, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntTreasureInfo treasureInfo = lookupTreasure(_refTreasure.Id());
            if (treasureInfo != null)
                return;

            BM bmObj = _m_comp.getUSServer().getBM();

            PlayerTreasureHuntTreasureBO bo = new PlayerTreasureHuntTreasureBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setTreasureId(bmObj, _refTreasure.Id());
            bo.setGainTimeMs(bmObj, CommonFunc.getNowTimeMS());
            bo.insert(bmObj);

            treasureInfo = new TreasureHuntTreasureInfo(this, bo, _refTreasure);
            _m_treasureMap.put(_refTreasure.Id(), treasureInfo);

            // 通知客户端新增奇物信息
            _m_comp.getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_051_OnTreasureHuntTreasureAdd(treasureInfo.makeProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否有必定获得的奇物
     * @param _refArea
     * @return
     */
    public RefTreasureHuntTreasure checkHadMustGainTreasure(RefTreasureHuntArea _refArea)
    {
        getUserData().lockUser();
        try
        {
            for (long treasureId : _refArea.treasure_list)
            {
                RefTreasureHuntTreasure refTreasure = RefTreasureHuntTreasure.getMgr().get(treasureId);
                if (refTreasure == null)
                {
                    USLog.error(_m_comp.getUSServer(), "TreasureHuntTreasureMgr hadMustGainTreasure refTreasure is null, cid:{}, treasureId:{}",
                            _m_comp.getUserData().getCid(), treasureId);
                    continue; // 奇物不存在
                }

                if (lookupTreasure(refTreasure.Id()) != null)
                    continue; // 已经获得的奇物不再计算

                //如果不是必定获得的奇物，则跳过
                if (!refTreasure.met_condition_must_gain)
                    continue;

                boolean canUnlock = NPPlayerConditionDealerMgr.IsEnable(refTreasure.unlock_condition, getComp().getUserData(), null);
                if (canUnlock)
                    return refTreasure; // 未满足解锁条件且不是必定获得的奇物
            }

            return null; // 没有必定获得的奇物
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取奇物
     * @param _refTreasure
     * @param _context
     * @return
     */
    public TreasureHunt_CaptureResult_Treasure gainTreasure(RefTreasureHuntTreasure _refTreasure,NPPlayerContext _context)
    {
        if (lookupTreasure(_refTreasure.Id()) != null)
        {
            USLog.error(_m_comp.getUSServer(), "TreasureHuntTreasureMgr gainTreasure treasure already gained, cid:{}, treasureId:{}",
                    _m_comp.getUserData().getCid(), _refTreasure.Id());
            return null; // 奇物已经获得
        }

        // 添加奇物到玩家
        addTreasure(_refTreasure, _context);

        TreasureHunt_CaptureResult_Treasure captureResultTreasure = new TreasureHunt_CaptureResult_Treasure();
        captureResultTreasure.setTreasureId(_refTreasure.Id());
        return captureResultTreasure;
    }

    /**
     * 产出解锁处理
     * @param _ref
     */
    public void onTreasureOutputUnlock(RefTreasureHuntTreasureOutput _ref)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntTreasureOutputInfo outputInfo = lookupTreasureOutput(_ref.Id());
            if (outputInfo != null)
                return;

            BM bmObj = _m_comp.getUSServer().getBM();
            PlayerTreasureHuntTreasureOutputBO bo = new PlayerTreasureHuntTreasureOutputBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setTreasureId(bmObj, _ref.Id());
            bo.setNextCanDrawTimeMs(bmObj, CommonFunc.getTodayZeroClockMS(0));
            bo.setNextCanDrawNum(bmObj, TreasureHuntTreasureOutputInfo.getOutputSpeed(getUserData(), _ref));
            bo.insert(bmObj);

            outputInfo = new TreasureHuntTreasureOutputInfo(_ref, bo, this);
            _m_treasureOutputMap.put(_ref.Id(), outputInfo);

            // 通知客户端新增奇物产出信息
            getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_061_OnTreasureHuntTreasureOutputChg(outputInfo.makeProto()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查所有奇物产出信息变更
     */
    public void checkAllTreasureOutputChange()
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntTreasureOutputInfo treasureInfo : _m_treasureOutputMap.values())
            {
                treasureInfo.checkOutputChange();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充列表到指定集合
     * @param _treasureList
     */
    public void fillProto(List<TreasureHunt_TreasureInfo> _treasureList)
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntTreasureInfo treasureInfo : _m_treasureMap.values())
            {
                _treasureList.add(treasureInfo.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 填充奇物产出信息到指定集合
     * @param _treasureOutputList
     */
    public void fillOutputProto(ArrayList<TreasureHunt_TreasureOutputInfo> _treasureOutputList)
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntTreasureOutputInfo treasureOutputInfo : _m_treasureOutputMap.values())
            {
                _treasureOutputList.add(treasureOutputInfo.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 收集奇物技能信息到指定列表中
     * 
     * @param _skillList 技能信息列表
     */
    public void collectSkillInfo(List<SkillInfo> _skillList)
    {
        for (TreasureHuntTreasureInfo treasureInfo : _m_treasureMap.values()) 
        {
            if (treasureInfo.getSkillInfo() != null && treasureInfo.getSkillInfo().getLevel() > 0) 
            {
                long skillId = treasureInfo.getRef().skill_id;
                int level = treasureInfo.getSkillInfo().getLevel();
                _skillList.add(new SkillInfo(skillId, level));
            }
        }
    }
}
