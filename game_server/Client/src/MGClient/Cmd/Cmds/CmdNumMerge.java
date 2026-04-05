package MGClient.Cmd.Cmds;

import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_ModeType;
import Hotfix.V02.Enum.NumMergeEnum.ENumMerge_MoveDir;
import Hotfix.V02.GC2GS.p202_NumMergeOp.*;
import Hotfix.V02.GS2GC.p202_NumMergeOp.*;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;
import NPCommon.Log.CommLog;

/**
 * 数字合并活动客户端测试命令
 *
 * 功能：
 * 1. 测试游戏初始化
 * 2. 测试移动操作
 * 3. 测试游戏重置
 * 4. 测试使用重排道具
 * 5. 测试使用消除道具
 */
@Commander(comment = "数字合并", name = "nummerge")
public class CmdNumMerge extends CmdBase
{
    /**
     * 初始化游戏数据
     *
     * 用法：nummerge init
     */
    @Command(comment = "初始化")
    public void init()
    {
        GC2GS_202_001_ReqNumMergeInit proto = new GC2GS_202_001_ReqNumMergeInit();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_202_001_RetNumMergeInit>(GS2GC_202_001_RetNumMergeInit.class)
        {
            @Override
            public void handle(GS2GC_202_001_RetNumMergeInit _response)
            {
                CommLog.info("NumMerge Init Response:");
                CommLog.info(_response.toString());
            }
        });
    }

    /**
     * 执行移动操作
     *
     * 用法：nummerge move <模式类型> <移动方向>
     *
     * 模式类型枚举：
     * - NORMAL (1): 普通模式
     * - ADVANCED (2): 快速模式
     * - ULTRA (3): 极速模式
     *
     * 移动方向枚举：
     * - UP (1): 向上
     * - DOWN (2): 向下
     * - LEFT (3): 向左
     * - RIGHT (4): 向右
     *
     * @param _modeType 游戏模式类型
     * @param _moveDir 移动方向
     */
    @Command(comment = "移动[模式类型][移动方向]")
    public void move(ENumMerge_ModeType _modeType, ENumMerge_MoveDir _moveDir)
    {
        GC2GS_202_003_ReqNumMergeMove proto = new GC2GS_202_003_ReqNumMergeMove();
        proto.setModeType(_modeType);
        proto.setMoveDir(_moveDir);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_202_003_RetNumMergeMove>(GS2GC_202_003_RetNumMergeMove.class)
        {
            @Override
            public void handle(GS2GC_202_003_RetNumMergeMove _response)
            {
                CommLog.info("NumMerge Move Response:");
                CommLog.info(_response.toString());
            }
        });
    }

    /**
     * 游戏结束/重新开始
     *
     * 用法：nummerge gameover
     */
    @Command(comment = "游戏结束")
    public void gameover()
    {
        GC2GS_202_004_ReqNumMergeGameOver proto = new GC2GS_202_004_ReqNumMergeGameOver();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_202_004_RetNumMergeGameOver>(GS2GC_202_004_RetNumMergeGameOver.class)
        {
            @Override
            public void handle(GS2GC_202_004_RetNumMergeGameOver _response)
            {
                CommLog.info("NumMerge GameOver Response:");
                CommLog.info(_response.toString());
            }
        });
    }

    /**
     * 使用重排道具
     *
     * 用法：nummerge organize
     *
     * 功能说明：
     * 使用重排道具将当前棋盘上的方块重新随机排列，
     * 帮助玩家创造更好的合成机会。
     */
    @Command(comment = "使用重排道具")
    public void organize()
    {
        GC2GS_202_005_ReqNumMergeUseOrganizeItem proto = new GC2GS_202_005_ReqNumMergeUseOrganizeItem();
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_202_005_RetNumMergeUseOrganizeItem>(GS2GC_202_005_RetNumMergeUseOrganizeItem.class)
        {
            @Override
            public void handle(GS2GC_202_005_RetNumMergeUseOrganizeItem _response)
            {
                CommLog.info("NumMerge UseOrganizeItem Response:");
                CommLog.info(_response.toString());
            }
        });
    }

    /**
     * 使用消除道具
     *
     * 用法：nummerge eliminate <方块索引>
     *
     * 功能说明：
     * 使用消除道具移除指定位置的方块，为新方块腾出空间。
     * 方块索引从0开始，根据棋盘大小计算（例如4x4棋盘为0-15）。
     *
     * @param _blockIndex 要消除的方块在棋盘上的索引位置
     */
    @Command(comment = "使用消除道具[方块索引]")
    public void eliminate(int _blockIndex)
    {
        GC2GS_202_006_ReqNumMergeUseEliminateItem proto = new GC2GS_202_006_ReqNumMergeUseEliminateItem();
        proto.setBlockIndex(_blockIndex);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_202_006_RetNumMergeUseEliminateItem>(GS2GC_202_006_RetNumMergeUseEliminateItem.class)
        {
            @Override
            public void handle(GS2GC_202_006_RetNumMergeUseEliminateItem _response)
            {
                CommLog.info("NumMerge UseEliminateItem Response:");
                CommLog.info(_response.toString());
            }
        });
    }
}
