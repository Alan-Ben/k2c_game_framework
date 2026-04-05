package ActivitiesV02.Cmds;

import ActivitiesV02.Activities.RegularActivity.NumMergeActivity;
import ActivitiesV02.Activities.RegularActivity.Player.NumMergePlayerInfo;
import ActivitiesV02.Refs.NumMerge.RefNumMergeBlock;
import CommonEnum.ECommonActivityType;
import NPCommon.ErrMain.Result.Result;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * 数字合并GM命令类
 *
 * 功能：
 * 1. 设置是否打印分步调试日志
 * 2. 打印当前棋盘状态
 * 3. 重置游戏棋盘
 * 4. 修改指定位置格子的等级
 * 5. 修改指定位置格子的等级和buff状态
 */
@ACommander(comment = "数字合并", name = "nummerge")
public class CmdNumMerge extends UsCmdBase
{
    /**
     * 设置是否打印分步日志
     *
     * 用法：nummerge setprint <true/false>
     *
     * @param _open true=开启打印，false=关闭打印
     * @return 执行结果
     */
    @ACommand(comment = "设置打印分步日志[开启]")
    public String setprint(boolean _open)
    {
        NumMergeActivity activity = getUserServer().getCommActivityMgr()
                .lookupOneActivityByType(ECommonActivityType.NUM_MERGE, NumMergeActivity.class);
        if (activity == null)
            return "activity not found!";

        if (!activity.isRunning())
            return "activity is not running!";

        NumMergePlayerInfo playerInfo = activity.getPlayerMgr().ensurePlayerInfo(getOwner());
        if (playerInfo == null)
            return "player data not found!";

        playerInfo.setNeedPrintStep(_open);
        return "ok, print step is now " + (_open ? "enabled" : "disabled");
    }

    /**
     * 打印当前棋盘状态
     *
     * 用法：nummerge print
     *
     * @return 棋盘状态
     */
    @ACommand(comment = "打印棋盘")
    public String print()
    {
        NumMergeActivity activity = getUserServer().getCommActivityMgr()
                .lookupOneActivityByType(ECommonActivityType.NUM_MERGE, NumMergeActivity.class);
        if (activity == null)
            return "activity not found!";

        if (!activity.isRunning())
            return "activity is not running!";

        NumMergePlayerInfo playerInfo = activity.getPlayerMgr().ensurePlayerInfo(getOwner());
        if (playerInfo == null)
            return "player data not found!";

        return playerInfo.printBoard();
    }

    /**
     * 重置游戏棋盘
     */
    @ACommand(comment = "重置棋盘")
    public String resetboard()
    {
        NumMergeActivity activity = getUserServer().getCommActivityMgr()
                .lookupOneActivityByType(ECommonActivityType.NUM_MERGE, NumMergeActivity.class);
        if (activity == null)
            return "activity not found!";

        if (!activity.isRunning())
            return "activity is not running!";

        NumMergePlayerInfo playerInfo = activity.getPlayerMgr().ensurePlayerInfo(getOwner());
        if (playerInfo == null)
            return "player data not found!";

        // 步骤4: 调用重置棋盘方法
        Result result = playerInfo.resetBoard(getOwner(), true, getContext());
        return result.toString();
    }

    /**
     * 修改指定位置格子的等级和buff状态（扩展版）
     *
     * 用法：nummerge chgblockex <索引> <等级> <buff步数>
     * 参数说明：
     * - 索引：0-15（4x4棋盘的一维索引，索引 = 行*4 + 列）
     * - 等级：0-10（0表示空格子，1-10表示不同等级）
     * - buff步数：>=0（0表示无buff，>0表示buff剩余步数）
     *
     * 注意：
     * - 空格子（等级0）不能有buff
     * - buff倍数通过配置表计算，通常为2x倍数
     *
     * 示例：
     * - nummerge chgblockex 0 5 3 （修改左上角为5级，带3步buff）
     * - nummerge chgblockex 5 8 0 （修改索引5为8级，无buff）
     *
     * @param _blockIndex 格子索引（0-15）
     * @param _level 格子等级（0-10）
     * @param _buffStep buff剩余步数（0表示无buff）
     * @return 执行结果和棋盘状态
     */
    @ACommand(comment = "修改格子（含BUFF）[索引][等级][BUFF步数]")
    public String chgblockex(int _blockIndex, int _level, int _buffStep)
    {
        // 步骤1：获取活动实例
        NumMergeActivity activity = getUserServer().getCommActivityMgr()
                .lookupOneActivityByType(ECommonActivityType.NUM_MERGE, NumMergeActivity.class);
        if (activity == null)
            return "activity not found!";

        if (!activity.isRunning())
            return "activity is not running!";

        // 步骤2：获取玩家游戏数据
        NumMergePlayerInfo playerInfo = activity.getPlayerMgr().ensurePlayerInfo(getOwner());
        if (playerInfo == null)
            return "player data not found!";

        // 步骤3：验证参数
        if (_blockIndex < 0 || _blockIndex >= 16)
            return "invalid block index: " + _blockIndex + ", must be 0-15";

        RefNumMergeBlock refNumMergeBlock = RefNumMergeBlock.getMgr().get(_level);
        if (refNumMergeBlock == null && _level != 0)
            return "invalid level: " + _level + ", no such level in config";

        if (_buffStep < 0)
            return "invalid buff step: " + _buffStep + ", must be >= 0";

        if (_level == 0 && _buffStep > 0)
            return "empty block (level 0) cannot have buff";

        // 步骤4：执行修改
        Result result = playerInfo.modifyBlock(getOwner(), _blockIndex, _level, _buffStep, getContext());
        if (!result.isSucc())
            return "modify failed: error code " + result.getCode();

        return "ok";
    }
}