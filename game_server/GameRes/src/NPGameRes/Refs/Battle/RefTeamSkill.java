package NPGameRes.Refs.Battle;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import WCGCommon.Enum.NPEnum.EWCGTeamSkillClass;
import WCGCommon.Enum.NPEnum.EWCGTeamSkillEffectType;
import WCGCommon.Enum.NPEnum.EWCGTeamSkillSignType;

import java.util.ArrayList;

/**
 * @author scott
 */
@RefTable(tableName = "team_skill")
public class RefTeamSkill extends RefBase
{
    private static RefTableContainer<RefTeamSkill> _g_mgr = new RefTableContainer<RefTeamSkill>();

    public static RefTableContainer<RefTeamSkill> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefTeamSkill> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefTeamSkill>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTeamSkill newRef = (RefTeamSkill) _newRef;
        id = newRef.id;
        skill_icon = newRef.skill_icon;
        radius = newRef.radius;
        default_use = newRef.default_use;
        trigger_list = newRef.trigger_list;
        init_cd = newRef.init_cd;
        cd_list = newRef.cd_list;
        min_cd_ms = newRef.min_cd_ms;
        reset_cost = newRef.reset_cost;
        name = newRef.name;
        desc = newRef.desc;
        use_num = newRef.use_num;
        is_can_be_replece = newRef.is_can_be_replece;
        skill_class = newRef.skill_class;
        skill_effect_type = newRef.skill_effect_type;
        max_radius = newRef.max_radius;
        target_select_contition = newRef.target_select_contition;
        show_target_select = newRef.show_target_select;
        show_dispatch_area = newRef.show_dispatch_area;
        sign_type = newRef.sign_type;
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
        return id;
    }
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//id

    public String skill_icon;

    public int radius;//技能半径  厘米

    public boolean default_use;//默认在战场中

    public String trigger_list;

    public int init_cd;//初始CD,表示该技能刚进入战斗时候的CD
    public ArrayList<Integer> cd_list;//cd列表

    public int min_cd_ms;//最小cd  无法用资源重置

    public String reset_cost;//重置cd消耗金币

    public String name;//名字
    public String desc;//描述

    public int use_num;//使用次数  -1代表无限使用 0代表没有使用此时 >=1标示具体使用次数，使用一次，这个次数会减1 当使用次数为0是，删除该指挥官技能

    public boolean is_can_be_replece;

    public EWCGTeamSkillClass skill_class;

    public EWCGTeamSkillEffectType skill_effect_type;

    public long max_radius;

    public String target_select_contition;

    public boolean show_target_select;

    public boolean show_dispatch_area;

    public EWCGTeamSkillSignType sign_type;

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////


}
