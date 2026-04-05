package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.Composite;

import Common.TreasureHuntObj.TreasureHunt_CompositeInfo;
import NPCommon.DB._ASelectCallback;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntCompositeCatalog;
import NPGameRes.Refs.TreasureHunt.RefTreasureHuntOre;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent;
import NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp.TreasureHuntComponent.SkillInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerTreasureHuntCompositeBO;

import java.util.ArrayList;
import java.util.List;

public class TreasureHuntCompositeMgr
{
    private TreasureHuntComponent _m_comp;
    private List<TreasureHuntCompositeInfo> _m_compositeList;

    public TreasureHuntCompositeMgr(TreasureHuntComponent _comp)
    {
        _m_comp = _comp;
        _m_compositeList = new ArrayList<>();
    }

    public TreasureHuntComponent getComp()
    {
        return _m_comp;
    }

    public NPUSUserData getUserData()
    {
        return _m_comp.getUserData();
    }

    /**
     * 初始化组合数据
     * @param _handler
     */
    public void _initCompositeFromDB(_ICallBackBool _handler)
    {
        _m_comp.getUSServer().getBM().getBM(PlayerTreasureHuntCompositeBO.class).findAll("cid", _m_comp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerTreasureHuntCompositeBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerTreasureHuntCompositeBO> _boList)
                    {
                        for (PlayerTreasureHuntCompositeBO bo : _boList)
                        {
                            RefTreasureHuntCompositeCatalog refComposite = RefTreasureHuntCompositeCatalog.getMgr().get(bo.getCompositeId());
                            if (refComposite == null)
                            {
                                USLog.error(_m_comp.getUSServer(), "TreasureHuntCompositeMgr _initCompositeFromDB refComposite is null， cid:{} compositeId:{}",
                                        _m_comp.getUserData().getCid(), bo.getCompositeId());
                                continue;
                            }

                            TreasureHuntCompositeInfo compositeInfo = new TreasureHuntCompositeInfo(TreasureHuntCompositeMgr.this, bo, refComposite);
                            _m_compositeList.add(compositeInfo);
                        }

                        _handler.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        _handler.onRunOver(false);
                    }
                });
    }

    /**
     * 获取已组合数量
     * @return
     */
    public long getHadCollectCount()
    {
        getUserData().lockUser();
        try
        {
            long count = 0;
            for (TreasureHuntCompositeInfo info : _m_compositeList)
            {
                if (info.hadCollectAllOre())
                {
                    count++;
                }
            }
            return count;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查询组合信息
     * @param _compositeId
     * @return
     */
    public TreasureHuntCompositeInfo lookupComposite(long _compositeId)
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntCompositeInfo info : _m_compositeList)
            {
                if (info.getCompositeId() == _compositeId)
                {
                    return info;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 确保组合信息存在
     * @param _compositeId
     * @return
     */
    public TreasureHuntCompositeInfo ensureComposite(long _compositeId)
    {
        getUserData().lockUser();
        try
        {
            TreasureHuntCompositeInfo compositeInfo = lookupComposite(_compositeId);
            if (compositeInfo == null)
            {
                RefTreasureHuntCompositeCatalog refComposite = RefTreasureHuntCompositeCatalog.getMgr().get(_compositeId);
                if (refComposite == null)
                {
                    USLog.error(_m_comp.getUSServer(), "TreasureHuntCompositeMgr ensureComposite refComposite is null, compositeId:{}", _compositeId);
                    return null;
                }

                compositeInfo = new TreasureHuntCompositeInfo(this, refComposite);
                _m_compositeList.add(compositeInfo);
            }
            return compositeInfo;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 当获得新的矿石
     * @param _refOre
     * @param _hadReachAdvanced
     */
    public void onGainOre(RefTreasureHuntOre _refOre, boolean _hadReachAdvanced)
    {
        for (Long compositeId : _refOre.compositeIdList)
        {
            TreasureHuntCompositeInfo compositeInfo = ensureComposite(compositeId);
            if (compositeInfo == null)
                continue;

            compositeInfo.addOre(_refOre.Id(), _hadReachAdvanced);
        }
    }

    /**
     * 填充组合信息到Proto对象列表
     * @param _compositeList
     */
    public void fillProto(List<TreasureHunt_CompositeInfo> _compositeList)
    {
        getUserData().lockUser();
        try
        {
            for (TreasureHuntCompositeInfo compositeInfo : _m_compositeList)
            {
                _compositeList.add(compositeInfo.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 收集组合物品技能信息到指定列表中
     * 
     * @param _skillList 技能信息列表
     */
    public void collectSkillInfo(List<SkillInfo> _skillList)
    {
        for (TreasureHuntCompositeInfo compositeInfo : _m_compositeList) 
        {
            // 普通技能
            if (compositeInfo.getNormalSkillInfo() != null && compositeInfo.getNormalSkillInfo().getLevel() > 0) 
            {
                long skillId = compositeInfo.getRef().normal_skill_id;
                int level = compositeInfo.getNormalSkillInfo().getLevel();
                _skillList.add(new SkillInfo(skillId, level));
            }
            
            // 高级技能
            if (compositeInfo.getAdvancedSkillInfo() != null && compositeInfo.getAdvancedSkillInfo().getLevel() > 0) 
            {
                long skillId = compositeInfo.getRef().advanced_skill_id;
                int level = compositeInfo.getAdvancedSkillInfo().getLevel();
                _skillList.add(new SkillInfo(skillId, level));
            }
        }
    }
}
