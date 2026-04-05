package NPUSServer.NPUSUserMgr.UserComp.PlayerEventRecordComp;

import Common.PlayerEnum.EPlayerEventRecordType;
import Common.PlayerObj.Player_EventRecordInfo;
import NPCommon.DB.BM.BM;
import NPGameRes.GameObjs.Battle.WCGIntRange;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USDB.Bo.PlayerEventRecordBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map.Entry;

/**
 * @description: 按类型管理的计数器管理器
 * @author: ricci
 * @date: 2022-08-26 15:51:56
 */
public class NPPlayerEventRecordInfoMgr
{
    /**
     * 计数器类型
     */
    private int _m_mgrTypeId;

    /**
     * id:NPPlayerEventRecordInfo
     */
    private HashMap<Long, NPPlayerEventRecordInfo> _m_mapRecordInfoMap;
    /**
     * 上级组件
     */
    private NPPlayerEventRecordComp _m_comp;


    public NPPlayerEventRecordInfoMgr(int _type, NPPlayerEventRecordComp _comp)
    {
        this._m_mgrTypeId = _type;
        this._m_comp = _comp;
        _m_mapRecordInfoMap = new HashMap<>();
    }

    public NPPlayerEventRecordComp getComp()
    {
        return _m_comp;
    }

    public int getTypeId()
    {
        return _m_mgrTypeId;
    }

    public long getCid()
    {
        return getComp().getCid();
    }

    protected void _initFromDB(PlayerEventRecordBO _bo)
    {
        NPPlayerEventRecordInfo info = new NPPlayerEventRecordInfo(this, _bo);
        _m_mapRecordInfoMap.put(_bo.getRecordSubId(), info);
    }

    protected NPPlayerEventRecordInfo _lookup(long _id)
    {
        return _m_mapRecordInfoMap.get(_id);
    }

    /**
     * 构造协议对象
     * @param _list
     */
    protected void makeProto(ArrayList<Player_EventRecordInfo> _list)
    {
        for (NPPlayerEventRecordInfo info : _m_mapRecordInfoMap.values())
        {
            if (null == info)
                continue;

            _list.add(info.toProto());
        }
    }

    /**
     * 获取计数器里的值
     * @param _recordSubId 细分id
     * @return 如果对象为null 返回0 ，否则返回对象里的count值
     */
    public long getCount(long _recordSubId)
    {
        NPPlayerEventRecordInfo recordInfo = _lookup(_recordSubId);
        if (recordInfo == null)
        {
            return 0;
        }
        return recordInfo.getCount();
    }

    /**
     * 获取id在某个范围内的统计值
     * @param _subIdRange 范围
     * @return long
     */
    public long getSum(WCGIntRange _subIdRange)
    {
        long sum = 0;
        for (NPPlayerEventRecordInfo info : _m_mapRecordInfoMap.values())
        {
            if (null == info || !_subIdRange.inRange(info.getSubId()))
            {
                continue;
            }

            sum += info.getCount();
        }
        return sum;
    }

    /**
     * 获取所有计数器里的总和
     * @return
     */
    public long getSum()
    {
        long sum = 0;
        for (NPPlayerEventRecordInfo info : _m_mapRecordInfoMap.values())
        {
            if (null == info)
                continue;

            sum += info.getCount();
        }
        return sum;
    }

    /**
     * 查找或创建record对象
     * @param _id recordId
     * @return NPPlayerEventRecordInfo
     */
    protected NPPlayerEventRecordInfo _ensure(long _id)
    {
        NPPlayerEventRecordInfo info = _m_mapRecordInfoMap.get(_id);
        if (info == null)
        {
            BM bmObj = _m_comp.getUSServer().getBM();

            PlayerEventRecordBO bo = new PlayerEventRecordBO();
            bo.setCid(bmObj, getCid());
            bo.setRecordTypeId(bmObj, getTypeId());
            bo.setRecordSubId(bmObj, _id);
            bo.setCount(bmObj, 0);
            bo.insert(bmObj);

            info = new NPPlayerEventRecordInfo(this, bo);
            _m_mapRecordInfoMap.put(_id, info);

            //推送协议
            getComp().getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_065_OnEventRecordChg(info.toProto()));
        }
        return info;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (Entry<Long, NPPlayerEventRecordInfo> entry : _m_mapRecordInfoMap.entrySet())
        {
            sb.append("\n").append(entry.getKey()).append(" ").append(entry.getValue()).append("\n");
        }
        return "NPPlayerEventRecordInfoMgr{" +
                "type = " + EPlayerEventRecordType.EPlayerEventRecordType_FromInt(getTypeId()) +
                ", _m_mapRecordInfoMap=" + sb +
                '}';
    }

}
