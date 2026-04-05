package NPUSServer.NPUSUserMgr.UserComp.PlayerComp.Param;

import GS2GC.p002_InitOp.GS2GC_002_001_RetPlayerInfo;
import NPCommon.Enum.EUsParam;
import NPCommon.NPCommon_PlayerParam;
import NPCommon.Param._TNPParamMgr;
import NPEnum.ENPPlayerParam;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyContainer;
import NPUSServer.NPUSUserMgr.UserComp.PlayerComp.NPPlayerComponent;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerBO;

/****************
 * 玩家的信息参数管理对象
 * @author mj
 *
 */
public class NPPlayerParamMgr extends _TNPParamMgr<PlayerBO, _ANPPlayerInfoParamObj>
{
    //玩家组件对象
    private NPPlayerComponent _m_pcPlayerComp;
    private NPUserServer _m_server;
    //玩家属性加成
    private NPPlayerPropertyContainer _m_pcPlayerPropertyContainer;
    //离线期间奖励时长（毫秒）
    private long _m_offlinePeriodRewardDurationMs = 0;

    public NPPlayerParamMgr()
    {
        _m_pcPlayerComp = null;
        _m_server = null;
    }

    public NPUserServer getUSServer() {return _m_server;}

    /***************
     * 初始化本参数管理对象
     * @param _playerComp
     */
    public void initMgr(NPPlayerComponent _playerComp)
    {
        _m_pcPlayerComp = _playerComp;
        _m_server = _m_pcPlayerComp.getUSServer();
        //玩家属性修改器
        _m_pcPlayerPropertyContainer = new NPPlayerPropertyContainer();
        _m_pcPlayerComp.getPropertyMgr().regPropertyContainer(_m_pcPlayerPropertyContainer);

        if (null == _m_pcPlayerComp) return;

        PlayerBO bo = _playerComp.getBo();

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.ICON, bo)
        {
            public long GetValue()
            {
                return getData().getIcon();
            }

            public void saveValue(long value)
            {
                getData().saveIcon(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.ICON_BGK, bo)
        {
            public long GetValue()
            {
                return getData().getIconBgk();
            }

            public void saveValue(long value)
            {
                getData().saveIconBgk(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.BUBBLE, bo)
        {
            public long GetValue()
            {
                return getData().getBubble();
            }

            public void saveValue(long value)
            {
                getData().saveBubble(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.ROOM_SKIN, bo)
        {
            public long GetValue()
            {
                return getData().getRoomSkin();
            }

            public void saveValue(long value)
            {
                getData().saveRoomSkin(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LEVEL, bo)
        {
            public long GetValue()
            {
                return getData().getLvl();
            }

            public void saveValue(long value)
            {
                getData().saveLvl(getUSServer().getBM(), (int) value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.VIP_LVL, bo)
        {
            public long GetValue()
            {
                return getData().getVipLvl();
            }

            public void saveValue(long value)
            {
                getData().saveVipLvl(getUSServer().getBM(), (int) value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.GM_LVL, bo)
        {
            public long GetValue()
            {
                return getData().getGmLevel();
            }

            public void saveValue(long value)
            {
                getData().saveGmLevel(getUSServer().getBM(), (int) value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.CREATE_TIME, bo)
        {
            public long GetValue()
            {
                return getData().getCreateTime();
            }

            public void saveValue(long value)
            {
                getData().saveCreateTime(getUSServer().getBM(), (int) value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LOGIN_DAY_COUNT, bo)
        {
            public long GetValue()
            {
                return getData().getLoginDayCount();
            }

            public void saveValue(long value)
            {
                getData().saveLoginDayCount(getUSServer().getBM(), (int) value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_LOGIN_DATE, bo)
        {
            public long GetValue()
            {
                return getData().getLastLoginDate();
            }

            public void saveValue(long value)
            {
                getData().saveLastLoginDate(getUSServer().getBM(), (int) value);
            }
        });
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_TAKE_VERSION, bo)
        {
            public long GetValue()
            {
                return getData().getLastTakeVersion();
            }

            public void saveValue(long value)
            {
                getData().saveLastTakeVersion(getUSServer().getBM(), value);
            }
        });
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.RECHARGED_GEM, bo)
        {
            public long GetValue()
            {
                return getData().getRechargedGem();
            }

            public void saveValue(long value)
            {
                getData().saveRechargedGem(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_LEFT_BAG_TIME, bo)
        {
            public long GetValue()
            {
                return getData().getLastLeftBagTimeS();
            }

            public void saveValue(long value)
            {
                getData().saveLastLeftBagTimeS(getUSServer().getBM(), (int) value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_OFFLINE_MS, bo)
        {
            public long GetValue()
            {
                return getData().getLastOfflineMs();
            }

            public void saveValue(long value)
            {
                getData().saveLastOfflineMs(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.PREFAB, bo)
        {
            public long GetValue()
            {
                return getData().getPrefab();
            }

            public void saveValue(long value)
            {
                getData().savePrefab(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.IS_SET_DEFAULT, bo)
        {
            public long GetValue()
            {
                return getData().getIsSetDefault();
            }

            public void saveValue(long value)
            {
                getData().saveIsSetDefault(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.BUILDINGS_EARNINGS_MAX_RECORD, bo)
        {
            public long GetValue()
            {
                return getData().getBuildingEarningsMaxRecord();
            }

            public void saveValue(long value)
            {
                getData().saveBuildingEarningsMaxRecord(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.CHILD_EARNINGS_MAX_RECORD, bo)
        {
            public long GetValue()
            {
                return getData().getChildEarningsMaxRecord();
            }

            public void saveValue(long value)
            {
                getData().saveChildEarningsMaxRecord(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.EXTRA_EARNINGS, bo)
        {
            public long GetValue()
            {
                return getData().getExtraEarnings();
            }

            public void saveValue(long value)
            {
                getData().saveExtraEarnings(getUSServer().getBM(), value);
            }
        });

        //最后一次登录周标记
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_LOGIN_WEEK_TAG, bo)
        {
            public long GetValue()
            {
                return getData().getLastLoginWeekTag();
            }

            public void saveValue(long value)
            {
                getData().saveLastLoginWeekTag(getUSServer().getBM(), value);
            }
        });

        //本周登录天数
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.WEEK_LOGIN_DAY_COUNT, bo)
        {
            public long GetValue()
            {
                return getData().getWeekLoginDayCount();
            }

            public void saveValue(long value)
            {
                getData().saveWeekLoginDayCount(getUSServer().getBM(), value);
            }
        });

        //最近一次登陆时间（毫秒）
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LATEST_LOGIN_TIME_MS, bo)
        {
            public long GetValue()
            {
                return getData().getLatestLoginTimeMs();
            }

            public void saveValue(long value)
            {
                getData().saveLatestLoginTimeMs(getUSServer().getBM(), value);
            }
        });
        
        //集市下一次刷新截至时间
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.MARKET_NEXT_REFRESH_MS, bo)
        {
            public long GetValue()
            {
                return getData().getMarketNextRefreshMs();
            }

            public void saveValue(long value)
            {
                getData().saveMarketNextRefreshMs(getUSServer().getBM(), value);
            }
        });

        //情人指定邀约下次刷新时间
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.CONSORT_CALL_NEXT_REFRESH_MS, bo)
        {
            public long GetValue()
            {
                return getData().getConsortCallNextRefreshMs();
            }

            public void saveValue(long value)
            {
                getData().saveConsortCallNextRefreshMs(getUSServer().getBM(), value);
            }
        });
        
        //上次拜访其他玩家
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_GAIN_VISIT_REWARD_DAY, bo)
        {
            public long GetValue()
            {
                return getData().getLastGainVisitRewardDay();
            }

            public void saveValue(long value)
            {
                getData().saveLastGainVisitRewardDay(getUSServer().getBM(), (int) value);
            }
        });

        //Q版形象ID
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.CUTE_ACTOR, bo)
        {
            public long GetValue()
            {
                return getData().getCuteActorId();
            }

            public void saveValue(long value)
            {
                getData().saveCuteActorId(getUSServer().getBM(), value);
            }
        });

        //离线期间奖励时长（毫秒）
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.OFFLINE_PERIOD_REWARD_DURATION_MS, bo)
        {
            public long GetValue()
            {
                return _m_offlinePeriodRewardDurationMs;
            }

            public void saveValue(long value)
            {
                _m_offlinePeriodRewardDurationMs = value;
            }
        });

        //玩家实力历史记录最高值
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.POWER_MAX_RECORD, bo)
        {
            public long GetValue()
            {
                return getData().getPowerMaxRecord();
            }

            public void saveValue(long value)
            {
                getData().savePowerMaxRecord(getUSServer().getBM(), value);;
            }
        });

        //玩家赚速历史记录最高值
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.EARNINGS_MAX_RECORD, bo)
        {
            public long GetValue()
            {
                return getData().getEarningsMaxRecord();
            }

            public void saveValue(long value)
            {
                getData().saveEarningsMaxRecord(getUSServer().getBM(), value);
            }
        });

        //卷王子嗣毕业历史数量
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.GIFTDE_CHILD_GRADUATE_COUNT, bo)
        {
            public long GetValue()
            {
                return getData().getGiftdeChildGraduateCount();
            }

            public void saveValue(long value)
            {
                getData().saveGiftdeChildGraduateCount(getUSServer().getBM(), (int) value);
            }
        });

        //下次出生卷王次数
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.BIRTH_GIFTDE_COUM, bo)
        {
            public long GetValue()
            {
                return getData().getBirthGiftdeCoum();
            }

            public void saveValue(long value)
            {
                getData().saveBirthGiftdeCoum(getUSServer().getBM(), (int) value);
            }
        });
        
        //子嗣记录收益总值（因为移除记录在玩家身上）
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.ADULT_RECORD_BONUS, bo)
        {
            public long GetValue()
            {
                return getData().getAdultRecordBonus();
            }

            public void saveValue(long value)
            {
            	getData().saveAdultRecordBonus(getUSServer().getBM(), value);
            }
        });
        
        //最后一次领取每日奖励的日期标记
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.LAST_DRAW_DAILY_REWARD_DATE, bo)
        {
            public long GetValue()
            {
                return getData().getLastDrawDailyRewardDate();
            }

            public void saveValue(long value)
            {
                getData().saveLastDrawDailyRewardDate(getUSServer().getBM(), (int) value);
            }
        });

        //服务器开启天数
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.SERVER_START_DAYS, bo)
        {
            public long GetValue()
            {
                return getUSServer().getServerStartDay();
            }

            public void saveValue(long value)
            {
            }
        });

        //七日登录天数
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT, bo)
        {
            public long GetValue()
            {
                int sevenDaysLoginCalOffset = _m_pcPlayerComp.getBo().getSevenDaysLoginCalOffset();
                if (sevenDaysLoginCalOffset == 0)
                    return 0;

                return _m_pcPlayerComp.getParamV(ENPPlayerParam.LOGIN_DAY_COUNT) - sevenDaysLoginCalOffset + 1;
            }

            public void saveValue(long value)
            {
            }
        });
        
        //当前玩家皮肤
        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.PLAYER_SKIN, bo)
        {
            public long GetValue()
            {
                return getData().getCurPlayerSkin();
            }

            public void saveValue(long value)
            {
                getData().saveCurPlayerSkin(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.IS_SET_PREFAB, bo)
        {
            public long GetValue()
            {
                return getData().getIsSetPrefab();
            }

            public void saveValue(long value)
            {
                getData().saveIsSetPrefab(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.DAY_HAD_SEND_PAY_AI_TIMES, bo)
        {
            public long GetValue()
            {
                return _m_pcPlayerComp.getUserData().getConsortChatComponent().getTodayPayAiTimes();
            }

            public void saveValue(long value)
            {
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.PENDING_ORDER_DB_ID, bo)
        {
            public long GetValue()
            {
                return getData().getPendingOrderDbId();
            }

            public void saveValue(long value)
            {
                getData().savePendingOrderDbId(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.HAD_DRAW_VIP_REWARD_LIST, bo)
        {
            public long GetValue()
            {
                return getData().getHadDrawVipLevelReward();
            }

            public void saveValue(long value)
            {
                getData().saveHadDrawVipLevelReward(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.HAD_DRAW_VIP_RECHARGE_REWARD_LIST, bo)
        {
            public long GetValue()
            {
                return getData().getHadDrawVipLevelRechargeReward();
            }

            public void saveValue(long value)
            {
                getData().saveHadDrawVipLevelRechargeReward(getUSServer().getBM(), value);
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.IS_REFUSE_MARRY_REQUEST, bo)
        {
            public long GetValue()
            {
                return getUSServer().getRefuseMarryMgr().isRefuseMarry(getData().getCid()) ? 1 : 0;
            }

            public void saveValue(long value)
            {
                if (value == 1)
                {
                    //添加到拒绝联姻列表
                    getUSServer().getRefuseMarryMgr().addRefuseCid(getData().getCid());
                }
                else
                {
                    //从拒绝联姻列表移除
                    getUSServer().getRefuseMarryMgr().removeRefuseCid(getData().getCid());
                }
            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.HAD_MARS_POWER_RANK_OPENED, bo)
        {
            public long GetValue()
            {
                return getUSServer().getUSParams().getParam(EUsParam.HAD_MARS_POWER_RANK_OPENED);
            }

            public void saveValue(long value)
            {

            }
        });

        registParam(new _ANPPlayerInfoParamObj(ENPPlayerParam.MARS_GO_ROUTE_ARRIVE_COUNT, bo)
        {
            public long GetValue()
            {
                return getUSServer().getUSParams().getParam(EUsParam.MARS_GO_ROUTE_ARRIVE_COUNT);
            }

            public void saveValue(long value)
            {

            }
        });
    }

    /***************************
     * 封装玩家参数：自己查询
     *
     * @param _protocol
     */
    public void WriteProto(GS2GC_002_001_RetPlayerInfo _protocol)
    {
        for (int i = 0; i < ENPPlayerParam.values().length; i++)
        {
            ENPPlayerParam eParam = ENPPlayerParam.values()[i];
            _ANPPlayerInfoParamObj param = getParam(eParam);

            if (eParam == ENPPlayerParam.NONE || null == param) continue;

            NPCommon_PlayerParam protoParam = new NPCommon_PlayerParam();
            protoParam.setParamIndex(eParam.ordinal());
            protoParam.setValue(param.GetValue());

            _protocol.addPlayerParams(protoParam);
        }
    }

    /***********
     * 获取对应参数对象
     * @param _param
     * @return
     */
    public _ANPPlayerInfoParamObj getParam(ENPPlayerParam _param)
    {
        return getParam(_param.ordinal());
    }

    public long getParamV(ENPPlayerParam _eParam)
    {
        _ANPPlayerInfoParamObj param = getParam(_eParam);
        if (null == param)
        {
            USLog.error(_m_pcPlayerComp.getUSServer(), "getParam can not find param :" + _eParam);
            return 0L;
        }

        return param.GetValue();
    }

    /**************
     * 设置对应的值
     * @param _eParam
     * @param _value
     */
    public void setParam(ENPPlayerParam _eParam, long _value)
    {
        _ANPPlayerInfoParamObj param = getParam(_eParam);
        if (null == param)
        {
            USLog.error(_m_pcPlayerComp.getUSServer(), "setParam can not find param :" + _eParam);
            return;
        }

        long oldValue = param.GetValue();
        if (oldValue != _value)
        {
            param.saveValue(_value);

            //同步到客户端
            _m_pcPlayerComp.getUserData().sendMsgToGC(param.makeSyncProto());
            _m_pcPlayerComp.getUserData().Events.OnParamChg.onAsyncEvent(_eParam, oldValue, _value);
        }
    }

    public long incParam(ENPPlayerParam _eParam, long _value)
    {
        long newValue = getParamV(_eParam) + _value;
        if (newValue < 0) newValue = 0;

        setParam(_eParam, newValue);
        return newValue;
    }

    public long incParam(ENPPlayerParam _eParam)
    {
        return incParam(_eParam, 1);
    }

    public void resetParam(ENPPlayerParam _eParam)
    {
        setParam(_eParam, 0);
    }
}
