package NPGameRes.Refs.Child;

import CommonEnum.ESpecAttrType;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * @author mark
 */
@RefTable(tableName = "child_attr")
public class RefChildAttr extends RefBase
{
    private static RefChildAttrMgr _g_mgr = new RefChildAttrMgr();
    public static RefChildAttrMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefChildAttrMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChildAttrMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChildAttr newRef = (RefChildAttr) _newRef;
        type = newRef.type;
        career_rand_group = newRef.career_rand_group;
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
        return type.ordinal();
    }
    
    public static class RefChildAttrMgr extends RefTableContainer<RefChildAttr>
    {
    	@Override
    	protected void _onTableLoaded()
        {
        }
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public ESpecAttrType type = ESpecAttrType.NONE;//属性类型
    public int career_rand_group;//职业随机组id
    
    @RefField(isIgnore = true)
    public WeightValueList<RefChildCareer> careerWeiList = new WeightValueList<>();
    /**
     * 获取随机职业
     * @return
     */
    public RefChildCareer rndCareer()
    {
    	WeightValueList<RefChildCareer> obj = this.careerWeiList;
    	return obj.random();
    }
}
