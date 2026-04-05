package NPGameRes.Refs.Child;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "child_quality")
public class RefChildQuality extends RefBase
{
    private static RefTableContainer<RefChildQuality> _g_mgr = new RefTableContainer<RefChildQuality>();
    public static RefTableContainer<RefChildQuality> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefChildQuality> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefChildQuality>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChildQuality newRef = (RefChildQuality) _newRef;
        id = newRef.id;
        step_lvl = newRef.step_lvl;
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
    public ArrayList<Integer> step_lvl = new ArrayList<>(); //阶段等级上限
    
    @RefField(isIgnore = true)
    public int maxLvl;
    @RefField(isIgnore = true)
    public ArrayList<Integer> addBonusStepList = new ArrayList<>(); //需要增加基础收益的等级列表
}
