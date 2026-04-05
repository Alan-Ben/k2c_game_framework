package ActivitiesV01.Activities.TileMatchActivity.Game;

import ALBasicProtocolPack._IALProtocolStructure;
import ActivitiesV01.MsgDealers.US2GCWriter_201_TileMatchOp;
import ActivitiesV01.Refs.TileMatch.RefTileMatchBlock;
import ActivitiesV01.Refs.TileMatch.RefTileMatchOther;
import Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_LogicType;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.Util.Pair.WCGPairInt;
import NPCommon.Util.Pair.WCGPairIntList;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.*;

public class TileMatchGameLogicContext
{
    //逻辑序列号
    private int _m_serialId = 0;
    //玩家上下文
    private NPPlayerContext _m_context;
    //需要执行的逻辑队列
    private Deque<_ATileMatchGameLogic> _m_logicQueue;
    //需要执行的逻辑队列
    private Deque<_ATileMatchGameLogic> _m_nextLogicQueue;
    //执行结果
    private List<TileMatch_LogicInfo> _m_logicResultList;
    //消除数量计数器
    private WCGPairIntList _m_removeScoreCounter;
    //连击次数
    private int _m_comboCount = 0;
    //分数
    private long _m_score = 0;
    // 当前批次彩虹鸟已选择的类型集合（用于去重，同一批次内不重复选择相同类型）
    private Set<Integer> _m_selectedRainbowTypes = new HashSet<>();

    //失败原因
    private Result _m_failResult;

    public TileMatchGameLogicContext(NPPlayerContext _context)
    {
        _m_context = _context;
        _m_logicQueue = new ArrayDeque<>();
        _m_nextLogicQueue = new ArrayDeque<>();
        _m_logicResultList = new ArrayList<>();
        _m_removeScoreCounter = new WCGPairIntList();
    }

    public WCGPairIntList getRemoveScoreCounter()
    {
        return _m_removeScoreCounter;
    }

    public long getScore()
    {
        return _m_score;
    }

    public NPPlayerContext getContext()
    {
        return _m_context;
    }

    public int getSerialId()
    {
        return _m_serialId;
    }

    public void setSerial(int _offset)
    {
        _m_serialId = _offset;
    }

    /**
     * 获取失败原因
     * @return
     */
    public Result getFailResult()
    {
        return _m_failResult;
    }

    /**
     * 设置失败原因
     * @param _result
     */
    public void setRunFail(Result _result)
    {
        _m_failResult = _result;
    }

    /**
     * 获取逻辑
     * @return
     */
    public _ATileMatchGameLogic pollLogic()
    {
        //如果当前逻辑队列不为空，则直接返回当前逻辑队列中的逻辑
        if (_m_logicQueue.isEmpty())
        {
            //如果当前逻辑队列为空，则将下一个逻辑队列中的逻辑添加到当前逻辑队列中
            if (!_m_nextLogicQueue.isEmpty())
            {
                _m_logicQueue.addAll(_m_nextLogicQueue);
                _m_nextLogicQueue.clear();

                //增加逻辑序列号
                _m_serialId++;

                // 新批次开始，清空彩虹鸟类型选择记录
                clearRainbowSelectedTypes();
            }
        }

        return _m_logicQueue.poll();
    }

    /**
     * 增加逻辑
     * @param _logic
     */
    public void addLogic(_ATileMatchGameLogic _logic)
    {
        _m_logicQueue.add(_logic);
    }

    /**
     * 增加逻辑
     * @param _logic
     */
    public void addNextLogic(_ATileMatchGameLogic _logic)
    {
        _m_nextLogicQueue.add(_logic);
    }

    /**
     * 增加逻辑
     * @param _logic
     */
    public void insertNextLogic(_ATileMatchGameLogic _logic)
    {
        _m_nextLogicQueue.addFirst(_logic);
    }

