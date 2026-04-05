package NPUSServer.NPUSUserMgr.UserComp.EquipComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.HeroObj.Equip_SkillInfo;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Common.RefOpCost;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_EQUIP_RESHAPE;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;
import USDB.Bo.PlayerEquipSkillBO;

public class EquipSkillInfo
{
    private long _m_dbId;
    private int _m_index;
    private int _m_normalRebuildNum;

    private int _m_value;

    //上次重塑的值
    private int _m_pendingValue;

    public EquipSkillInfo(PlayerEquipSkillBO _bo)
    {
        _m_dbId = _bo.getId();
        _m_index = _bo.getIndex();
        _m_value = _bo.getValue();
        _m_pendingValue = _bo.getPendingValue();
        _m_normalRebuildNum = _bo.getNormalReshapeNum();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public int getIndex()
    {
        return _m_index;
    }

    /**
     * 获取加成值
     * @return
     */
    public int getValue()
    {
        return _m_value;
    }

    /**
     * 重塑技能
     * @param _equipInfo
     * @param _isAdvance
     * @param _context
     * @return
     */
    public Result reshapeSkill(EquipInfo _equipInfo, boolean _isAdvance, NPPlayerContext _context)
    {
        //记录洗练前的属性值
        int beforeAttr = _m_value;

        //支持首次藏品重塑必定有当前大臣相性20%的技能加成[GOB-1574]
        long reshapeTimes = _equipInfo.getComp().getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.EQUIP_RESHARP_TIMES);
        //随机技能属性
        int value = reshapeTimes == 0 ? 2000 : _equipInfo._randomSkillValue(_isAdvance);
        if (value == 0)
            return HeroErr.EQUIP_RESHAPE_FAIL;

        //区分高级和普通重塑
        if (_isAdvance)
        {
            //消耗道具
            if (!_equipInfo.getComp().getUserData().spendItem(RefGeneral.Ref().equip_advance_cost, _context))
                return CommErr.ITEM_NOT_ENOUGH;
        } else
        {
            //获取重塑消耗
            RefOpCost opCostRef = RefOpCost.getMgr().getOpCostRef(RefGeneral.Ref().equip_normal_cost_group_id, _m_normalRebuildNum + 1);
            if (opCostRef == null)
                return CommErr.REF_NOT_FOUND;

            //消耗道具
            if (!_equipInfo.getComp().getUserData().spendItem(opCostRef.cost_item, _context))
                return CommErr.ITEM_NOT_ENOUGH;
        }

        //更新数据
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        if (!_isAdvance)
        {
            _m_normalRebuildNum++;
            updateValue.addValueObj("normal_reshape_num", _m_normalRebuildNum);
        }

        _m_pendingValue = value;
        int isSucceed = 0;
        if (value > _m_value)
        {
            //如果新值大于当前值，则更新当前值
            _m_value = value;
            updateValue.addValueObj("value", _m_value);
            isSucceed = 1;
        }

        updateValue.addValueObj("pending_value", _m_pendingValue);
        _equipInfo.getComp().getUSServer().getBM().getBM(PlayerEquipSkillBO.class).update("id", _m_dbId, updateValue);

        //推送消息
        _equipInfo.getComp().getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_067_OnEquipSkillChg(_equipInfo.getDbId(), toProto()));

        _equipInfo.getComp().getUserData().onLogicEvent(new Event_P_EQUIP_RESHAPE(_context));

        _equipInfo.getComp().getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.EQUIP_RESHARP_TIMES, 1, _context);

        //记录藏品洗练日志
        MJEventLog.logArtifactRefine(
            _equipInfo.getComp().getUserData(),
            _equipInfo.getEquipId(),
            _equipInfo.getDbId(),
            _equipInfo.getLevel(),
            _equipInfo.getAddTalentPoint(),
            _equipInfo.getWearHeroId(),
            1,
            isSucceed,
            beforeAttr,
            _m_value
        );

        return Result.SUCC;
    }

    /**
     * 重置技能
     * @param _equipInfo
     * @param _context
     */
    public void resetSkill(EquipInfo _equipInfo, NPPlayerContext _context)
    {
        _m_value = 100;
        _m_pendingValue = 0;
        _m_normalRebuildNum = 0;

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("value", _m_value);
        updateValue.addValueObj("pending_value", _m_pendingValue);
        updateValue.addValueObj("normal_reshape_num", _m_normalRebuildNum);

        _equipInfo.getComp().getUSServer().getBM().getBM(PlayerEquipSkillBO.class).update("id", _m_dbId, updateValue);

        //推送消息
        _equipInfo.getComp().getUserData().sendMsgToGC(US2GCWriter_013_HeroOp.make_067_OnEquipSkillChg(_equipInfo.getDbId(), toProto()));
    }

    /**
     * 构造信息
     * @return
     */
    public Equip_SkillInfo toProto()
    {
        Equip_SkillInfo proto = new Equip_SkillInfo();
        proto.setIndex(_m_index);
        proto.setValue(_m_value);
        proto.setPendingValue(_m_pendingValue);
        proto.setNormalRebuildNum(_m_normalRebuildNum);
        return proto;
    }
}
