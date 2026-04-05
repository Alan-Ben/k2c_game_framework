package NPUSServer.NPUSUserMgr.UserComp.QuestComp;

import Common.QuestEnum.EQuestType;
import Common.QuestObj.Quest_Count;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.Quest.RefQuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_028_QuestOp;
import USDB.Bo.PlayerQuestCountBO;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * 玩家任务完成记录
 * @author mark
 */
public class QuestCountMgr
{
    //任务组件
    private PlayerQuestComponent _m_comp;
    
    //任务完成数据
    private HashMap<Long, QuestCountInfo> _m_hmQuestCountMap;
    
    //统计主线任务数量（不统计完成次数，只统计主线任务数），用于条件/高级公式
    private int _m_iMainQuestCount;

    protected QuestCountMgr(PlayerQuestComponent _comp)
    {
        _m_comp = _comp;
        _m_hmQuestCountMap = new HashMap<>();
    }
    
    /**
     * 初始化数据
     * @param _bo
     */
    protected void initBo(PlayerQuestCountBO _bo)
    {
        QuestCountInfo info = new QuestCountInfo(_m_comp, _bo);
        _m_hmQuestCountMap.put(info.getQuestId(), info);

        //只统计主线任务数量
        _checkAndIncrMainQuest(info);
    }

    public PlayerQuestComponent getComp()
    {
        return _m_comp;
    }
    
    public int getMainQuestCount()
    {
    	return _m_iMainQuestCount;
    }

    /**
     * 构造初始化协议
     * @param _list
     */
    protected void makeProto(ArrayList<Quest_Count> _list)
    {
        for (QuestCountInfo info : _m_hmQuestCountMap.values())
        {
            if (null == info)
                continue;
            
            _list.add(info.toProto());
        }
    }

    /**
     * 增加完成计数
     * @param _questId
     * @param _chgCount
     * @param _context
     */
    protected void incrDoneCount(long _questId, int _chgCount, NPPlayerContext _context)
    {
        QuestCountInfo info = _m_hmQuestCountMap.get(_questId);
        if (null == info)
        {
            BM bmObj = _m_comp.getUSServer().getBM();

            PlayerQuestCountBO bo = new PlayerQuestCountBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setQuestId(bmObj, _questId);
            bo.setDoneCount(bmObj, _chgCount);
            bo.insert(bmObj);

            info = new QuestCountInfo(_m_comp, bo);
            _m_hmQuestCountMap.put(info.getQuestId(), info);
            
            //只统计主线任务数量
            _checkAndIncrMainQuest(info);
        } else
        {
            info.incrDoneCount(_chgCount);
        }

        //推送协议
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_053_OnPlayerQuestCountChg(info.toProto()));
    }

    /**
     * 获取玩家指定任务的完成次数
     * @param _questId
     * @return
     */
    protected int getDoneCount(long _questId)
    {
        QuestCountInfo info = _m_hmQuestCountMap.get(_questId);

        return null == info ? 0 : info.getDoneCount();
    }

    /**
     * 设置完成次数
     * @param _questId
     * @param _count
     * @param _context
     */
    protected void setDoneCount(long _questId, int _count, NPPlayerContext _context)
    {
        QuestCountInfo info = _m_hmQuestCountMap.get(_questId);
        if (null == info)
        {
            BM bmObj = _m_comp.getUSServer().getBM();

            PlayerQuestCountBO bo = new PlayerQuestCountBO();
            bo.setCid(bmObj, _m_comp.getUserData().getCid());
            bo.setQuestId(bmObj, _questId);
            bo.setDoneCount(bmObj, _count);
            bo.insert(bmObj);

            info = new QuestCountInfo(_m_comp, bo);
            _m_hmQuestCountMap.put(info.getQuestId(), info);
        }

        //推送协议
        _m_comp.getUserData().sendMsgToGC(US2GCWriter_028_QuestOp.make_053_OnPlayerQuestCountChg(info.toProto()));
    }
    
    /**
     * 检查是否主线任务，如果是，增加主线任务统计数量
     * @param _info
     */
    private void _checkAndIncrMainQuest(QuestCountInfo _info)
    {
        RefQuest questRef = RefQuest.getMgr().get(_info.getQuestId());
        if(null != questRef && EQuestType.MAIN == questRef.quest_type)
        {
        	_m_iMainQuestCount++;
        }
    }

    /**
     * 清空数据
     */
    public void clear()
    {
    	_m_comp.getUserData().lockUser();
    	
    	try
    	{
            for (QuestCountInfo countInfo : _m_hmQuestCountMap.values())
            {
                if (countInfo == null)
                {
                    continue;
                }
                countInfo.deal();
            }
            _m_hmQuestCountMap.clear();
            
            _m_iMainQuestCount = 0;
    	}
    	finally 
    	{
    		_m_comp.getUserData().unlockUser();
		}
    }
}
