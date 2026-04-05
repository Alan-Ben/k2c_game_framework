package ActivitiesV02.Refs.NumMerge;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefSingleContainer;

/**
 * 数字合并其他配置（单例配置）
 *
 * 主要功能：
 * 1. 定义全局的游戏参数
 * 2. 配置buff相关参数
 * 3. 配置初始生成规则
 * 4. 配置游戏基础设置
 */
@RefTable(ignore = true)
public class RefNumMergeOther extends RefBase
{
    // 全局管理器实例
    private static RefNumMergeOtherMgr _g_mgr = new RefNumMergeOtherMgr();

    public static RefNumMergeOtherMgr getMgr()
    {
        return _g_mgr;
    }

    /**
     * 获取单例配置对象
     */
    public static RefNumMergeOther Ref()
    {
        return getMgr().getRef();
    }

    /**
     * 配置管理器（单例模式）
     */
    public static class RefNumMergeOtherMgr extends RefSingleContainer<RefNumMergeOther>
    {
        protected RefNumMergeOtherMgr()
        {
            super();
            // 设置默认值
            initPut(new RefNumMergeOther());
        }
    }

    @Override
    public RefNumMergeOtherMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefNumMergeOtherMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefNumMergeOther newRef = (RefNumMergeOther) _newRef;
        num_merge_start_gen_block_num = newRef.num_merge_start_gen_block_num;
        num_merge_each_gen_block_num = newRef.num_merge_each_gen_block_num;
        num_merge_ticket_item = newRef.num_merge_ticket_item;
        num_merge_organize_item = newRef.num_merge_organize_item;
        num_merge_eliminate_item = newRef.num_merge_eliminate_item;
        num_merge_first_buff_stamina_cost = newRef.num_merge_first_buff_stamina_cost;
        num_merge_buff_stamina_cost = newRef.num_merge_buff_stamina_cost;
        num_merge_max_buff_count = newRef.num_merge_max_buff_count;
        num_merge_buff_duration_round = newRef.num_merge_buff_duration_round;
        num_merge_lazy_cd_id = newRef.num_merge_lazy_cd_id;
        num_merge_buff_bonus_rate = newRef.num_merge_buff_bonus_rate;
        num_merge_box_reward_mail_id = newRef.num_merge_box_reward_mail_id;
    }

    /**
     * 获取对象数据Id
     */
    @Override
    public long Id()
    {
        return 0;
    }

    /**
     * 配置校验
     */
    public boolean NewAssert()
    {
        return true;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 配置字段（通过RefNumMergeOther_Process从数据库加载）

    // 开局生成格子数量
    public int num_merge_start_gen_block_num;

    // 每回合生成格子数量
    public int num_merge_each_gen_block_num;

    // 奖券CommonItem
    public NPCommonItem num_merge_ticket_item;

    // 重排道具CommonItem
    public NPCommonItem num_merge_organize_item;

    // 消除道具CommonItem
    public NPCommonItem num_merge_eliminate_item;

    // 花费多少体力后触发第一次buff生成
    public int num_merge_first_buff_stamina_cost;

    // 第2+次的buff生CmdNumMerge成所需要花费的体力值
    public int num_merge_buff_stamina_cost;

    // 最大同时存在的buff数量（若达到数量，生成buff的机会作废）
    public int num_merge_max_buff_count;

    // buff可以存在的回合（滑动一次棋盘）数范围
    public int num_merge_buff_duration_round;

    // 体力lazy_cd_id
    public long num_merge_lazy_cd_id;

    // 获得buff时，棋子合成积分乘以的倍数（万分比）
    public int num_merge_buff_bonus_rate;

    // 宝箱奖励补发邮件ID
    public long num_merge_box_reward_mail_id;
}
