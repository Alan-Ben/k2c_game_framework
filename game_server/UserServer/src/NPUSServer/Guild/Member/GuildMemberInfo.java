package NPUSServer.Guild.Member;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_IntList;
import Common.GuildEnum.EGuildPermissionType;
import Common.GuildEnum.EGuildPositionType;
import Common.GuildObj.Guild_MemberBaseInfo;
import Common.GuildObj.Guild_MemberContributeInfo;
import Common.GuildObj.Guild_MemberEntrustInfo;
import GS2GC.p032_GuildOp.GS2GC_032_056_OnSelfGuildContributeChg;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildPosition;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.GuildMemberBO;

import java.nio.ByteBuffer;
import java.util.Arrays;

public class GuildMemberInfo
{
    private GuildMemberMgr _m_memberMgr;
    private GuildMemberBO _m_bo;

    private RefGuildPosition _m_refPosition;
    private int[] _m_sevenDaysContributeRecord;

    private int _m_sevenDaysContribute;
    private long _m_lastCalTimestamp;
    
    //联盟宝箱相关数据
    private GuildMemberGuildBoxInfo _m_memberGuildBoxInfo;

    private MutexAtom _m_mutex;

    public static final int SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE = 7;

    public GuildMemberInfo(GuildMemberMgr _guildMemberMgr, GuildMemberBO _bo)
    {
        _m_memberMgr = _guildMemberMgr;
        _m_mutex = new MutexAtom();
        //降低优先级避免冲突
        _m_mutex.reducePriority(10);

        _m_bo = _bo;
        _m_refPosition = RefGuildPosition.getMgr().get(_bo.getPosition());
        _m_sevenDaysContributeRecord = new int[SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE];
        if (_bo.getPastDayContribute() != null)
        {
            Common_IntList sevenDaysContributeRecord = new Common_IntList();
            sevenDaysContributeRecord.readPackage(ByteBuffer.wrap(_bo.getPastDayContribute()));
            for (int i = 0; i < sevenDaysContributeRecord.getValueList().size(); i++)
            {
                _m_sevenDaysContributeRecord[i] = sevenDaysContributeRecord.getValueList().get(i);
            }
        }
        
        _m_memberGuildBoxInfo = new GuildMemberGuildBoxInfo(this);
    }
    
    public GuildMemberMgr getMemberMgr()
    {
    	return _m_memberMgr;
    }
    
    public GuildMemberBO getBo()
    {
    	return _m_bo;
    }

    public long getCid()
    {
        return _m_bo.getCid();
    }
    
