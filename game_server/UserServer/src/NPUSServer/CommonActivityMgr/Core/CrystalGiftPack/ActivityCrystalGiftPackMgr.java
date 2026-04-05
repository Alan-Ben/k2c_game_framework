package NPUSServer.CommonActivityMgr.Core.CrystalGiftPack;

import Common.CommonFuncObj.CrystalGiftPack_Info;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.CrystalGiftPack.RefCrystalGiftPackGroup;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.ActivityPlayerCrystalGiftPackBuyRecordBO;
import USDB.Bo.ActivityPlayerCrystalGiftPackInfoBO;

import java.util.ArrayList;

/**
 * 活动钻石礼包管理器
 */
public class ActivityCrystalGiftPackMgr
{
    // 所属活动
    private _AActivityBase _m_activity;
    // 礼包组列表
    private ArrayList<ActivityCrystalGiftPackGroupInfo> _m_groupList;

    /**
     * 构造函数
     * @param _activity 所属活动
     */
    public ActivityCrystalGiftPackMgr(_AActivityBase _activity)
    {
        _m_activity = _activity;
        _m_groupList = new ArrayList<>();
    }

    /**
     * 初始化礼包组列表
     */
    public void initGroups()
    {
        //如果没有配置钻石礼包组ID，则不需要初始化
        long crystalGiftPackGroupId = _m_activity.getRef().crystal_gift_pack_group_id;
        if (crystalGiftPackGroupId <= 0)
            return;

        // 获取钻石礼包组配置
        RefCrystalGiftPackGroup refGroup = RefCrystalGiftPackGroup.getMgr().get(crystalGiftPackGroupId);
        if (refGroup == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityCrystalGiftPackMgr initGroups get null refGroup:{} activity:{}",
                    crystalGiftPackGroupId, _m_activity.getActivityId());
            return;
        }

        // 创建钻石礼包组对象
        ActivityCrystalGiftPackGroupInfo groupInfo = new ActivityCrystalGiftPackGroupInfo(_m_activity, refGroup);
        _m_groupList.add(groupInfo);
    }

    /**
     * 获取礼包组信息
     * @param _groupId 礼包组ID
     */
    public ActivityCrystalGiftPackGroupInfo lookupGroupInfo(long _groupId)
    {
        for (ActivityCrystalGiftPackGroupInfo groupInfo : _m_groupList)
        {
            if (groupInfo.getGroupId() == _groupId)
            {
                return groupInfo;
            }
        }
        return null;
    }

    /**
     * 初始化礼包组信息
     * @param _bo 数据库对象
     */
    public void initInfoFromDB(ActivityPlayerCrystalGiftPackInfoBO _bo)
    {
        ActivityCrystalGiftPackGroupInfo groupInfo = lookupGroupInfo(_bo.getGroupId());
        if (groupInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityCrystalGiftPackMgr initInfoFromDB group not found, groupId:{} activityId:{}",
                    _bo.getGroupId(), _m_activity.getActivityId());
            return;
        }

        groupInfo.initInfoFromDB(_bo);
    }

    /**
     * 初始化购买记录
     * @param _bo 数据库对象
     */
    public void initBuyRecordFromDB(ActivityPlayerCrystalGiftPackBuyRecordBO _bo)
    {
        ActivityCrystalGiftPackGroupInfo groupInfo = lookupGroupInfo(_bo.getGroupId());
        if (groupInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityCrystalGiftPackMgr initBuyRecordFromDB group not found, groupId:{} activityId:{}",
                    _bo.getGroupId(), _m_activity.getActivityId());
            return;
        }

        groupInfo.initBuyRecordFromDB(_bo);
    }

    /**
     * 刷新玩家礼包组
     * @param _userData 玩家数据
     * @param _groupId 礼包组ID
     * @return 是否成功刷新
     */
    public Result refreshGroup(NPUSUserData _userData, long _groupId)
    {
        ActivityCrystalGiftPackGroupInfo groupInfo = lookupGroupInfo(_groupId);
        if (groupInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityCrystalGiftPackMgr refreshGroup group not found, groupId:{} activityId:{}",
                    _groupId, _m_activity.getActivityId());
            return ActivityErr.ACTIVITY_GIFT_PACK_GROUP_NOT_FOUND;
        }

        return groupInfo.refreshGroup(_userData);
    }

    /**
     * 购买礼包
     * @param _userData 玩家数据
     * @param _groupId 礼包组ID
     * @param _packId 礼包ID
     * @param _buyCount 购买数量
     * @return 是否成功购买
     */
    public Result buyGiftPack(NPUSUserData _userData, long _groupId, long _packId, int _buyCount, NPPlayerContext _context)
    {
        ActivityCrystalGiftPackGroupInfo groupInfo = lookupGroupInfo(_groupId);
        if (groupInfo == null)
        {
            USLog.error(_m_activity.getUSServer(), "ActivityCrystalGiftPackMgr buyGiftPack group not found, groupId:{} activityId:{}",
                    _groupId, _m_activity.getActivityId());
            return ActivityErr.ACTIVITY_GIFT_PACK_GROUP_NOT_FOUND;
        }

        return groupInfo.buyGiftPack(_userData, _packId, _buyCount, _context);
    }

    /**
     * 构建玩家礼包组数据
     * @param _cid 玩家数据
     * @return 礼包组信息
     */
    public CrystalGiftPack_Info makeProtoGiftPackInfo(long _cid)
    {
        // 只获取第一个礼包组信息
        if (_m_groupList.isEmpty())
            return null;

        return _m_groupList.get(0).makeProtoGiftPackInfo(_cid);
    }
}
