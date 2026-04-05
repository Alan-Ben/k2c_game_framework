package NPGameRes.Refs.BagItem;

import Common.BagItemUseEnum.EBagItemUse_ConsortType;
import CommonEnum.EBasicAttrType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerVariable.NPPlayerVariableGroupObj;

@RefTable(tableName = "bag_item_consort")
public class RefBagItemConsort extends RefBase
{
    private static RefTableContainer<RefBagItemConsort> _g_mgr = new RefTableContainer<RefBagItemConsort>();

    public static RefTableContainer<RefBagItemConsort> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBagItemConsort> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBagItemConsort>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBagItemConsort newRef = (RefBagItemConsort) _newRef;
        id = newRef.id;
        attr_type = newRef.attr_type;
        consort_id = newRef.consort_id;
        value = newRef.value;
        show_type = newRef.show_type;
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
    public EBasicAttrType attr_type;//增加的属性类型
    public NPPlayerVariableGroupObj consort_id;//妃子id
    public NPPlayerVariableGroupObj value;//值
    public EBagItemUse_ConsortType show_type;//道具类型
}
