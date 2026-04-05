package NPUSServer.StepReward;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.Util.Pair.WCGPairLong;
import NPUSServer.NPUserServer;
import USDB.Bo.UsStepRewardObjBO;

/**
 * 阶段奖励数据对象
 */
public class StepRewardScoreObj
{
    private StepRewardObj _m_stepRewardObj;

    private long _m_dbId;
    private long _m_cid;
    private long _m_score;

    public StepRewardScoreObj(StepRewardObj _stepRewardobj, UsStepRewardObjBO _stepRewardBo)
    {
    	_m_stepRewardObj = _stepRewardobj;
        _m_dbId = _stepRewardBo.getId();
        _m_cid = _stepRewardBo.getCid();
        _m_score = _stepRewardBo.getScore();
    }

    public StepRewardObj getStepRewardObj() {return _m_stepRewardObj;}
    public StepRewardList getStepRewardList() {return getStepRewardObj().getStepRewardList();}
    public NPUserServer getUSServer(){return getStepRewardList().getUSServer();}
    
    public long getCid() {return _m_cid;}
    public long getScore() {return _m_score;}

    /**
     * 设置分数
     * @param _chgScore 新的分数
     * @param _setGreater 是否设置更大的分数
     */
    public void setScore(long _chgScore, boolean _setGreater)
    {
        //如果设置更大的分数，但是当前分数已经比设置的分数大，则不设置
        if (_setGreater && _m_score >= _chgScore)
            return;

        _m_score = _chgScore;

        _saveToDb();
    }

    /**
     * 增加分数
     * @param _chgScore 增加的分数
     */
    public void chgScore(long _chgScore)
    {
        _m_score += _chgScore;

        _saveToDb();
    }

    /**
     * 保存到数据库
     */
    protected void _saveToDb()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("score", _m_score);
        getUSServer().getBM().getBM(UsStepRewardObjBO.class).update("id", _m_dbId, updateValue);
    }

    /**
     * 构造玩家得分信息
     */
    public WCGPairLong getScoreInfo()
    {
        return new WCGPairLong(_m_cid, _m_score);
    }
}
