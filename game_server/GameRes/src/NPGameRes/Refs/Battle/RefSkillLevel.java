package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import WCGCommon.Enum.NPEnum.EWCGSkillRollingTimeType;

import java.util.ArrayList;

/**
 * @author scott 地  图  信  息
 */
@RefTable(tableName = "skill_level")
public class RefSkillLevel extends RefBase
{
    private static RefTableContainer<RefSkillLevel> _g_mgr = new RefTableContainer<RefSkillLevel>();

    public static RefTableContainer<RefSkillLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefSkillLevel> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefSkillLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefSkillLevel newRef = (RefSkillLevel) _newRef;
        id = newRef.id;
        minLevel = newRef.minLevel;
        aura_interval = newRef.aura_interval;
        interval_trigger = newRef.interval_trigger;
        action_name = newRef.action_name;
        action_time = newRef.action_time;
        cast_distance = newRef.cast_distance;
        cd_group = newRef.cd_group;
        cdTime = newRef.cdTime;
        skill_property = newRef.skill_property;
        skill_property_up = newRef.skill_property_up;
        team_property = newRef.team_property;
        aura_sfx_id = newRef.aura_sfx_id;
        audio_timeline = newRef.audio_timeline;
        relation_atk_speed = newRef.relation_atk_speed;
        cd_rel_atk_speed = newRef.cd_rel_atk_speed;
        skill_rolling_time = newRef.skill_rolling_time;
        skill_break_effect_list = newRef.skill_break_effect_list;
        skill_rolling_ui_type = newRef.skill_rolling_ui_type;
        skill_use_effect = newRef.skill_use_effect;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return id * 10000 + minLevel;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public int minLevel;
    public int aura_interval;
    public String interval_trigger;
    public String action_name;
    public int action_time;//动作时长 毫秒
    public int cast_distance; //释放距离
    public int cd_group;
    public int cdTime;//技能CD 毫秒
    public String skill_property;
    public String skill_property_up;
    public String team_property;
    public ArrayList<Long> aura_sfx_id;
    public String audio_timeline;
    public boolean relation_atk_speed;
    public boolean cd_rel_atk_speed;
    public int skill_rolling_time;
    public ArrayList<Long> skill_break_effect_list;
    public EWCGSkillRollingTimeType skill_rolling_ui_type;
    public ArrayList<Long> skill_use_effect;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
