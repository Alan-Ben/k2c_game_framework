package NPGameRes.Refs.AvatarGacha;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.EQuality;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.List;

@RefTable(tableName = "gacha_item")
public class RefGachaItem extends RefBase
{
    private static RefTableContainer<RefGachaItem> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefGachaItem> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefGachaItem> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefGachaItem>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGachaItem newRef = (RefGachaItem) _newRef;
        id = newRef.id;
        pool_id = newRef.pool_id;
        weight = newRef.weight;
        extra_weight_condition = newRef.extra_weight_condition;
        roll_condition = newRef.roll_condition;
        extra_weight = newRef.extra_weight;
        quality = newRef.quality;
        item = newRef.item;
        tag_list = newRef.tag_list;
        if_show = newRef.if_show;
        if_show_marquee = newRef.if_show_marquee;
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

    public long id;//卡池物品id
    public long pool_id;//卡池id
    public int weight;//基础权重
    public NPPlayerConditionGroupObj extra_weight_condition;//增加额外权重条件
    public NPPlayerConditionGroupObj roll_condition;//抽取条件
    public int extra_weight;//额外权重
    public EQuality quality;//道具
    public NPCommonCostItem item;//道具
    public List<String> tag_list;//道具标记列表
    public boolean if_show;//抽到是否在公屏展示
    public boolean if_show_marquee;//抽到是否在跑马灯展示

    public EQuality getQuality()
    {
        return quality;
    }
}