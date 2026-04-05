package NPUSServer.NPUSUserMgr.UserComp.QuestComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.QuestObj.Quest_Count;
import USDB.Bo.PlayerQuestCountBO;

public class QuestCountInfo
{
    //任务组件
    private PlayerQuestComponent _m_comp;
    //任务完成数据
    private long _m_lDbid; //dbid
    private long _m_lQuestId; //任务配置ID
    private int _m_iDoneCount; //任务完成次数

    protected QuestCountInfo(PlayerQuestComponent _comp, PlayerQuestCountBO _bo)
    {
        _m_comp = _comp;

        _m_lDbid = _bo.getId();
        _m_lQuestId = _bo.getQuestId();
        _m_iDoneCount = _bo.getDoneCount();
    }

    public PlayerQuestComponent getComp()
    {
        return _m_comp;
    }

    public long getQuestId()
    {
        return _m_lQuestId;
    }

    public int getDoneCount()
    {
        return _m_iDoneCount;
    }

    /**
     * 构造协议对象
     * @return
     */
    public Quest_Count toProto()
    {
        Quest_Count proto = new Quest_Count();
        proto.setQuestId(_m_lQuestId);
        proto.setDoneCount(_m_iDoneCount);

        return proto;
    }

    /**
     * 增加任务开启计数
     * @param _chgCount
     */
    protected void incrDoneCount(int _chgCount)
    {
        _m_iDoneCount += _chgCount;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("done_count", _m_iDoneCount);
        _m_comp.getUSServer().getBM().getBM(PlayerQuestCountBO.class).update("id", _m_lDbid, updateValue);
    }

    /**
     * 设置进攻次数
     * @param _count
     * @return
     */
    protected int setDoneCount(int _count)
    {
        int chgCount = _count - _m_iDoneCount;

        _m_iDoneCount = _count;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("done_count", _m_iDoneCount);
        _m_comp.getUSServer().getBM().getBM(PlayerQuestCountBO.class).update("id", _m_lDbid, updateValue);

        return chgCount;
    }

    public void deal()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerQuestCountBO.class).delAll("id", _m_lDbid);
    }
}
