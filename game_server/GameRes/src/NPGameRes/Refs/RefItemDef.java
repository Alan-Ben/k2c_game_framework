package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

@RefTable(tableName = "item_def")
public class RefItemDef extends RefBase
{
    private static RefTableContainer<RefItemDef> _g_mgr = new RefTableContainer<RefItemDef>();

    public static RefTableContainer<RefItemDef> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefItemDef> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefItemDef>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefItemDef newRef = (RefItemDef) _newRef;
        id = newRef.id;
        item = newRef.item;
        count_formula = newRef.count_formula;
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

    public long id;
    public NPCommonItem item; //CommonItem
    public NPPlayerVariableGroupObj count_formula; //物品数量高级计算公式
}