    /**
     * 添加逻辑结果
     * @param _logicType
     * @param _score
     * @param _proto
     */
    public void addLogicResult(ETileMatch_LogicType _logicType, int _score, _IALProtocolStructure _proto)
    {
        TileMatch_LogicInfo logicInfo = new TileMatch_LogicInfo();
        logicInfo.setSerialId(_m_serialId);
        logicInfo.setLogicType(_logicType);
        logicInfo.setData(_proto.makePackage().array());
        logicInfo.setScore(_score);
        addLogicResult(logicInfo);
    }

    /**
     * 添加逻辑结果
     */
    public void addLogicResult(TileMatch_LogicInfo _logicInfo)
    {
        _m_logicResultList.add(_logicInfo);
    }

    /**
     * 推送逻辑结果到客户端
     */
    public void pushLogicResult(NPUSUserData _userData, ETileMatch_ModeType _modeType)
    {
        //将_m_logicResultList分段发送给客户端，每10个发一条
        if (_m_logicResultList.isEmpty())
            return;

        int needProtoCount = (int) Math.ceil(_m_logicResultList.size() * 1.0 / 10);
        for (int i = 0; i < needProtoCount; i++)
        {
            int startIndex = i * 10;
            int endIndex = Math.min(startIndex + 10, _m_logicResultList.size());
            _userData.sendMsgToGC(US2GCWriter_201_TileMatchOp.make_051_OnTileMatchLogicProcess(_modeType, _m_logicResultList.subList(startIndex, endIndex)));
        }
    }

    /**
     * 计算分数且记录
     * @return
     */
    public int calScoreAndRecord(TileMatchBlockCounter _removeBlockCounter)
    {
        //基础分数
        int removeBlockGainScore = 0;

        //获取连击加分
        int comboAddScore = getComboAddScore(_m_comboCount);

        WCGPairIntList removeScoreCounter = _removeBlockCounter.getBlockCountList();
        for (WCGPairInt pair : removeScoreCounter.getList())
        {
            RefTileMatchBlock refBlock = RefTileMatchBlock.getMgr().get(pair.first());
            if (refBlock == null)
            {
                CommLog.error("TileMatchPlayerGameInfo.settle, RefTileMatchBlock not found for blockId:{}", pair.first());
                continue;
            }

            removeBlockGainScore += (refBlock.basic_score + comboAddScore) * pair.second();

            //记录数量到总的计数器
            _m_removeScoreCounter.ensure(pair.first()).addSecond(pair.second());
        }

        //记录分数到总分上
        _m_score += removeBlockGainScore;

        return removeBlockGainScore;
    }

    /**
     * 获取连击加分分数
     * @return
     */
    public int getComboAddScore(int _comboCount)
    {
        List<Integer> tilematchContinuousExtScore = RefTileMatchOther.Ref().tilematch_continuous_ext_score;
        if (tilematchContinuousExtScore == null || tilematchContinuousExtScore.isEmpty())
            return 0;

        return tilematchContinuousExtScore.get(Math.min(tilematchContinuousExtScore.size() - 1, _comboCount));
    }

    public void addComboCount()
    {
        _m_comboCount ++;
    }

    /**
     * 尝试为彩虹鸟选择一个类型（带去重）
     *
     * 用于彩虹鸟被动触发时的类型去重机制，确保同一批次内的多个彩虹鸟不会选择相同的类型。
     *
     * @param _type 要选择的类型ID
     * @return true=选择成功（该类型首次被选择），false=该类型已被其他彩虹鸟选择
     */
    public boolean trySelectRainbowType(int _type)
    {
        return _m_selectedRainbowTypes.add(_type);
    }

    /**
     * 清空彩虹鸟类型选择记录
     *
     * 调用时机：逻辑序列号增加时（新批次开始）
     * 作用域：每个批次（相同序列号）内的彩虹鸟共享去重集合，不同批次独立
     */
    public void clearRainbowSelectedTypes()
    {
        _m_selectedRainbowTypes.clear();
    }
}
