package NPUSServer.NPUSUserMgr.UserComp.WeekCardComp;

import Common.WeekCardObj.WeekCard_SettleDetailInfo;
import Common.WeekCardObj.WeekCard_SingleSettingInfo;
import CommonEnum.EWeekCardSettleType;
import NPEnum.ENPFunctionType;
import NPGameRes.Refs.RefFuncUnlock;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerWeekCardSettingBO;

import java.nio.ByteBuffer;

public abstract class _AWeekCardDealer
{
    private WeekCardComponent _m_comp;
    private PlayerWeekCardSettingBO _m_settingBo;

    public _AWeekCardDealer(WeekCardComponent _comp)
    {
        _m_comp = _comp;
    }

    public WeekCardComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 设置周卡设置数据
     * @param _bo
     */
    public void setSettingBo(PlayerWeekCardSettingBO _bo)
    {
        _m_settingBo = _bo;

        try
        {
            _onSettingChg(_bo.getExtraInfo());
        } catch (Exception e)
        {
            USLog.error(_m_comp.getUSServer(), "_AWeekCardDealer setSettingBo error, cid:{}, type:{}", getUserData().getCid(), getType(), e);
        }
    }

    /**
     * 修改设置
     */
    public boolean chgSetting(WeekCard_SingleSettingInfo _settingInfo)
    {
        //检查参数合法性
        if (!_checkSettingParamLegal(_settingInfo.getExtraInfo()))
            return false;

        //如果没有设置数据，则新建
        if (_m_settingBo == null)
        {
            _m_settingBo = new PlayerWeekCardSettingBO();
            _m_settingBo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
            _m_settingBo.setSettleType(getUserData().getUSServer().getBM(), getType().ordinal());
            _m_settingBo.setExtraInfo(getUserData().getUSServer().getBM(), _settingInfo.getExtraInfo());
            _m_settingBo.insert(getUserData().getUSServer().getBM());
        } else
        {
            _m_settingBo.setExtraInfo(getUserData().getUSServer().getBM(), _settingInfo.getExtraInfo());
            _m_settingBo.saveAllMarked(getUserData().getUSServer().getBM());
        }

        try
        {
            _onSettingChg(_settingInfo.getExtraInfo());
        } catch (Exception e)
        {
            USLog.error(_m_comp.getUSServer(), "_AWeekCardDealer chgSetting error, cid:{}, type:{}", getUserData().getCid(), getType(), e);
        }
        return true;
    }

    /**
     * 处理离线期间溢出的CD
     * @return
     */
    public WeekCard_SettleDetailInfo settleOverflowCd(long _startSettleTimeMs, long _endSettleTimeMs, NPPlayerContext _context)
    {
        WeekCard_SettleDetailInfo proto = new WeekCard_SettleDetailInfo();
        proto.setType(getType());

        //调用实际的处理逻辑
        ByteBuffer byteBuffer = settleOverflowCdDetail(_startSettleTimeMs, _endSettleTimeMs, _context);
        if (byteBuffer == null)
            return null;

        proto.setDetailInfo(byteBuffer);
        return proto;
    }

    /**
     * 构造设置协议
     * @return 设置协议
     */
    public WeekCard_SingleSettingInfo makeSettingProto()
    {
        WeekCard_SingleSettingInfo settingInfo = new WeekCard_SingleSettingInfo();
        settingInfo.setType(getType());
        if (_m_settingBo != null)
        {
            settingInfo.setExtraInfo(_m_settingBo.getExtraInfo());
        }
        return settingInfo;
    }

    /**
     * 是否解锁当前功能
     * @return
     */
    protected boolean isFuncUnlock()
    {
        //查找对应配置
        RefFuncUnlock refFuncUnlock = RefFuncUnlock.getMgr().get(getFuncType().ordinal());
        //如果没有配置，则默认解锁
        if (refFuncUnlock == null)
            return true;

        //条件检查
        return NPPlayerConditionDealerMgr.IsEnable(refFuncUnlock.simple_unlock_id, getUserData(), null);
    }

    /**
     * 对应的功能类型
     * @return
     */
    protected abstract ENPFunctionType getFuncType();

    /**
     * 检查设置参数是否合法
     * @param _extraInfo 额外数据
     * @return 是否合法
     */
    protected abstract boolean _checkSettingParamLegal(byte[] _extraInfo);

    /**
     * 设置变更时调用
     * @param _extraInfo 额外数据
     */
    protected abstract void _onSettingChg(byte[] _extraInfo);

    /**
     * 获取处理类型
     * @return 处理类型
     */
    public abstract EWeekCardSettleType getType();

    /**
     * 在离线时对CD进行一次计算
     * @param _context 玩家上下文
     */
    public abstract void doOfflineCdSettle(NPPlayerContext _context);

    /**
     * 处理离线期间溢出的CD
     * @param _startSettleTimeMs 开始结算时间
     * @param _endSettleTimeMs   结束结算时间
     * @param _context           玩家上下文
     * @return 协议
     */
    public abstract ByteBuffer settleOverflowCdDetail(long _startSettleTimeMs, long _endSettleTimeMs, NPPlayerContext _context);

    /**
     * 初始化设置信息
     */
    public void checkSetting()
    {

    }
}
