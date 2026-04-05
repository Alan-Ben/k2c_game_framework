package ActivitiesV01.MsgDealers;

import Hotfix.V01.Common.TileMatchObj.TileMatch_BlockBaseInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_Info;
import Hotfix.V01.Common.TileMatchObj.TileMatch_LogicInfo;
import Hotfix.V01.Common.TileMatchObj.TileMatch_TaskInfo;
import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType;
import Hotfix.V01.GS2GC.p201_TileMatchOp.*;

import java.util.List;

/**
 * 201 协议writer
 */
public class US2GCWriter_201_TileMatchOp
{
    public static GS2GC_201_001_RetTileMatchBlockInit make_001_RetTileMatchBlockInit(List<TileMatch_BlockBaseInfo> _blockList, TileMatch_TaskInfo _taskInfo)
    {
        GS2GC_201_001_RetTileMatchBlockInit proto = new GS2GC_201_001_RetTileMatchBlockInit();
        proto.getBlockList().addAll(_blockList);
        proto.setTaskInfo(_taskInfo);
        return proto;
    }

    public static GS2GC_201_002_RetTileMatchSwitch make_002_RetTileMatchSwitch()
    {
        return new GS2GC_201_002_RetTileMatchSwitch();
    }

    public static GS2GC_201_003_RetTileMatchGameOver make_003_RetTileMatchGameOver(List<TileMatch_BlockBaseInfo> _blockList)
    {
        GS2GC_201_003_RetTileMatchGameOver proto = new GS2GC_201_003_RetTileMatchGameOver();
        proto.getBlockList().addAll(_blockList);
        return proto;
    }

    public static GS2GC_201_004_RetTileMatchInit make_004_RetTileMatchInit(TileMatch_Info _info)
    {
        GS2GC_201_004_RetTileMatchInit proto = new GS2GC_201_004_RetTileMatchInit();
        proto.setInfo(_info);
        return proto;
    }

    public static GS2GC_201_005_RetTileMatchDrawStepReward make_005_RetTileMatchDrawStepReward()
    {
        return new GS2GC_201_005_RetTileMatchDrawStepReward();
    }

    public static GS2GC_201_051_OnTileMatchLogicProcess make_051_OnTileMatchLogicProcess(ETileMatch_ModeType _modeType, List<TileMatch_LogicInfo> _logicList)
    {
        GS2GC_201_051_OnTileMatchLogicProcess proto = new GS2GC_201_051_OnTileMatchLogicProcess();
        proto.setModeType(_modeType);
        proto.getLogicList().addAll(_logicList);
        return proto;
    }

}
