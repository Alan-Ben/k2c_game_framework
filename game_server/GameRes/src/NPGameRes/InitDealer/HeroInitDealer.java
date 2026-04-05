package NPGameRes.InitDealer;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelAreaMgr;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.Refs.Consort.RefConsort;
import NPGameRes.Refs.Hero.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 大臣相关数据后初始化逻辑
 *
 * 参照 StepRewardDealer 模式：先整理好所有关联数据，再统一赋值给对应变量。
 */
public class HeroInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        List<RefHero> heroList = RefHero.getMgr().getList();

        // 初始化大臣关联妃子数据
        _initConsortData(heroList);

        // 初始化大臣光环技能等级数据
        _initHaloLevelData();

        // 初始化大臣皮肤等级数据
        List<RefHeroSkin> skinList = RefHeroSkin.getMgr().getList();
        _initSkinLevelData(skinList);

        // 初始化大臣套系技能等级数据
        _initSuitSkillLevelData();

        // 初始化大臣天赋技能等级数据
        _initTalentSkillLevelData();

        // 初始化大臣觉醒技能等级数据
        _initStarSkillLevelData();

        // 初始化大臣皮肤关联数据（大臣→皮肤列表）
        _initHeroSkinData(heroList, skinList);

        // 初始化大臣套件关联数据
        _initSuitHeroData(heroList);
    }

    /** 初始化大臣关联妃子 */
    private void _initConsortData(List<RefHero> _heroList)
    {
        Map<Long, ArrayList<RefConsort>> consortByHeroMap = new HashMap<>();
        List<RefConsort> consortList = RefConsort.getMgr().getList();
        for (int i = 0; i < consortList.size(); i++)
        {
            RefConsort consortRef = consortList.get(i);
            if (null == consortRef)
                continue;
            for (int j = 0; j < consortRef.relation_hero_id_list.size(); j++)
            {
                long heroId = consortRef.relation_hero_id_list.get(j);
                if (null == RefHero.getMgr().get(heroId))
                {
                    CommLog.error("HeroInitDealer._initConsortData - hero ref not found: heroId={}, consortId={}", heroId, consortRef.id);
                    continue;
                }
                consortByHeroMap.computeIfAbsent(heroId, k -> new ArrayList<>()).add(consortRef);
            }
        }
        for (int i = 0; i < _heroList.size(); i++)
        {
            RefHero ref = _heroList.get(i);
            if (null == ref)
                continue;
            ArrayList<RefConsort> list = consortByHeroMap.get(ref.id);
            ref.setRelationConsortList(list != null ? list : new ArrayList<>());
        }
    }

    /** 初始化大臣光环技能等级数据 */
    private void _initHaloLevelData()
    {
        Map<Long, _TLevelMapMgr<RefHeroHaloLevel>> haloLevelMap = new HashMap<>();
        List<RefHeroHaloLevel> haloLevelList = RefHeroHaloLevel.getMgr().getList();
        for (int i = 0; i < haloLevelList.size(); i++)
        {
            RefHeroHaloLevel ref = haloLevelList.get(i);
            if (null == ref)
                continue;
            haloLevelMap.computeIfAbsent(ref.halo_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }
        List<RefHeroHalo> haloList = RefHeroHalo.getMgr().getList();
        for (int i = 0; i < haloList.size(); i++)
        {
            RefHeroHalo ref = haloList.get(i);
            if (null == ref)
                continue;
            _TLevelMapMgr<RefHeroHaloLevel> mgr = haloLevelMap.get(ref.id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelMapMgr<>());
        }
    }

    /** 初始化大臣皮肤等级数据 */
    private void _initSkinLevelData(List<RefHeroSkin> _skinList)
    {
        Map<Long, _TLevelMapMgr<RefHeroSkinLevel>> skinLevelMap = new HashMap<>();
        List<RefHeroSkinLevel> skinLevelList = RefHeroSkinLevel.getMgr().getList();
        for (int i = 0; i < skinLevelList.size(); i++)
        {
            RefHeroSkinLevel ref = skinLevelList.get(i);
            if (null == ref)
                continue;
            skinLevelMap.computeIfAbsent(ref.skin_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }
        for (int i = 0; i < _skinList.size(); i++)
        {
            RefHeroSkin ref = _skinList.get(i);
            if (null == ref)
                continue;
            _TLevelMapMgr<RefHeroSkinLevel> mgr = skinLevelMap.get(ref.id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelMapMgr<>());
        }
    }

    /** 初始化大臣套系技能等级数据 */
    private void _initSuitSkillLevelData()
    {
        Map<Long, _TLevelAreaMgr<RefHeroSuitSkillLevel>> suitSkillLevelMap = new HashMap<>();
        List<RefHeroSuitSkillLevel> suitSkillLevelList = RefHeroSuitSkillLevel.getMgr().getList();
        for (int i = 0; i < suitSkillLevelList.size(); i++)
        {
            RefHeroSuitSkillLevel ref = suitSkillLevelList.get(i);
            if (null == ref)
                continue;
            suitSkillLevelMap.computeIfAbsent(ref.halo_suit_skill_id, k -> new _TLevelAreaMgr<>())._initAddLevelData(ref);
        }
        List<RefHeroSuitSkill> suitSkillList = RefHeroSuitSkill.getMgr().getList();
        for (int i = 0; i < suitSkillList.size(); i++)
        {
            RefHeroSuitSkill ref = suitSkillList.get(i);
            if (null == ref)
                continue;
            _TLevelAreaMgr<RefHeroSuitSkillLevel> mgr = suitSkillLevelMap.get(ref.id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelAreaMgr<>());
        }
    }

    /** 初始化大臣天赋技能等级数据 */
    private void _initTalentSkillLevelData()
    {
        Map<Long, _TLevelAreaMgr<RefHeroTalentSkillLevel>> talentLevelMap = new HashMap<>();
        List<RefHeroTalentSkillLevel> talentLevelList = RefHeroTalentSkillLevel.getMgr().getList();
        for (int i = 0; i < talentLevelList.size(); i++)
        {
            RefHeroTalentSkillLevel ref = talentLevelList.get(i);
            if (null == ref)
                continue;
            talentLevelMap.computeIfAbsent(ref.talent_skill_id, k -> new _TLevelAreaMgr<>())._initAddLevelData(ref);
        }
        List<RefHeroTalentSkill> talentSkillList = RefHeroTalentSkill.getMgr().getList();
        for (int i = 0; i < talentSkillList.size(); i++)
        {
            RefHeroTalentSkill ref = talentSkillList.get(i);
            if (null == ref)
                continue;
            _TLevelAreaMgr<RefHeroTalentSkillLevel> mgr = talentLevelMap.get(ref.id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelAreaMgr<>());
        }
    }

    /** 初始化大臣觉醒技能等级数据 */
    private void _initStarSkillLevelData()
    {
        Map<Long, _TLevelAreaMgr<RefHeroStarSkillLevel>> starSkillLevelMap = new HashMap<>();
        List<RefHeroStarSkillLevel> starSkillLevelList = RefHeroStarSkillLevel.getMgr().getList();
        for (int i = 0; i < starSkillLevelList.size(); i++)
        {
            RefHeroStarSkillLevel ref = starSkillLevelList.get(i);
            if (null == ref)
                continue;
            starSkillLevelMap.computeIfAbsent(ref.skill_id, k -> new _TLevelAreaMgr<>())._initAddLevelData(ref);
        }
        List<RefHeroStarSkill> starSkillList = RefHeroStarSkill.getMgr().getList();
        for (int i = 0; i < starSkillList.size(); i++)
        {
            RefHeroStarSkill ref = starSkillList.get(i);
            if (null == ref)
                continue;
            _TLevelAreaMgr<RefHeroStarSkillLevel> mgr = starSkillLevelMap.get(ref.skill_id);
            ref.setLevelAreaMgr(mgr != null ? mgr : new _TLevelAreaMgr<>());
        }
    }

    /** 初始化大臣皮肤关联数据（大臣→皮肤列表） */
    private void _initHeroSkinData(List<RefHero> _heroList, List<RefHeroSkin> _skinList)
    {
        Map<Long, ArrayList<RefHeroSkin>> skinByHeroMap = new HashMap<>();
        for (int i = 0; i < _skinList.size(); i++)
        {
            RefHeroSkin ref = _skinList.get(i);
            if (null == ref)
                continue;
            if (null == RefHero.getMgr().get(ref.hero_id))
            {
                ALServerLog.Error("HeroInitDealer._initHeroSkinData - hero ref not found: skinId=" + ref.id + ", heroId=" + ref.hero_id);
                continue;
            }
            skinByHeroMap.computeIfAbsent(ref.hero_id, k -> new ArrayList<>()).add(ref);
        }
        for (int i = 0; i < _heroList.size(); i++)
        {
            RefHero ref = _heroList.get(i);
            if (null == ref)
                continue;
            ArrayList<RefHeroSkin> list = skinByHeroMap.get(ref.id);
            ref.setSkinList(list != null ? list : new ArrayList<>());
        }
    }

    /** 初始化大臣套件关联英雄数据 */
    private void _initSuitHeroData(List<RefHero> _heroList)
    {
        Map<Long, ArrayList<RefHero>> heroBySuitMap = new HashMap<>();
        for (int i = 0; i < _heroList.size(); i++)
        {
            RefHero ref = _heroList.get(i);
            if (null == ref || ref.suit_id == 0)
                continue;
            if (null == RefHeroSuit.getMgr().get(ref.suit_id))
            {
                ALServerLog.Error("HeroInitDealer._initSuitHeroData - suit ref not found: heroId=" + ref.id + ", suitId=" + ref.suit_id);
                continue;
            }
            heroBySuitMap.computeIfAbsent(ref.suit_id, k -> new ArrayList<>()).add(ref);
        }
        List<RefHeroSuit> suitList = RefHeroSuit.getMgr().getList();
        for (int i = 0; i < suitList.size(); i++)
        {
            RefHeroSuit ref = suitList.get(i);
            if (null == ref)
                continue;
            ArrayList<RefHero> list = heroBySuitMap.get(ref.id);
            ref.setHeroList(list != null ? list : new ArrayList<>());
        }
    }
}
