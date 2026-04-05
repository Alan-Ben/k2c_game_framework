package NPGameRes.Refs.AvatarGacha;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;
import java.util.List;

@RefTable(tableName = "gacha_pool")
public class RefGachaPool extends RefBase
{
    private static RefTableContainer<RefGachaPool> _g_mgr = new RefTableContainer<>();

    public static RefTableContainer<RefGachaPool> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefGachaPool> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefGachaPool>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGachaPool newRef = (RefGachaPool) _newRef;
        id = newRef.id;
        activity_id = newRef.activity_id;
        condition = newRef.condition;
        roll_cost = newRef.roll_cost;
        ten_roll_cost = newRef.ten_roll_cost;
        cumulative_reward_need_num = newRef.cumulative_reward_need_num;
        cumulative_reward_item = newRef.cumulative_reward_item;
        roll_record_limit = newRef.roll_record_limit;
        fixed_cd_id = newRef.fixed_cd_id;
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
    public long activity_id;//关联活动id(没配置即常驻卡池, 如果配置了则活动开启才能抽)
    public NPPlayerConditionGroupObj condition;//前置条件
    public List<NPCommonCostItem> roll_cost;//单次抽卡消耗
    public List<NPCommonCostItem> ten_roll_cost;//十连抽消耗
    public int cumulative_reward_need_num;//获得累计奖励抽卡次数
    public NPCommonCostItem cumulative_reward_item;//抽卡累计奖励
    public int roll_record_limit;//抽卡记录上限
    public long fixed_cd_id;//固定消耗道具id(配置后优先消耗该道具，否则消耗roll_cost)

    @RefField(isIgnore = true)
    public List<RefGachaItem> itemRefList = new ArrayList<>();
    @RefField(isIgnore = true)
    public List<RefGachaPoolWeight> weightRefList = new ArrayList<>();

}