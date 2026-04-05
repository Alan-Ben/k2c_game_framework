package NPGameRes.Refs.TreasureHunt;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;
import java.util.List;

/**
 * 技能id
 * id
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_skill")
public class RefTreasureHuntSkill extends RefBase
{
    private static RefTreasureHuntSkillMgr _g_mgr = new RefTreasureHuntSkillMgr();

    public static RefTreasureHuntSkillMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntSkillMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntSkillMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntSkill newRef = (RefTreasureHuntSkill) _newRef;
        id = newRef.id;
    }

    public static class RefTreasureHuntSkillMgr extends RefTableContainer<RefTreasureHuntSkill>
    {
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;//技能id

    @RefField(isIgnore = true)
    public List<RefTreasureHuntSkillLevel> skillLevelList = new ArrayList<>();//技能等级列表

    public void setSkillLevelList(List<RefTreasureHuntSkillLevel> _skillLevelList)
    {
        skillLevelList = _skillLevelList;
    }

    /**
     * 获取技能等级
     * @param _skillLevel
     * @return
     */
    public RefTreasureHuntSkillLevel getSkillLevel(int _skillLevel)
    {
        for (RefTreasureHuntSkillLevel level : skillLevelList)
        {
            if (level.level == _skillLevel)
            {
                return level;
            }
        }
        return null;
    }

}
