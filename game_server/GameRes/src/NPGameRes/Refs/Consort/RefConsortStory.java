package NPGameRes.Refs.Consort;

import Common.ConsortEnum.EConsortStoryType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

/**
 * @author mark
 */
@RefTable(tableName = "consort_story")
public class RefConsortStory extends RefBase
{
    private static RefConsortStoryMgr _g_mgr = new RefConsortStoryMgr();
    public static RefConsortStoryMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortStory> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortStoryMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortStory newRef = (RefConsortStory) _newRef;
        id = newRef.id;
        consort_id = newRef.consort_id;
        dialog_type = newRef.dialog_type;
        unlock_cg = newRef.unlock_cg;
        unlock_condition = newRef.unlock_condition;
    }
    
    public static class RefConsortStoryMgr extends RefTableContainer<RefConsortStory>
    {
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

    public long id;
    public long consort_id; //家人id
    public EConsortStoryType dialog_type = EConsortStoryType.NONE;
    public long unlock_cg; //解锁的cg
    public NPPlayerConditionGroupObj unlock_condition;//解锁条件
}
