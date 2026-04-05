package NPGameRes.Refs.Share;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

/**
 * @author mark 主城表
 */
@RefTable(tableName = "box_comm")
public class RefBoxComm extends RefBase
{
    private static RefTableContainer<RefBoxComm> _g_mgr = new RefTableContainer<RefBoxComm>();

    public static RefTableContainer<RefBoxComm> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBoxComm> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBoxComm>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBoxComm newRef = (RefBoxComm) _newRef;
        id = newRef.id;
        box_limit = newRef.box_limit;
        is_sender_gain = newRef.is_sender_gain;
        item_list = newRef.item_list;
        expire_secs = newRef.expire_secs;
        cost_item_list = newRef.cost_item_list;
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id;
    public int box_limit;//宝箱可被领取上限
    public boolean is_sender_gain;//sender是否可以领取
    public ArrayList<NPCommonCostItem> item_list;//宝箱奖励
    public int expire_secs;//过期秒数

    /***
     * NP-7184 【v0.4】新增-分享聊天频道功能
     * https://www.teambition.com/task/63f08e5eb207988138f61902
     */
    public ArrayList<NPCommonCostItem> cost_item_list;//消耗物品列表

    /**********************
     * 任意随机一个物品
     * @return
     */
    public NPCommonCostItem getRndItem()
    {
        if (item_list.isEmpty())
            return null;

        int idx = CommonFunc.randomInt(item_list.size() - 1);
        return item_list.get(idx).duplicate();
    }
}