    public GuildMemberGuildBoxInfo getMemberGuildBoxInfo()
    {
    	return _m_memberGuildBoxInfo;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getTotalContribution()
    {
        return _m_bo.getTotalContribute();
    }
    
    public EGuildPositionType getPosition()
    {
        return EGuildPositionType.EGuildPositionType_FromInt(_m_bo.getPosition());
    }

    /**
     * 获取离线时间
     * @return
     */
    public long getOfflineTimeMs()
    {
        return _m_bo.getIsOnline() ? 0 : CommonFunc.getNowTimeMS() - _m_bo.getLastReportOnlineTimestamp();
    }

    /**
     * 获取七天贡献
     * @return
     */
    public int getPastDaysContribute()
    {
        _lock();
        try
        {
            long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);
            if (_m_lastCalTimestamp >= todayZeroClockMS)
                return _m_sevenDaysContribute;

            //按照时间间隔，计算新的七天贡献
            int interval = (int) Math.min(((CommonFunc.getNowTimeMS() - _m_bo.getLastResetDataTimestamp()) / 86400000L), SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE);
            int newSevenDaysContributeValue = 0;
            for (int i = interval; i < SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE; i++)
            {
                newSevenDaysContributeValue += _m_sevenDaysContributeRecord[i];
            }

            _m_sevenDaysContribute = newSevenDaysContributeValue;
            _m_lastCalTimestamp = todayZeroClockMS;

            return _m_sevenDaysContribute;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取当日处理委托次数
     * @return
     */
    public int getDayDealEntrustNum()
    {
        if (_m_bo.getLastResetDataTimestamp() < CommonFunc.getTodayZeroClockMS(0))
            return 0;

        return _m_bo.getDayDealEntrustNum();
    }

    /**
     * 获取总处理委托次数
     * @return
     */
    public int getTotalDealEntrustNum()
    {
        return _m_bo.getTotalDealEntrustNum();
    }
    
    /**
     * 分享宝箱匿名
     * @return
     */
    public boolean isGuildBoxShareAnonymous()
    {
        return _m_bo.getIsGuildBoxShareAnonymous();
    }
    public void saveIsGuildBoxShareAnonymous(boolean _value)
    {
    	_m_bo.saveIsGuildBoxShareAnonymous(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), _value);
    }

    /**
     * 检查是否拥有指定权限
     * @param _needPermission
     * @return
     */
    public boolean checkPermission(EGuildPermissionType _needPermission)
    {
        RefGuildPosition refPosition = _m_refPosition;
        return refPosition != null && refPosition.checkPermission(_needPermission);
    }

    /**
     * 变更职位
     * @param _ref
     */
    public void setPosition(RefGuildPosition _ref)
    {
        _lock();
        try
        {
            _m_refPosition = _ref;
            _m_bo.savePosition(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), _ref.type.ordinal());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否可以任命
     * @param _positionType
     * @return
     */
    public boolean checkCanAppoint(EGuildPositionType _positionType)
    {
        RefGuildPosition refPosition = _m_refPosition;
        return refPosition != null && refPosition.checkCanAppoint(_positionType);
    }

    private void _checkResetData(long _todayZeroClockMS)
    {
        _lock();
        try
        {
            if (_m_bo.getLastResetDataTimestamp() >= _todayZeroClockMS)
                return;

            //记录历史贡献度
            //计算间隔时间，移动数组元素位置
            int interval = (int) ((_todayZeroClockMS - _m_bo.getLastResetDataTimestamp()) / 86400000L);
            if (interval > 0)
            {
                if (interval >= SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE)
                {
                    Arrays.fill(_m_sevenDaysContributeRecord, 0);
                } else
                {
                    System.arraycopy(_m_sevenDaysContributeRecord, interval, _m_sevenDaysContributeRecord, 0, SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE - interval);
                    Arrays.fill(_m_sevenDaysContributeRecord, SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE - interval, SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE, 0);
                }
            }

            //计算新的七天贡献
            int newSevenDaysContributeValue = 0;
            Common_IntList sevenDaysContributeRecord = new Common_IntList();
            for (int i : _m_sevenDaysContributeRecord)
            {
                sevenDaysContributeRecord.getValueList().add(i);
                newSevenDaysContributeValue += i;
            }

            _m_sevenDaysContribute = newSevenDaysContributeValue;
            _m_lastCalTimestamp = _todayZeroClockMS;

            BM bmObj = _m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM();

            _m_bo.setDayDealEntrustNum(bmObj, 0);
            _m_bo.setPastDayContribute(bmObj, sevenDaysContributeRecord.makePackage().array());
            _m_bo.setLastResetDataTimestamp(bmObj, _todayZeroClockMS);
            _m_bo.saveAllMarked(bmObj);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 增加贡献
     * @param _addDevote
     * @param _context
     */
    public void gainDevote(int _addDevote, NPPlayerContext _context)
    {
        if (_addDevote <= 0)
            return;

        _lock();
        try
        {
            long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);
            //检查是否重置数据
            _checkResetData(todayZeroClockMS);

            //增加贡献
            _m_sevenDaysContributeRecord[SEVEN_DAYS_CONTRIBUTE_RECORD_SIZE - 1] += _addDevote;

            //计算新的七天贡献
            int newSevenDaysContributeValue = 0;
            Common_IntList sevenDaysContributeRecord = new Common_IntList();
            for (int i : _m_sevenDaysContributeRecord)
            {
                sevenDaysContributeRecord.getValueList().add(i);
                newSevenDaysContributeValue += i;
            }

            _m_sevenDaysContribute = newSevenDaysContributeValue;
            _m_lastCalTimestamp = todayZeroClockMS;

            BM bmObj = _m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM();
            _m_bo.setTotalContribute(bmObj, _m_bo.getTotalContribute() + _addDevote);
            _m_bo.setPastDayContribute(bmObj, sevenDaysContributeRecord.makePackage().array());
            _m_bo.saveAllMarked(bmObj);
        } finally
        {
            _unlock();
        }

        _context.getCollector().addItem(RefGeneral.Ref().personal_contribution_common_item, _addDevote);

        getMemberMgr().getGuildInfo().getGuildMgr().getServer().sendMsgToGC(
                getCid(), new GS2GC_032_056_OnSelfGuildContributeChg(makeContributeProto()));
    }

    /**
     * 记录处理委托次数
     */
    public void recordDealEntrustNum(int _addValue)
    {
        if (_addValue <= 0)
            return;

        _lock();
        try
        {
            long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);
            //检查是否重置数据
            _checkResetData(todayZeroClockMS);

            BM bmObj = _m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM();
            _m_bo.setDayDealEntrustNum(bmObj, _m_bo.getDayDealEntrustNum() + _addValue);
            _m_bo.setTotalDealEntrustNum(bmObj, _m_bo.getTotalDealEntrustNum() + _addValue);
            _m_bo.saveAllMarked(bmObj);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 生成协议
     * @return
     */
    public Guild_MemberBaseInfo makeProto()
    {
        Guild_MemberBaseInfo proto = new Guild_MemberBaseInfo();
        proto.setCid(_m_bo.getCid());
        proto.setPositionId(_m_bo.getPosition());
        return proto;
    }

    /**
     * 生成协议
     * @return
     */
    public Guild_MemberContributeInfo makeContributeProto()
    {
        Guild_MemberContributeInfo proto = new Guild_MemberContributeInfo();
        proto.setCid(_m_bo.getCid());
        proto.setTotalContribute(_m_bo.getTotalContribute());
        proto.setSevenDaysContribute(getPastDaysContribute());
        return proto;
    }

    /**
     * 销毁数据
     */
    public void discard()
    {
        _m_bo.del(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    /**
     * 玩家上线
     */
    public void onOnline(long _timeMS)
    {
        //时间更早则不处理
        if(_timeMS < _m_bo.getLastReportOnlineTimestamp())
            return ;

        _m_bo.setIsOnline(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), true);
        _m_bo.setLastReportOnlineTimestamp(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), _timeMS);
        _m_bo.saveAllMarked(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    /**
     * 玩家下线
     */
    public void onOffline(long _timeMS)
    {
        //时间更早则不处理
        if(_timeMS < _m_bo.getLastReportOnlineTimestamp())
            return ;

        _m_bo.setIsOnline(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), false);
        _m_bo.setLastReportOnlineTimestamp(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), _timeMS);
        _m_bo.saveAllMarked(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    /**
     * 标记玩家下线
     * @param _timeMs
     */
    public void setOfflineTime(int _timeMs)
    {
        _m_bo.setIsOnline(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), false);
        _m_bo.setLastReportOnlineTimestamp(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM(), CommonFunc.getNowTimeMS() - _timeMs - 1);
        _m_bo.saveAllMarked(_m_memberMgr.getGuildInfo().getGuildMgr().getServer().getBM());
    }

    /**
     * 构造玩家处理委托的记录
     * @return
     */
    public Guild_MemberEntrustInfo makeEntrustProto()
    {
        Guild_MemberEntrustInfo proto = new Guild_MemberEntrustInfo();
        proto.setCid(_m_bo.getCid());
        proto.setDayDealTimes(getDayDealEntrustNum());
        proto.setTotalDealTimes(_m_bo.getTotalDealEntrustNum());
        return proto;
    }
    
    /**
     * 对玩家发送协议
     * @param _proto
     */
    public void sendMsg(_IALProtocolStructure _proto) 
    {
    	ALSynTaskManager.getInstance().regTask(() ->
        {
            NPUSUserData userData = _m_memberMgr.getGuildInfo().getGuildMgr().getServer().getUsUserMgr().lookupCacheUserData(getCid());
            if (userData == null)
                return;

            userData.safeCall(() -> userData.sendMsgToGC(_proto));
        });
    }
}
