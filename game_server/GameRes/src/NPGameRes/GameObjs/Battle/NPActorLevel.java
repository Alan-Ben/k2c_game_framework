package NPGameRes.GameObjs.Battle;

import NPGameRes.GameObjs.NPActorProperty.NPPropertyContainer;
import NPGameRes.GameObjs.NPActorProperty.NPPropertyModifier;
import NPGameRes.Refs.Battle.RefActorHeroSkill;
import NPGameRes.Refs.Battle.RefActorLevel;
import NPGameRes.Refs.Battle.RefActorLvlOperation;
import WCGCommon.Enum.NPEnum.EWCGModelType;

import java.util.ArrayList;

public class NPActorLevel
{
    public long id;
    public int min_level;
    public NPPropertyModifier lvl_basic_property;// 到达这个等级的基础属性加成
    public NPPropertyModifier addition_property;// 之后每级提升会增加的属性属性加成

    public NPTeamPropertyModifier team_property;// 增加的队伍属性

    public long evolve_actor;  //进化角色

    public int ai_id;// ai id

    public int defence_ai_id;//防御状态下ai id

    public long attackSkillId;// 默认攻击id
    public ArrayList<Long> ex_att_id;//额外的默认攻击Id
    public long deathSkillId;//死亡触发技能

    public long birth_skill_id;//出生时使用的技能Id
    public long deploy_skill_id;//部署时使用的技能Id
    public long recycle_skill_id;//回收时使用的技能Id

    public ArrayList<Long> skillIdList = new ArrayList<>();// 其他技能id
    public ArrayList<WCGActorSkill> skill_info_list = new ArrayList<>();// 其他技能id

    public ArrayList<WCGActorOperation> operation_list;// 3d控制列表

    public EWCGModelType model_type;// 模型类型 3d或者2d
    public String model;// 模型资源
    public int model_scale;// 模型缩放 万分比

    public ArrayList<Long> link_effect_list;// 关联的特效列表 用于特效预加载

    public ArrayList<WCGHeroSkill> heroSkillList;//英雄技能(非英雄时为空 )

    public WCGActorOperation GetOperation(int _operationId)
    {
        if (operation_list == null)
        {
            return null;
        }

        WCGActorOperation operation = null;
        for (int i = 0; i < operation_list.size(); i++)
        {
            operation = operation_list.get(i);
            if (operation.operationId == _operationId)
            {
                return operation;
            }
        }
        return null;
    }

    /***************
     * 计算对应等级的属性加成
     *
     * @author alzq.z
     * @time 2021年6月24日 上午12:00:13
     */
    public void calPropertyBonus(long _lvl, NPPropertyContainer _container)
    {
        if (null == _container)
            return;

        //添加基础加成
        _container.addModifier(lvl_basic_property);
        //添加等级加成
        _container.addModifier(addition_property, _lvl - min_level);
    }


    public void adapt(RefActorLevel refLevel)
    {
        this.id = refLevel.id;
        this.min_level = refLevel.min_level;
        this.lvl_basic_property = NPPropertyModifier.readPropertyModifier(
                refLevel.lvl_basic_property, "lvl_basic_property"); // 属性加成
        this.addition_property = NPPropertyModifier.readPropertyModifier(
                refLevel.addition_property, "addition_property"); // 属性加成
        this.team_property = NPTeamPropertyModifier.readPropertyModifier(
                refLevel.team_property, "team_property");

        this.evolve_actor = refLevel.evolve_actor;

        this.ai_id = refLevel.ai_id;// aiid
        this.defence_ai_id = refLevel.defence_ai_id;

        this.attackSkillId = refLevel.attackSkillId;// 默认攻击id
        this.deathSkillId = refLevel.deathSkillId;
        this.skillIdList = refLevel.skillIdList;// 其他技能id
        this.skill_info_list = WCGActorSkill.readSkillInfoList(refLevel.skill_info_list);

        this.model_type = refLevel.model_type;
        this.model = refLevel.model;// 模型资源
        this.model_scale = refLevel.model_scale;

        this.link_effect_list = refLevel.link_effect_list;// 关联的特效列表用于特效预加载

        this.birth_skill_id = refLevel.birth_skill_id;//出生时使用的技能Id
        this.ex_att_id = refLevel.ex_att_id;

        // this.main_city_skill_list = refLevel.main_city_skill_list;//
        // 和主城等级挂钩的技能列表

        // this.building_ing = refLevel.building_ing;// 生产前提不满足时的建筑单位

        this.operation_list = new ArrayList<WCGActorOperation>();
        for (RefActorLvlOperation refOpt : RefActorLvlOperation.getMgr().getList())
        {
            if (refOpt.id == this.id && refOpt.level == this.min_level)
            {
                WCGActorOperation optObj = new WCGActorOperation();
                optObj.adapt(refOpt);
                this.operation_list.add(optObj);
            }
        }

        for (RefActorHeroSkill ref : RefActorHeroSkill.getMgr().getList())
        {
            if (ref.actor_id == this.id && ref.actor_lvl == this.min_level)
            {
                WCGHeroSkill heroSkill = new WCGHeroSkill();
                heroSkill.adapt(ref);

                if (null == this.heroSkillList)
                    this.heroSkillList = new ArrayList<WCGHeroSkill>();

                this.heroSkillList.add(heroSkill);
            }
        }


    }

    //判断是否普通攻击
    public boolean isDefaultAtt(long _skillId)
    {
        if (attackSkillId == _skillId)
            return true;

        if (null != ex_att_id && ex_att_id.size() > 0)
        {
            for (int i = 0; i < ex_att_id.size(); i++)
            {
                if (ex_att_id.get(i) == _skillId)
                    return true;
            }
        }

        return false;
    }
}