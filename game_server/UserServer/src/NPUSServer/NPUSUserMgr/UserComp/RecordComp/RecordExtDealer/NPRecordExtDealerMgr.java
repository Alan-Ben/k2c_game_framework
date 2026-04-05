package NPUSServer.NPUSUserMgr.UserComp.RecordComp.RecordExtDealer;

import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPRecordExtDealerMgr
{
    public static NPRecordExtDealerMgr _g_instance = new NPRecordExtDealerMgr();
    public static NPRecordExtDealerMgr getInstance()
    {
        return _g_instance;
    }

    ///////////////////////////////////////////////////////////////

    //玩家记录数据对象对应的额外处理操作
    private _ANPRecordExtDealer[] _m_arrRecordExtDealerArr;

    public NPRecordExtDealerMgr()
    {
        _m_arrRecordExtDealerArr = new _ANPRecordExtDealer[ENPPlayerRecordParam.ENPPlayerRecordParam_Length];

        //注册处理对象
        regDealer(new NPRecordExtDealer_PLAYER_CONSORT_INTIMACY());//玩家妃子亲密度记录
        regDealer(new NPRecordExtDealer_PLAYER_CONSORT_INTIMACY_ADD());//玩家妃子亲密度增加记录（不包括初始值）
        regDealer(new NPRecordExtDealer_PLAYER_CONSORT_CHARM_ADD());//玩家妃子加护值增加记录（不包括初始值）
        regDealer(new NPRecordExtDealer_MARS_TEAM_MAX_POWER());//火星队伍最大实力
        regDealer(new NPRecordExtDealer_MARS_MAX_POWER());//火星最大实力
        regDealer(new NPRecordExtDealer_MARS_TEAM_COLLECT_SUM());//火星矿场采集资源量
        
        regDealer(new NPRecordExtDealer_MARS_TIME_REDUCED_MIN_SUM());//火星累计使用道具加速时长之和（分钟）
        regDealer(new NPRecordExtDealer_MARS_TECH_TIME_REDUCED_MIN_SUM());//火星研究科技累计使用道具加速之和（分钟）
        regDealer(new NPRecordExtDealer_MARS_BUILD_TIME_REDUCED_MIN_SUM());//火星建筑升级累计使用道具加速之和（分钟）
        regDealer(new NPRecordExtDealer_MARS_TEAM_REPAIR_TIME_REDUCED_MIN_SUM());//火星队伍修复累计使用道具加速之和（分钟）
    }

    /**
     * 注册处理对象
     * @param _dealer
     */
    public void regDealer(_ANPRecordExtDealer _dealer)
    {
        _m_arrRecordExtDealerArr[_dealer.getRecordParam().ordinal()] = _dealer;
    }

    /**
     * 执行计数变化处理
     * @param _userData
     * @param _recordType
     * @param _oriCount
     * @param _curCount
     * @param _context
     */
    public void dealOnCountChg(NPUSUserData _userData, ENPPlayerRecordParam _recordType, long _oriCount, long _curCount, NPPlayerContext _context)
    {
        //获取对应的dealer对象
        _ANPRecordExtDealer dealer = _m_arrRecordExtDealerArr[_recordType.ordinal()];
        if (null != dealer)
        {
            //执行计数变化触发事件
            dealer.onCountChg(_userData, _oriCount, _curCount, _context);
        }
    }
}
