package NPGameRes.Refs.BagItem;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;
import NPGameRes.Refs.Activity.RefActivityBagItem;
import NPGameRes.Refs.Parse.NPItemListParse;

@RefTable(tableName = "bag_item")
public class RefBagItem extends RefBase
{
    private static RefTableContainer<RefBagItem> _g_mgr = new RefTableContainer<RefBagItem>();

    public static RefTableContainer<RefBagItem> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefBagItem> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefBagItem>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefBagItem newRef = (RefBagItem) _newRef;
        id = newRef.id;
        can_sell = newRef.can_sell;
        sell_price = newRef.sell_price;
        need_redtip_when_add = newRef.need_redtip_when_add;
        quality = newRef.quality;
        is_spend_record = newRef.is_spend_record;
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
    public boolean can_sell; //能否出售
    public NPItemListParse sell_price = new NPItemListParse(); //商品出售价格
    public boolean need_redtip_when_add; //数量累加时，是否需要小红点提示
    public EQuality quality = EQuality.NONE; //品质

    @RefField(isIgnore = true)
    public RefActivityBagItem relativeActivityBagItemRef;

    /**********
     * NP-8944 【NP-0】增加收藏品-计数条件
     * https://www.teambition.com/task/6453137ec0381050b916f4d7
     */
    public boolean is_spend_record;//是否花费进入累计计数
}
