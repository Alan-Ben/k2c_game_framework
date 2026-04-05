package ActivitiesV02.Activities.RegularActivity.Player;

import ActivitiesV02.Activities.RegularActivity.NumMergeActivity;
import ActivitiesV02.Bo.NumMergePlayerInfoBO;
import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_GameEvent;
import NPCommon.DB.BM.BM;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.HashMap;
import java.util.Map;

/**
 * 数字合并玩家管理器
 * <p>
 * 主要功能：
 * 1. 管理玩家游戏数据的加载和创建
 * 2. 缓存玩家游戏信息对象
 * 3. 提供玩家数据的统一访问接口
 * <p>
 * 设计特点：
 * - 每个玩家只有一个NumMergePlayerInfo实例
 * - 使用HashMap缓存已加载的玩家数据
 * - 支持异步数据加载
 * <p>
 * 线程安全：依赖于玩家级别锁保护
 */
public class NumMergePlayerMgr
{
    // 活动引用
    private NumMergeActivity _m_activity;

    // 玩家数据映射表 <玩家CID, 玩家游戏信息>
    private Map<Long, NumMergePlayerInfo> _m_playerInfoMap;

    public NumMergePlayerMgr(NumMergeActivity _activity)
    {
        _m_activity = _activity;
        _m_playerInfoMap = new HashMap<>();
    }

    /**
     * 获取活动实例ID
     * @return 活动实例ID
     */
    public long getActivityInstanceId()
    {
        return _m_activity.getInstanceId();
    }

    /**
     * 获取活动实例
     * @return 活动实例
     */
    public _AActivityBase getActivity()
    {
        return _m_activity;
    }

    /**
     * 获取BM
     */
    public BM getBM()
    {
        return _m_activity.getUSServer().getBM();
    }

    /**
     * 从数据库初始化玩家数据
     * @param playerBo
     */
    public void initFromDB(NumMergePlayerInfoBO playerBo)
    {
        _m_playerInfoMap.put(playerBo.getCid(), new NumMergePlayerInfo(this, playerBo));
    }

    /**
     * 获取玩家游戏信息（带缓存）
     * <p>
     * 执行流程：
     * 1. 检查缓存中是否已存在
     * 2. 如果不存在，创建新BO并插入数据库
     * 3. 用BO创建NumMergePlayerInfo
     * 4. 加入缓存并返回
     * @param _userdata 玩家
     * @return 玩家游戏信息
     */
    public NumMergePlayerInfo ensurePlayerInfo(NPUSUserData _userdata)
    {
        NumMergePlayerInfo info = _m_playerInfoMap.get(_userdata.getCid());
        if (info == null)
        {
            // 创建新BO并立即插入数据库
            NumMergePlayerInfoBO bo = new NumMergePlayerInfoBO();
            bo.setCid(getBM(), _userdata.getCid());
            bo.setActivityInstanceId(getBM(), getActivityInstanceId());
            bo.insert(getBM());

            // 用已有ID的BO创建PlayerInfo
            info = new NumMergePlayerInfo(this, bo);

            // 初始化游戏棋盘
            NPPlayerContext context = NPPlayerContext.createNew(ENumMerge_GameEvent.NUM_MERGE_GAME_INIT.value());
            info.resetBoard(_userdata, true, context);
            _m_playerInfoMap.put(_userdata.getCid(), info);
        }
        return info;
    }

    /**
     * 补发未领取的阶梯奖励
     */
    public void dispatchUnclaimedBox()
    {
        for (NumMergePlayerInfo playerInfo : _m_playerInfoMap.values())
        {
            playerInfo.dispatchUnclaimedBox();
        }
    }
}
