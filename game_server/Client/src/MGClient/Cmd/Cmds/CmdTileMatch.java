package MGClient.Cmd.Cmds;

import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType;
import Hotfix.V01.GC2GS.p201_TileMatchOp.GC2GS_201_001_ReqTileMatchBlockInit;
import Hotfix.V01.GC2GS.p201_TileMatchOp.GC2GS_201_002_ReqTileMatchSwitchItem;
import Hotfix.V01.GS2GC.p201_TileMatchOp.GS2GC_201_001_RetTileMatchBlockInit;
import Hotfix.V01.GS2GC.p201_TileMatchOp.GS2GC_201_002_RetTileMatchSwitch;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.Annotation.Commander;
import MGClient.Cmd.CmdBase;

/**
 * @author Scott
 * @date 2016年7月8日
 */
@Commander(comment = "三消", name = "tilematch")
public class CmdTileMatch extends CmdBase
{
    @Command(comment = "初始化")
    public void init(ETileMatch_ModeType _mode)
    {
        GC2GS_201_001_ReqTileMatchBlockInit proto = new GC2GS_201_001_ReqTileMatchBlockInit();
        proto.setModeType(_mode);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_201_001_RetTileMatchBlockInit>(GS2GC_201_001_RetTileMatchBlockInit.class)
        {
            @Override
            public void handle(GS2GC_201_001_RetTileMatchBlockInit _response)
            {
            }
        });
    }

    @Command(comment = "交换")
    public void switchBlock(ETileMatch_ModeType _mode, int _startIndex, int _endIndex)
    {
        GC2GS_201_002_ReqTileMatchSwitchItem proto = new GC2GS_201_002_ReqTileMatchSwitchItem();
        proto.setModeType(_mode);
        proto.setStartIndex(_startIndex);
        proto.setEndIndex(_endIndex);
        getOwner().request(proto, new _AClientRequestHandler<GS2GC_201_002_RetTileMatchSwitch>(GS2GC_201_002_RetTileMatchSwitch.class)
        {
            @Override
            public void handle(GS2GC_201_002_RetTileMatchSwitch _response)
            {
            }
        });
    }
}
