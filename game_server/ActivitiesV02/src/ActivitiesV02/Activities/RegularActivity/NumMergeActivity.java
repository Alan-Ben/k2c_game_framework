package ActivitiesV02.Activities.RegularActivity;

import ActivitiesV02.Activities.RegularActivity.Player.NumMergePlayerMgr;
import ActivitiesV02.Bo.NumMergePlayerInfoBO;
import CommonEnum.ECommonActivityType;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityBaseBO;

import java.util.List;

/**
 * 数字合并活动类 - 2048游戏活动
 * <p>
 * 主要功能：
 * 1. 管理活动生命周期（启动、结束、关闭）
 * 2. 协调玩家管理器和玩家组件
 * 3. 处理活动级别的事件和逻辑
 * <p>
 * 设计特点：
 * - 继承通用活动基类，复用活动框架
 * - 通过PlayerMgr管理所有玩家数据
 * - 支持活动实例的完整生命周期
 * <p>
 * 活动类型ID: ECommonActivityType.NUM_MERGE
 */
public class NumMergeActivity extends _AActivityBase
{
    // 活动类型ID
    public static final int s_typeId = ECommonActivityType.NUM_MERGE.ordinal();

    // 玩家管理器
    private NumMergePlayerMgr _m_playerMgr;

    /**
     * 构造函数
     * @param _server 用户服务器实例
     * @param _bo     活动基础BO对象
     */
    public NumMergeActivity(NPUserServer _server, ActivityBaseBO _bo)
    {
        super(_server, _bo);
        _m_playerMgr = new NumMergePlayerMgr(this);
    }

    /**
     * 获取玩家管理器
     */
    public NumMergePlayerMgr getPlayerMgr()
    {
        return _m_playerMgr;
    }

    /**
     * 获取活动类型ID
     */
    @Override
    public int getActivityTypeId()
    {
        return s_typeId;
    }

    /**
     * 子类初始化 - 新创建活动
     * <p>
     * 用于活动首次创建时的初始化逻辑
     * @return true=初始化成功，false=初始化失败
     */
    @Override
    protected boolean _subInitNew()
    {
        return true;
    }

    /**
     * 子类初始化 - 从数据库加载
     * <p>
     * 用于活动从数据库加载时的初始化逻辑
     * @return true=初始化成功，false=初始化失败
     */
    @Override
    protected boolean _subInitStatic()
    {
        //初始化玩家管理器
        List<NumMergePlayerInfoBO> playerBoList = getUSServer().getBM().getBM(NumMergePlayerInfoBO.class).s_findAll("activity_instance_id", getInstanceId());
        if (playerBoList == null)
        {
            USLog.error(getUSServer(), "NumMergeActivity init failed, playerBoList is null, instanceId: " + getInstanceId());
            return false;
        }
        for (NumMergePlayerInfoBO playerBo : playerBoList)
        {
            _m_playerMgr.initFromDB(playerBo);
        }


        return true;
    }

    /**
     * 注册额外事件监听器
     * <p>
     * 如果活动需要监听全局事件（如玩家登录、任务完成等），
     * 可以在此方法中注册事件监听器
     */
    @Override
    protected void _regExtraEvent()
    {
        // 数字合并活动不需要监听额外的全局事件
    }

    /**
     * 活动启动时的处理
     * <p>
     * 当活动进入运行状态时调用，可以在此处理：
     * - 发送全服公告
     * - 初始化活动相关数据
     * - 开启定时任务等
     */
    @Override
    protected void _onActivityStart()
    {
        // 数字合并活动在启动时不需要特殊处理
        // 玩家数据在玩家登录时按需加载
    }

    /**
     * 活动结束时的处理
     * <p>
     * 当活动进入结束状态时调用，可以在此处理：
     * - 结算活动排行榜
     * - 发放活动奖励
     * - 发送活动结束公告等
     */
    @Override
    protected void _onActivityEnd()
    {
    }

    /**
     * 活动关闭时的处理
     * <p>
     * 当活动被关闭（删除）时调用，可以在此处理：
     * - 清理活动相关数据
     * - 释放资源等
     */
    @Override
    protected void _onActivityClosed()
    {
        //补发未领取的阶段奖励
        _m_playerMgr.dispatchUnclaimedBox();
    }

    /**
     * 活动资源清理
     * <p>
     * 释放活动占用的所有资源
     */
    @Override
    protected void _discard()
    {
        getUSServer().getBM().getBM(NumMergePlayerInfoBO.class).delAll("activity_instance_id", getInstanceId());
    }
}
