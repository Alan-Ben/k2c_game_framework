package ActivitiesV02.MsgDealers;

import Hotfix.V02.Common.NumMergeObj.NumMerge_BoardData;
import Hotfix.V02.Common.NumMergeObj.NumMerge_BoxInfo;
import Hotfix.V02.Common.NumMergeObj.NumMerge_Info;
import Hotfix.V02.GS2GC.p202_NumMergeOp.*;

/**
 * 数字合并协议Writer类 - 202协议包
 *
 * 功能：
 * 提供构造响应协议对象的便利方法
 */
public class US2GCWriter_202_NumMergeOp
{
    /**
     * 构造初始化响应协议
     * @param _info 完整的游戏状态信息
     * @return 初始化响应协议对象
     */
    public static GS2GC_202_001_RetNumMergeInit make_001_RetNumMergeInit(NumMerge_Info _info)
    {
        GS2GC_202_001_RetNumMergeInit proto = new GS2GC_202_001_RetNumMergeInit();
        proto.setInfo(_info);
        return proto;
    }

    /**
     * 构造移动响应协议
     * @return 移动响应协议对象
     */
    public static GS2GC_202_003_RetNumMergeMove make_003_RetNumMergeMove()
    {
        return new GS2GC_202_003_RetNumMergeMove();
    }

    /**
     * 构造游戏结束响应协议
     * @return 游戏结束响应协议对象
     */
    public static GS2GC_202_004_RetNumMergeGameOver make_004_RetNumMergeGameOver()
    {
        return new GS2GC_202_004_RetNumMergeGameOver();
    }

    /**
     * 构造使用重排道具响应协议
     * @return 使用重排道具响应协议对象
     */
    public static GS2GC_202_005_RetNumMergeUseOrganizeItem make_005_RetNumMergeUseOrganizeItem()
    {
        return new GS2GC_202_005_RetNumMergeUseOrganizeItem();
    }

    /**
     * 构造使用消除道具响应协议
     * @return 使用消除道具响应协议对象
     */
    public static GS2GC_202_006_RetNumMergeUseEliminateItem make_006_RetNumMergeUseEliminateItem()
    {
        return new GS2GC_202_006_RetNumMergeUseEliminateItem();
    }

    /**
     * 构造棋盘变化通知协议
     * @param _boardData 棋盘数据
     * @param _isBuffTriggered 是否有buff格子参与了本次合并
     * @return 棋盘变化通知协议对象
     */
    public static GS2GC_202_050_OnNumMergeBoardChg make_050_OnNumMergeBoardChg(NumMerge_BoardData _boardData, boolean _isBuffTriggered)
    {
        GS2GC_202_050_OnNumMergeBoardChg proto = new GS2GC_202_050_OnNumMergeBoardChg();
        proto.setBoardData(_boardData);
        proto.setIsBuffTriggered(_isBuffTriggered);
        return proto;
    }

    /**
     * 构造领取宝箱响应协议
     * @return 领取宝箱响应协议对象
     */
    public static GS2GC_202_007_RetNumMergeDrawBox make_007_RetNumMergeDrawBox()
    {
        return new GS2GC_202_007_RetNumMergeDrawBox();
    }

    /**
     * 构造宝箱信息变更推送协议
     * @param _boxInfo 宝箱信息
     * @return 宝箱信息变更推送协议对象
     */
    public static GS2GC_202_051_OnNumMergeBoxChg make_051_OnNumMergeBoxChg(NumMerge_BoxInfo _boxInfo)
    {
        GS2GC_202_051_OnNumMergeBoxChg proto = new GS2GC_202_051_OnNumMergeBoxChg();
        proto.setBoxInfo(_boxInfo);
        return proto;
    }
}
