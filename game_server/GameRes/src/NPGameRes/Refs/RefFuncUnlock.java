package NPGameRes.Refs;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPFunctionType;

import java.util.ArrayList;

@RefTable(tableName = "func_unlock")
public class RefFuncUnlock extends RefBase
{
    private static RefTableContainer<RefFuncUnlock> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefFuncUnlock> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefFuncUnlock> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefFuncUnlock>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefFuncUnlock newRef = (RefFuncUnlock) _newRef;
        type = newRef.type;
        simple_unlock_id = newRef.simple_unlock_id;
        gain_item_list = newRef.gain_item_list;
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
    ////////////////////////

    public ENPFunctionType type;
    //解锁id
    public long simple_unlock_id;
    //奖励id
    public ArrayList<NPCommonCostItem> gain_item_list = new ArrayList<>();
}