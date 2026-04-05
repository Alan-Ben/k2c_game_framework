package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite;

import Common.TreasureHuntObj.TreasureHunt_CompositeInfo;
import MJLog.MJEventLog;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.TreasureHuntErr;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntCompositeCatalog;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntSkill;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.Skill.TreasureHuntCompositeAdvancedSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite.Skill.TreasureHuntCompositeNormalSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp._ATreasureHuntSkillInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_036_TreasureHuntOp;
import USDB.Bo.PlayerTreasureHuntCompositeBO;

import java.util.ArrayList;
import java.util.List;

public class TreasureHuntCompositeInfo
{
    private TreasureHuntCompositeMgr _m_mgr;
    private PlayerTreasureHuntCompositeBO _m_bo;
    private RefTreasureHuntCompositeCatalog _m_ref;

    private List<Long> _m_normalOreIdList;
    private List<Long> _m_advancedOreIdList;

    private TreasureHuntCompositeNormalSkillInfo _m_normalSkillInfo;
    private TreasureHuntCompositeAdvancedSkillInfo _m_advancedSkillInfo;

    public TreasureHuntCompositeInfo(TreasureHuntCompositeMgr _mgr, RefTreasureHuntCompositeCatalog _ref)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;
        _m_normalOreIdList = new ArrayList<>();
        _m_advancedOreIdList = new ArrayList<>();

