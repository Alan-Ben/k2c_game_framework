package MGClient.Cmd.Cmds;

import GC2GS.p002_InitOp.GC2GS_002_081_ReqPushGiftPackList;
import GC2GS.p004_PlayerOp.GC2GS_004_046_ReqTriggerPushGiftGroup;
import GC2GS.p004_PlayerOp.GC2GS_004_048_ReqMarkPushGiftAsRead;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * 推送礼包命令类
 *
 * 主要功能：
 * 1. 请求推送礼包列表
 * 2. 触发推送礼包组
 * 3. 购买推送礼包
 * 4. 标记推送礼包为已读
 */
@Commander(comment = "推送礼包相关", name = "pushGift")
public class CmdPushGift extends CmdBase
{
    /**
     * 请求推送礼包列表
     *
     * 用途：获取所有礼包组的完整状态信息
     * 调用时机：登录时或需要刷新礼包列表时
     */
    @Command(comment = "获取推送礼包列表")
    public void list()
    {
        GC2GS_002_081_ReqPushGiftPackList proto = new GC2GS_002_081_ReqPushGiftPackList();
        getOwner().sendGameMsg(proto);
    }

    /**
     * 触发推送礼包组
     *
     * 用途：客户端主动触发礼包组，激活下一个礼包
     *
     * @param _groupId 礼包组id
     */
    @Command(comment = "触发推送礼包组[礼包组id]")
    public void trigger(long _groupId)
    {
        GC2GS_004_046_ReqTriggerPushGiftGroup proto = new GC2GS_004_046_ReqTriggerPushGiftGroup();
        proto.setGroupId(_groupId);
        getOwner().sendGameMsg(proto);
    }

    /**
     * 标记推送礼包为已读
     *
     * 用途：将指定礼包标记为已读状态
     *
     * @param _groupId 礼包组id
     * @param _pushGiftId 推送礼包id
     */
    @Command(comment = "标记推送礼包为已读[礼包组id][推送礼包id]")
    public void markRead(long _groupId, long _pushGiftId)
    {
        GC2GS_004_048_ReqMarkPushGiftAsRead proto = new GC2GS_004_048_ReqMarkPushGiftAsRead();
        proto.setGroupId(_groupId);
        proto.setPushGiftId(_pushGiftId);
        getOwner().sendGameMsg(proto);
    }
}