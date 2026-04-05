package NPUSServer.Guild.Dispatch;

import ALBasicServer.ALBasicMutex.MutexAtom;
import AllRpcData.US_Service.Guild.GuildRecalMemberBuilding_2C;
import Common.GuildObj.Guild_AttrDispatchInfo;
import Common.GuildObj.Guild_DispatchData;
import Common.GuildObj.Guild_DispatchHeroDetailInfo;
import Common.GuildObj.Guild_DispatchHeroInfo;
import CommonEnum.ESpecAttrType;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Hero.RefHero;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;
import USDB.Bo.GuildHeroDispatchBO;

import java.util.ArrayList;
import java.util.List;

public class GuildHeroDispatchMgr
{
    private GuildInfo _m_guildInfo;
    //特长属性加成值
    private int[] _m_addPerList;
    //特长属性大臣数量
    private int[] _m_attrNumList;
    private List<GuildHeroDispatchInfo> _m_dispatchList;
    private MutexAtom _m_mutex;

    public GuildHeroDispatchMgr(GuildInfo _guildInfo)
    {
        _m_guildInfo = _guildInfo;
        _m_addPerList = new int[ESpecAttrType.ESpecAttrType_Length];
        _m_attrNumList = new int[ESpecAttrType.ESpecAttrType_Length];
        _m_dispatchList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    /**
     * 初始化数据
     * @param _bo
     */
    public void _initBo(GuildHeroDispatchBO _bo)
    {
        RefHero refHero = RefHero.getMgr().get(_bo.getHeroId());
        if (refHero == null)
        {
            USLog.error(_m_guildInfo.getGuildMgr().getServer(), "GuildHeroDispatchMgr _initBo fail, refHero is null, guildId:{} cid:{} heroId:{}",
                    _m_guildInfo.getGuildId(), _bo.getHeroId(), _bo.getHeroId());
            return;
        }

        _m_addPerList[refHero.spec_attr_type.ordinal()] += _bo.getAddValue();
        _m_attrNumList[refHero.spec_attr_type.ordinal()]++;
        _m_dispatchList.add(new GuildHeroDispatchInfo(refHero, _bo, this));
    }

    /**
     * 获取相性加成值
     * @param _attrType
     */
    public int getAddValue(ESpecAttrType _attrType)
    {
        _lock();
        try
        {
            return _m_addPerList[_attrType.ordinal()];
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查询玩家的派遣记录
     * @param _cid
     */
    public GuildHeroDispatchInfo lookupByCid(long _cid)
    {
        _lock();
        try
        {
            for (GuildHeroDispatchInfo dispatchInfo : _m_dispatchList)
            {
                if (dispatchInfo.getCid() == _cid)
                    return dispatchInfo;
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 替换加成值
     * @param _attrType
     * @param _oriValue
     * @param _newValue
     */
    public void replaceAddValue(ESpecAttrType _attrType, int _oriValue, int _newValue)
    {
        //数值一样不需要替换
        if (_oriValue == _newValue)
            return;

        _lock();
        try
        {
            _m_addPerList[_attrType.ordinal()] -= _oriValue;
            _m_addPerList[_attrType.ordinal()] += _newValue;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 退盟处理
     * @param _cid
     */
    public void removeDispatchByCid(long _cid)
    {
        //查询不到记录
        GuildHeroDispatchInfo dispatchInfo = lookupByCid(_cid);
        if (dispatchInfo == null)
            return;

        _lock();
        try
        {
            _m_dispatchList.remove(dispatchInfo);
            _m_addPerList[dispatchInfo.getSpecAttrType().ordinal()] -= dispatchInfo.getAddValue();
            _m_attrNumList[dispatchInfo.getSpecAttrType().ordinal()]--;

            dispatchInfo.discard();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 派遣大臣
     * @param _heroId
     * @return
     */
    public Result dispatchHero(long _cid, long _heroId, int _dispatchValue, int _lvl, long _power, long _skinId)
    {
        //查询派遣记录
        GuildHeroDispatchInfo dispatchInfo = lookupByCid(_cid);

        if (_heroId == 0)
        {
            cancelDispatch(dispatchInfo);
        } else
        {
            //检查是否派遣同一个英雄
            if (dispatchInfo != null && _heroId == dispatchInfo.getHeroId())
                return Result.SUCC;

            //计算派遣加成值
            RefHero refHero = RefHero.getMgr().get(_heroId);

            //替换或新增派遣
            Result result;
            if (dispatchInfo != null)
            {
                result = replaceDispatch(dispatchInfo, refHero, _dispatchValue);
            } else
            {
                result = addNewDispatch(_cid, refHero, _dispatchValue);
            }

            if (!result.isSucc())
                return result;

            //需要同步大臣数据到联盟
            onDispatchHeroInfoChg(_cid, _heroId, _lvl, _power, _skinId);
        }

        //重新计算大臣产出
        recalMemberBuilding();

        return Result.SUCC;
    }

    /**
     * 派遣大臣加成数值变更
     */
    public void onHeroDispatchAddValueChg(long _cid, long _heroId, int _dispatchValue)
    {
        //查询派遣记录
        GuildHeroDispatchInfo dispatchInfo = lookupByCid(_cid);
        if (dispatchInfo == null)
            return;

        if (_heroId != dispatchInfo.getHeroId())
            return;

        //获取大臣数据
        RefHero refHero = RefHero.getMgr().get(_heroId);
        if(null == refHero)
            return ;

        _lock();
        try
        {
            if (_heroId != dispatchInfo.getHeroId())
                return;

            // 原始加成值
            int oriAddValue = dispatchInfo.getAddValue();
            // 如果加成值没有变化则不处理
            if (oriAddValue == _dispatchValue)
                return;

            dispatchInfo.updateAddValue(_dispatchValue);

            // 更新加成值
            _m_addPerList[dispatchInfo.getSpecAttrType().ordinal()] += (_dispatchValue - oriAddValue);
        } finally
        {
            _unlock();
        }

        //广播属性变更
        broadcastDispatchChange(refHero.spec_attr_type);

        //重新计算大臣产出
        recalMemberBuilding();
    }

    /**
     * 派遣大臣实力数值变更
     */
    public void onDispatchHeroInfoChg(long _cid, long _heroId, int _lvl, long _power, long _skinId)
    {
        //查询派遣记录
        GuildHeroDispatchInfo dispatchInfo = lookupByCid(_cid);
        if (dispatchInfo == null)
            return;

        _lock();
        try
        {
            if (_heroId != dispatchInfo.getHeroId())
                return;

            dispatchInfo.updateInfo(_lvl, _power, _skinId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 取消派遣
     * @param _dispatchInfo
     */
    private void cancelDispatch(GuildHeroDispatchInfo _dispatchInfo)
    {
        if (_dispatchInfo == null)
            return;

        //移除派遣
        ESpecAttrType attrType = _dispatchInfo.getSpecAttrType();

        _lock();
        try
        {
            _m_dispatchList.remove(_dispatchInfo);
            _m_addPerList[attrType.ordinal()] -= _dispatchInfo.getAddValue();
            _m_attrNumList[attrType.ordinal()]--;
            _dispatchInfo.discard();
        } finally
        {
            _unlock();
        }

        broadcastDispatchChange(attrType);
    }

    /**
     * 替换派遣
     * @param _dispatchInfo
     * @param _refHero
     * @param _addValue
     * @return
     */
    private Result replaceDispatch(GuildHeroDispatchInfo _dispatchInfo, RefHero _refHero, int _addValue)
    {
        ESpecAttrType oriAttrType;
        ESpecAttrType newAttrType;

        _lock();
        try
        {
            oriAttrType = _dispatchInfo.getSpecAttrType();
            newAttrType = _refHero.spec_attr_type;

            //需要判断下目标属性的大臣数量是否超过要求
            if (oriAttrType != newAttrType
                    && _m_attrNumList[newAttrType.ordinal()] >= RefGeneral.Ref().each_attr_can_dispatch_hero_num)
                return GuildErr.GUILD_HERO_DISPATCH_ATTR_LIMIT;

            _m_addPerList[oriAttrType.ordinal()] -= _dispatchInfo.getAddValue();
            _m_attrNumList[oriAttrType.ordinal()]--;

            _dispatchInfo.replaceHero(_refHero, _addValue);

            _m_addPerList[newAttrType.ordinal()] += _addValue;
            _m_attrNumList[newAttrType.ordinal()]++;
        } finally
        {
            _unlock();
        }

        broadcastDispatchChange(oriAttrType);
        //如果不是相同属性, 需要广播新属性
        if (oriAttrType != newAttrType)
            broadcastDispatchChange(newAttrType);

        return Result.SUCC;
    }

    /**
     * 新增派遣
     * @param _cid
     * @param _refHero
     * @param _addValue
     * @return
     */
    private Result addNewDispatch(long _cid, RefHero _refHero, int _addValue)
    {
        _lock();
        try
        {
            //需要判断下目标属性的大臣数量是否超过要求
            if (_m_attrNumList[_refHero.spec_attr_type.ordinal()] >= RefGeneral.Ref().each_attr_can_dispatch_hero_num)
                return GuildErr.GUILD_HERO_DISPATCH_ATTR_LIMIT;

            GuildHeroDispatchBO bo = new GuildHeroDispatchBO();
            bo.setGuildId(_m_guildInfo.getGuildMgr().getServer().getBM(), _m_guildInfo.getGuildId());
            bo.setCid(_m_guildInfo.getGuildMgr().getServer().getBM(), _cid);
            bo.setHeroId(_m_guildInfo.getGuildMgr().getServer().getBM(), _refHero.Id());
            bo.setAddValue(_m_guildInfo.getGuildMgr().getServer().getBM(), _addValue);
            bo.insert(_m_guildInfo.getGuildMgr().getServer().getBM());

            _m_dispatchList.add(new GuildHeroDispatchInfo(_refHero, bo, this));
            _m_addPerList[_refHero.spec_attr_type.ordinal()] += _addValue;
            _m_attrNumList[_refHero.spec_attr_type.ordinal()]++;
        } finally
        {
            _unlock();
        }

        broadcastDispatchChange(_refHero.spec_attr_type);

        return Result.SUCC;
    }

    /**
     * 广播派遣变化
     * @param attrType
     */
    private void broadcastDispatchChange(ESpecAttrType attrType)
    {
        _m_guildInfo.getMemberMgr().broadcastMsg(
                US2GCWriter_032_GuildOp.make_072_OnGuildDispatchChg(makeAttrDispatchInfo(attrType)));
    }

    /**
     * 构造指定属性的派遣信息
     * @param _attrType
     * @return
     */
    private Guild_AttrDispatchInfo makeAttrDispatchInfo(ESpecAttrType _attrType)
    {
        Guild_AttrDispatchInfo proto = new Guild_AttrDispatchInfo();
        proto.setAttr(_attrType);
        _lock();
        try{
            for (GuildHeroDispatchInfo dispatchInfo : _m_dispatchList)
            {
                if (dispatchInfo.getSpecAttrType() != _attrType)
                    continue;

                Guild_DispatchHeroInfo dispatchHeroProto = new Guild_DispatchHeroInfo();
                dispatchHeroProto.setCid(dispatchInfo.getCid());
                dispatchHeroProto.setHeroId(dispatchInfo.getHeroId());
                dispatchHeroProto.setAddPer(dispatchInfo.getAddValue());

                proto.addHeroList(dispatchHeroProto);
            }
        }finally
        {
            _unlock();
        }
        return proto;
    }

    /**
     * 构造完全的派遣信息
     */
    public Guild_DispatchData makeFullDispatchInfo()
    {
        Guild_AttrDispatchInfo[] dispatchInfoList = new Guild_AttrDispatchInfo[ESpecAttrType.ESpecAttrType_Length];

        Guild_DispatchData proto = new Guild_DispatchData();
        _lock();
        try{
            for (GuildHeroDispatchInfo dispatchInfo : _m_dispatchList)
            {
                Guild_AttrDispatchInfo attrDispatchInfo = dispatchInfoList[dispatchInfo.getSpecAttrType().ordinal()];
                if (attrDispatchInfo == null)
                {
                    attrDispatchInfo = new Guild_AttrDispatchInfo();
                    attrDispatchInfo.setAttr(dispatchInfo.getSpecAttrType());
                    dispatchInfoList[dispatchInfo.getSpecAttrType().ordinal()] = attrDispatchInfo;
                }

                Guild_DispatchHeroInfo dispatchHeroProto = new Guild_DispatchHeroInfo();
                dispatchHeroProto.setCid(dispatchInfo.getCid());
                dispatchHeroProto.setHeroId(dispatchInfo.getHeroId());
                dispatchHeroProto.setAddPer(dispatchInfo.getAddValue());

                attrDispatchInfo.addHeroList(dispatchHeroProto);
            }
        }finally
        {
            _unlock();
        }

        for (Guild_AttrDispatchInfo attrDispatchInfo : dispatchInfoList)
        {
            if (attrDispatchInfo == null)
                continue;

            proto.addDispatchList(attrDispatchInfo);
        }

        return proto;
    }

    /**
     * 获取所有大臣的详细信息
     */
    public List<Guild_DispatchHeroDetailInfo> makeAllDetailInfo()
    {
        List<Guild_DispatchHeroDetailInfo> list = new ArrayList<>();
        _lock();
        try{
            for (GuildHeroDispatchInfo heroDispatchInfo : _m_dispatchList)
            {
                list.add(heroDispatchInfo.makeDetailInfo());
            }
        }finally
        {
            _unlock();
        }
        return list;
    }

    /**
     * 重新计算成员建筑
     */
    public void recalMemberBuilding()
    {
        //向所有成员广播重算赚速RPC
        GuildRecalMemberBuilding_2C rpc = new GuildRecalMemberBuilding_2C();
        rpc.req().setGuildId(_m_guildInfo.getGuildId());
        //放入新的赚速加成
        for(int i = 0; i < ESpecAttrType.values().length; i++) {
            rpc.req().getBuildingAddPerArr().add(getAddValue(ESpecAttrType.ESpecAttrType_FromInt(i)));
        }

        //重新计算大臣产出
        _m_guildInfo.getMemberMgr().broadcastRPC(rpc);
    }
}