        // 初始化技能信息
        RefTreasureHuntSkill normalSkillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.normal_skill_id);
        if (normalSkillRef != null)
            _m_normalSkillInfo = new TreasureHuntCompositeNormalSkillInfo(this, normalSkillRef, 0);

        RefTreasureHuntSkill advancedSkillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.advanced_skill_id);
        if (advancedSkillRef != null)
            _m_advancedSkillInfo = new TreasureHuntCompositeAdvancedSkillInfo(this, advancedSkillRef, 0);
    }

    public TreasureHuntCompositeInfo(TreasureHuntCompositeMgr _mgr, PlayerTreasureHuntCompositeBO _bo, RefTreasureHuntCompositeCatalog _ref)
    {
        _m_mgr = _mgr;
        _m_ref = _ref;
        _m_normalOreIdList = new ArrayList<>();
        _m_advancedOreIdList = new ArrayList<>();
        _m_bo = _bo;

        // 初始化技能信息
        RefTreasureHuntSkill normalSkillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.normal_skill_id);
        if (normalSkillRef != null)
            _m_normalSkillInfo = new TreasureHuntCompositeNormalSkillInfo(this, normalSkillRef, _bo.getIsNormalSkillActive() ? 1 : 0);

        RefTreasureHuntSkill advancedSkillRef = RefTreasureHuntSkill.getMgr().get(_m_ref.advanced_skill_id);
        if (advancedSkillRef != null)
            _m_advancedSkillInfo = new TreasureHuntCompositeAdvancedSkillInfo(this, advancedSkillRef, _bo.getIsAdvancedSkillActive() ? 1 : 0);
    }

    /**
     * 获取组合ID
     * @return
     */
    public long getCompositeId()
    {
        return _m_ref.Id();
    }

    public TreasureHuntCompositeMgr getMgr()
    {
        return _m_mgr;
    }

    public NPUSUserData getUserData()
    {
        return _m_mgr.getUserData();
    }

    public PlayerTreasureHuntCompositeBO getBo()
    {
        return _m_bo;
    }

    public BM getBMObj()
    {
        return _m_mgr.getComp().getUSServer().getBM();
    }

    /**
     * 普通组合是否已经完成
     * @return
     */
    public boolean hadCollectAllOre()
    {
        return _m_bo != null;
    }

    /**
     * 高级组合是否已经完成
     * @return
     */
    public boolean hadCollectAdvancedOre()
    {
        return _m_advancedOreIdList.size() == _m_ref.ore_list.size();
    }

    /**
     * 新增矿石的处理
     * @param _oreId
     * @param _hadReachAdvanced
     */
    public void addOre(long _oreId, boolean _hadReachAdvanced)
    {
        getUserData().lockUser();
        try
        {
            if (!_m_ref.ore_list.contains(_oreId))
                return;

            if (!_m_normalOreIdList.contains(_oreId))
                _m_normalOreIdList.add(_oreId);

            if (_hadReachAdvanced && !_m_advancedOreIdList.contains(_oreId))
                _m_advancedOreIdList.add(_oreId);

            // 如果组合完成，且没有BO对象，则创建一个
            if (_m_bo == null && _m_normalOreIdList.size() >= _m_ref.ore_list.size())
            {
                BM bmObj = _m_mgr.getComp().getUSServer().getBM();
                // 组合完成，创建BO
                PlayerTreasureHuntCompositeBO bo = new PlayerTreasureHuntCompositeBO();
                bo.setCid(bmObj, _m_mgr.getComp().getUserData().getCid());
                bo.setCompositeId(bmObj, _m_ref.Id());
                bo.setCollectTimeMs(bmObj, CommonFunc.getNowTimeMS());
                bo.insert(bmObj);
                _m_bo = bo;
                _m_mgr.getComp().getUserData().sendMsgToGC(US2GCWriter_036_TreasureHuntOp.make_056_OnTreasureCompositeChg(makeProto()));
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 激活技能
     * @param _isAdvanced 是否是高级技能
     * @return
     */
    public Result activeSkill(boolean _isAdvanced, NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _isAdvanced ? _m_advancedSkillInfo : _m_normalSkillInfo;
        if (skillInfo == null)
            return TreasureHuntErr.TREASURE_HUNT_SKILL_NOT_EXIST;

        int beforeNormalLevel = _m_normalSkillInfo != null ? _m_normalSkillInfo.getLevel() : 0;
        int beforeAdvancedLevel = _m_advancedSkillInfo != null ? _m_advancedSkillInfo.getLevel() : 0;

        Result result = skillInfo.activeSkill(_context);
        if (!result.isSucc())
            return result;

        int afterNormalLevel = _m_normalSkillInfo != null ? _m_normalSkillInfo.getLevel() : 0;
        int afterAdvancedLevel = _m_advancedSkillInfo != null ? _m_advancedSkillInfo.getLevel() : 0;

        // 构造等级字符串，格式：{normal:1,advanced:0}
        String beforeLevelStr = String.format("{normal:%d,advanced:%d}", beforeNormalLevel, beforeAdvancedLevel);
        String newLevelStr = String.format("{normal:%d,advanced:%d}", afterNormalLevel, afterAdvancedLevel);

        // 记录日志
        MJEventLog.logFishItemRecord(
                getUserData(),
                3,  // 3=组合
                getCompositeId(),
                _isAdvanced ? 1 : 2,  // 1=高级；2=普通
                1,  // 1=解锁
                beforeLevelStr,
                newLevelStr,
                _context.getContextId()
        );

        return result;
    }

    /**
     * 获取普通技能信息
     * @return 普通技能信息对象
     */
    public TreasureHuntCompositeNormalSkillInfo getNormalSkillInfo()
    {
        return _m_normalSkillInfo;
    }

    /**
     * 获取高级技能信息
     * @return 高级技能信息对象
     */
    public TreasureHuntCompositeAdvancedSkillInfo getAdvancedSkillInfo()
    {
        return _m_advancedSkillInfo;
    }

    /**
     * 获取配置引用
     * @return 配置对象
     */
    public RefTreasureHuntCompositeCatalog getRef()
    {
        return _m_ref;
    }

    /**
     * 设置技能等级（仅用于初始化数据）
     * @param _isAdvanced
     * @param _level
     * @param _context
     */
    public void setSkillLevel(boolean _isAdvanced, int _level, NPPlayerContext _context)
    {
        _ATreasureHuntSkillInfo skillInfo = _isAdvanced ? _m_advancedSkillInfo : _m_normalSkillInfo;
        if (skillInfo != null)
        {
            skillInfo.setLevel(_level);
        }
    }

    /**
     * 构造协议
     * @return
     */
    public TreasureHunt_CompositeInfo makeProto()
    {
        TreasureHunt_CompositeInfo proto = new TreasureHunt_CompositeInfo();
        proto.setRefId(_m_ref.Id());
        if (_m_bo != null)
        {
            proto.setCollectTimeMs(_m_bo.getCollectTimeMs());
            proto.setIsNormalActive(_m_bo.getIsNormalSkillActive());
            proto.setIsAdvancedActive(_m_bo.getIsAdvancedSkillActive());
        }
        return proto;
    }
}
