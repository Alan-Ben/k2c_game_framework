package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.Refs.Consort.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ConsortInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        ////////////////// ========== RefConsortHaloLvl 按 halo_id 预分组 ========== //////////////////
        Map<Long, _TLevelMapMgr<RefConsortHaloLvl>> haloLevelByHaloIdMap = new HashMap<>();
        List<RefConsortHaloLvl> haloLvlRefList = RefConsortHaloLvl.getMgr().getList();
        for (int i = 0; i < haloLvlRefList.size(); i++)
        {
            RefConsortHaloLvl ref = haloLvlRefList.get(i);
            if (null == ref)
                continue;
            haloLevelByHaloIdMap.computeIfAbsent(ref.halo_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }

        ////////////////// ========== RefConsortStory 按 consort_id 预分组 ========== //////////////////
        Map<Long, HashMap<Integer, ArrayList<RefConsortStory>>> storyByConsortMap = new HashMap<>();
        List<RefConsortStory> callStoryRefList = RefConsortStory.getMgr().getList();
        for (int i = 0; i < callStoryRefList.size(); i++)
        {
            RefConsortStory ref = callStoryRefList.get(i);
            if (null == ref)
                continue;
            storyByConsortMap
                .computeIfAbsent(ref.consort_id, k -> new HashMap<>())
                .computeIfAbsent(ref.dialog_type.ordinal(), k -> new ArrayList<>())
                .add(ref);
        }

        ////////////////// ========== RefConsort ========== //////////////////
        List<RefConsort> consortRefList = RefConsort.getMgr().getList();
        for (int i = 0; i < consortRefList.size(); i++)
        {
            RefConsort consortRef = consortRefList.get(i);
            if (null == consortRef)
                continue;

            // 加护技能数据
            ArrayList<RefConsortBlessSkill> blessSkillList = new ArrayList<>();
            for (int j = 0; j < consortRef.bless_skill_id_list.size(); j++)
            {
                RefConsortBlessSkill blessSkillRef = RefConsortBlessSkill.getMgr().get(consortRef.bless_skill_id_list.get(j));
                if (null == blessSkillRef)
                {
                    CommLog.error("ConsortInitDealer.dealInit - consort ref:{} not find bless skill:{} ref.", consortRef.id, consortRef.bless_skill_id_list.get(j));
                }
                else
                {
                    blessSkillList.add(blessSkillRef);
                }
            }
            consortRef.blessSkillList = blessSkillList;

            // 星辉等级：直接从预分组 Map 中取
            _TLevelMapMgr<RefConsortHaloLvl> haloLevelMapMgr = haloLevelByHaloIdMap.get(consortRef.consort_halo_id);
            consortRef.setHaloLevelMapMgr(haloLevelMapMgr != null ? haloLevelMapMgr : new _TLevelMapMgr<>());

            // 家人事件数据：直接从预分组 Map 中取
            HashMap<Integer, ArrayList<RefConsortStory>> callStoryRefListMap = storyByConsortMap.get(consortRef.id);
            consortRef.callStoryRefListMap = callStoryRefListMap != null ? callStoryRefListMap : new HashMap<>();
        }

        ////////////////// ========== RefConsortBlessSkill ========== //////////////////
        // 预整理：按 bless_skill_id 分组加护技能等级数据
        Map<Long, _TLevelMapMgr<RefConsortBlessSkillLvl>> blessLevelMap = new HashMap<>();
        List<RefConsortBlessSkillLvl> blessSkillLvlRefList = RefConsortBlessSkillLvl.getMgr().getList();
        for (int i = 0; i < blessSkillLvlRefList.size(); i++)
        {
            RefConsortBlessSkillLvl ref = blessSkillLvlRefList.get(i);
            if (null == ref)
                continue;
            blessLevelMap.computeIfAbsent(ref.bless_skill_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }
        List<RefConsortBlessSkill> blessSkillRefList = RefConsortBlessSkill.getMgr().getList();
        for (int i = 0; i < blessSkillRefList.size(); i++)
        {
            RefConsortBlessSkill ref = blessSkillRefList.get(i);
            if (null == ref)
                continue;
            _TLevelMapMgr<RefConsortBlessSkillLvl> mgr = blessLevelMap.get(ref.bless_skill_id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelMapMgr<>());
        }

        //////////////////========== RefConsortHaloSkill ========== //////////////////
        // 预整理：按 halo_skill_id 分组星辉技能等级数据
        Map<Long, _TLevelMapMgr<RefConsortHaloSkillLvl>> haloSkillLevelMap = new HashMap<>();
        List<RefConsortHaloSkillLvl> consortHaloSkillLvlRefList = RefConsortHaloSkillLvl.getMgr().getList();
        for (int i = 0; i < consortHaloSkillLvlRefList.size(); i++)
        {
            RefConsortHaloSkillLvl ref = consortHaloSkillLvlRefList.get(i);
            if (null == ref)
                continue;
            haloSkillLevelMap.computeIfAbsent(ref.halo_skill_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }
        List<RefConsortHaloSkill> consortHaloSkillRefList = RefConsortHaloSkill.getMgr().getList();
        for (int i = 0; i < consortHaloSkillRefList.size(); i++)
        {
            RefConsortHaloSkill ref = consortHaloSkillRefList.get(i);
            if (null == ref)
                continue;
            _TLevelMapMgr<RefConsortHaloSkillLvl> mgr = haloSkillLevelMap.get(ref.halo_skill_id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelMapMgr<>());
        }

        ////////////////// ========== RefConsortHaloLvl haloSkillList ========== //////////////////
        for (int i = 0; i < haloLvlRefList.size(); i++)
        {
            RefConsortHaloLvl haloLvlRef = haloLvlRefList.get(i);
            if (null == haloLvlRef)
                continue;

            ArrayList<ConsortHaloSkillObj> haloSkillList = new ArrayList<>();
            for (int j = 0; j < haloLvlRef.halo_skill_list.size(); j++)
            {
                ConsortHaloSkillParseObj skillParseObj = haloLvlRef.halo_skill_list.get(j);
                if (null == skillParseObj)
                    continue;

                RefConsortHaloSkill skillRef = RefConsortHaloSkill.getMgr().get(skillParseObj.first);
                if (null == skillRef)
                {
                    CommLog.error("ConsortInitDealer.dealInit - consort halo:{} lvl:{} load skill:{} fail.", haloLvlRef.id, haloLvlRef.level, skillParseObj.first);
                    continue;
                }
                RefConsortHaloSkillLvl skillLvlRef = skillRef.getLevelMapMgr().getLevelData(skillParseObj.second);
                if (null == skillLvlRef)
                {
                    CommLog.error("ConsortInitDealer.dealInit - consort halo:{} lvl:{} load skill:{} lvl:{} fail.", haloLvlRef.id, haloLvlRef.level, skillParseObj.first, skillParseObj.second);
                    continue;
                }

                haloSkillList.add(new ConsortHaloSkillObj(skillRef, skillLvlRef));
            }
            haloLvlRef.haloSkillList = haloSkillList;
        }

        //////////////////========== RefConsortFettersSkill ========== //////////////////
        // 预整理：按 fetters_skill_id 分组羁绊技能等级数据
        Map<Long, _TLevelMapMgr<RefConsortFettersSkillLvl>> fettersLevelMap = new HashMap<>();
        List<RefConsortFettersSkillLvl> consortFettersSkillLvlRefList = RefConsortFettersSkillLvl.getMgr().getList();
        for (int i = 0; i < consortFettersSkillLvlRefList.size(); i++)
        {
            RefConsortFettersSkillLvl ref = consortFettersSkillLvlRefList.get(i);
            if (null == ref)
                continue;
            fettersLevelMap.computeIfAbsent(ref.fetters_skill_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }
        List<RefConsortFettersSkill> consortFettersSkillRefList = RefConsortFettersSkill.getMgr().getList();
        for (int i = 0; i < consortFettersSkillRefList.size(); i++)
        {
            RefConsortFettersSkill ref = consortFettersSkillRefList.get(i);
            if (null == ref)
                continue;
            _TLevelMapMgr<RefConsortFettersSkillLvl> mgr = fettersLevelMap.get(ref.fetters_skill_id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelMapMgr<>());
        }
    }
}
